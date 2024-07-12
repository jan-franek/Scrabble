using System.Windows;
using System.Windows.Controls;

namespace Scrabble.Pages
{
	/// <summary>
	/// Interaction logic for MenuPage.xaml
	/// </summary>
	public partial class MenuPage : Page
	{
		public MenuPage()
		{
			InitializeComponent();
		}

		private void StartGame_Click(object sender, RoutedEventArgs e)
		{
			var mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.GoToGame();
		}

		private void GameRules_Click(object sender, RoutedEventArgs e)
		{
			var mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.GoToHelp();
		}
	}
}
