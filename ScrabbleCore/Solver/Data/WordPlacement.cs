using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver.Data;

/// <summary>
/// Represents a word placement.
/// Mirrors the WordPlacement struct in scrabble_solver.
/// </summary>
public readonly struct WordPlacement(string word, Coordinates startTile, Direction direction)
{
  [JsonPropertyName("dir")]
  public Direction Direction { get; init; } = direction;

  [JsonPropertyName("start")]
  public Coordinates StartTile { get; init; } = startTile;

  [JsonPropertyName("word")]
  public string Word { get; init; } = word;
}
