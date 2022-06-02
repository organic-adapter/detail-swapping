using WarGames.Contracts.Arsenal;

namespace WarGames.Business.Arsenal.Missiles
{
	public class MIRVICBM : IMissile
	{
		public float DamageRadiusKm => 0f;

		public IMissile? MIRV => new MIRVCluster();

		public byte MIRVCount => 16;

		public float RangeKm => 10000f;

		public float SpeedKps => 6.705f;
	}
}