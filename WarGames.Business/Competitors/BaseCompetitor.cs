using WarGames.Contracts.Competitors;

namespace WarGames.Business.Competitors
{
	public abstract class BaseCompetitor : ICompetitor
	{
		public abstract string Id { get; }
		public abstract string Name { get; }
	}
}