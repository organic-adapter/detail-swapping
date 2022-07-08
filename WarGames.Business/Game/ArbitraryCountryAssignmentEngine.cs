using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Contracts.V2.Sides;
using WarGames.Resources.Game;
using WarGames.Resources.Planet;
using WarGames.Resources.Sides;

namespace WarGames.Business.Game
{
	public class ArbitraryCountryAssignmentEngine : ICountryAssignmentEngine
	{
		private readonly Dictionary<CountryAssignment, Func<CurrentGame, IEnumerable<PlayerSide>, Task>> assignmentDelegates;
		private readonly ICountryResource countryResource;
		private readonly IPlayerResource playerResource;
		private readonly ISettlementResource settlementResource;
		private readonly ISideResource sideResource;

		public ArbitraryCountryAssignmentEngine
			(
				ICountryResource countryResource
				, IPlayerResource playerResource
				, ISettlementResource settlementResource
				, ISideResource sideResource
			)
		{
			this.countryResource = countryResource;
			this.playerResource = playerResource;
			this.settlementResource = settlementResource;
			this.sideResource = sideResource;

			assignmentDelegates = new()
			{
				{ CountryAssignment.Random, CountryAssignmentRandomAsync },
			};
		}

		public async Task AssignCountriesAsync(CurrentGame currentGame, CountryAssignment assignmentType)
		{
			var playerSides = await sideResource.RetrieveManyAsync<PlayerSide>(currentGame.GameSession);
			await assignmentDelegates[assignmentType](currentGame, playerSides);
		}

		public Task AssignCountriesAsync(World world, IEnumerable<ICompetitor> competitors, CountryAssignment assignmentType)
		{
			throw new NotImplementedException();
		}

		private async Task AssignCountryAsync(CurrentGame currentGame, Side side, Contracts.V2.World.Country country)
		{
			await countryResource.AssignAsync(currentGame.GameSession, side, country);
		}

		private async Task CountryAssignmentRandomAsync(CurrentGame currentGame, IEnumerable<PlayerSide> sides)
		{
			var assignmentQueue = new Queue<Contracts.V2.World.Country>();
			var competitorToggles = MakeCompetitorToggles(sides);
			var countries = await countryResource.RetrieveManyAsync(currentGame.GameSession);

			foreach (var country in countries.OrderBy(w => Guid.NewGuid()))
				assignmentQueue.Enqueue(country);

			byte nextToggle = (byte)(assignmentQueue.Count % competitorToggles.Count);
			while (assignmentQueue.Any())
			{
				var nextPlayerSide = competitorToggles[nextToggle];
				var nextPlayer = await playerResource.RetrieveAsync(currentGame.GameSession, nextPlayerSide.PlayerId);
				var nextSide = await sideResource.RetrieveAsync(currentGame.GameSession, nextPlayer);
				var nextCountry = assignmentQueue.Dequeue();

				await AssignCountryAsync(currentGame, nextSide, nextCountry);
				nextToggle = (byte)(assignmentQueue.Count % competitorToggles.Count);
			}
		}

		private Dictionary<byte, PlayerSide> MakeCompetitorToggles(IEnumerable<PlayerSide> playerSides)
		{
			var returnMe = new Dictionary<byte, PlayerSide>();
			byte nextIndex = 0;
			foreach (var playerSide in playerSides)
			{
				returnMe.Add(nextIndex, playerSide);
				nextIndex++;
			}
			return returnMe;
		}
	}
}