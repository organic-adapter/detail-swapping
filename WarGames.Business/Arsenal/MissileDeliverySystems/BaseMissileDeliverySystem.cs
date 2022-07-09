using Map.Engine;
using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public abstract class BaseMissileDeliverySystem : IMissileDeliverySystem
	{
		public Target Assignment { get; set; }
		public Coord Coord { get; set; }
		public bool HasTarget => Assignment != Target.Empty;
		public short PayloadCount { get; set; }

		public IMissile PayloadType { get; set; }

		public bool InAttackRange(Coord coord)
		{
			var distanceKm = Coord.DistanceKm(coord);
			return PayloadType.RangeKm >= distanceKm;
		}
	}
}