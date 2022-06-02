using WarGames.Contracts.Arsenal;

namespace WarGames.Business.Arsenal.Missiles
{
	public class MIRVCluster : IMissile
	{
		public float DamageRadiusKm => 1.6f;

		public IMissile? MIRV => null;

		public byte MIRVCount => 0;

		public float RangeKm => 1.5f;

		public float SpeedKps => 1000f;
	}
}