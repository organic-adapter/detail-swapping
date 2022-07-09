using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Resources.Arsenal
{
	public interface IMissileDeliverySystemResource
	{
		Task AssignAsync(GameSession gameSession, Side side, IMissileDeliverySystem missileDeliverySystem);

		Task<IEnumerable<IMissileDeliverySystem>> RetrieveManyAsync(GameSession gameSession, Side side);
	}
}