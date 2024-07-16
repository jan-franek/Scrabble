using ScrabbleCore.Enums;
using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

public class Game
{
	public List<Player> Players { get; private set; }
	public Board<Tile> Board { get; private set; }
	public Pouch Pouch { get; private set; }

	public Game(AIDifficulty difficulty)
	{
		Players = new List<Player>(2);
		Board = new Board<Tile>(Tile.Empty);
		Pouch = new Pouch();

		Initialize(difficulty);
	}

	private void Initialize(AIDifficulty difficulty)
	{
		Players.Add(new HumanPlayer("Player 1"));
		Players.Add(new AIPlayer("Player 2", difficulty));

		foreach (var player in Players) player.DrawFullRack(Pouch);
	}
}
