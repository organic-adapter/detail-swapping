using WarGames.Business.Game;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Resources;

namespace WarGames.Contracts.Game.GameDefaults
{
	public class SinglePlayerDefaults : IGameDefaults
	{
		private readonly ICompetitorResource competitorResource;
		private readonly IPlayer cpu0Player;

		public SinglePlayerDefaults(ICompetitorResource competitorResource)
		{
			cpu0Player = new Player("JOSHUA", Guid.NewGuid().ToString());
			this.competitorResource = competitorResource;
			this.competitorResource.Choose(cpu0Player, competitorResource.AvailableSides.First());
		}

		public ArsenalAssignment ArsenalAssignment => ArsenalAssignment.Arbitrary;

		public IEnumerable<string> ArsenalTags => new List<string>();

		public CountryAssignment CountryAssignment => CountryAssignment.Random;

		public IEnumerable<string> CountryTags => new List<string>();

		public IDictionary<IPlayer, ICompetitor> GetPlayers()
		{
			return competitorResource.PlayerSelections;
		}
	}
}