using WarGames.Contracts.V2;
using WarGames.Contracts.V2.World;

namespace WarGames.Business.Game
{
	public interface ICountryAssignmentEngine
	{
		public Task AssignCountriesAsync(GameSession gameSession, CountryAssignment assignmentType);
	}
}