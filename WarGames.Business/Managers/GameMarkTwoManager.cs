using WarGames.Business.Arsenal;
using WarGames.Business.Competitors;
using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Business.Planet;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Game;
using WarGames.Resources.Planet;
using static WarGames.Business.Game.CurrentGame;

namespace WarGames.Business.Managers
{
	public class GameMarkTwoManager : IGameManager
	{
		private const byte MAX_PLAYERS = 2;
		private readonly IArsenalAssignmentEngine arsenalAssignmentEngine;
		private readonly ICountryAssignmentEngine countryAssignmentEngine;
		private readonly ICountryResource countryResource;
		private readonly CurrentGame currentGame;
		private readonly IDamageCalculator damageCalculator;
		private readonly IEnumerable<IGameDefaults> gameDefaults;
		private readonly Dictionary<ICompetitor, ICompetitor> opposingSides;
		private readonly ISettlementResource settlementResource;
		private readonly IPlayerResource playerResource;
		private readonly ITargetingCalculator targetingCalculator;
		private readonly ITargetResource targetResource;
		private readonly IWorldBuildingEngine worldBuildingEngine;
		private Contracts.V2.Games.IGameDefaults? activeGameDefaults;

		public GameMarkTwoManager
		(
			CurrentGame currentGame,
			IArsenalAssignmentEngine arsenalAssignmentEngine,
			ICountryAssignmentEngine countryAssignmentEngine,
			ICountryResource countryResource,
			IDamageCalculator damageCalculator,
			IEnumerable<IGameDefaults> gameDefaults,
			IPlayerResource playerResource,
			ISettlementResource settlementResource,
			ITargetResource targetResource,
			ITargetingCalculator targetingCalculator,
			IWorldBuildingEngine worldBuildingEngine
		)
		{
			this.currentGame = currentGame;
			this.arsenalAssignmentEngine = arsenalAssignmentEngine;
			this.countryAssignmentEngine = countryAssignmentEngine;
			this.countryResource = countryResource;
			this.damageCalculator = damageCalculator;
			this.gameDefaults = gameDefaults;
			this.playerResource = playerResource;
			this.settlementResource = settlementResource;
			this.targetingCalculator = targetingCalculator;
			this.targetResource = targetResource;
			this.worldBuildingEngine = worldBuildingEngine;

			opposingSides = new Dictionary<ICompetitor, ICompetitor>();
		}

		public GamePhase CurrentPhase { get; set; }
		public IDictionary<IPlayer, ICompetitor> LoadedPlayers => throw new NotImplementedException();

		public async Task AddTargetAsync(Settlement settlement, TargetPriority targetPriority)
		{
			await targetResource.AddTargetAsync(settlement, targetPriority);
		}

		public async Task AddTargetAsync(Contracts.V2.World.Settlement settlement, TargetPriority targetPriority)
		{
			await targetResource.AddTargetAsync(settlement, targetPriority);
		}

		public async Task AssignArsenalAsync(ArsenalAssignment assignmentType)
		{
			await arsenalAssignmentEngine.AssignArsenalAsync(World.Deprecating, LoadedPlayers.Values, assignmentType);
		}

		public async Task AssignCountriesAsync(CountryAssignment assignmentType)
		{
			if (LoadedPlayers.Count < MAX_PLAYERS)
				throw new PlayersNotReady();

			CurrentPhase = GamePhase.PickTargets;
			await countryAssignmentEngine.AssignCountriesAsync(currentGame, assignmentType);
		}

		public async Task<IEnumerable<ICompetitor>> AvailableSidesAsync()
		{
			return await Task.Run(() => new List<ICompetitor> { new Capitalism(), new Communism() });
		}

		public async Task<IEnumerable<Target>> GetCurrentTargetsAsync(IPlayer source)
		{
			var side = LoadedPlayers[source];
			var opponent = opposingSides[side];
			return await targetResource.GetAsync(opponent);
		}

		public Task<IEnumerable<Target>> GetCurrentTargetsAsync(Contracts.V2.Sides.Player source)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Settlement>> GetPotentialTargetsAsync(IPlayer source)
		{
			var side = LoadedPlayers[source];
			var opponent = opposingSides[side];
			return await Task.Run(() => opponent.Settlements);
		}

		public Task<IEnumerable<Contracts.V2.World.Settlement>> GetPotentialTargetsAsync(Contracts.V2.Sides.Player source)
		{
			throw new NotImplementedException();
		}

		public async Task InitializeDefaultsAsync()
		{
			await Task.Run(() =>
			{
				activeGameDefaults = gameDefaults.First(gd => gd.MetRequirements());
				activeGameDefaults.Trigger();
				opposingSides.Add(LoadedPlayers.Values.First(), LoadedPlayers.Values.Last());
				opposingSides.Add(LoadedPlayers.Values.Last(), LoadedPlayers.Values.First());
			});
		}

		public async Task LoadPlayerAsync(IPlayer player, ICompetitor competitor)
		{
			if (LoadedPlayers.Any(lp => lp.Value == competitor && lp.Key != player))
				throw new CompetitorAlreadyTaken();

			await Task.Run(() =>
			{
				if (LoadedPlayers.ContainsKey(player))
					LoadedPlayers[player] = competitor;
				else
					LoadedPlayers.Add(player, competitor);
			});

			if (LoadedPlayers.Count == MAX_PLAYERS)
			{
				opposingSides.Add(LoadedPlayers.Values.First(), LoadedPlayers.Values.Last());
				opposingSides.Add(LoadedPlayers.Values.Last(), LoadedPlayers.Values.First());
			}
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

			var cpuPlayers = await playerResource.RetrieveManyAsync(currentGame.GameSession, Contracts.V2.Sides.PlayerType.Cpu);
			foreach (var ai in cpuPlayers)
			{
				var potentialTargets = await GetPotentialTargetsAsync(ai);
				await Task.Run(() =>
				{
					activeGameDefaults.CalculateAiTargets(() => potentialTargets, AddTarget);
				});
			}
		}

		public async Task RainFireAsync()
		{
			var targets = await targetResource.GetAllAsync();
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

		public async Task SetTargetAssignmentsAsync()
		{
			await targetingCalculator.SetTargetAssignmentsByPriorityAsync(LoadedPlayers.Select(kvp => kvp.Value));
		}

		public async Task<ICompetitor> WhatIsPlayerAsync(IPlayer player)
		{
			return await Task.Run(() => LoadedPlayers[player]);
		}

		public async Task<IEnumerable<IPlayer>> WhoIsPlayingAsync()
		{
			return await Task.Run(() => LoadedPlayers.Select(lp => lp.Key));
		}

		private void AddTarget(Contracts.V2.World.Settlement settlement, TargetPriority targetPriority)
		{
			AddTargetAsync(settlement, targetPriority).Wait();
		}
	}
}