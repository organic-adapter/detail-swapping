namespace WarGames.Contracts.Arsenal
{
	public interface IMissile
	{
		public float DamageRadiusKm { get; }
		public IMissile? MIRV { get; }
		public byte MIRVCount { get; }
		public float RangeKm { get; }
		public float SpeedKps { get; }
	}
}