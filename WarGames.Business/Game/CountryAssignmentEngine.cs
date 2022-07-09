using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;
using WarGames.Resources.Planet;
using WarGames.Resources.Sides;

namespace WarGames.Business.Game
{
	public class CountryAssignmentEngine : ICountryAssignmentEngine
	{
		private readonly Dictionary<CountryAssignment, Func<GameSession, Task>> assignmentDelegates;
		private readonly ICountryResource countryResource;
		private readonly ISettlementResource settlementResource;
		private readonly ISideResource sideResource;

		public CountryAssignmentEngine
				(
					ICountryResource countryResource
					, ISettlementResource settlementResource
					, ISideResource sideResource
				)
		{
			this.countryResource = countryResource;
			this.settlementResource = settlementResource;
			this.sideResource = sideResource;

			assignmentDelegates = new()
			{
				{ CountryAssignment.Random, CountryAssignmentRandom },
				{ CountryAssignment.ByName, CountryAssignmentByName }
			};
		}

		public async Task AssignCountriesAsync(GameSession gameSession, CountryAssignment assignmentType)
		{
			await Task.Run(() => assignmentDelegates[assignmentType](gameSession));
		}

		private async Task AssignCountry(GameSession gameSession, Side side, Country country)
		{
			await countryResource.AssignAsync(gameSession, side, country);
			side.Countries.Add(country.Id);
			foreach(var settlementId in country.SettlementIds)
			{
				var settlement = await settlementResource.RetrieveAsync(gameSession, settlementId);
				await settlementResource.AssignAsync(gameSession, side, settlement);
			}
		}

		private async Task CountryAssignmentByName(GameSession gameSession)
		{
			var sides = await sideResource.RetrieveManyAsync(gameSession);
			var countries = await countryResource.RetrieveManyAsync(gameSession);

			foreach (var side in sides)
			{
				var countriesByName = FilterCountriesBySideName(countries, side);
				foreach (var country in countriesByName)
					await AssignCountry(gameSession, side, country);
			}
		}

		private async Task CountryAssignmentRandom(GameSession gameSession)
		{
			var assignmentQueue = await RandomizeAssignmentsQueue(gameSession);
			var sides = await sideResource.RetrieveManyAsync(gameSession);
			var sideToggles = GenerateSideToggles(sides);

			byte nextToggle = (byte)(assignmentQueue.Count % sideToggles.Count);
			while (assignmentQueue.Any())
			{
				var nextSide = sideToggles[nextToggle];
				var nextCountry = assignmentQueue.Dequeue();

				await AssignCountry(gameSession, nextSide, nextCountry);
				nextToggle = (byte)(assignmentQueue.Count % sideToggles.Count);
			}
		}

		private IEnumerable<Country> FilterCountriesBySideName(IEnumerable<Country> countries, Side side)
		{
			return countries
				.Where(country =>
							country.Name.Contains(side.DisplayName)
							|| country.Name.Contains(side.Id, StringComparison.OrdinalIgnoreCase)
						);
		}

		private Dictionary<byte, Side> GenerateSideToggles(IEnumerable<Side> sides)
		{
			var returnMe = new Dictionary<byte, Side>();
			byte nextIndex = 0;
			foreach (var side in sides)
			{
				returnMe.Add(nextIndex, side);
				nextIndex++;
			}
			return returnMe;
		}

		private async Task<Queue<Country>> RandomizeAssignmentsQueue(GameSession gameSession)
		{
			var assignmentQueue = new Queue<Country>();
			var countries = await countryResource.RetrieveManyAsync(gameSession);
			foreach (var country in countries.OrderBy(w => Guid.NewGuid()))
				assignmentQueue.Enqueue(country);

			return assignmentQueue;
		}
	}
}