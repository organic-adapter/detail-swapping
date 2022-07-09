using AutoMapper;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Resources.Sides
{
	public class QADSideResource : ISideResource
	{
		private readonly IMapper mapper;
		private readonly Dictionary<GameSession, Dictionary<Player, Side>> playerMap;

		private readonly Dictionary<GameSession, Dictionary<Side, Player>> playerReverseMap;

		private readonly Dictionary<GameSession, Dictionary<string, Side>> sides;

		public QADSideResource(IMapper mapper)
		{
			this.mapper = mapper;

			sides = new();
			playerMap = new();
			playerReverseMap = new();
		}

		public async Task AssignAsync(GameSession game, Player player, Side side)
		{
			await Task.Run(() =>
			{
				Unassign(game, player);
				playerMap[game].Add(player, side);
				playerReverseMap[game].Add(side, player);
			});
		}

		public async Task<bool> IsAvailableAsync(GameSession game, Side side)
		{
			return await Task.Run
							(() =>
								playerReverseMap.ContainsKey(game)
								&& !playerReverseMap[game].ContainsKey(side)
							);
		}

		public async Task<Side> RetrieveAsync(GameSession game, string sideId)
		{
			return await Task.Run(() => sides[game][sideId]);
		}

		public async Task<Side> RetrieveAsync(GameSession game, Player player)
		{
			return await Task.Run(() => playerMap[game][player]);
		}

		public async Task<IEnumerable<Side>> RetrieveManyAsync(GameSession game)
		{
			return await Task.Run(() => playerMap[game].Select(kvp => kvp.Value));
		}

		public async Task<IEnumerable<T>> RetrieveManyAsync<T>(GameSession game)
			where T : ISideUnique
		{
			return await Task.Run(() => playerMap[game].Select(kvp => mapper.Map<T>(kvp)));
		}

		public async Task<Side> RetrieveOpposingSideAsync(GameSession game, Player player)
		{
			return await Task.Run(() =>
				{
					if (playerMap[game].Any(kvp => !player.Equals(kvp.Key)))
						return playerMap[game].First(kvp => !player.Equals(kvp.Key)).Value;

					return Side.Empty;
				});
		}

		public async Task SaveAsync(GameSession game, Side side)
		{
			await Task.Run(() =>
			{
				EnforceExistence(game);

				if (!sides[game].ContainsKey(side.Id))
					sides[game].Add(side.Id, Side.Empty);

				sides[game][side.Id] = side;
			});
		}

		public async Task SaveManyAsync(GameSession game, IEnumerable<Side> sides)
		{
			foreach (var side in sides)
				await SaveAsync(game, side);
		}

		private void EnforceExistence(GameSession gameSession)
		{
			if (!playerMap.ContainsKey(gameSession))
				playerMap.Add(gameSession, new());

			if (!playerReverseMap.ContainsKey(gameSession))
				playerReverseMap.Add(gameSession, new());

			if (!sides.ContainsKey(gameSession))
				sides.Add(gameSession, new());
		}

		private void Unassign(GameSession game, Player player)
		{
			playerMap[game].Remove(player);

			var removeMe = playerReverseMap[game].FirstOrDefault(kvp => kvp.Value.Equals(player)).Key ?? Side.Empty;
			playerReverseMap[game].Remove(removeMe);
		}
	}
}