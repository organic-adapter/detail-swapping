using Microsoft.Extensions.DependencyInjection;
using WarGames.Business.Planet;
using WarGames.CLI.Views;
using WarGames.Resources.Planet;
using WarGames.Startups;

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
			services.AddAutoMapper(typeof(PlanetMapperProfiles));

			services.InitializeCoreGameServices()
				.AddScoped<MenuController>()
				.AddScoped<ConsoleView, LogonView>()
				.AddScoped<ConsoleView, MainView>()
				.AddScoped<ConsoleView, GTWView>();
		}
	}
}