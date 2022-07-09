using WarGames.Contracts.Game;

namespace WarGames.Business.NUnit.Mockers
{
	internal static class Defaults
	{
		public static MissileDefaults Missile = new();
		public static MissileDeliverySystemDefaults MissileDeliverySystem = new();
	}

	internal class MissileDefaults
	{
		public readonly float DAMAGE_RANGE = 10f;
		public readonly byte MIRV_COUNT = 0;
		public readonly float RANGE = 1000.0f;
		public readonly float SPEED = 1800.0f;
	}
	internal class MissileDeliverySystemDefaults
	{
		public readonly float MOVEMENT_KPS = 1f;
		public readonly short PAYLOAD_COUNT = 1;
	}
}