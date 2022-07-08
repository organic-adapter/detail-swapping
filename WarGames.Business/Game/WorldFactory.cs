using WarGames.Contracts.Game;
using WarGames.Resources;

namespace WarGames.Business.Game
{
	[Obsolete("World class is obsoleting.")]
	public class WorldFactory
	{
		private readonly IReadResource<Country, string> countryResource;
		private readonly IReadResource<Settlement, string> settlementResource;
		private readonly World? defaultWorld;
		public WorldFactory(World defaultWorld)
		{
			this.defaultWorld = defaultWorld;
			countryResource = new InMemoryReadResource<Country, string>();
			settlementResource = new InMemoryReadResource<Settlement, string>();
		}
		public WorldFactory(IReadResource<Country, string> countryResource, IReadResource<Settlement, string> settlementResource)
		{
			this.countryResource = countryResource;
			this.settlementResource = settlementResource;
		}

		public async Task<World> BuildAsync()
		{
			var returnMe = defaultWorld ?? new World();
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