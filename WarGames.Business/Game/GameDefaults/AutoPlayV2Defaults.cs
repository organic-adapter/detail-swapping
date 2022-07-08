using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources;
using WarGames.Resources.Sides;

namespace WarGames.Game.GameDefaults
{
	public class AutoPlayV2Defaults : IGameDefaults
	{
		private readonly IPlayerSideManager playerSideManager;
		private readonly Contracts.V2.Sides.Player cpu0Player;
		private readonly Contracts.V2.Sides.Player cpu1Player;

		public AutoPlayV2Defaults(IPlayerSideManager playerSideManager)
		{

			cpu0Player = new Contracts.V2.Sides.Player("JOSHUA0", Guid.NewGuid().ToString(), PlayerType.Cpu);
			cpu1Player = new Contracts.V2.Sides.Player("JOSHUA1", Guid.NewGuid().ToString(), PlayerType.Cpu);
			
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
			return !playerSideManager.HasPlayerType(PlayerType.Human);
		}

		public void Trigger()
		{
			var cpu0Side = playerSideManager.NextAvailableSideAsync().Result;
			playerSideManager.ChooseAsync(cpu0Player, cpu0Side);

			var cpu1Side = playerSideManager.NextAvailableSideAsync().Result;
			playerSideManager.ChooseAsync(cpu1Player, cpu1Side);		
		}
	}
}