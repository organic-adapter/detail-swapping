using WarGames.Contracts.Arsenal;

namespace WarGames.Contracts.Game
{
	public interface IGameDefaults
	{
		public ArsenalAssignment ArsenalAssignment { get; }
		public IEnumerable<string> ArsenalTags { get; }
		public CountryAssignment CountryAssignment { get; }
		public IEnumerable<string> CountryTags { get; }
		public IEnumerable<IPlayer> GetPlayers();
	}
}