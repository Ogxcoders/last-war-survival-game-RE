using System;
using UnityEngine;

namespace BitBenderGames;

[RequireComponent(typeof(MobileTouchCamera))]
public class FocusCameraOnItem : MonoBehaviourWrapped
{
	[SerializeField]
	private float transitionDuration = 0.5f;

	private Vector3 posTransitionStart;

	private Vector3 posTransitionEnd;

	private float timeTransitionStart;

	private bool isTransitionStarted;

	private MobileTouchCamera MobileTouchCamera { get; set; }

	public void Awake()
	{
		MobileTouchCamera = GetComponent<MobileTouchCamera>();
		isTransitionStarted = false;
	}

	public void LateUpdate()
	{
		if (isTransitionStarted)
		{
			if (Time.time < timeTransitionStart + transitionDuration)
			{
				UpdatePosition();
				return;
			}
			SetPosition(posTransitionEnd);
			isTransitionStarted = false;
		}
	}

	private void UpdatePosition()
	{
		float num = (Time.time - timeTransitionStart) / transitionDuration;
		Vector3 position = Vector3.Lerp(posTransitionStart, posTransitionEnd, Mathf.Sin(-MathF.PI / 2f + num * MathF.PI) * 0.5f + 0.5f);
		SetPosition(position);
	}

	public void OnPickItem(RaycastHit hitInfo)
	{
		FocusCameraOnTransform(hitInfo.transform);
	}

	public void OnPickItem2D(RaycastHit2D hitInfo2D)
	{
		FocusCameraOnTransform(hitInfo2D.transform);
	}

	public void OnPickableTransformSelected(Transform pickableTransform)
	{
		FocusCameraOnTransform(pickableTransform);
	}

	public void FocusCameraOnTransform(Transform targetTransform)
	{
		if (!(targetTransform == null))
		{
			FocusCameraOnTarget(targetTransform.position);
		}
	}

	public void FocusCameraOnTransform(Vector3 targetPosition)
	{
		FocusCameraOnTarget(targetPosition);
	}

	public void FocusCameraOnTarget(Vector3 targetPosition)
	{
		if (Mathf.Approximately(transitionDuration, 0f))
		{
			SetPosition(targetPosition);
			return;
		}
		timeTransitionStart = Time.time;
		isTransitionStarted = true;
		posTransitionStart = base.Transform.position;
	}

	private void SetPosition(Vector3 newPosition)
	{
		Vector3 position = base.Transform.position;
		position.x = newPosition.x;
		position.z = newPosition.z;
		base.Transform.position = position;
	}
}
