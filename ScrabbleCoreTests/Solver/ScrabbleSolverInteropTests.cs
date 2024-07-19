using ScrabbleCore.Classes;
using ScrabbleCore.Solver;
using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;

namespace ScrabbleCoreTests.Solver
{
	public class SolverInteropTests
	{
		[Fact]
		public void SolverInterop_CreatesInstance_Successfully()
		{
			IntPtr solverPtr = IntPtr.Zero;

			try
			{
				solverPtr = SolverInterop.CreateSolver(SolverInterop.dictionaryPath);

				Assert.NotEqual(IntPtr.Zero, solverPtr);
			}
			finally
			{
				SolverInterop.DeleteSolver(solverPtr);
			}
		}

		[Fact]
		public void SolverInterop_Solve_ReturnsValidResults()
		{
			var board = new TileBoard();
			var firstWord = new WordPlacement("HELLO", new Coordinates(7, 7), Direction.Horizontal);
			board.PlaceWord(firstWord);
			Console.WriteLine(board);

			var rack = new TileRack();
			var letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

			foreach (var letter in letters)
			{
				rack.Add(new Tile(letter));
			}

			var gameState = new GameState(board, rack);

			using var solver = new SolverInterop();

			var results = solver.Solve(gameState);

			Assert.NotNull(results);
			Assert.True(results.Count > 0);
		}
	}
}
