using ScrabbleCore.Classes;
using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;

namespace ScrabbleCoreTests.Model
{
	public class TileBoardTests
	{
		[Fact]
		public void Board_Initializes_WithEmptyTiles()
		{
			var board = new TileBoard();
			for (int y = 0; y < TileBoard.Height; y++)
			{
				for (int x = 0; x < TileBoard.Width; x++)
				{
					Assert.Equal(Tile.Empty, board[y, x]);
				}
			}
		}

		[Theory]
		[InlineData("HELLO", 7, 7, Direction.Horizontal, true)]
		[InlineData("WORLD", 7, 7, Direction.Vertical, true)]
		public void PlaceWord_SuccessfulPlacement(string word, uint startX, uint startY, Direction direction, bool expectedSuccess)
		{
			var board = new TileBoard();
			var placement = new WordPlacement(word, new Coordinates(startX, startY), direction);
			bool result = board.PlaceWord(placement);
			Assert.Equal(expectedSuccess, result);
		}

		[Theory]
		[InlineData("SIXTEENLETTERSAA", Direction.Vertical)]
		[InlineData("SIXTEENLETTERSAA", Direction.Horizontal)]
		[InlineData("TOOLONGWORDTHATDOESNOTFIT", Direction.Vertical)]
		[InlineData("TOOLONGWORDTHATDOESNOTFIT", Direction.Horizontal)]
		public void PlaceWord_FailsWhenWordDoesNotFit(string word, Direction direction)
		{
			var board = new TileBoard();
			var placement = new WordPlacement(word, new Coordinates(0, 0), direction);
			bool result = board.PlaceWord(placement);
			Assert.False(result);
		}

		[Fact]
		public void PlaceWord_FailsOnIncorrectOverlap()
		{
			var board = new TileBoard();
			board.PlaceWord(new WordPlacement("HELLO", new Coordinates(0, 0), Direction.Horizontal));

			// Attempt to place "OH" vertically, should overlap incorrectly with "HELLO"
			var placement = new WordPlacement("OH", new Coordinates(1, 0), Direction.Vertical);
			bool result = board.PlaceWord(placement);
			Assert.False(result);
		}
	}
}
