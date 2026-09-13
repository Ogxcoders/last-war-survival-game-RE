using UnityEngine;

namespace Spine.Unity;

public class FollowSkeletonUtilityRootRotation : MonoBehaviour
{
	private const float FLIP_ANGLE_THRESHOLD = 100f;

	public Transform reference;

	private Vector3 prevLocalEulerAngles;

	private void Start()
	{
		prevLocalEulerAngles = base.transform.localEulerAngles;
	}

	private void FixedUpdate()
	{
		base.transform.rotation = reference.rotation;
		bool flag = Mathf.Abs(base.transform.localEulerAngles.y - prevLocalEulerAngles.y) > 100f;
		bool num = Mathf.Abs(base.transform.localEulerAngles.x - prevLocalEulerAngles.x) > 100f;
		if (flag)
		{
			CompensatePositionToYRotation();
		}
		if (num)
		{
			CompensatePositionToXRotation();
		}
		prevLocalEulerAngles = base.transform.localEulerAngles;
	}

	private void CompensatePositionToYRotation()
	{
		Vector3 position = reference.position + (reference.position - base.transform.position);
		position.y = base.transform.position.y;
		base.transform.position = position;
	}

	private void CompensatePositionToXRotation()
	{
		Vector3 position = reference.position + (reference.position - base.transform.position);
		position.x = base.transform.position.x;
		base.transform.position = position;
	}
}
