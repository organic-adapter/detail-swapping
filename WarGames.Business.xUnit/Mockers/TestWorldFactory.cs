using Map.Engine;
using WarGames.Contracts.Game;
using WarGames.Contracts.Game.TargetValues;

namespace WarGames.Business.xUnit.Mockers
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
			{
				var direction = country.Name.StartsWith("Communism") ? (int)CardinalLong.E : (int)CardinalLong.W;
				country.Settlements = MakeSettlements(country, direction);
			}
			return returnMe;
		}

		private static Settlement MakeSettlement(Country country, string settlementName, double lon)
		{
			var returnMe = new Settlement()
			{
				Id = Guid.NewGuid()
				,
				Name = $"{country.Name} {settlementName}",
				Location = new Location(country, new Coord(0, lon)),
			};

			returnMe.TargetValues.Add(MakeTargetValue<CivilianPopulation>(10000, 10000000));
			return returnMe;
		}

		private static List<Settlement> MakeSettlements(Country country, int direction)
		{
			var returnMe = new List<Settlement>
			{
				MakeSettlement(country, "Primary Target", direction * 4),
				MakeSettlement(country, "Secondary Target", direction *  8),
				MakeSettlement(country, "Tertiary Target", direction * 16),
				MakeSettlement(country, "Left Over Target", direction * 32),
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