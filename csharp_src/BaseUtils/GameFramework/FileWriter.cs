using System.IO;

namespace GameFramework;

public class FileWriter
{
	private readonly object _lock = new object();

	private readonly string _filePath;

	public FileWriter(string filePath)
	{
		_filePath = filePath;
	}

	public void WriteLine(string message)
	{
		lock (_lock)
		{
			using StreamWriter streamWriter = new StreamWriter(_filePath, append: true);
			streamWriter.WriteLine(message);
		}
	}

	public void ClearFile()
	{
		lock (_lock)
		{
			using StreamWriter streamWriter = new StreamWriter(_filePath, append: false);
			streamWriter.Write(string.Empty);
		}
	}
}
