using WarGames.CLI.Views;

namespace WarGames.CLI
{
	internal class MenuController
	{
		private readonly Dictionary<string, ConsoleView> views;
		private Action<string> currentAction;
		private ConsoleView currentView;
		private bool showMenu = true;

		public MenuController()
		{
			views = new Dictionary<string, ConsoleView>();
			currentView = new EmptyView();
			currentAction = ConsoleView.NoOp;
		}

		public MenuController(IEnumerable<ConsoleView> views)
		{
			this.views = views.ToDictionary(view => view.Title, view => view);
			if (views.Any())
				currentView = LoadView(views.First());
			else
				currentView = new EmptyView();

			currentAction = ConsoleView.NoOp;
		}

		public bool ShowMenu => showMenu;
		public void DoAction(string parameters)
		{
			currentAction(parameters);
		}
		public void GetNextAction()
		{
			currentAction = currentView.NextAction();
		}

		public void HandleViewDisposal()
		{
		}

		public void LoadView(string title)
		{
			LoadView(views[title]);
		}

		public ConsoleView LoadView(ConsoleView view)
		{
			currentView = view;
			currentView.OnLoad(HandleViewDisposal, LoadView);
			return currentView;
		}
	}
}