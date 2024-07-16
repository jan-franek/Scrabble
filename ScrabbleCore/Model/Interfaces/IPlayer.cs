using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

//TODO: remove?
public interface IPlayer
{
	string Name { get; }
	int Score { get; }
	TileRack Rack { get; }

	bool DrawTile(Pouch pouch);
	void ReturnTile(Pouch pouch, Tile tile);
	void AddScore(int score);
}
