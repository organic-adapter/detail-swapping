using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.NUnit.Mockers;
using WarGames.Contracts.Arsenal;
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
		private IPlayer playerCommunism;
		private IPlayer playerCapitalism;
		private IRepository<ICompetitor, string> competitorRepository;
		private ICountryAssignmentEngine countryAssignmentEngine;
		private IGameManager gameManager;
		private ITargetingEngine targetingEngine;
		private ITargetResource targetResource;
		private TestData testData;

		#region Set Ups

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
			targetResource = new TargetResource();
			competitorRepository = new InMemoryCompetitorRepository();

			targetingEngine = new TargetingEngine(targetResource);
			gameManager = new GameManager(testData.World, countryAssignmentEngine, targetResource);

			playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await gameManager.LoadPlayerAsync(playerCommunism, testData.Communism);
			await gameManager.LoadPlayerAsync(playerCapitalism, testData.Capitalism);

			await gameManager.AssignCountriesAsync(CountryAssignment.ByName);
		}

		#endregion Set Ups

		[Test]
		public async Task Can_Find_Targets_In_Range()
		{
			short arbitraryShortGreaterThanZero = 1;
			var arbitraryFloat = 1.0f;
			var maxDistanceKm = 1000f;
			var priority = TargetPriority.Primary;
			var deliverySettlement = testData.Communism.Countries.First().Settlements.OrderBy(settlement => settlement.Location.Coord.Longitude).First();
			var testMissile = new TestMissile(arbitraryFloat, 0, maxDistanceKm, arbitraryFloat);
			var testMissileDeliverySystem = new TestMissileDeliverySystem(deliverySettlement.Location.Area, deliverySettlement.Location, TerrainType.None, 0, arbitraryShortGreaterThanZero, testMissile);
			testData.Communism.Add(testMissileDeliverySystem);

			var closestSettlement = testData.Capitalism.Countries.First().Settlements.OrderByDescending(settlement => settlement.Location.Coord.Longitude).First();
			var farthestSettlement = testData.Capitalism.Countries.First().Settlements.OrderBy(settlement => settlement.Location.Coord.Longitude).First();
			await gameManager.AddTargetAsync(closestSettlement, priority);
			await gameManager.AddTargetAsync(farthestSettlement, priority);

			var targetsInRange = await targetingEngine.CalculateTargetsInRangeAsync(testData.Communism, testData.Capitalism);
			Assert.That(targetsInRange, Is.Not.Null);
			Assert.That(targetsInRange.Count, Is.EqualTo(1));
			Assert.That(targetsInRange.Any(tir => tir.Key == closestSettlement), Is.True);
			Assert.That(targetsInRange.Any(tir => tir.Key == farthestSettlement), Is.False);
		}

		[Test]
		public async Task Can_Get_Settlements()
		{
			var engine = new TargetingEngine(targetResource);

			var comSettlements = await engine.GetSettlementsAsync(testData.Communism);
			var capSettlements = await engine.GetSettlementsAsync(testData.Capitalism);
			Assert.That(comSettlements.Any(), Is.True);
			Assert.That(capSettlements.Any(), Is.True);
			Assert.That(comSettlements.Count, Is.EqualTo(testData.Communism.Countries.Sum(country => country.Settlements.Count)));
			Assert.That(capSettlements.Count, Is.EqualTo(testData.Capitalism.Countries.Sum(country => country.Settlements.Count)));

			Assert.That(testData.Capitalism.Countries.All(country => country.Settlements.All(settlement => capSettlements.Contains(settlement))), Is.True);
			Assert.That(testData.Communism.Countries.All(country => country.Settlements.All(settlement => comSettlements.Contains(settlement))), Is.True);
		}
	}
}