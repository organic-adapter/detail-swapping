using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Competitors
{
	public abstract class BaseCompetitor : ICompetitor
	{
		public BaseCompetitor()
		{
			Countries = new List<Country>();
		}
		public List<Country> Countries { get; set; }
		public abstract string Id { get; }
		public abstract string Name { get; }
	}
}