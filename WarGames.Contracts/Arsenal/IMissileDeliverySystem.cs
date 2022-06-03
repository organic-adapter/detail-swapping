using Map.Engine;
using WarGames.Contracts.Game;

namespace WarGames.Contracts.Arsenal
{
	public interface IMissileDeliverySystem
	{
		public Coord Location { get; }

		public TerrainType MovementConstraint { get; }

		public float MovementSpeed { get; }

		public short PayloadCount { get; }

		public IMissile PayloadType { get; }

		public void MoveTo(Coord coord);
	}
}