using Microsoft.Extensions.Options;
using System.Text.Json;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Resources.Sides
{
	public class FilePlayerResource : IPlayerResource
	{
		private const string folderName = "players";
		private readonly IOptionsMonitor<JsonFileConfiguration<GameSession, string>> options;

		public FilePlayerResource
				(
					IOptionsMonitor<JsonFileConfiguration<GameSession, string>> options
				)
		{
			this.options = options;
		}

		public async Task<Player> RetrieveAsync(GameSession game, string playerId)
		{
			EnforcePathExistence(game);
			var gameSessionId = game.Id;
			var path = Path.Combine
				(
					options.CurrentValue.RootPath
					, $"{gameSessionId}/{folderName}/{playerId}.json"
				);
			var json = await File.ReadAllTextAsync(path);
			return JsonSerializer.Deserialize<Player>(json)
				?? Player.NotFound;
		}

		public async Task<IEnumerable<Player>> RetrieveManyAsync(GameSession game)
		{
			var players = new List<Player>();

			var gameSessionId = game.Id;
			var directory = Path.Combine
				(
					options.CurrentValue.RootPath
					, $"{gameSessionId}/{folderName}"
				);
			var playerFiles = Directory.GetFiles(directory);

			foreach (var playerFile in playerFiles)
				players.Add(await RetrieveAsync(game, Path.GetFileNameWithoutExtension(playerFile)));

			return players;
		}

		public async Task<IEnumerable<Player>> RetrieveManyAsync(GameSession game, PlayerType playerType)
		{
			return (await RetrieveManyAsync(game)).Where(player => player.PlayerType == playerType);
		}

		public async Task SaveAsync(GameSession game, Player player)
		{
			EnforcePathExistence(game);

			var gameSessionId = game.Id;
			var playerId = player.Id;
			var path = Path.Combine
				(
					options.CurrentValue.RootPath
					, $"{gameSessionId}/{folderName}/{playerId}.json"
				);
			var json = JsonSerializer.Serialize(player);
			await File.WriteAllTextAsync(path, json);
		}

		public async Task SaveManyAsync(GameSession game, IEnumerable<Player> players)
		{
			foreach (var player in players)
				await SaveAsync(game, player);
		}

		private void EnforcePathExistence(GameSession gameSession)
		{
			var directory = Path.Combine
				(
					options.CurrentValue.RootPath
					, $"{gameSession.Id}/{folderName}"
				);
			Directory.CreateDirectory(directory);
		}
	}
}