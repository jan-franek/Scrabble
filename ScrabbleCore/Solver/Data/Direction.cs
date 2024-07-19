using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver.Data;

/// <summary>
/// Represents a direction.
/// Mirrors the Direction enum in scrabble_solver.
/// </summary>
public enum Direction
{
	[JsonPropertyName("Horizontal")]
	Horizontal,

	[JsonPropertyName("Vertical")]
	Vertical,
}
