using ScrabbleCore.Classes;
using System.Text;

namespace ScrabbleCore.Solver.Data
{
	/// <summary>
	/// State of the game from the point of view of one player.
	/// Mirrors the GameState struct in scrabble_solver.
	/// </summary>
	public readonly struct GameState(TileBoard board, TileRack rack)
	{
		public TileBoard Board { get; init; } = board;
		public TileRack Rack { get; init; } = rack;

		/// <summary>
		/// Serializes <see cref="GameState"/> into a string accepted by the solver.
		/// First 225 (15x15) characters are the board, followed by 7 characters representing the rack.
		/// </summary>
		public string Serialize()
		{
			var sb = new StringBuilder(225 + 1 + 7);

			for (var y = 0; y < TileBoard.Height; y++)
			{
				for (var x = 0; x < TileBoard.Width; x++)
				{
					// The coords are inverted because the solver interprets the board as (y, x) instead of (x, y).
					sb.Append(Board[y, x].Letter);
				}
			}

			for (var i = 0; i < TileRack.Size; i++)
			{
				sb.Append(Rack[i].Letter);
			}

			return sb.ToString();
		}
	}
}
