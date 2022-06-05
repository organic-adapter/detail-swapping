using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Game
{
	public class CountryAssignmentEngine : ICountryAssignmentEngine
	{
		private readonly Dictionary<CountryAssignment, Action<World, IEnumerable<ICompetitor>>> assignmentDelegates;

		public CountryAssignmentEngine()
		{
			assignmentDelegates = new Dictionary<CountryAssignment, Action<World, IEnumerable<ICompetitor>>>()
			{
				{ CountryAssignment.Random, CountryAssignmentRandom },
				{ CountryAssignment.ByName, CountryAssignmentByName }
			};
		}

		public async Task AssignCountriesAsync(World world, IEnumerable<ICompetitor> competitors, CountryAssignment assignmentType)
		{
			await Task.Run(() => assignmentDelegates[assignmentType](world, competitors));
		}

		private void AssignCountry(ICompetitor competitor, Country country)
		{
			competitor.Countries.Add(country);
			country.Owner = competitor;
		}

		private void CountryAssignmentByName(World world, IEnumerable<ICompetitor> competitors)
		{
			var assignmentQueue = new Queue<Country>();

			foreach (var competitor in competitors)
				foreach (var country in world.Countries
											.Where(country =>
														country.Name.Contains(competitor.Name)
														|| country.Name.Contains(competitor.Id, StringComparison.OrdinalIgnoreCase))
												  )
					AssignCountry(competitor, country);
		}

		private void CountryAssignmentRandom(World world, IEnumerable<ICompetitor> competitors)
		{
			var assignmentQueue = new Queue<Country>();
			var competitorToggles = MakeCompetitorToggles(competitors);

			foreach (var country in world.Countries.OrderBy(w => Guid.NewGuid()))
				assignmentQueue.Enqueue(country);

			byte nextToggle = (byte)(assignmentQueue.Count % competitorToggles.Count);
			while (assignmentQueue.Any())
			{
				var nextCompetitor = competitorToggles[nextToggle];
				var nextCountry = assignmentQueue.Dequeue();

				AssignCountry(nextCompetitor, nextCountry);
				nextToggle = (byte)(assignmentQueue.Count % competitorToggles.Count);
			}
		}

		private Dictionary<byte, ICompetitor> MakeCompetitorToggles(IEnumerable<ICompetitor> competitors)
		{
			var returnMe = new Dictionary<byte, ICompetitor>();
			byte nextIndex = 0;
			foreach (var competitor in competitors)
			{
				returnMe.Add(nextIndex, competitor);
				nextIndex++;
			}
			return returnMe;
		}
	}
}