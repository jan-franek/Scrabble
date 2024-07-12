using ScrabbleCore.Enums;

namespace ScrabbleCore.Structs;

public readonly struct Tile
{
	public char Letter { get; }
	public TileType Type { get; }

	private Tile(char letter, TileType type)
	{
		Letter = letter;
		Type = type;
	}

	#region Static Members
	public static Tile Empty => GetEmpty();
	public static Tile Blank => GetBlank();

	public static Tile GetLetter(char letter)
	{
		if (letter is < 'A' or > 'Z')
		{
			throw new ArgumentOutOfRangeException(nameof(letter), "Letter must be an uppercase letter.");
		}
		return new(letter, TileType.Letter);
	}

	private static Tile GetBlank() => new('*', TileType.Blank);
	private static Tile GetEmpty() => new(' ', TileType.Empty);
	#endregion
}