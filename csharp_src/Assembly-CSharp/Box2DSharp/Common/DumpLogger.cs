namespace Box2DSharp.Common;

public static class DumpLogger
{
	public static IDumpLogger Instance { get; set; } = new InternalDumpLogger();

	public static void Log(string message)
	{
		Instance.Log(message);
	}
}
