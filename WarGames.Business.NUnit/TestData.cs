using Map.Engine;
using System.Collections.Generic;
using WarGames.Business.NUnit.Mockers;
using WarGames.Business.Sides;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Business.NUnit
{
	internal class TestData
	{
		public Capitalism Capitalism;
		public Communism Communism;
		public Side Empty;

		public TestData()
		{
			Capitalism = new Capitalism();
			Communism = new Communism();
			Empty = Side.Empty;
			Sides = new List<Side>()
			{
				Capitalism,
				Communism
			};
		}

		public IEnumerable<Side> Sides { get; set; }
		public IMissile StandardMissile => new StandardMissile();

		public IMissileDeliverySystem StandardMissileDeliverySystem(Coord coord)
		{
			return new StandardMissileDeliverySystem(coord);
		}
	}
}