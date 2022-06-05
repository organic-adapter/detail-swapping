using WarGames.Business.Competitors;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;
using WarGames.Resources.Game;

namespace WarGames.Simulator.NUnit
{
	[TestFixture]
	public class BasicSimulation
	{
		private IRepository<ICompetitor, string> competitorRepository;
		private IGameManager gameManager;
		private ITargetResource targetResource;
		private IRepository<World, Guid> worldRepository;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			competitorRepository = new InMemoryCompetitorRepository();
			targetResource = new TargetResource();
			worldRepository = new InMemoryWorldRepository();

			gameManager.LoadPlayerAsync(new Player("Smith", Guid.NewGuid().ToString()), new Capitalism());
			gameManager.LoadPlayerAsync(new Player("Marx", Guid.NewGuid().ToString()), new Communism());

			gameManager.LoadWorldAsync();
		}
	}
}