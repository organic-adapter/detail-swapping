using WarGames.Contracts.Game;

namespace WarGames.Business.NUnit.Mockers
{
	internal class StandardMissileDeliverySystem : TestMissileDeliverySystem
	{
		public StandardMissileDeliverySystem(IGeographicalArea currentArea, ILocation location)
			: base(
					  currentArea
					  , location
					  , Defaults.MissileDeliverySystem.MOVEMENT_CONSTRAINT
					  , Defaults.MissileDeliverySystem.MOVEMENT_KPS
					  , Defaults.MissileDeliverySystem.PAYLOAD_COUNT
					  , new StandardMissile()
				  )
		{
		}
	}
}