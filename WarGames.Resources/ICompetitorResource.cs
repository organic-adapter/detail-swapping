using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Resources
{
	public interface ICompetitorResource
	{
		public IEnumerable<ICompetitor> AvailableSides { get; }
		public bool HasAvailableSide { get; }
		public IDictionary<IPlayer, ICompetitor> PlayerSelections { get; }
		public IEnumerable<ICompetitor> Sides { get; }

		public void Choose(IPlayer player, ICompetitor side);
	}
}