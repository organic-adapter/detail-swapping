using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Managers
{
	[Obsolete("The concept of competitors is being removed in V2")]
	public interface ICompetitorBasedGame
	{
		public IDictionary<IPlayer, ICompetitor> LoadedPlayers { get; }

		public Task<IEnumerable<ICompetitor>> AvailableSidesAsync();

		public Task LoadPlayerAsync(IPlayer player, ICompetitor competitor);

		public Task<ICompetitor> WhatIsPlayerAsync(IPlayer player);
	}
}