using System.Windows.Input;

namespace ScrabbleGame.Helpers;

public class RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null) : ICommand
{
	private readonly Action<object?> _execute = execute ?? throw new ArgumentNullException(nameof(execute));

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute(object? parameter)
	{
		return canExecute is null || canExecute(parameter);
	}

	public void Execute(object? parameter)
	{
		if (CanExecute(parameter))
		{
			_execute(parameter);
		}
	}
}
