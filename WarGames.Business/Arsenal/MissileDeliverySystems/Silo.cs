using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class Silo : BaseMissileDeliverySystem
	{
		public Silo(float movementSpeed, short payloadCount, IMissile payloadType)
		{
			MovementSpeedKps = movementSpeed;
			PayloadCount = payloadCount;
			PayloadType = payloadType;
		}

		public override TerrainType MovementConstraint => TerrainType.None;
	}
}