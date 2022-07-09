using Map.Engine;

namespace WarGames.Business.NUnit.Mockers
{
	internal class StandardMissileDeliverySystem : TestMissileDeliverySystem
	{
		public StandardMissileDeliverySystem(Coord coord)
			: base(
					  coord
					  , Defaults.MissileDeliverySystem.PAYLOAD_COUNT
					  , new StandardMissile()
				  )
		{
		}
	}
}