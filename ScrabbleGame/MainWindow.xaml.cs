using Scrabble.Pages;
using ScrabbleCore.Classes;
using ScrabbleCore.Enums;
using System.Windows;

namespace Scrabble
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			GoToMenu();
		}

		public void GoToMenu()
		{
			MainFrame.Navigate(new MenuPage());
		}

		public void GoToHelp()
		{
			MainFrame.Navigate(new HelpPage());
		}

		public void GoToGame(AIDifficulty difficulty)
		{
			MainFrame.Navigate(new GamePage(difficulty));
		}

		public void GoToGameOver(Game game)
		{
			MainFrame.Navigate(new GameOverPage(game));
		}
	}
}