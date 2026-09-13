using System;
using System.IO;
using UnityEngine;

namespace GameFramework;

public class LogFile
{
	private FileWriter _fileWriter;

	private const int MaxLogLevelLength = 9;

	public LogFile()
	{
		_fileWriter = new FileWriter($"{Application.persistentDataPath}/{DateTime.Now:yyyy-MM-dd-HH-mm-ss}_Log.txt");
	}

	public void Write(LogType logType, string trace, string message)
	{
		string text = ("[" + logType.ToString() + "]").PadRight(9);
		string message2 = $"{DateTime.Now:yyyy/MM/dd/HH:mm:ss:fff} {text}: {message}\n {trace}";
		_fileWriter.WriteLine(message2);
	}

	public void ClearOld()
	{
		DateTime now = DateTime.Now;
		string[] files = Directory.GetFiles(Application.persistentDataPath, "*_Log.txt");
		foreach (string path in files)
		{
			if ((now - File.GetCreationTime(path)).Days > 3)
			{
				File.Delete(path);
			}
		}
	}
}
