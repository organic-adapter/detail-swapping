using WarGames.Contracts.Game;

namespace WarGames.Contracts.Arsenal
{
	public interface IMissileDeliverySystem
	{
		public TerrainType MovementConstraint { get; }
		public float MovementSpeed { get; }
		public short PayloadCount { get; }
		public IMissile PayloadType { get; }
	}
}