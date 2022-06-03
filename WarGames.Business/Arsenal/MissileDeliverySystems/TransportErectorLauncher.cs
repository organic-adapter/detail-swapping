using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class TransportErectorLauncher : BaseMissileDeliverySystem
	{
		public TransportErectorLauncher(float movementSpeed, short payloadCount, IMissile payloadType)
		{
			MovementSpeed = movementSpeed;
			PayloadCount = payloadCount;
			PayloadType = payloadType;
		}

		public override TerrainType MovementConstraint => TerrainType.Land;
	}
}