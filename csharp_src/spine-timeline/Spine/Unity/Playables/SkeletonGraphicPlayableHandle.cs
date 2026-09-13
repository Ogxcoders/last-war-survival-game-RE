using UnityEngine;

namespace Spine.Unity.Playables;

[AddComponentMenu("Spine/Playables/SkeletonGraphic Playable Handle (Playables)")]
public class SkeletonGraphicPlayableHandle : SpinePlayableHandleBase
{
	public SkeletonGraphic skeletonGraphic;

	public override Skeleton Skeleton => skeletonGraphic.Skeleton;

	public override SkeletonData SkeletonData => skeletonGraphic.Skeleton.Data;

	private void Awake()
	{
		InitializeReference();
	}

	private void InitializeReference()
	{
		if ((Object)(object)skeletonGraphic == null)
		{
			skeletonGraphic = GetComponent<SkeletonGraphic>();
		}
	}
}
