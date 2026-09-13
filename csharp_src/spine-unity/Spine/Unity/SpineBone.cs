namespace Spine.Unity;

public class SpineBone : SpineAttributeBase
{
	public SpineBone(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false)
	{
		base.startsWith = startsWith;
		base.dataField = dataField;
		base.includeNone = includeNone;
		base.fallbackToTextField = fallbackToTextField;
	}

	public static Bone GetBone(string boneName, SkeletonRenderer renderer)
	{
		if (renderer.skeleton != null)
		{
			return renderer.skeleton.FindBone(boneName);
		}
		return null;
	}

	public static BoneData GetBoneData(string boneName, SkeletonDataAsset skeletonDataAsset)
	{
		return skeletonDataAsset.GetSkeletonData(quiet: true).FindBone(boneName);
	}
}
