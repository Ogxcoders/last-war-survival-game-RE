using UnityEngine;

namespace MoreMountains.NiceVibrations;

[CreateAssetMenu(fileName = "MMNVPathDefinition", menuName = "MoreMountains/NiceVibrations/MMNVPathDefinition")]
public class MMNVPath : ScriptableObject
{
	[Header("Swift")]
	public bool ForceAlwaysEmbedSwiftSLForFramework;

	public bool ForceAlwaysEmbedSwiftSLForMainTarget;

	[Header("Bindings")]
	public Object Header;

	public Object ModuleMap;
}
