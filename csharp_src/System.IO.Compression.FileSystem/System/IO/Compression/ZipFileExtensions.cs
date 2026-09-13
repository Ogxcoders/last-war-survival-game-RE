using System.ComponentModel;

namespace System.IO.Compression;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class ZipFileExtensions
{
	public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName)
	{
		return DoCreateEntryFromFile(destination, sourceFileName, entryName, null);
	}

	public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName, CompressionLevel compressionLevel)
	{
		return DoCreateEntryFromFile(destination, sourceFileName, entryName, compressionLevel);
	}

	public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName)
	{
		source.ExtractToDirectory(destinationDirectoryName, overwrite: false);
	}

	public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName, bool overwrite)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (destinationDirectoryName == null)
		{
			throw new ArgumentNullException("destinationDirectoryName");
		}
		string text = Directory.CreateDirectory(destinationDirectoryName).FullName;
		if (!text.EndsWith(Path.DirectorySeparatorChar))
		{
			text += Path.DirectorySeparatorChar;
		}
		foreach (ZipArchiveEntry entry in source.Entries)
		{
			string fullPath = Path.GetFullPath(Path.Combine(text, entry.FullName));
			if (!fullPath.StartsWith(text, System.IO.PathInternal.StringComparison))
			{
				throw new IOException("Extracting Zip entry would have resulted in a file outside the specified destination directory.");
			}
			if (Path.GetFileName(fullPath).Length == 0)
			{
				if (entry.Length != 0L)
				{
					throw new IOException("Zip entry name ends in directory separator character but contains data.");
				}
				Directory.CreateDirectory(fullPath);
			}
			else
			{
				Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
				entry.ExtractToFile(fullPath, overwrite);
			}
		}
	}

	internal static ZipArchiveEntry DoCreateEntryFromFile(ZipArchive destination, string sourceFileName, string entryName, CompressionLevel? compressionLevel)
	{
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (sourceFileName == null)
		{
			throw new ArgumentNullException("sourceFileName");
		}
		if (entryName == null)
		{
			throw new ArgumentNullException("entryName");
		}
		using Stream stream = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: false);
		ZipArchiveEntry zipArchiveEntry = (compressionLevel.HasValue ? destination.CreateEntry(entryName, compressionLevel.Value) : destination.CreateEntry(entryName));
		DateTime dateTime = File.GetLastWriteTime(sourceFileName);
		if (dateTime.Year < 1980 || dateTime.Year > 2107)
		{
			dateTime = new DateTime(1980, 1, 1, 0, 0, 0);
		}
		zipArchiveEntry.LastWriteTime = dateTime;
		using (Stream destination2 = zipArchiveEntry.Open())
		{
			stream.CopyTo(destination2);
		}
		return zipArchiveEntry;
	}

	public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName)
	{
		source.ExtractToFile(destinationFileName, overwrite: false);
	}

	public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName, bool overwrite)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (destinationFileName == null)
		{
			throw new ArgumentNullException("destinationFileName");
		}
		FileMode mode = ((!overwrite) ? FileMode.CreateNew : FileMode.Create);
		using (Stream destination = new FileStream(destinationFileName, mode, FileAccess.Write, FileShare.None, 4096, useAsync: false))
		{
			using Stream stream = source.Open();
			stream.CopyTo(destination);
		}
		File.SetLastWriteTime(destinationFileName, source.LastWriteTime.DateTime);
	}
}
