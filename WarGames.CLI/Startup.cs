using Microsoft.Extensions.DependencyInjection;
using SimpleMap.Contracts;
using WarGames.Business.Arsenal;
using WarGames.Business.Competitors;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.CLI.Views;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Contracts.Game.GameDefaults;
using WarGames.Contracts.V2.Games;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;
using WarGames.Resources.Game;
using WarGames.Resources.Sides;

namespace WarGames.CLI
{
	public class Startup
	{
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

		private void InitializeServices(IServiceCollection services)
		{
			services.AddAutoMapper(typeof(WorldMapperProfiles));

			services.InitializeCoreGameServices()
				.AddScoped<MenuController>()
				.AddScoped<ConsoleView, LogonView>()
				.AddScoped<ConsoleView, MainView>()
				.AddScoped<ConsoleView, GTWView>();
		}
	}

	internal static class WarGamesServiceExtensions
	{
		private const string settlementsFile = "settlements.copyright.json";
		private readonly static string rootDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

		public static IServiceCollection InitializeCoreGameServices(this IServiceCollection services)
		{
			services
				.InitializeConfigurations()
				.InitializeDataServices()
				.InitializeBusinessServices();

			services.AddSingleton<CurrentGame>();
			services.AddSingleton<LocationResolver>();
			services.AddSingleton<WorldFactory>();

			services.AddSingleton<ICompetitor, Capitalism>();
			services.AddSingleton<ICompetitor, Communism>();

			services.AddSingleton<IGameDefaults, SinglePlayerDefaults>();
			services.AddSingleton<IGameDefaults, TwoPlayerDefaults>();
			services.AddSingleton<IGameDefaults, AutoPlayDefaults>();


			return services;
		}
		public static IServiceCollection InitializeConfigurations(this IServiceCollection services)
		{
			services.Configure<JsonFileConfiguration<Country, string>>(CountryConfiguration);
			services.Configure<JsonFileConfiguration<Settlement, string>>(SettlementConfiguration);
			
			return services;
		}
		public static IServiceCollection InitializeBusinessServices(this IServiceCollection services)
		{
			services.AddSingleton<IArsenalAssignmentEngine, ArsenalAssignmentEngine>();
			services.AddSingleton<ICountryAssignmentEngine, CountryAssignmentEngine>();

			services.AddSingleton<IGameManager, GameManager>();
			services.AddSingleton<IPlayerSideManager, PlayerSideManager>();

			services.AddSingleton<IDamageCalculator, DamageCalculator>();
			services.AddSingleton<ITargetingCalculator, TargetingCalculator>();

			return services;
		}

		public static IServiceCollection InitializeDataServices(this IServiceCollection services)
		{
			services.AddSingleton<ITargetResource, TargetResource>();
			services.AddSingleton<ISideResource, QuickAndDirtySideResource>();
			services.AddSingleton<IPlayerResource, QuickAndDirtyPlayerResource>();
			services.AddSingleton<IReadResource<Country, string>, ReadonlyJsonFileResource<Country, string>>();
			services.AddSingleton<IReadResource<Settlement, string>, ReadonlyJsonFileResource<Settlement, string>>();
			services.AddSingleton<IRepository<ICompetitor, string>, InMemoryCompetitorRepository>();
			services.AddSingleton<ICompetitorResource, CompetitorResource>();
			services.AddSingleton<IRepository<World, Guid>, InMemoryWorldRepository>();

			return services;
		}

		private static void CountryConfiguration(JsonFileConfiguration<Country, string> fillMe)
		{
			fillMe.RootPath = Path.Combine(rootDataDir, settlementsFile);
			fillMe.ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Country>>();
		}
		private static void SettlementConfiguration(JsonFileConfiguration<Settlement, string> fillMe)
		{
			fillMe.RootPath = Path.Combine(rootDataDir, settlementsFile);
			fillMe.ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Settlement>>();
		}
	}
}
