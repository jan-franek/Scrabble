using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

public class AIPlayer(string name) : IPlayer
{
	public string Name { get; } = name;

	public int Score { get; set; } = 0;
	public Rack<Tile> Rack { get; init; } = new Rack<Tile>(Tile.Empty);

	public bool DrawTile(Pouch pouch)
	{
		throw new NotImplementedException();
	}
}
