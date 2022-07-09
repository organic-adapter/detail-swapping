using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Business.Arsenal
{
	public interface ITargetingCalculator
	{
		public Task<IDictionary<Target, IEnumerable<IMissileDeliverySystem>>> CalculateTargetsInRangeAsync(Side currentSide, Side opposingSide);

		Task SetTargetAssignmentsByPriorityAsync(IEnumerable<Side> sides);
	}
}