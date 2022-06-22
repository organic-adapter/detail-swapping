using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Resources;

namespace WarGames.Contracts.Game.GameDefaults
{
	public class TwoPlayerDefaults : IGameDefaults
	{
		private readonly ICompetitorResource competitorResource;

		public TwoPlayerDefaults(ICompetitorResource competitorResource)
		{
			this.competitorResource = competitorResource;
		}

		public ArsenalAssignment ArsenalAssignment => ArsenalAssignment.Arbitrary;

		public IEnumerable<string> ArsenalTags => new List<string>();

		public CountryAssignment CountryAssignment => CountryAssignment.Random;

		public IEnumerable<string> CountryTags => new List<string>();

		public IDictionary<IPlayer, ICompetitor> GetPlayers()
		{
			return competitorResource.PlayerSelections;
		}

		public bool MetRequirements()
		{
			return competitorResource.Players.Where(p => p.PlayerType == PlayerType.Human).Count() == 2;

		}

		public void Trigger()
		{
			//no-op
		}
	}
}