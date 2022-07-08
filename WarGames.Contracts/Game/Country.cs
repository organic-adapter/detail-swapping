using Map.Engine;
using WarGames.Contracts.Competitors;

namespace WarGames.Contracts.Game
{
	[Obsolete(ObsoleteConstants.Version2Incoming)]
	[Serializable]
	public class Country : IGeographicalArea, IUnique<string>
	{
		public Country()
		{
			Settlements = new List<Settlement>();
			Id = Guid.NewGuid().ToString();
			Name = string.Empty;
			AdjacentCountries = new List<Country>();
			AdjacentOceans = new List<Ocean>();
			LoiteringPositions = new List<ILocation>();
		}

		public List<Country> AdjacentCountries { get; }
		public IEnumerable<IGeographicalArea> AdjacentGeographicalAreas => AdjacentGeographicalAreas.Union(AdjacentCountries);
		public List<Ocean> AdjacentOceans { get; }
		public virtual string Id { get; set; }
		public IEnumerable<ILocation> LoiteringPositions { get; set; }
		public virtual string Name { get; set; }
		public ICompetitor? Owner { get; set; }
		public virtual List<Settlement> Settlements { get; set; }

		//TODO: This will have little effect on non-UI implementations. We do not need to define this yet.
		public IGeoShape? Shape { get; }

		public IEnumerable<ILocation> TargetablePositions => Settlements.Select(s => s.Location);
		public TerrainType TerrainType => TerrainType.Ocean;

		public override bool Equals(object? obj)
		{
			if (obj is not Country compareMe)
				return false;

			return compareMe.Id.Equals(Id, StringComparison.Ordinal);
		}
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}