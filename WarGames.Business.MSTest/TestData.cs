using System.Collections.Generic;
using WarGames.Business.Competitors;
using WarGames.Business.MSTest.Mockers;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.MSTest
{
	public class TestData
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
		public World World { get; set; }
	}
}