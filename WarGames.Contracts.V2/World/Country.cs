namespace WarGames.Contracts.V2.World
{
	[Serializable]
	public class Country : IUnique<string>
	{
		public static readonly Country Empty = new();

		public Country()
		{
			Id = string.Empty;
			Name = string.Empty;
			SettlementIds = new List<string>();
		}

		public string Id { get; set; }

		public string Name { get; set; }

		public List<string> SettlementIds { get; set; }

		public override bool Equals(object? obj)
		{
			var other = obj as Country;

			if(other == null) return false;

			if (object.ReferenceEquals(obj, null)) return false;

			if (object.ReferenceEquals(this, obj)) return true;

			return Id.Equals(other.Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}