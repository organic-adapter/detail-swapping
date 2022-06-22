using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Resources
{
	public interface ICompetitorResource
	{
		public IEnumerable<ICompetitor> AvailableSides { get; }
		public bool HasAvailableSide { get; }
		public IEnumerable<IPlayer> Players { get; }
		public IDictionary<IPlayer, ICompetitor> PlayerSelections { get; }
		public IEnumerable<ICompetitor> Sides { get; }

		public void Choose(IPlayer player, ICompetitor side);
		public void Choose<T>(IPlayer player)
			where T : ICompetitor;
	}
}