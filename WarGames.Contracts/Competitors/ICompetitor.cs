namespace WarGames.Contracts.Competitors
{
	public interface ICompetitor : IUnique<string>
	{
		public string Name { get; }
	}
}