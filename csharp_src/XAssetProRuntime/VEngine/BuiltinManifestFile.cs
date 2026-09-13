using System.IO;
using GameFramework;
using UnityEngine.Networking;

namespace VEngine;

public class BuiltinManifestFile : ManifestFile
{
	private UnityWebRequest request;

	private void DownloadAsync(string url, string savePath)
	{
		if (File.Exists(savePath))
		{
			File.Delete(savePath);
		}
		request = UnityWebRequest.Get(url);
		request.downloadHandler = new DownloadHandlerFile(savePath);
		request.SendWebRequest();
	}

	private static string GetTemporaryPath(string filename)
	{
		return Versions.GetTemporaryPath($"Builtin/{filename}");
	}

	public override void Override()
	{
		if (versionFile == null)
		{
			return;
		}
		Versions.Override(base.target);
		string downloadDataPath = Versions.GetDownloadDataPath(Manifest.GetVersionFile(base.target.name));
		ManifestVersionFile manifestVersionFile = ManifestVersionFile.Load(downloadDataPath);
		if (manifestVersionFile.version > versionFile.version)
		{
			downloadDataPath = Versions.GetDownloadDataPath(base.target.name);
			if (File.Exists(downloadDataPath))
			{
				using FileStream stream = File.OpenRead(downloadDataPath);
				if (Utility.ComputeCRC32(stream) == manifestVersionFile.crc)
				{
					base.target.Load(downloadDataPath);
					return;
				}
			}
		}
		downloadDataPath = GetTemporaryPath(base.name);
		base.target.Load(downloadDataPath);
	}

	protected override void OnLoad()
	{
		base.OnLoad();
		base.pathOrURL = Versions.GetPlayerDataURL(base.name);
		string text = Manifest.GetVersionFile(base.name);
		string playerDataURL = Versions.GetPlayerDataURL(text);
		DownloadAsync(playerDataURL, GetTemporaryPath(text));
		base.status = LoadableStatus.CheckVersion;
	}

	protected override void SyncOnLoad()
	{
		base.OnLoad();
		string downloadDataPath = Versions.GetDownloadDataPath(base.name);
		string path = Manifest.GetVersionFile(downloadDataPath);
		string playerDataPath = Versions.GetPlayerDataPath(base.name);
		string path2 = Manifest.GetVersionFile(playerDataPath);
		ManifestVersionFile manifestVersionFile = ManifestVersionFile.Load(path);
		ManifestVersionFile manifestVersionFile2 = ManifestVersionFile.Load(path2);
		bool flag = false;
		Log.Info($"Compare {base.name} Version download:{manifestVersionFile.version}, package:{manifestVersionFile2.version}");
		if (manifestVersionFile.version > manifestVersionFile2.version)
		{
			uint fileCrc = GetFileCrc(downloadDataPath);
			if (fileCrc == manifestVersionFile.crc)
			{
				Log.Info("Use Download Manifest " + downloadDataPath);
				base.target.Load(downloadDataPath);
				flag = true;
			}
			else
			{
				Log.Error($"Check manifest crc error:{base.name}[{manifestVersionFile.version}]::{fileCrc} : {manifestVersionFile.crc}");
			}
		}
		if (!flag)
		{
			Log.Info($"Use {base.name} Package Manifest {playerDataPath} [{manifestVersionFile2.version}]");
			base.target.Load(playerDataPath);
		}
		Versions.Override(base.target);
		Finish();
	}

	private uint GetFileCrc(string path)
	{
		uint num = 0u;
		using FileStream stream = File.OpenRead(path);
		return Utility.ComputeCRC32(stream);
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
			Finish();
			break;
		}
	}

	private void UpdateDownloading()
	{
		if (request == null)
		{
			Finish("request == nul with " + base.status);
			return;
		}
		base.progress = 0.2f + request.downloadProgress;
		if (request.isDone)
		{
			if (!string.IsNullOrEmpty(request.error))
			{
				Finish(request.error);
				return;
			}
			request.Dispose();
			request = null;
			base.status = LoadableStatus.Loading;
		}
	}

	private void UpdateVersion()
	{
		if (request == null)
		{
			Finish("request == null with " + base.status);
			return;
		}
		base.progress = 0.2f * request.downloadProgress;
		if (!request.isDone)
		{
			return;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			Finish(request.error);
			return;
		}
		string temporaryPath = GetTemporaryPath(Manifest.GetVersionFile(base.name));
		if (!File.Exists(temporaryPath))
		{
			Finish("version not exist.");
			return;
		}
		versionFile = ManifestVersionFile.Load(temporaryPath);
		request.Dispose();
		request = null;
		string temporaryPath2 = GetTemporaryPath(base.name);
		if (File.Exists(temporaryPath2))
		{
			using FileStream stream = File.OpenRead(temporaryPath2);
			if (Utility.ComputeCRC32(stream) == versionFile.crc)
			{
				base.status = LoadableStatus.Loading;
				return;
			}
		}
		if (File.Exists(temporaryPath2))
		{
			File.Delete(temporaryPath2);
		}
		DownloadAsync(base.pathOrURL, temporaryPath2);
		base.status = LoadableStatus.Downloading;
	}
}
