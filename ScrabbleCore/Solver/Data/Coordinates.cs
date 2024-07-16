using ScrabbleCore.Classes;
using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver.Data;

/// <summary>
/// Represents a set of coordinates on the board.
/// Mirrors the Coordinates struct in scrabble_solver.
///
/// The coordinates starts at (0, 0) in the top-left corner.
/// </summary>
public readonly struct Coordinates
{
	public Coordinates(uint x, uint y)
	{
		if (x >= TileBoard.Width) throw new ArgumentOutOfRangeException(nameof(x), $"The {nameof(x)} coordinate is out of bounds.");
		if (y >= TileBoard.Height) throw new ArgumentOutOfRangeException(nameof(y), $"The {nameof(y)} coordinate is out of bounds.");

		X = x;
		Y = y;
	}

	[JsonPropertyName("x")]
  public uint X { get; init; }

  [JsonPropertyName("y")]
  public uint Y { get; init; }

}
