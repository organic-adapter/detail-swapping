using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Resources.Competitors
{
	public class Competitor : ICompetitor
	{
		public Competitor(string name, string id)
		{
			Name = name;
			Id = id;
			Countries = new List<Country>();
			MissileDeliverySystems = new List<IMissileDeliverySystem>();
		}

		public Competitor()
		{
			Name = string.Empty;
			Id = string.Empty;
			Countries = new List<Country>();
			MissileDeliverySystems = new List<IMissileDeliverySystem>();
		}

		public static Competitor Empty => new Competitor();
		public List<Country> Countries { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }

		public List<IMissileDeliverySystem> MissileDeliverySystems { get; set; }

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