using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[AddComponentMenu("Cinemachine/CinemachineTargetGroup")]
[SaveDuringPlay]
[ExecuteAlways]
public class CinemachineTargetGroup : MonoBehaviour
{
	[Serializable]
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	public struct Target
	{
		[Tooltip("The target objects.  This object's position and orientation will contribute to the group's average position and orientation, in accordance with its weight")]
		public Transform target;

		[Tooltip("How much weight to give the target when averaging.  Cannot be negative")]
		public float weight;

		[Tooltip("The radius of the target, used for calculating the bounding box.  Cannot be negative")]
		public float radius;
	}

	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	public enum PositionMode
	{
		GroupCenter,
		GroupAverage
	}

	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	public enum RotationMode
	{
		Manual,
		GroupAverage
	}

	public enum UpdateMethod
	{
		Update,
		FixedUpdate,
		LateUpdate
	}

	[Tooltip("How the group's position is calculated.  Select GroupCenter for the center of the bounding box, and GroupAverage for a weighted average of the positions of the members.")]
	public PositionMode m_PositionMode;

	[Tooltip("How the group's rotation is calculated.  Select Manual to use the value in the group's transform, and GroupAverage for a weighted average of the orientations of the members.")]
	public RotationMode m_RotationMode;

	[Tooltip("When to update the group's transform based on the position of the group members")]
	public UpdateMethod m_UpdateMethod = UpdateMethod.LateUpdate;

	[NoSaveDuringPlay]
	[Tooltip("The target objects, together with their weights and radii, that will contribute to the group's average position, orientation, and size.")]
	public Target[] m_Targets = new Target[0];

	private float mAverageWeight;

	private Vector3 mAveragePos;

	public Bounds BoundingBox { get; private set; }

	public BoundingSphere Sphere
	{
		get
		{
			Bounds boundingBox = BoundingBox;
			return new BoundingSphere(boundingBox.center, ((boundingBox.max - boundingBox.min) / 2f).magnitude);
		}
	}

	public bool IsEmpty
	{
		get
		{
			for (int i = 0; i < m_Targets.Length; i++)
			{
				if (m_Targets[i].target != null && m_Targets[i].weight > 0.0001f)
				{
					return false;
				}
			}
			return true;
		}
	}

	public void AddMember(Transform t, float weight, float radius)
	{
		int num = 0;
		if (m_Targets == null)
		{
			m_Targets = new Target[1];
		}
		else
		{
			num = m_Targets.Length;
			Target[] targets = m_Targets;
			m_Targets = new Target[num + 1];
			Array.Copy(targets, m_Targets, num);
		}
		m_Targets[num].target = t;
		m_Targets[num].weight = weight;
		m_Targets[num].radius = radius;
	}

	public void RemoveMember(Transform t)
	{
		int num = FindMember(t);
		if (num >= 0)
		{
			Target[] targets = m_Targets;
			m_Targets = new Target[m_Targets.Length - 1];
			if (num > 0)
			{
				Array.Copy(targets, m_Targets, num);
			}
			if (num < m_Targets.Length - 1)
			{
				Array.Copy(targets, num + 1, m_Targets, num, m_Targets.Length - num - 1);
			}
		}
	}

	public int FindMember(Transform t)
	{
		if (m_Targets != null)
		{
			for (int num = m_Targets.Length - 1; num >= 0; num--)
			{
				if (m_Targets[num].target == t)
				{
					return num;
				}
			}
		}
		return -1;
	}

	public BoundingSphere GetWeightedBoundsForMember(int index)
	{
		if (index < 0 || index >= m_Targets.Length)
		{
			return Sphere;
		}
		return WeightedMemberBounds(m_Targets[index], mAveragePos, mAverageWeight);
	}

	public Bounds GetViewSpaceBoundingBox(Matrix4x4 observer)
	{
		Matrix4x4 inverse = observer.inverse;
		Bounds result = new Bounds(inverse.MultiplyPoint3x4(mAveragePos), Vector3.zero);
		for (int i = 0; i < m_Targets.Length; i++)
		{
			BoundingSphere weightedBoundsForMember = GetWeightedBoundsForMember(i);
			weightedBoundsForMember.position = inverse.MultiplyPoint3x4(weightedBoundsForMember.position);
			result.Encapsulate(new Bounds(weightedBoundsForMember.position, weightedBoundsForMember.radius * 2f * Vector3.one));
		}
		return result;
	}

	private static BoundingSphere WeightedMemberBounds(Target t, Vector3 avgPos, float avgWeight)
	{
		float num = 0f;
		Vector3 b = avgPos;
		if (t.target != null)
		{
			b = t.target.position;
			num = Mathf.Max(0f, t.weight);
			num = ((!(avgWeight > 0.0001f) || !(num < avgWeight)) ? 1f : (num / avgWeight));
		}
		return new BoundingSphere(Vector3.Lerp(avgPos, b, num), t.radius * num);
	}

