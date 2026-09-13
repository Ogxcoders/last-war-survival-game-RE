using UnityEngine;

namespace BitBenderGames.CameraState;

public class CameraStateQuitFocus : CameraStateBase
{
	public Vector3 _target;

	public float _time;

	public bool _focusToCenter;

	private Vector3 _startPos;

	private Vector3 _endPos;

	private Quaternion _startRotation;

	private Quaternion _endRotation;

	private AnimationCurve _lerpCurve;

	private float _animTime;

	public CameraStateQuitFocus(MobileTouchCamera touchCamera)
		: base(touchCamera)
	{
	}

	public override void OnEnter(params object[] enterParam)
	{
		_startRotation = _touchCamera.GetRotation();
		_endRotation = Quaternion.Euler(_touchCamera.CamZoomInitRotation, 0f, 0f);
		_startPos = _touchCamera.GetCameraPos();
		if (_focusToCenter)
		{
			Vector3 vector = new Vector3(0f, _touchCamera.CamZoomInit, 0f);
			Vector3 vector2 = _endRotation * Vector3.forward;
			Vector3 intersectionPoint = _touchCamera.GetIntersectionPoint(new Ray(vector, vector2));
			_endPos = _target - vector2 * Vector3.Distance(vector, intersectionPoint);
		}
		else
		{
			Vector3 pos = _touchCamera.WorldToScreenPoint(_target);
			Vector3 cameraPos = _touchCamera.GetCameraPos();
			Quaternion rotation = _touchCamera.GetRotation();
			Vector3 vector3 = new Vector3(0f, _touchCamera.CamZoomInit, 0f);
			_touchCamera.SetCameraPos(vector3);
			_touchCamera.SetRotation(_endRotation);
			Ray ray = _touchCamera.ScreenPointToRay(pos);
			_touchCamera.SetCameraPos(cameraPos);
			_touchCamera.SetRotation(rotation);
			Vector3 intersectionPoint2 = _touchCamera.GetIntersectionPoint(ray);
			_endPos = _target - ray.direction * Vector3.Distance(vector3, intersectionPoint2);
		}
		_lerpCurve = _touchCamera.CameraFocusCurve;
		_animTime = 0f;
	}

	public override void OnUpdate()
	{
		_animTime += Time.deltaTime;
		if (_animTime <= _time)
		{
			float t = EvaluateLerpCurve(_animTime / _time);
			_touchCamera.SetCameraPos(Vector3.Lerp(_startPos, _endPos, t));
			_touchCamera.SetRotation(Quaternion.Lerp(_startRotation, _endRotation, t));
		}
		else
		{
			_touchCamera.SetCameraPos(_endPos);
			_touchCamera.SetRotation(_endRotation);
			_touchCamera.SetState(MobileTouchCamera.State.Idle, false);
		}
	}

	public override void OnLeave()
	{
	}

	private float EvaluateLerpCurve(float t)
	{
		if (_lerpCurve == null || _lerpCurve.length == 0)
		{
			return t;
		}
		return _lerpCurve.Evaluate(t);
	}
}
