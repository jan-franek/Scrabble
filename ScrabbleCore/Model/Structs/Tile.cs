using ScrabbleCore.Enums;

namespace ScrabbleCore.Structs;

public readonly struct Tile : IEquatable<Tile>
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

	#region Equality Members
	public bool Equals(Tile other) => Type == other.Type && Letter == other.Letter;

	public override bool Equals(object? obj) => obj is Tile tile && Equals(tile);

	public override int GetHashCode() => HashCode.Combine(Letter, Type);

	public static bool operator ==(Tile left, Tile right) => left.Equals(right);

	public static bool operator !=(Tile left, Tile right) => !(left == right);
	#endregion
}