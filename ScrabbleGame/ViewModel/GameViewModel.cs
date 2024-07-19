using ScrabbleCore;
using ScrabbleCore.Classes;
using ScrabbleCore.Enums;
using ScrabbleCore.Solver.Data;
using ScrabbleCore.Structs;
using ScrabbleGame.Helpers;
using System.ComponentModel;
using System.Windows.Input;

namespace ScrabbleGame.ViewModel
{
	public class GameViewModel : INotifyPropertyChanged
	{
		public Game Game { get; init; }
		public List<List<CellViewModel>> Board { get; private set; } = [];
		public List<CellViewModel> Rack { get; private set; } = [];
		public int TurnNumber => (Game.TurnNumber + 1) / 2;

		private CellViewModel _selectedTile = CellViewModel.Empty;
		public CellViewModel SelectedTile
		{
			get => _selectedTile;
			set
			{
				_selectedTile.IsSelected = false;

				// Deselect the tile if it's already selected
				_selectedTile = _selectedTile == value ? CellViewModel.Empty : value;

				_selectedTile.IsSelected = true;
				OnPropertyChanged(nameof(SelectedTile));
			}
		}

		public ICommand SelectTileCommand { get; }
		public ICommand PlaceSelectedTileCommand { get; }
		public ICommand RemoveTileCommand { get; }

		public GameViewModel(string player1Name, AIDifficulty difficulty, string player2Name)
		{
			var aiName = $"{player2Name} ({difficulty})";
			Game = Game.InitializePvAIGame(player1Name, difficulty, aiName);
			Game.PropertyChanged += Game_PropertyChanged;

			SelectTileCommand = new RelayCommand(SelectTile, CanSelectTile);
			PlaceSelectedTileCommand = new RelayCommand(PlaceTile, CanPlaceTile);
			RemoveTileCommand = new RelayCommand(RemoveTile, CanRemoveTile);

			RefreshBoard();
			RefreshRack(Game.Player1.Rack);
		}

		//TODO: AI vs AI game, sometime in the future - probably its own page
		//public GameViewModel(string player1Name, AIDifficulty difficulty1, string player2Name, AIDifficulty difficulty2)
		//{
		//	var ai1Name = $"{player1Name} ({difficulty1})";
		//	var ai2Name = $"{player2Name} ({difficulty2})";
		//	Game = Game.InitializeAIvAIGame(ai1Name, difficulty1, ai2Name, difficulty2);
		//	Game.PropertyChanged += Game_PropertyChanged;

		//	RefreshBoard();
		//	RefreshRack();
		//}

		private bool CanSelectTile(object? parameter)
		{
			if (parameter is null) return false;

			if (Game.GameType is not GameType.PvAI || Game.IsPlayer2Turn) return false;

			if (parameter is CellViewModel tile && Rack.Contains(tile)) return true;

			return false;
		}
		private void SelectTile(object? parameter) => SelectedTile = (CellViewModel)parameter!;

		private bool CanPlaceTile(object? parameter)
		{
			if (parameter is null) return false;

			if (Game.GameType is not GameType.PvAI || Game.IsPlayer2Turn) return false;

			if (parameter is Coordinates position)
			{
				return Game.Board.CanPlaceTile(position, SelectedTile.Tile) && ((HumanPlayer)Game.Player1).TemporaryBoard.CanPlaceTile(position, SelectedTile.Tile);
			}

			return false;
		}
		private void PlaceTile(object? parameter)
		{
			var position = (Coordinates)parameter!;
			var humanPlayer = (HumanPlayer)Game.Player1;
			humanPlayer.PlaceTemporaryTile(position, Rack.IndexOf(SelectedTile));

			SelectedTile = CellViewModel.Empty;
		}

		private bool CanRemoveTile(object? parameter)
		{
			if (parameter is null) return false;

			if (Game.GameType is not GameType.PvAI || Game.IsPlayer2Turn) return false;

			if (parameter is Coordinates position)
			{
				return ((HumanPlayer)Game.Player1).TemporaryBoard[position] != Tile.Empty;
			}

			return false;
		}
		private void RemoveTile(object? parameter)
		{
			var position = (Coordinates)parameter!;
			var humanPlayer = (HumanPlayer)Game.Player1;
			humanPlayer.RemoveTemporaryTile(position);
		}

		private void Game_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(Game.Board):
					RefreshBoard();
					OnPropertyChanged(nameof(Board));
					break;
				case nameof(Game.Pouch):
					OnPropertyChanged(nameof(Game.Pouch.Count));
					break;
				case nameof(Game.Player1):
					RefreshBoard();
					RefreshRack(Game.Player1.Rack);
					OnPropertyChanged(nameof(Board));
					OnPropertyChanged(nameof(Rack));
					OnPropertyChanged(nameof(Game.Player1.Score));
					break;
				case nameof(Game.Player2):
					OnPropertyChanged(nameof(Game.Player2.Score));
					break;
				case nameof(Game.TurnNumber):
					OnPropertyChanged(nameof(TurnNumber));
					break;
				default:
					break;
			}
		}

		private void RefreshBoard()
		{
			Board = [];

			if (Game.GameType is GameType.PvAI && Game.IsPlayer1Turn)
			{
				var humanPlayer1 = (HumanPlayer)Game.Player1;

				for (var y = 0; y < TileBoard.Height; y++)
				{
					var row = new List<CellViewModel>();
					for (var x = 0; x < TileBoard.Width; x++)
					{
						var cellType = Defaults.BoardCells[y, x];
						var coordinates = new Coordinates(x, y);

						Tile tile;
						if (humanPlayer1.TemporaryBoard[y, x] != Tile.Empty)
						{
							// There may be overlap where there is a tile on the board and the temporary board but that's fine
							tile = humanPlayer1.TemporaryBoard[y, x];
						}
						else
						{
							tile = Game.Board[y, x];
						}

						row.Add(new CellViewModel(cellType, tile, coordinates));
					}
					Board.Add(row);
				}
			}
			else
			{
				for (var y = 0; y < TileBoard.Height; y++)
				{
					var row = new List<CellViewModel>();
					for (var x = 0; x < TileBoard.Width; x++)
					{
						var cellType = Defaults.BoardCells[y, x];
						var tile = Game.Board[y, x];
						row.Add(new CellViewModel(cellType, tile, new Coordinates(x, y)));
					}
					Board.Add(row);
				}
			}
		}

		private void RefreshRack(TileRack tiles)
		{
			Rack = [];
			foreach (var tile in tiles)
			{
				Rack.Add(new CellViewModel(CellType.Normal, tile, new Coordinates(0, 0)));
			}
		}

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}