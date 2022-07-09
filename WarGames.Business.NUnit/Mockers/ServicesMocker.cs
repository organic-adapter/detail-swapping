using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using WarGames.Resources.Planet;
using WarGames.Startups;

namespace WarGames.Business.NUnit.Mockers
{
	public static class ServicesMockerExtensions
	{
		public static ServicesMocker AddSingleton<TInterface, TImplementation>(this ServicesMocker mocker)
			where TInterface : class
			where TImplementation : class, TInterface
		{
			var serviceDescriptor = mocker.Services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TInterface));
			if (serviceDescriptor != null)
				mocker.Services.Remove(serviceDescriptor);

			mocker.Services.AddSingleton<TInterface, TImplementation>();
			return mocker;
		}
		public static ServicesMocker AddSingleton<TInterface, TImplementation>(this ServicesMocker mocker, TImplementation concrete)
			where TInterface : class
			where TImplementation : class, TInterface
		{
			var serviceDescriptor = mocker.Services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TInterface));
			if (serviceDescriptor != null)
				mocker.Services.Remove(serviceDescriptor);

			mocker.Services.AddSingleton<TInterface>(concrete);
			return mocker;
		}
		public static ServicesMocker MockSingleton<TInterface>(this ServicesMocker mocker)
					where TInterface : class
		{
			var serviceDescriptor = mocker.Services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TInterface));
			if (serviceDescriptor != null)
				mocker.Services.Remove(serviceDescriptor);

			mocker.Services.AddSingleton(Mock.Of<TInterface>());
			return mocker;
		}
	}

	public class ServicesMocker
	{
		private readonly IServiceCollection services;

		public ServicesMocker()
		{
			services = new ServiceCollection();
			InitializeDefaultServices(services);
		}

		public static ServicesMocker DefaultMocker => new ServicesMocker();
		public IServiceCollection Services => services;

		public IServiceProvider Build()
		{
			return services.BuildServiceProvider();
		}

		private static void InitializeDefaultServices(IServiceCollection services)
		{
			services.AddAutoMapper(typeof(PlanetMapperProfiles));
			services.InitializeCoreGameServices();
		}
	}
}