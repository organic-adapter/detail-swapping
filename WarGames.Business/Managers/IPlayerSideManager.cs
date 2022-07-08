using WarGames.Contracts.V2.Sides;

namespace WarGames.Business.Managers
{
	public interface IPlayerSideManager
	{
		public Task AddAsync(Player player);

		public Task AddAsync(Side side);

		public Task ChooseAsync(Player player, Side side);

		public int Count(Contracts.Game.PlayerType playerType);

		public bool HasPlayerType(Contracts.Game.PlayerType playerType);

		public Task<Side> NextAvailableSideAsync();
	}
}