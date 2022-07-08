using System.Text.Json.Serialization;

namespace WarGames.Contracts.V2.World
{
	[Serializable]
	public class Country : Game.Country, IUnique<string>
	{
		public static readonly Country Empty = new();

		public Country()
		{
			Id = string.Empty;
			Name = string.Empty;
			SettlementIds = new List<string>();

			Settlements = new List<Game.Settlement>();
		}

		public override string Id { get; set; }

		public override string Name { get; set; }

		public List<string> SettlementIds { get; set; }

		public override bool Equals(object? obj)
		{
			if (object.ReferenceEquals(obj, null)) return false;

			if (object.ReferenceEquals(this, obj)) return true;

			return Id.Equals(Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}