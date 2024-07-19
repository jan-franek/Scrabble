using ScrabbleCore.Classes;
using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver.Data;

/// <summary>
/// Represents a set of coordinates on the board.
/// Mirrors the Coordinates struct in scrabble_solver.
///
/// The coordinates starts at (0, 0) in the top-left corner.
/// </summary>
public readonly struct Coordinates : IEquatable<Coordinates>
{
	public Coordinates(uint x, uint y)
	{
		if (x >= TileBoard.Width) throw new ArgumentOutOfRangeException(nameof(x), $"The {nameof(x)} coordinate is out of bounds.");
		if (y >= TileBoard.Height) throw new ArgumentOutOfRangeException(nameof(y), $"The {nameof(y)} coordinate is out of bounds.");

		X = x;
		Y = y;
	}

	public Coordinates(int x, int y)
	{
		if (x is < 0 or >= TileBoard.Width) throw new ArgumentOutOfRangeException(nameof(x), $"The {nameof(x)} coordinate is out of bounds.");
		if (y is < 0 or >= TileBoard.Height) throw new ArgumentOutOfRangeException(nameof(y), $"The {nameof(y)} coordinate is out of bounds.");

		X = (uint)x;
		Y = (uint)y;
	}

	[JsonPropertyName("x")]
	public uint X { get; init; }

	[JsonPropertyName("y")]
	public uint Y { get; init; }

	#region Equality Members
	public bool Equals(Coordinates other)
	{
		return X == other.X && Y == other.Y;
	}
	public override bool Equals(object? obj) => obj is Coordinates wp && Equals(wp);

	public override int GetHashCode() => HashCode.Combine(X, Y);

	public static bool operator ==(Coordinates left, Coordinates right) => left.Equals(right);

	public static bool operator !=(Coordinates left, Coordinates right) => !(left == right);
	#endregion

}
