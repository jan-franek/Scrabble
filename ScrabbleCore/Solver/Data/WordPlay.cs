using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver.Data;

public readonly struct WordPlay(WordPlacement wordPlacement, IReadOnlyList<int> blankPositions, int score)
{

  [JsonPropertyName("blanks")]
  public IReadOnlyList<int> BlankPositions { get; init; } = blankPositions;

  [JsonPropertyName("score")]
  public int Score { get; init; } = score;

  [JsonPropertyName("wp")]
  public WordPlacement WordPlacement { get; init; } = wordPlacement;
}