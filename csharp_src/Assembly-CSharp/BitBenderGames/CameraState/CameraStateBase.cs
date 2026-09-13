namespace BitBenderGames.CameraState;

public abstract class CameraStateBase
{
	protected MobileTouchCamera _touchCamera;

	public CameraStateBase(MobileTouchCamera touchCamera)
	{
		_touchCamera = touchCamera;
	}

	public abstract void OnEnter(params object[] enterParams);

	public abstract void OnUpdate();

	public abstract void OnLeave();
}
