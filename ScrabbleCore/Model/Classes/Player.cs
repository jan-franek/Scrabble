using ScrabbleCore.Solver;
using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;
using System.ComponentModel;

namespace ScrabbleCore.Classes;

/// <summary>
/// Represents a player in the game.
/// </summary>
public abstract class Player : INotifyPropertyChanged
{
	protected static readonly SolverInterop solver = new();

	public string Name { get; init; }
	public TileRack Rack { get; init; } = [];

	private int _score = 0;
	public int Score
	{
		get => _score;
		set
		{
			if (_score != value)
			{
				_score = value;
				OnPropertyChanged(nameof(_score));
			}
		}
	}

	// Make the solver initialization blocking
	static Player() { }

	/// <param name="name"> The name of the player. </param>
	protected Player(string name)
	{
		Name = name;
		Rack = [];

		Rack.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(Rack));
	}

	#region INotifyPropertyChanged
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	#endregion


	/// <summary>
	/// Draws a tile from the pouch and adds it to the player's rack.
	/// </summary>
	/// <param name="pouch"> The pouch to draw from. </param>
	/// <returns> True if the tile was drawn successfully, false otherwise. </returns>
	public bool DrawTile(Pouch pouch)
	{
		if (Rack.IsFull || pouch.IsEmpty) return false;

		var tile = pouch.Draw();

		Rack.Add(tile);
		return true;
	}

	/// <summary>
	/// Draws tiles from the pouch until the rack is full or the pouch is empty.
	/// </summary>
	/// <param name="pouch"> The pouch to draw from. </param>
	public void DrawFullRack(Pouch pouch)
	{
		while (!Rack.IsFull && !pouch.IsEmpty)
		{
			DrawTile(pouch);
		}
	}

	/// <summary>
	/// Returns a tile to the pouch.
	/// </summary>
	/// <param name="pouch"> The pouch to return the tile to. </param>
	/// <param name="tile"> The tile to return. </param>
	public void ReturnTile(Pouch pouch, Tile tile)
	{
		if (tile == Tile.Empty) return;

		if (!Rack.Remove(tile)) return;

		pouch.Add(tile);
	}

	/// <summary>
	/// Adds the given score to the player's total score.
	/// </summary>
	/// <param name="score"> The score to add. </param>
	public void AddScore(int score) => Score += score;



	/// <summary>
	/// Either skip the move or exchange letters from the pouch if there are still letters in the pouch.
	/// The player will always exchange as much letters as possible.
	/// </summary>
	protected void SkipMove(Pouch pouch)
	{
		if (pouch.IsEmpty) return;

		var numberOfLettersToExchange = Math.Min(Rack.Count, pouch.Count);

		var lettersToExchange = new List<Tile>(numberOfLettersToExchange);
		var exchangeCount = 0;

		for (var i = 0; i < TileRack.Size; i++)
		{
			if (exchangeCount >= numberOfLettersToExchange) break;
			if (Rack[i] == Tile.Empty) continue;

			lettersToExchange.Add(Rack[i]);
			Rack[i] = Tile.Empty;
			exchangeCount++;
		}

		DrawFullRack(pouch);

		foreach (var letter in lettersToExchange)
		{
			pouch.Add(letter);
		}
	}

	/// <summary>
	/// Plays a move on the board and updates the player's rack and score.
	/// </summary>
	/// <param name="move"> The move to play. </param>
	/// <param name="board"> The board to play the move on. </param>
	/// <exception cref="InvalidOperationException"> Thrown when the move could not be played. </exception>"
	protected void PlayMove(WordPlay move, TileBoard board)
	{
		var blanksSet = new HashSet<int>(move.BlankPositions);

		for (var i = 0; i < move.WordPlacement.Word.Length; i++)
		{
			if (blanksSet.Contains(i)) Rack.Remove(Tile.Blank);
			else Rack.Remove(new Tile(move.WordPlacement.Word[i]));
		}

		if (!board.PlaceWord(move.WordPlacement)) throw new InvalidOperationException("The move could not be played.");

		AddScore(move.Score);
	}
}
