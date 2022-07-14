using Microsoft.Extensions.Options;
using System.Text.Json;
using WarGames.Contracts.V2;

namespace WarGames.Resources.Games
{
	public class FileGameSessionResource : IGameSessionResource
	{
		private readonly IOptionsMonitor<JsonFileConfiguration<GameSession, string>> options;

		public FileGameSessionResource
				(
					IOptionsMonitor<JsonFileConfiguration<GameSession, string>> options
				)
		{
			this.options = options;
		}

		public async Task<GameSession> RetrieveAsync(string gameSessionId)
		{
			var path = Path.Combine(
							options.CurrentValue.RootPath
							, $"{gameSessionId}/gameSession.json"
						);
			if (!File.Exists(path))
				return GameSession.NotFound;

			var json = await File.ReadAllTextAsync(path);

			return JsonSerializer.Deserialize<GameSession>(json)
					?? GameSession.NotFound;
		}

		public async Task SaveAsync(GameSession gameSession)
		{
			EnforcePathExistence(gameSession);
			var path = Path.Combine
				(
					options.CurrentValue.RootPath
					, $"{gameSession.Id}/gameSession.json"
				);
			var json = JsonSerializer.Serialize(gameSession);
			await File.WriteAllTextAsync(path, json);
		}

		private void EnforcePathExistence(GameSession gameSession)
		{
			var directory = Path.Combine
				(
					options.CurrentValue.RootPath
					, $"{gameSession.Id}"
				);
			Directory.CreateDirectory(directory);
		}
	}
}