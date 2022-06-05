using System;
using System.Collections.Generic;
using WarGames.Contracts.Game;
using WarGames.Contracts.Game.TargetValues;

namespace WarGames.Business.NUnit.Mockers
{
	public static class TestWorldFactory
	{
		private static readonly Random rando = new Random();

		public static World Make()
		{
			return new World() { Countries = MakeCountries() };
		}

		private static List<Country> MakeCountries()
		{
			var returnMe = new List<Country>();

			returnMe.Add(new Country() { Name = "NATO Capital" });
			returnMe.Add(new Country() { Name = "NATO Military Center" });
			returnMe.Add(new Country() { Name = "NATO Financial Center" });
			returnMe.Add(new Country() { Name = "NATO Small City" });
			returnMe.Add(new Country() { Name = "NATO Small Towns 1" });
			returnMe.Add(new Country() { Name = "NATO Small Towns 2" });
			returnMe.Add(new Country() { Name = "NATO Small Towns 3" });

			returnMe.Add(new Country() { Name = "Communism Capital" });
			returnMe.Add(new Country() { Name = "Communism Military Center" });
			returnMe.Add(new Country() { Name = "Communism Financial Center" });
			returnMe.Add(new Country() { Name = "Communism Small City" });
			returnMe.Add(new Country() { Name = "Communism Small Towns 1" });
			returnMe.Add(new Country() { Name = "Communism Small Towns 2" });
			returnMe.Add(new Country() { Name = "Communism Small Towns 3" });

			foreach (var country in returnMe)
				country.Settlements = MakeSettlements(country.Name);

			return returnMe;
		}

		private static Settlement MakeSettlement(string countryName, string settlementName)
		{
			var returnMe = new Settlement()
			{
				Id = Guid.NewGuid()
				,
				Name = $"{countryName} {settlementName}"
			};

			returnMe.TargetValues.Add(MakeTargetValue<CivilianPopulation>(10000, 10000000));
			return returnMe;
		}

		private static List<Settlement> MakeSettlements(string countryName)
		{
			var returnMe = new List<Settlement>
			{
				MakeSettlement(countryName, "Primary Target"),
				MakeSettlement(countryName, "Secondary Target"),
				MakeSettlement(countryName, "Tertiary Target"),
				MakeSettlement(countryName, "Left Over Target"),
			};

			return returnMe;
		}

		private static T MakeTargetValue<T>(int minValue, int maxValue)
			where T : TargetValue
		{
			var value = rando.Next(minValue, maxValue);
			return Activator.CreateInstance(typeof(T), value) as T ?? (T)new TargetValue(typeof(T).Name, value);
		}
	}
}