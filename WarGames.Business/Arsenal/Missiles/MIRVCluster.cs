using Map.Engine;
using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal.Missiles
{
	public class MIRVCluster : IMissile
	{
		public float DamageRadiusKm => 1.6f;

		public Coord? LaunchSource { get; set; }
		public float? LaunchTimeIndex { get; set; }
		public IMissile? MIRV => null;

		public byte MIRVCount => 0;

		public float RangeKm => 1.5f;

		public float SpeedKps => 1000f;
		public Coord? TargetDestination { get; set; }
		public float? TimeIndexToImpact { get; set; }
	}
}