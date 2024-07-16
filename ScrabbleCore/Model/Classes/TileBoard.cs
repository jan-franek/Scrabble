using System.Text;

using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

/// <summary>
/// Represents the game board filled with tiles.
/// </summary>
public sealed class TileBoard : Board<Tile>
{
	public TileBoard() : base(Tile.Empty) { }

	/// <summary>
	/// Tries to place a word on the board.
	/// </summary>
	/// <param name="wordPlacement"> The word to place. </param>
	/// <returns> True if the word was placed successfully, false otherwise. </returns>
	public bool PlaceWord(WordPlacement wordPlacement)
	{
		if (!CanPlaceWord(wordPlacement)) return false;

		for (var i = 0; i < wordPlacement.Word.Length; i++)
		{
			var letter = wordPlacement.Word[i];
			var tile = new Tile(letter);

			if (wordPlacement.Direction is Direction.Horizontal)
			{
				this[(int)wordPlacement.StartTile.Y, (int)wordPlacement.StartTile.X + i] = tile;
			}
			else
			{
				this[(int)wordPlacement.StartTile.Y + i, (int)wordPlacement.StartTile.X] = tile;
			}
		}

		return true;
	}

	public override string ToString()
	{
		var sb = new StringBuilder();

		sb.Append('+');
		for (int i = 0; i < Width; i++) sb.Append('-');
		sb.Append('+');
		sb.AppendLine();

		for (int y = 0; y < Height; y++)
		{
			sb.Append('|');
			for (int x = 0; x < Width; x++) sb.Append(this[y, x]);
			sb.Append('|');
			sb.AppendLine();
		}

		sb.Append('+');
		for (int i = 0; i < Width; i++) sb.Append('-');
		sb.Append('+');

		return sb.ToString();
	}

	/// <summary>
	/// Checks if a word can be placed on the board.
	/// Checks if the word fits on the board and if it doesn't overlap with different letters.
	/// </summary>
	/// <param name="wordPlacement"> The word to place. </param>
	/// <returns> True if the word can be placed, false otherwise. </returns>
	private bool CanPlaceWord(WordPlacement wordPlacement)
	{
		if (wordPlacement.Direction is Direction.Horizontal)
		{
			var endX = wordPlacement.StartTile.X + wordPlacement.Word.Length - 1;
			if (endX >= Width) return false;
		}
		else
		{
			var endY = wordPlacement.StartTile.Y + wordPlacement.Word.Length - 1;
			if (endY >= Height) return false;
		}

		for (int i = 0; i < wordPlacement.Word.Length; i++)
		{
			var newLetter = wordPlacement.Word[i];

			var letterCoords = wordPlacement.Direction is Direction.Horizontal
				? new Coordinates((uint)(wordPlacement.StartTile.X + i), wordPlacement.StartTile.Y)
				: new Coordinates(wordPlacement.StartTile.X, (uint)(wordPlacement.StartTile.Y + i));

			var existingLetter = this[(int)letterCoords.Y, (int)letterCoords.X];

			if (existingLetter != Tile.Empty && existingLetter.Letter != newLetter) return false;
		}

		return true;
	}
}
