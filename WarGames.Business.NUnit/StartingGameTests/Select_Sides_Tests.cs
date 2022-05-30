﻿using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WarGames.Business.Exceptions;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Resources.Competitors;

namespace WarGames.Business.NUnit.StartingGameTests
{
	[TestFixture]
	public class Select_Sides_Tests
	{
		private ICompetitorManager competitorManager;
		private TestData testData;
		private IWarManager warManager;

		[Test]
		public async Task Can_Select_Capitalism()
		{
			var competitors = await competitorManager.GetCompetitorsAsync();
			Assert.That(competitors.Any(c => c.Equals(testData.Capitalism)));
		}

		[Test]
		public async Task Can_Select_Communism()
		{
			var competitors = await competitorManager.GetCompetitorsAsync();
			Assert.That(competitors.Any(c => c.Equals(testData.Communism)));
		}

		[Test]
		public async Task Cannot_Select_Empty()
		{
			var competitors = await competitorManager.GetCompetitorsAsync();
			Assert.That(!competitors.Any(c => c.Equals(testData.Empty)));
		}

		[Test]
		public async Task New_Selection_Ovewrites_Previous_Selection()
		{
			var player1 = new Player("Test Player", Guid.NewGuid().ToString());

			await warManager.LoadPlayerAsync(player1, testData.Communism);
			await warManager.LoadPlayerAsync(player1, testData.Capitalism);

			Assert.That(await warManager.WhatIsPlayerAsync(player1), Is.EqualTo(testData.Capitalism));
		}

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			testData = new TestData();
		}

		[Test]
		public async Task Select_Capitalism()
		{
			var player1 = new Player("Test Player", Guid.NewGuid().ToString());

			await warManager.LoadPlayerAsync(player1, testData.Capitalism);

			Assert.That(await warManager.WhatIsPlayerAsync(player1), Is.EqualTo(testData.Capitalism));
		}

		[Test]
		public async Task Select_Communism()
		{
			var player1 = new Player("Test Player", Guid.NewGuid().ToString());

			await warManager.LoadPlayerAsync(player1, testData.Communism);

			Assert.That(await warManager.WhatIsPlayerAsync(player1), Is.EqualTo(testData.Communism));
		}

		[SetUp]
		public void SetUp()
		{
			//We can use the InMemoryCompetitorRepository directly rather than Mock these.
			warManager = new WarManager();
			competitorManager = new CompetitorManager(new InMemoryCompetitorRepository(testData.Competitors));
		}

		/// <summary>
		/// The interface to the manager should be in charge of making sure there is no
		/// collision on who is what.
		/// </summary>
		/// <returns></returns>
		[Test]
		public async Task Two_Players_Cannot_Both_Pick_The_Same_Side()
		{
			var player1 = new Player("Test Player 1", Guid.NewGuid().ToString());
			var player2 = new Player("Test Player 2", Guid.NewGuid().ToString());
			var theSameSide = testData.Communism;

			await warManager.LoadPlayerAsync(player1, theSameSide);
			Assert.ThrowsAsync<CompetitorAlreadyTaken>(() => warManager.LoadPlayerAsync(player2, theSameSide));
		}

		[Test]
		public async Task Two_Players_Select_Communism_Other_Capitalism()
		{
			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await warManager.LoadPlayerAsync(playerCommunism, testData.Communism);
			await warManager.LoadPlayerAsync(playerCapitalism, testData.Capitalism);

			Assert.That(await warManager.WhatIsPlayerAsync(playerCommunism), Is.EqualTo(testData.Communism));
			Assert.That(await warManager.WhatIsPlayerAsync(playerCapitalism), Is.EqualTo(testData.Capitalism));
		}
	}
}