using ScrabbleCore.Classes;
using ScrabbleCore.Structs;
using System.Text;

namespace ScrabbleCore.Solver.Data
{
	/// <summary>
	/// State of the game from the point of view of one player.
	/// Mirror of the scrabble_solvers GameState struct.
	/// </summary>
	public readonly struct GameState(Board<Tile> board, TileRack rack)
	{
		public Board<Tile> Board { get; init; } = board;
		public TileRack Rack { get; init; } = rack;

		/// <summary>
		/// Serializes <see cref="GameState"/> into a string accepted by the solver.
		/// First 225 (15x15) characters are the board, followed by 7 characters representing the rack.
		/// </summary>
		public string Serialize()
		{
			var sb = new StringBuilder(225 + 1 + 7);

			for (int y = 0; y < Board<Tile>.Height; y++)
			{
				for (int x = 0; x < Board<Tile>.Width; x++)
				{
					sb.Append(Board[y, x].Letter);
				}
			}

			for (int i = 0; i < TileRack.Size; i++)
			{
				sb.Append(Rack[i].Letter);
			}

			return sb.ToString();
		}
	}
}
