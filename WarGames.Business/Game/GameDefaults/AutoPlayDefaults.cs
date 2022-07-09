using WarGames.Business.Managers;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Games;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Business.Game.GameDefaults
{
	public class AutoPlayDefaults : IGameDefaults
	{
		private readonly Player cpu0Player;
		private readonly Player cpu1Player;
		private readonly IPlayerSideManager playerSideManager;

		public AutoPlayDefaults(IPlayerSideManager playerSideManager)
		{
			cpu0Player = new Player("JOSHUA0", Guid.NewGuid().ToString(), PlayerType.Cpu);
			cpu1Player = new Player("JOSHUA1", Guid.NewGuid().ToString(), PlayerType.Cpu);

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

		public void CalculateAiTargets()
		{
			throw new NotImplementedException();
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