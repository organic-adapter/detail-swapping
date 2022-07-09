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
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Games;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Planet;
using WarGames.Resources.Sides;

namespace WarGames.Business.NUnit.ArsenalTests
{
	[TestFixture]
	public class ValueTargetingTests
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
								.Build();

			currentGame = GetService<CurrentGame>();
			currentGame.GameSession = new GameSession("TEST", GameSession.SessionPhase.New);

			var gameManager = GetService<IGameManager>();
			var playerSideManager = GetService<IPlayerSideManager>();
			var playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString(), PlayerType.Human);
			var playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString(), PlayerType.Human);


			await playerSideManager.AddAsync(testData.Communism, testData.Capitalism);
			await playerSideManager.AddAsync(playerCommunism, playerCapitalism);

			await playerSideManager.ChooseAsync(playerCommunism, testData.Communism);
			await playerSideManager.ChooseAsync(playerCapitalism, testData.Capitalism);

			await gameManager.LoadWorldAsync();
			await gameManager.AssignCountriesAsync(CountryAssignment.ByName);
		}
		private T GetService<T>()
		{
			return serviceProvider.GetService<T>()
				?? throw new ArgumentNullException();
		}
		#endregion Set Ups

		[Test]
		public async Task Game_Can_Target()
		{
			var gameManager = GetService<IGameManager>();
			var targetResource = GetService<ITargetResource>();
			var settlementResource = GetService<ISettlementResource>();
			var settlements = await settlementResource.RetrieveManyAsync(currentGame.GameSession, testData.Capitalism);

			var capHighestValueTarget = settlements.OrderByDescending(s => s.TargetValues.Sum(tv => tv.Value)).First();

			await gameManager.AddTargetAsync(testData.Capitalism, capHighestValueTarget, TargetPriority.Primary);
			var target = await targetResource.RetrieveAsync(currentGame.GameSession, capHighestValueTarget);

			Assert.That(target, Is.Not.Null);
			Assert.That(target.Priority, Is.EqualTo(TargetPriority.Primary));
		}
	}
}