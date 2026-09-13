using System.Runtime.CompilerServices;
using System.Text;
using Mono;

namespace System.IO;

internal class LogcatTextWriter : TextWriter
{
	private readonly byte[] appname;

	private TextWriter stdout;

	private StringBuilder line = new StringBuilder();

	public override Encoding Encoding => Encoding.UTF8;

	public LogcatTextWriter(string appname, TextWriter stdout)
	{
		this.appname = Encoding.UTF8.GetBytes(appname + "\0");
		this.stdout = stdout;
	}

	public override void Write(string s)
	{
		if (s != null)
		{
			foreach (char value in s)
			{
				Write(value);
			}
		}
	}

	public override void Write(char value)
	{
		if (value == '\n')
		{
			WriteLine();
		}
		else
		{
			line.Append(value);
		}
	}

	public override void WriteLine()
	{
		string text = line.ToString();
		line.Clear();
		Log(text);
		stdout.WriteLine(text);
	}

	private unsafe void Log(string message)
	{
		if (Encoding.UTF8.GetByteCount(message) < 512)
		{
			try
			{
				fixed (char* chars = message)
				{
					byte* ptr = stackalloc byte[512];
					int bytes = Encoding.UTF8.GetBytes(chars, message.Length, ptr, 511);
					ptr[bytes] = 0;
					Log(ptr);
					return;
				}
			}
			catch (ArgumentException)
			{
			}
		}
		using SafeStringMarshal safeStringMarshal = new SafeStringMarshal(message);
		Log((byte*)(void*)safeStringMarshal.Value);
	}

	private unsafe void Log(byte* b_message)
	{
		fixed (byte* ptr = appname)
		{
			Log(ptr, 32, b_message);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Log(byte* appname, int level, byte* message);
}
