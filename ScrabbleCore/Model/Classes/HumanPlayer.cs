namespace ScrabbleCore.Classes;

/// <summary>
/// Represents a human player.
/// </summary>
/// <param name="name"> The name of the player. </param>
public sealed class HumanPlayer(string name) : Player(name)
{
}
