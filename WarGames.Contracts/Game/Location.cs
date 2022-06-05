using Map.Engine;

namespace WarGames.Contracts.Game
{
	public class Location : ILocation
	{
		public Location(IGeographicalArea area, Coord coord)
		{
			Area = area;
			Coord = coord;
		}

		public IGeographicalArea Area { get; }

		public Coord Coord { get; }
	}
}