using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System;

internal sealed class AndroidTzData : IAndroidTimeZoneDB
{
	internal static readonly string[] Paths = new string[4]
	{
		GetApexTimeDataRoot() + "/etc/tz/tzdata",
		GetApexRuntimeRoot() + "/etc/tz/tzdata",
		Environment.GetEnvironmentVariable("ANDROID_DATA") + "/misc/zoneinfo/tzdata",
		Environment.GetEnvironmentVariable("ANDROID_ROOT") + "/usr/share/zoneinfo/tzdata"
	};

	private string tzdataPath;

	private Stream data;

	private string version;

	private string zoneTab;

	private string[] ids;

	private int[] byteOffsets;

	private int[] lengths;

	public string Version => version;

	public string ZoneTab => zoneTab;

	public AndroidTzData(params string[] paths)
	{
		foreach (string path in paths)
		{
			if (LoadData(path))
			{
				tzdataPath = path;
				return;
			}
		}
		Console.Error.WriteLine("Couldn't find any tzdata!");
		tzdataPath = "/";
		version = "missing";
		zoneTab = "# Emergency fallback data.\n";
		ids = new string[1] { "GMT" };
	}

	private static string GetApexTimeDataRoot()
	{
		string environmentVariable = Environment.GetEnvironmentVariable("ANDROID_TZDATA_ROOT");
		if (!string.IsNullOrEmpty(environmentVariable))
		{
			return environmentVariable;
		}
		return "/apex/com.android.tzdata";
	}

	private static string GetApexRuntimeRoot()
	{
		string environmentVariable = Environment.GetEnvironmentVariable("ANDROID_RUNTIME_ROOT");
		if (!string.IsNullOrEmpty(environmentVariable))
		{
			return environmentVariable;
		}
		return "/apex/com.android.runtime";
	}

	private bool LoadData(string path)
	{
		if (!File.Exists(path))
		{
			return false;
		}
		try
		{
			data = File.OpenRead(path);
		}
		catch (IOException)
		{
			return false;
		}
		catch (UnauthorizedAccessException)
		{
			return false;
		}
		try
		{
			ReadHeader();
			return true;
		}
		catch (Exception arg)
		{
			Console.Error.WriteLine("tzdata file \"{0}\" was present but invalid: {1}", path, arg);
		}
		return false;
	}

	private unsafe void ReadHeader()
	{
		byte[] buffer = new byte[Math.Max(Marshal.SizeOf(typeof(AndroidTzDataHeader)), Marshal.SizeOf(typeof(AndroidTzDataEntry)))];
		AndroidTzDataHeader androidTzDataHeader = ReadAt<AndroidTzDataHeader>(0L, buffer);
		androidTzDataHeader.indexOffset = NetworkToHostOrder(androidTzDataHeader.indexOffset);
		androidTzDataHeader.dataOffset = NetworkToHostOrder(androidTzDataHeader.dataOffset);
		androidTzDataHeader.zoneTabOffset = NetworkToHostOrder(androidTzDataHeader.zoneTabOffset);
		sbyte* ptr = (sbyte*)androidTzDataHeader.signature;
		if (new string(ptr, 0, 6, Encoding.ASCII) != "tzdata" || androidTzDataHeader.signature[11] != 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("bad tzdata magic:");
			for (int i = 0; i < 12; i++)
			{
				stringBuilder.Append(" ").Append(((byte)ptr[i]).ToString("x2"));
			}
			throw new InvalidOperationException("bad tzdata magic: " + stringBuilder.ToString());
		}
		version = new string(ptr, 6, 5, Encoding.ASCII);
		ReadIndex(androidTzDataHeader.indexOffset, androidTzDataHeader.dataOffset, buffer);
		ReadZoneTab(androidTzDataHeader.zoneTabOffset, checked((int)data.Length) - androidTzDataHeader.zoneTabOffset);
	}

	private unsafe T ReadAt<T>(long position, byte[] buffer) where T : struct
	{
		int num = Marshal.SizeOf(typeof(T));
		if (buffer.Length < num)
		{
			throw new InvalidOperationException("Internal error: buffer too small");
		}
		data.Position = position;
		int num2;
		if ((num2 = data.Read(buffer, 0, num)) < num)
		{
			throw new InvalidOperationException($"Error reading '{tzdataPath}': read {num2} bytes, expected {num}");
		}
		fixed (byte* ptr = buffer)
		{
			return (T)Marshal.PtrToStructure((IntPtr)ptr, typeof(T));
		}
	}

	private static int NetworkToHostOrder(int value)
	{
		if (!BitConverter.IsLittleEndian)
		{
			return value;
		}
		return ((value >> 24) & 0xFF) | ((value >> 8) & 0xFF00) | ((value << 8) & 0xFF0000) | (value << 24);
	}

	private unsafe void ReadIndex(int indexOffset, int dataOffset, byte[] buffer)
	{
		int num = (dataOffset - indexOffset) / Marshal.SizeOf(typeof(AndroidTzDataEntry));
		int num2 = Marshal.SizeOf(typeof(AndroidTzDataEntry));
		byteOffsets = new int[num];
		ids = new string[num];
		lengths = new int[num];
		for (int i = 0; i < num; i++)
		{
			AndroidTzDataEntry androidTzDataEntry = ReadAt<AndroidTzDataEntry>(indexOffset + num2 * i, buffer);
			sbyte* ptr = (sbyte*)androidTzDataEntry.id;
			byteOffsets[i] = NetworkToHostOrder(androidTzDataEntry.byteOffset) + dataOffset;
			ids[i] = new string(ptr, 0, GetStringLength(ptr, 40), Encoding.ASCII);
			lengths[i] = NetworkToHostOrder(androidTzDataEntry.length);
			if (lengths[i] < Marshal.SizeOf(typeof(AndroidTzDataHeader)))
			{
				throw new InvalidOperationException("Length in index file < sizeof(tzhead)");
			}
		}
	}

	private unsafe static int GetStringLength(sbyte* s, int maxLength)
	{
		int num = 0;
		while (num < maxLength && *s != 0)
		{
			num++;
			s++;
		}
		return num;
	}

	private void ReadZoneTab(int zoneTabOffset, int zoneTabSize)
	{
		byte[] array = new byte[zoneTabSize];
		data.Position = zoneTabOffset;
		int num;
		if ((num = data.Read(array, 0, array.Length)) < array.Length)
		{
			throw new InvalidOperationException($"Error reading zonetab: read {num} bytes, expected {zoneTabSize}");
		}
		zoneTab = Encoding.ASCII.GetString(array, 0, array.Length);
	}

	public IEnumerable<string> GetAvailableIds()
	{
		return ids;
	}

	public byte[] GetTimeZoneData(string id)
	{
		int num = Array.BinarySearch(ids, id, StringComparer.Ordinal);
		if (num < 0)
		{
			return null;
		}
		int num2 = byteOffsets[num];
		int num3 = lengths[num];
		byte[] array = new byte[num3];
		lock (data)
		{
			data.Position = num2;
			int num4;
			if ((num4 = data.Read(array, 0, array.Length)) < array.Length)
			{
				throw new InvalidOperationException($"Unable to fully read from file '{tzdataPath}' at offset {num2} length {num3}; read {num4} bytes expected {array.Length}.");
			}
		}
		TimeZoneInfo.DumpTimeZoneDataToFile(id, array);
		return array;
	}
}
