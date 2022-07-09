using Map.Engine;
using WarGames.Contracts.Game;

namespace WarGames.Contracts.V2.Arsenal
{
	public interface IMissileDeliverySystem
	{
		public Target Assignment { get; set; }

		public bool HasTarget { get; }

		public Coord Coord { get; set; }

		public short PayloadCount { get; set; }

		public IMissile PayloadType { get; set; }

		public bool InAttackRange(Coord coord);
	}
}