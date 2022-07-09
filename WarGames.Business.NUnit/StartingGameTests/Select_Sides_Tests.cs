using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.NUnit.Mockers;
using WarGames.Business.Planet;
using WarGames.Business.Sides;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Games;
using WarGames.Contracts.V2.Sides;
using WarGames.Resources.Arsenal;
using static WarGames.Business.Managers.PlayerSideManager;

namespace WarGames.Business.NUnit.StartingGameTests
{
	[TestFixture]
	public class Select_Sides_Tests
	{
		private CurrentGame currentGame;
		private IServiceProvider serviceProvider;
		private TestData testData;

		#region Set Ups

		[SetUp]
		public async Task SetUp()
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

			var playerSideManager = GetService<IPlayerSideManager>();
		}

		private T GetService<T>()
		{
			return serviceProvider.GetService<T>()
				?? throw new ArgumentNullException();
		}
		#endregion Set Ups

		[Test]
		public async Task Can_Select_Capitalism()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var expectedSide = testData.Capitalism;

			var actualSide = await playerSideManager.GetSideAsync(expectedSide.Id);
			Assert.That(actualSide, Is.EqualTo(expectedSide));
		}

		[Test]
		public async Task Can_Select_Communism()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var expectedSide = testData.Communism;

			var actualSide = await playerSideManager.GetSideAsync(expectedSide.Id);
			Assert.That(actualSide, Is.EqualTo(expectedSide));
		}

		[Test]
		public async Task Cannot_Select_Empty()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var expectedSide = testData.Empty;

			Assert.ThrowsAsync<KeyNotFoundException>(() => playerSideManager.GetSideAsync(expectedSide.Id));
		}

		[Test]
		public async Task New_Selection_Ovewrites_Previous_Selection()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var player1 = new Player("Test Player", Guid.NewGuid().ToString(), PlayerType.Human);

			await playerSideManager.AddAsync(player1);
			await playerSideManager.ChooseAsync(player1, testData.Communism);
			await playerSideManager.ChooseAsync(player1, testData.Capitalism);
			var playerSide = await playerSideManager.WhatIsPlayerAsync(player1);

			Assert.That(playerSide, Is.EqualTo(testData.Capitalism));
		}

		[Test]
		public async Task Select_Capitalism()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var player1 = new Player("Test Player", Guid.NewGuid().ToString(), PlayerType.Human);

			await playerSideManager.AddAsync(player1);
			await playerSideManager.ChooseAsync(player1, testData.Capitalism);
			var playerSide = await playerSideManager.WhatIsPlayerAsync(player1);

			Assert.That(playerSide, Is.EqualTo(testData.Capitalism));
		}

		[Test]
		public async Task Select_Communism()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var player1 = new Player("Test Player", Guid.NewGuid().ToString(), PlayerType.Human);

			await playerSideManager.AddAsync(player1);
			await playerSideManager.ChooseAsync(player1, testData.Communism);
			var playerSide = await playerSideManager.WhatIsPlayerAsync(player1);

			Assert.That(playerSide, Is.EqualTo(testData.Communism));
		}

		/// <summary>
		/// The interface to the manager should be in charge of making sure there is no
		/// collision on who is what.
		/// </summary>
		/// <returns></returns>
		[Test]
		public async Task Two_Players_Cannot_Both_Pick_The_Same_Side()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var player1 = new Player("Test Player 1", Guid.NewGuid().ToString(), PlayerType.Human);
			var player2 = new Player("Test Player 2", Guid.NewGuid().ToString(), PlayerType.Human);
			var theSameSide = testData.Communism;

			await playerSideManager.AddAsync(player1);
			await playerSideManager.AddAsync(player2);

			await playerSideManager.ChooseAsync(player1, theSameSide);
			Assert.ThrowsAsync<SideAlreadyTakenException>(() => playerSideManager.ChooseAsync(player2, theSameSide));
		}

		[Test]
		public async Task Two_Players_Select_Communism_Other_Capitalism()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString(), PlayerType.Human);
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString(), PlayerType.Human);

			await playerSideManager.AddAsync(playerCommunism);
			await playerSideManager.AddAsync(playerCapitalism);
			await playerSideManager.ChooseAsync(playerCommunism, testData.Communism);
			await playerSideManager.ChooseAsync(playerCapitalism, testData.Capitalism);

			var communism = await playerSideManager.WhatIsPlayerAsync(playerCommunism);
			var capitalism = await playerSideManager.WhatIsPlayerAsync(playerCapitalism);

			Assert.That(communism, Is.EqualTo(testData.Communism));
			Assert.That(capitalism, Is.EqualTo(testData.Capitalism));
		}
	}
}