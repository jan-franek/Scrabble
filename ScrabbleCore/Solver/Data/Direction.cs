using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver.Data;

public enum Direction
{
  [JsonPropertyName("Horizontal")]
  Horizontal,

  [JsonPropertyName("Vertical")]
  Vertical,
}
