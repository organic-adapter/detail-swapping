using Map.Engine;

namespace WarGames.Contracts.Game
{
	public interface ILocation
	{
		public IGeographicalArea Area { get; }
		public Coord Coord { get; }
	}
}