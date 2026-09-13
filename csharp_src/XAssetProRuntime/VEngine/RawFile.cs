using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

namespace VEngine;

public sealed class RawFile : Loadable
{
	private static readonly Dictionary<string, RawFile> Cache = new Dictionary<string, RawFile>();

	private static readonly List<RawFile> Unused = new List<RawFile>();

	public Action<RawFile> completed;

	private BundleInfo info;

	public string name;

	private UnityWebRequest request;

	public string savePath { get; private set; }

	public byte[] bytes { get; private set; }

	protected override void OnLoad()
	{
		if (!Versions.GetDependencies(name, out info, out var _))
		{
			Finish("File not found.");
			return;
		}
		base.pathOrURL = Versions.GetBundlePathOrURL(info);
		savePath = Versions.GetDownloadDataPath(info.name);
		string directoryName = Path.GetDirectoryName(savePath);
		if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		base.status = LoadableStatus.CheckVersion;
	}

	protected override void OnUnload()
	{
		if (request != null)
		{
			request.Dispose();
			request = null;
		}
		if (bytes != null)
		{
			bytes = null;
		}
	}

	protected override void OnComplete()
	{
		if (completed != null)
		{
			Action<RawFile> value = completed;
			if (completed != null)
			{
				completed(this);
			}
			completed = (Action<RawFile>)Delegate.Remove(completed, value);
		}
	}

	protected override void OnUpdate()
	{
		switch (base.status)
		{
		case LoadableStatus.CheckVersion:
			UpdateChecking();
			break;
		case LoadableStatus.Loading:
			UpdateLoading();
			break;
		}
	}

	protected override void OnUnused()
	{
		completed = null;
		Unused.Add(this);
	}

	private void UpdateLoading()
	{
		if (request == null)
		{
			Finish("request == null");
		}
		else
		{
			if (!request.isDone)
			{
				return;
			}
			if (!string.IsNullOrEmpty(request.error))
			{
				Finish(request.error);
				return;
			}
			if (File.Exists(savePath))
			{
				bytes = File.ReadAllBytes(savePath);
			}
			Finish();
		}
	}

	private void UpdateChecking()
	{
		if (File.Exists(savePath))
		{
			using (FileStream fileStream = File.OpenRead(savePath))
			{
				if (info.size == (ulong)fileStream.Length && Utility.ComputeCRC32(fileStream) == info.crc)
				{
					Finish();
					return;
				}
			}
			File.Delete(savePath);
		}
		request = UnityWebRequest.Get(base.pathOrURL);
		request.downloadHandler = new DownloadHandlerFile(savePath);
		request.SendWebRequest();
		base.status = LoadableStatus.Loading;
	}

	public static void UpdateFiles()
	{
		for (int i = 0; i < Unused.Count; i++)
		{
			RawFile rawFile = Unused[i];
			if (!Updater.busy)
			{
				if (rawFile.isDone)
				{
					Unused.RemoveAt(i);
					Cache.Remove(rawFile.name);
					i--;
					rawFile.Unload();
				}
				continue;
			}
			break;
		}
	}

	public static RawFile LoadAsync(string filename)
	{
		if (!Cache.TryGetValue(filename, out var value))
		{
			value = new RawFile
			{
				name = filename
			};
			Cache.Add(filename, value);
		}
		value.Load();
		return value;
	}
}
