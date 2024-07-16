using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

public sealed class TileRack : Rack<Tile>
{
	public int Count => rack.Count(t => t != Tile.Empty);

	public bool IsFull => Count == Size;

	public TileRack() : base(Tile.Empty) { }

	public bool Contains(Tile tile) => rack.Contains(tile);

	public int CountOf(Tile tile) => rack.Count(t => t == tile);

	public bool Remove(Tile tile)
	{
		int index = Array.IndexOf(rack, tile);
		if (index == -1) return false;

		rack[index] = Tile.Empty;
		return true;
	}

	public bool Add(Tile tile)
	{
		int index = Array.IndexOf(rack, Tile.Empty);
		if (index == -1) return false;

		rack[index] = tile;
		return true;
	}
}
