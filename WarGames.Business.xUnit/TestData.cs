using WarGames.Business.Sides;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Business.xUnit
{
	internal class TestData
	{
		public Side Capitalism;
		public Side Communism;
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
	}
}