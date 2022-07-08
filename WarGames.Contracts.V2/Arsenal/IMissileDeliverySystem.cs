using WarGames.Contracts.Game;

namespace WarGames.Contracts.V2.Arsenal
{
	public interface IMissileDeliverySystem
	{
		public Target Assignment { get; set; }

		public IGeographicalArea CurrentArea { get; }

		public bool HasTarget { get; }
		public ILocation Location { get; }

		public TerrainType MovementConstraint { get; }

		public float MovementSpeedKps { get; }

		public short PayloadCount { get; }

		public IMissile PayloadType { get; }

		public bool InAttackRange(ILocation location);

		public void MoveTo(ILocation location);
	}
}