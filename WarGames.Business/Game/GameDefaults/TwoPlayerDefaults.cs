using WarGames.Business.Managers;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Games;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Business.Game.GameDefaults
{
	public class TwoPlayerDefaults : IGameDefaults
	{
		private readonly IPlayerSideManager playerSideManager;

		public TwoPlayerDefaults(IPlayerSideManager playerSideManager)
		{
			this.playerSideManager = playerSideManager;
		}

		public ArsenalAssignment ArsenalAssignment => ArsenalAssignment.Arbitrary;

		public IEnumerable<string> ArsenalTags => new List<string>();

		public CountryAssignment CountryAssignment => CountryAssignment.Random;

		public IEnumerable<string> CountryTags => new List<string>();

		public void CalculateAiTargets(Func<IEnumerable<Settlement>> targets, Action<Settlement, TargetPriority> addAction)
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