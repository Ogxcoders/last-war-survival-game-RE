using System;

namespace UnityEngine;

[Flags]
public enum HideFlags
{
	None = 0,
	HideInHierarchy = 1,
	HideInInspector = 2,
	DontSaveInEditor = 4,
	NotEditable = 8,
	DontSaveInBuild = 0x10,
	DontUnloadUnusedAsset = 0x20,
	DontSave = 0x34,
	HideAndDontSave = 0x3D
}
