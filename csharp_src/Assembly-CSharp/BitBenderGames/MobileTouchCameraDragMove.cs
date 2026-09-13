using UnityEngine;

namespace BitBenderGames;

public class MobileTouchCameraDragMove
{
	private MobileTouchCamera _touchCamera;

	private Vector3 targetPositionClamped = Vector3.zero;

	private float timeRealDragStop;

	private Vector3 cameraScrollVelocity;

	private Vector3 camVelocity = Vector3.zero;

	private Vector3 posLastFrame = Vector3.zero;

	private bool isDragging;

	private bool canDrag;

	private Vector3 dragStartCamPos;

	public float sqrCameraSpeed => camVelocity.sqrMagnitude;

	public MobileTouchCameraDragMove(MobileTouchCamera camera)
	{
		_touchCamera = camera;
		Reset();
	}

	public void Reset()
	{
		targetPositionClamped = Vector3.zero;
		timeRealDragStop = 0f;
		cameraScrollVelocity = Vector3.zero;
		camVelocity = Vector3.zero;
		posLastFrame = Vector3.zero;
		isDragging = false;
		dragStartCamPos = Vector3.zero;
	}

	public void OnForceChangePosition()
	{
		posLastFrame = _touchCamera.GetCameraPos();
	}

	public void OnFingerDown()
	{
		cameraScrollVelocity = Vector3.zero;
	}

	public void OnDragStart(Vector3 dragPosStart)
	{
		canDrag = !_touchCamera.InputDonwOnUI(dragPosStart);
		if (canDrag)
		{
			posLastFrame = _touchCamera.GetCameraPos();
			cameraScrollVelocity = Vector3.zero;
			camVelocity = Vector3.zero;
			dragStartCamPos = _touchCamera.GetCameraPos();
			isDragging = true;
			SetTargetPosition(_touchCamera.GetCameraPos());
		}
	}

	public void OnDragUpdate(Vector3 dragPosStart, Vector3 dragPosCurrent, Vector3 correctionOffset)
	{
		if (canDrag && isDragging)
		{
			Vector3 dragVector = GetDragVector(dragPosStart, dragPosCurrent + correctionOffset);
			Vector3 targetPosition = dragStartCamPos - dragVector;
			SetTargetPosition(targetPosition);
		}
	}

	public void OnDragStop()
	{
		if (canDrag && isDragging)
		{
			cameraScrollVelocity = -_touchCamera.ProjectVector3(camVelocity) * 0.5f;
			timeRealDragStop = Time.realtimeSinceStartup;
			isDragging = false;
		}
	}

	public bool UpdateAutoScroll()
	{
		if (cameraScrollVelocity.sqrMagnitude <= float.Epsilon)
		{
			return false;
		}
		float num = Mathf.Clamp01((Time.realtimeSinceStartup - timeRealDragStop) * _touchCamera.DampFactorTimeMultiplier);
		float num2 = cameraScrollVelocity.magnitude / _touchCamera.AutoScrollVelocityMax;
		int lodLevel = _touchCamera.LodLevel;
		float num3 = ((lodLevel < 0 || lodLevel >= _touchCamera.AutoScrollDamps.Count) ? _touchCamera.AutoScrollDamp : _touchCamera.AutoScrollDamps[lodLevel]);
		Vector3 vector = num * cameraScrollVelocity.normalized * num3 * Time.deltaTime;
		vector *= EvaluateAutoScrollDampCurve(Mathf.Clamp01(1f - num2));
		if (vector.sqrMagnitude >= cameraScrollVelocity.sqrMagnitude)
		{
			cameraScrollVelocity = Vector3.zero;
			return false;
		}
		cameraScrollVelocity -= vector;
		return true;
	}

	public void UpdatePosition(float deltaTime)
	{
		if (isDragging)
		{
			float t = Mathf.Clamp01(Time.deltaTime * _touchCamera.CamFollowFactor);
			Vector3 cameraPos = Vector3.Lerp(_touchCamera.GetCameraPos(), targetPositionClamped, t);
			_touchCamera.SetCameraPos(cameraPos);
		}
		Vector2 vector = -cameraScrollVelocity * deltaTime;
		Vector3 cameraPos2 = _touchCamera.GetCameraPos();
		cameraPos2.x += vector.x;
		cameraPos2.z += vector.y;
		_touchCamera.SetCameraPos(cameraPos2);
		camVelocity = (_touchCamera.GetCameraPos() - posLastFrame) / Time.deltaTime;
		posLastFrame = _touchCamera.GetCameraPos();
	}

	private Vector3 GetDragVector(Vector3 dragPosStart, Vector3 dragPosCurrent)
	{
		Vector3 intersectionPoint = _touchCamera.GetIntersectionPoint(_touchCamera.ScreenPointToRay(dragPosStart));
		return _touchCamera.GetIntersectionPoint(_touchCamera.ScreenPointToRay(dragPosCurrent)) - intersectionPoint;
	}

	private void SetTargetPosition(Vector3 newPositionClamped)
	{
		targetPositionClamped = newPositionClamped;
	}

	private float EvaluateAutoScrollDampCurve(float t)
	{
		if (_touchCamera.AutoScrollDampCurve == null || _touchCamera.AutoScrollDampCurve.length == 0)
		{
			return 1f;
		}
		return _touchCamera.AutoScrollDampCurve.Evaluate(t);
	}

	public void StopCameraScroll()
	{
		cameraScrollVelocity = Vector3.zero;
	}
}
