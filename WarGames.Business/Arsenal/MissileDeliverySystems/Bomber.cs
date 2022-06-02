using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class Bomber : IMissileDeliverySystem
	{
		public Bomber(float movementSpeed, short payloadCount, IMissile payloadType)
		{
			MovementSpeed = movementSpeed;
			PayloadCount = payloadCount;
			PayloadType = payloadType;
		}

		public TerrainType MovementConstraint => TerrainType.Land | TerrainType.Ocean;

		public float MovementSpeed { get; set; }

		public short PayloadCount { get; set; }

		public IMissile PayloadType { get; set; }
	}
}