using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class TransportErectorLauncher : IMissileDeliverySystem
	{
		public TransportErectorLauncher(float movementSpeed, short payloadCount, IMissile payloadType)
		{
			MovementSpeed = movementSpeed;
			PayloadCount = payloadCount;
			PayloadType = payloadType;
		}

		public TerrainType MovementConstraint => TerrainType.Land;

		public float MovementSpeed { get; set; }

		public short PayloadCount { get; set; }

		public IMissile PayloadType { get; set; }
	}
}