﻿using ScrabbleCore.Enums;

namespace ScrabbleCore.Structs;

public struct Pouch
{
	private readonly List<Tile> letters;
	private static readonly Random random = new();

	public readonly int Count => letters.Count;
	public readonly bool IsEmpty => letters.Count == 0;

	public Pouch()
	{
		letters = new List<Tile>(100);
		Initialize();
	}

	/// <summary>
	/// Draws a random tile from the pouch.
	/// Check if the pouch is empty before calling this method.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when you try to draw a tile from an empty pouch.</exception>
	public Tile Draw()
	{
		if (letters.Count == 0)
		{
			throw new InvalidOperationException("The pouch is empty.");
		}

		int index = random.Next(letters.Count);
		Tile tile = letters[index];
		letters.RemoveAt(index);
		return tile;
	}

	public void Add(Tile tile)
	{
		if (tile.Type == TileType.Empty)
		{
			throw new ArgumentException("Cannot add an empty tile.", nameof(tile));
		}

		letters.Add(tile);
	}

	private void Initialize()
	{
		{
			char c = 'A';
			for (int i = 0; i < Defaults.NumberOfLetters; i++, c++)
			{
				int count = Defaults.PouchLetterCounts[i];

				for (int j = 0; j < count; j++)
				{
					letters.Add(Tile.GetLetter(c));
				}
			}
		}

		for (int i = 0; i < 2; i++)
		{
			letters.Add(Tile.Blank);
		}
	}
}