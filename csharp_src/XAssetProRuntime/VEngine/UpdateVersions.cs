using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VEngine;

public sealed class UpdateVersions : Operation
{
	public string[] manifests;

	public readonly List<ManifestFile> assets = new List<ManifestFile>();

	private Download downloadVersion;

	public List<Manifest> Manifests => assets.Select((ManifestFile a) => a.target).ToList();

	public bool isRetry
	{
		get
		{
			foreach (DownloadManifestFile asset in assets)
			{
				if (asset.isRetry)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Manifest GetManifest(string name)
	{
		return assets.Find((ManifestFile a) => a.target.name == name)?.target;
	}

	public override void Start()
	{
		base.Start();
		if (Versions.SkipUpdate)
		{
			Finish();
			return;
		}
		string temporaryPath = DownloadManifestFile.GetTemporaryPath("manifest.version");
		downloadVersion = Download.DownloadAsync(Versions.CheckVersionURL, temporaryPath, null, 0uL);
	}

	public void Override()
	{
		if (Versions.SkipUpdate)
		{
			return;
		}
		foreach (ManifestFile asset in assets)
		{
			asset.Override();
		}
	}

	public void Dispose()
	{
		foreach (ManifestFile asset in assets)
		{
			if (asset.status != LoadableStatus.Unloaded)
			{
				asset.Release();
			}
		}
		assets.Clear();
	}

	protected override void Update()
	{
		OperationStatus operationStatus = base.status;
		if (operationStatus != OperationStatus.Processing)
		{
			return;
		}
		if (downloadVersion != null)
		{
			if (!downloadVersion.isDone)
			{
				return;
			}
			if (!string.IsNullOrEmpty(downloadVersion.error))
			{
				Finish(downloadVersion.error);
				return;
			}
			string temporaryPath = DownloadManifestFile.GetTemporaryPath("manifest.version");
			SplitVersionFile(temporaryPath);
			downloadVersion = null;
			string[] array = manifests;
			foreach (string name in array)
			{
				assets.Add(ManifestFile.LoadAsync(name));
			}
			return;
		}
		foreach (ManifestFile asset in assets)
		{
			if (!asset.isDone)
			{
				return;
			}
		}
		List<string> list = new List<string>();
		foreach (ManifestFile asset2 in assets)
		{
			if (asset2.status == LoadableStatus.Unloaded)
			{
				list.Add("Failed to load " + Path.GetFileName(asset2.pathOrURL) + " with " + asset2.error);
			}
		}
		Finish((list.Count == 0) ? null : string.Join("\n", list.ToArray()));
	}

	private void SplitVersionFile(string versionPath)
	{
		string[] array = File.ReadAllLines(versionPath);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Trim().Split(new char[1] { ',' });
			if (array2.Length >= 4)
			{
				string text = array2[0];
				string text2 = array2[1];
				string text3 = array2[2];
				string text4 = array2[3];
				string contents = text2 + "," + text3 + "," + text4;
				File.WriteAllText(DownloadManifestFile.GetTemporaryPath(Manifest.GetVersionFile(text.ToLower())), contents);
			}
		}
	}
}
