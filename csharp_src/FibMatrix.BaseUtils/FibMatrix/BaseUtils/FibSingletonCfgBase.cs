using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.BaseUtils;

[HelpURL("https://rivergame.feishu.cn/wiki/wikcnOfVFdt3OumjgiL2CtJ3PIO")]
public class FibSingletonCfgBase : ScriptableObject
{
	private const string k_SaveDir = "Assets/Plugins/FibSettings";

	[HideInInspector]
	public bool runtimeCfg;

	private static HashSet<string> s_ErrorAssets = new HashSet<string>();

	private static string GetAssetPath(string fileName, bool runtimeAsset)
	{
		string text = (runtimeAsset ? "Resources" : "Editor");
		return "Assets/Plugins/FibSettings/" + text + "/" + fileName + ".asset";
	}

	protected static T LoadOrCreate<T>(bool runtimeAsset) where T : FibSingletonCfgBase
	{
		FibSingletonCfgBase fibSingletonCfgBase = null;
		if (runtimeAsset)
		{
			fibSingletonCfgBase = Resources.Load<T>(typeof(T).Name);
		}
		return fibSingletonCfgBase as T;
	}
}
