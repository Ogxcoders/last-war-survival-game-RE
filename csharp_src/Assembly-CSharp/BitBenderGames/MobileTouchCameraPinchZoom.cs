using UnityEngine;

namespace BitBenderGames;

public class MobileTouchCameraPinchZoom
{
	private MobileTouchCamera _touchCamera;

	private bool isPinching;

	private bool canPinch;

	private Vector3 pinchCenterCurrent;

	private float pinchDistanceCurrent;

	private Vector3 pinchCenterCurrentLerp;

	private float pinchDistanceCurrentLerp;

	private float pinchDistanceLastLerp;

	private Vector3 pinchStartIntersectionCenter;

	private float pinchDistanceLast;

	public MobileTouchCameraPinchZoom(MobileTouchCamera camera)
	{
		_touchCamera = camera;
		isPinching = false;
	}

	public void OnPinchStart(Vector3 pinchCenter, float pinchDistance)
	{
		canPinch = !_touchCamera.InputDonwOnUI(pinchCenter);
		if (canPinch)
		{
			pinchDistanceCurrent = pinchDistance;
			pinchCenterCurrent = pinchCenter;
			pinchDistanceLast = pinchDistance;
			isPinching = true;
		}
	}

	public void OnPinchUpdate(PinchUpdateData pinchUpdateData)
	{
		if (canPinch)
		{
			pinchCenterCurrent = pinchUpdateData.pinchCenter;
			pinchDistanceCurrent = pinchUpdateData.pinchDistance;
		}
	}

	public void OnPinchStop()
	{
		if (canPinch)
		{
			isPinching = false;
		}
	}

	public bool UpdatePinch()
	{
		if (isPinching)
		{
			if (!_touchCamera.GetTouchTerrainPos(pinchCenterCurrent.x, pinchCenterCurrent.y, out var pos))
			{
				return true;
			}
			float num = (pinchDistanceCurrent - pinchDistanceLast) * _touchCamera.GetZoomSensitivity();
			float value = _touchCamera.CamZoom - num;
			value = Mathf.Clamp(value, _touchCamera.CamZoomMin - _touchCamera.CamOverZoomMargin, _touchCamera.CamZoomMax + _touchCamera.CamOverZoomMargin);
			_touchCamera.CamZoom = value;
			if (!_touchCamera.GetTouchTerrainPos(pinchCenterCurrent.x, pinchCenterCurrent.y, out var pos2))
			{
				return true;
			}
			Vector3 vector = pos2 - pos;
			Vector3 cameraPos = _touchCamera.GetCameraPos() - vector;
			_touchCamera.SetCameraPos(cameraPos);
			pinchDistanceLast = pinchDistanceCurrent;
			return true;
		}
		return false;
	}

	private bool DoEditorZoom()
	{
		return false;
	}
}
