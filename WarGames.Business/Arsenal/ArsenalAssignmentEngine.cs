using WarGames.Business.Arsenal.MissileDeliverySystems;
using WarGames.Business.Arsenal.Missiles;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Planet;
using WarGames.Resources.Sides;

namespace WarGames.Business.Arsenal
{
	public class ArsenalAssignmentEngine : IArsenalAssignmentEngine
	{
		private const int arbitraryNumber = 10;
		private readonly Dictionary<ArsenalAssignment, Func<GameSession, ArsenalAssignment, Task>> assignmentDelegates;
		private readonly ICountryResource countryResource;
		private readonly IMissileDeliverySystemResource missileDeliverySystemResource;
		private readonly ISettlementResource settlementResource;
		private readonly ISideResource sideResource;
		public ArsenalAssignmentEngine(
			ICountryResource countryResource
			, IMissileDeliverySystemResource missileDeliverySystemResource
			, ISettlementResource settlementResource
			, ISideResource sideResource
			)
		{
			this.countryResource = countryResource;
			this.missileDeliverySystemResource = missileDeliverySystemResource;
			this.settlementResource = settlementResource;
			this.sideResource = sideResource;

			assignmentDelegates = new()
			{
				{ ArsenalAssignment.Arbitrary, ArsenalAssignmentArbitrary },
			};
		}

		public async Task AssignArsenalAsync(GameSession gameSession, ArsenalAssignment assignmentType)
		{
			await Task.Run(() => assignmentDelegates[assignmentType](gameSession, assignmentType));
		}

		private async Task ArsenalAssignmentArbitrary(GameSession gameSession, ArsenalAssignment assignmentType)
		{
			short payloadCount = 1;
			var sides = await sideResource.RetrieveManyAsync(gameSession);
			foreach (var side in sides)
			{
				var countries = (await countryResource.RetrieveManyAsync(gameSession, side)).ToList();
				for (var i = 0; i < arbitraryNumber; i++)
				{
					var payload = new ICBM();
					var settlement = (await settlementResource.RetrieveManyAsync(gameSession, countries[i])).First();

					await missileDeliverySystemResource.AssignAsync(gameSession, side, new Silo(payloadCount, payload) { Coord = settlement.Coord });
				}
			}
		}
	}
}