using System;
using UnityEngine;

namespace BitBenderGames.CameraState;

public class CameraStateFocus : CameraStateBase
{
	private MobileTouchCameraDragMove _dragMove;

	private bool _inputDonwOnUI;

	public Vector3 _target;

	public float _time;

	public float _zoom;

	public float _rotation;

	public AnimationCurve _lerpCurve;

	public bool _focusToCenter;

	public bool _lockView;

	public Action _onComplete;

	private Vector3 _startPos;

	private Vector3 _endPos;

	private Quaternion _startRotation;

	private Quaternion _endRotation;

	private const int AnimFocus = 0;

	private const int AnimEnd = 1;

	private int _anim;

	private float _animTime;

	public CameraStateFocus(MobileTouchCamera touchCamera)
		: base(touchCamera)
	{
		_dragMove = new MobileTouchCameraDragMove(touchCamera);
	}

	public override void OnEnter(params object[] enterParam)
	{
		_touchCamera.touchInput.OnFingerDown += OnFingerDown;
		_touchCamera.touchInput.OnDragStart += OnDragStart;
		_touchCamera.touchInput.OnDragUpdate += OnDragUpdate;
		_touchCamera.touchInput.OnDragStop += OnDragStop;
		_startRotation = _touchCamera.GetRotation();
		_endRotation = Quaternion.Euler(_rotation, 0f, 0f);
		_startPos = _touchCamera.GetCameraPos();
		_zoom = Mathf.Clamp(_zoom, _touchCamera.CamZoomMin, _touchCamera.CamZoomMax);
		if (_focusToCenter)
		{
			Vector3 vector = _endRotation * Vector3.forward;
			Vector3 vector2 = new Vector3(0f, _zoom, 0f);
			Vector3 intersectionPoint = _touchCamera.GetIntersectionPoint(new Ray(vector2, vector));
			_endPos = _target - vector * Vector3.Distance(vector2, intersectionPoint);
		}
		else
		{
			Vector3 pos = _touchCamera.WorldToScreenPoint(_target);
			Vector3 cameraPos = _touchCamera.GetCameraPos();
			Quaternion rotation = _touchCamera.GetRotation();
			Vector3 vector3 = new Vector3(0f, _zoom, 0f);
			_touchCamera.SetCameraPos(vector3);
			_touchCamera.SetRotation(_endRotation);
			Ray ray = _touchCamera.ScreenPointToRay(pos);
			_touchCamera.SetCameraPos(cameraPos);
			_touchCamera.SetRotation(rotation);
			Vector3 intersectionPoint2 = _touchCamera.GetIntersectionPoint(ray);
			_endPos = _target - ray.direction * Vector3.Distance(vector3, intersectionPoint2);
		}
		_anim = 0;
		_animTime = 0f;
		_dragMove.Reset();
	}

	public override void OnUpdate()
	{
		if (_anim == 0)
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
				_anim = 1;
				_onComplete?.Invoke();
			}
		}
		else if (_anim == 1)
		{
			_dragMove.UpdatePosition(Time.deltaTime);
			_dragMove.UpdateAutoScroll();
		}
	}

	public override void OnLeave()
	{
		_touchCamera.touchInput.OnFingerDown -= OnFingerDown;
		_touchCamera.touchInput.OnDragStart -= OnDragStart;
		_touchCamera.touchInput.OnDragUpdate -= OnDragUpdate;
		_touchCamera.touchInput.OnDragStop -= OnDragStop;
		_onComplete = null;
	}

	private void OnFingerDown(Vector3 pos)
	{
		_inputDonwOnUI = _touchCamera.InputDonwOnUI(pos);
		_dragMove.OnFingerDown();
	}

	private void OnDragStart(Vector3 dragPosStart, bool isLongTap)
	{
		if (_touchCamera.CanMoveing && !_inputDonwOnUI && !_lockView)
		{
			_dragMove.OnDragStart(dragPosStart);
		}
	}

	private void OnDragUpdate(Vector3 dragPosStart, Vector3 dragPosCurrent, Vector3 correctionOffset)
	{
		if (_touchCamera.CanMoveing && !_inputDonwOnUI && !_lockView)
		{
			_dragMove.OnDragUpdate(dragPosStart, dragPosCurrent, correctionOffset);
		}
	}

	private void OnDragStop(Vector3 dragStopPos, Vector3 dragFinalMomentum)
	{
		_dragMove.OnDragStop();
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
