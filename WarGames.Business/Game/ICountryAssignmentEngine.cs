using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Business.Game
{
	public interface ICountryAssignmentEngine
	{
		public Task AssignCountriesAsync(CurrentGame currentGame, CountryAssignment assignmentType);

		[Obsolete("World class obsoleting")]
		public Task AssignCountriesAsync(World world, IEnumerable<ICompetitor> competitors, CountryAssignment assignmentType);
	}
}