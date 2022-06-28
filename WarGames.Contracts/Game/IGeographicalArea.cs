namespace WarGames.Contracts.Game
{
	/// <summary>
	/// This is a lovely future Idea. Full of wonderful possibilities that we will not explore in these exercises.
	/// Sadly this falls into overengineering.
	/// </summary>
	[Obsolete("Overengineering. We tried to solve too many problems early on.")]
	public interface IGeographicalArea
	{
		public IEnumerable<IGeographicalArea> AdjacentGeographicalAreas { get; }
		public IEnumerable<ILocation> LoiteringPositions { get; }
		public IEnumerable<ILocation> TargetablePositions { get; }
	}
}