using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;
using WarGames.Contracts.V2.Games;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;

namespace WarGames.Business.NUnit.ArsenalTests
{
	[TestFixture]
	public class TargetingCalculatorTests
	{
		private ICompetitorBasedGame competitorBasedGame;
		private IGameManager gameManager;
		private IPlayer playerCapitalism;
		private IPlayer playerCommunism;
		private ITargetingCalculator targetingCalculator;
		private TestData testData;

		#region Set Ups

		[SetUp]
		public async Task SetUp()
		{
			SetUpPlayers();
			await SetUpGameManager();
			SetUpMds();
		}

		private async Task SetUpGameManager()
		{
			testData = new TestData();
			//We can use the InMemoryRepositories directly rather than Mock these.
			var targetResource = new TargetResource();
			targetingCalculator = new TargetingCalculator(targetResource);
			gameManager = new GameManager
					(
						new WorldFactory(testData.World)
						, Mock.Of<IArsenalAssignmentEngine>()
						, new CompetitorResource(testData.Competitors)
						, new CountryAssignmentEngine()
						, Mock.Of<IDamageCalculator>()
						, Mock.Of<IEnumerable<IGameDefaults>>()
						, targetResource
						, targetingCalculator
					);
			competitorBasedGame = gameManager as ICompetitorBasedGame;
			await gameManager.LoadWorldAsync();

			await competitorBasedGame.LoadPlayerAsync(playerCommunism, testData.Communism);
			await competitorBasedGame.LoadPlayerAsync(playerCapitalism, testData.Capitalism);

			await gameManager.AssignCountriesAsync(CountryAssignment.ByName);
		}

		private void SetUpMds()
		{
			var communismSettlement = testData.Communism.Settlements.OrderBy(settlement => settlement.Location.Coord.Longitude).First();
			var capitalismSettlement = testData.Capitalism.Settlements.OrderBy(settlement => settlement.Location.Coord.Longitude).First();
			testData.Communism.Add(testData.StandardMissileDeliverySystem(communismSettlement.Location.Area, communismSettlement.Location));
			testData.Capitalism.Add(testData.StandardMissileDeliverySystem(capitalismSettlement.Location.Area, capitalismSettlement.Location));
		}

		private void SetUpPlayers()
		{
			playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());
		}

		#endregion Set Ups

		[Test]
		public async Task Can_Find_Targets_In_Range()
		{
			var closestSettlement = testData.Capitalism.Settlements.OrderByDescending(settlement => settlement.Location.Coord.Longitude).First() as Contracts.V2.World.Settlement;
			var farthestSettlement = testData.Capitalism.Settlements.OrderBy(settlement => settlement.Location.Coord.Longitude).First() as Contracts.V2.World.Settlement;
			await gameManager.AddTargetAsync(closestSettlement, TargetPriority.Primary);
			await gameManager.AddTargetAsync(farthestSettlement, TargetPriority.Primary);

			var targetsInRange = await targetingCalculator.CalculateTargetsInRangeAsync(testData.Communism, testData.Capitalism);

			Assert.That(targetsInRange, Is.Not.Null);
			Assert.That(targetsInRange.Count, Is.EqualTo(1));
			Assert.That(targetsInRange.Any(tir => tir.Key.Key == closestSettlement), Is.True);
			Assert.That(targetsInRange.Any(tir => tir.Key.Key == farthestSettlement), Is.False);
		}

		[Test]
		public async Task Can_Get_Settlements()
		{
			var comSettlements = await targetingCalculator.GetSettlementsAsync(testData.Communism);
			var capSettlements = await targetingCalculator.GetSettlementsAsync(testData.Capitalism);

			Assert.That(comSettlements.Any(), Is.True);
			Assert.That(capSettlements.Any(), Is.True);
			Assert.That(comSettlements.Count, Is.EqualTo(testData.Communism.Countries.Sum(country => country.Settlements.Count)));
			Assert.That(capSettlements.Count, Is.EqualTo(testData.Capitalism.Countries.Sum(country => country.Settlements.Count)));

			Assert.That(testData.Capitalism.Countries.All(country => country.Settlements.All(settlement => capSettlements.Contains(settlement))), Is.True);
			Assert.That(testData.Communism.Countries.All(country => country.Settlements.All(settlement => comSettlements.Contains(settlement))), Is.True);
		}
	}
}