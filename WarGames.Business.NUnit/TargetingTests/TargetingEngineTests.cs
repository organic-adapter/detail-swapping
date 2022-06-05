using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;
using WarGames.Resources.Game;

namespace WarGames.Business.NUnit.TargetingTests
{
	[TestFixture]
	public class TargetingEngineTests
	{
		private IRepository<ICompetitor, string> competitorRepository;
		private ICountryAssignmentEngine countryAssignmentEngine;
		private IGameManager gameManager;
		private ITargetResource targetResource;
		private TestData testData;

		[Test]
		public async Task Can_Get_Settlements()
		{
			var engine = new TargetingEngine(targetResource, competitorRepository);

			var comSettlements = await engine.GetSettlementsAsync(testData.Communism);
			var capSettlements = await engine.GetSettlementsAsync(testData.Capitalism);
			Assert.That(comSettlements.Any(), Is.True);
			Assert.That(capSettlements.Any(), Is.True);
			Assert.That(comSettlements.Count, Is.EqualTo(testData.Communism.Countries.Sum(country => country.Settlements.Count)));
			Assert.That(capSettlements.Count, Is.EqualTo(testData.Capitalism.Countries.Sum(country => country.Settlements.Count)));

			Assert.That(testData.Capitalism.Countries.All(country => country.Settlements.All(settlement => capSettlements.Contains(settlement))), Is.True);
			Assert.That(testData.Communism.Countries.All(country => country.Settlements.All(settlement => comSettlements.Contains(settlement))), Is.True);
		}

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			testData = new TestData();
			countryAssignmentEngine = new CountryAssignmentEngine();
		}

		[SetUp]
		public async Task SetUp()
		{
			//We can use the InMemoryRepositories directly rather than Mock these.
			competitorRepository = new InMemoryCompetitorRepository();
			gameManager = new GameManager(new InMemoryWorldRepository(testData.World), countryAssignmentEngine);
			targetResource = new TargetResource();

			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await gameManager.LoadPlayerAsync(playerCommunism, testData.Communism);
			await gameManager.LoadPlayerAsync(playerCapitalism, testData.Capitalism);

			await gameManager.LoadWorldAsync();

			await gameManager.AssignCountriesAsync(CountryAssignment.ByName);
		}
	}
}