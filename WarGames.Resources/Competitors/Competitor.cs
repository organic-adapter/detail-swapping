using WarGames.Contracts.Competitors;

namespace WarGames.Resources.Competitors
{
	public class Competitor : ICompetitor
	{
		public Competitor(string name, string id)
		{
			Name = name;
			Id = id;
		}

		public Competitor()
		{
			Name = string.Empty;
			Id = string.Empty;
		}

		public static Competitor Empty => new Competitor();
		public string Id { get; set; }
		public string Name { get; set; }
	}
}