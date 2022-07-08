using WarGames.Contracts.V2.Sides;

namespace WarGames.Business.Sides
{
	public class Capitalism : Side
	{
		public static readonly Capitalism Instance = new Capitalism();
		public Capitalism()
		{
			Id = "capitalism";
			DisplayName = "NATO";
		}
	}
}