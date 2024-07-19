using ScrabbleCore.Enums;
using System.ComponentModel;

namespace ScrabbleCore.Classes;

/// <summary>
/// Represents a game of Scrabble.
/// </summary>
public class Game : INotifyPropertyChanged
{
	public GameType GameType { get; init; }
	public Player Player1 { get; init; }
	public Player Player2 { get; init; }
	public TileBoard Board { get; init; }
	public Pouch Pouch { get; init; }

	private int _turnNumber = 1;
	public int TurnNumber
	{
		get => _turnNumber;
		set
		{
			if (_turnNumber != value)
			{
				_turnNumber = value;
				OnPropertyChanged(nameof(TurnNumber));
			}
		}
	}
	public bool IsPlayer1Turn => TurnNumber % 2 == 1;
	public bool IsPlayer2Turn => TurnNumber % 2 == 0;

	private bool player1FirstSkip = false;
	private bool player1SecondSkip = false;
	private bool player2FirstSkip = false;
	private bool player2SecondSkip = false;

	public bool IsGameOver
	{
		get
		{
			if (player1FirstSkip && player2FirstSkip && player1SecondSkip && player2SecondSkip) return true;

			if ((Player1.Rack.IsEmpty || Player2.Rack.IsEmpty) && Pouch.IsEmpty) return true;

			return false;
		}
	}

	private Game(Player player1, Player player2)
	{
		Player1 = player1;
		Player2 = player2;

		Board = new TileBoard();
		Pouch = new Pouch();

		Player1.DrawFullRack(Pouch);
		Player2.DrawFullRack(Pouch);

		// We don't care about their specific property changes, just that they changed.
		Player1.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(Player1));
		Player2.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(Player2));
		Board.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(Board));
		Pouch.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(Pouch));
	}

	public static Game InitializePvAIGame(string player1Name, AIDifficulty difficulty, string player2Name)
	{
		return new Game(
			new HumanPlayer(player1Name),
			new AIPlayer(player2Name, difficulty)
		)
		{
			GameType = GameType.PvAI,
		};
	}

	public static Game InitializeAIvAIGame(string player1Name, AIDifficulty difficulty1, string player2Name, AIDifficulty difficulty2)
	{
		return new Game(
			new AIPlayer(player1Name, difficulty1),
			new AIPlayer(player2Name, difficulty2)
		)
		{
			GameType = GameType.AIvAI,
		};
	}

	public void Player1SkipMove()
	{
		if (player1FirstSkip) player1SecondSkip = true;
		else player1FirstSkip = true;
	}

	public void Player2SkipMove()
	{
		if (player2FirstSkip) player2SecondSkip = true;
		else player2FirstSkip = true;
	}

	public void ComputeFinalScore()
	{
		if (!IsGameOver) return;

		var players = new List<Player> { Player1, Player2 };
		foreach (var player in players)
		{
			var playerPenalty = player.Rack.Sum(tile => tile.Value);

			player.Score -= playerPenalty;
		}
	}

	#region INotifyPropertyChanged
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	#endregion
}
