using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver.Data;

public readonly struct Coordinates(int x, int y)
{
  [JsonPropertyName("x")]
  public int X { get; init; } = x;

  [JsonPropertyName("y")]
  public int Y { get; init; } = y;
}
