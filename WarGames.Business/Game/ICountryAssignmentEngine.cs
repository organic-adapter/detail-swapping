using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Game
{
	public interface ICountryAssignmentEngine
	{
		public Task AssignCountriesAsync(World world, IEnumerable<ICompetitor> competitors, CountryAssignment assignmentType);
	}
}