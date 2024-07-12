using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

public interface IPlayer
{
	string Name { get; }
	int Score { get; set; }
	Rack<Tile> Rack { get; init; }

	bool DrawTile(Pouch pouch);
}
