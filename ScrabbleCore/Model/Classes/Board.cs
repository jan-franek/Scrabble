using ScrabbleCore.Solver.Data;
using System.ComponentModel;

namespace ScrabbleCore.Classes;

/// <summary>
/// Generic class representing a board.
///
/// Keep in mind that changing the value of a cell using the getter will not trigger a PropertyChanged event.
/// </summary>
/// <typeparam name="T"> The type contained in the cells of the board. </typeparam>
public class Board<T> : INotifyPropertyChanged
{
	public const int Height = 15;
	public const int Width = 15;

	#region INotifyPropertyChanged
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	#endregion

	public Board(T? defaultValue = default)
	{
		board = new T[Height, Width];
		Initialize(defaultValue);
	}

	public Board(T?[,] board)
	{
		ArgumentNullException.ThrowIfNull(board, nameof(board));

		if (board.GetLength(0) != Height || board.GetLength(1) != Width)
		{
			throw new ArgumentException($"Input array must be exactly {Height}x{Width} in size.", nameof(board));
		}

		this.board = new T[Height, Width];
		Array.Copy(board, this.board, this.board.Length);
	}

	private readonly T?[,] board;
	public T? this[int y, int x]
	{
		get => board[y, x];
		set
		{
			board[y, x] = value;
			OnPropertyChanged(nameof(board));
		}
	}

	public T? this[uint y, uint x]
	{
		get => board[y, x];
		set
		{
			board[y, x] = value;
			OnPropertyChanged(nameof(board));
		}
	}

	public T? this[Coordinates coordinates]
	{
		get => board[coordinates.Y, coordinates.X];
		set
		{
			board[coordinates.Y, coordinates.X] = value;
			OnPropertyChanged(nameof(board));
		}
	}


	private void Initialize(T? defaultValue = default)
	{
		for (var y = 0; y < Height; y++)
		{
			for (var x = 0; x < Width; x++)
			{
				board[y, x] = defaultValue;
			}
		}
	}
}