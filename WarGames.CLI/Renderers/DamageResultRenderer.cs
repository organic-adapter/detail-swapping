using System.Globalization;
using WarGames.Contracts.Game.TargetValues;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.CLI.Renderers
{
	internal class DamageResultRenderer
	{
		private const int maxHeight = 20;
		private readonly Player player1;
		private readonly Player player2;
		private readonly IEnumerable<Settlement> side1Settlements;
		private readonly IEnumerable<Settlement> side2Settlements;
		private readonly Side side1;
		private readonly Side side2;
		private readonly int split2 = Console.WindowWidth / 2;
		private readonly int split4 = Console.WindowWidth / 4;
		private readonly int width = Console.WindowWidth;

		public DamageResultRenderer
				(
				 IEnumerable<Settlement> side1Settlements
				, IEnumerable<Settlement> side2Settlements
				, Player player1
				, Player player2
				, Side side1
				, Side side2
				)
		{
			this.side1Settlements = side1Settlements;
			this.side2Settlements = side2Settlements;
			this.player1 = player1;
			this.player2 = player2;
			this.side1 = side1;
			this.side2 = side2;
		}

		public void Draw()
		{
			var player1DrawPositions = new DrawPositions(2, 1, 2, 5);
			var player2DrawPositions = new DrawPositions(split2 + 4, 1, 2, 5);

			Console.Clear();
			FillOutline();
			WritePlayerDetails(player1DrawPositions, side1Settlements, player1, side1);
			WritePlayerDetails(player2DrawPositions, side2Settlements, player2, side2);
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

		private void WritePlayerDetails(DrawPositions pos, IEnumerable<Settlement> settlements, Player player, Side side)
		{
			Console.SetCursorPosition(pos.columnStart, pos.header1);
			Console.Write(player.Name);
			Console.SetCursorPosition(pos.columnStart, pos.header2);
			Console.Write(side.DisplayName);

			Console.SetCursorPosition(pos.columnStart, pos.bodyStart);
			var startingPopulation = settlements.Sum(settlement => settlement.TargetValues.First(tv => typeof(CivilianPopulation) == tv.GetType()).Value);
			var finalPopulation = settlements.Sum(settlement => settlement.AftermathValues.First(tv => typeof(CivilianPopulation) == tv.GetType()).Value);

			var currentLine = pos.bodyStart;

			Console.SetCursorPosition(pos.columnStart, currentLine);
			Console.Write("Starting Population");
			Console.SetCursorPosition(pos.columnStart + split4, currentLine);
			Console.Write("Final Population");
			currentLine++;
			Console.SetCursorPosition(pos.columnStart, currentLine);
			Console.WriteLine(startingPopulation.ToString("#.#", CultureInfo.InvariantCulture));
			Console.SetCursorPosition(pos.columnStart + split4, currentLine);
			Console.WriteLine(finalPopulation.ToString("#.#", CultureInfo.InvariantCulture));
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