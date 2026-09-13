using UnityEngine;

namespace Spine.Unity;

[CreateAssetMenu(menuName = "Spine/Animation Reference Asset", order = 100)]
public class AnimationReferenceAsset : ScriptableObject, IHasSkeletonDataAsset, ISpineComponent
{
	private const bool QuietSkeletonData = true;

	[SerializeField]
	protected SkeletonDataAsset skeletonDataAsset;

	[SerializeField]
	[SpineAnimation("", "", true, false)]
	protected string animationName;

	private Animation animation;

	public SkeletonDataAsset SkeletonDataAsset => skeletonDataAsset;

	public Animation Animation
	{
		get
		{
			if (animation == null)
			{
				Initialize();
			}
			return animation;
		}
	}

	public void Initialize()
	{
		if (!(skeletonDataAsset == null))
		{
			animation = skeletonDataAsset.GetSkeletonData(quiet: true)?.FindAnimation(animationName);
			if (animation == null)
			{
				Debug.LogWarningFormat("Animation '{0}' not found in SkeletonData : {1}.", animationName, skeletonDataAsset.name);
			}
		}
	}

	public static implicit operator Animation(AnimationReferenceAsset asset)
	{
		return asset.Animation;
	}
}
