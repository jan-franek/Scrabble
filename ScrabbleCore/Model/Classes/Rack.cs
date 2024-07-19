using System.Collections;
using System.ComponentModel;

namespace ScrabbleCore.Classes;

/// <summary>
/// Generic class representing a rack.
///
/// Keep in mind that changing the value of a cell using the getter will not trigger a PropertyChanged event.
/// </summary>
/// <typeparam name="T"> The type contained in the rack. </typeparam>
public class Rack<T> : INotifyPropertyChanged, IEnumerable<T> where T : notnull
{
	public const int Size = 7;

	public Rack(T defaultValue)
	{
		rack = new T[Size];
		Initialize(defaultValue);
	}

	public Rack(T[] rack)
	{
		ArgumentNullException.ThrowIfNull(rack, nameof(rack));

		if (rack.Length != Size)
		{
			throw new ArgumentException($"Input array must be exactly {Size} elements.", nameof(rack));
		}

		this.rack = new T[Size];
		Array.Copy(rack, this.rack, this.rack.Length);
	}

	#region INotifyPropertyChanged
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	#endregion

	protected readonly T[] rack;
	public T this[int i]
	{
		get => rack[i];
		set
		{
			rack[i] = value;
			OnPropertyChanged(nameof(rack));
		}
	}

	private void Initialize(T defaultValue)
	{
		for (var i = 0; i < Size; i++)
		{
			this[i] = defaultValue;
		}
	}

	#region IEnumerable<T> Implementation
	public IEnumerator<T> GetEnumerator()
	{
		return ((IEnumerable<T>)rack).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return rack.GetEnumerator();
	}
	#endregion
}
