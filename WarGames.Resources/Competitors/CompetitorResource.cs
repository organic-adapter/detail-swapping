using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Resources.Competitors
{
	public class CompetitorResource : ICompetitorResource
	{
		private readonly Dictionary<IPlayer, ICompetitor> competitors;
		private readonly IEnumerable<ICompetitor> sides;
		public CompetitorResource(IEnumerable<ICompetitor> sides)
		{
			this.sides = sides;
			competitors = new Dictionary<IPlayer, ICompetitor>();
		}

		public IEnumerable<ICompetitor> AvailableSides => sides;

		public bool HasAvailableSide => competitors.Count == sides.Count();

		public IDictionary<IPlayer, ICompetitor> PlayerSelections => competitors;

		public IEnumerable<ICompetitor> Sides => sides;

		public void Choose(IPlayer player, ICompetitor side)
		{
			if (competitors.ContainsValue(side))
				throw new CompetitorAlreadyUsed();
			else
				competitors.Add(player, side);
		}

		public class CompetitorAlreadyUsed : Exception
		{
		}
	}
}