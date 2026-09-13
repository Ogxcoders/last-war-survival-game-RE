using System;
using System.Collections.Generic;
using System.IO;
using BaseUtils;
using UnityEngine;
using UnityEngine.Networking;

namespace VEngine;

public sealed class InitializeVersions : Operation
{
	private readonly List<ManifestFile> assets = new List<ManifestFile>();

	private readonly List<string> errors = new List<string>();

	public string[] manifests;

	public string bkgroundManifest;

	public string packageResManifest;

	private bool _offsetDone;

	private bool _bundleOffsetDone;

	private bool _aliasOffsetDone;

	public override void Start()
	{
		if (Versions.syncLoadPackageManifest)
		{
			bool flag = SyncReader.CheckAndInit();
			Versions.syncLoadPackageManifest &= flag;
		}
		string[] array;
		if (Versions.syncLoadPackageManifest)
		{
			array = manifests;
			for (int i = 0; i < array.Length; i++)
			{
				ManifestFile.Load(array[i], builtin: true);
			}
			Finish();
			Complete();
			return;
		}
		base.Start();
		array = manifests;
		foreach (string name in array)
		{
			assets.Add(ManifestFile.LoadAsync(name, builtin: true));
		}
		_offsetDone = false;
		_bundleOffsetDone = false;
		_aliasOffsetDone = false;
		(string, Action)[] array2 = new(string, Action)[2]
		{
			(CommonUtils.BUNDLE_OFFSET_TABLE_FILE, delegate
			{
				_bundleOffsetDone = true;
				_offsetDone = _bundleOffsetDone && _aliasOffsetDone;
			}),
			(CommonUtils.BUNDLE_ALIAS_OFFSET_TABLE_FILE, delegate
			{
				_aliasOffsetDone = true;
				_offsetDone = _bundleOffsetDone && _aliasOffsetDone;
			})
		};
		for (int i = 0; i < array2.Length; i++)
		{
			(string, Action) offset = array2[i];
			string item = offset.Item1;
			string dstPath = Path.Combine(Application.persistentDataPath, item);
			CommonUtils.DeleteOffsetFile();
			string playerDataPath = Versions.GetPlayerDataPath(item);
			Debug.Log("Copy " + item + " form " + playerDataPath + " to " + dstPath);
			WebRequestManagerProxy.Instance.Get(playerDataPath, delegate(UnityWebRequest request, bool hasErr, object userdata)
			{
				if (!hasErr)
				{
					if (request.isDone)
					{
						File.WriteAllBytes(dstPath, request.downloadHandler.data);
					}
				}
				else
				{
					Debug.LogError(request.error);
				}
				offset.Item2();
			});
		}
	}

	protected override void Update()
	{
		OperationStatus operationStatus = base.status;
		if (operationStatus != OperationStatus.Processing || !_offsetDone)
		{
			return;
		}
		foreach (ManifestFile asset in assets)
		{
			if (!asset.isDone)
			{
				return;
			}
		}
		foreach (ManifestFile asset2 in assets)
		{
			if (asset2.status != LoadableStatus.Unloaded)
			{
				asset2.Override();
				asset2.Release();
			}
			else
			{
				errors.Add("Failed to load " + asset2.pathOrURL + " with " + asset2.error);
			}
		}
		assets.Clear();
		Finish((errors.Count == 0) ? null : string.Join("\n", errors.ToArray()));
	}
}
