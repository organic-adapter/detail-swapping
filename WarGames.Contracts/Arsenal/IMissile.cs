using Map.Engine;

namespace WarGames.Contracts.Arsenal
{
	public interface IMissile
	{
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