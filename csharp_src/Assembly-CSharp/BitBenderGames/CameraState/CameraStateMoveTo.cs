using System;
using UnityEngine;

namespace BitBenderGames.CameraState;

public class CameraStateMoveTo : CameraStateBase
{
	public enum MoveType
	{
		ZoomOnly,
		MoveAndZoom
	}

	public Vector3 _target;

	public float _time;

	public float _zoom;

	public Action _onComplete;

	public MoveType _moveType;

	private Quaternion _startRot;

	private Quaternion _endRot;

	private Vector3 _startPos;

	private Vector3 _endPos;

	private float _startZoom;

	private float _endZoom;

	private int _anim;

	private float _animTime;

	public CameraStateMoveTo(MobileTouchCamera touchCamera)
		: base(touchCamera)
	{
	}

	public void TransToSelf()
	{
		_startZoom = _endZoom;
		_endZoom = _zoom;
		_animTime = _time - _animTime;
	}

	public override void OnEnter(params object[] enterParam)
	{
		_zoom = Mathf.Clamp(_zoom, _touchCamera.CamZoomMin, _touchCamera.CamZoomMax);
		_touchCamera.CalcZoom(_zoom, _touchCamera.GetCameraPos(), out var outPos, out var outRot);
		if (_moveType == MoveType.MoveAndZoom)
		{
			_startRot = _touchCamera.GetRotation();
			_endRot = outRot;
			_startPos = _touchCamera.GetCameraPos();
			_endPos = outPos + (_target - _touchCamera.GetCameraTargetPos());
		}
		else if (_moveType == MoveType.ZoomOnly)
		{
			_startZoom = _touchCamera.CamZoom;
			_endZoom = _zoom;
		}
		_animTime = 0f;
	}

	public override void OnUpdate()
	{
		_animTime += Time.deltaTime;
		if (_animTime <= _time)
		{
			if (_moveType == MoveType.MoveAndZoom)
			{
				_touchCamera.SetCameraPos(Vector3.Lerp(_startPos, _endPos, _animTime / _time));
				_touchCamera.SetRotation(Quaternion.Lerp(_startRot, _endRot, _animTime / _time));
			}
			else if (_moveType == MoveType.ZoomOnly)
			{
				_touchCamera.CamZoom = Mathf.Lerp(_startZoom, _endZoom, _animTime / _time);
			}
			return;
		}
		if (_moveType == MoveType.MoveAndZoom)
		{
			_touchCamera.SetCameraPos(_endPos);
			_touchCamera.SetRotation(_endRot);
		}
		else if (_moveType == MoveType.ZoomOnly)
		{
			_touchCamera.CamZoom = _endZoom;
		}
		_touchCamera.SetState(MobileTouchCamera.State.Idle, false);
	}

	public override void OnLeave()
	{
		Action onComplete = _onComplete;
		_onComplete = null;
		onComplete?.Invoke();
	}
}
