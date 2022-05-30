using System.Collections.Generic;
using WarGames.Business.Competitors;
using WarGames.Contracts.Competitors;

namespace WarGames.Business.NUnit
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
		}

		public IEnumerable<ICompetitor> Competitors { get; set; }
	}
}