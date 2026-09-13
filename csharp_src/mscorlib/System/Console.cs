using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System;

public static class Console
{
	internal static TextWriter stdout;

	private static TextWriter stderr;

	private static TextReader stdin;

	private const string LibLog = "/system/lib/liblog.so";

	private const string LibLog64 = "/system/lib64/liblog.so";

	internal static bool IsRunningOnAndroid;

	private static Encoding inputEncoding;

	private static Encoding outputEncoding;

	internal const ConsoleColor UnknownColor = (ConsoleColor)(-1);

	private static ConsoleColor s_trackedForegroundColor;

	private static ConsoleColor s_trackedBackgroundColor;

	public static TextWriter Error => stderr;

	public static TextWriter Out => stdout;

	public static TextReader In => stdin;

	public static Encoding InputEncoding
	{
		get
		{
			return inputEncoding;
		}
		set
		{
			inputEncoding = value;
			SetupStreams(inputEncoding, outputEncoding);
		}
	}

	public static Encoding OutputEncoding
	{
		get
		{
			return outputEncoding;
		}
		set
		{
			outputEncoding = value;
			SetupStreams(inputEncoding, outputEncoding);
		}
	}

	public static ConsoleColor ForegroundColor
	{
		get
		{
			return s_trackedForegroundColor;
		}
		set
		{
			lock (Out)
			{
				s_trackedForegroundColor = value;
			}
		}
	}

	public static ConsoleColor BackgroundColor
	{
		get
		{
			return s_trackedBackgroundColor;
		}
		set
		{
			lock (Out)
			{
				s_trackedBackgroundColor = value;
			}
		}
	}

