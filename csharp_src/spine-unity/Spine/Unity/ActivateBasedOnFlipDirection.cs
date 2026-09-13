using UnityEngine;

namespace Spine.Unity;

public class ActivateBasedOnFlipDirection : MonoBehaviour
{
	public SkeletonRenderer skeletonRenderer;

	public SkeletonGraphic skeletonGraphic;

	public GameObject activeOnNormalX;

	public GameObject activeOnFlippedX;

	private HingeJoint2D[] jointsNormalX;

	private HingeJoint2D[] jointsFlippedX;

	private ISkeletonComponent skeletonComponent;

	private bool wasFlippedXBefore;

	private void Start()
	{
		jointsNormalX = activeOnNormalX.GetComponentsInChildren<HingeJoint2D>();
		jointsFlippedX = activeOnFlippedX.GetComponentsInChildren<HingeJoint2D>();
		ISkeletonComponent obj;
		if (!(skeletonRenderer != null))
		{
			ISkeletonComponent skeletonComponent = skeletonGraphic;
			obj = skeletonComponent;
		}
		else
		{
			ISkeletonComponent skeletonComponent = skeletonRenderer;
			obj = skeletonComponent;
		}
		this.skeletonComponent = obj;
	}

	private void FixedUpdate()
	{
		bool flag = skeletonComponent.Skeleton.ScaleX < 0f;
		if (flag != wasFlippedXBefore)
		{
			HandleFlip(flag);
		}
		wasFlippedXBefore = flag;
	}

	private void HandleFlip(bool isFlippedX)
	{
		GameObject gameObject = (isFlippedX ? activeOnFlippedX : activeOnNormalX);
		GameObject gameObject2 = (isFlippedX ? activeOnNormalX : activeOnFlippedX);
		gameObject.SetActive(value: true);
		gameObject2.SetActive(value: false);
		ResetJointPositions(isFlippedX ? jointsFlippedX : jointsNormalX);
		ResetJointPositions(isFlippedX ? jointsNormalX : jointsFlippedX);
		CompensateMovementAfterFlipX(gameObject.transform, gameObject2.transform);
	}

	private void ResetJointPositions(HingeJoint2D[] joints)
	{
		foreach (HingeJoint2D hingeJoint2D in joints)
		{
			Transform transform = hingeJoint2D.connectedBody.transform;
			hingeJoint2D.transform.position = transform.TransformPoint(hingeJoint2D.connectedAnchor);
		}
	}

	private void CompensateMovementAfterFlipX(Transform toActivate, Transform toDeactivate)
	{
		Transform child = toDeactivate.GetChild(0);
		Transform child2 = toActivate.GetChild(0);
		toActivate.position += child.position - child2.position;
	}
}
