using WarGames.Business.Arsenal;
using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources.Arsenal;

namespace WarGames.Business.Managers
{
	public class GameManager : IGameManager
	{
		private const byte MAX_PLAYERS = 2;
		private readonly IArsenalAssignmentEngine arsenalAssignmentEngine;
		private readonly ICountryAssignmentEngine countryAssignmentEngine;
		private readonly IDamageCalculator damageCalculator;
		private readonly Dictionary<IPlayer, ICompetitor> loadedPlayers;
		private readonly Dictionary<ICompetitor, ICompetitor> opposingSides;
		private readonly ITargetingEngine targetingEngine;
		private readonly ITargetResource targetResource;
		private readonly WorldFactory? worldFactory;
		private World world;

		public GameManager
				(
					WorldFactory worldFactory,
					IArsenalAssignmentEngine arsenalAssignmentEngine,
					ICountryAssignmentEngine countryAssignmentEngine,
					IDamageCalculator damageCalculator,
					ITargetResource targetResource,
					ITargetingEngine targetingEngine
				)
		{
			this.arsenalAssignmentEngine = arsenalAssignmentEngine;
			this.countryAssignmentEngine = countryAssignmentEngine;
			this.damageCalculator = damageCalculator;
			this.targetingEngine = targetingEngine;
			this.targetResource = targetResource;
			this.worldFactory = worldFactory;

			loadedPlayers = new Dictionary<IPlayer, ICompetitor>();
			opposingSides = new Dictionary<ICompetitor, ICompetitor>();
			world = World.Empty;
		}

		//I am a fan at defining delegate maps versus switches
		public GameManager
				(
					World world,
					IArsenalAssignmentEngine arsenalAssignmentEngine,
					ICountryAssignmentEngine countryAssignmentEngine,
					ITargetResource targetResource
				)
		{
			this.arsenalAssignmentEngine = arsenalAssignmentEngine;
			this.countryAssignmentEngine = countryAssignmentEngine;
			this.damageCalculator = new DamageCalculator();
			this.targetingEngine = new TargetingEngine(targetResource);
			this.targetResource = targetResource;
			this.worldFactory = null;
			loadedPlayers = new Dictionary<IPlayer, ICompetitor>();
			opposingSides = new Dictionary<ICompetitor, ICompetitor>();
			this.world = world;
		}

		public GamePhase CurrentPhase { get; set; }

		public async Task AddTargetAsync(Settlement settlement, TargetPriority targetPriority)
		{
			await targetResource.AddTargetAsync(settlement, targetPriority);
		}

		public async Task AssignArsenalAsync(ArsenalAssignment assignmentType)
		{
			await arsenalAssignmentEngine.AssignArsenalAsync(world, loadedPlayers.Values, assignmentType);
		}

		public async Task AssignCountriesAsync(CountryAssignment assignmentType)
		{
			if (loadedPlayers.Count < MAX_PLAYERS)
				throw new PlayersNotReady();

			CurrentPhase = GamePhase.PickTargets;
			await countryAssignmentEngine.AssignCountriesAsync(world, loadedPlayers.Values, assignmentType);
		}

		public async Task<IEnumerable<Target>> GetCurrentTargetsAsync(IPlayer source)
		{
			var side = loadedPlayers[source];
			var opponent = opposingSides[side];
			return await targetResource.GetAsync(opponent);
		}

		public async Task<IEnumerable<Settlement>> GetPotentialTargets(IPlayer source)
		{
			var side = loadedPlayers[source];
			var opponent = opposingSides[side];
			return await Task.Run(() => opponent.Settlements);
		}
		public async Task LoadPlayerAsync(IPlayer player, ICompetitor competitor)
		{
			if (loadedPlayers.Any(lp => lp.Value == competitor && lp.Key != player))
				throw new CompetitorAlreadyTaken();

			await Task.Run(() =>
			{
				if (loadedPlayers.ContainsKey(player))
					loadedPlayers[player] = competitor;
				else
					loadedPlayers.Add(player, competitor);
			});

			if (loadedPlayers.Count == MAX_PLAYERS)
			{
				opposingSides.Add(loadedPlayers.Values.First(), loadedPlayers.Values.Last());
				opposingSides.Add(loadedPlayers.Values.Last(), loadedPlayers.Values.First());
			}
		}

		/// <summary>
		/// TODO: Not complete. We need a way to load a specific world or trigger
		/// a randomized world.
		/// </summary>
		/// <returns></returns>
		public async Task LoadWorldAsync()
		{
			if (worldFactory == null)
				throw new Exception("Missing WorldFactory dependency");

			world = await worldFactory.BuildAsync();
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

			await damageCalculator.CalculateAfterMathAsync(world);
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
			await targetingEngine.SetTargetAssignmentsByPriorityAsync(loadedPlayers.Select(kvp => kvp.Value));
		}

		public async Task<ICompetitor> WhatIsPlayerAsync(IPlayer player)
		{
			return await Task.Run(() => loadedPlayers[player]);
		}

		public async Task<IEnumerable<IPlayer>> WhoIsPlayingAsync()
		{
			return await Task.Run(() => loadedPlayers.Select(lp => lp.Key));
		}
	}
}