using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Business.Managers
{
	public interface IWorldManager
	{
		public Task AssignAsync(Side side, Settlement settlement);
		public Task<IEnumerable<Settlement>> GetSettlementsAsync(Side side);
	}
}