using NUnit.Framework;
using System;
using System.Threading.Tasks;
using WarGames.Business.Arsenal;
using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Game;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Game;

namespace WarGames.Business.NUnit.StartingGameTests
{
	[TestFixture]
	public class World_Creation_Tests
	{
		private IArsenalAssignmentEngine arsenalAssignmentEngine;
		private ICountryAssignmentEngine countryAssignmentEngine;
		private IGameManager gameManager;
		private ITargetResource targetResource;
		private TestData testData;

		#region Set Ups

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			testData = new TestData();
			arsenalAssignmentEngine = new ArsenalAssignmentEngine();
			countryAssignmentEngine = new CountryAssignmentEngine();
			targetResource = new TargetResource();
		}

		[SetUp]
		public void SetUp()
		{
			//We can use the InMemoryRepositories directly rather than Mock these.
			gameManager = new GameManager(testData.World, arsenalAssignmentEngine, countryAssignmentEngine, targetResource);
		}

		#endregion Set Ups

		[Test]
		public async Task Game_Can_Randomize_Assignment_Evenly()
		{
			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await gameManager.LoadPlayerAsync(playerCommunism, testData.Communism);
			await gameManager.LoadPlayerAsync(playerCapitalism, testData.Capitalism);
			var communism = await gameManager.WhatIsPlayerAsync(playerCommunism);
			var capitalism = await gameManager.WhatIsPlayerAsync(playerCapitalism);

			await gameManager.AssignCountriesAsync(CountryAssignment.Random);
			Assert.That(communism.Countries.Count, Is.GreaterThan(0));
			Assert.That(communism.Countries.Count, Is.EqualTo(capitalism.Countries.Count));
		}

		[Test]
		public void Game_Cannot_Assign_Countries_Until_Players_Are_Ready()
		{
			Assert.ThrowsAsync<PlayersNotReady>(() => gameManager.AssignCountriesAsync(CountryAssignment.Random));
		}
	}
}