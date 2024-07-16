using System.Text;

using ScrabbleCore.Structs;

namespace ScrabbleCore.Classes;

/// <summary>
/// Represents a player's tile rack.
/// </summary>
public sealed class TileRack : Rack<Tile>
{
	/// <summary>
	/// The number of non-empty tiles in the rack.
	/// </summary>
	public int Count => rack.Count(t => t != Tile.Empty);
	public bool IsFull => Count == Size;

	public TileRack() : base(Tile.Empty) { }

	/// <summary>
	/// Checks if the rack contains a specific tile.
	/// </summary>
	/// <param name="tile"></param>
	/// <returns></returns>
	public bool Contains(Tile tile) => rack.Contains(tile);

	/// <summary>
	/// Counts the number of occurrences of a specific tile in the rack.
	/// </summary>
	/// <param name="tile"> The tile to count. </param>
	/// <returns> The number of occurrences of the tile. </returns>
	public int CountOf(Tile tile) => rack.Count(t => t == tile);

	/// <summary>
	/// Removes the first occurrence of a tile from the rack.
	/// </summary>
	/// <param name="tile"> The tile to remove. </param>
	/// <returns> True if the tile was removed, false otherwise. </returns>
	public bool Remove(Tile tile)
	{
		int index = Array.IndexOf(rack, tile);
		if (index == -1) return false;

		rack[index] = Tile.Empty;
		return true;
	}

	/// <summary>
	/// Attempts to add a tile to the rack.
	/// </summary>
	/// <param name="tile"> The tile to add. </param>
	/// <returns> True if the tile was added, false otherwise. </returns>
	public bool Add(Tile tile)
	{
		int index = Array.IndexOf(rack, Tile.Empty);
		if (index == -1) return false;

		rack[index] = tile;
		return true;
	}

	public override string ToString()
	{
		var sb = new StringBuilder();

		sb.Append('+');
		for (int i = 0; i < Size; i++) sb.Append("-+");
		sb.AppendLine();

		sb.Append('|' + string.Join('|', rack) + '|');
		sb.AppendLine();

		sb.Append('+');
		for (int i = 0; i < Size; i++) sb.Append("-+");

		return sb.ToString();
	}
}
