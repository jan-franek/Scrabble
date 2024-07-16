using ScrabbleCore.Enums;
using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

/// <summary>
/// Represents a game of Scrabble.
/// </summary>
public class Game
{
	public Game(AIDifficulty difficulty)
	{
		Players = new List<Player>(2);
		Board = new TileBoard();
		Pouch = new Pouch();

		Initialize(difficulty);
	}

	public List<Player> Players { get; private set; }
	public TileBoard Board { get; private set; }
	public Pouch Pouch { get; private set; }

	private void Initialize(AIDifficulty difficulty)
	{
		Players.Add(new HumanPlayer("Player 1"));
		Players.Add(new AIPlayer("Player 2", difficulty));

		foreach (var player in Players) player.DrawFullRack(Pouch);
	}
}
