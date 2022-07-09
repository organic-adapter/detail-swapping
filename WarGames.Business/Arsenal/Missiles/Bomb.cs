using Map.Engine;
using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal.Missiles
{
	public class Bomb : IMissile
	{
		public float DamageRadiusKm => 100.6f;

		public Coord? LaunchSource { get; set; }
		public float? LaunchTimeIndex { get; set; }
		public IMissile? MIRV => null;

		public byte MIRVCount => 0;

		public float RangeKm => 0;

		public float SpeedKps => 0;
		public Coord? TargetDestination { get; set; }
		public float? TimeIndexToImpact { get; set; }
	}
}