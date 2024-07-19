using ScrabbleCore.Enums;
using ScrabbleCore.Structs;
using System.ComponentModel;

namespace ScrabbleCore.Classes;

/// <summary>
/// Represents a pouch of tiles.
/// </summary>
public class Pouch : INotifyPropertyChanged
{
	private readonly List<Tile> letters;
	private static readonly Random random = new();

	public Pouch()
	{
		letters = new List<Tile>(100);
		Initialize();
	}

	public int Count => letters.Count;
	public bool IsEmpty => letters.Count == 0;

	#region INotifyPropertyChanged
	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	#endregion

	/// <summary>
	/// Draws a random tile from the pouch.
	/// Check if the pouch is empty before calling this method.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when you try to draw a tile from an empty pouch.</exception>
	public Tile Draw()
	{
		if (IsEmpty)
		{
			throw new InvalidOperationException("The pouch is empty.");
		}

		var index = random.Next(letters.Count);
		var tile = letters[index];
		letters.RemoveAt(index);
		OnPropertyChanged(nameof(Count));
		return tile;
	}

	/// <summary>
	/// Adds a tile to the pouch.
	/// </summary>
	/// <param name="tile"> The tile to add. </param>
	/// <exception cref="ArgumentException"> Thrown when you try to add an empty tile. </exception>
	public void Add(Tile tile)
	{
		if (tile.Type == TileType.Empty)
		{
			throw new ArgumentException("Cannot add an empty tile.", nameof(tile));
		}

		letters.Add(tile);
		OnPropertyChanged(nameof(Count));
	}

	private void Initialize()
	{
		{
			var c = 'A';
			for (var i = 0; i < Defaults.NumberOfLetters; i++, c++)
			{
				var count = Defaults.PouchLetterCounts[i];

				for (var j = 0; j < count; j++)
				{
					letters.Add(new Tile(c));
				}
			}
		}

		for (var i = 0; i < 2; i++)
		{
			letters.Add(Tile.Blank);
		}
	}
}