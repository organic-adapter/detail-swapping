using System.Collections.Generic;
using WarGames.Business.Competitors;
using WarGames.Business.NUnit.Mockers;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.NUnit
{
	internal class TestData
	{
		public ICompetitor Capitalism;
		public ICompetitor Communism;
		public ICompetitor Empty;

		public TestData()
		{
			Capitalism = new Capitalism();
			Communism = new Communism();
			Empty = Competitor.Empty;
			Competitors = new List<ICompetitor>()
			{
				Capitalism,
				Communism
			};

			World = TestWorldFactory.Make();
		}

		public IEnumerable<ICompetitor> Competitors { get; set; }
		public IMissile StandardMissile => new StandardMissile();
		public World World { get; set; }

		public IMissileDeliverySystem StandardMissileDeliverySystem(IGeographicalArea area, ILocation location)
		{
			return new StandardMissileDeliverySystem(location);
		}
	}
}