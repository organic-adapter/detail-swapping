using WarGames.Business.Managers;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.V2.Games;

namespace WarGames.Contracts.Game.GameDefaults
{
	public class TwoPlayerV2Defaults : IGameDefaults
	{
		private readonly IPlayerSideManager playerSideManager;

		public TwoPlayerV2Defaults(IPlayerSideManager playerSideManager)
		{
			this.playerSideManager = playerSideManager;
		}

		public ArsenalAssignment ArsenalAssignment => ArsenalAssignment.Arbitrary;

		public IEnumerable<string> ArsenalTags => new List<string>();

		public CountryAssignment CountryAssignment => CountryAssignment.Random;

		public IEnumerable<string> CountryTags => new List<string>();

		public void CalculateAiTargets(Func<IEnumerable<V2.World.Settlement>> targets, Action<V2.World.Settlement, TargetPriority> addAction)
		{
			//no-op
		}

		public bool MetRequirements()
		{
			return playerSideManager.Count(PlayerType.Human) == 2;
		}

		public void Trigger()
		{
			//no-op
		}
	}
}