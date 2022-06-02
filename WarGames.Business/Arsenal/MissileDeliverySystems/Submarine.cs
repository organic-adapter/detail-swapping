using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public class Submarine : IMissileDeliverySystem
	{
		public TerrainType MovementConstraint => TerrainType.Ocean;

		public float MovementSpeed { get; set; }

		public short PayloadCount { get; set; }

		public IMissile PayloadType { get; set; }
	}
}