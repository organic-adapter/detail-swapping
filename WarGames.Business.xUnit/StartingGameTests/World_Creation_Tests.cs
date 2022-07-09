using Microsoft.Extensions.DependencyInjection;
using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.xUnit.Mockers;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Business.xUnit.StartingGameTests
{
	public class World_Creation_Tests : IDisposable
	{
		private CurrentGame currentGame;
		private IServiceProvider serviceProvider;
		private TestData testData;

		#region Set Ups

		public World_Creation_Tests()
		{
			testData = new TestData();
			serviceProvider = ServicesMocker
								.DefaultMocker
								.Build();

			currentGame = GetService<CurrentGame>();
		}

		private T GetService<T>()
		{
			return serviceProvider.GetService<T>()
				?? throw new ArgumentNullException();
		}

		#endregion Set Ups

		public void Dispose()
		{
			//no-op
		}

		[Fact]
		public async Task Game_Can_Randomize_Assignment_Evenly()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var gameManager = GetService<IGameManager>();
			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString(), PlayerType.Human);
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString(), PlayerType.Human);
			await playerSideManager.AddAsync(playerCommunism);
			await playerSideManager.AddAsync(playerCapitalism);

			await playerSideManager.ChooseAsync(playerCommunism, testData.Communism);
			await playerSideManager.ChooseAsync(playerCapitalism, testData.Capitalism);
			var communism = await playerSideManager.WhatIsPlayerAsync(playerCommunism);
			var capitalism = await playerSideManager.WhatIsPlayerAsync(playerCapitalism);

			await gameManager.AssignCountriesAsync(CountryAssignment.Random);
			Assert.True(communism.Countries.Count > 0);
			Assert.Equal(communism.Countries.Count, capitalism.Countries.Count);
		}

		[Fact]
		public void Game_Cannot_Assign_Countries_Until_Players_Are_Ready()
		{
			var gameManager = GetService<IGameManager>();
			Assert.ThrowsAsync<PlayersNotReady>(() => gameManager.AssignCountriesAsync(CountryAssignment.Random));
		}
	}
}