using Map.Engine;
using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal.Missiles
{
	public class ICBM : IMissile
	{
		public float DamageRadiusKm => 10.6f;

		public Coord? LaunchSource { get; set; }
		public float? LaunchTimeIndex { get; set; }
		public IMissile? MIRV => null;

		public byte MIRVCount => 0;

		public float RangeKm => 10000f;

		public float SpeedKps => 6.705f;
		public Coord? TargetDestination { get; set; }
		public float? TimeIndexToImpact { get; set; }
	}
}