using WarGames.Contracts.V2;
using WarGames.Contracts.V2.World;
using WarGames.Resources;
using WarGames.Resources.Planet;
using static WarGames.Contracts.V2.GameSession;

namespace WarGames.Business.Planet
{
	public class ArbitraryWorldBuildingEngine : IWorldBuildingEngine
	{
		private readonly Dictionary<SessionPhase, Func<GameSession, Task>> buildMaps;
		private readonly IReadResource<Country, string> countryReadResource;
		private readonly ICountryResource countryResource;
		private readonly IReadResource<Settlement, string> settlementReadResource;
		private readonly ISettlementResource settlementResource;

		public ArbitraryWorldBuildingEngine(
			ICountryResource countryResource
			, ISettlementResource settlementResource
			, IReadResource<Country, string> countryReadResource
			, IReadResource<Settlement, string> settlementReadResource
			)
		{
			this.countryResource = countryResource;
			this.settlementResource = settlementResource;
			this.countryReadResource = countryReadResource;
			this.settlementReadResource = settlementReadResource;

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

		private async Task BuildNewAsync(GameSession gameSession)
		{

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

		public class AttemptedLoadFinishedGameException : Exception
		{ }

		public class UnknownSessionPhaseException : Exception
		{ }
	}
}