using Moq;
using WarGames.Business.Arsenal;
using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Game;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;

namespace WarGames.Business.xUnit.StartingGameTests
{
	public class World_Creation_Tests : IDisposable
	{
		private IArsenalAssignmentEngine arsenalAssignmentEngine;
		private ICompetitorBasedGame competitorBasedGame;
		private ICountryAssignmentEngine countryAssignmentEngine;
		private IGameManager gameManager;
		private ITargetResource targetResource;
		private TestData testData;

		#region Set Ups

		public World_Creation_Tests()
		{
			arsenalAssignmentEngine = new ArsenalAssignmentEngine();
			testData = new TestData();
			countryAssignmentEngine = new CountryAssignmentEngine();
			targetResource = new TargetResource();

			//We can use the InMemoryRepositories directly rather than Mock these.
			gameManager = new GameManager
					(
						new WorldFactory(testData.World)
						, arsenalAssignmentEngine
						, new CompetitorResource(testData.Competitors)
						, countryAssignmentEngine
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

		[Fact]
		public async Task Game_Can_Randomize_Assignment_Evenly()
		{
			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await competitorBasedGame.LoadPlayerAsync(playerCommunism, testData.Communism);
			await competitorBasedGame.LoadPlayerAsync(playerCapitalism, testData.Capitalism);
			var communism = await competitorBasedGame.WhatIsPlayerAsync(playerCommunism);
			var capitalism = await competitorBasedGame.WhatIsPlayerAsync(playerCapitalism);

			await gameManager.AssignCountriesAsync(CountryAssignment.Random);
			Assert.True(communism.Countries.Count > 0);
			Assert.Equal(communism.Countries.Count, capitalism.Countries.Count);
		}

		[Fact]
		public void Game_Cannot_Assign_Countries_Until_Players_Are_Ready()
		{
			Assert.ThrowsAsync<PlayersNotReady>(() => gameManager.AssignCountriesAsync(CountryAssignment.Random));
		}
	}
}