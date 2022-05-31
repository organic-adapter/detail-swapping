using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Resources.Competitors
{
	public class Competitor : ICompetitor
	{
		public Competitor(string name, string id)
		{
			Name = name;
			Id = id;
			Countries = new List<Country>();
		}

		public Competitor()
		{
			Name = string.Empty;
			Id = string.Empty;
			Countries = new List<Country>();
		}

		public static Competitor Empty => new Competitor();
		public List<Country> Countries { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
	}
}