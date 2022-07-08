using Map.Engine;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using WarGames.Contracts.Game;

namespace WarGames.Contracts.V2.World
{
	[Serializable]
	public class Settlement : IUnique<string>
	{
		public static readonly Settlement Empty = new Settlement();

		public Settlement()
		{
			AftermathValues = new ConcurrentBag<TargetValue>();
			Id = string.Empty;
			Name = string.Empty;
			Coord = Coord.NoAssignment;
			TargetValues = new List<TargetValue>();
		}

		public ConcurrentBag<TargetValue> AftermathValues { get; set; }
		public int Hits { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public Coord Coord { get; set; }
		public List<TargetValue> TargetValues { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj == null)
				return false;

			return base.GetHashCode().Equals(obj.GetHashCode());
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}