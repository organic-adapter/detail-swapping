using WarGames.Contracts.V2;

namespace WarGames.Business.Game
{
	public class CurrentGame
	{
		private GameSession gameSession;

		public CurrentGame()
		{
			gameSession = GameSession.NotLoaded;
		}

		public GameSession GameSession
		{
			get { return gameSession; }
			set { gameSession = value; }
		}

		public bool IsNew => gameSession.IsNew;
		public bool NotLoaded => gameSession == GameSession.NotLoaded;

		public void CreateNew()
		{
			gameSession = new GameSession() { Id = Guid.NewGuid().ToString(), Phase = GameSession.SessionPhase.New };
		}
		public void CreateNew(string gameSessionId)
		{
			gameSession = new GameSession() { Id = gameSessionId, Phase = GameSession.SessionPhase.New };
		}
		public void Start()
		{
			gameSession.Phase = GameSession.SessionPhase.Started;
		}

		public class GameSessionNotLoadedException : Exception
		{
		}
	}
}