namespace WarGames.Contracts.Game
{
	[Serializable]
	public class Settlement : IUnique<string>
	{
		public Settlement()
		{
			Id = Guid.NewGuid().ToString();
			Name = string.Empty;
			Location = Game.Location.Empty;
			TargetValues = new List<TargetValue>();
		}

		public string Id { get; set; }
		public ILocation Location { get; set; }
		public string Name { get; set; }
		public List<TargetValue> TargetValues { get; set; }
	}
}