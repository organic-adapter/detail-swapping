using WarGames.Business.Game;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Business.Managers
{
	public interface IGameManager
	{
		public GamePhase CurrentPhase { get; }

		public Task AddTargetAsync(Contracts.V2.Sides.Side side, Settlement settlement, TargetPriority targetPriority);

		public Task AssignArsenalAsync(ArsenalAssignment assignmentType);

		public Task AssignCountriesAsync(CountryAssignment assignmentType);

		public Task<IEnumerable<Target>> GetCurrentTargetsAsync(Player source);

		public Task<IEnumerable<Settlement>> GetPotentialTargetsAsync(Player source);

		public Task InitializeDefaultsAsync();

		public Task LoadWorldAsync();

		public Task MakeAiDecisionsAsync();

		public Task RainFireAsync();

		public void ReadyForLaunch();

		public void ReadyForTargetAssignments();

		public Task SetTargetAssignmentsAsync();

		public Task<IEnumerable<Player>> WhoIsPlayingAsync();
	}
}