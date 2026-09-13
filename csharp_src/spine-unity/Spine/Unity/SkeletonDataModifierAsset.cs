using UnityEngine;

namespace Spine.Unity;

public abstract class SkeletonDataModifierAsset : ScriptableObject
{
	public abstract void Apply(SkeletonData skeletonData);
}
