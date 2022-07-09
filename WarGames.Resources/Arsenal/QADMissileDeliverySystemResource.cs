using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Resources.Arsenal
{
	public class QADMissileDeliverySystemResource : IMissileDeliverySystemResource
	{
		private readonly Dictionary<GameSession, Dictionary<Side, HashSet<IMissileDeliverySystem>>> sideMap;

		public QADMissileDeliverySystemResource()
		{
			sideMap = new();
		}

		public async Task AssignAsync(GameSession gameSession, Side side, IMissileDeliverySystem missileDeliverySystem)
		{
			await Task.Run(() =>
							{
								EnforceExistence(gameSession, side);
								sideMap[gameSession][side].Add(missileDeliverySystem);
							});
		}

		public async Task<IEnumerable<IMissileDeliverySystem>> RetrieveManyAsync(GameSession gameSession, Side side)
		{
			return await Task.Run(() => sideMap[gameSession][side]);
		}

		private void EnforceExistence(GameSession gameSession, Side side)
		{
			if (!sideMap.ContainsKey(gameSession))
				sideMap.Add(gameSession, new());

			if (!sideMap[gameSession].ContainsKey(side))
				sideMap[gameSession].Add(side, new());
		}
	}
}