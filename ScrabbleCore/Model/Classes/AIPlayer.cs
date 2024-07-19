using ScrabbleCore.Enums;
using ScrabbleCore.Solver.Data;

namespace ScrabbleCore.Classes;

/// <summary>
/// Represents an AI player.
/// </summary>
/// <param name="name"> The name of the player. </param>
/// <param name="difficulty"> The difficulty of the AI player. </param>
public sealed class AIPlayer(string name, AIDifficulty difficulty) : Player(name)
{
	private readonly Random random = new();

	public AIDifficulty Difficulty { get; init; } = difficulty;

	/// <summary>
	/// Plays a turn for the AI player.
	/// </summary>
	/// <param name="board"> The game board. </param>
	/// <param name="pouch"> The pouch of tiles. </param>
	/// <returns> True if the player skipped the turn, false otherwise. </returns>
	public bool PlayTurn(TileBoard board, Pouch pouch)
	{
		var gameState = new GameState(board, Rack);

		var results = solver.Solve(gameState);

		if (results.Count == 0)
		{
			SkipMove(pouch);
			return true;
		}

		var move = ChooseMove(results);

		PlayMove(move, board);

		DrawFullRack(pouch);

		return false;
	}

	/// <summary>
	/// Chooses a move based on the AI difficulty.
	/// </summary>
	/// <param name="results"> The list of possible moves. </param>
	/// <returns> The chosen move. </returns>
	/// <exception cref="InvalidOperationException"> Thrown when there are no moves available. </exception>
	private WordPlay ChooseMove(List<WordPlay> results)
	{
		if (results.Count == 0) throw new InvalidOperationException("No moves available.");

		var index = Difficulty switch
		{
			// bottom 50%
			AIDifficulty.Easy => random.Next(results.Count / 2, results.Count),
			// top 50%
			AIDifficulty.Medium => random.Next(0, results.Count / 2),
			// top 20%
			AIDifficulty.Hard => random.Next(0, results.Count / 5),
			_ => throw new InvalidOperationException("Invalid AI difficulty."),
		};

		return results[index];
	}
}
