using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

public class Game
{
	public List<IPlayer> Players { get; private set; }
	public Board<Tile> Board { get; private set; }
	public Pouch Pouch { get; private set; }

	public Game()
	{
		Players = new List<IPlayer>(2);
		Board = new Board<Tile>(Tile.Empty);
		Pouch = new Pouch();

		Initialize();
	}

	private void Initialize()
	{
		Players.Add(new HumanPlayer("Player 1"));
		Players.Add(new AIPlayer("Player 2"));

		foreach (var player in Players)
		{
			for (int i = 0; i < Rack<Tile>.Size; i++)
			{
				if (!player.DrawTile(Pouch)) break;
			}
		}
	}
}
