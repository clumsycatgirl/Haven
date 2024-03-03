
namespace Haven.Utils;

public static class Log {
	public static readonly string LogFilesDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
	private static string logFile = Path.Combine(LogFilesDirectory, "logs.log");
	public static string LogFile {
		get => logFile;
		set {
			logFile = value;
			Initialize();
		}
	}

	public enum LogLevel {
		None,
		Info,
		Warning,
		Error
	}

	private static TextWriter outputStream = Console.Out;
	public static TextWriter OutputStream {
		get => outputStream;
		set => outputStream = value ?? throw new ArgumentNullException(nameof(value));
	}

	private static readonly Lazy<string> assemblyName = new(() => Assembly.GetEntryAssembly().GetName().Name);
	private static readonly Lazy<string> assemblyVersion = new(() => Assembly.GetEntryAssembly().GetName().Version.ToString());
	private static readonly Lazy<string> assemblyCompany = new(() => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "Unknown");

	private static StreamWriter writer = null;

	private static void Initialize() {
		if (!Directory.Exists(LogFilesDirectory)) {
			Directory.CreateDirectory(LogFilesDirectory);
		}

		if (File.Exists(LogFile)) {
			File.Delete(LogFile);
		}

		writer = new StreamWriter(LogFile, append: true);

		StringBuilder sb = new();
		sb.AppendLine("------ [logs] ------");
		sb.AppendLine("--- [project configuration] ---");
		sb.AppendLine($" - Name: '{assemblyName.Value}'");
		sb.AppendLine($" - Version: '{assemblyVersion.Value}'");
		sb.AppendLine($" - Author: '{assemblyCompany.Value}'");
		sb.AppendLine($" - Build: '" +
#if DEBUG
		"Debug" +
#else
        "Release" +
#endif
		"'\n");

		WriteLine(LogLevel.None, sb.ToString());
	}

	internal static void Dispose() {
		writer?.Close();
	}

	static Log() {
		Initialize();
	}

	/// <summary>
	/// Writes the specified string data to:
	/// <para>
	///	<br>- Debug.Listeneres collection if DEBUG flag is set</br>
	///	<br>- Standard output string if CONSOLE flag is set</br>
	///	<br>- <see cref="LogFile"/></br>
	/// </para>
	/// </summary>
	/// <param name="data"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteLine(params string[] data) {
		WriteLine(LogLevel.None, data);
	}

	/// <summary>
	/// Writes the specified string data to:
	/// <para>
	///	<br>- Debug.Listeneres collection if DEBUG flag is set</br>
	///	<br>- Standard output string if CONSOLE flag is set</br>
	///	<br>- <see cref="LogFile"/></br>
	/// </para>
	/// </summary>
	/// <param name="logLevel"></param>
	/// <param name="data"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteLine(LogLevel logLevel, params string[] data) {
		Console.ForegroundColor = logLevel switch {
			LogLevel.None => ConsoleColor.White,
			LogLevel.Info => ConsoleColor.Green,
			LogLevel.Warning => ConsoleColor.Yellow,
			LogLevel.Error => ConsoleColor.Red,
			_ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
		};

		foreach (string str in data) {
#if DEBUG
			outputStream.WriteLine(str);
#endif
			writer.WriteLine(str);
		}

		writer.Flush();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteLineIf(bool condition, params string[] data) {
		WriteLineIf(LogLevel.None, condition, data);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteLineIf(LogLevel logLevel, bool condition, params string[] data) {
#if DEBUG
		if (condition) WriteLine(logLevel, data);
#endif
	}

	/// <summary>
	/// Check for condition and throw <see cref="Exception"/> if true, with given error message
	/// </summary>
	/// <param name="condition">condition to check for</param>
	/// <param name="error">error message to log</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteErrorIf(bool condition, string error) {
		WriteErrorIf<Exception>(condition, error);
	}

	/// <summary>
	/// Check for condition and throw error if true
	/// <para> Logs to <see cref="LogFile"/> </para>
	/// </summary>
	/// <typeparam name="T">typeof Exception to throw, if no constructor new(string) present, <see cref="Exception"/> with error as message will be used</typeparam>
	/// <param name="condition">condition to check for</param>
	/// <param name="error">error message to log</param>
	/// <exception cref="Exception"></exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteErrorIf<T>(bool condition, string error) where T : Exception {
#if DEBUG
		if (condition) {
			StackTrace stackTrace = new();
			WriteLine(LogLevel.Error, $"--- [error] ---\n - type: '{typeof(T).Name}'\n - message: '{error}'\n{stackTrace}\n");

			ConstructorInfo stringConstructor = typeof(T).GetConstructor(new[] { typeof(string) });

			if (stringConstructor is not null)
				throw (T)Activator.CreateInstance(typeof(T), error);

			// missing constructor new(string)
			throw new($"type='{typeof(T).Name}' with message='{error}'");
		}
#endif
	}
}
