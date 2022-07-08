using AutoMapper;
using Moq;
using WarGames.Business.Arsenal;
using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Game;
using WarGames.Contracts.V2.Games;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;
using WarGames.Resources.Game;

namespace WarGames.Business.MSTest.StartingGameTests
{
	[TestClass]
	public class Select_Sides_Tests
	{
		private ICompetitorManager competitorManager;
		private ICountryAssignmentEngine countryAssignmentEngine;
		private IGameManager gameManager;
		private ICompetitorBasedGame competitorBasedGame;
		private ITargetResource targetResource;
		private TestData testData;

		#region Set Ups

		public Select_Sides_Tests()
		{
			var arsenalAssignmentEngine = new ArsenalAssignmentEngine();
			testData = new TestData();
			countryAssignmentEngine = new CountryAssignmentEngine();
			targetResource = new TargetResource();

			//We can use the InMemoryRepositories directly rather than Mock these.
			gameManager = new GameManager
								(
									Mock.Of<IMapper>()
									, new WorldFactory(testData.World)
									, arsenalAssignmentEngine
									, new CompetitorResource(testData.Competitors)
									, countryAssignmentEngine
									, Mock.Of<IDamageCalculator>()
									, Mock.Of<IEnumerable<IGameDefaults>>()
									, targetResource
									, Mock.Of<ITargetingCalculator>()
								);
			competitorBasedGame = gameManager as ICompetitorBasedGame;
			competitorManager = new CompetitorManager(new InMemoryCompetitorRepository(testData.Competitors));
		}

		#endregion Set Ups

		[TestMethod]
		public async Task Can_Select_Capitalism()
		{
			var competitors = await competitorManager.GetCompetitorsAsync();
			var list = competitors.ToList();
			Assert.IsTrue(competitors.Any(c => c.Equals(testData.Capitalism)));
		}

		[TestMethod]
		public async Task Can_Select_Communism()
		{
			var competitors = await competitorManager.GetCompetitorsAsync();
			Assert.IsTrue(competitors.Any(c => c.Equals(testData.Communism)));
		}

		[TestMethod]
		public async Task Cannot_Select_Empty()
		{
			var competitors = await competitorManager.GetCompetitorsAsync();
			Assert.IsFalse(competitors.Any(c => c.Equals(testData.Empty)));
		}

		[TestMethod]
		public async Task New_Selection_Ovewrites_Previous_Selection()
		{
			var player1 = new Player("Test Player", Guid.NewGuid().ToString());

			await competitorBasedGame.LoadPlayerAsync(player1, testData.Communism);
			await competitorBasedGame.LoadPlayerAsync(player1, testData.Capitalism);
			var playerCompetitor = await competitorBasedGame.WhatIsPlayerAsync(player1);

			Assert.AreEqual(testData.Capitalism, playerCompetitor);
		}

		[TestMethod]
		public async Task Select_Capitalism()
		{
			var player1 = new Player("Test Player", Guid.NewGuid().ToString());

			await competitorBasedGame.LoadPlayerAsync(player1, testData.Capitalism);
			var playerCompetitor = await competitorBasedGame.WhatIsPlayerAsync(player1);

			Assert.AreEqual(testData.Capitalism, playerCompetitor);
		}

		[TestMethod]
		public async Task Select_Communism()
		{
			var player1 = new Player("Test Player", Guid.NewGuid().ToString());

			await competitorBasedGame.LoadPlayerAsync(player1, testData.Communism);
			var playerCompetitor = await competitorBasedGame.WhatIsPlayerAsync(player1);

			Assert.AreEqual(testData.Communism, playerCompetitor);
		}

		/// <summary>
		/// The interface to the manager should be in charge of making sure there is no
		/// collision on who is what.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		[ExpectedException(typeof(CompetitorAlreadyTaken))]
		public async Task Two_Players_Cannot_Both_Pick_The_Same_Side()
		{
			var player1 = new Player("Test Player 1", Guid.NewGuid().ToString());
			var player2 = new Player("Test Player 2", Guid.NewGuid().ToString());
			var theSameSide = testData.Communism;

			await competitorBasedGame.LoadPlayerAsync(player1, theSameSide);
			await competitorBasedGame.LoadPlayerAsync(player2, theSameSide);
		}

		[TestMethod]
		public async Task Two_Players_Select_Communism_Other_Capitalism()
		{
			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await competitorBasedGame.LoadPlayerAsync(playerCommunism, testData.Communism);
			await competitorBasedGame.LoadPlayerAsync(playerCapitalism, testData.Capitalism);
			var communism = await competitorBasedGame.WhatIsPlayerAsync(playerCommunism);
			var capitalism = await competitorBasedGame.WhatIsPlayerAsync(playerCapitalism);
			Assert.AreEqual(testData.Communism, communism);
			Assert.AreEqual(testData.Capitalism, capitalism);
		}
	}
}