using ScrabbleCore;
using ScrabbleCore.Structs;

namespace ScrabbleCoreTests.Model
{
	public class PouchTests
	{
		/// <summary>
		/// The total number of tiles in the pouch plus two blank tiles.
		/// </summary>
		public static readonly int TotalTiles = Defaults.PouchLetterCounts.Sum() + 2;

		[Fact]
		public void Pouch_InitializesCorrectly()
		{
			var pouch = new Pouch();
			int expectedCount = TotalTiles; // This should be the total tiles count calculated from Defaults
			Assert.Equal(expectedCount, pouch.Count);
		}

		[Fact]
		public void Add_ThrowsArgumentException_WhenAddingEmptyTile()
		{
			var pouch = new Pouch();
			var emptyTile = Tile.Empty;

			Assert.Throws<ArgumentException>(() => pouch.Add(emptyTile));
		}

		[Fact]
		public void Add_IncreasesCount()
		{
			var pouch = new Pouch();
			var tile = new Tile('A');
			pouch.Add(tile);

			Assert.Equal(TotalTiles + 1, pouch.Count);
		}

		[Fact]
		public void Draw_DecreasesCount()
		{
			var pouch = new Pouch();
			_ = pouch.Draw();

			Assert.Equal(TotalTiles - 1, pouch.Count);
		}

		[Fact]
		public void Draw_ThrowsInvalidOperationException_WhenPouchIsEmpty()
		{
			var pouch = new Pouch();
			// Draw all tiles
			for (int i = 0; i < TotalTiles; i++)
			{
				pouch.Draw();
			}

			Assert.Throws<InvalidOperationException>(() => pouch.Draw());
		}

		[Fact]
		public void IsEmpty_ReturnsTrue_WhenPouchIsEmpty()
		{
			var pouch = new Pouch();
			// Draw all tiles
			while (!pouch.IsEmpty)
			{
				pouch.Draw();
			}

			Assert.True(pouch.IsEmpty);
		}
	}
}
