using Map.Engine;
using WarGames.Contracts.V2.Arsenal;

namespace WarGames.Business.Arsenal.Missiles
{
	public class MIRVICBM : IMissile
	{
		public float DamageRadiusKm => 0f;

		public Coord? LaunchSource { get; set; }
		public float? LaunchTimeIndex { get; set; }
		public IMissile? MIRV => new MIRVCluster();

		public byte MIRVCount => 16;

		public float RangeKm => 10000f;

		public float SpeedKps => 6.705f;
		public Coord? TargetDestination { get; set; }
		public float? TimeIndexToImpact { get; set; }
	}
}