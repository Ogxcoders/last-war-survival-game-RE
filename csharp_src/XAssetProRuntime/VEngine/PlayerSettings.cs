using System.Collections.Generic;
using UnityEngine;

namespace VEngine;

public class PlayerSettings : ScriptableObject
{
	public List<string> assets = new List<string>();

	public List<string> whiteList = new List<string>();

	[Tooltip("初始化的清单配置，配置包外的清单，底层会自动按需更新下载")]
	public List<string> manifests = new List<string>();

	[Tooltip("后台更新的清单配置，游戏启动时不下载更新，在进入游戏后，在后台更新")]
	public string bkgroundManifest;

	[Tooltip("包内清单配置，资源不热更，随包更新")]
	public string packageResManifest;
}
