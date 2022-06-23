using WarGames.Contracts.Competitors;

namespace WarGames.Contracts.Game
{
	public interface IPlayerSide
	{
		public ICompetitor Competitor { get; set; }
		public IPlayer Player { get; set; }
	}
}