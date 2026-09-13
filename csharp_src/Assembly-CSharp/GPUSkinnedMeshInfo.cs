using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GPUSkinnedMeshInfo
{
	public string[] animStateNames;

	public string[] animAssetNames;

	public GPUSkinAnimTexData animTexData;

	private Dictionary<int, GPUSkinAnimTexData.ClipData> _clipMap;

	private Dictionary<string, GPUSkinAnimTexData.ClipData> _stateNameToClipIndex;

	public Dictionary<int, GPUSkinAnimTexData.ClipData> clipMap
	{
		get
		{
			if (_clipMap == null)
			{
				_clipMap = new Dictionary<int, GPUSkinAnimTexData.ClipData>(animTexData.ClipDatas.Count);
				foreach (GPUSkinAnimTexData.ClipData clipData in animTexData.ClipDatas)
				{
					clipData.nameHash = Animator.StringToHash(clipData.name);
					_clipMap.Add(clipData.nameHash, clipData);
				}
			}
			return _clipMap;
		}
	}

	public GPUSkinAnimTexData.ClipData GetClipByStateIndex(string stateName)
	{
		if (_stateNameToClipIndex == null)
		{
			_stateNameToClipIndex = new Dictionary<string, GPUSkinAnimTexData.ClipData>(animStateNames.Length);
		}
		if (_stateNameToClipIndex.TryGetValue(stateName, out var value))
		{
			return value;
		}
		if (animStateNames.Length != animAssetNames.Length)
		{
			Debug.LogError("stete name and clip name mismatch: " + animTexData.name);
			return animTexData.ClipDatas[0];
		}
		int num = Array.FindIndex(animStateNames, (string s) => s == stateName);
		if (num < 0)
		{
			Debug.LogError("state not found: " + stateName + ", use default instead");
			num = 0;
		}
		string clipName = animAssetNames[num];
		int num2 = animTexData.ClipDatas.FindIndex((GPUSkinAnimTexData.ClipData data) => data.name == clipName);
		if (num2 < 0)
		{
			num2 = 0;
			Debug.LogError("clip not found in animTexData: " + animTexData.name + ", state:" + stateName + ", clipName:" + clipName);
		}
		GPUSkinAnimTexData.ClipData clipData = animTexData.ClipDatas[num2];
		_stateNameToClipIndex[stateName] = clipData;
		return clipData;
	}
}
