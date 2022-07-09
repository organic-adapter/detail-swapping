using Map.Engine;
using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal.Missiles
{
	public class SRM : IMissile
	{
		public float DamageRadiusKm => 4.6f;

		public Coord? LaunchSource { get; set; }
		public float? LaunchTimeIndex { get; set; }
		public IMissile? MIRV => null;

		public byte MIRVCount => 0;

		public float RangeKm => 1800f;

		public float SpeedKps => 3.205f;
		public Coord? TargetDestination { get; set; }
		public float? TimeIndexToImpact { get; set; }
	}
}