using System;
using System.IO;
using UnityEngine;

namespace VEngine;

internal class LocalBundle : Bundle
{
	private AssetBundleCreateRequest request;

	private static LocalBundleOffsetMapper _mapper;

	private long _startTime;

	public static LocalBundleOffsetMapper Mapper
	{
		get
		{
			if (_mapper == null)
			{
				_mapper = LocalBundleOffsetMapper.Init();
			}
			return _mapper;
		}
	}

	protected override void OnLoad()
	{
		if (!Versions.CheckWhiteList || Versions.IsInWhiteList(GetOriginBundleName(info.name)))
		{
			Mapper.GetFileInfo(info.name, out var mappingRelativePath, out var offset);
			if (offset >= 0)
			{
				request = AssetBundle.LoadFromFileAsync(mappingRelativePath, 0u, (ulong)offset);
			}
			else
			{
				request = AssetBundle.LoadFromFileAsync(base.pathOrURL);
			}
		}
		else
		{
			Versions.WhiteListFailed.Add(info.name);
			Finish("LocalBundle.OnLoad, not in whitelist");
		}
	}

	private string GetOriginBundleName(string nameWithAppendHash)
	{
		int num = nameWithAppendHash.LastIndexOf('_');
		if (num <= 0)
		{
			return nameWithAppendHash;
		}
		num = nameWithAppendHash.LastIndexOf('_', num - 1);
		if (num <= 0)
		{
			return nameWithAppendHash;
		}
		return nameWithAppendHash.Substring(0, num);
	}

	public override void LoadImmediate()
	{
		if (!base.isDone)
		{
			base.assetBundle = request.assetBundle;
			if (base.assetBundle == null)
			{
				Finish("LocalBundle.LoadImmediate, assetBundle == null");
				return;
			}
			Finish();
			request = null;
		}
	}

	private void OnLoaded()
	{
		if (request == null)
		{
			Finish("LocalBundle.OnLoaded, request == null");
			return;
		}
		base.assetBundle = request.assetBundle;
		request = null;
		if (base.assetBundle == null)
		{
			Finish("LocalBundle.OnLoaded, assetBundle == null");
		}
		else
		{
			Finish();
		}
	}

	protected override void OnUpdate()
	{
		if (base.status != LoadableStatus.Loading)
		{
			return;
		}
		if (request == null)
		{
			OnLoad();
			return;
		}
		base.progress = request.progress;
		if (request.isDone)
		{
			OnLoaded();
		}
	}

	protected override void ForceStop()
	{
		if (request != null)
		{
			AssetBundle assetBundle = request.assetBundle;
			if ((bool)assetBundle)
			{
				assetBundle.Unload(unloadAllLoadedObjects: false);
			}
			request = null;
		}
	}

	protected override void OnComplete()
	{
		if (base.status != LoadableStatus.FailedToLoad)
		{
			return;
		}
		try
		{
			Mapper.GetFileInfo(info.name, out var mappingRelativePath, out var offset);
			if (offset >= 0)
			{
				Logger.E($"Unable to load LocalBundle {base.pathOrURL}, relativePath: {mappingRelativePath}, offset: {offset}");
				return;
			}
			FileInfo fileInfo = new FileInfo(base.pathOrURL);
			if (fileInfo.Exists)
			{
				long length = fileInfo.Length;
				uint num = Utility.ComputeCRC32(base.pathOrURL);
				Logger.E($"Unable to load LocalBundle {base.pathOrURL}, size: {length}, crc: {num}, {info.crc}");
				if (num != info.crc)
				{
					Versions.AddErrorBundle(base.pathOrURL);
				}
			}
			else
			{
				Logger.E("Unable to load LocalBundle " + base.pathOrURL + " not exist");
			}
		}
		catch (Exception arg)
		{
			Logger.E($"Unable to load LocalBundle {base.pathOrURL}, error: {arg}");
		}
	}
}
