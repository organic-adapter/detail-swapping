using WarGames.Business.Arsenal.MissileDeliverySystems;
using WarGames.Business.Arsenal.Missiles;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal
{
	public class ArsenalAssignmentEngine : IArsenalAssignmentEngine
	{
		private const int arbitraryNumber = 10;
		private readonly Dictionary<ArsenalAssignment, Action<World, IEnumerable<ICompetitor>>> assignmentDelegates;

		public ArsenalAssignmentEngine()
		{
			assignmentDelegates = new Dictionary<ArsenalAssignment, Action<World, IEnumerable<ICompetitor>>>()
			{
				{ ArsenalAssignment.Arbitrary, ArsenalAssignmentArbitrary },
			};
		}

		public async Task AssignArsenalAsync(World world, IEnumerable<ICompetitor> competitors, ArsenalAssignment assignmentType)
		{
			await Task.Run(() => assignmentDelegates[assignmentType](world, competitors));
		}

		private void ArsenalAssignmentArbitrary(World world, IEnumerable<ICompetitor> competitors)
		{
			foreach(var competitor in competitors)
			{
				for(var i = 0; i < arbitraryNumber; i++)
				{
					var payload = new ICBM();
					var settlement = competitor.Countries[i].Settlements.First();
					competitor.MissileDeliverySystems.Add(new Silo(0f, 1, payload) { Location = settlement.Location });	
				}
			}
		}
	}
}