using Map.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal.MissileDeliverySystems
{
	public abstract class BaseMissileDeliverySystem : IMissileDeliverySystem
	{
		protected Coord? moveToLocation;
		public Coord Location { get; set; }

		public abstract TerrainType MovementConstraint { get; }

		public float MovementSpeed { get; set; }

		public short PayloadCount { get; set; }

		public IMissile PayloadType { get; set; }

		public void MoveTo(Coord coord)
		{
			moveToLocation = coord;
		}
	}
}
