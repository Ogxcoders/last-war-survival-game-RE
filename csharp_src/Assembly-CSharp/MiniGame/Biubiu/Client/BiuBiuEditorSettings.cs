using MiniGame.Core.Client;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

[CreateAssetMenu(fileName = "BiuBiuEditorSettings", menuName = "ScriptableObjects/BiuBiuEditorSettings", order = 0)]
public class BiuBiuEditorSettings : EditorEcsSettings
{
	public string MapPrefabPath = "Assets/Main/MiniGame/biubiu/Resource/Prefab";

	public string GetMapPathWithoutExtension(string mapName)
	{
		return MapPrefabPath + "/" + mapName;
	}

	public string GetMapPrefabPath(string prefabName)
	{
		return MapPrefabPath + "/" + prefabName + ".prefab";
	}
}
