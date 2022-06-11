using WarGames.CLI.Constants;

namespace WarGames.CLI.Views
{
	internal class LogonView : ConsoleView
	{
		public LogonView() : base(new Dictionary<string, Action<string>>())
		{
			Commands.Add("HELP", Help);
			Commands.Add("HELP LOGON", Help);
			Commands.Add("HELP GAMES", HelpGames);
			Commands.Add("LIST GAMES", ListGames);
			Commands.Add("JOSHUA", Login);
		}

		public override string Title => "LOGON";

		private void Help(string parameters)
		{
			Console.WriteLine("Here be dragons");
			Console.WriteLine(string.Empty);
		}

		private void HelpGames(string parameters)
		{
			Console.WriteLine("We got games.");
			Console.WriteLine(string.Empty);
		}

		private void ListGames(string parameters)
		{
			foreach (var game in Games.List)
			{
				Console.WriteLine(game);
				Thread.Sleep(450);
			}
		}

		private void Login(string parameters)
		{
			gotoOtherView("MAIN");
		}
	}
}