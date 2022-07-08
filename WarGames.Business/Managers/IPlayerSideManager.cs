using WarGames.Contracts.V2.Sides;

namespace WarGames.Business.Managers
{
	public interface IPlayerSideManager
	{
		public Task AddAsync(Player player);

		public Task AddAsync(Side side);

		public Task ChooseAsync(Player player, Side side);
	}
}