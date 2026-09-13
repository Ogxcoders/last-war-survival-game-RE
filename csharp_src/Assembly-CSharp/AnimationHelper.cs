using DG.Tweening;
using UnityEngine;

public class AnimationHelper
{
	public static AnimationState GetState(Animation animation, string name)
	{
		return animation[name];
	}

	public static Tween DOBezierCurve3D(Transform transform, Vector3 startPos, Vector3 destPos, float duration, float controller = 0.5f, bool forward = true)
	{
		Vector3 forward2 = Camera.main.transform.forward;
		forward2.y = 0f;
		Vector3 vector = destPos - startPos;
		Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
		Vector3 toDirection = (forward ? (-forward2) : forward2);
		Vector3 vector3 = Quaternion.FromToRotation(vector2, toDirection) * vector2;
		Vector3 vector4 = startPos + vector3 * controller;
		Vector3 vector5 = destPos + (startPos + vector3 - destPos) * controller;
		Vector3[] path = new Vector3[3] { destPos, vector4, vector5 };
		return transform.DOPath(path, duration, PathType.CubicBezier, PathMode.Ignore, 10, Color.cyan);
	}
}
