using ScrabbleCore.Classes;
using ScrabbleCore.Enums;
using ScrabbleCore.Solver.Data;
using ScrabbleGame.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScrabbleGame.Pages
{
	/// <summary>
	/// Interaction logic for GamePage.xaml
	/// </summary>
	public partial class GamePage : Page
	{
		public GamePage(AIDifficulty aiDifficulty)
		{
			InitializeComponent();

			DataContext = new GameViewModel("Human player", aiDifficulty, "AI player");
		}

		/// <summary>
		/// For fun, let the AI play by itself.
		/// </summary>
		//private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		//{
		//	var game = ((GameViewModel)DataContext).Game;

		//	if (game.GameType is GameType.PvP) return;

		//	var aiPlayer = game.Player2 as AIPlayer;

		//	aiPlayer!.PlayTurn(game.Board, game.Pouch);
		//}

		/// <summary>
		/// Play a turn for both players.
		/// </summary>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var game = ((GameViewModel)DataContext).Game;

			if (!(game.GameType is GameType.PvAI && game.IsPlayer1Turn)) return;

			var humanPlayer = game.Player1 as HumanPlayer;
			var aiPlayer = game.Player2 as AIPlayer;

			// Player1 turn
			var (success, skipped) = humanPlayer!.TryPlayTurn(game.Board, game.Pouch);

			if (!success)
			{
				MessageBox.Show("Invalid move");
				return;
			}

			game.TurnNumber++;

			if (skipped)
			{
				//MessageBox.Show($"{humanPlayer.Name} skipped the turn.");
				game.Player1SkipMove();
			}

			CheckGameOver(game);

			// Player2 turn
			var player2Skipped = aiPlayer!.PlayTurn(game.Board, game.Pouch);

			if (player2Skipped)
			{
				MessageBox.Show($"{aiPlayer.Name} skipped the turn.");
				game.Player2SkipMove();
			}

			CheckGameOver(game);

			game.TurnNumber++;
		}

		private void CheckGameOver(Game game)
		{
			if (game.IsGameOver)
			{
				game.ComputeFinalScore();

				var mainWindow = (MainWindow)Window.GetWindow(this);
				mainWindow.GoToGameOver(game);
			}
		}

		private void RackTile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is FrameworkElement { Tag: CellViewModel tile })
			{
				((GameViewModel)DataContext).SelectTileCommand.Execute(tile);
			}
		}

		private void BoardCell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is FrameworkElement { Tag: Coordinates tile })
			{
				((GameViewModel)DataContext).PlaceSelectedTileCommand.Execute(tile);
			}
		}

		private void BoardTile_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is FrameworkElement { Tag: Coordinates position })
			{
				((GameViewModel)DataContext).RemoveTileCommand.Execute(position);
			}
		}
	}
}