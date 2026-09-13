using UnityEngine;

namespace Cinemachine.Utility;

public static class UnityVectorExtensions
{
	public const float Epsilon = 0.0001f;

	public static float ClosestPointOnSegment(this Vector3 p, Vector3 s0, Vector3 s1)
	{
		Vector3 vector = s1 - s0;
		float num = Vector3.SqrMagnitude(vector);
		if (num < 0.0001f)
		{
			return 0f;
		}
		return Mathf.Clamp01(Vector3.Dot(p - s0, vector) / num);
	}

	public static float ClosestPointOnSegment(this Vector2 p, Vector2 s0, Vector2 s1)
	{
		Vector2 vector = s1 - s0;
		float num = Vector2.SqrMagnitude(vector);
		if (num < 0.0001f)
		{
			return 0f;
		}
		return Mathf.Clamp01(Vector2.Dot(p - s0, vector) / num);
	}

	public static Vector3 ProjectOntoPlane(this Vector3 vector, Vector3 planeNormal)
	{
		return vector - Vector3.Dot(vector, planeNormal) * planeNormal;
	}

	public static Vector3 Abs(this Vector3 v)
	{
		return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
	}

	public static bool AlmostZero(this Vector3 v)
	{
		return v.sqrMagnitude < 9.999999E-09f;
	}

	public static float Angle(Vector3 v1, Vector3 v2)
	{
		v1.Normalize();
		v2.Normalize();
		return Mathf.Atan2((v1 - v2).magnitude, (v1 + v2).magnitude) * 57.29578f * 2f;
	}

	public static float SignedAngle(Vector3 v1, Vector3 v2, Vector3 up)
	{
		float num = Angle(v1, v2);
		if (Mathf.Sign(Vector3.Dot(up, Vector3.Cross(v1, v2))) < 0f)
		{
			return 0f - num;
		}
		return num;
	}

	public static Vector3 SlerpWithReferenceUp(Vector3 vA, Vector3 vB, float t, Vector3 up)
	{
		float magnitude = vA.magnitude;
		float magnitude2 = vB.magnitude;
		if (magnitude < 0.0001f || magnitude2 < 0.0001f)
		{
			return Vector3.Lerp(vA, vB, t);
		}
		Vector3 forward = vA / magnitude;
		Vector3 forward2 = vB / magnitude2;
		Quaternion qA = Quaternion.LookRotation(forward, up);
		Quaternion qB = Quaternion.LookRotation(forward2, up);
		return UnityQuaternionExtensions.SlerpWithReferenceUp(qA, qB, t, up) * Vector3.forward * Mathf.Lerp(magnitude, magnitude2, t);
	}
}
