using Map.Engine;

namespace WarGames.Contracts.Game
{
	[Serializable]
	public class Ocean : IGeographicalArea
	{
		private IEnumerable<IGeographicalArea> adjacentGeographicalAreas;
		public Ocean()
		{
			AdjacentCountries = new List<Country>();
			AdjacentOceans = new List<Ocean>();
			LoiteringPositions = new List<ILocation>();
		}

		public List<Country> AdjacentCountries { get; set; }
		public List<Ocean> AdjacentOceans { get; set; }
		public IEnumerable<ILocation> LoiteringPositions { get; set; }

		public IGeoShape? Shape { get; }
		public IEnumerable<ILocation> TargetablePositions => new List<ILocation>();
		public TerrainType TerrainType => TerrainType.Ocean;

		public IEnumerable<IGeographicalArea> AdjacentGeographicalAreas => adjacentGeographicalAreas;
	}
}