using DG.Tweening;
using GameFramework;
using UnityEngine;

namespace BitBenderGames.CameraState;

public class CameraStateSyncWithTimeline : CameraStateBase
{
	private const float DEFAULT_FOV = 10f;

	private Camera _camInTimeline;

	private float _transitionTime = 1f;

	private float _transitionTimer;

	public CameraStateSyncWithTimeline(MobileTouchCamera touchCamera)
		: base(touchCamera)
	{
	}

	public override void OnEnter(params object[] enterParams)
	{
		if (enterParams == null || enterParams.Length < 1)
		{
			Log.Error("CameraStateSyncWithTimeline enterParam is invalid");
			_camInTimeline = null;
			return;
		}
		_camInTimeline = enterParams[0] as Camera;
		if (_camInTimeline == null)
		{
			Log.Error("CameraStateSyncWithTimeline enterParams[0] is not a Camera");
			return;
		}
		if (enterParams.Length > 1 && enterParams[1] is float)
		{
			_transitionTime = (float)enterParams[1];
		}
		else
		{
			_transitionTime = 1f;
		}
		_transitionTimer = 0f;
	}

	public override void OnUpdate()
	{
		if (_camInTimeline == null)
		{
			_touchCamera.SetState(MobileTouchCamera.State.Idle, false);
		}
		else if (_transitionTimer < _transitionTime)
		{
			Vector3 position = _camInTimeline.transform.position;
			Quaternion rotation = _camInTimeline.transform.rotation;
			float fieldOfView = _camInTimeline.fieldOfView;
			float t = Mathf.Clamp01(_transitionTimer / _transitionTime);
			_transitionTimer += Time.deltaTime;
			_touchCamera.SetCameraPos(Vector3.Lerp(_touchCamera.GetCameraPos(), position, t));
			_touchCamera.SetRotation(Quaternion.Slerp(_touchCamera.GetRotation(), rotation, t));
			_touchCamera.SetFov(Mathf.Lerp(_touchCamera.GetFov(), fieldOfView, t));
		}
		else
		{
			_touchCamera.SetCameraPos(_camInTimeline.transform.position);
			_touchCamera.SetRotation(_camInTimeline.transform.rotation);
			_touchCamera.SetFov(_camInTimeline.fieldOfView);
		}
	}

	public override void OnLeave()
	{
		_camInTimeline = null;
		DOTween.To(() => _touchCamera.GetFov(), delegate(float x)
		{
			_touchCamera.SetFov(x);
		}, 10f, _transitionTime);
	}
}
