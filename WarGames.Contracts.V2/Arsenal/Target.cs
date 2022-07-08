namespace WarGames.Contracts.V2.Arsenal
{
	public class Target
	{
		public static Target Empty = new Target(World.Settlement.Empty, TargetPriority.Probe);

		public Target(World.Settlement key, TargetPriority priority)
		{
			Key = key;
			Assignments = new List<IMissileDeliverySystem>();
			Priority = priority;
		}

		public List<IMissileDeliverySystem> Assignments { get; }
		public World.Settlement Key { get; }
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