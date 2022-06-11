using WarGames.Business.Arsenal;
using WarGames.Business.Competitors;
using WarGames.Business.Game;
using WarGames.Business.Managers;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Game;

namespace WarGames.CLI.Views
{
	internal class GTWView : ConsoleView
	{
		private string chatterText = "I'm afraid I can't do that";
		private IPlayer cpu0;
		private IGameManager gameManager;
		private IPlayer human;

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

		private int totalPicks;
		private List<Settlement> potentialTargets;

		public GTWView(IGameManager gameManager) : base(new Dictionary<string, Action<string>>())
		{
			chatter = Chatter;
			this.gameManager = gameManager;
			cpu0 = new Player() { Name = "JOSHUA", Id = Guid.NewGuid().ToString() };
			human = new Player() { Name = "HOOMAN", Id = Guid.NewGuid().ToString() };
			potentialTargets = new List<Settlement>();
		}

		public override string Title => "GTW";

		protected override void Initialize()
		{
			Console.WriteLine("What side do you wish to play? \r\n NATO \r\n USSR");
			Commands.Add("NATO", PickNato);
			Commands.Add("USSR", PickUssr);
			totalPicks = 9;
			potentialTargets = new List<Settlement>();
			base.Initialize();
		}

		private void CalculateTopTenTargets()
		{
			if (gameManager.CurrentPhase == GamePhase.PickTargets)
			{
				Commands.Clear();
				var topTen = (gameManager.GetPotentialTargets(human).Result).OrderByDescending(target => target.TargetValues.First().Value).Take(10).ToList();

				for(var i = 0; i < topTen.Count; i++)
				{
					var t = topTen[i];
					var pack = i.ToString();
					potentialTargets.Add(t);
					Commands.Add(i.ToString(), (string parameters) => SelectTarget(t, pack));
				}
				DrawTargetDetails();
			}
		}

		private void Chatter()
		{
			Console.WriteLine(chatterText);
		}

		private async Task CpuSelectTargetsAsync()
		{
			var topTen = (gameManager.GetCurrentTargetsAsync(cpu0).Result).OrderByDescending(target => target.Key.TargetValues.First()).Take(10);
			foreach (var t in topTen)
				await gameManager.AddTargetAsync(t.Key, TargetPriority.Primary);
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
			Console.WriteLine($"{totalPicks} remaining choices");
			Console.WriteLine("---------------------------------");

			DrawCurrentTargetList();
			DrawAvailableTargetList();
		}

		private void LoadGamePrerequisites()
		{
			gameManager.LoadWorldAsync().Wait();
			gameManager.AssignCountriesAsync(CountryAssignment.Random).Wait();
			gameManager.AssignArsenalAsync(ArsenalAssignment.Arbitrary).Wait();

			CpuSelectTargetsAsync().Wait();
			CalculateTopTenTargets();
		}

		private void PickNato(string parameters)
		{
			if (gameManager.CurrentPhase == GamePhase.PickPlayers)
			{
				gameManager.LoadPlayerAsync(human, new Capitalism()).Wait();
				gameManager.LoadPlayerAsync(cpu0, new Communism()).Wait();

				LoadGamePrerequisites();
			}
		}

		private void PickUssr(string parameters)
		{
			if (gameManager.CurrentPhase == GamePhase.PickPlayers)
			{
				gameManager.LoadPlayerAsync(human, new Communism()).Wait();
				gameManager.LoadPlayerAsync(cpu0, new Capitalism()).Wait();

				LoadGamePrerequisites();
			}
		}

		private void SelectTarget(Settlement selectMe, string index)
		{
			gameManager.AddTargetAsync(selectMe, prioritySelectionMap[totalPicks]).Wait();
			Commands.Remove(index);
			DrawTargetDetails();
			totalPicks--;
			if (totalPicks == 0)
				PrepareTargetAssignmentPhase();
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

		private void PrepareForEndOfWorld()
		{
			Console.Clear();
			Console.WriteLine("Launch Detected. Firing in retaliation.");
			Thread.Sleep(1000);
			Console.WriteLine("3");
			Thread.Sleep(1000);
			Console.WriteLine("2");
			Thread.Sleep(1000);
			Console.WriteLine("1");
			Thread.Sleep(1000);
			Console.WriteLine("Launch");
			gameManager.RainFireAsync().Wait();
		}
	}
}