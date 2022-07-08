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
	public class Select_Sides_Tests
	{
		private IArsenalAssignmentEngine arsenalAssignmentEngine;
		private ICompetitorBasedGame competitorBasedGame;
		private ICompetitorManager competitorManager;
		private ICountryAssignmentEngine countryAssignmentEngine;
		private IGameManager gameManager;
		private ITargetResource targetResource;
		private TestData testData;

		#region Set Ups

		public Select_Sides_Tests()
		{
			testData = new TestData();
			arsenalAssignmentEngine = new ArsenalAssignmentEngine();
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
			competitorManager = new CompetitorManager(new InMemoryCompetitorRepository(testData.Competitors));
		}

		#endregion Set Ups

		[Fact]
		public async Task Can_Select_Capitalism()
		{
			var competitors = await competitorManager.GetCompetitorsAsync();
			var list = competitors.ToList();
			Assert.Contains(competitors, c => c.Equals(testData.Capitalism));
		}

		[Fact]
		public async Task Can_Select_Communism()
		{
			var competitors = await competitorManager.GetCompetitorsAsync();
			Assert.Contains(competitors, c => c.Equals(testData.Communism));
		}

		[Fact]
		public async Task Cannot_Select_Empty()
		{
			var competitors = await competitorManager.GetCompetitorsAsync();
			Assert.DoesNotContain(competitors, c => c.Equals(testData.Empty));
		}

		[Fact]
		public async Task New_Selection_Ovewrites_Previous_Selection()
		{
			var player1 = new Player("Test Player", Guid.NewGuid().ToString());

			await competitorBasedGame.LoadPlayerAsync(player1, testData.Communism);
			await competitorBasedGame.LoadPlayerAsync(player1, testData.Capitalism);
			var playerCompetitor = await competitorBasedGame.WhatIsPlayerAsync(player1);

			Assert.Equal(testData.Capitalism, playerCompetitor);
		}

		[Fact]
		public async Task Select_Capitalism()
		{
			var player1 = new Player("Test Player", Guid.NewGuid().ToString());

			await competitorBasedGame.LoadPlayerAsync(player1, testData.Capitalism);
			var playerCompetitor = await competitorBasedGame.WhatIsPlayerAsync(player1);

			Assert.Equal(testData.Capitalism, playerCompetitor);
		}

		[Fact]
		public async Task Select_Communism()
		{
			var player1 = new Player("Test Player", Guid.NewGuid().ToString());

			await competitorBasedGame.LoadPlayerAsync(player1, testData.Communism);
			var playerCompetitor = await competitorBasedGame.WhatIsPlayerAsync(player1);

			Assert.Equal(testData.Communism, playerCompetitor);
		}

		/// <summary>
		/// The interface to the manager should be in charge of making sure there is no
		/// collision on who is what.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Two_Players_Cannot_Both_Pick_The_Same_Side()
		{
			var player1 = new Player("Test Player 1", Guid.NewGuid().ToString());
			var player2 = new Player("Test Player 2", Guid.NewGuid().ToString());
			var theSameSide = testData.Communism;

			await competitorBasedGame.LoadPlayerAsync(player1, theSameSide);
			await Assert.ThrowsAsync<CompetitorAlreadyTaken>(() => competitorBasedGame.LoadPlayerAsync(player2, theSameSide));
		}

		[Fact]
		public async Task Two_Players_Select_Communism_Other_Capitalism()
		{
			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await competitorBasedGame.LoadPlayerAsync(playerCommunism, testData.Communism);
			await competitorBasedGame.LoadPlayerAsync(playerCapitalism, testData.Capitalism);
			var communism = await competitorBasedGame.WhatIsPlayerAsync(playerCommunism);
			var capitalism = await competitorBasedGame.WhatIsPlayerAsync(playerCapitalism);
			Assert.Equal(testData.Communism, communism);
			Assert.Equal(testData.Capitalism, capitalism);
		}
	}
}