using ScrabbleGame.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Scrabble.Pages
{
	/// <summary>
	/// Interaction logic for MenuPage.xaml
	/// </summary>
	public partial class MenuPage : Page
	{
		private MenuViewModel _viewModel;

		public MenuPage()
		{
			InitializeComponent();
			_viewModel = new MenuViewModel();
			DataContext = _viewModel;

			CreateRadioButtons();
		}

		private void CreateRadioButtons()
		{
			foreach (var difficulty in _viewModel.Difficulties)
			{
				var radioButton = new RadioButton
				{
					Content = difficulty.ToString(),
					GroupName = "DifficultyGroup",
					Margin = new Thickness(5),
					FontSize = 24,
					Foreground = Brushes.White,
				};

				radioButton.Checked += (sender, e) => _viewModel.SelectedDifficulty = difficulty;
				radioButton.IsChecked = _viewModel.SelectedDifficulty == difficulty;

				AIDifficultiesPanel.Children.Add(radioButton);
			}

			_viewModel.PropertyChanged += ViewModel_PropertyChanged;
		}

		private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_viewModel.SelectedDifficulty))
			{
				foreach (RadioButton radioButton in AIDifficultiesPanel.Children)
				{
					if (radioButton.Content.ToString() == _viewModel.SelectedDifficulty.ToString())
					{
						radioButton.IsChecked = true;
					}
				}
			}
		}

		private void StartGame_Click(object sender, RoutedEventArgs e)
		{
			var mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.GoToGame(_viewModel.SelectedDifficulty);
		}

		private void GameRules_Click(object sender, RoutedEventArgs e)
		{
			var mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.GoToHelp();
		}
	}
}
