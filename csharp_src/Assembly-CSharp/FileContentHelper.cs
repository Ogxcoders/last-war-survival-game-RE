using System.IO;
using UnityEngine;

public class FileContentHelper
{
	public static bool useBinary = true;

	public static string GetRootDirectory()
	{
		return Path.Combine(Application.persistentDataPath, "FileContents");
	}
}
