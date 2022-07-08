using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Arsenal;

namespace WarGames.Contracts.Game.GameDefaults
{
	public class SinglePlayerV2Defaults : IGameDefaults
	{
		private readonly V2.Sides.Player cpu0Player;
		private readonly IPlayerSideManager playerSideManager;

		public SinglePlayerV2Defaults(IPlayerSideManager playerSideManager)
		{
			cpu0Player = new V2.Sides.Player("JOSHUA", Guid.NewGuid().ToString(), PlayerType.Cpu);

			this.playerSideManager = playerSideManager;
		}

		public ArsenalAssignment ArsenalAssignment => ArsenalAssignment.Arbitrary;

		public IEnumerable<string> ArsenalTags => new List<string>();

		public CountryAssignment CountryAssignment => CountryAssignment.Random;

		public IEnumerable<string> CountryTags => new List<string>();

		public void CalculateAiTargets(Func<IEnumerable<Settlement>> targets, Action<Settlement, TargetPriority> addAction)
		{
			var topTen = targets().OrderByDescending(target => target.TargetValues.First().Value).Take(10);
			foreach (var t in topTen)
				addAction(t, TargetPriority.Primary);
		}

		public bool MetRequirements()
		{
			return playerSideManager.Count(PlayerType.Human) == 1;
		}

		public void Trigger()
		{
			var cpu0Side = playerSideManager.NextAvailableSideAsync().Result;
			playerSideManager.ChooseAsync(cpu0Player, cpu0Side);
		}
	}
}