using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver.Data;

public readonly struct WordPlacement(string word, Coordinates startTile, Direction direction)
{
  [JsonPropertyName("dir")]
  public Direction Direction { get; init; } = direction;

  [JsonPropertyName("start")]
  public Coordinates StartTile { get; init; } = startTile;

  [JsonPropertyName("word")]
  public string Word { get; init; } = word;
}
