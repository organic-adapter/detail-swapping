using AutoMapper;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Resources.Sides
{
	public class QuickAndDirtySideResource : ISideResource
	{
		private readonly IMapper mapper;
		private readonly Dictionary<GameSession, Dictionary<Player, Side>> playerMap;

		private readonly Dictionary<GameSession, Dictionary<Side, Player>> playerReverseMap;

		private readonly Dictionary<GameSession, Dictionary<string, Side>> sides;

		public QuickAndDirtySideResource(IMapper mapper)
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
				UnassignPlayer(game, side);
				playerMap[game].Add(player, side);
				playerReverseMap[game].Add(side, player);
			});
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

		public async Task SaveAsync(GameSession game, Side side)
		{
			await Task.Run(() =>
			{
				if (!sides.ContainsKey(game))
					sides.Add(game, new());

				if (!sides[game].ContainsKey(side.Id))
					sides[game].Add(side.Id, Side.Empty);
			});
		}

		public async Task SaveManyAsync(GameSession game, IEnumerable<Side> sides)
		{
			foreach (var side in sides)
				await SaveAsync(game, side);
		}

		private void UnassignPlayer(GameSession game, Side side)
		{
			if (!playerReverseMap[game].ContainsKey(side))
				return;

			var player = playerReverseMap[game][side];
			playerReverseMap[game].Remove(side);
			playerMap[game].Remove(player);
		}
	}
}