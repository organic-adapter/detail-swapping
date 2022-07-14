namespace WarGames.WebAPI.Models
{
	public class GameSessionBase
	{
		public GameSessionBase(string gameSessionId)
		{
			GameSessionId = gameSessionId;
		}

		public GameSessionBase()
		{
			GameSessionId = string.Empty;
		}

		public string GameSessionId { get; set; }
	}
}