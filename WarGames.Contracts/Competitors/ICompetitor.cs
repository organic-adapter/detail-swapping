using WarGames.Contracts.Game;

namespace WarGames.Contracts.Competitors
{
	public interface ICompetitor : IUnique<string>
	{
		public string Name { get; }
		public List<Country> Countries { get; }
	}
}