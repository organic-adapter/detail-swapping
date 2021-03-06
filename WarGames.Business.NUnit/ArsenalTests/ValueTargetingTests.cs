using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;
using WarGames.Resources.Arsenal;

namespace WarGames.Business.NUnit.ArsenalTests
{
	[TestFixture]
	public class ValueTargetingTests
	{
		private IGameManager gameManager;
		private ITargetResource targetResource;
		private TestData testData;

		#region Set Ups

		[SetUp]
		public async Task SetUp()
		{
			testData = new TestData();
			targetResource = new TargetResource();

			//We can use the InMemoryRepositories directly rather than Mock these.
			gameManager = new GameManager
					(
						testData.World
						, Mock.Of<IArsenalAssignmentEngine>()
						, new CountryAssignmentEngine()
						, targetResource
					); ;

			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await gameManager.LoadPlayerAsync(playerCommunism, testData.Communism);
			await gameManager.LoadPlayerAsync(playerCapitalism, testData.Capitalism);

			await gameManager.AssignCountriesAsync(CountryAssignment.ByName);
		}

		#endregion Set Ups

		[Test]
		public async Task Game_Can_Target()
		{
			var capHighestValueTarget = testData.Capitalism.Settlements.OrderByDescending(s => s.TargetValues.Sum(tv => tv.Value)).First();
			
			await gameManager.AddTargetAsync(capHighestValueTarget, TargetPriority.Primary);
			var target = await targetResource.GetAsync(capHighestValueTarget);

			Assert.That(target, Is.Not.Null);
			Assert.That(target.Priority, Is.EqualTo(TargetPriority.Primary));
		}
	}
}