using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class Silo : IMissileDeliverySystem
	{
		public Silo(float movementSpeed, short payloadCount, IMissile payloadType)
		{
			MovementSpeed = movementSpeed;
			PayloadCount = payloadCount;
			PayloadType = payloadType;
		}

		public TerrainType MovementConstraint => TerrainType.None;

		public float MovementSpeed { get; set; }

		public short PayloadCount { get; set; }

		public IMissile PayloadType { get; set; }
	}
}