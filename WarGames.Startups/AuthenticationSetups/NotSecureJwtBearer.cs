using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WarGames.Startups.AuthenticationSetups
{
	public static class NotSecureJwtBearer
	{
		public static IServiceCollection AddJwtBearer(this IServiceCollection services, ConfigurationManager configuration)
		{
			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidAudience = configuration["MockSecurity:Jwt:Audience"],
						ValidIssuer = configuration["MockSecurity:Jwt:Issuer"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["MockSecurity:Jwt:Key"]))
					};
				});
			return services;
		}
	}
}