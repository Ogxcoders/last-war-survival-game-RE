using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace VEngine;

public class DownloadManifestFile : ManifestFile
{
	private Download download;

	public string versionName { get; set; }

	public bool isRetry
	{
		get
		{
			if (download != null)
			{
				return download.isRetry;
			}
			return false;
		}
	}

	public static string GetTemporaryPath(string filename)
	{
		return Versions.GetTemporaryPath($"Download/{filename}");
	}

	protected override void OnLoad()
	{
		base.OnLoad();
		versionName = Manifest.GetVersionFile(base.name);
		string temporaryPath = GetTemporaryPath(versionName);
		if (!File.Exists(temporaryPath))
		{
			Finish("version not exist.");
			return;
		}
		versionFile = ManifestVersionFile.Load(temporaryPath);
		base.pathOrURL = Versions.GetDownloadURL(string.Format("{0}{1}_v{2}", base.name, "_small", versionFile.version));
		base.status = LoadableStatus.CheckVersion;
	}

	protected override void OnUpdate()
	{
		switch (base.status)
		{
		case LoadableStatus.CheckVersion:
			UpdateVersion();
			break;
		case LoadableStatus.Downloading:
			UpdateDownloading();
			break;
		case LoadableStatus.Loading:
		{
			string temporaryPath = GetTemporaryPath(base.name);
			base.target.Load(temporaryPath);
			Finish();
			break;
		}
		}
	}

	public override void Override()
	{
		string[] array = base.name.Split(new char[1] { '_' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length > 1)
		{
			string text = array[0];
			base.target.name = text;
		}
		string temporaryPath = GetTemporaryPath(base.name);
		string text2 = Versions.GetDownloadDataPath(base.name).Replace(base.name, base.target.name);
		if (File.Exists(temporaryPath))
		{
			File.Copy(temporaryPath, text2, overwrite: true);
		}
		temporaryPath = GetTemporaryPath(versionName);
		if (File.Exists(temporaryPath))
		{
			string destFileName = Versions.GetDownloadDataPath(versionName).Replace(base.name, base.target.name);
			File.Copy(temporaryPath, destFileName, overwrite: true);
		}
		if (Versions.IsChanged(base.target.name) && File.Exists(text2))
		{
			base.target.Load(text2);
			Versions.Override(base.target);
		}
	}

	private void UpdateDownloading()
	{
		if (download == null)
		{
			Finish("request == nul with " + base.status);
			return;
		}
		base.progress = download.progress;
		if (download.isDone)
		{
			if (!string.IsNullOrEmpty(download.error))
			{
				Finish(download.error);
				return;
			}
			string temporaryPath = GetTemporaryPath(base.name + "_small");
			new FastZip().ExtractZip(temporaryPath, Path.GetDirectoryName(temporaryPath), "");
			download = null;
			base.status = LoadableStatus.Loading;
		}
	}

	private void UpdateVersion()
	{
		string temporaryPath = GetTemporaryPath(base.name);
		if (Versions.Manifests.Exists((Manifest m) => m.version == versionFile.version && base.name.Contains(m.name)))
		{
			if (File.Exists(temporaryPath))
			{
				File.Delete(temporaryPath);
			}
			Finish();
			return;
		}
		if (File.Exists(temporaryPath))
		{
			using FileStream stream = File.OpenRead(temporaryPath);
			if (Utility.ComputeCRC32(stream) == versionFile.crc)
			{
				base.status = LoadableStatus.Loading;
				return;
			}
		}
		if (File.Exists(temporaryPath))
		{
			File.Delete(temporaryPath);
		}
		string temporaryPath2 = GetTemporaryPath(base.name + "_small");
		download = Download.DownloadAsync(base.pathOrURL, temporaryPath2, null, 0uL);
		base.status = LoadableStatus.Downloading;
	}
}
