using System.Globalization;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Contracts.Game.TargetValues;

namespace WarGames.CLI.Renderers
{
	internal class DamageResultRenderer
	{
		private const int maxHeight = 20;
		private readonly ICompetitor competitor1;
		private readonly ICompetitor competitor2;
		private readonly IPlayer player1;
		private readonly IPlayer player2;
		private readonly int split2 = Console.WindowWidth / 2;
		private readonly int split4 = Console.WindowWidth / 4;
		private readonly int width = Console.WindowWidth;

		public DamageResultRenderer(IPlayer player1, IPlayer player2, ICompetitor competitor1, ICompetitor competitor2)
		{
			this.player1 = player1;
			this.player2 = player2;
			this.competitor1 = competitor1;
			this.competitor2 = competitor2;
		}

		public void Draw()
		{
			var player1DrawPositions = new DrawPositions(2, 1, 2, 5);
			var player2DrawPositions = new DrawPositions(split2 + 4, 1, 2, 5);

			Console.Clear();
			FillOutline();
			WritePlayerDetails(player1DrawPositions, player1, competitor1);
			WritePlayerDetails(player2DrawPositions, player2, competitor2);
			Console.SetCursorPosition(0, maxHeight + 5);
		}

		private void FillOutline()
		{
			var horizontalLine = $"|{new string('=', width - 2)}|";
			var columns = $"|{new string(' ', split2 - 2)}||{new string(' ', split2 - 2)}|";
			Console.WriteLine(horizontalLine);
			Console.WriteLine(columns);
			Console.WriteLine(columns);
			Console.WriteLine(horizontalLine);
			for (var i = 0; i < maxHeight; i++)
				Console.WriteLine(columns);
			Console.WriteLine(horizontalLine);
		}

		private void WritePlayerDetails(DrawPositions pos, IPlayer player, ICompetitor competitor)
		{
			Console.SetCursorPosition(pos.columnStart, pos.header1);
			Console.Write(player.Name);
			Console.SetCursorPosition(pos.columnStart, pos.header2);
			Console.Write(competitor.Name);

			Console.SetCursorPosition(pos.columnStart, pos.bodyStart);
			var startingPopulation = competitor.Settlements.Sum(settlement => settlement.TargetValues.First(tv => typeof(CivilianPopulation) == tv.GetType()).Value);
			var finalPopulation = competitor.Settlements.Sum(settlement => settlement.AftermathValues.First(tv => typeof(CivilianPopulation) == tv.GetType()).Value);

			var currentLine = pos.bodyStart;

			Console.SetCursorPosition(pos.columnStart, currentLine);
			Console.Write("Starting Population");
			Console.SetCursorPosition(pos.columnStart + split4, currentLine);
			Console.Write("Final Population");
			currentLine++;
			Console.SetCursorPosition(pos.columnStart, currentLine);
			Console.WriteLine(startingPopulation.ToString("#.#,K", CultureInfo.InvariantCulture));
			Console.SetCursorPosition(pos.columnStart + split4, currentLine);
			Console.WriteLine(finalPopulation.ToString("#.#,K", CultureInfo.InvariantCulture));
		}

		private class DrawPositions
		{
			public int bodyStart;

			public int columnStart;

			public int header1;

			public int header2;

			public DrawPositions(int columnStart, int header1, int header2, int bodyStart)
			{
				this.columnStart = columnStart;
				this.header1 = header1;
				this.header2 = header2;
				this.bodyStart = bodyStart;
			}
		}
	}
}