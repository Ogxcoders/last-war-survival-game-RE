using System.IO;
using System.IO.Compression;
using ProtoBufNet;
using ZstdNet;

public class CommonCompressor
{
	public static void CompressFileGZip(string path, string desPath)
	{
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		desPath = (string.IsNullOrEmpty(desPath) ? (path + "_DefaultGZip") : desPath);
		using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		using FileStream stream = new FileStream(desPath, FileMode.Create, FileAccess.Write);
		using GZipStream destination = new GZipStream(stream, CompressionMode.Compress);
		fileStream.CopyTo(destination);
	}

	public static void DecompressFileGZip(string path, string desPath)
	{
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		desPath = (string.IsNullOrEmpty(desPath) ? (path + "_ori1") : desPath);
		using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
		using FileStream destination = new FileStream(desPath, FileMode.Create, FileAccess.Write);
		using GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress);
		gZipStream.CopyTo(destination);
	}

	public static void CompressFile(string path, string desPath)
	{
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		desPath = (string.IsNullOrEmpty(desPath) ? (path + "_Wrap") : desPath);
		if (NetPacketConst.useNewPacket && NetPacketConst.useZStd)
		{
			using (FileStream fileStream = File.OpenRead(path))
			{
				using FileStream stream = File.Create(desPath);
				using CompressionStream destination = new CompressionStream(stream);
				fileStream.CopyTo(destination);
				return;
			}
		}
		CompressFileGZip(path, desPath);
	}

	public static void DecompressFile(string path, string desPath)
	{
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		desPath = (string.IsNullOrEmpty(desPath) ? (path + "_ori4") : desPath);
		if (NetPacketConst.useNewPacket && NetPacketConst.useZStd)
		{
			using (FileStream stream = File.OpenRead(path))
			{
				using FileStream destination = File.Create(desPath);
				using DecompressionStream decompressionStream = new DecompressionStream(stream);
				decompressionStream.CopyTo(destination);
				return;
			}
		}
		DecompressFileGZip(path, desPath);
	}
}
