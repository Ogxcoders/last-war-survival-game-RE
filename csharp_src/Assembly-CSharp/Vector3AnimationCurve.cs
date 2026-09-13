using UnityEngine;

public class Vector3AnimationCurve
{
	private AnimationCurve curveX = new AnimationCurve();

	private AnimationCurve curveY = new AnimationCurve();

	private AnimationCurve curveZ = new AnimationCurve();

	public void AddKey(float time, Vector3 vec)
	{
		curveX.AddKey(time, vec.x);
		curveY.AddKey(time, vec.y);
		curveZ.AddKey(time, vec.z);
	}

	public Vector3 Evaluate(float time)
	{
		return new Vector3
		{
			x = curveX.Evaluate(time),
			y = curveY.Evaluate(time),
			z = curveZ.Evaluate(time)
		};
	}
}
