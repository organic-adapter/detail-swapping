using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal
{
	public interface IArsenalAssignmentEngine
	{
		public Task AssignArsenalAsync(World world, IEnumerable<ICompetitor> competitors, ArsenalAssignment assignmentType);
	}
}