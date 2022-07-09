using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal
{
	public interface IArsenalAssignmentEngine
	{
		public Task AssignArsenalAsync(GameSession gameSession, ArsenalAssignment assignmentType);
	}
}