	private void DoUpdate()
	{
		if (!IsEmpty)
		{
			mAveragePos = CalculateAveragePosition(out mAverageWeight);
			BoundingBox = CalculateBoundingBox(mAveragePos, mAverageWeight);
			switch (m_PositionMode)
			{
			case PositionMode.GroupCenter:
				base.transform.position = BoundingBox.center;
				break;
			case PositionMode.GroupAverage:
				base.transform.position = mAveragePos;
				break;
			}
			RotationMode rotationMode = m_RotationMode;
			if (rotationMode != RotationMode.Manual && rotationMode == RotationMode.GroupAverage)
			{
				base.transform.rotation = CalculateAverageOrientation();
			}
		}
	}

	private Vector3 CalculateAveragePosition(out float averageWeight)
	{
		Vector3 result = Vector3.zero;
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (m_Targets[i].target != null && m_Targets[i].weight > 0.0001f)
			{
				num2++;
				num += m_Targets[i].weight;
				result += m_Targets[i].target.position * m_Targets[i].weight;
			}
		}
		if (num > 0.0001f)
		{
			result /= num;
			averageWeight = num / (float)num2;
		}
		else
		{
			averageWeight = 0f;
			result = base.transform.position;
		}
		return result;
	}

	private Quaternion CalculateAverageOrientation()
	{
		Quaternion q = Quaternion.identity;
		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (m_Targets[i].target != null)
			{
				float weight = m_Targets[i].weight;
				Quaternion rotation = m_Targets[i].target.rotation;
				q = new Quaternion(q.x + rotation.x * weight, q.y + rotation.y * weight, q.z + rotation.z * weight, q.w + rotation.w * weight);
			}
		}
		return q.Normalized();
	}

	private Bounds CalculateBoundingBox(Vector3 avgPos, float averageWeight)
	{
		Bounds result = new Bounds(avgPos, Vector3.zero);
		if (averageWeight > 0.0001f)
		{
			for (int i = 0; i < m_Targets.Length; i++)
			{
				if (m_Targets[i].target != null)
				{
					BoundingSphere boundingSphere = WeightedMemberBounds(m_Targets[i], mAveragePos, mAverageWeight);
					result.Encapsulate(new Bounds(boundingSphere.position, boundingSphere.radius * 2f * Vector3.one));
				}
			}
		}
		return result;
	}

	private void OnValidate()
	{
		for (int i = 0; i < m_Targets.Length; i++)
		{
			m_Targets[i].weight = Mathf.Max(0f, m_Targets[i].weight);
			m_Targets[i].radius = Mathf.Max(0f, m_Targets[i].radius);
		}
	}

	private void FixedUpdate()
	{
		if (m_UpdateMethod == UpdateMethod.FixedUpdate)
		{
			DoUpdate();
		}
	}

	private void Update()
	{
		if (!Application.isPlaying || m_UpdateMethod == UpdateMethod.Update)
		{
			DoUpdate();
		}
	}

	private void LateUpdate()
	{
		if (m_UpdateMethod == UpdateMethod.LateUpdate)
		{
			DoUpdate();
		}
	}

	public void GetViewSpaceAngularBounds(Matrix4x4 observer, out Vector2 minAngles, out Vector2 maxAngles, out Vector2 zRange)
	{
		Matrix4x4 inverse = observer.inverse;
		minAngles = Vector2.zero;
		maxAngles = Vector2.zero;
		zRange = Vector3.zero;
		for (int i = 0; i < m_Targets.Length; i++)
		{
			BoundingSphere weightedBoundsForMember = GetWeightedBoundsForMember(i);
			Vector3 lookAtDir = inverse.MultiplyPoint3x4(weightedBoundsForMember.position);
			float magnitude = lookAtDir.magnitude;
			Vector2 vector = Vector2.zero;
			if (magnitude > 0.0001f && weightedBoundsForMember.radius > 0.0001f)
			{
				vector = Mathf.Atan2(weightedBoundsForMember.radius, magnitude) * 57.29578f * Vector2.one;
			}
			Vector2 cameraRotationToTarget = Quaternion.identity.GetCameraRotationToTarget(lookAtDir, Vector3.up);
			if (i == 0)
			{
				minAngles = cameraRotationToTarget - vector;
				maxAngles = cameraRotationToTarget + vector;
				zRange = new Vector2(lookAtDir.z - weightedBoundsForMember.radius, lookAtDir.z + weightedBoundsForMember.radius);
			}
			else
			{
				minAngles = Vector3.Min(minAngles, cameraRotationToTarget - vector);
				maxAngles = Vector3.Max(maxAngles, cameraRotationToTarget + vector);
				zRange.x = Mathf.Min(zRange.x, lookAtDir.z - weightedBoundsForMember.radius);
				zRange.y = Mathf.Max(zRange.y, lookAtDir.z + weightedBoundsForMember.radius);
			}
		}
	}
}
