using UnityEngine;

namespace Spine.Unity;

[ExecuteAlways]
[RequireComponent(typeof(SkeletonUtilityBone))]
[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonUtilityConstraint")]
public abstract class SkeletonUtilityConstraint : MonoBehaviour
{
	protected SkeletonUtilityBone bone;

	protected SkeletonUtility hierarchy;

	protected virtual void OnEnable()
	{
		bone = GetComponent<SkeletonUtilityBone>();
		hierarchy = base.transform.GetComponentInParent<SkeletonUtility>();
		hierarchy.RegisterConstraint(this);
	}

	protected virtual void OnDisable()
	{
		hierarchy.UnregisterConstraint(this);
	}

	public abstract void DoUpdate();
}
