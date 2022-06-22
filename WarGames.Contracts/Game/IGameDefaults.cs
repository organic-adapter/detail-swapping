using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;

namespace WarGames.Contracts.Game
{
	public interface IGameDefaults
	{
		public ArsenalAssignment ArsenalAssignment { get; }
		public IEnumerable<string> ArsenalTags { get; }
		public CountryAssignment CountryAssignment { get; }
		public IEnumerable<string> CountryTags { get; }
		public IDictionary<IPlayer, ICompetitor> GetPlayers();
	}
}