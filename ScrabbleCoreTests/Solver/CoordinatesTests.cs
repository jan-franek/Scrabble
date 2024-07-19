using ScrabbleCore.Classes;
using ScrabbleCore.Solver.Data;

namespace ScrabbleCoreTests.Solver
{
	public class CoordinatesTests
	{
		[Theory]
		[InlineData(0, 0)]
		[InlineData(TileBoard.Width - 1, TileBoard.Height - 1)]
		public void Coordinates_CreatesSuccessfully_WithinValidBounds(uint x, uint y)
		{
			// Act
			var coordinates = new Coordinates(x, y);

			// Assert
			Assert.Equal(x, coordinates.X);
			Assert.Equal(y, coordinates.Y);
		}

		[Theory]
		[InlineData(TileBoard.Width, 0)]
		[InlineData(0, TileBoard.Height)]
		public void Coordinates_ThrowsArgumentOutOfRangeException_WhenOutOfBounds(uint x, uint y)
		{
			// Act & Assert
			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinates(x, y));

			// Further assert to check if the correct parameter name triggered the exception
			Assert.True(exception.ParamName is (nameof(x)) or (nameof(y)));
		}
	}
}
