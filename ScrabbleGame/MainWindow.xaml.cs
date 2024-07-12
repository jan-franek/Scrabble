using System.Windows;

using Scrabble.Pages;

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

		public void GoToGame()
		{
			MainFrame.Navigate(new GamePage());
		}

		public void GoToGameOver()
		{
			MainFrame.Navigate(new GameOverPage());
		}
	}
}