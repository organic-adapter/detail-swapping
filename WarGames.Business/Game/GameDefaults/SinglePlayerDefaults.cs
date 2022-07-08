using WarGames.Business.Game;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.V2.Games;
using WarGames.Resources;

namespace WarGames.Contracts.Game.GameDefaults
{
	public class SinglePlayerDefaults : IGameDefaults
	{
		private readonly ICompetitorResource competitorResource;
		private readonly IPlayer cpu0Player;
		private readonly Dictionary<GamePhase, Action> phaseActions;
		public SinglePlayerDefaults(ICompetitorResource competitorResource)
		{
			cpu0Player = new Player("JOSHUA", Guid.NewGuid().ToString(), PlayerType.Cpu);
			this.competitorResource = competitorResource;
			phaseActions = new Dictionary<GamePhase, Action>();
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
			return competitorResource.Players.Where(p => p.PlayerType == PlayerType.Human).Count() == 1;
		}

		public void Trigger()
		{
			competitorResource.Choose(cpu0Player, competitorResource.AvailableSides.First());
		}

		public void CalculateAiTargets(Func<IEnumerable<V2.World.Settlement>> targets, Action<V2.World.Settlement, TargetPriority> addAction)
		{
			var topTen = targets().OrderByDescending(target => target.TargetValues.First().Value).Take(10);
			foreach (var t in topTen)
				addAction(t, TargetPriority.Primary);
		}
	}
}