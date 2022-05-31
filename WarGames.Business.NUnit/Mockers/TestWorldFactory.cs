using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

			returnMe.Add(new Country() { Name = "Capitalism Capital" });
			returnMe.Add(new Country() { Name = "Capitalism Military Center" });
			returnMe.Add(new Country() { Name = "Capitalism Financial Center" });
			returnMe.Add(new Country() { Name = "Capitalism Small City" });
			returnMe.Add(new Country() { Name = "Capitalism Small Town 1" });
			returnMe.Add(new Country() { Name = "Capitalism Small Town 2" });
			returnMe.Add(new Country() { Name = "Capitalism Small Town 3" });

			returnMe.Add(new Country() { Name = "Communism Capital" });
			returnMe.Add(new Country() { Name = "Communism Military Center" });
			returnMe.Add(new Country() { Name = "Communism Financial Center" });
			returnMe.Add(new Country() { Name = "Communism Small City" });
			returnMe.Add(new Country() { Name = "Communism Small Town 1" });
			returnMe.Add(new Country() { Name = "Communism Small Town 2" });
			returnMe.Add(new Country() { Name = "Communism Small Town 3" });

			return returnMe;
		}
	}
}
