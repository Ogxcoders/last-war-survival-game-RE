using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using VEngine;

public class DeleteUnversionedFileTask : IQueuedThreadTask
{
	private List<(string, string)> allFiles = new List<(string, string)>();

	public Dictionary<string, string> validList;

	public DeleteUnversionedFileTask()
	{
		allFiles = new List<(string, string)>();
		string[] array = new string[2] { "*.bundle", "*.raw" };
		foreach (string searchPattern in array)
		{
			foreach (string item in Directory.EnumerateFiles(Versions.DownloadDataPath, searchPattern))
			{
				allFiles.Add((Path.GetFileName(item), item));
			}
		}
	}

	public void Process()
	{
		Manifest[] array = new Manifest[2]
		{
			Versions.GetManifest(GameEntry.Resource.GameResManifestName),
			Versions.GetManifest(GameEntry.Resource.DllResManifestName)
		};
		validList = new Dictionary<string, string>();
		foreach (var allFile in allFiles)
		{
			try
			{
				BundleInfo bundleInfo = null;
				Manifest[] array2 = array;
				foreach (Manifest manifest in array2)
				{
					if (manifest != null)
					{
						bundleInfo = manifest.GetBundle(allFile.Item1);
						if (bundleInfo == null)
						{
							bundleInfo = manifest.GetBundleByAlias(allFile.Item1);
						}
						if (bundleInfo != null)
						{
							break;
						}
					}
				}
				if (bundleInfo == null)
				{
					if (File.Exists(allFile.Item2))
					{
						File.Delete(allFile.Item2);
					}
					continue;
				}
				FileInfo fileInfo = new FileInfo(allFile.Item2);
				if (fileInfo.Exists && fileInfo.Length == (long)bundleInfo.size)
				{
					validList[bundleInfo.name] = allFile.Item2;
				}
			}
			catch (Exception arg)
			{
				Log.Error($"DeleteUnversionedFileTask: {arg}");
			}
		}
	}
}
