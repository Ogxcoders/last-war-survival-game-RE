using System.Collections.Generic;
using System.IO;

namespace VEngine;

public sealed class ClearVersions : Operation
{
	private readonly List<string> usedFiles = new List<string>();

	private string[] allFiles;

	private int index;

	public override void Start()
	{
		base.Start();
		allFiles = Directory.GetFiles(Versions.DownloadDataPath);
		index = 0;
		foreach (Manifest manifest in Versions.Manifests)
		{
			usedFiles.Add(Versions.GetDownloadDataSystemPath(manifest.name));
			usedFiles.Add(Versions.GetDownloadDataSystemPath(Manifest.GetVersionFile(manifest.name)));
			foreach (BundleInfo bundle in manifest.bundles)
			{
				if (!string.IsNullOrEmpty(bundle.name))
				{
					usedFiles.Add(Versions.GetDownloadDataSystemPath(bundle.name));
				}
			}
		}
	}

	protected override void Update()
	{
		OperationStatus operationStatus = base.status;
		if (operationStatus != OperationStatus.Processing)
		{
			return;
		}
		if (allFiles == null)
		{
			Finish();
			return;
		}
		int num = allFiles.Length;
		if (index >= num)
		{
			Finish();
			return;
		}
		while (index < num)
		{
			base.progress = (float)(num - (index + 1)) / (float)num;
			string text = allFiles[index];
			if (!usedFiles.Contains(text) && File.Exists(text))
			{
				File.Delete(text);
			}
			index++;
			if (Updater.busy)
			{
				break;
			}
		}
		if (index >= num)
		{
			Finish();
		}
	}
}
