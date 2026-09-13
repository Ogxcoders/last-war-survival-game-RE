public abstract class BaseAutoDoMovePosState
{
	public AutoDoMovePos autoDoMovePos;

	public AutoDoMovePosMachine machine;

	public BaseAutoDoMovePosState(AutoDoMovePos autoDo, AutoDoMovePosMachine autoMachine)
	{
		autoDoMovePos = autoDo;
		machine = autoMachine;
	}

	public abstract void OnEnter();

	public abstract void OnUpdate(float deltaTime);

	public abstract void OnLeave();
}
