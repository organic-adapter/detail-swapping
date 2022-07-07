namespace WarGames.Contracts.V2.World
{
	[Serializable]
	public class Country : IUnique<string>
	{
		public static readonly Country Empty = new ();

		public Country()
		{
			Id = string.Empty;
			SettlementIds = new List<string>();
		}

		public string Id { get; init; }

		public List<string> SettlementIds { get; set; }
	}
}