using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public abstract class BaseMissileDeliverySystem : IMissileDeliverySystem
	{
		protected ILocation? moveToLocation;
		public Target Assignment { get; set; }
		public IGeographicalArea CurrentArea => Location.Area;
		public bool HasTarget => Assignment != Target.Empty;
		public ILocation Location { get; set; }

		public abstract TerrainType MovementConstraint { get; }

		public float MovementSpeedKps { get; set; }

		public short PayloadCount { get; set; }

		public IMissile PayloadType { get; set; }

		public bool InAttackRange(ILocation target)
		{
			var distanceKm = Location.Coord.DistanceKm(target.Coord);
			return PayloadType.RangeKm >= distanceKm;
		}

		public void MoveTo(ILocation location)
		{
			moveToLocation = location;
		}
	}
}