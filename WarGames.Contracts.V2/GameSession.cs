namespace WarGames.Contracts.V2
{
	public static class GameSessionExtensions
	{
		public static bool IsNotFound(this GameSession gameSession)
		{
			return gameSession == GameSession.NotFound;
		}
	}

	[Serializable]
	public class GameSession : IUnique<string>
	{
		public static readonly GameSession NotFound = new();
		public static readonly GameSession NotLoaded = new();

		public GameSession(string id, SessionPhase phase)
		{
			Id = id;
			Phase = phase;
		}

		public GameSession()
		{
			Id = string.Empty;
			Phase = SessionPhase.New;
		}

		public enum SessionPhase
		{
			Unknown = 0,
			New = 1,
			Started = 2,
			Finished = 3,
		}

		public string Id { get; set; }
		public bool IsNew => Phase == SessionPhase.New;
		public SessionPhase Phase { get; set; }
	}
}