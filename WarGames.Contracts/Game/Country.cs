using Map.Engine;
using WarGames.Contracts.Competitors;

namespace WarGames.Contracts.Game
{
	[Serializable]
	public class Country : IGeographicalArea, IUnique<Guid>
	{
		public Country()
		{
			Settlements = new List<Settlement>();
			Id = Guid.NewGuid();
			Name = string.Empty;
			AdjacentCountries = new List<Country>();
			AdjacentOceans = new List<Ocean>();
		}

		public List<Country> AdjacentCountries { get; }
		public List<Ocean> AdjacentOceans { get; }
		public Guid Id { get; set; }
		public IEnumerable<ILocation> LoiteringPositions { get; set; }
		public string Name { get; set; }
		public ICompetitor? Owner { get; set; }
		public List<Settlement> Settlements { get; set; }

		//TODO: This will have little effect on non-UI implementations. We do not need to define this yet.
		public IGeoShape? Shape { get; }

		public IEnumerable<ILocation> TargetablePositions => Settlements.Select(s => s.Location);
		public TerrainType TerrainType => TerrainType.Ocean;

		public IEnumerable<IGeographicalArea> AdjacentGeographicalAreas => AdjacentGeographicalAreas.Union(AdjacentCountries);
	}
}