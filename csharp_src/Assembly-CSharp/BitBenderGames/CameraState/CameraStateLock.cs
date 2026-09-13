using DG.Tweening;
using UnityEngine;

namespace BitBenderGames.CameraState;

public class CameraStateLock : CameraStateBase
{
	public CameraStateLock(MobileTouchCamera touchCamera)
		: base(touchCamera)
	{
	}

	public override void OnEnter(params object[] enterParam)
	{
		Vector3 endValue = (Vector3)enterParam[0];
		float duration = (float)enterParam[1];
		_touchCamera.StopMove();
		_touchCamera.transform.DOMove(endValue, duration);
	}

	public override void OnUpdate()
	{
	}

	public override void OnLeave()
	{
	}
}
