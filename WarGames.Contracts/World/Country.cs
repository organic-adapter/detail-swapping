namespace WarGames.Contracts.World
{
	[Serializable]
	public class Country : IUnique<string>
	{
		public Country()
		{
			Id = string.Empty;
			SettlementIds = new List<string>();
		}

		public string Id { get; init; }

		public List<string> SettlementIds { get; set; }
	}
}