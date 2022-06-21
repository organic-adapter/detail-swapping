using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Managers
{
	public interface IGameManager
	{
		public GamePhase CurrentPhase { get; }

		public Task AddTargetAsync(Settlement settlement, TargetPriority targetPriority);

		public Task AssignArsenalAsync(ArsenalAssignment assignmentType);

		public Task AssignCountriesAsync(CountryAssignment assignmentType);

		public Task<IEnumerable<ICompetitor>> AvailableSidesAsync();

		public Task<IEnumerable<Target>> GetCurrentTargetsAsync(IPlayer source);

		public Task<IEnumerable<Settlement>> GetPotentialTargets(IPlayer source);
		public Task LoadPlayerAsync(IPlayer player, ICompetitor competitor);

		public Task LoadWorldAsync();

		public Task RainFireAsync();

		public void ReadyForLaunch();

		public void ReadyForTargetAssignments();

		public Task SetTargetAssignmentsAsync();

		public Task<ICompetitor> WhatIsPlayerAsync(IPlayer player);

		public Task<IEnumerable<IPlayer>> WhoIsPlayingAsync();
	}
}