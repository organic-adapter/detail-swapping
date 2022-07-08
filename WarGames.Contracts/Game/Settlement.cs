using System.Collections.Concurrent;

namespace WarGames.Contracts.Game
{
	[Obsolete(ObsoleteConstants.Version2Incoming)]
	[Serializable]
	public class Settlement : IUnique<string>
	{
		public static Settlement Empty = new Settlement();

		public Settlement()
		{
			Id = Guid.NewGuid().ToString();
			Name = string.Empty;
			Location = Game.Location.Empty;
			AftermathValues = new ConcurrentBag<TargetValue>();
			TargetValues = new List<TargetValue>();
		}

		public virtual ConcurrentBag<TargetValue> AftermathValues { get; set; }
		public virtual int Hits { get; set; }
		public virtual string Id { get; set; }
		public ILocation Location { get; set; }
		public virtual string Name { get; set; }
		public virtual List<TargetValue> TargetValues { get; set; }

		[Obsolete("For serialization I added a setter to Hits. I don't need this in the contract.")]
		public void Hit()
		{
			Hits++;
		}
	}
}