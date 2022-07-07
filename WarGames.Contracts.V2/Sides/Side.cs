namespace WarGames.Contracts.V2.Sides
{
	[Serializable]
	public class Side : IUnique<string>
	{
		public Side()
		{
			Id = Guid.NewGuid().ToString();
			DisplayName = string.Empty;
			Countries = new List<string>();
		}

		public Side(string id, string displayName)
		{
			Id = id;
			DisplayName = displayName;
			Countries = new List<string>();
		}

		public List<string> Countries { get; set; }
		public string DisplayName { get; set; }
		public string Id { get; set; }
	}
}