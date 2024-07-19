using ScrabbleCore.Classes;
using ScrabbleCore.Enums;
using ScrabbleCore.Solver;
using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;

namespace ScrabbleTesting;

internal class Program
{
	static void Main()
	{
		//ScrabbleSolverTest();
		ScrabbleAIPlayerTest();
	}

	static void ScrabbleSolverTest()
	{
		var board = new TileBoard();
		var firstWord = new WordPlacement("HI", new Coordinates(0, 14), Direction.Horizontal);
		board.PlaceWord(firstWord);
		Console.WriteLine(board);

		var rack = new TileRack();
		var letters = new char[] { 'A', 'B', 'C', 'D' };

		foreach (var letter in letters)
		{
			rack.Add(new Tile(letter));
		}

		var gameState = new GameState(board, rack);

		using var solver = new SolverInterop();

		var results = solver.Solve(gameState);

		foreach (var result in results)
		{
			Console.WriteLine(result);
		}
	}

	static void ScrabbleAIPlayerTest()
	{
		var player = new AIPlayer("AI", AIDifficulty.Medium);
		var board = new TileBoard();
		var pouch = new Pouch();

		var skippedLastTurn = false;
		for (var i = 0; i < 100; i++)
		{
			// The AI does this automatically, but we need to do it manually so we can see it before the AI plays.
			player.DrawFullRack(pouch);

			Console.WriteLine(" === Turn " + i + " === ");
			Console.WriteLine($"Score: {player.Score}");
			Console.WriteLine();
			Console.WriteLine("Board:");
			Console.WriteLine(board);
			Console.WriteLine();
			Console.WriteLine("Rack:");
			Console.WriteLine(player.Rack);
			Console.WriteLine();

			var skippedTurn = player.PlayTurn(board, pouch);

			if (skippedTurn && skippedLastTurn)
			{
				Console.WriteLine("AI skipped their turn 2 times in a row. The game is over.");
				Console.WriteLine($"Final score: {player.Score}");
				break;
			}
			skippedLastTurn = skippedTurn;
			Console.ReadLine();
		}
	}
}
