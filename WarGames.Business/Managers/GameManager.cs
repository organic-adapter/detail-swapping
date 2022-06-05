using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources;

namespace WarGames.Business.Managers
{
	public class GameManager : IGameManager
	{
		private const byte MAX_PLAYERS = 2;
		private readonly ICountryAssignmentEngine countryAssignmentEngine;
		private readonly Dictionary<IPlayer, ICompetitor> loadedPlayers;
		private readonly IRepository<World, Guid> worldRepository;
		private World world;
		//I am a fan at defining delegate maps versus switches

		public GameManager
				(
					IRepository<World, Guid> worldRepository,
					ICountryAssignmentEngine countryAssignmentEngine
				)
		{
			this.countryAssignmentEngine = countryAssignmentEngine;
			this.worldRepository = worldRepository;

			loadedPlayers = new Dictionary<IPlayer, ICompetitor>();
		}

		public async Task AssignCountriesAsync(CountryAssignment assignmentType)
		{
			if (loadedPlayers.Count < 2)
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