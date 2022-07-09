using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.NUnit.Mockers;
using WarGames.Business.Planet;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Planet;
using WarGames.Startups;

namespace WarGames.Business.NUnit.ArsenalTests
{
	[TestFixture]
	public class TargetingCalculatorTests
	{
		private IServiceProvider serviceProvider;
		private TestData testData;
		private CurrentGame currentGame;
		private Player playerCapitalism;
		private Player playerCommunism;

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

			await SetUpPlayers();
			await SetUpWorld();
			await SetUpMds();
		}
		private async Task SetUpWorld()
		{
			var gameManager = GetService<IGameManager>();

			await gameManager.LoadWorldAsync();
		}
		private T GetService<T>()
		{
			return serviceProvider.GetService<T>()
				?? throw new ArgumentNullException();
		}
		private async Task SetUpMds()
		{
			var settlementResource = GetService<ISettlementResource>();
			var missileDeliverySystemResource = GetService<IMissileDeliverySystemResource>();
			var worldManager = GetService<IWorldManager>();

			var settlements = await settlementResource.RetrieveManyAsync(currentGame.GameSession);

			foreach (var settlement in settlements)
			{
				if (settlement.Name.Contains(testData.Communism.DisplayName))
				{
					await settlementResource.AssignAsync(currentGame.GameSession, playerCommunism, settlement);
					await settlementResource.AssignAsync(currentGame.GameSession, testData.Communism, settlement);
				}
				else
				{
					await settlementResource.AssignAsync(currentGame.GameSession, playerCapitalism, settlement);
					await settlementResource.AssignAsync(currentGame.GameSession, testData.Capitalism, settlement);
				}
			}
			var communismSettlement = (await settlementResource.RetrieveManyAsync(currentGame.GameSession, testData.Communism))
										.OrderByDescending(settlement => settlement.Coord.Longitude).First();
			var capitalismSettlement = (await settlementResource.RetrieveManyAsync(currentGame.GameSession, testData.Capitalism))
										.OrderByDescending(settlement => settlement.Coord.Longitude).First();

			await missileDeliverySystemResource.AssignAsync(currentGame.GameSession, testData.Communism, testData.StandardMissileDeliverySystem(communismSettlement.Coord));
			await missileDeliverySystemResource.AssignAsync(currentGame.GameSession, testData.Capitalism, testData.StandardMissileDeliverySystem(capitalismSettlement.Coord));
		}

		private async Task SetUpPlayers()
		{
			var playerSideManager = GetService<IPlayerSideManager>();

			playerCommunism = new Player("Test Player Communism", Guid.NewGuid().ToString(), PlayerType.Human);
			playerCapitalism = new Player("Test Player Capitalism", Guid.NewGuid().ToString(), PlayerType.Human);
			await playerSideManager.AddAsync(testData.Communism, testData.Capitalism);
			await playerSideManager.AddAsync(playerCommunism, playerCapitalism);

			await playerSideManager.ChooseAsync(playerCommunism, testData.Communism);
			await playerSideManager.ChooseAsync(playerCapitalism, testData.Capitalism);
		}

		#endregion Set Ups

		[Test]
		public async Task Can_Find_Targets_In_Range()
		{
			var gameManager = GetService<IGameManager>();
			var targetingCalculator = GetService<ITargetingCalculator>();
			var settlementResource = GetService<ISettlementResource>();
			var settlements = await settlementResource.RetrieveManyAsync(currentGame.GameSession, playerCapitalism);

			Settlement closestSettlement = settlements.OrderByDescending(settlement => settlement.Coord.Longitude).First();
			Settlement farthestSettlement = settlements.OrderBy(settlement => settlement.Coord.Longitude).First();
			await gameManager.AddTargetAsync(testData.Capitalism, closestSettlement, TargetPriority.Primary);
			await gameManager.AddTargetAsync(testData.Capitalism, farthestSettlement, TargetPriority.Primary);

			var targetsInRange = await targetingCalculator.CalculateTargetsInRangeAsync(testData.Communism, testData.Capitalism);

			Assert.That(targetsInRange, Is.Not.Null);
			Assert.That(targetsInRange.Count, Is.EqualTo(1));
			Assert.That(targetsInRange.Any(kvp => kvp.Key.Key.Equals(closestSettlement)), Is.True);
			Assert.That(targetsInRange.Any(kvp => kvp.Key.Key.Equals(farthestSettlement)), Is.False);
		}

		[Test]
		public async Task Can_Get_Settlements()
		{
			var targetingCalculator = GetService<ITargetingCalculator>();
			var settlementResource = GetService<ISettlementResource>();
			var comSettlements = await settlementResource.RetrieveManyAsync(currentGame.GameSession, testData.Communism);
			var capSettlements = await settlementResource.RetrieveManyAsync(currentGame.GameSession, testData.Capitalism);

			Assert.That(comSettlements.Any(), Is.True);
			Assert.That(capSettlements.Any(), Is.True);
		}
	}
}