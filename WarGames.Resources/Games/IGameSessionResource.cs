using WarGames.Contracts.V2;

namespace WarGames.Resources.Games
{
	public interface IGameSessionResource
	{
		public Task<GameSession> RetrieveAsync(string gameSessionId);

		public Task SaveAsync(GameSession gameSession);
	}
}