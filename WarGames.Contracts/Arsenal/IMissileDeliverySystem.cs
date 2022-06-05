using Map.Engine;
using WarGames.Contracts.Game;

namespace WarGames.Contracts.Arsenal
{
	public interface IMissileDeliverySystem
	{
		public IGeographicalArea CurrentArea { get; }
		public ILocation Location { get; }

		public TerrainType MovementConstraint { get; }

		public float MovementSpeedKps { get; }

		public short PayloadCount { get; }

		public IMissile PayloadType { get; }

		public void MoveTo(ILocation location);
	}
}