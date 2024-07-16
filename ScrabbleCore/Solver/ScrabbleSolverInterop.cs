using ScrabbleCore.Solver.Data;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScrabbleCore.Solver;

/// <summary>
/// Interop class for calling the Scrabble solver.
/// It is responsible for creating and disposing the solver instance.
/// </summary>
public partial class SolverInterop : IDisposable
{
	private const string dictionaryPath = @"C:\Code\C#\Scrabble\ScrabbleCore\Solver\Dictionary\csw19.txt";
	private static readonly JsonSerializerOptions jsonOptions = new()
	{
		Converters = { new JsonStringEnumConverter(namingPolicy: JsonNamingPolicy.CamelCase) },
		PropertyNameCaseInsensitive = true
	};

	private IntPtr solverInstance;

	/// <summary>
	/// Creates a new solver instance.
	/// </summary>
	/// <param name="dictionaryPath"> The absolute path to the dictionary file. </param>
	/// <returns> Pointer to the solver instance. </returns>
	[LibraryImport("scrabble_solver_api.dll", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
	[UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
	private static partial IntPtr CreateSolver(string dictionaryPath);

	/// <summary>
	/// Calls the solver to solve the given input.
	/// </summary>
	/// <param name="solver"> Pointer to the solver instance. </param>
	/// <param name="input"> The input to solve. </param>
	/// <returns> Pointer to the JSON string containing the results. </returns>
	[LibraryImport("scrabble_solver_api.dll", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
	[UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
	private static partial IntPtr SolveScrabble(IntPtr solver, string input);

	/// <summary>
	/// Deletes the solver instance.
	/// </summary>
	/// <param name="solver"> Pointer to the solver instance. </param>
	[LibraryImport("scrabble_solver_api.dll")]
	[UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
	private static partial void DeleteSolver(IntPtr solver);

	/// <summary>
	/// Free the memory allocated to the JSON result string.
	/// </summary>
	/// <param name="ptr"> Pointer to the JSON result string. </param>
	[LibraryImport("scrabble_solver_api.dll")]
	private static partial void FreeMemory(IntPtr ptr);

	public SolverInterop()
	{
		solverInstance = CreateSolver(dictionaryPath);
		if (solverInstance == IntPtr.Zero)
		{
			throw new InvalidOperationException("Failed to create solver instance.");
		}
	}

	/// <summary>
	/// Solves the given game state.
	/// </summary>
	/// <param name="gameState"> The game state to solve. </param>
	/// <returns> The list of word plays. </returns>
	/// <exception cref="InvalidOperationException"> Thrown when the solver fails to solve the game state. </exception>
	public List<WordPlay> Solve(GameState gameState)
	{
		IntPtr resultsPtr = IntPtr.Zero;
		string? jsonResults;

		try
		{
			resultsPtr = SolveScrabble(solverInstance, gameState.Serialize());

			if (resultsPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to solve.");

			jsonResults = Marshal.PtrToStringAnsi(resultsPtr);
		}
		finally
		{
			if (resultsPtr != IntPtr.Zero) FreeMemory(resultsPtr);
		}

		if (string.IsNullOrWhiteSpace(jsonResults)) throw new InvalidOperationException("Failed to solve.");

		var results = JsonSerializer.Deserialize<List<WordPlay>>(jsonResults, jsonOptions);

		if (results is null) throw new InvalidOperationException("Failed to deserialize results.");

		return results;
	}

	#region IDisposable Support
	private bool disposedValue = false;

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (solverInstance != IntPtr.Zero)
			{
				DeleteSolver(solverInstance);
				solverInstance = IntPtr.Zero;
			}
			disposedValue = true;
		}
	}

	~SolverInterop()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
	#endregion
}
