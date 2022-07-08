using Microsoft.Extensions.DependencyInjection;
using SimpleMap.Contracts;
using WarGames.Business.Arsenal;
using WarGames.Business.Competitors;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.Planet;
using WarGames.CLI.Views;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Contracts.Game.GameDefaults;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;
using WarGames.Resources.Game;
using WarGames.Resources.Planet;
using WarGames.Resources.Sides;

namespace WarGames.CLI
{
	public class Startup
	{
		private const string settlementsFile = "settlements.copyright.json";
		private readonly string rootDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
		private IServiceCollection services;

		public Startup()
		{
			services = new ServiceCollection();
			InitializeServices(services);
		}

		public ServiceProvider Start()
		{
			return services.BuildServiceProvider();
		}

		private void CountryConfiguration(JsonFileConfiguration<Contracts.V2.World.Country, string> fillMe)
		{
			fillMe.RootPath = Path.Combine(rootDataDir, settlementsFile);
			fillMe.ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Contracts.V2.World.Country>>();
		}

		private void CountryConfiguration(JsonFileConfiguration<Country, string> fillMe)
		{
			fillMe.RootPath = Path.Combine(rootDataDir, settlementsFile);
			fillMe.ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Country>>();
		}

		private IServiceCollection InitializeCoreGameServices(IServiceCollection services)
		{
			services.Configure<JsonFileConfiguration<Contracts.V2.World.Country, string>>(CountryConfiguration);
			services.Configure<JsonFileConfiguration<Contracts.V2.World.Settlement, string>>(SettlementConfiguration);
			services.Configure<JsonFileConfiguration<Country, string>>(CountryConfiguration);
			services.Configure<JsonFileConfiguration<Settlement, string>>(SettlementConfiguration);

			services.AddSingleton<LocationResolver>();
			services.AddSingleton<WorldFactory>();
			services.AddSingleton<IWorldBuildingEngine, ArbitraryWorldBuildingEngine>();
			services.AddSingleton<CurrentGame>();
			services.AddSingleton<IReadResource<Contracts.V2.World.Country, string>, ReadonlyJsonFileResource<Contracts.V2.World.Country, string>>();
			services.AddSingleton<IReadResource<Contracts.V2.World.Settlement, string>, ReadonlyJsonFileResource<Contracts.V2.World.Settlement, string>>();
			services.AddSingleton<IReadResource<Country, string>, ReadonlyJsonFileResource<Country, string>>();
			services.AddSingleton<IReadResource<Settlement, string>, ReadonlyJsonFileResource<Settlement, string>>();

			services.AddSingleton<ICountryResource, QuickAndDirtyCountryResource>();
			services.AddSingleton<IPlayerResource, QuickAndDirtyPlayerResource>();
			services.AddSingleton<ISettlementResource, QuickAndDirtySettlementResource>();
			services.AddSingleton<ISideResource, QuickAndDirtySideResource>();

			services.AddSingleton<IRepository<ICompetitor, string>, InMemoryCompetitorRepository>();
			services.AddSingleton<IArsenalAssignmentEngine, ArsenalAssignmentEngine>();
			services.AddSingleton<ICountryAssignmentEngine, ArbitraryCountryAssignmentEngine>();

			services.AddSingleton<IPlayerSideManager, PlayerSideManager>();
			services.AddSingleton<IGameManager, GameMarkTwoManager>();
			services.AddSingleton<ITargetResource, TargetResource>();
			services.AddSingleton<IDamageCalculator, DamageCalculator>();
			services.AddSingleton<ITargetingCalculator, TargetingCalculator>();
			services.AddSingleton<IRepository<World, Guid>, InMemoryWorldRepository>();
			services.AddSingleton<ICompetitorResource, CompetitorResource>();
			services.AddSingleton<ICompetitor, Capitalism>();
			services.AddSingleton<ICompetitor, Communism>();
			services.AddSingleton<IGameDefaults, SinglePlayerDefaults>();
			services.AddSingleton<IGameDefaults, TwoPlayerDefaults>();
			services.AddSingleton<IGameDefaults, AutoPlayDefaults>();

			services.AddAutoMapper(typeof(WorldMapperProfiles));

			return services;
		}

		private void InitializeServices(IServiceCollection services)
		{
			InitializeCoreGameServices(services);
			services.AddScoped<MenuController>();
			services.AddScoped<ConsoleView, LogonView>();
			services.AddScoped<ConsoleView, MainView>();
			services.AddScoped<ConsoleView, GTWView>();
		}

		private void SettlementConfiguration(JsonFileConfiguration<Contracts.V2.World.Settlement, string> fillMe)
		{
			fillMe.RootPath = Path.Combine(rootDataDir, settlementsFile);
			fillMe.ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Contracts.V2.World.Settlement>>();
		}

		private void SettlementConfiguration(JsonFileConfiguration<Settlement, string> fillMe)
		{
			fillMe.RootPath = Path.Combine(rootDataDir, settlementsFile);
			fillMe.ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Settlement>>();
		}
	}
}