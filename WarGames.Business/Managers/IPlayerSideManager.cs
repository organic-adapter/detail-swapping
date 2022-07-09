using WarGames.Contracts.V2.Sides;

namespace WarGames.Business.Managers
{
	public interface IPlayerSideManager
	{
		public Task AddAsync(Player player);

		Task AddAsync(params Player[] players);

		public Task AddAsync(Side side);

		Task AddAsync(params Side[] side);

		public Task ChooseAsync(Player player, Side side);

		public int Count(PlayerType playerType);

		public Task<IEnumerable<Player>> GetPlayersAsync();

		public Task<Side> GetSideAsync(string sideId);

		public bool HasPlayerType(PlayerType playerType);

		public Task<Side> NextAvailableSideAsync();

		public Task<Side> WhatIsPlayerAsync(Player player);
	}
}