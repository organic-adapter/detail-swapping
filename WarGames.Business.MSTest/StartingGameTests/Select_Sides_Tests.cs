using Microsoft.Extensions.DependencyInjection;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.MSTest.Mockers;
using WarGames.Contracts.V2.Sides;
using static WarGames.Business.Managers.PlayerSideManager;

namespace WarGames.Business.MSTest.StartingGameTests
{
	[TestClass]
	public class Select_Sides_Tests
	{
		private CurrentGame currentGame;
		private IServiceProvider serviceProvider;
		private TestData testData;

		#region Set Ups

		public Select_Sides_Tests()
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

		[TestMethod]
		public async Task Can_Select_Capitalism()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var expectedSide = testData.Capitalism;

			var actualSide = await playerSideManager.GetSideAsync(expectedSide.Id);

			Assert.AreEqual(expectedSide, actualSide);
		}

		[TestMethod]
		public async Task Can_Select_Communism()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var expectedSide = testData.Communism;

			var actualSide = await playerSideManager.GetSideAsync(expectedSide.Id);

			Assert.AreEqual(expectedSide, actualSide);
		}

		[TestMethod]
		public async Task Cannot_Select_Empty()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var expectedSide = testData.Empty;

			await Assert.ThrowsExceptionAsync<Exception>(() => playerSideManager.GetSideAsync(expectedSide.Id));
		}

		[TestMethod]
		public async Task New_Selection_Ovewrites_Previous_Selection()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var player1 = new Player("Test Player", Guid.NewGuid().ToString(), PlayerType.Human);

			await playerSideManager.AddAsync(player1);
			await playerSideManager.ChooseAsync(player1, testData.Communism);
			await playerSideManager.ChooseAsync(player1, testData.Capitalism);
			var playerSide = await playerSideManager.WhatIsPlayerAsync(player1);

			Assert.AreEqual(testData.Capitalism, playerSide);
		}

		[TestMethod]
		public async Task Select_Capitalism()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var player1 = new Player("Test Player", Guid.NewGuid().ToString(), PlayerType.Human);

			await playerSideManager.AddAsync(player1);
			await playerSideManager.ChooseAsync(player1, testData.Capitalism);
			var playerSide = await playerSideManager.WhatIsPlayerAsync(player1);

			Assert.AreEqual(testData.Capitalism, playerSide);
		}

		[TestMethod]
		public async Task Select_Communism()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var player1 = new Player("Test Player", Guid.NewGuid().ToString(), PlayerType.Human);

			await playerSideManager.AddAsync(player1);
			await playerSideManager.ChooseAsync(player1, testData.Communism);
			var playerSide = await playerSideManager.WhatIsPlayerAsync(player1);

			Assert.AreEqual(testData.Communism, playerSide);
		}

		/// <summary>
		/// The interface to the manager should be in charge of making sure there is no
		/// collision on who is what.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		[ExpectedException(typeof(SideAlreadyTakenException))]
		public async Task Two_Players_Cannot_Both_Pick_The_Same_Side()
		{
			var playerSideManager = GetService<IPlayerSideManager>();
			var player1 = new Player("Test Player 1", Guid.NewGuid().ToString(), PlayerType.Human);
			var player2 = new Player("Test Player 2", Guid.NewGuid().ToString(), PlayerType.Human);
			var theSameSide = testData.Communism;

			await playerSideManager.AddAsync(player1);
			await playerSideManager.AddAsync(player2);

			await playerSideManager.ChooseAsync(player1, theSameSide);
			await playerSideManager.ChooseAsync(player2, theSameSide);
		}

		[TestMethod]
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
			Assert.AreEqual(testData.Communism, communism);
			Assert.AreEqual(testData.Capitalism, capitalism);
		}
	}
}