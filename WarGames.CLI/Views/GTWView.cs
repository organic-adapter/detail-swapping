using WarGames.Business.Competitors;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.CLI.Renderers;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;
using WarGames.Resources;

namespace WarGames.CLI.Views
{
	internal class GTWView : ConsoleView
	{
		private const int maxPicks = 9;
		private readonly CurrentGame currentGame;
		private readonly IPlayerSideManager playerSideManager;
		private string chatterText = "I'm afraid I can't do that";
		private ICompetitorResource competitorResource;
		private Contracts.V2.Sides.Player cpu0;
		private Contracts.V2.Sides.Player cpu1;
		private List<Contracts.V2.World.Settlement> currentPicks;
		private IGameManager gameManager;
		private Contracts.V2.Sides.Player humanPlayer;
		private List<Contracts.V2.World.Settlement> potentialTargets;

		private Dictionary<int, TargetPriority> prioritySelectionMap = new Dictionary<int, TargetPriority>()
		{
			{ 9, TargetPriority.Primary },
			{ 8, TargetPriority.Primary },
			{ 7, TargetPriority.Secondary },
			{ 6, TargetPriority.Secondary },
			{ 5, TargetPriority.Secondary },
			{ 4, TargetPriority.Tertiary },
			{ 3, TargetPriority.Tertiary },
			{ 2, TargetPriority.Tertiary },
			{ 1, TargetPriority.Tertiary },
		};

		public GTWView
				(
					CurrentGame currentGame
					, IGameManager gameManager
					, IPlayerSideManager playerSideManager
					, ICompetitorResource competitorResource
				) : base(new Dictionary<string, Action<string>>())
		{
			chatter = Chatter;
			this.currentGame = currentGame;
			currentGame.GameSession = new Contracts.V2.GameSession() { Id = Guid.NewGuid().ToString() };
			this.gameManager = gameManager;
			this.playerSideManager = playerSideManager;
			this.competitorResource = competitorResource;
			potentialTargets = new List<Contracts.V2.World.Settlement>();
			currentPicks = new List<Contracts.V2.World.Settlement>();

			cpu0 = Contracts.V2.Sides.Player.Empty;
			cpu1 = Contracts.V2.Sides.Player.Empty;
			humanPlayer = Contracts.V2.Sides.Player.Empty;
		}

		public override string Title => "GTW";
		private int TotalPicks => maxPicks - currentPicks.Count;

		protected override void Initialize()
		{
			playerSideManager.AddAsync(Business.Sides.Capitalism.Instance).Wait();
			playerSideManager.AddAsync(Business.Sides.Communism.Instance).Wait();

			Console.WriteLine("How many players?");
			Commands.Add("0", SetAutoPlay);
			Commands.Add("1", SetSinglePlayer);
			base.Initialize();
		}

		private void CalculateTopTenTargets()
		{
			if (gameManager.CurrentPhase == GamePhase.PickTargets)
			{
				Commands.Clear();
				var topTen = (gameManager.GetPotentialTargetsAsync(humanPlayer).Result).OrderByDescending(target => target.TargetValues.First().Value).Take(10).ToList();

				for (var i = 0; i < topTen.Count; i++)
				{
					var item = topTen[i];
					var pack = i.ToString();
					potentialTargets.Add(item);
					Commands.Add(i.ToString(), (string parameters) => SelectTarget(item, pack));
				}
				DrawTargetDetails();
			}
		}

		private void Chatter()
		{
			Console.WriteLine(chatterText);
		}

		private void DisplayDamageResults()
		{
			var player1 = gameManager.LoadedPlayers.First();
			var player2 = gameManager.LoadedPlayers.Last();

			var damageResultRenderer = new DamageResultRenderer(player1.Key, player2.Key, player1.Value, player2.Value);
			damageResultRenderer.Draw();
		}

		private void DrawAvailableTargetList()
		{
			Console.WriteLine("-------Available Selections--------");

			foreach (var command in Commands)
				Console.WriteLine($"{command.Key}: {potentialTargets[int.Parse(command.Key)].Name}");
		}

