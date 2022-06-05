using System;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.NUnit.Mockers
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
		}

		public IGeographicalArea CurrentArea { get; }

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