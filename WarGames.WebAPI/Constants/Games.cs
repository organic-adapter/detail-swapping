namespace WarGames.WebAPI.Constants
{
	/// <summary>
	/// We're just being silly here. No need for any complex injections or anything.
	/// </summary>
	internal class Games
	{
		public static IEnumerable<string> List = new List<string>()
		{
			"Checkers",
			"Chess",
			"Backgammon",
			"Bowling",
			"Wreck It-George",
			"Global Thermonuclear War",
		};
	}
}
