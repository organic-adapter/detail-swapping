using WarGames.Business.Exceptions;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Managers
{
	public class WarManager : IWarManager
	{
		private readonly Dictionary<IPlayer, ICompetitor> loadedPlayers;

		public WarManager()
		{
			loadedPlayers = new Dictionary<IPlayer, ICompetitor>();
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