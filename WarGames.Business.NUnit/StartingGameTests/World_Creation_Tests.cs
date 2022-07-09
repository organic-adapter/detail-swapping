using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.NUnit.Mockers;
using WarGames.Business.Planet;
using WarGames.Business.Sides;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Business.NUnit.StartingGameTests
{
	[TestFixture]
	public class World_Creation_Tests
	{
		private CurrentGame currentGame;
		private IServiceProvider serviceProvider;
		private TestData testData;

		#region Set Ups

		[SetUp]
		public void SetUp()
		{
			testData = new TestData();
			serviceProvider = ServicesMocker
								.DefaultMocker
								.AddSingleton<IWorldBuildingEngine, TestWorldBuildingEngine>()
								.AddSingleton<Side, Capitalism>(testData.Capitalism)
								.AddSingleton<Side, Communism>(testData.Communism)
								.Build();

			currentGame = GetService<CurrentGame>();
			currentGame.GameSession = new GameSession("TEST", GameSession.SessionPhase.New);

		}

		private T GetService<T>()
		{
			return serviceProvider.GetService<T>()
				?? throw new ArgumentNullException();
		}

		#endregion Set Ups

		[Test]
		public async Task Game_Can_Randomize_Assignment_Evenly()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var gameManager = GetService<IGameManager>();
			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString(), PlayerType.Human);
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString(), PlayerType.Human);
			await playerSideManager.AddAsync(playerCommunism, playerCapitalism);

			await playerSideManager.ChooseAsync(playerCommunism, testData.Communism);
			await playerSideManager.ChooseAsync(playerCapitalism, testData.Capitalism);
			var communism = await playerSideManager.WhatIsPlayerAsync(playerCommunism);
			var capitalism = await playerSideManager.WhatIsPlayerAsync(playerCapitalism);

			await gameManager.LoadWorldAsync();
			await gameManager.AssignCountriesAsync(CountryAssignment.Random);

			Assert.That(communism.Countries.Count, Is.GreaterThan(0));
			Assert.That(communism.Countries.Count, Is.EqualTo(capitalism.Countries.Count));
		}

		[Test]
		public void Game_Cannot_Assign_Countries_Until_Players_Are_Ready()
		{
			var gameManager = GetService<IGameManager>();
			Assert.ThrowsAsync<PlayersNotReady>(() => gameManager.AssignCountriesAsync(CountryAssignment.Random));
		}
	}
}