using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class Bomber : BaseMissileDeliverySystem
	{
		public Bomber(float movementSpeed, short payloadCount, IMissile payloadType)
		{
			MovementSpeed = movementSpeed;
			PayloadCount = payloadCount;
			PayloadType = payloadType;
		}

		public override TerrainType MovementConstraint => TerrainType.Land | TerrainType.Ocean;
	}
}