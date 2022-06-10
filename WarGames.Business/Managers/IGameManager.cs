using WarGames.Business.Game;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Managers
{
	public interface IGameManager
	{
		public Task AddTargetAsync(Settlement settlement, TargetPriority targetPriority);

		public Task AssignCountriesAsync(CountryAssignment assignmentType);

		public Task LoadPlayerAsync(IPlayer player, ICompetitor competitor);

		public Task LoadWorldAsync();

		public Task<ICompetitor> WhatIsPlayerAsync(IPlayer player);

		public Task<IEnumerable<IPlayer>> WhoIsPlayingAsync();
	}
}