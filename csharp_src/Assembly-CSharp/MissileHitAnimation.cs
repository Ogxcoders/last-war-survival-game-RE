using Spine;
using Spine.Unity;
using UnityEngine;

public class MissileHitAnimation : MonoBehaviour
{
	public class Param
	{
		public Vector3 pos;
	}

	private SkeletonAnimation skeletonAnimation;

	protected internal void CSShow(object userData)
	{
		if (userData is Param param)
		{
			base.transform.position = param.pos;
		}
		skeletonAnimation = GetComponent<SkeletonAnimation>();
		if (skeletonAnimation != null)
		{
			skeletonAnimation.AnimationState.End += OnAnimationDone;
			skeletonAnimation.AnimationState.Complete += OnAnimationDone;
		}
	}

	private void OnAnimationDone(TrackEntry trackEntry)
	{
		skeletonAnimation.AnimationState.End -= OnAnimationDone;
		skeletonAnimation.AnimationState.Complete -= OnAnimationDone;
		Object.Destroy(base.gameObject);
		MissileHitAniContro.Instance.HideAni(base.gameObject);
	}
}
