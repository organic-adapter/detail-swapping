using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.MSTest.Mockers
{
	internal class TestMissileDeliverySystem : IMissileDeliverySystem
	{
		public TestMissileDeliverySystem(IGeographicalArea currentArea, ILocation location, TerrainType movementConstraint, float movementSpeedKps, short payloadCount, IMissile payloadType)
		{
			CurrentArea = currentArea;
			Location = location;
			MovementConstraint = movementConstraint;
			MovementSpeedKps = movementSpeedKps;
			PayloadCount = payloadCount;
			PayloadType = payloadType;
			Assignment = Target.Empty;
		}

		public Target Assignment { get; set; }
		public IGeographicalArea CurrentArea { get; }

		public bool HasTarget => Assignment != Target.Empty;
		public ILocation Location { get; }

		public TerrainType MovementConstraint { get; }

		public float MovementSpeedKps { get; }

		public short PayloadCount { get; }

		public IMissile PayloadType { get; }

		public bool InAttackRange(ILocation target)
		{
			var distanceKm = Location.Coord.DistanceKm(target.Coord);
			return PayloadType.RangeKm >= distanceKm;
		}

		public void MoveTo(ILocation location)
		{
			throw new NotImplementedException();
		}
	}
}