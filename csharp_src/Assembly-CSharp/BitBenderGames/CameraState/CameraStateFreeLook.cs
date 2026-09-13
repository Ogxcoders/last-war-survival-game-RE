using UnityEngine;

namespace BitBenderGames.CameraState;

public class CameraStateFreeLook : CameraStateBase
{
	private MobileTouchCameraDragMove _dragMove;

	private MobileTouchCameraPinchZoom _pinchZoom;

	public float sqrCameraSpeed => _dragMove.sqrCameraSpeed;

	public CameraStateFreeLook(MobileTouchCamera touchCamera)
		: base(touchCamera)
	{
		_dragMove = new MobileTouchCameraDragMove(touchCamera);
		_pinchZoom = new MobileTouchCameraPinchZoom(touchCamera);
	}

	public override void OnEnter(params object[] enterParam)
	{
		_touchCamera.touchInput.OnFingerDown += OnFingerDown;
		_touchCamera.touchInput.OnDragStart += OnDragStart;
		_touchCamera.touchInput.OnDragUpdate += OnDragUpdate;
		_touchCamera.touchInput.OnDragStop += OnDragStop;
		_touchCamera.touchInput.OnPinchStart += OnPinchStart;
		_touchCamera.touchInput.OnPinchUpdateExtended += OnPinchUpdate;
		_touchCamera.touchInput.OnPinchStop += OnPinchStop;
		_touchCamera.OnForceChangePositionEvent += OnForceChangePosition;
		_dragMove.Reset();
	}

	public override void OnUpdate()
	{
		bool flag = false;
		bool flag2 = false;
		if (_touchCamera.CanMoveing)
		{
			_dragMove.UpdatePosition(Time.deltaTime);
			flag = _dragMove.UpdateAutoScroll();
			flag2 = _pinchZoom.UpdatePinch();
		}
		bool flag3 = false;
		if (TouchWrapper.TouchCount == 0 && !flag3 && !flag && !flag2)
		{
			_touchCamera.SetState(MobileTouchCamera.State.Idle, false);
		}
	}

	public override void OnLeave()
	{
		_touchCamera.touchInput.OnFingerDown -= OnFingerDown;
		_touchCamera.touchInput.OnDragStart -= OnDragStart;
		_touchCamera.touchInput.OnDragUpdate -= OnDragUpdate;
		_touchCamera.touchInput.OnDragStop -= OnDragStop;
		_touchCamera.touchInput.OnPinchStart -= OnPinchStart;
		_touchCamera.touchInput.OnPinchUpdateExtended -= OnPinchUpdate;
		_touchCamera.touchInput.OnPinchStop -= OnPinchStop;
		_touchCamera.OnForceChangePositionEvent -= OnForceChangePosition;
	}

	private void OnForceChangePosition()
	{
		_dragMove.OnForceChangePosition();
	}

	private void OnFingerDown(Vector3 pos)
	{
		_dragMove.OnFingerDown();
	}

	private void OnDragStart(Vector3 dragPosStart, bool isLongTap)
	{
		_dragMove.OnDragStart(dragPosStart);
	}

	private void OnDragUpdate(Vector3 dragPosStart, Vector3 dragPosCurrent, Vector3 correctionOffset)
	{
		if (_touchCamera.CanMoveing)
		{
			_dragMove.OnDragUpdate(dragPosStart, dragPosCurrent, correctionOffset);
		}
	}

	private void OnDragStop(Vector3 dragStopPos, Vector3 dragFinalMomentum)
	{
		_dragMove.OnDragStop();
	}

	public void OnPinchStart(Vector3 pinchCenter, float pinchDistance)
	{
		if (_touchCamera.CanMoveing)
		{
			_pinchZoom.OnPinchStart(pinchCenter, pinchDistance);
		}
	}

	private void OnPinchUpdate(PinchUpdateData pinchUpdateData)
	{
		if (_touchCamera.CanMoveing)
		{
			_pinchZoom.OnPinchUpdate(pinchUpdateData);
		}
	}

	private void OnPinchStop()
	{
		if (_touchCamera.CanMoveing)
		{
			_pinchZoom.OnPinchStop();
		}
	}
}
