using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WarGames.Business.Arsenal;
using WarGames.Business.Game;
using WarGames.Business.Game.GameDefaults;
using WarGames.Business.Managers;
using WarGames.Contracts.V2.Games;
using WarGames.Contracts.V2.World;
using WarGames.Resources;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Planet;
using WarGames.WebAPI;
using WarGames.WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<JsonFileConfiguration<Country, string>>(Configurations.CountryConfiguration); //TODO: Demonstrate the appsettings.json version of this.
builder.Services.Configure<JsonFileConfiguration<Settlement, string>>(Configurations.SettlementConfiguration); //TODO: Demonstrate the appsettings.json version of this.
builder.Services.Configure<ThisIsNotSecureExampleOnly>(builder.Configuration.GetSection("MockSecurity"));
builder.Services.AddSingleton<IReadResource<Country, string>, ReadonlyJsonFileResource<Country, string>>();
builder.Services.AddSingleton<IReadResource<Settlement, string>, ReadonlyJsonFileResource<Settlement, string>>();
builder.Services.AddSingleton<IArsenalAssignmentEngine, ArsenalAssignmentEngine>();
builder.Services.AddSingleton<ICountryAssignmentEngine, CountryAssignmentEngine>();
builder.Services.AddSingleton<IGameManager, GameManager>();
builder.Services.AddSingleton<ITargetResource, QADTargetResource>();
builder.Services.AddSingleton<IDamageCalculator, DamageCalculator>();
builder.Services.AddSingleton<ITargetingCalculator, TargetingCalculator>();
builder.Services.AddSingleton<IGameDefaults, SinglePlayerDefaults>();
builder.Services.AddSingleton<IGameDefaults, TwoPlayerDefaults>();
builder.Services.AddSingleton<IGameDefaults, AutoPlayDefaults>();

builder.Services.AddAutoMapper(typeof(PlanetMapperProfiles));

builder.Services.AddControllers();

/*
 * There is this thing, called user secrets... yep... user secrets.
 *
 * Don't go adding passwords to source control.
 */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidAudience = builder.Configuration["MockSecurity:Jwt:Audience"],
		ValidIssuer = builder.Configuration["MockSecurity:Jwt:Issuer"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["MockSecurity:Jwt:Key"]))
	};
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();