	public static int BufferWidth
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static int BufferHeight
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static bool CapsLock
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static int CursorLeft
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static int CursorTop
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static int CursorSize
	{
		get
		{
			return 100;
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static bool CursorVisible
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static bool KeyAvailable
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static int LargestWindowWidth
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static int LargestWindowHeight
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static bool NumberLock
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static string Title
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static bool TreatControlCAsInput
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static int WindowHeight
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static int WindowLeft
	{
		get
		{
			return 0;
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static int WindowTop
	{
		get
		{
			return 0;
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static int WindowWidth
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static bool IsErrorRedirected
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static bool IsInputRedirected
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static bool IsOutputRedirected
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static event ConsoleCancelEventHandler CancelKeyPress
	{
		add
		{
			throw new PlatformNotSupportedException();
		}
		remove
		{
			throw new PlatformNotSupportedException();
		}
	}

	static Console()
	{
		IsRunningOnAndroid = File.Exists("/system/lib/liblog.so") || File.Exists("/system/lib64/liblog.so");
		s_trackedForegroundColor = (ConsoleColor)(-1);
		s_trackedBackgroundColor = (ConsoleColor)(-1);
		int code_page = 0;
		EncodingHelper.InternalCodePage(ref code_page);
		if (code_page != -1 && ((code_page & 0xFFFFFFF) == 3 || (code_page & 0x10000000) != 0))
		{
			inputEncoding = (outputEncoding = EncodingHelper.UTF8Unmarked);
		}
		else
		{
			inputEncoding = (outputEncoding = Encoding.Default);
		}
		SetupStreams(inputEncoding, outputEncoding);
	}

	private static void SetupStreams(Encoding inputEncoding, Encoding outputEncoding)
	{
		stdin = TextReader.Synchronized(new UnexceptionalStreamReader(OpenStandardInput(0), inputEncoding));
		stdout = TextWriter.Synchronized(new UnexceptionalStreamWriter(OpenStandardOutput(0), outputEncoding)
		{
			AutoFlush = true
		});
		stderr = TextWriter.Synchronized(new UnexceptionalStreamWriter(OpenStandardError(0), outputEncoding)
		{
			AutoFlush = true
		});
		if (IsRunningOnAndroid)
		{
			stdout = TextWriter.Synchronized(new LogcatTextWriter("mono-stdout", stdout));
			stderr = TextWriter.Synchronized(new LogcatTextWriter("mono-stderr", stderr));
		}
		GC.SuppressFinalize(stdout);
		GC.SuppressFinalize(stderr);
		GC.SuppressFinalize(stdin);
	}

	private static Stream Open(IntPtr handle, FileAccess access, int bufferSize)
	{
		try
		{
			FileStream fileStream = new FileStream(handle, access, ownsHandle: false, bufferSize, isAsync: false, isConsoleWrapper: true);
			GC.SuppressFinalize(fileStream);
			return fileStream;
		}
		catch (IOException)
		{
			return Stream.Null;
		}
	}

	public static Stream OpenStandardError()
	{
		return OpenStandardError(0);
	}

	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	public static Stream OpenStandardError(int bufferSize)
	{
		return Open(MonoIO.ConsoleError, FileAccess.Write, bufferSize);
	}

	public static Stream OpenStandardInput()
	{
		return OpenStandardInput(0);
	}

	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	public static Stream OpenStandardInput(int bufferSize)
	{
		return Open(MonoIO.ConsoleInput, FileAccess.Read, bufferSize);
	}

	public static Stream OpenStandardOutput()
	{
		return OpenStandardOutput(0);
	}

	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	public static Stream OpenStandardOutput(int bufferSize)
	{
		return Open(MonoIO.ConsoleOutput, FileAccess.Write, bufferSize);
	}

	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	public static void SetError(TextWriter newError)
	{
		if (newError == null)
		{
			throw new ArgumentNullException("newError");
		}
		stderr = TextWriter.Synchronized(newError);
	}

	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	public static void SetIn(TextReader newIn)
	{
		if (newIn == null)
		{
			throw new ArgumentNullException("newIn");
		}
		stdin = TextReader.Synchronized(newIn);
	}

	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	public static void SetOut(TextWriter newOut)
	{
		if (newOut == null)
		{
			throw new ArgumentNullException("newOut");
		}
		stdout = TextWriter.Synchronized(newOut);
	}

	public static void Write(bool value)
	{
		stdout.Write(value);
	}

	public static void Write(char value)
	{
		stdout.Write(value);
	}

	public static void Write(char[] buffer)
	{
		stdout.Write(buffer);
	}

	public static void Write(decimal value)
	{
		stdout.Write(value);
	}

	public static void Write(double value)
	{
		stdout.Write(value);
	}

	public static void Write(int value)
	{
		stdout.Write(value);
	}

	public static void Write(long value)
	{
		stdout.Write(value);
	}

	public static void Write(object value)
	{
		stdout.Write(value);
	}

	public static void Write(float value)
	{
		stdout.Write(value);
	}

	public static void Write(string value)
	{
		stdout.Write(value);
	}

	[CLSCompliant(false)]
	public static void Write(uint value)
	{
		stdout.Write(value);
	}

	[CLSCompliant(false)]
	public static void Write(ulong value)
	{
		stdout.Write(value);
	}

	public static void Write(string format, object arg0)
	{
		stdout.Write(format, arg0);
	}

	public static void Write(string format, params object[] arg)
	{
		if (arg == null)
		{
			stdout.Write(format);
		}
		else
		{
			stdout.Write(format, arg);
		}
	}

	public static void Write(char[] buffer, int index, int count)
	{
		stdout.Write(buffer, index, count);
	}

	public static void Write(string format, object arg0, object arg1)
	{
		stdout.Write(format, arg0, arg1);
	}

	public static void Write(string format, object arg0, object arg1, object arg2)
	{
		stdout.Write(format, arg0, arg1, arg2);
	}

	[CLSCompliant(false)]
	public static void Write(string format, object arg0, object arg1, object arg2, object arg3, __arglist)
	{
		ArgIterator argIterator = new ArgIterator(__arglist);
		int remainingCount = argIterator.GetRemainingCount();
		object[] array = new object[remainingCount + 4];
		array[0] = arg0;
		array[1] = arg1;
		array[2] = arg2;
		array[3] = arg3;
		for (int i = 0; i < remainingCount; i++)
		{
			TypedReference nextArg = argIterator.GetNextArg();
			array[i + 4] = TypedReference.ToObject(nextArg);
		}
		stdout.Write(string.Format(format, array));
	}

	public static void WriteLine()
	{
		stdout.WriteLine();
	}

	public static void WriteLine(bool value)
	{
		stdout.WriteLine(value);
	}

	public static void WriteLine(char value)
	{
		stdout.WriteLine(value);
	}

	public static void WriteLine(char[] buffer)
	{
		stdout.WriteLine(buffer);
	}

	public static void WriteLine(decimal value)
	{
		stdout.WriteLine(value);
	}

	public static void WriteLine(double value)
	{
		stdout.WriteLine(value);
	}

	public static void WriteLine(int value)
	{
		stdout.WriteLine(value);
	}

	public static void WriteLine(long value)
	{
		stdout.WriteLine(value);
	}

	public static void WriteLine(object value)
	{
		stdout.WriteLine(value);
	}

	public static void WriteLine(float value)
	{
		stdout.WriteLine(value);
	}

	public static void WriteLine(string value)
	{
		stdout.WriteLine(value);
	}

	[CLSCompliant(false)]
	public static void WriteLine(uint value)
	{
		stdout.WriteLine(value);
	}

	[CLSCompliant(false)]
	public static void WriteLine(ulong value)
	{
		stdout.WriteLine(value);
	}

	public static void WriteLine(string format, object arg0)
	{
		stdout.WriteLine(format, arg0);
	}

	public static void WriteLine(string format, params object[] arg)
	{
		if (arg == null)
		{
			stdout.WriteLine(format);
		}
		else
		{
			stdout.WriteLine(format, arg);
		}
	}

	public static void WriteLine(char[] buffer, int index, int count)
	{
		stdout.WriteLine(buffer, index, count);
	}

	public static void WriteLine(string format, object arg0, object arg1)
	{
		stdout.WriteLine(format, arg0, arg1);
	}

	public static void WriteLine(string format, object arg0, object arg1, object arg2)
	{
		stdout.WriteLine(format, arg0, arg1, arg2);
	}

	[CLSCompliant(false)]
	public static void WriteLine(string format, object arg0, object arg1, object arg2, object arg3, __arglist)
	{
		ArgIterator argIterator = new ArgIterator(__arglist);
		int remainingCount = argIterator.GetRemainingCount();
		object[] array = new object[remainingCount + 4];
		array[0] = arg0;
		array[1] = arg1;
		array[2] = arg2;
		array[3] = arg3;
		for (int i = 0; i < remainingCount; i++)
		{
			TypedReference nextArg = argIterator.GetNextArg();
			array[i + 4] = TypedReference.ToObject(nextArg);
		}
		stdout.WriteLine(string.Format(format, array));
	}

	public static int Read()
	{
		return stdin.Read();
	}

	public static string ReadLine()
	{
		return stdin.ReadLine();
	}

	public static void Beep()
	{
		throw new PlatformNotSupportedException();
	}

	public static void Beep(int frequency, int duration)
	{
		throw new PlatformNotSupportedException();
	}

	public static void Clear()
	{
		throw new PlatformNotSupportedException();
	}

	public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
	{
		throw new PlatformNotSupportedException();
	}

	public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
	{
		throw new PlatformNotSupportedException();
	}

	public static ConsoleKeyInfo ReadKey()
	{
		return ReadKey(intercept: false);
	}

	public static ConsoleKeyInfo ReadKey(bool intercept)
	{
		throw new PlatformNotSupportedException();
	}

	public static void ResetColor()
	{
		lock (Out)
		{
			s_trackedForegroundColor = (ConsoleColor)(-1);
			s_trackedBackgroundColor = (ConsoleColor)(-1);
		}
	}

	public static void SetBufferSize(int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public static void SetCursorPosition(int left, int top)
	{
		throw new PlatformNotSupportedException();
	}

	public static void SetWindowPosition(int left, int top)
	{
		throw new PlatformNotSupportedException();
	}

	public static void SetWindowSize(int width, int height)
	{
		throw new PlatformNotSupportedException();
	}
}
