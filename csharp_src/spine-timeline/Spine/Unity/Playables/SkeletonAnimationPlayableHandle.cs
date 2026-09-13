using UnityEngine;

namespace Spine.Unity.Playables;

[AddComponentMenu("Spine/Playables/SkeletonAnimation Playable Handle (Playables)")]
public class SkeletonAnimationPlayableHandle : SpinePlayableHandleBase
{
	public SkeletonAnimation skeletonAnimation;

	public override Skeleton Skeleton => skeletonAnimation.Skeleton;

	public override SkeletonData SkeletonData => skeletonAnimation.Skeleton.Data;

	private void Awake()
	{
		InitializeReference();
	}

	private void InitializeReference()
	{
		if (skeletonAnimation == null)
		{
			skeletonAnimation = GetComponent<SkeletonAnimation>();
		}
	}
}
