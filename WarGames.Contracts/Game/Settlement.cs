namespace WarGames.Contracts.Game
{
	[Serializable]
	public class Settlement : IUnique<string>
	{
		public static Settlement Empty = new Settlement();

		public Settlement()
		{
			Id = Guid.NewGuid().ToString();
			Name = string.Empty;
			Location = Game.Location.Empty;
			TargetValues = new List<TargetValue>();
		}

		public int Hits { get; set; }
		public string Id { get; set; }
		public ILocation Location { get; set; }
		public string Name { get; set; }
		public List<TargetValue> TargetValues { get; set; }

		public void Hit()
		{
			Hits++;
		}
	}
}