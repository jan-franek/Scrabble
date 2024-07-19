using ScrabbleCore.Enums;
using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;
using System.ComponentModel;
using System.Windows.Media;

namespace ScrabbleGame.ViewModel
{
	public class CellViewModel(CellType cellType, Tile tile, Coordinates position) : INotifyPropertyChanged
	{
		public CellType CellType { get; set; } = cellType;
		public Tile Tile { get; set; } = tile;
		public Coordinates Position { get; set; } = position;

		public bool IsTileVisible => Tile.Type != TileType.Empty;
		public object CellTypeColour
		{
			get
			{
				return CellType switch
				{
					CellType.Normal => new SolidColorBrush(Color.FromRgb(0x0F, 0xA2, 0x92)),
					CellType.DoubleLetter => Brushes.LightSkyBlue,
					CellType.TripleLetter => Brushes.RoyalBlue,
					CellType.DoubleWord => Brushes.Pink,
					CellType.TripleWord => Brushes.Red,
					_ => Brushes.White,
				};
			}
		}

		public string CellTypeText
		{
			get
			{
				return CellType switch
				{
					CellType.DoubleLetter => "DL",
					CellType.TripleLetter => "TL",
					CellType.DoubleWord => "DW",
					CellType.TripleWord => "TW",
					_ => "",
				};
			}
		}

		public Brush BackgroundColor => IsSelected ? Brushes.Pink : new SolidColorBrush(Color.FromRgb(0xF2, 0xF7, 0xBF));

		private bool _isSelected;
		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					OnPropertyChanged(nameof(IsSelected));
					OnPropertyChanged(nameof(BackgroundColor));
				}
			}
		}

		public static CellViewModel Empty => new(CellType.Normal, Tile.Empty, new Coordinates(0, 0));

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
