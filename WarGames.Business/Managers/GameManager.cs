using AutoMapper;
using WarGames.Business.Arsenal;
using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Business.Planet;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Games;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Games;
using WarGames.Resources.Planet;
using WarGames.Resources.Sides;
using static WarGames.Business.Game.CurrentGame;

namespace WarGames.Business.Managers
{
	public class GameManager : IGameManager, ISideBasedGame
	{
		private const byte MAX_PLAYERS = 2;
		private readonly IEnumerable<(Contracts.V2.Sides.Player, Contracts.V2.Sides.Side)> activePlayers;
		private readonly IArsenalAssignmentEngine arsenalAssignmentEngine;
		private readonly ICountryAssignmentEngine countryAssignmentEngine;
		private readonly ICountryResource countryResource;
		private readonly CurrentGame currentGame;
		private readonly IDamageCalculator damageCalculator;
		private readonly IEnumerable<IGameDefaults> gameDefaults;
		private readonly IGameSessionResource gameSessionResource;
		private readonly IMapper mapper;
		private readonly IPlayerResource playerResource;
		private readonly ISettlementResource settlementResource;
		private readonly ISideResource sideResource;
		private readonly ITargetingCalculator targetingCalculator;
		private readonly ITargetResource targetResource;
		private readonly IWorldBuildingEngine worldBuildingEngine;
		private IGameDefaults? activeGameDefaults;

		public GameManager
		(
			IMapper mapper,

			CurrentGame currentGame,
			IArsenalAssignmentEngine arsenalAssignmentEngine,
			ICountryAssignmentEngine countryAssignmentEngine,
			ICountryResource countryResource,
			IDamageCalculator damageCalculator,
			IEnumerable<IGameDefaults> gameDefaults,
			IGameSessionResource gameSessionResource,
			IPlayerResource playerResource,
			ISettlementResource settlementResource,
			ISideResource sideResource,
			ITargetResource targetResource,
			ITargetingCalculator targetingCalculator,
			IWorldBuildingEngine worldBuildingEngine
		)
		{
			this.mapper = mapper;

			this.currentGame = currentGame;
			this.arsenalAssignmentEngine = arsenalAssignmentEngine;
			this.countryAssignmentEngine = countryAssignmentEngine;
			this.countryResource = countryResource;
			this.damageCalculator = damageCalculator;
			this.gameDefaults = gameDefaults;
			this.gameSessionResource = gameSessionResource;
			this.playerResource = playerResource;
			this.settlementResource = settlementResource;
			this.sideResource = sideResource;
			this.targetingCalculator = targetingCalculator;
			this.targetResource = targetResource;
			this.worldBuildingEngine = worldBuildingEngine;

			activePlayers = new List<(Contracts.V2.Sides.Player player, Contracts.V2.Sides.Side)>();
		}

		public GamePhase CurrentPhase { get; set; }

		public async Task AddTargetAsync(Contracts.V2.Sides.Side side, Contracts.V2.World.Settlement settlement, Contracts.V2.Arsenal.TargetPriority targetPriority)
		{
			await targetResource.AddTargetAsync(currentGame.GameSession, side, settlement, targetPriority);
		}

		public async Task AssignArsenalAsync(ArsenalAssignment assignmentType)
		{
			await arsenalAssignmentEngine.AssignArsenalAsync(currentGame.GameSession, assignmentType);
		}

		public async Task AssignCountriesAsync(CountryAssignment assignmentType)
		{
			if (!await ReachedMaxPlayers())
				throw new PlayersNotReady();

			CurrentPhase = GamePhase.PickTargets;
			await countryAssignmentEngine.AssignCountriesAsync(currentGame.GameSession, assignmentType);
		}

		public IEnumerable<(Player, Side)> GetActivePlayers()
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Target>> GetCurrentTargetsAsync(Player source)
		{
			var opponent = await sideResource.RetrieveOpposingSideAsync(currentGame.GameSession, source);
			return await targetResource.RetrieveManyAsync(currentGame.GameSession, opponent);
		}

		public async Task<IEnumerable<Settlement>> GetPotentialTargetsAsync(Player source)
		{
			var opponent = await sideResource.RetrieveOpposingSideAsync(currentGame.GameSession, source);
			return await settlementResource.RetrieveManyAsync(currentGame.GameSession, opponent);
		}

		public async Task InitializeDefaultsAsync()
		{
			await Task.Run(() =>
			{
				activeGameDefaults = gameDefaults.First(gd => gd.MetRequirements());
				activeGameDefaults.Trigger();
			});
		}

		public async Task LoadWorldAsync()
		{
			if (currentGame.NotLoaded)
				throw new GameSessionNotLoadedException();

			await worldBuildingEngine.BuildAsync(currentGame.GameSession);
		}

		public async Task MakeAiDecisionsAsync()
		{
			if (activeGameDefaults == null)
				throw new Exception("Cannot make AI decisions unless game defaults have been initialized");

			var ais = await playerResource.RetrieveManyAsync(currentGame.GameSession, PlayerType.Cpu);
			foreach (var ai in ais)
			{
				var aiSide = await sideResource.RetrieveAsync(currentGame.GameSession, ai);
				var potentialTargets = await GetPotentialTargetsAsync(ai);
				await Task.Run(() =>
				{
					Action<Settlement, TargetPriority> addTarget = (Settlement settlement, TargetPriority targetPriority) =>
					{
						AddTargetAsync(aiSide, settlement, targetPriority).Wait();
					};
					activeGameDefaults.CalculateAiTargets(() => potentialTargets, addTarget);
				});
			}
		}

		public async Task RainFireAsync()
		{
			var targets = await targetResource.RetrieveAllAsync(currentGame.GameSession);
			var options = new ParallelOptions { MaxDegreeOfParallelism = 8 };
			await Parallel.ForEachAsync
				(
					targets,
					options,
					async (target, token) =>
					{
						await Task.Run(() =>
						{
							target.ResolveHits();
						});
					}
				); ;
			var settlements = await settlementResource.RetrieveManyAsync(currentGame.GameSession);
			await damageCalculator.CalculateAfterMathAsync(settlements);
		}

		public void ReadyForLaunch()
		{
			CurrentPhase = GamePhase.EndOfWorld;
		}

		public void ReadyForTargetAssignments()
		{
			CurrentPhase = GamePhase.FinalizeAssignments;
		}

		public async Task SetGameSession(string gameSessionId)
		{
			gameSessionId
				.When(string.IsNullOrEmpty)
				.AbortWith<InvalidGameSessionIdException>();

			currentGame.GameSession = await gameSessionResource.RetrieveAsync(gameSessionId);
			if (currentGame.GameSession.IsNotFound())
				await CreateNewAsync(gameSessionId);
		}

		public async Task SetTargetAssignmentsAsync()
		{
			var sides = await sideResource.RetrieveManyAsync(currentGame.GameSession);
			await targetingCalculator.SetTargetAssignmentsByPriorityAsync(sides);
		}

		public async Task<IEnumerable<Player>> WhoIsPlayingAsync()
		{
			return await playerResource.RetrieveManyAsync(currentGame.GameSession);
		}

		private async Task CreateNewAsync(string gameSessionId)
		{
			currentGame.CreateNew(gameSessionId);
			await gameSessionResource.SaveAsync(currentGame.GameSession);
		}

		private async Task<bool> ReachedMaxPlayers()
		{
			var activePlayers = await playerResource.RetrieveManyAsync(currentGame.GameSession);
			return activePlayers.Count() == MAX_PLAYERS;
		}

		public class InvalidGameSessionIdException : Exception
		{
		}
	}
}