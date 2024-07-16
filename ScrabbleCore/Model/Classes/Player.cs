using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

public class Player(string name) : IPlayer
{
	public string Name { get; init; } = name;
	public int Score { get; private set; } = 0;
	public TileRack Rack { get; init; } = new();

	public bool DrawTile(Pouch pouch)
	{
		if (Rack.IsFull || pouch.IsEmpty) return false;

		var tile = pouch.Draw();

		Rack.Add(tile);
		return true;
	}

	public void DrawFullRack(Pouch pouch)
	{
		while (!Rack.IsFull)
		{
			if (!DrawTile(pouch)) break;
		}
	}

	public void ReturnTile(Pouch pouch, Tile tile)
	{
		if (tile == Tile.Empty) return;

		if (!Rack.Remove(tile)) return;

		pouch.Add(tile);
	}

	public void AddScore(int score)
	{
		Score += score;
	}
}
