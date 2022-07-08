using Map.Engine;
using System.Collections.Concurrent;
using WarGames.Contracts.Game;

namespace WarGames.Contracts.V2.World
{
	[Serializable]
	public class Settlement : Game.Settlement, IUnique<string>
	{
		public static new readonly Settlement Empty = new Settlement();

		public Settlement()
		{
			AftermathValues = new ConcurrentBag<TargetValue>();
			Id = string.Empty;
			Name = string.Empty;
			Coord = Coord.NoAssignment;
			TargetValues = new List<TargetValue>();
		}

		public override ConcurrentBag<TargetValue> AftermathValues { get; set; }
		public override int Hits { get; set; }
		public override string Id { get; set; }
		public Coord Coord { get; set; }
		public override string Name { get; set; }
		public override List<TargetValue> TargetValues { get; set; }

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