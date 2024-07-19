using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;
using System.Text;

namespace ScrabbleCore.Classes;

/// <summary>
/// Represents a human player.
/// </summary>
public sealed class HumanPlayer : Player
{
	/// <summary>
	/// It's possible to place blanks on temporary board.
	/// </summary>
	public TileBoard TemporaryBoard { get; init; }

	/// <param name="name"> The name of the player. </param>
	public HumanPlayer(string name) : base(name)
	{
		TemporaryBoard = new TileBoard();

		TemporaryBoard.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(TemporaryBoard));
	}

	/// <summary>
	/// Places a tile from the player's rack onto the temporary board.
	/// </summary>
	/// <param name="position"> The position to place the tile. </param>
	/// <param name="tileIndex"> The index of the tile in the player's rack. </param>
	/// <returns> True if the tile was placed successfully, false otherwise. </returns>
	public bool PlaceTemporaryTile(Coordinates position, int tileIndex)
	{
		if (!TemporaryBoard.CanPlaceTile(position, Rack[tileIndex])) return false;

		if (Rack[tileIndex] == Tile.Empty) return false;

		TemporaryBoard[position] = Rack[tileIndex];
		Rack[tileIndex] = Tile.Empty;
		return true;
	}

	/// <summary>
	/// Removes a tile from the temporary board and returns it back to the player's rack.
	/// </summary>
	/// <param name="tilePosition"> The position of the tile to remove. </param>
	/// <returns> True if the tile was removed successfully, false otherwise. </returns>
	public bool RemoveTemporaryTile(Coordinates tilePosition)
	{
		if (TemporaryBoard[tilePosition] == Tile.Empty) return false;

		Rack.Add(TemporaryBoard[tilePosition]);
		TemporaryBoard[tilePosition] = Tile.Empty;
		return true;
	}

	/// <summary>
	/// Tries to play a turn for the human player.
	/// </summary>
	/// <param name="board"> The game board. </param>
	/// <param name="pouch"> The pouch of tiles. </param>
	/// <returns> True if the turn was played successfully, false otherwise. </returns>
	public (bool success, bool skipped) TryPlayTurn(TileBoard board, Pouch pouch)
	{
		var (success, noTiles) = TryGetWordPlacement(board, out var wordPlacement);

		if (!success) return (false, false);

		if (noTiles)
		{
			SkipMove(pouch);
			return (true, true);
		}

		// Make a temporary rack with all player's tiles for the solver
		var tempRack = GetFullTempRack();

		var gameState = new GameState(board, tempRack);

		var possibleMoves = solver.Solve(gameState);

		if (possibleMoves.Count == 0) return (false, false);

		WordPlay wordPlay;

		// If there are blanks in the wordPlacement, we need to check all possible moves
		if (wordPlacement.Word.Contains(Tile.Blank.Letter))
		{
			var possibleWordPlacements = new List<WordPlacement>();

			var blankIndex = wordPlacement.Word.IndexOf(Tile.Blank.Letter);

			for (var c = 'A'; c <= 'Z'; c++)
			{
				var word = wordPlacement.Word.ToCharArray();
				word[blankIndex] = c;

				var newWordPlacement = new WordPlacement(new string(word), wordPlacement.StartTile, wordPlacement.Direction);

				possibleWordPlacements.Add(newWordPlacement);
			}

			// Two blanks in the wordPlacement
			if (possibleWordPlacements[0].Word.Contains(Tile.Blank.Letter))
			{
				var tempWordPlacements = new List<WordPlacement>();

				foreach (var wp in possibleWordPlacements)
				{
					var blankIndex2 = wp.Word.IndexOf(Tile.Blank.Letter);

					for (var c = 'A'; c <= 'Z'; c++)
					{
						var word = wp.Word.ToCharArray();
						word[blankIndex2] = c;

						var newWordPlacement = new WordPlacement(new string(word), wp.StartTile, wp.Direction);

						tempWordPlacements.Add(newWordPlacement);
					}
				}

				possibleWordPlacements = tempWordPlacements;
			}

			var validMoves = new List<WordPlay>();

			foreach (var wp in possibleWordPlacements)
			{
				var move = possibleMoves.FirstOrDefault(m => m.WordPlacement == wp);

				if (!move.Equals(default(WordPlay))) validMoves.Add(move);
			}

			// Get the move with the biggest score
			wordPlay = validMoves.OrderByDescending(m => m.Score).FirstOrDefault();
		}
		else // No blanks in the wordPlacement
		{
			// Find the move that matches the wordPlacement
			wordPlay = possibleMoves.FirstOrDefault(m => m.WordPlacement == wordPlacement);
		}

		if (wordPlay.Equals(default(WordPlay))) return (false, false);

		PlayMove(wordPlay, board);
		TemporaryBoard.Clear();
		AddScore(wordPlay.Score);
		DrawFullRack(pouch);

		return (true, false);
	}

	/// <summary>
	/// Tries to get the word placement from the temporary board.
	/// </summary>
	/// <param name="baseBoard"> The base board. </param>
	/// <param name="wordPlacement"> The word placement. </param>
	/// <returns> True if the word placement was found, false otherwise. </returns>
	public (bool success, bool noTiles) TryGetWordPlacement(TileBoard baseBoard, out WordPlacement wordPlacement)
	{
		wordPlacement = default;

		var placedTiles = new List<Coordinates>();

		// Identify the coordinates of all newly placed tiles
		for (var y = 0; y < TileBoard.Height; y++)
		{
			for (var x = 0; x < TileBoard.Width; x++)
			{
				if (TemporaryBoard[y, x] != Tile.Empty)
				{
					if (baseBoard[y, x] != Tile.Empty) throw new InvalidOperationException("Tile placed on an existing tile.");

					placedTiles.Add(new Coordinates(x, y));
				}
			}
		}

		if (!placedTiles.Any()) return (true, true); // No tiles placed (skipped turn)

		// Just one tile placed (can't determine the direction)
		if (placedTiles.Count == 1)
		{
			var tile = TemporaryBoard[placedTiles[0]];

			// Try get both directions
			var horizontalWord = GetWordFromDirection(baseBoard, placedTiles[0], placedTiles[0], Direction.Horizontal);
			var verticalWord = GetWordFromDirection(baseBoard, placedTiles[0], placedTiles[0], Direction.Vertical);

			if (horizontalWord.Word.Length == 1 && verticalWord.Word.Length == 1)
			{
				return (false, false); // No word found
			}

			wordPlacement = horizontalWord.Word.Length > 1 ? horizontalWord : verticalWord;
		}

		// Get the furthest placed tiles
		Coordinates start;
		Coordinates end;
		{
			var oderedTiles = placedTiles.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();
			start = oderedTiles.First();
			end = oderedTiles.Last();
		}

		// Compare all tiles with the start tile
		foreach (var tile in placedTiles)
		{
			if (!(tile.X == start.X || tile.Y == start.Y))
			{
				return (false, false); // Tiles are not in a straight line
			}
		}

		// Determine the direction of the word
		var direction = start.X == end.X ? Direction.Vertical : Direction.Horizontal;

		// Check that there are no gaps
		if (direction == Direction.Horizontal)
		{
			for (var x = start.X; x <= end.X; x++)
			{
				if (TemporaryBoard[start.Y, x] == Tile.Empty)
				{
					if (baseBoard[start.Y, x] == Tile.Empty)
					{
						return (false, false); // Gap in the word
					}
				}
			}
		}
		else
		{
			for (var y = start.Y; y <= end.Y; y++)
			{
				if (TemporaryBoard[y, start.X] == Tile.Empty)
				{
					if (baseBoard[y, start.X] == Tile.Empty)
					{
						return (false, false); // Gap in the word
					}
				}
			}
		}

		// Construct the word from newly placed tiles and tiles already on the board
		wordPlacement = GetWordFromDirection(baseBoard, start, end, direction);

		return (true, false);
	}

	private WordPlacement GetWordFromDirection(TileBoard baseBoard, Coordinates start, Coordinates end, Direction direction)
	{
		var dx = direction == Direction.Horizontal ? 1 : 0;
		var dy = direction == Direction.Vertical ? 1 : 0;
		var startX = (int)start.X;
		var startY = (int)start.Y;

		// Extend start position to the beginning of the word
		while (startX - dx >= 0 && startY - dy >= 0 && (baseBoard[startY - dy, startX - dx] != Tile.Empty || TemporaryBoard[startY - dy, startX - dx] != Tile.Empty))
		{
			startX -= dx;
			startY -= dy;
		}

		var endX = (int)end.X;
		var endY = (int)end.Y;

		// Extend end position to the end of the word
		while (endX + dx < TileBoard.Width && endY + dy < TileBoard.Height && (baseBoard[endY + dy, endX + dx] != Tile.Empty || TemporaryBoard[endY + dy, endX + dx] != Tile.Empty))
		{
			endX += dx;
			endY += dy;
		}

		// Construct the word from the extended range
		var word = new StringBuilder();
		for (int x = startX, y = startY; x <= endX && y <= endY; x += dx, y += dy)
		{
			var tile = TemporaryBoard[y, x] != Tile.Empty ? TemporaryBoard[y, x] : baseBoard[y, x];
			word.Append(tile.Letter);
		}

		return new WordPlacement(word.ToString(), new Coordinates(startX, startY), direction);
	}

	private TileRack GetFullTempRack()
	{
		var tempRack = new TileRack();

		foreach (var tile in Rack)
		{
			tempRack.Add(tile);
		}

		for (var y = 0; y < TileBoard.Height; y++)
		{
			for (var x = 0; x < TileBoard.Width; x++)
			{
				if (TemporaryBoard[y, x] != Tile.Empty)
				{
					tempRack.Add(TemporaryBoard[y, x]);
				}
			}
		}

		return tempRack;
	}
}
