using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using SimpleMap.Contracts;
using WarGames.Business.Arsenal;
using WarGames.Business.Arsenal.MissileDeliverySystems;
using WarGames.Business.Arsenal.Missiles;
using WarGames.Business.Competitors;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;
using WarGames.Resources.Game;

namespace WarGames.Simulator.NUnit
{
	[TestFixture]
	public class BasicSimulation
	{
		private const string settlementsFile = "settlements.copyright.json";
		private readonly string rootDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
		private IGameManager gameManager;
		private Player playerCap;
		private Player playerCom;

		#region Set Ups

		[OneTimeSetUp]
		public async Task OneTimeSetUp()
		{
			playerCap = new Player("Smith", Guid.NewGuid().ToString());
			playerCom = new Player("Marx", Guid.NewGuid().ToString());
			var services = MockServices();
			var provider = services.BuildServiceProvider();

#pragma warning disable CS8601 // Possible null reference assignment.

			gameManager = provider.GetService<IGameManager>();
			await gameManager.LoadPlayerAsync(playerCap, new Capitalism());
			await gameManager.LoadPlayerAsync(playerCom, new Communism());

			await gameManager.LoadWorldAsync();

			await gameManager.AssignCountriesAsync(CountryAssignment.Random);

#pragma warning restore CS8601 // Possible null reference assignment.
		}

		private IOptionsMonitor<T> BuildMockOptionsMonitor<T>(T withMe)
			where T : class
		{
			return Mock.Of<IOptionsMonitor<T>>(_ => _.CurrentValue == withMe);
		}

		private IServiceCollection MockServices()
		{
			IServiceCollection services = new ServiceCollection();
			var mockCountryConfig = BuildMockOptionsMonitor
										(
											new JsonFileConfiguration<Country, string>()
											{
												RootPath = Path.Combine(rootDataDir, settlementsFile)
												,
												ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Country>>()
											}
										);
			var mockSettlementConfig = BuildMockOptionsMonitor
							(
								new JsonFileConfiguration<Settlement, string>()
								{
									RootPath = Path.Combine(rootDataDir, settlementsFile)
									,
									ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Settlement>>()
								}
							);
			services.AddSingleton(mockCountryConfig);
			services.AddSingleton(mockSettlementConfig);
			services.AddSingleton<LocationResolver>();
			services.AddSingleton<WorldFactory>();
			services.AddSingleton<IReadResource<Country, string>, ReadonlyJsonFileResource<Country, string>>();
			services.AddSingleton<IReadResource<Settlement, string>, ReadonlyJsonFileResource<Settlement, string>>();
			services.AddSingleton<IRepository<ICompetitor, string>, InMemoryCompetitorRepository>();
			services.AddSingleton<IArsenalAssignmentEngine, ArsenalAssignmentEngine>();
			services.AddSingleton<ICountryAssignmentEngine, CountryAssignmentEngine>();
			services.AddSingleton<IGameManager, GameManager>();
			services.AddSingleton<ITargetResource, TargetResource>();
			services.AddSingleton<ITargetingEngine, TargetingEngine>();
			services.AddSingleton<IRepository<World, Guid>, InMemoryWorldRepository>();

			services.AddAutoMapper(typeof(WorldMapperProfiles));

			return services;
		}

		#endregion Set Ups

		[Test]
		public async Task Blow_Up_The_World()
		{
			var cap = await gameManager.WhatIsPlayerAsync(playerCap);
			var com = await gameManager.WhatIsPlayerAsync(playerCom);

			SeedArsenal(cap);
			SeedArsenal(com);
			SetTargets(cap);
			SetTargets(com);

			await gameManager.SetTargetAssignmentsAsync();
			await gameManager.RainFireAsync();
		}

		private void SeedArsenal(ICompetitor competitor)
		{
			foreach (var settlement in competitor.Settlements)
				competitor.MissileDeliverySystems.Add(new Silo(0, 1, new SRM()) { CurrentArea = settlement.Location.Area, Location = settlement.Location });
		}

		private void SetTargets(ICompetitor competitor)
		{
			foreach (var settlement in competitor.Settlements)
				gameManager.AddTargetAsync(settlement, TargetPriority.Primary);
		}
	}
}