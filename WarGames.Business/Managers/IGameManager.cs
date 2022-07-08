using WarGames.Business.Game;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Managers
{
	public interface IGameManager
	{
		public GamePhase CurrentPhase { get; }

		public Task AddTargetAsync(Settlement settlement, TargetPriority targetPriority);

		public Task AssignArsenalAsync(ArsenalAssignment assignmentType);

		public Task AssignCountriesAsync(CountryAssignment assignmentType);

		public Task<IEnumerable<Target>> GetCurrentTargetsAsync(IPlayer source);

		public Task<IEnumerable<Settlement>> GetPotentialTargetsAsync(IPlayer source);

		public Task InitializeDefaultsAsync();

		public Task LoadWorldAsync();

		public Task MakeAiDecisionsAsync();

		public Task RainFireAsync();

		public void ReadyForLaunch();

		public void ReadyForTargetAssignments();

		public Task SetTargetAssignmentsAsync();

		public Task<IEnumerable<IPlayer>> WhoIsPlayingAsync();
	}
}