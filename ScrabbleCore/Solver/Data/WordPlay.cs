using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver.Data;

/// <summary>
/// Represents a word play.
/// Mirrors the WordPlay struct in scrabble_solver.
/// </summary>
public readonly struct WordPlay(WordPlacement wordPlacement, IReadOnlyList<int> blankPositions, int score)
{

  [JsonPropertyName("blanks")]
  public IReadOnlyList<int> BlankPositions { get; init; } = blankPositions;

  [JsonPropertyName("score")]
  public int Score { get; init; } = score;

  [JsonPropertyName("wp")]
  public WordPlacement WordPlacement { get; init; } = wordPlacement;

  public override string ToString()
	{
    return
      $"Score: [{Score}], " +
      $"Word: [{WordPlacement.Word}], " +
      $"Placement: [{WordPlacement.StartTile.X}, {WordPlacement.StartTile.Y}] - {WordPlacement.Direction}";
	}
}