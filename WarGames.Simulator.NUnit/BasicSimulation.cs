using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using WarGames.Business.Arsenal.MissileDeliverySystems;
using WarGames.Business.Arsenal.Missiles;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.Sides;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Planet;
using WarGames.Startups;

namespace WarGames.Simulator.NUnit
{
	/// <summary>
	/// Demonstrates the original claim. Isolating your business layer allows you to quickly swap out UI.
	/// </summary>
	[Ignore("Long running. This simulator is not intended to be a TEST.")]
	[TestFixture]
	public class BasicSimulation
	{
		private CurrentGame currentGame;
		private Player playerCap;
		private Player playerCom;
		private IServiceProvider serviceProvider;
		#region Set Ups

		[OneTimeSetUp]
		public async Task OneTimeSetUp()
		{
			playerCap = new Player("Smith", Guid.NewGuid().ToString(), PlayerType.Human);
			playerCom = new Player("Marx", Guid.NewGuid().ToString(), PlayerType.Human);
			MockServices();

			currentGame = GetService<CurrentGame>();
			var gameManager = GetService<IGameManager>();
			var playerSideManager = GetService<IPlayerSideManager>();
			await playerSideManager.AddAsync(playerCap);
			await playerSideManager.AddAsync(playerCom);
			await playerSideManager.ChooseAsync(playerCap, new Capitalism());
			await playerSideManager.ChooseAsync(playerCom, new Communism());

			await gameManager.LoadWorldAsync();

			await gameManager.AssignCountriesAsync(CountryAssignment.Random);
		}

		private IOptionsMonitor<T> BuildMockOptionsMonitor<T>(T withMe)
			where T : class
		{
			return Mock.Of<IOptionsMonitor<T>>(_ => _.CurrentValue == withMe);
		}

		private T GetService<T>()
		{
			return serviceProvider.GetService<T>()
				?? throw new ArgumentNullException();
		}

		private IServiceCollection MockServices()
		{
			IServiceCollection services = new ServiceCollection();

			services.InitializeCoreGameServices();

			return services;
		}

		#endregion Set Ups

		[Test]
		public async Task Blow_Up_The_World()
		{
			var gameManager = GetService<IGameManager>();
			var playerSideManager = GetService<IPlayerSideManager>();
			var capSide = await playerSideManager.WhatIsPlayerAsync(playerCap);
			var comSide = await playerSideManager.WhatIsPlayerAsync(playerCom);

			await SeedArsenal(capSide);
			await SeedArsenal(comSide);
			await SetTargets(capSide);
			await SetTargets(comSide);

			await gameManager.SetTargetAssignmentsAsync();
			await gameManager.RainFireAsync();
		}

		private async Task SeedArsenal(Side side)
		{
			short payloadCount = 1;
			var missileDeliverySystemResource = GetService<IMissileDeliverySystemResource>();
			var settlementResource = GetService<ISettlementResource>();
			var settlements = await settlementResource.RetrieveManyAsync(currentGame.GameSession, side);
			foreach (var settlement in settlements)
				await missileDeliverySystemResource.AssignAsync(currentGame.GameSession, side, new Silo(payloadCount, new SRM()) { Coord = settlement.Coord });
		}

		private async Task SetTargets(Side side)
		{
			var gameManager = GetService<IGameManager>();
			var settlementResource = GetService<ISettlementResource>();
			var settlements = await settlementResource.RetrieveManyAsync(currentGame.GameSession, side);
			foreach (var settlement in settlements)
				await gameManager.AddTargetAsync(side, settlement, TargetPriority.Primary);
		}
	}
}