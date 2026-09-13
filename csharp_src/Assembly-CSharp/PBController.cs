using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using VEngine;
using XLua;
using XLua.LuaDLL;

public class PBController
{
	private List<string> pbFiles = new List<string>(10);

	private List<Asset> preloadRequest = new List<Asset>();

	private List<Asset> loadRequest = new List<Asset>();

	public bool IsInitDone { get; private set; }

	public bool IsInitSuccess { get; private set; }

	public int loadSuccessCount { get; private set; }

	public int preloadCount { get; private set; }

	public void Reload()
	{
		IsInitDone = false;
		IsInitSuccess = true;
		loadSuccessCount = 0;
		LuaTable inPath = GameEntry.Lua.Env.Global.GetInPath<LuaTable>("PBController.ProtoConfig");
		int num = 1;
		string text = "";
		pbFiles.Clear();
		do
		{
			text = inPath.Get<string>(num++);
			if (!text.IsNullOrEmpty())
			{
				pbFiles.Add(text);
			}
		}
		while (!text.IsNullOrEmpty());
		loadRequest.ForEach(delegate(Asset i)
		{
			i.Release();
		});
		loadRequest.Clear();
		preloadCount = 0;
		preloadRequest.ForEach(delegate(Asset i)
		{
			i.Release();
		});
		preloadRequest.Clear();
		foreach (string pbFile in pbFiles)
		{
			Asset asset = GameEntry.Resource.LoadAssetAsync(pbFile, typeof(TextAsset));
			loadRequest.Add(asset);
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset request)
			{
				if (!request.isError)
				{
					OnLoadAssets(request);
				}
				else
				{
					string text2 = "LoadTable error " + request.pathOrURL + " " + request.error;
					GameEntry.Lua.Call("CSharpCallLuaInterface.SendErrorMessageToServer", text2);
					Log.Error(text2);
				}
				preloadCount++;
				if (preloadCount == pbFiles.Count)
				{
					IsInitDone = true;
					IsInitSuccess = preloadRequest.TrueForAll((Asset i) => !i.isError);
					loadRequest.ForEach(delegate(Asset i)
					{
						i.Release();
					});
					loadRequest.Clear();
					preloadRequest.ForEach(delegate(Asset i)
					{
						i.Release();
					});
					preloadRequest.Clear();
				}
			});
		}
	}

	private void OnLoadAssets(Asset request)
	{
		byte[] bytes = ((TextAsset)request.asset).bytes;
		if (Lua.Load_luaUtil(GameEntry.Lua.Env.L, bytes, bytes.Length) > 0)
		{
			loadSuccessCount++;
		}
	}
}
