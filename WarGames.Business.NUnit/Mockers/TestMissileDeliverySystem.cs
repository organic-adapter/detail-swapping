using Map.Engine;
using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.NUnit.Mockers
{
	internal class TestMissileDeliverySystem : IMissileDeliverySystem
	{
		public TestMissileDeliverySystem(Coord coord, short payloadCount, IMissile payloadType)
		{
			Coord = coord;
			PayloadCount = payloadCount;
			PayloadType = payloadType;
			Assignment = Target.Empty;
		}

		public Target Assignment { get; set; }
		public bool HasTarget => Assignment != Target.Empty;
		public Coord Coord { get; set; }
		public short PayloadCount { get; set; }
		public IMissile PayloadType { get; set; }

		public bool InAttackRange(Coord coord)
		{
			var distanceKm = Coord.DistanceKm(coord);
			return PayloadType.RangeKm >= distanceKm;
		}
	}
}