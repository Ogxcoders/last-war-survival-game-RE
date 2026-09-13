using System;
using DG.Tweening;
using UnityEngine;

public class UIJumpFly : MonoBehaviour
{
	private Animator anim;

	private static int SEGMENT_COUNT = 20;

	private Vector3[] _paths = new Vector3[SEGMENT_COUNT];

	public AnimationCurve firstCurve;

	public bool isRun;

	public Vector3 controlPointOffset;

	public float middlePos = 0.5f;

	public float moveTime;

	public Vector3 targetPos;

	private bool isFinish;

	private double delayTime;

	private Action onComplete;

	private void Awake()
	{
		anim = GetComponent<Animator>();
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

	private Vector3 CalculateCubicBezierPointfor2C(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float num = 1f - t;
		float num2 = t * t;
		return num * num * p0 + 2f * num * t * p1 + num2 * p2;
	}

	public void DoJump()
	{
		anim.enabled = false;
		Vector3 position = base.transform.position;
		Vector3 vector = targetPos;
		Vector3 vector2 = Vector3.Cross(position, vector);
		Vector3 zero = Vector3.zero;
		zero = ((!(vector2.y > 0f)) ? ((position + vector) * middlePos - controlPointOffset) : ((position + vector) * middlePos + controlPointOffset));
		Vector3[] path = Bezier2Path(position, zero, vector);
		Sequence s = DOTween.Sequence();
		s.Append(base.transform.DOPath(path, moveTime)).SetEase(firstCurve);
		s.AppendCallback(delegate
		{
			onComplete?.Invoke();
		}).SetDelay((float)delayTime);
	}

	public void DoFly(Vector3 targetPos, Action onComplete = null, bool isLeft = false)
	{
		this.onComplete = onComplete;
		this.targetPos = targetPos;
		anim.SetTrigger(isLeft ? "left" : "right");
	}

	public void DoFlyNew(Vector3 targetPos, Action onComplete = null, bool isLeft = false, double delayTime = 0.0)
	{
		this.onComplete = onComplete;
		this.targetPos = targetPos;
		this.delayTime = delayTime;
		anim.SetTrigger(isLeft ? "left" : "right");
	}

	private void OnDisable()
	{
		anim.enabled = true;
		isFinish = false;
	}

	private void OnEnable()
	{
		anim.enabled = true;
		isFinish = false;
	}

	private void Update()
	{
		if (isRun)
		{
			anim.SetTrigger("right");
			isRun = false;
		}
	}
}
