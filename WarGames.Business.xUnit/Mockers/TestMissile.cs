using Map.Engine;
using WarGames.Contracts.Arsenal;

namespace WarGames.Business.xUnit.Mockers
{
	internal class TestMissile : IMissile
	{
		internal TestMissile(float damageRadiusKm, byte mirvCount, float rangeKm, float speedKps)
		{
			DamageRadiusKm = damageRadiusKm;
			MIRVCount = mirvCount;
			RangeKm = rangeKm;
			SpeedKps = speedKps;
		}

		public float DamageRadiusKm { get; }

		public Coord? LaunchSource { get; set; }
		public float? LaunchTimeIndex { get; set; }

		public IMissile? MIRV { get; }

		public byte MIRVCount { get; }

		public float RangeKm { get; }

		public float SpeedKps { get; }

		public Coord? TargetDestination { get; set; }
		public float? TimeIndexToImpact { get; set; }
	}
}