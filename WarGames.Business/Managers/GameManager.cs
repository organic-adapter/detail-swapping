using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources;
using WarGames.Resources.Arsenal;

namespace WarGames.Business.Managers
{
	public class GameManager : IGameManager
	{
		private const byte MAX_PLAYERS = 2;
		private readonly ICountryAssignmentEngine countryAssignmentEngine;
		private readonly Dictionary<IPlayer, ICompetitor> loadedPlayers;
		private readonly ITargetResource targetResource;
		private readonly IRepository<World, Guid> worldRepository;
		private World world;
		//I am a fan at defining delegate maps versus switches

		public GameManager
				(
					IRepository<World, Guid> worldRepository,
					ICountryAssignmentEngine countryAssignmentEngine,
					ITargetResource targetResource
				)
		{
			this.countryAssignmentEngine = countryAssignmentEngine;
			this.targetResource = targetResource;
			this.worldRepository = worldRepository;

			loadedPlayers = new Dictionary<IPlayer, ICompetitor>();
			world = World.Empty;
		}

		public async Task AddTargetAsync(Settlement settlement, TargetPriority targetPriority)
		{
			await targetResource.AddTargetAsync(settlement, targetPriority);
		}

		public async Task AssignCountriesAsync(CountryAssignment assignmentType)
		{
			if (loadedPlayers.Count < MAX_PLAYERS)
				throw new PlayersNotReady();

			await countryAssignmentEngine.AssignCountriesAsync(world, loadedPlayers.Values, assignmentType);
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
		}

		/// <summary>
		/// TODO: Not complete. We need a way to load a specific world or trigger
		/// a randomized world.
		/// </summary>
		/// <returns></returns>
		public async Task LoadWorldAsync()
		{
			var worlds = await worldRepository.GetAllAsync();
			world = worlds.First();
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