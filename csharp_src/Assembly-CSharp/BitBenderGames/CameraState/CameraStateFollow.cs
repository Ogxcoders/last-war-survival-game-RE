using UnityEngine;

namespace BitBenderGames.CameraState;

public class CameraStateFollow : CameraStateBase
{
	public GameObject _leaderObj;

	public float _moveToLeaderTime;

	public Vector3 Offset;

	private float _animTime;

	public CameraStateFollow(MobileTouchCamera touchCamera)
		: base(touchCamera)
	{
	}

	public override void OnEnter(params object[] enterParam)
	{
		_animTime = 0f;
	}

	public void Reset()
	{
		_animTime = 0f;
	}

	public override void OnUpdate()
	{
		if (_leaderObj == null)
		{
			_touchCamera.SetState(MobileTouchCamera.State.FreeLook, false);
		}
		else if (_animTime <= _moveToLeaderTime)
		{
			_animTime += Time.deltaTime;
			Vector3 cameraPos = _touchCamera.GetCameraPos();
			Vector3 vector = _touchCamera.AdjustTarget(_leaderObj.transform.position + Offset);
			Vector3 b = cameraPos + (vector - _touchCamera.GetCameraTargetPos());
			Vector3 cameraPos2 = Vector3.Lerp(cameraPos, b, _animTime / _moveToLeaderTime);
			_touchCamera.SetCameraPos(cameraPos2);
		}
		else
		{
			_touchCamera.LookAt(_leaderObj.transform.position + Offset);
		}
	}

	public override void OnLeave()
	{
		_leaderObj = null;
	}
}
