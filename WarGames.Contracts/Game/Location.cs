using Map.Engine;

namespace WarGames.Contracts.Game
{
	public class Location : ILocation
	{
		public static readonly Location Empty = new Location(NoAssignment.Instance, Coord.NoAssignment);

		public Location(IGeographicalArea area, Coord coord)
		{
			Area = area;
			Coord = coord;
		}

		public IGeographicalArea Area { get; }

		public Coord Coord { get; }

		public class NoAssignment : IGeographicalArea
		{
			public static readonly IGeographicalArea Instance = new NoAssignment();

			public IEnumerable<IGeographicalArea> AdjacentGeographicalAreas => new List<IGeographicalArea>();

			public IEnumerable<ILocation> LoiteringPositions => new List<ILocation>();

			public IEnumerable<ILocation> TargetablePositions => new List<ILocation>();
		}
	}
}