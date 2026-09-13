using System.IO;
using UnityEngine;

namespace VEngine;

public static class Utility
{
	public const string buildPath = "AssetBundles";

	public const string buildCachePath = "AssetBundlesCache";

	public const string luaMD5RecordPath = "LuaMD5Record";

	public const string unsupportedPlatform = "Unsupported";

	public static int[] IntArrayEmpty = new int[0];

	private static readonly double[] byteUnits = new double[4] { 1073741824.0, 1048576.0, 1024.0, 1.0 };

	private static readonly string[] byteUnitsNames = new string[4] { "GB", "MB", "KB", "B" };

	private static readonly CRC32 _crc32 = new CRC32();

	public static string GetPlatformName()
	{
		return Application.platform switch
		{
			RuntimePlatform.Android => "Android", 
			RuntimePlatform.WindowsPlayer => "Windows", 
			RuntimePlatform.IPhonePlayer => "iOS", 
			RuntimePlatform.WebGLPlayer => "WebGL", 
			_ => "Unsupported", 
		};
	}

	public static string FormatBytes(ulong bytes)
	{
		string result = "0 B";
		if (bytes == 0L)
		{
			return result;
		}
		for (int i = 0; i < byteUnits.Length; i++)
		{
			double num = byteUnits[i];
			if ((double)bytes >= num)
			{
				result = $"{(double)bytes / num:##.##} {byteUnitsNames[i]}";
				break;
			}
		}
		return result;
	}

	public static uint ComputeCRC32(Stream stream)
	{
		return new CRC32().Compute(stream);
	}

	public static uint ComputeCRC32(string filename)
	{
		if (!File.Exists(filename))
		{
			return 0u;
		}
		_crc32.ClearCrc();
		using FileStream stream = File.OpenRead(filename);
		return _crc32.Compute(stream);
	}
}
