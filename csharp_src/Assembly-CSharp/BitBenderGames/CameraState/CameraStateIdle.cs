using UnityEngine;

namespace BitBenderGames.CameraState;

public class CameraStateIdle : CameraStateBase
{
	public CameraStateIdle(MobileTouchCamera touchCamera)
		: base(touchCamera)
	{
	}

	public override void OnEnter(params object[] enterParam)
	{
		_touchCamera.touchInput.OnFingerDown += OnFingerDown;
		_touchCamera.touchInput.OnPinchStart += OnPinchStart;
	}

	public override void OnUpdate()
	{
	}

	public override void OnLeave()
	{
		_touchCamera.touchInput.OnFingerDown -= OnFingerDown;
		_touchCamera.touchInput.OnPinchStart -= OnPinchStart;
	}

	private void OnFingerDown(Vector3 pos)
	{
		if (_touchCamera.CanMoveing && !_touchCamera.InputDonwOnUI(pos))
		{
			_touchCamera.SetState(MobileTouchCamera.State.FreeLook, false);
		}
	}

	private void OnPinchStart(Vector3 pinchCenter, float pinchDistance)
	{
		if (!_touchCamera.InputDonwOnUI(pinchCenter))
		{
			_touchCamera.SetState(MobileTouchCamera.State.FreeLook, false);
			(_touchCamera.GetState(MobileTouchCamera.State.FreeLook) as CameraStateFreeLook).OnPinchStart(pinchCenter, pinchDistance);
		}
	}
}
