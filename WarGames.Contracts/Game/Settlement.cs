namespace WarGames.Contracts.Game
{
	[Serializable]
	public class Settlement : IUnique<Guid>
	{
		public Settlement()
		{
			Id = Guid.NewGuid();
			Name = string.Empty;
			Location = Game.Location.Empty;
			TargetValues = new List<TargetValue>();
		}

		public Guid Id { get; set; }
		public ILocation Location { get; set; }
		public string Name { get; set; }
		public IEnumerable<TargetValue> TargetValues { get; set; }
	}
}