using Microsoft.Extensions.DependencyInjection;
using SimpleMap.Contracts;
using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Business.Game.GameDefaults;
using WarGames.Business.Managers;
using WarGames.Business.Sides;
using WarGames.Contracts.V2.Games;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Planet;
using WarGames.Resources.Sides;

namespace WarGames.Startups
{
	public static class DefaultStartupServiceExtensions
	{
		private const string settlementsFile = "settlements.copyright.json";
		private static readonly string rootDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

		public static IServiceCollection InitializeBusinessServices(this IServiceCollection services)
		{
			services.AddSingleton<IArsenalAssignmentEngine, ArsenalAssignmentEngine>();
			services.AddSingleton<ICountryAssignmentEngine, CountryAssignmentEngine>();

			services.AddSingleton<IGameManager, GameManager>();
			services.AddSingleton<IPlayerSideManager, PlayerSideManager>();
			services.AddSingleton<IWorldManager, WorldManager>();

			services.AddSingleton<IDamageCalculator, DamageCalculator>();
			services.AddSingleton<ITargetingCalculator, TargetingCalculator>();

			return services;
		}

		public static IServiceCollection InitializeConfigurations(this IServiceCollection services)
		{
			services.Configure<JsonFileConfiguration<Country, string>>(CountryConfiguration);
			services.Configure<JsonFileConfiguration<Settlement, string>>(SettlementConfiguration);

			return services;
		}

		public static IServiceCollection InitializeCoreGameServices(this IServiceCollection services)
		{
			services
				.InitializeConfigurations()
				.InitializeDataServices()
				.InitializeBusinessServices();

			services.AddSingleton<CurrentGame>();

			services.AddSingleton<Side, Capitalism>();
			services.AddSingleton<Side, Communism>();

			services.AddSingleton<IGameDefaults, SinglePlayerDefaults>();
			services.AddSingleton<IGameDefaults, TwoPlayerDefaults>();
			services.AddSingleton<IGameDefaults, AutoPlayDefaults>();

			return services;
		}

		public static IServiceCollection InitializeDataServices(this IServiceCollection services)
		{
			services.AddSingleton<ITargetResource, QADTargetResource>();
			services.AddSingleton<ISideResource, QADSideResource>();
			services.AddSingleton<IPlayerResource, QADPlayerResource>();
			services.AddSingleton<ICountryResource, QADCountryResource>();
			services.AddSingleton<ISettlementResource, QADSettlementResource>();
			services.AddSingleton<IMissileDeliverySystemResource, QADMissileDeliverySystemResource>();
			services.AddSingleton<IReadResource<Country, string>, ReadonlyJsonFileResource<Country, string>>();
			services.AddSingleton<IReadResource<Settlement, string>, ReadonlyJsonFileResource<Settlement, string>>();

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

	public class ServiceNotFoundException : Exception
	{
	}
}