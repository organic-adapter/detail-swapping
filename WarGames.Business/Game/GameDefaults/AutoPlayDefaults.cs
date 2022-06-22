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

		public void CalculateAiTargets(Func<IEnumerable<Settlement>> targets, Action<Settlement, TargetPriority> addAction)
		{
			var topTen = targets().OrderByDescending(target => target.TargetValues.First().Value).Take(10);
			foreach (var t in topTen)
				addAction(t, TargetPriority.Primary);
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