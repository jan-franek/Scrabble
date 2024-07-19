using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver.Data;

/// <summary>
/// Represents a word placement.
/// Mirrors the WordPlacement struct in scrabble_solver.
/// </summary>
public readonly struct WordPlacement(string word, Coordinates startTile, Direction direction) : IEquatable<WordPlacement>
{
	[JsonPropertyName("dir")]
	public Direction Direction { get; init; } = direction;

	[JsonPropertyName("start")]
	public Coordinates StartTile { get; init; } = startTile;

	[JsonPropertyName("word")]
	public string Word { get; init; } = word;

	#region Equality Members
	public bool Equals(WordPlacement other)
	{
		return Word == other.Word && StartTile == other.StartTile && Direction == other.Direction;
	}

	public override bool Equals(object? obj) => obj is WordPlacement wp && Equals(wp);

	public override int GetHashCode() => HashCode.Combine(Direction, StartTile, Word);

	public static bool operator ==(WordPlacement left, WordPlacement right) => left.Equals(right);

	public static bool operator !=(WordPlacement left, WordPlacement right) => !(left == right);
	#endregion
}
