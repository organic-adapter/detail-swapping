using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.Contracts.Competitors
{
	public interface ICompetitor : IUnique<string>
	{
		public List<Country> Countries { get; }
		public IEnumerable<Settlement> Settlements { get; }
		public List<IMissileDeliverySystem> MissileDeliverySystems { get; }
		public string Name { get; }

		public void Add(IEnumerable<IMissileDeliverySystem> missileDeliverySystems);

		public void Add(IMissileDeliverySystem missileDeliverySystem);
	}
}