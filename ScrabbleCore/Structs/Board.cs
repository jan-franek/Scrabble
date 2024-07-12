namespace ScrabbleCore.Structs;

public struct Board<T>
{
	private readonly T?[,] board;

	public const int Height = 15;
	public const int Width = 15;

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

	public T? this[int y, int x]
	{
		readonly get => board[y, x];
		set => board[y, x] = value;
	}

	private void Initialize(T? defaultValue = default)
	{
		for (int y = 0; y < Height; y++)
		{
			for (int x = 0; x < Width; x++)
			{
				board[y, x] = defaultValue;
			}
		}
	}
}