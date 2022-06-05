using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.NUnit.Mockers;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Game;

namespace WarGames.Business.NUnit.TargetingTests
{
	[TestFixture]
	public class ValueTargettingTests
	{
		private ICountryAssignmentEngine countryAssignmentEngine;
		private IGameManager gameManager;
		private ITargetResource targetResource;
		private TestData testData;

		#region Set Ups

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			testData = new TestData();
			countryAssignmentEngine = new CountryAssignmentEngine();
			targetResource = new TargetResource();
		}

		[SetUp]
		public async Task SetUp()
		{
			//We can use the InMemoryRepositories directly rather than Mock these.
			gameManager = new GameManager(new InMemoryWorldRepository(testData.World), countryAssignmentEngine, targetResource);

			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await gameManager.LoadPlayerAsync(playerCommunism, testData.Communism);
			await gameManager.LoadPlayerAsync(playerCapitalism, testData.Capitalism);

			await gameManager.LoadWorldAsync();

			await gameManager.AssignCountriesAsync(CountryAssignment.ByName);
		}

		#endregion Set Ups

		[Test]
		public async Task Game_Can_Target()
		{
			var priority = TargetPriority.Primary;
			var capHighestValueTarget = testData.Capitalism.Countries.SelectMany(country => country.Settlements.OrderByDescending(s => s.TargetValues.Sum(tv => tv.Value))).First();
			await gameManager.AddTargetAsync(capHighestValueTarget, priority);

			var target = await targetResource.GetAsync(capHighestValueTarget);
			Assert.That(target, Is.Not.Null);
			Assert.That(target.Priority, Is.EqualTo(priority));
		}

	}
}