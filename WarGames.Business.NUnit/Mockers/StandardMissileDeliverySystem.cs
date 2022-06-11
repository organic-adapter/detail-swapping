using WarGames.Contracts.Game;

namespace WarGames.Business.NUnit.Mockers
{
	internal class StandardMissileDeliverySystem : TestMissileDeliverySystem
	{
		public StandardMissileDeliverySystem(ILocation location)
			: base(
					  location
					  , Defaults.MissileDeliverySystem.MOVEMENT_CONSTRAINT
					  , Defaults.MissileDeliverySystem.MOVEMENT_KPS
					  , Defaults.MissileDeliverySystem.PAYLOAD_COUNT
					  , new StandardMissile()
				  )
		{
		}
	}
}