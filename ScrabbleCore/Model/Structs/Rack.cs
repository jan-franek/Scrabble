namespace ScrabbleCore.Structs;

public class Rack<T>
{
	public readonly T?[] rack;

	public const int Size = 7;

	public Rack(T? defaultValue = default)
	{
		rack = new T[Size];
		Initialize(defaultValue);
	}

	public Rack(T?[] rack)
	{
		ArgumentNullException.ThrowIfNull(rack, nameof(rack));

		if (rack.Length != Size)
		{
			throw new ArgumentException($"Input array must be exactly {Size} elements.", nameof(rack));
		}

		this.rack = new T[Size];
		Array.Copy(rack, this.rack, this.rack.Length);
	}

	public T? this[int i]
	{
		get => rack[i];
		set => rack[i] = value;
	}

	private void Initialize(T? defaultValue = default)
	{
		for (int i = 0; i < Size; i++)
		{
			rack[i] = defaultValue;
		}
	}
}
