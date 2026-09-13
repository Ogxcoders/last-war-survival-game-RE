using UnityEngine;

namespace MiniGame.Core.Client;

public abstract class EditorEcsSettings : ScriptableObject
{
	public string EditorPath;

	public string EditorScenePath;

	public string SaveTempPath;

	public string SaveConfigPath;

	public string MapFileName;
}
