using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Resources.Sides
{
	public class QuickAndDirtyPlayerResource : IPlayerResource
	{
		private readonly Dictionary<GameSession, Dictionary<string, Player>> players;

		public QuickAndDirtyPlayerResource()
		{
			players = new();
		}

		public async Task<Player> RetrieveAsync(GameSession game, string playerId)
		{
			return await Task.Run(() => players[game][playerId]);
		}

		public async Task<IEnumerable<Player>> RetrieveManyAsync(GameSession game)
		{
			return await Task.Run(() => players[game].Select(kvp => kvp.Value));
		}

		public async Task<IEnumerable<Player>> RetrieveManyAsync(GameSession game, Contracts.Game.PlayerType playerType)
		{
			return await Task.Run
				(
					() =>
						players[game]
							.Where(kvp => kvp.Value.PlayerType.Equals(playerType))
							.Select(kvp => kvp.Value)
				);
		}

		public async Task SaveAsync(GameSession game, Player player)
		{
			await Task.Run(() =>
			{
				if (!players.ContainsKey(game))
					players.Add(game, new());

				if (!players[game].ContainsKey(player.Id))
					players[game].Add(player.Id, Player.Empty);

				players[game][player.Id] = player;
			});
		}

		public async Task SaveManyAsync(GameSession game, IEnumerable<Player> players)
		{
			foreach (var player in players)
				await SaveAsync(game, player);
		}
	}
}