		private void DrawCurrentTargetList()
		{
			Console.WriteLine("-------Current Selections--------");
			IEnumerable<Target> currentTargets = gameManager.GetCurrentTargetsAsync(humanPlayer).Result;
			foreach (var target in currentTargets)
				Console.WriteLine(target.Key.Name);
		}

		private void DrawTargetDetails()
		{
			Console.Clear();
			Console.WriteLine("Pick your targets");
			Console.WriteLine($"{TotalPicks} remaining choices");
			Console.WriteLine("---------------------------------");

			DrawCurrentTargetList();
			DrawAvailableTargetList();
		}

		private void LoadGamePrerequisites()
		{
			gameManager.InitializeDefaultsAsync().Wait();
			gameManager.LoadWorldAsync().Wait();
			gameManager.AssignCountriesAsync(CountryAssignment.Random).Wait();
			gameManager.AssignArsenalAsync(ArsenalAssignment.Arbitrary).Wait();
			gameManager.MakeAiDecisionsAsync().Wait();

			CalculateTopTenTargets();
		}

		private void PickNato(string parameters)
		{
			if (gameManager.CurrentPhase == GamePhase.PickPlayers)
			{
				playerSideManager.ChooseAsync(humanPlayer, Business.Sides.Capitalism.Instance);
				LoadGamePrerequisites();
			}
		}

		private void PickUssr(string parameters)
		{
			if (gameManager.CurrentPhase == GamePhase.PickPlayers)
			{
				playerSideManager.ChooseAsync(humanPlayer, Business.Sides.Communism.Instance);
				LoadGamePrerequisites();
			}
		}

		private void PrepareForEndOfWorld()
		{
			Console.Clear();
			Console.WriteLine("Launch Detected. Firing in retaliation.");
			Thread.Sleep(1000);
			Console.WriteLine("5");
			Thread.Sleep(1000);
			Console.WriteLine("4");
			Thread.Sleep(1000);
			Console.WriteLine("3");
			Thread.Sleep(1000);
			Console.WriteLine("2");
			Thread.Sleep(1000);
			Console.WriteLine("1");
			Thread.Sleep(1000);
			Console.WriteLine("Launch");
			var task = gameManager.RainFireAsync();
			var tick = 1;
			while (!task.IsCompleted)
			{
				Console.SetCursorPosition(2, 10);
				Console.WriteLine(new string(' ', Console.WindowWidth));
				Console.SetCursorPosition(2, 10);
				Console.WriteLine(new string('.', tick));
				Thread.Sleep(350);
				tick++;
				if (tick >= 10)
					tick = 1;
			}
			DisplayDamageResults();
		}

		private void PrepareTargetAssignmentPhase()
		{
			Commands.Clear();
			Console.Clear();
			Console.WriteLine("Assigning Targets to Missile Defense Systems");
			gameManager.ReadyForTargetAssignments();
			gameManager.SetTargetAssignmentsAsync().Wait();
			gameManager.ReadyForLaunch();
			Thread.Sleep(1000);
			PrepareForEndOfWorld();
		}

		private void SelectTarget(Contracts.V2.World.Settlement selectMe, string index)
		{
			gameManager.AddTargetAsync(selectMe, prioritySelectionMap[TotalPicks]).Wait();
			currentPicks.Add(selectMe);
			Commands.Remove(index);
			DrawTargetDetails();
			if (TotalPicks == 0)
				PrepareTargetAssignmentPhase();
		}

		private void SetAutoPlay(string _)
		{
			gameManager.InitializeDefaultsAsync().Wait();
		}

		private void SetSinglePlayer(string _)
		{
			humanPlayer = new Contracts.V2.Sides.Player("Hooman", Guid.NewGuid().ToString(), Contracts.V2.Sides.PlayerType.Human);

			Commands.Clear();
			Console.WriteLine("What side do you wish to play? \r\n NATO \r\n USSR");
			Commands.Add("NATO", PickNato);
			Commands.Add("USSR", PickUssr);
			potentialTargets = new List<Contracts.V2.World.Settlement>();
			currentPicks = new List<Contracts.V2.World.Settlement>();
		}
	}
}