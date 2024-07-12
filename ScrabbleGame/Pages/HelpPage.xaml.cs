using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Scrabble.Pages
{
	/// <summary>
	/// Interaction logic for HelpPage.xaml
	/// </summary>
	public partial class HelpPage : Page
	{
		public HelpPage()
		{
			InitializeComponent();
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
			e.Handled = true;
		}

		public void ReturnToMenu_Click(object sender, RoutedEventArgs e)
		{
			var mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.GoToMenu();
		}
	}
}
