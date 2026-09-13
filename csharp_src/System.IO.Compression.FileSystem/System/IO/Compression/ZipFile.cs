using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace System.IO.Compression;

public static class ZipFile
{
	private const char PathSeparator = '/';

	public static ZipArchive OpenRead(string archiveFileName)
	{
		return Open(archiveFileName, ZipArchiveMode.Read);
	}

	public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode)
	{
		return Open(archiveFileName, mode, null);
	}

	public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode, Encoding entryNameEncoding)
	{
		FileMode mode2;
		FileAccess access;
		FileShare share;
		switch (mode)
		{
		case ZipArchiveMode.Read:
			mode2 = FileMode.Open;
			access = FileAccess.Read;
			share = FileShare.Read;
			break;
		case ZipArchiveMode.Create:
			mode2 = FileMode.CreateNew;
			access = FileAccess.Write;
			share = FileShare.None;
			break;
		case ZipArchiveMode.Update:
			mode2 = FileMode.OpenOrCreate;
			access = FileAccess.ReadWrite;
			share = FileShare.None;
			break;
		default:
			throw new ArgumentOutOfRangeException("mode");
		}
		FileStream fileStream = new FileStream(archiveFileName, mode2, access, share, 4096, useAsync: false);
		try
		{
			return new ZipArchive(fileStream, mode, leaveOpen: false, entryNameEncoding);
		}
		catch
		{
			fileStream.Dispose();
			throw;
		}
	}

	public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName)
	{
		DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, null, includeBaseDirectory: false, null);
	}

	public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory)
	{
		DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionLevel, includeBaseDirectory, null);
	}

	public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
	{
		DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionLevel, includeBaseDirectory, entryNameEncoding);
	}

	public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
	{
		ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName, null);
	}

	public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, bool overwrite)
	{
		ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName, null, overwrite);
	}

	public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, Encoding entryNameEncoding)
	{
		ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName, entryNameEncoding, overwrite: false);
	}

	public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, Encoding entryNameEncoding, bool overwrite)
	{
		if (sourceArchiveFileName == null)
		{
			throw new ArgumentNullException("sourceArchiveFileName");
		}
		using ZipArchive source = Open(sourceArchiveFileName, ZipArchiveMode.Read, entryNameEncoding);
		source.ExtractToDirectory(destinationDirectoryName, overwrite);
	}

	private static void DoCreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel? compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
	{
		sourceDirectoryName = Path.GetFullPath(sourceDirectoryName);
		destinationArchiveFileName = Path.GetFullPath(destinationArchiveFileName);
		using ZipArchive zipArchive = Open(destinationArchiveFileName, ZipArchiveMode.Create, entryNameEncoding);
		bool flag = true;
		DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectoryName);
		string fullName = directoryInfo.FullName;
		if (includeBaseDirectory && directoryInfo.Parent != null)
		{
			fullName = directoryInfo.Parent.FullName;
		}
		char[] buffer = ArrayPool<char>.Shared.Rent(260);
		try
		{
			foreach (FileSystemInfo item in directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
			{
				flag = false;
				int length = item.FullName.Length - fullName.Length;
				if (item is FileInfo)
				{
					string entryName = EntryFromPath(item.FullName, fullName.Length, length, ref buffer);
					ZipFileExtensions.DoCreateEntryFromFile(zipArchive, item.FullName, entryName, compressionLevel);
				}
				else if (item is DirectoryInfo possiblyEmptyDir && IsDirEmpty(possiblyEmptyDir))
				{
					string entryName2 = EntryFromPath(item.FullName, fullName.Length, length, ref buffer, appendPathSeparator: true);
					zipArchive.CreateEntry(entryName2);
				}
			}
			if (includeBaseDirectory && flag)
			{
				zipArchive.CreateEntry(EntryFromPath(directoryInfo.Name, 0, directoryInfo.Name.Length, ref buffer, appendPathSeparator: true));
			}
		}
		finally
		{
			ArrayPool<char>.Shared.Return(buffer);
		}
	}

	private static string EntryFromPath(string entry, int offset, int length, ref char[] buffer, bool appendPathSeparator = false)
	{
		while (length > 0 && (entry[offset] == Path.DirectorySeparatorChar || entry[offset] == Path.AltDirectorySeparatorChar))
		{
			offset++;
			length--;
		}
		if (length == 0)
		{
			if (!appendPathSeparator)
			{
				return string.Empty;
			}
			return '/'.ToString();
		}
		int num = (appendPathSeparator ? (length + 1) : length);
		EnsureCapacity(ref buffer, num);
		entry.CopyTo(offset, buffer, 0, length);
		for (int i = 0; i < length; i++)
		{
			char c = buffer[i];
			if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
			{
				buffer[i] = '/';
			}
		}
		if (appendPathSeparator)
		{
			buffer[length] = '/';
		}
		return new string(buffer, 0, num);
	}

	private static void EnsureCapacity(ref char[] buffer, int min)
	{
		if (buffer.Length < min)
		{
			int num = buffer.Length * 2;
			if (num < min)
			{
				num = min;
			}
			ArrayPool<char>.Shared.Return(buffer);
			buffer = ArrayPool<char>.Shared.Rent(num);
		}
	}

	private static bool IsDirEmpty(DirectoryInfo possiblyEmptyDir)
	{
		using IEnumerator<string> enumerator = Directory.EnumerateFileSystemEntries(possiblyEmptyDir.FullName).GetEnumerator();
		return !enumerator.MoveNext();
	}
}
