using ScrabbleCore;
using ScrabbleCore.Enums;
using ScrabbleCore.Structs;

namespace ScrabbleCoreTests.Model
{
	public class TileTests
	{
		[Theory]
		[InlineData('A')]
		[InlineData('Z')]
		public void Constructor_ValidLetter_CreatesTile(char letter)
		{
			var tile = new Tile(letter);
			Assert.Equal(letter, tile.Letter);
			Assert.Equal(TileType.Letter, tile.Type);
		}

		[Theory]
		[InlineData('a')]
		[InlineData('*')]
		[InlineData(' ')]
		[InlineData('1')]
		public void Constructor_InvalidLetter_Throws(char letter)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new Tile(letter));
		}

		[Fact]
		public void EmptyTile_CorrectProperties()
		{
			var tile = Tile.Empty;
			Assert.Equal(' ', tile.Letter);
			Assert.Equal(TileType.Empty, tile.Type);
			Assert.Equal(0, tile.Value);
		}

		[Fact]
		public void BlankTile_CorrectProperties()
		{
			var tile = Tile.Blank;
			Assert.Equal('*', tile.Letter);
			Assert.Equal(TileType.Blank, tile.Type);
			Assert.Equal(0, tile.Value);
		}

		[Theory]
		[InlineData('A')]
		[InlineData('Z')]
		public void ValueProperty_CorrectCalculation(char letter)
		{
			var tile = new Tile(letter);
			Assert.Equal(letter, tile.Letter);
			Assert.Equal(TileType.Letter, tile.Type);
			Assert.Equal(Defaults.LetterValues[letter - 'A'], tile.Value);
		}

		[Fact]
		public void LetterTilesEquality_CorrectBehavior()
		{
			var tile1 = new Tile('A');
			var tile2 = new Tile('A');
			var tile3 = new Tile('B');

			Assert.True(tile1.Equals(tile2));
			Assert.False(tile1.Equals(tile3));
			Assert.True(tile1 == tile2);
			Assert.True(tile1 != tile3);
		}

		[Fact]
		public void BlankTilesEquality_CorrectBehavior()
		{
			var tile1 = Tile.Blank;
			var tile2 = Tile.Blank;
			var tile3 = new Tile('A');
			var tile4 = Tile.Empty;

			Assert.True(tile1.Equals(tile2));
			Assert.False(tile1.Equals(tile3));
			Assert.False(tile1.Equals(tile4));
			Assert.True(tile1 == tile2);
			Assert.True(tile1 != tile3);
			Assert.True(tile1 != tile4);
		}

		[Fact]
		public void EmptyTilesEquality_CorrectBehavior()
		{
			var tile1 = Tile.Empty;
			var tile2 = Tile.Empty;
			var tile3 = new Tile('A');
			var tile4 = Tile.Blank;

			Assert.True(tile1.Equals(tile2));
			Assert.False(tile1.Equals(tile3));
			Assert.False(tile1.Equals(tile4));
			Assert.True(tile1 == tile2);
			Assert.True(tile1 != tile3);
			Assert.True(tile1 != tile4);
		}

		[Fact]
		public void GetHashCode_ConsistentWithEquality()
		{
			var tile1 = new Tile('A');
			var tile2 = new Tile('A');
			var tile3 = new Tile('B');

			Assert.Equal(tile1.GetHashCode(), tile2.GetHashCode());
			Assert.NotEqual(tile1.GetHashCode(), tile3.GetHashCode());
		}
	}
}