using ScrabbleCore.Classes;
using ScrabbleGame.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Scrabble.Pages
{
	/// <summary>
	/// Interaction logic for GameOverPage.xaml
	/// </summary>
	public partial class GameOverPage : Page
	{
		public GameOverPage(Game game)
		{
			InitializeComponent();

			DataContext = new GameOverViewModel(game);
		}

		public void ReturnToMenu_Click(object sender, RoutedEventArgs e)
		{
			var mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.GoToMenu();
		}
	}
}
