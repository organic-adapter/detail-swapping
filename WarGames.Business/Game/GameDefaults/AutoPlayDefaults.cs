using WarGames.Business.Game;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Resources;

namespace WarGames.Contracts.Game.GameDefaults
{
	public class AutoPlayDefaults : IGameDefaults
	{
		private readonly ICompetitorResource competitorResource;
		private readonly IPlayer cpu0Player;
		private readonly IPlayer cpu1Player;

		public AutoPlayDefaults(ICompetitorResource competitorResource)
		{
			cpu0Player = new Player("JOSHUA0", Guid.NewGuid().ToString(), PlayerType.Cpu);
			cpu1Player = new Player("JOSHUA1", Guid.NewGuid().ToString(), PlayerType.Cpu);
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
			return !competitorResource.Players.Any(p => p.PlayerType == PlayerType.Human);
		}

		public void Trigger()
		{
			competitorResource.Choose(cpu0Player, competitorResource.AvailableSides.First());
			competitorResource.Choose(cpu1Player, competitorResource.AvailableSides.First());
		}
	}
}