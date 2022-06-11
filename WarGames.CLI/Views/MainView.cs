using WarGames.CLI.Constants;

namespace WarGames.CLI.Views
{
	internal class MainView : ConsoleView
	{
		private string chatterText = "I'm afraid I can't do that";

		public MainView() : base(new Dictionary<string, Action<string>>())
		{
			Commands.Add("LIST GAMES", ListGames);
			Commands.Add("GLOBAL THERMONUCLEAR WAR", GlobalThermonuclearWar);
			Commands.Add("GTW", GlobalThermonuclearWar);
			chatter = Chatter;
			greeting = "Greetings Professor Falcon";
		}

		public override string Title => "MAIN";

		private void Chatter()
		{
			Console.WriteLine(chatterText);
		}

		private void GlobalThermonuclearWar(string parameters)
		{
			gotoOtherView("GTW");
		}

		private void ListGames(string parameters)
		{
			foreach (var game in Games.List)
			{
				Console.WriteLine(game);
				Thread.Sleep(450);
			}
		}
	}
}