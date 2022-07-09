using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Business.Sides;
using WarGames.CLI.Renderers;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.CLI.Views
{
	internal class GTWView : ConsoleView
	{
		private const int maxPicks = 9;
		private readonly CurrentGame currentGame;
		private readonly IEnumerable<Side> defaultSides;
		private readonly IGameManager gameManager;
		private readonly IPlayerSideManager playerSideManager;
		private readonly IWorldManager worldManager;
		private string chatterText = "I'm afraid I can't do that";
		private Player cpu0;
		private Player cpu1;
		private List<Settlement> currentPicks;
		private Player human;
		private List<Settlement> potentialTargets;

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
					, IEnumerable<Side> defaultSides
					, IGameManager gameManager
					, IPlayerSideManager playerSideManager
					, IWorldManager worldManager
				) : base(new Dictionary<string, Action<string>>())
		{
			this.currentGame = currentGame;
			this.defaultSides = defaultSides;
			this.gameManager = gameManager;
			this.playerSideManager = playerSideManager;
			this.worldManager = worldManager;

			chatter = Chatter;
			potentialTargets = new List<Settlement>();
			currentPicks = new List<Settlement>();

			cpu0 = Player.Empty;
			cpu1 = Player.Empty;
			human = Player.Empty;
		}

		public override string Title => "GTW";
		private Capitalism Capitalism => (Capitalism)defaultSides.First(side => side.GetType() == typeof(Capitalism));
		private Communism Communism => (Communism)defaultSides.First(side => side.GetType() == typeof(Communism));
		private int TotalPicks => maxPicks - currentPicks.Count;

		protected override void Initialize()
		{
			currentGame.CreateNew();
			playerSideManager.AddAsync(defaultSides.ToArray());

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
				var side = playerSideManager.WhatIsPlayerAsync(human).Result;
				var topTen = (gameManager.GetPotentialTargetsAsync(human).Result).OrderByDescending(target => target.TargetValues.First().Value).Take(10).ToList();

				for (var i = 0; i < topTen.Count; i++)
				{
					var t = topTen[i];
					var pack = i.ToString();
					potentialTargets.Add(t);
					Commands.Add(i.ToString(), (string parameters) => SelectTarget(side, t, pack));
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
			var players = playerSideManager.GetPlayersAsync().Result;
			var player1 = players.First();
			var player2 = players.Last();
			var side1 = playerSideManager.WhatIsPlayerAsync(player1).Result;
			var side2 = playerSideManager.WhatIsPlayerAsync(player2).Result;
			var side1Settlements = worldManager.GetSettlementsAsync(side1).Result;
			var side2Settlements = worldManager.GetSettlementsAsync(side2).Result;

			var damageResultRenderer = new DamageResultRenderer(side1Settlements, side2Settlements, player1, player2, side1, side2);
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
			IEnumerable<Target> currentTargets = gameManager.GetCurrentTargetsAsync(human).Result;
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

			currentGame.Start();
			CalculateTopTenTargets();
		}

		private void PickNato(string parameters)
		{
			if (gameManager.CurrentPhase == GamePhase.PickPlayers)
			{
				playerSideManager.ChooseAsync(human, Capitalism).Wait();
				LoadGamePrerequisites();
			}
		}

		private void PickUssr(string parameters)
		{
			if (gameManager.CurrentPhase == GamePhase.PickPlayers)
			{
				playerSideManager.ChooseAsync(human, Communism).Wait();

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

		private void SelectTarget(Side side, Settlement selectMe, string index)
		{
			gameManager.AddTargetAsync(side, selectMe, prioritySelectionMap[TotalPicks]).Wait();
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
			human = new Player("Hooman", Guid.NewGuid().ToString(), PlayerType.Human);
			playerSideManager.AddAsync(human).Wait();

			Commands.Clear();
			Console.WriteLine("What side do you wish to play? \r\n NATO \r\n USSR");
			Commands.Add("NATO", PickNato);
			Commands.Add("USSR", PickUssr);
			potentialTargets = new List<Settlement>();
			currentPicks = new List<Settlement>();
		}
	}
}