using Map.Engine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarGames.Business.Game;
using WarGames.Business.Planet;
using WarGames.Contracts.Game;
using WarGames.Contracts.Game.TargetValues;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.World;
using WarGames.Resources.Planet;
using static WarGames.Contracts.V2.GameSession;

namespace WarGames.Business.NUnit.Mockers
{
	internal class TestWorldBuildingEngine : IWorldBuildingEngine
	{
		private static readonly Random rando = new Random();
		private readonly Dictionary<SessionPhase, Func<GameSession, Task>> buildMaps;
		private readonly ICountryResource countryResource;
		private readonly CurrentGame currentGame;
		private readonly ISettlementResource settlementResource;

		public TestWorldBuildingEngine(
			CurrentGame currentGame
			, ICountryResource countryResource
			, ISettlementResource settlementResource
			)
		{
			this.currentGame = currentGame;
			this.countryResource = countryResource;
			this.settlementResource = settlementResource;

			buildMaps = new()
			{
				{ SessionPhase.Unknown, CannotBuildAnUnknownGame },
				{ SessionPhase.New, BuildNewAsync },
				{ SessionPhase.Started, LoadExistingAsync },
				{ SessionPhase.Finished, CannotLoadAFinishedGame }
			};
		}

		public async Task BuildAsync(GameSession gameSession)
		{
			await buildMaps[gameSession.Phase](gameSession);
		}

		private static Settlement MakeSettlement(Country country, string settlementName, double lon)
		{
			var returnMe = new Settlement()
			{
				Id = Guid.NewGuid().ToString()
				,
				Name = $"{country.Name} {settlementName}",
				Coord = new Coord(0, lon),
			};
			country.SettlementIds.Add(returnMe.Id);
			returnMe.TargetValues.Add(MakeTargetValue<CivilianPopulation>(10000, 10000000));
			return returnMe;
		}

		private static T MakeTargetValue<T>(int minValue, int maxValue)
			where T : TargetValue
		{
			var value = rando.Next(minValue, maxValue);
			return Activator.CreateInstance(typeof(T), value) as T ?? (T)new TargetValue(typeof(T).Name, value);
		}

		private async Task BuildNewAsync(GameSession gameSession)
		{
			await MakeCountriesAsync();
		}

		private async Task CannotBuildAnUnknownGame(GameSession gameSession)
		{
			throw new UnknownSessionPhaseException();
		}

		private async Task CannotLoadAFinishedGame(GameSession gameSession)
		{
			throw new AttemptedLoadFinishedGameException();
		}

		private async Task LoadExistingAsync(GameSession gameSession)
		{
		}

		private async Task MakeCountriesAsync()
		{
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "NATO Capital" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "NATO Military Center" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "NATO Financial Center" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "NATO Small City" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "NATO Small Towns 1" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "NATO Small Towns 2" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "NATO Small Towns 3" });

			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "USSR Capital" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "USSR Military Center" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "USSR Financial Center" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "USSR Small City" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "USSR Small Towns 1" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "USSR Small Towns 2" });
			await countryResource.SaveAsync(currentGame.GameSession, new Country() { Id = Guid.NewGuid().ToString(), Name = "USSR Small Towns 3" });

			var countries = await countryResource.RetrieveManyAsync(currentGame.GameSession);
			foreach (var country in countries)
			{
				var direction = country.Name.StartsWith("Communism") ? (int)CardinalLong.E : (int)CardinalLong.W;
				await MakeSettlements(country, direction);
			}
		}

		private async Task MakeSettlements(Country country, int direction)
		{
			var settlement1 = MakeSettlement(country, "Primary Target", direction * 4);
			await settlementResource.SaveAsync(currentGame.GameSession, settlement1);
			await settlementResource.AssignAsync(currentGame.GameSession, country, settlement1);

			var settlement2 = MakeSettlement(country, "Secondary Target", direction * 8);
			await settlementResource.SaveAsync(currentGame.GameSession, settlement2);
			await settlementResource.AssignAsync(currentGame.GameSession, country, settlement2);


			var settlement3 = MakeSettlement(country, "Tertiary Target", direction * 16);
			await settlementResource.SaveAsync(currentGame.GameSession, settlement3);
			await settlementResource.AssignAsync(currentGame.GameSession, country, settlement3);

			var settlement4 = MakeSettlement(country, "Left Over Target", direction * 32);
			await settlementResource.SaveAsync(currentGame.GameSession, settlement4);
			await settlementResource.AssignAsync(currentGame.GameSession, country, settlement4);
		}

		public class AttemptedLoadFinishedGameException : Exception
		{ }

		public class UnknownSessionPhaseException : Exception
		{ }
	}
}