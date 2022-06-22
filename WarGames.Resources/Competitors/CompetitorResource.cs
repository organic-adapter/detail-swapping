using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Resources.Competitors
{
	public class CompetitorResource : ICompetitorResource
	{
		private readonly Dictionary<IPlayer, ICompetitor> competitors;
		private readonly IEnumerable<ICompetitor> sides;
		private readonly Dictionary<Type, ICompetitor> typeMap;

		public CompetitorResource(IEnumerable<ICompetitor> sides)
		{
			this.sides = sides;
			typeMap = sides.ToDictionary(side => side.GetType(), side => side);
			competitors = new Dictionary<IPlayer, ICompetitor>();
		}

		public IEnumerable<ICompetitor> AvailableSides => sides.Where(side => !competitors.ContainsValue(side));

		public bool HasAvailableSide => competitors.Count == sides.Count();

		public IEnumerable<IPlayer> Players => competitors.Keys;
		public IDictionary<IPlayer, ICompetitor> PlayerSelections => competitors;

		public IEnumerable<ICompetitor> Sides => sides;

		public void Choose(IPlayer player, ICompetitor side)
		{
			if (competitors.ContainsValue(side))
				throw new CompetitorAlreadyUsed();
			else
				competitors.Add(player, side);
		}

		public void Choose<T>(IPlayer player)
			where T : ICompetitor
		{
			var side = typeMap[typeof(T)];
			Choose(player, side);
		}

		public class CompetitorAlreadyUsed : Exception
		{
		}
	}
}