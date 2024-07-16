using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using ScrabbleCore.Solver.Data;

namespace ScrabbleCore.Solver;

public partial class SolverInterop : IDisposable
{
	private const string dictionaryPath = @"C:\Code\C#\Scrabble\ScrabbleCore\Solver\Dictionary\csw19.txt";
	private static readonly JsonSerializerOptions jsonOptions = new()
	{
		Converters = { new JsonStringEnumConverter( namingPolicy: JsonNamingPolicy.CamelCase ) },
		PropertyNameCaseInsensitive = true
	};

	private IntPtr solverInstance;

	[LibraryImport("scrabble_solver_api.dll", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
	[UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
	private static partial IntPtr CreateSolver(string dictionaryPath);

	[LibraryImport("scrabble_solver_api.dll", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
	[UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
	public static partial IntPtr SolveScrabble(IntPtr solver, string input);

	[LibraryImport("scrabble_solver_api.dll")]
	[UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
	private static partial void DeleteSolver(IntPtr solver);

	public SolverInterop()
	{
		solverInstance = CreateSolver(dictionaryPath);
		if (solverInstance == IntPtr.Zero)
		{
			throw new InvalidOperationException("Failed to create solver instance.");
		}
	}

	public List<WordPlay> Solve(GameState gameState)
	{
		var resultsPtr = SolveScrabble(solverInstance, gameState.Serialize());

		if (resultsPtr == IntPtr.Zero) throw new InvalidOperationException("Failed to solve.");

		var jsonResults = Marshal.PtrToStringAnsi(resultsPtr);

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
