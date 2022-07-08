using WarGames.Contracts.V2.Sides;

namespace WarGames.Business.Sides
{
	public class Communism : Side
	{
		public static readonly Communism Instance = new Communism();

		public Communism()
		{
			Id = "communism";
			DisplayName = "USSR";
		}
	}
}