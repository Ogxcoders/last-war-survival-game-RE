using System.IO;
using GameKit.Base;
using VEngine;

public class LWLuaFileUtil
{
	public static readonly char[] s_Magic = new char[4] { 'L', 'W', 'L', 'F' };

	public static readonly int OriginalFileVersion = 1;

	public static readonly int EncryptedFileVersion = 2;

	public const string s_CompressedFileName = "LWScripts.bz2";

	public const string s_FileName = "LWScripts.data";

	public const string s_VersionFileName = "LWScripts.txt";

	public static string FormatCompressedFileName(int version, bool isU440)
	{
		if (isU440)
		{
			return $"LWScripts_{version}_u440.bz2";
		}
		return $"LWScripts_{version}.bz2";
	}

	public static string FormatCompressedFileName(string version, bool isU440)
	{
		if (isU440)
		{
			return "LWScripts_" + version + "_u440.bz2";
		}
		return "LWScripts_" + version + ".bz2";
	}

	public static string FormatPatchFileName(int version, int oldVersion, bool isU440)
	{
		if (isU440)
		{
			return $"LWScripts_{version}_{oldVersion}_u440.patch";
		}
		return $"LWScripts_{version}_{oldVersion}.patch";
	}

	public static void ReadSizeAndCrc(string filePath, out ulong size, out uint crc)
	{
		string[] array = File.ReadAllText(filePath).Split(new char[1] { '|' });
		size = ulong.Parse(array[0]);
		crc = uint.Parse(array[1]);
	}

	public static void WriteSizeAndCrc(string filePath, ulong size, uint crc)
	{
		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}
		File.WriteAllText(filePath, $"{size}|{crc}");
	}

	private static uint ComputeCRC32(Stream stream)
	{
		return new VEngine.CRC32().Compute(stream);
	}

	public static void GetSizeAndCrc(string file, out ulong size, out uint crc)
	{
		using FileStream fileStream = File.OpenRead(file);
		size = (ulong)fileStream.Length;
		crc = ComputeCRC32(fileStream);
	}

	public static void EncodeLWLuaFileData(MemoryStream stream)
	{
		byte[] buffer = stream.GetBuffer();
		if (!IsLWLuaFile(buffer))
		{
			return;
		}
		stream.Seek(0L, SeekOrigin.Begin);
		BinaryReader binaryReader = new BinaryReader(stream);
		binaryReader.ReadBytes(4);
		if (binaryReader.ReadInt32() == OriginalFileVersion)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.Seek(4, SeekOrigin.Begin);
			binaryWriter.Write(EncryptedFileVersion);
			binaryReader.ReadInt32();
			int num = binaryReader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				binaryReader.ReadString();
				int num2 = binaryReader.ReadInt32();
				EncryptUtils.SuperEncrypt(buffer, binaryReader.BaseStream.Position, num2);
				binaryReader.BaseStream.Seek(num2, SeekOrigin.Current);
			}
			stream.Seek(0L, SeekOrigin.Begin);
		}
	}

	public static void DecodeLWLuaFileData(MemoryStream stream)
	{
		byte[] buffer = stream.GetBuffer();
		if (!IsLWLuaFile(buffer))
		{
			return;
		}
		stream.Seek(0L, SeekOrigin.Begin);
		BinaryReader binaryReader = new BinaryReader(stream);
		binaryReader.ReadBytes(4);
		if (binaryReader.ReadInt32() == EncryptedFileVersion)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.Seek(4, SeekOrigin.Begin);
			binaryWriter.Write(OriginalFileVersion);
			binaryReader.ReadInt32();
			int num = binaryReader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				binaryReader.ReadString();
				int num2 = binaryReader.ReadInt32();
				EncryptUtils.SuperDecrypt(buffer, binaryReader.BaseStream.Position, num2);
				binaryReader.BaseStream.Seek(num2, SeekOrigin.Current);
			}
			stream.Seek(0L, SeekOrigin.Begin);
		}
	}

	public static bool IsOriginalLWLuaFile(MemoryStream stream)
	{
		return CheckLWFileVersion(stream, OriginalFileVersion);
	}

	public static bool IsEncodedLWLuaFile(MemoryStream stream)
	{
		return CheckLWFileVersion(stream, EncryptedFileVersion);
	}

	private static bool CheckLWFileVersion(MemoryStream stream, int fileVersion)
	{
		if (!IsLWLuaFile(stream.GetBuffer()))
		{
			return false;
		}
		stream.Seek(0L, SeekOrigin.Begin);
		BinaryReader binaryReader = new BinaryReader(stream);
		binaryReader.ReadBytes(4);
		int num = binaryReader.ReadInt32();
		stream.Seek(0L, SeekOrigin.Begin);
		return num == fileVersion;
	}

	public static bool IsLWLuaFile(byte[] bytes)
	{
		if (bytes.Length < 4)
		{
			return false;
		}
		if (bytes[0] == s_Magic[0] && bytes[1] == s_Magic[1] && bytes[2] == s_Magic[2])
		{
			return bytes[3] == s_Magic[3];
		}
		return false;
	}
}
