using Microsoft.Extensions.DependencyInjection;
using SimpleMap.Contracts;
using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.CLI.Views;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Competitors;
using WarGames.Resources.Game;

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

		private void CountryConfiguration(JsonFileConfiguration<Country, string> fillMe)
		{
			fillMe.RootPath = Path.Combine(rootDataDir, settlementsFile);
			fillMe.ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Country>>();
		}
		private void SettlementConfiguration(JsonFileConfiguration<Settlement, string> fillMe)
		{
			fillMe.RootPath = Path.Combine(rootDataDir, settlementsFile);
			fillMe.ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Settlement>>();
		}

		private IServiceCollection InitializeCoreGameServices(IServiceCollection services)
		{
			services.Configure<JsonFileConfiguration<Country, string>>(CountryConfiguration);
			services.Configure<JsonFileConfiguration<Settlement, string>>(SettlementConfiguration);
			services.AddSingleton<LocationResolver>();
			services.AddSingleton<WorldFactory>();
			services.AddSingleton<IReadResource<Country, string>, ReadonlyJsonFileResource<Country, string>>();
			services.AddSingleton<IReadResource<Settlement, string>, ReadonlyJsonFileResource<Settlement, string>>();
			services.AddSingleton<IRepository<ICompetitor, string>, InMemoryCompetitorRepository>();
			services.AddSingleton<IArsenalAssignmentEngine, ArsenalAssignmentEngine>();
			services.AddSingleton<ICountryAssignmentEngine, CountryAssignmentEngine>();
			services.AddSingleton<IGameManager, GameManager>();
			services.AddSingleton<ITargetResource, TargetResource>();
			services.AddSingleton<IDamageCalculator, DamageCalculator>();
			services.AddSingleton<ITargetingCalculator, TargetingCalculator>();
			services.AddSingleton<IRepository<World, Guid>, InMemoryWorldRepository>();

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
	}
}
