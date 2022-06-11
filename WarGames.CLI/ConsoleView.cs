namespace WarGames.CLI
{
	internal abstract class ConsoleView : IDisposable
	{
		protected Action chatter;
		protected Action disposeHook;
		protected Action<string> gotoOtherView;
		protected string greeting;

		public ConsoleView(Dictionary<string, Action<string>> commands)
		{
			Commands = commands;
			disposeHook = NoOp;
			gotoOtherView = NoOp;
			chatter = AccessDenied;
			greeting = string.Empty;
		}

		public Dictionary<string, Action<string>> Commands { get; }

		public abstract string Title { get; }

		public static void NoOp()
		{
		}

		public static void NoOp(string noop)
		{
		}

		public virtual void Dispose()
		{
			disposeHook();
		}

		protected virtual void Initialize()
		{
			//NoOp
		}

		public Action<string> NextAction()
		{
			return ListenForCommand();
		}

		public void OnLoad(Action disposeHook, Action<string> otherView)
		{
			Console.Title = Title;
			this.disposeHook = disposeHook;
			this.gotoOtherView = otherView;
			Initialize();
		}

		protected Action<string> ListenForCommand()
		{
			if (!string.IsNullOrEmpty(greeting))
				Console.WriteLine(greeting);

			Console.Write($"{Title}>");

			var response = (Console.ReadLine() ?? string.Empty).ToUpper();
			if (Commands.ContainsKey(response))
				return Commands[response];
			else
				chatter();

			return NoOp;
		}

		private void AccessDenied()
		{
			Console.WriteLine("Access denied. Connection Terminated.");
			Environment.Exit(0);
		}
	}
}