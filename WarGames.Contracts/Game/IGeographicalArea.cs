namespace WarGames.Contracts.Game
{
	public interface IGeographicalArea
	{
		public IEnumerable<IGeographicalArea> AdjacentGeographicalAreas { get; }
		public IEnumerable<ILocation> LoiteringPositions { get; }
		public IEnumerable<ILocation> TargetablePositions { get; }
	}
}