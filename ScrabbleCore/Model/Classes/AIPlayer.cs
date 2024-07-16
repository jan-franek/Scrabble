using ScrabbleCore.Enums;
using ScrabbleCore.Solver;
using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

public sealed class AIPlayer(string name, AIDifficulty difficulty) : Player(name)
{
	public AIDifficulty Difficulty { get; init; } = difficulty;

	private readonly SolverInterop solver = new();
	private readonly Random random = new();

	public void PlayTurn(Board<Tile> board, Pouch pouch)
	{
		DrawFullRack(pouch);

		var gameState = new GameState(board, Rack);

		// TODO: make this async
		var results = solver.Solve(gameState);

		if (results.Count == 0) ; //TODO: skip move

		var move = ChooseMove(results);

		PlayMove(move, board);
	}

	private WordPlay ChooseMove(List<WordPlay> results)
	{
		if (results.Count == 0) throw new InvalidOperationException("No moves available.");

		int index;

		switch (Difficulty)
		{
			case AIDifficulty.Easy: // bottom 50%
				index = random.Next(results.Count / 2, results.Count);
				break;

			case AIDifficulty.Medium: // top 50%
				index = random.Next(0, results.Count / 2);
				break;

			case AIDifficulty.Hard: // top 20%
				index = random.Next(0, results.Count / 5);
				break;

			case AIDifficulty.God: // top result
				return results.First();

			default:
				throw new InvalidOperationException("Invalid AI difficulty.");
		}

		return results[index];
	}

}
