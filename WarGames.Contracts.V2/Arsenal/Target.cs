using WarGames.Contracts.V2.World;

namespace WarGames.Contracts.V2.Arsenal
{
	public class Target
	{
		public static Target Empty = new Target(Settlement.Empty, TargetPriority.Probe);

		public Target(Settlement key, TargetPriority priority)
		{
			Key = key;
			Assignments = new List<IMissileDeliverySystem>();
			Priority = priority;
		}

		public List<IMissileDeliverySystem> Assignments { get; }
		public Settlement Key { get; }
		public TargetPriority Priority { get; }

		public void Assign(IMissileDeliverySystem missileDeliverySystem)
		{
			Assignments.Add(missileDeliverySystem);
		}

		public void ResolveHits()
		{
			Key.Hits += Assignments.Count;
		}
	}
}