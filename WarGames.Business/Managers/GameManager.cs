using WarGames.Business.Exceptions;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources;

namespace WarGames.Business.Managers
{
	public class GameManager : IGameManager
	{
		private const byte MAX_PLAYERS = 2;
		private readonly Dictionary<CountryAssignment, Action> assignmentDelegates;
		private readonly Dictionary<IPlayer, ICompetitor> loadedPlayers;
		private readonly IRepository<World, Guid> worldRepository;
		private World world;
		//I am a fan at defining delegate maps versus switches

		public GameManager(IRepository<World, Guid> worldRepository)
		{
			loadedPlayers = new Dictionary<IPlayer, ICompetitor>();
			this.worldRepository = worldRepository;
			assignmentDelegates = new Dictionary<CountryAssignment, Action>()
			{
				{ CountryAssignment.Random, CountryAssignmentRandom }
			};
		}

		public async Task AssignCountriesAsync(CountryAssignment assignmentType)
		{
			if (loadedPlayers.Count < 2)
				throw new PlayersNotReady();
			await Task.Run(() => assignmentDelegates[assignmentType]());
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

		private void AssignCountry(ICompetitor competitor, Country country)
		{
			competitor.Countries.Add(country);
			country.Owner = competitor;
		}

		private void CountryAssignmentRandom()
		{
			var assignmentQueue = new Queue<Country>();
			var competitorToggles = MakeCompetitorToggles();

			foreach (var country in world.Countries.OrderBy(w => Guid.NewGuid()))
				assignmentQueue.Enqueue(country);

			byte nextToggle = (byte)(assignmentQueue.Count % competitorToggles.Count);
			while (assignmentQueue.Any())
			{
				var nextCompetitor = competitorToggles[nextToggle];
				var nextCountry = assignmentQueue.Dequeue();

				AssignCountry(nextCompetitor, nextCountry);
				nextToggle = (byte)(assignmentQueue.Count % competitorToggles.Count);
			}
		}

		private Dictionary<byte, ICompetitor> MakeCompetitorToggles()
		{
			var returnMe = new Dictionary<byte, ICompetitor>();
			byte nextIndex = 0;
			foreach (var competitor in loadedPlayers.Values)
			{
				returnMe.Add(nextIndex, competitor);
				nextIndex++;
			}
			return returnMe;
		}
	}
}