using ScrabbleCore.Classes;

namespace ScrabbleGame.ViewModel
{
	public class GameOverViewModel(Game game)
	{
		public Game Game { get; init; } = game;
		public string WinnerName
		{
			get
			{
				if (Game.Player1.Score > Game.Player2.Score) return Game.Player1.Name;
				if (Game.Player2.Score > Game.Player1.Score) return Game.Player2.Name;
				return "It's a tie!";
			}
		}
	}
}
