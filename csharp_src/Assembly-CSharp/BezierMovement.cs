using System;
using UnityEngine;

public class BezierMovement : MonoBehaviour
{
	private Vector3 p0;

	private Vector3 p1;

	private Vector3 p2;

	private Vector3 p3;

	[SerializeField]
	private bool useCurve;

	[SerializeField]
	public AnimationCurve curve;

	private float duration = 1f;

	private float timer;

	private bool isWorking;

	private Action completeCallback;

	private Vector3 GetBezierPoint(float t)
	{
		t = Mathf.Clamp01(t);
		float num = 1f - t;
		return num * num * num * p0 + 3f * num * num * t * p1 + 3f * num * t * t * p2 + t * t * t * p3;
	}

	public void Init(Vector3 start, Vector3 end, float duration, float maxHeight)
	{
		p0 = start;
		p3 = end;
		Vector3 vector = p3 - p0;
		float magnitude = vector.magnitude;
		vector.Normalize();
		p1 = p0 + vector * magnitude * 0f + Vector3.up * maxHeight;
		p2 = p0 + vector * magnitude * 0.3f + Vector3.up * maxHeight * 0.7f;
		this.duration = duration;
		base.transform.position = start;
	}

	public void StartAuto(Action onComplete)
	{
		isWorking = true;
		completeCallback = onComplete;
		timer = 0f;
	}

	public void ManualUpdate(float progress)
	{
		if (useCurve && curve != null)
		{
			progress = curve.Evaluate(progress);
		}
		base.transform.position = GetBezierPoint(progress);
	}

	private void Update()
	{
		if (!isWorking)
		{
			return;
		}
		timer += Time.deltaTime;
		if (timer >= duration)
		{
			if (isWorking)
			{
				isWorking = false;
				completeCallback?.Invoke();
				completeCallback = null;
			}
		}
		else
		{
			ManualUpdate(Mathf.Clamp01(timer / duration));
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(p0, p1);
		Gizmos.DrawLine(p1, p2);
		Gizmos.DrawLine(p2, p3);
	}
}
