using WarGames.Contracts.Arsenal;

namespace WarGames.Business.Arsenal.Missiles
{
	public class Bomb : IMissile
	{
		public float DamageRadiusKm => 100.6f;

		public IMissile? MIRV => null;

		public byte MIRVCount => 0;

		public float RangeKm => 0;

		public float SpeedKps => 0;
	}
}