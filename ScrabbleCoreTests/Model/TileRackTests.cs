using ScrabbleCore.Classes;
using ScrabbleCore.Structs;

namespace ScrabbleCoreTests.Model
{
	public class TileRackTests
	{
		[Fact]
		public void Rack_Initializes_WithEmptyTiles()
		{
			var rack = new TileRack();
			foreach (var tile in rack)
			{
				Assert.Equal(Tile.Empty, tile);
			}
			Assert.False(rack.IsFull);
		}

		[Fact]
		public void AddTile_AddsTileCorrectly()
		{
			var rack = new TileRack();
			var tile = new Tile('A');
			bool result = rack.Add(tile);

			Assert.True(result);
			Assert.True(rack.Contains(tile));
			Assert.Equal(1, rack.Count);
		}

		[Fact]
		public void AddTile_FailsWhenFull()
		{
			var rack = new TileRack();
			for (int i = 0; i < TileRack.Size; i++)
			{
				rack.Add(new Tile((char)('A' + i)));
			}
			Assert.True(rack.IsFull);

			bool result = rack.Add(new Tile('Z'));
			Assert.False(result);
			Assert.True(rack.IsFull);
		}

		[Fact]
		public void AddTile_FailsAddingEmptyTile()
		{
			var rack = new TileRack();

			bool result = rack.Add(Tile.Empty);

			Assert.False(result);
		}

		[Fact]
		public void RemoveTile_RemovesTileCorrectly()
		{
			var rack = new TileRack();
			var tile = new Tile('A');
			rack.Add(tile);
			bool result = rack.Remove(tile);

			Assert.True(result);
			Assert.False(rack.Contains(tile));
			Assert.Equal(0, rack.Count);
		}

		[Fact]
		public void RemoveTile_FailsWhenTileNotPresent()
		{
			var rack = new TileRack();
			rack.Add(new Tile('A'));
			bool result = rack.Remove(new Tile('B'));

			Assert.False(result);
			Assert.Equal(1, rack.Count);
		}

		[Fact]
		public void ContainsTile_ChecksTilePresence()
		{
			var rack = new TileRack();
			var tileA = new Tile('A');
			var tileB = new Tile('B');
			rack.Add(tileA);

			Assert.True(rack.Contains(tileA));
			Assert.False(rack.Contains(tileB));
		}

		[Fact]
		public void CountOfTile_ReturnsCorrectCount()
		{
			var rack = new TileRack();
			var tile = new Tile('A');
			rack.Add(tile);
			rack.Add(tile);

			Assert.Equal(2, rack.CountOf(tile));
			Assert.Equal(0, rack.CountOf(new Tile('B')));
		}
	}
}
