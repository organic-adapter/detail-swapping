using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Competitors
{
	public abstract class BaseCompetitor : ICompetitor
	{
		public BaseCompetitor()
		{
			Countries = new List<Country>();
			MissileDeliverySystems = new List<IMissileDeliverySystem>();
		}

		public List<Country> Countries { get; set; }
		public abstract string Id { get; }
		public List<IMissileDeliverySystem> MissileDeliverySystems { get; set; }
		public abstract string Name { get; }

		public IEnumerable<Settlement> Settlements => Countries.SelectMany(country => country.Settlements);

		public void Add(IEnumerable<IMissileDeliverySystem> missileDeliverySystems)
		{
			MissileDeliverySystems.AddRange(missileDeliverySystems);
		}

		public void Add(IMissileDeliverySystem missileDeliverySystem)
		{
			MissileDeliverySystems.Add(missileDeliverySystem);
		}
	}
}