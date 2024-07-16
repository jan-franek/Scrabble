using ScrabbleCore.Enums;
using ScrabbleCore.Solver;
using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

/// <summary>
/// Represents an AI player.
/// </summary>
/// <param name="name"> The name of the player. </param>
/// <param name="difficulty"> The difficulty of the AI player. </param>
public sealed class AIPlayer(string name, AIDifficulty difficulty) : Player(name)
{
	private readonly SolverInterop solver = new();
	private readonly Random random = new();

	public AIDifficulty Difficulty { get; init; } = difficulty;

	/// <summary>
	/// Plays a turn for the AI player.
	/// </summary>
	/// <param name="board"> The game board. </param>
	/// <param name="pouch"> The pouch of tiles. </param>
	/// <returns> True if the AI player skipped the turn, false otherwise. </returns>
	public bool PlayTurn(TileBoard board, Pouch pouch)
	{
		DrawFullRack(pouch);

		var gameState = new GameState(board, Rack);

		var results = solver.Solve(gameState);

		if (results.Count == 0)
		{
			SkipMove(pouch);
			return true;
		}

		var move = ChooseMove(results);

		PlayMove(move, board);
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

			default:
				throw new InvalidOperationException("Invalid AI difficulty.");
		}

		return results[index];
	}

	/// <summary>
	/// Either skip the move or exchange letters from the pouch if there are still letters in the pouch.
	/// </summary>
	private void SkipMove(Pouch pouch)
	{
		if (pouch.IsEmpty) return;

		var numberOfLettersToExchange = Math.Min(Rack.Count, pouch.Count);

		var lettersToExchange = new List<Tile>(numberOfLettersToExchange);

		for (int i = 0; i < numberOfLettersToExchange; i++)
		{
			var tile = Rack.rack.First(t => t != Tile.Empty);
			Rack.Remove(tile);
			lettersToExchange.Add(tile);
		}

		DrawFullRack(pouch);

		foreach (var letter in lettersToExchange)
		{
			pouch.Add(letter);
		}
	}
}
