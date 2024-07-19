using ScrabbleCore.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ScrabbleGame.ViewModel
{
	public class MenuViewModel : INotifyPropertyChanged
	{
		private AIDifficulty _selectedDifficulty;

		public MenuViewModel()
		{
			Difficulties = new ObservableCollection<AIDifficulty>(Enum.GetValues(typeof(AIDifficulty)).Cast<AIDifficulty>());
		}

		public ObservableCollection<AIDifficulty> Difficulties { get; }

		public AIDifficulty SelectedDifficulty
		{
			get => _selectedDifficulty;
			set
			{
				if (_selectedDifficulty != value)
				{
					_selectedDifficulty = value;
					OnPropertyChanged(nameof(SelectedDifficulty));
				}
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
