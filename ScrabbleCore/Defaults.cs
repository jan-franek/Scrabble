using ScrabbleCore.Enums;
using ScrabbleCore.Structs;

namespace ScrabbleCore;

public static class Defaults
{
	public const int NumberOfLetters = 'Z' - 'A' + 1;

	// We need to enforce the size of the array by the constant.
#pragma warning disable IDE0300 // Simplify collection initialization
	public static readonly int[] LetterValues = new int[NumberOfLetters]
	{
		1, 3, 3, 2, 1, 4, 2, 4, 1, 8, 5, 1, 3, 1, 1, 3, 10, 1, 1, 1, 1, 4, 4, 8, 4, 10
	};

	public static readonly int[] PouchLetterCounts = new int[NumberOfLetters]
	{
		9, 2, 2, 4, 12, 2, 3, 2, 9, 1, 1, 4, 2, 6, 8, 2, 1, 6, 4, 6, 4, 2, 2, 1, 2, 1
	};
#pragma warning restore IDE0300 // Simplify collection initialization

	public static readonly (int x, int y) StartTile = (7, 7);

	public static readonly Board<CellType> BoardCells = new(new CellType[Board<CellType>.Height, Board<CellType>.Width]
	{
			{ CellType.TripleWord, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.TripleWord },
			{ CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal },
			{ CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal },
			{ CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.DoubleLetter },
			{ CellType.Normal, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.Normal },
			{ CellType.Normal, CellType.TripleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleLetter, CellType.Normal },
			{ CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal },
			{ CellType.TripleWord, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.TripleWord },
			{ CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal },
			{ CellType.Normal, CellType.TripleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleLetter, CellType.Normal },
			{ CellType.Normal, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.Normal },
			{ CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.DoubleLetter },
			{ CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal },
			{ CellType.Normal, CellType.DoubleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleWord, CellType.Normal },
			{ CellType.TripleWord, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.Normal, CellType.TripleWord, CellType.Normal, CellType.Normal, CellType.Normal, CellType.DoubleLetter, CellType.Normal, CellType.Normal, CellType.TripleWord }
	});
}
