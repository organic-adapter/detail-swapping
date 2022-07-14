using WarGames.Contracts.V2;

namespace WarGames.Resources.Games
{
	public class QADGameSessionResource : IGameSessionResource
	{
		private readonly Dictionary<string, GameSession> gameSessions;

		public QADGameSessionResource()
		{
			gameSessions = new();
		}

		public async Task<GameSession> RetrieveAsync(string gameSessionId)
		{
			return await Task.Run(() =>
			{
				if (!gameSessions.ContainsKey(gameSessionId))
					return GameSession.NotFound;

				return gameSessions[gameSessionId];
			});
		}

		public async Task SaveAsync(GameSession gameSession)
		{
			await Task.Run(() => gameSessions[gameSession.Id] = gameSession);
		}
	}
}