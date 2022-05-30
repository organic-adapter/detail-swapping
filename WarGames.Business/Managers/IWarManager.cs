using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Managers
{
	public interface IWarManager
	{
		public Task LoadPlayerAsync(IPlayer player, ICompetitor competitor);

		public Task<ICompetitor> WhatIsPlayerAsync(IPlayer player);

		public Task<IEnumerable<IPlayer>> WhoIsPlayingAsync();
	}
}