using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Game;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Game;

namespace WarGames.Business.xUnit.StartingGameTests
{
	public class World_Creation_Tests : IDisposable
	{
		private ICountryAssignmentEngine countryAssignmentEngine;
		private IGameManager gameManager;
		private ITargetResource targetResource;
		private TestData testData;

		#region Set Ups

		public World_Creation_Tests()
		{
			testData = new TestData();
			countryAssignmentEngine = new CountryAssignmentEngine();
			targetResource = new TargetResource();

			//We can use the InMemoryRepositories directly rather than Mock these.
			gameManager = new GameManager(new InMemoryWorldRepository(testData.World), countryAssignmentEngine, targetResource);
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

			await gameManager.LoadPlayerAsync(playerCommunism, testData.Communism);
			await gameManager.LoadPlayerAsync(playerCapitalism, testData.Capitalism);
			var communism = await gameManager.WhatIsPlayerAsync(playerCommunism);
			var capitalism = await gameManager.WhatIsPlayerAsync(playerCapitalism);

			await gameManager.LoadWorldAsync();

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