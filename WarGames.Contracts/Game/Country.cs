using Map.Engine;
using WarGames.Contracts.Competitors;

namespace WarGames.Contracts.Game
{
	[Serializable]
	public class Country : IUnique<Guid>
	{
		public Country()
		{
			Settlements = new List<Settlement>();
			Id = Guid.NewGuid();
			Name = string.Empty;
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
		public ICompetitor? Owner { get; set; }
		public List<Settlement> Settlements { get; }
		//TODO: This will have little effect on non-UI implementations. We do not need to define this yet.
		public IGeoShape Shape { get; }
	}
}