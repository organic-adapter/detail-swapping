using WarGames.Contracts.Game;
using WarGames.Resources;

namespace WarGames.Business.Game
{
	public class WorldFactory
	{
		private readonly IReadResource<Country, string> countryResource;
		private readonly IReadResource<Settlement, string> settlementResource;

		public WorldFactory(IReadResource<Country, string> countryResource, IReadResource<Settlement, string> settlementResource)
		{
			this.countryResource = countryResource;
			this.settlementResource = settlementResource;
		}

		public async Task<World> BuildAsync()
		{
			var returnMe = new World();
			var options = new ParallelOptions { MaxDegreeOfParallelism = 8 };
			await Parallel.ForEachAsync
				(
					await countryResource.GetAllAsync(),
					options,
					async (target, token) =>
					{
						await Task.Run(() =>
						{
							var countrySettlements = settlementResource.GetAll().Where(sr=>sr.Location.Area.Equals(target));
							target.Settlements.AddRange(countrySettlements);
							returnMe.Countries.Add(target);
						});
					}
				);

			return returnMe;
		}
	}
}