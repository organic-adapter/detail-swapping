using SimpleMap.Contracts;
using WarGames.Contracts.Game;
using WarGames.Contracts.V2.World;
using WarGames.Resources;

namespace WarGames.WebAPI
{
	internal class Configurations
	{
		private const string settlementsFile = "settlements.copyright.json";
		private static readonly string rootDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

		internal static void CountryConfiguration(JsonFileConfiguration<Country, string> fillMe)
		{
			fillMe.RootPath = Path.Combine(rootDataDir, settlementsFile);
			fillMe.ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Country>>();
		}

		internal static void SettlementConfiguration(JsonFileConfiguration<Settlement, string> fillMe)
		{
			fillMe.RootPath = Path.Combine(rootDataDir, settlementsFile);
			fillMe.ConversionRequired = new ConversionRequired<List<SimpleMapEntry>, List<Settlement>>();
		}
	}
}