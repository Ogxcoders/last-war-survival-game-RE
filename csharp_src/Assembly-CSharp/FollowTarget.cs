using UnityEngine;

public class FollowTarget : MonoBehaviour
{
	public Transform target;

	public Vector3 offset;

	public bool globalOffset;

	public void SetTarget(Transform target)
	{
		this.target = target;
		offset = Vector3.zero;
	}

	public void SetTargetAndOffset(Transform target, Vector3 offset)
	{
		this.target = target;
		this.offset = offset;
	}

	public void SetTargetAndOffsetXYZ(Transform target, float offsetX, float offsetY, float offsetZ)
	{
		this.target = target;
		offset = new Vector3(offsetX, offsetY, offsetZ);
	}

	private void LateUpdate()
	{
		if (target != null)
		{
			if (globalOffset)
			{
				base.transform.position = target.position + offset;
			}
			else
			{
				base.transform.position = target.TransformPoint(offset);
			}
		}
	}
}
