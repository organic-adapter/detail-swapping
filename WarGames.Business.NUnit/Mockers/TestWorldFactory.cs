using System;
using System.Collections.Generic;
using WarGames.Contracts.Game;

namespace WarGames.Business.NUnit.Mockers
{
	public static class TestWorldFactory
	{
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

		private static List<Settlement> MakeSettlements(string countryName)
		{
			var returnMe = new List<Settlement>();

			returnMe.Add(new Settlement() { Id = Guid.NewGuid(), Name = $"{countryName} Main Target" });
			returnMe.Add(new Settlement() { Id = Guid.NewGuid(), Name = $"{countryName} Secondary Target" });
			returnMe.Add(new Settlement() { Id = Guid.NewGuid(), Name = $"{countryName} Tertiary Target" });
			returnMe.Add(new Settlement() { Id = Guid.NewGuid(), Name = $"{countryName} Left Over Target" });
			returnMe.Add(new Settlement() { Id = Guid.NewGuid(), Name = $"{countryName} Left Over Target" });

			return returnMe;
		}
	}
}