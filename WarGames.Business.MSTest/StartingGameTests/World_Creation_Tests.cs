using AutoMapper;
using Moq;
using WarGames.Business.Arsenal;
using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.MSTest;
using WarGames.Contracts.Game;
using WarGames.Contracts.V2.Games;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;
using WarGames.Resources.Game;

namespace WarGames.Business.MSTest.StartingGameTests
{
	[TestClass]
	public class World_Creation_Tests : IDisposable
	{
		private ICompetitorBasedGame competitorBasedGame;
		private IGameManager gameManager;
		private ITargetResource targetResource;
		private TestData testData;

		#region Set Ups

		public World_Creation_Tests()
		{
			testData = new TestData();
			targetResource = new TargetResource();

			//We can use the InMemoryRepositories directly rather than Mock these.
			gameManager = new GameManager
							(
							Mock.Of<IMapper>()
							, new WorldFactory(testData.World)
							, new ArsenalAssignmentEngine()
							, new CompetitorResource(testData.Competitors)
							, new CountryAssignmentEngine()
							, Mock.Of<IDamageCalculator>()
							, Mock.Of<IEnumerable<IGameDefaults>>()
							, targetResource
							, Mock.Of<ITargetingCalculator>()							
							);
			competitorBasedGame = gameManager as ICompetitorBasedGame;
			gameManager.LoadWorldAsync().Wait();
		}

		#endregion Set Ups

		public void Dispose()
		{
			//no-op
		}

		[TestMethod]
		public async Task Game_Can_Randomize_Assignment_Evenly()
		{
			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await competitorBasedGame.LoadPlayerAsync(playerCommunism, testData.Communism);
			await competitorBasedGame.LoadPlayerAsync(playerCapitalism, testData.Capitalism);
			var communism = await competitorBasedGame.WhatIsPlayerAsync(playerCommunism);
			var capitalism = await competitorBasedGame.WhatIsPlayerAsync(playerCapitalism);

			await gameManager.AssignCountriesAsync(CountryAssignment.Random);
			Assert.IsTrue(communism.Countries.Count > 0);
			Assert.AreEqual(communism.Countries.Count, capitalism.Countries.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(PlayersNotReady))]
		public async Task Game_Cannot_Assign_Countries_Until_Players_Are_Ready()
		{
			await gameManager.AssignCountriesAsync(CountryAssignment.Random);
		}
	}
}