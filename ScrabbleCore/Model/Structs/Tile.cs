using ScrabbleCore.Enums;

namespace ScrabbleCore.Structs;

/// <summary>
/// Represents a tile in the game.
/// </summary>
public readonly struct Tile : IEquatable<Tile>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Tile"/> struct.
	/// Only use this constructor for letters.
	/// For empty tiles, use <see cref="Empty"/>.
	/// For blank tiles, use <see cref="Blank"/>.
	/// </summary>
	/// <param name="letter"> Uppercase letter to represent the tile. </param>
	/// <exception cref="ArgumentOutOfRangeException"> Thrown when the letter is not an uppercase letter. </exception>
	public Tile(char letter)
	{
		if (letter is < 'A' or > 'Z')
		{
			throw new ArgumentOutOfRangeException(nameof(letter), "Letter must be an uppercase letter.");
		}

		Letter = letter;
		Type = TileType.Letter;
	}

	public override string ToString() => Letter.ToString();

	private Tile(char letter, TileType type)
	{
		Letter = letter;
		Type = type;
	}

	public char Letter { get; init; }
	public TileType Type { get; init; }
	public int Value
	{
		get
		{
			if (Type is TileType.Empty or TileType.Blank) return 0;

			return Defaults.LetterValues[Letter - 'A'];
		}
	}

	public static Tile Empty => new(' ', TileType.Empty);
	public static Tile Blank => new('*', TileType.Blank);

	#region Equality Members
	public bool Equals(Tile other) => Type == other.Type && Letter == other.Letter;

	public override bool Equals(object? obj) => obj is Tile tile && Equals(tile);

	public override int GetHashCode() => HashCode.Combine(Letter, Type);

	public static bool operator ==(Tile left, Tile right) => left.Equals(right);

	public static bool operator !=(Tile left, Tile right) => !(left == right);
	#endregion
}