using System;
using System.IO;
using System.IO.Compression;
using GameFramework;
using VEngine;

internal class LoadManifestTask : IQueuedThreadTask
{
	private byte[] data;

	private MemoryStream contentStream;

	public string name { get; private set; }

	public int version { get; private set; }

	public Manifest manifest { get; private set; }

	public LoadManifestTask(string name, int version, byte[] data)
	{
		this.name = name;
		this.version = version;
		this.data = data;
	}

	public void Process()
	{
		contentStream = Unzip();
		Load();
	}

	private MemoryStream Unzip()
	{
		using (MemoryStream stream = new MemoryStream(data))
		{
			using ZipArchive zipArchive = new ZipArchive(stream);
			ZipArchiveEntry entry = zipArchive.GetEntry(name);
			if (entry != null)
			{
				using (Stream stream2 = entry.Open())
				{
					MemoryStream memoryStream = new MemoryStream();
					stream2.CopyTo(memoryStream);
					memoryStream.Position = 0L;
					return memoryStream;
				}
			}
		}
		data = null;
		return null;
	}

	private void Load()
	{
		if (contentStream == null)
		{
			return;
		}
		try
		{
			manifest = new Manifest();
			manifest.thread = true;
			using StreamReader sr = new StreamReader(contentStream);
			manifest.ParseManifest(sr);
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		contentStream.Dispose();
	}
}
