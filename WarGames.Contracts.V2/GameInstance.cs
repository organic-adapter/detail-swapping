namespace WarGames.Contracts.V2
{
	[Serializable]
	public class GameInstance : IUnique<string>
	{
		public GameInstance()
		{
			Id = string.Empty;
		}
		public string Id { get; set; }
	}
}