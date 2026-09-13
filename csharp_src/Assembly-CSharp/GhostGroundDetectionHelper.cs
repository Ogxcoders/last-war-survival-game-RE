using GameFramework;
using UnityEngine;

public class GhostGroundDetectionHelper
{
	private const float DETECTION_DISTANCE = 1f;

	private RaycastHit[] hitsCache;

	private Transform targetTransform;

	private float skinWidth = 0.1f;

	private Vector3[] raycastPoints;

	private Vector3 _lastTransPos;

	public void Init(Transform transform, float skinWidth, float maxHeight)
	{
		targetTransform = transform;
		_lastTransPos = targetTransform.position;
		this.skinWidth = skinWidth;
		hitsCache = new RaycastHit[5];
	}

	public void Uninit()
	{
		targetTransform = null;
		raycastPoints = null;
		hitsCache = null;
	}

	public void DetectGround(float newX, float newY, float newZ, out bool isGrounded, out float contactPointY)
	{
		if (targetTransform == null)
		{
			Log.Error("GhostGroundDetectionHelper is not Init");
			isGrounded = false;
			contactPointY = 0f;
			return;
		}
		_lastTransPos = targetTransform.position;
		Vector3 vector = new Vector3(newX, newY, newZ);
		Vector3 down = Vector3.down;
		Vector3 vector2 = vector - _lastTransPos;
		int num = Physics.SphereCastNonAlloc(_lastTransPos, skinWidth, Vector3.Normalize(vector2), hitsCache, Vector3.Magnitude(vector2), LayerMask.GetMask("Terrain"));
		if (num > 0)
		{
			float num2 = _lastTransPos.y + 5f;
			num = Physics.RaycastNonAlloc(new Vector3(vector.x, num2, vector.z), down, hitsCache, Mathf.Max(0.1f, num2 - newY + 0.1f), LayerMask.GetMask("Terrain"));
			if (num > 0)
			{
				isGrounded = false;
				contactPointY = 0f;
				float num3 = -1f;
				for (int i = 0; i < num; i++)
				{
					RaycastHit raycastHit = hitsCache[i];
					if (!raycastHit.collider.CompareTag("Finish"))
					{
						isGrounded = true;
						if (raycastHit.point.y > num3)
						{
							num3 = (contactPointY = raycastHit.point.y);
						}
					}
				}
			}
			else
			{
				isGrounded = false;
				contactPointY = newY;
			}
			return;
		}
		num = Physics.RaycastNonAlloc(vector + Vector3.up * skinWidth, maxDistance: 1f + skinWidth, direction: down, results: hitsCache, layerMask: LayerMask.GetMask("Terrain"));
		if (num > 0)
		{
			isGrounded = false;
			contactPointY = 0f;
			float num4 = -1f;
			for (int j = 0; j < num; j++)
			{
				RaycastHit raycastHit2 = hitsCache[j];
				if (!raycastHit2.collider.CompareTag("Finish"))
				{
					isGrounded = true;
					if (raycastHit2.point.y > num4)
					{
						num4 = (contactPointY = raycastHit2.point.y);
					}
				}
			}
		}
		else
		{
			isGrounded = newY <= 0f;
			contactPointY = 0f;
		}
	}
}
