using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGames.Startups.AuthenticationSetups
{
	public static class AzureBusinessToClient
	{
		public static IServiceCollection AddAzureB2C(this IServiceCollection services, ConfigurationManager configuration)
		{
			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddMicrosoftIdentityWebApi(options => 
				{
					configuration.Bind("AzureAdB2C", options);
					options.TokenValidationParameters.NameClaimType = "name";
				},
				
				options => { configuration.Bind("AzureAdB2C", options); });
			return services;
		}
	}
}
