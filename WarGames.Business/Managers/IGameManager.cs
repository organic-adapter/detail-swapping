using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Managers
{
	public interface IGameManager
	{
		public Task AssignCountriesAsync(CountryAssignment assignmentType);

		public Task LoadWorldAsync();

		public Task LoadPlayerAsync(IPlayer player, ICompetitor competitor);

		public Task<ICompetitor> WhatIsPlayerAsync(IPlayer player);

		public Task<IEnumerable<IPlayer>> WhoIsPlayingAsync();
	}
}