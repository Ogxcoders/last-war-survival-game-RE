using System;
using UnityEngine;

public class RandMove : MonoBehaviour
{
	public enum FlyState
	{
		None,
		Up,
		Track
	}

	private static float _pointCount = 5f;

	private static int SEGMENT_COUNT = 20;

	private static Vector3[] _paths = new Vector3[SEGMENT_COUNT];

	public AnimationCurve firstCurve;

	public AnimationCurve secondCurve;

	public Vector3 rotDir;

	public Vector3 startPos;

	public float upTime = 1f;

	public float rotSpeed;

	public float moveHight = 1f;

	private float deltaTime;

	private float deltaTime1;

	public GameObject target;

	public Vector3 controlPointOffset;

	public FlyState flyState;

	public float speed = 1f;

	private Action onComplete;

	public float closeDis = 0.1f;

	public float middleValue = 0.5f;

	public void StartFly(Vector3 startPos, GameObject target, Action onComplete)
	{
		deltaTime = 0f;
		deltaTime1 = 0f;
		this.startPos = startPos;
		this.target = target;
		this.onComplete = onComplete;
		base.transform.position = startPos;
		flyState = FlyState.Up;
	}

	private void Update()
	{
		switch (flyState)
		{
		case FlyState.Up:
			deltaTime += Time.deltaTime;
			if (upTime > 0f)
			{
				float num = firstCurve.Evaluate(deltaTime / upTime);
				base.transform.position = startPos + num * moveHight * Vector3.up;
			}
			if (deltaTime >= upTime)
			{
				flyState = FlyState.Track;
			}
			break;
		case FlyState.Track:
		{
			deltaTime1 += Time.deltaTime * speed;
			Vector3 position = base.transform.position;
			Vector3 position2 = target.transform.position;
			if (deltaTime1 >= 1f || Vector3.Distance(position, position2) < closeDis)
			{
				flyState = FlyState.None;
				onComplete?.Invoke();
			}
			else
			{
				float t = secondCurve.Evaluate(deltaTime1);
				Vector3 p = (position + position2) * middleValue + controlPointOffset;
				base.transform.position = CalculateCubicBezierPointfor2C(t, position, p, position2);
			}
			break;
		}
		}
		base.transform.Rotate(Time.deltaTime * rotSpeed * rotDir);
	}

	private Vector3[] Bezier2Path(Vector3 startPos, Vector3 controlPos, Vector3 endPos)
	{
		for (int i = 1; i <= SEGMENT_COUNT; i++)
		{
			float t = (float)i / (float)SEGMENT_COUNT;
			Vector3 vector = CalculateCubicBezierPointfor2C(t, startPos, controlPos, endPos);
			_paths[i - 1] = vector;
		}
		return _paths;
	}

	public static Vector3 Bezier2(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float t)
	{
		return (1f - t) * (1f - t) * startPos + 2f * t * (1f - t) * controlPos + t * t * endPos;
	}

	public static Vector3 Bezier3(Vector3 startPos, Vector3 controlPos1, Vector3 controlPos2, Vector3 endPos, float t)
	{
		float num = 1f - t;
		return num * num * num * startPos + 3f * t * num * num * controlPos1 + 3f * t * t * num * controlPos2 + t * t * t * endPos;
	}

	private Vector3 CalculateCubicBezierPointfor2C(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float num = 1f - t;
		float num2 = t * t;
		return num * num * p0 + 2f * num * t * p1 + num2 * p2;
	}
}
