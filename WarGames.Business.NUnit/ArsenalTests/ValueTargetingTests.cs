﻿using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;
using WarGames.Contracts.V2.Games;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;

namespace WarGames.Business.NUnit.ArsenalTests
{
	[TestFixture]
	public class ValueTargetingTests
	{
		private ICompetitorBasedGame competitorBasedGame;
		private IGameManager gameManager;
		private ITargetResource targetResource;
		private TestData testData;

		#region Set Ups

		[SetUp]
		public async Task SetUp()
		{
			testData = new TestData();
			targetResource = new TargetResource();

			//We can use the InMemoryRepositories directly rather than Mock these.
			gameManager = new GameManager
					(
						Mock.Of<IMapper>()
						, new WorldFactory(testData.World)
						, Mock.Of<IArsenalAssignmentEngine>()
						, new CompetitorResource(testData.Competitors)
						, new CountryAssignmentEngine()
						, Mock.Of<IDamageCalculator>()
						, Mock.Of<IEnumerable<IGameDefaults>>()
						, targetResource
						, Mock.Of<ITargetingCalculator>()
					);
			competitorBasedGame = gameManager as ICompetitorBasedGame;

			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString());
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString());

			await gameManager.LoadWorldAsync();

			await competitorBasedGame.LoadPlayerAsync(playerCommunism, testData.Communism);
			await competitorBasedGame.LoadPlayerAsync(playerCapitalism, testData.Capitalism);

			await gameManager.AssignCountriesAsync(CountryAssignment.ByName);
		}

		#endregion Set Ups

		[Test]
		public async Task Game_Can_Target()
		{
			var capHighestValueTarget = testData.Capitalism.Settlements.OrderByDescending(s => s.TargetValues.Sum(tv => tv.Value)).First() as Contracts.V2.World.Settlement;

			await gameManager.AddTargetAsync(capHighestValueTarget, TargetPriority.Primary);
			var target = await targetResource.GetAsync(capHighestValueTarget);

			Assert.That(target, Is.Not.Null);
			Assert.That(target.Priority, Is.EqualTo(TargetPriority.Primary));
		}
	}
}