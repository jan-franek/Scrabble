using ScrabbleCore.Classes;
using ScrabbleCore.Solver;
using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;

namespace ScrabbleTesting;

internal class Program
{
	static void Main()
	{
		var board = new Board<Tile>(Tile.Empty);

		var rack = new TileRack();
		var letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

		foreach (var letter in letters)
		{
			rack.Add(Tile.GetLetter(letter));
		}

		var gameState = new GameState(board, rack);

		using var solver = new SolverInterop();

		var results = solver.Solve(gameState);

		foreach (var result in results)
		{
			Console.WriteLine(result);
		}
	}
}
