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
			Location = Location.Empty;
			TargetValues = new List<TargetValue>();
		}

		public ConcurrentBag<TargetValue> AftermathValues { get; set; }
		public int Hits { get; set; }
		public string Id { get; init; }
		public Location Location { get; init; }
		public List<TargetValue> TargetValues { get; set; }
	}
}