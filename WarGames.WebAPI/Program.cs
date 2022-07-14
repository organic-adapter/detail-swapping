using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.World;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Games;
using WarGames.Resources.Planet;
using WarGames.Resources.Sides;
using WarGames.Startups;
using WarGames.Startups.AuthenticationSetups;
using WarGames.WebAPI.Models;

const string allowOrigins = "allowOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ThisIsNotSecureExampleOnly>(builder.Configuration.GetSection("MockSecurity"));

builder.Services
	.Configure<AzureB2CConfig>(builder.Configuration.GetSection("AzureAdB2C"))
	.Configure<JsonFileConfiguration<GameSession, string>>(builder.Configuration.GetSection("GameSessionConfiguration"))
	.InitializeGameServices()
	.InitializeBusinessServices()
	.InitializeConfigurations()
	.AddSingleton<ITargetResource, QADTargetResource>()
	.AddSingleton<ISideResource, QADSideResource>()
	.AddSingleton<IPlayerResource, FilePlayerResource>()
	.AddSingleton<ICountryResource, QADCountryResource>()
	.AddSingleton<ISettlementResource, QADSettlementResource>()
	.AddSingleton<IGameSessionResource, QADGameSessionResource>()
	.AddSingleton<IMissileDeliverySystemResource, QADMissileDeliverySystemResource>()
	.AddSingleton<IReadResource<Country, string>, ReadonlyJsonFileResource<Country, string>>()
	.AddSingleton<IReadResource<Settlement, string>, ReadonlyJsonFileResource<Settlement, string>>()
	.AddSingleton<IGameSessionResource, FileGameSessionResource>();

builder.Services.AddAutoMapper(typeof(PlanetMapperProfiles));

builder.Services.AddCors
	(
	options => 
		{
			options.AddPolicy
				(
					allowOrigins
					, policy =>
					{
						policy.WithMethods("DELETE", "GET", "POST", "PUT");
						policy.WithOrigins("http://localhost:8080");
					}
				);
		}
	);
builder.Services.AddControllers();

/*
 * There is this thing, called user secrets... yep... user secrets.
 *
 * Don't go adding passwords to source control.
 */
builder
	.Services
	.AddAzureB2C(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(allowOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();