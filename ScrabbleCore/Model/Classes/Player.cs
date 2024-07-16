using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;
using System.Diagnostics.Metrics;

namespace ScrabbleCore.Classes;

/// <summary>
/// Represents a player in the game.
/// </summary>
/// <param name="name"> The name of the player. </param>
public abstract class Player(string name)
{
	public string Name { get; init; } = name;
	public int Score { get; private set; } = 0;
	public TileRack Rack { get; init; } = new();

	/// <summary>
	/// Draws a tile from the pouch and adds it to the player's rack.
	/// </summary>
	/// <param name="pouch"> The pouch to draw from. </param>
	/// <returns> True if the tile was drawn successfully, false otherwise. </returns>
	public bool DrawTile(Pouch pouch)
	{
		if (Rack.IsFull || pouch.IsEmpty) return false;

		var tile = pouch.Draw();

		Rack.Add(tile);
		return true;
	}

	/// <summary>
	/// Draws tiles from the pouch until the rack is full or the pouch is empty.
	/// </summary>
	/// <param name="pouch"> The pouch to draw from. </param>
	public void DrawFullRack(Pouch pouch)
	{
		while (!Rack.IsFull && !pouch.IsEmpty)
		{
			DrawTile(pouch);
		}
	}

	/// <summary>
	/// Returns a tile to the pouch.
	/// </summary>
	/// <param name="pouch"> The pouch to return the tile to. </param>
	/// <param name="tile"> The tile to return. </param>
	public void ReturnTile(Pouch pouch, Tile tile)
	{
		if (tile == Tile.Empty) return;

		if (!Rack.Remove(tile)) return;

		pouch.Add(tile);
	}

	/// <summary>
	/// Adds the given score to the player's total score.
	/// </summary>
	/// <param name="score"> The score to add. </param>
	public void AddScore(int score) => Score += score;

	/// <summary>
	/// Plays a move on the board and updates the player's rack and score.
	/// </summary>
	/// <param name="move"> The move to play. </param>
	/// <param name="board"> The board to play the move on. </param>
	/// <exception cref="InvalidOperationException"> Thrown when the move could not be played. </exception>"
	protected void PlayMove(WordPlay move, TileBoard board)
	{
		var blanksSet = new HashSet<int>(move.BlankPositions);

		for (int i = 0; i < move.WordPlacement.Word.Length; i++)
		{
			if (blanksSet.Contains(i)) Rack.Remove(Tile.Blank);
			else Rack.Remove(new Tile(move.WordPlacement.Word[i]));
		}

		if (!board.PlaceWord(move.WordPlacement)) throw new InvalidOperationException("The move could not be played.");

		AddScore(move.Score);
	}
}
