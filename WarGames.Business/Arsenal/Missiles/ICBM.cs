using WarGames.Contracts.Arsenal;

namespace WarGames.Business.Arsenal.Missiles
{
	public class ICBM : IMissile
	{
		public float DamageRadiusKm => 10.6f;

		public IMissile? MIRV => null;

		public byte MIRVCount => 0;

		public float RangeKm => 10000f;

		public float SpeedKps => 6.705f;
	}
}