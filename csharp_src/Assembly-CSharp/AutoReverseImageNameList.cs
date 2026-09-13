using System;
using System.Collections.Generic;
using VEngine;

public static class AutoReverseImageNameList
{
	public static HashSet<string> AutoReverseImageName = new HashSet<string>();

	public static HashSet<string> DontAutoReverseRawImageName = new HashSet<string>();

	private static Asset dataListAsset1;

	private static Asset dataListAsset2;

	public static void Init()
	{
		if (dataListAsset1 == null || dataListAsset2 == null)
		{
			Load();
		}
	}

	private static void Load()
	{
		if (GameEntry.Resource == null)
		{
			return;
		}
		dataListAsset1 = GameEntry.Resource.LoadAssetAsync("Assets/Main/Prefabs/UI/Arabic/ArabicMirrorAutoReverseImageList.asset", typeof(ArabicMirrorAutoReverseImageListData));
		Asset asset = dataListAsset1;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			AutoReverseImageName.Clear();
			string[] reverseImageList = dataListAsset1.Get<ArabicMirrorAutoReverseImageListData>().reverseImageList;
			foreach (string item in reverseImageList)
			{
				AutoReverseImageName.Add(item);
			}
			dataListAsset1.Release();
		});
		dataListAsset2 = GameEntry.Resource.LoadAssetAsync("Assets/Main/Prefabs/UI/Arabic/ArabicMirrorDontAutoReverseRawImageList.asset", typeof(ArabicMirrorAutoReverseImageListData));
		Asset asset2 = dataListAsset2;
		asset2.completed = (Action<Asset>)Delegate.Combine(asset2.completed, (Action<Asset>)delegate
		{
			DontAutoReverseRawImageName.Clear();
			string[] reverseImageList = dataListAsset2.Get<ArabicMirrorAutoReverseImageListData>().reverseImageList;
			foreach (string item in reverseImageList)
			{
				DontAutoReverseRawImageName.Add(item);
			}
			dataListAsset2.Release();
		});
	}

	public static bool IsAutoReverseImage(string name)
	{
		if (dataListAsset1 == null)
		{
			Load();
			return false;
		}
		return AutoReverseImageName.Contains(name);
	}

	public static bool IsDontAutoReverseRawImage(string name)
	{
		if (dataListAsset2 == null)
		{
			Load();
			return false;
		}
		return DontAutoReverseRawImageName.Contains(name);
	}
}
