public class BuildBotWorkState : IBuildBotState
{
	private bool getPathFinish;

	private float tempTime;

	private bool hasOpenScan;

	public void OnEnter(WorldBuildRobot robot)
	{
		tempTime = 0f;
		robot.transform.position = robot.MoveEndPos;
		robot.animator.SetTrigger(WorldBuildRobot.Building);
		float endHeight = robot.WorkEndPos.y - robot.MoveEndPos.y;
		robot.BotMove.InitWorkMove(robot.WorkingPath, robot.posCenter, endHeight, robot.buildTime);
		getPathFinish = true;
	}

	public void OnUpdate(WorldBuildRobot robot, float deltaTime)
	{
		if (!getPathFinish)
		{
			return;
		}
		tempTime += deltaTime;
		if (!hasOpenScan && tempTime > 0.1f)
		{
			robot.scan.gameObject.SetActive(value: true);
			hasOpenScan = true;
		}
		robot.BotMove.UpdateMove(deltaTime);
		if (tempTime > robot.buildTime)
		{
			if (robot.isOther)
			{
				robot.ChangeState(WorldBuildRobot.State.Idle);
				SceneManager.World.AddToNeedRemoveList(robot.uuid);
			}
			else
			{
				robot.ChangeState(WorldBuildRobot.State.GoBackRotation);
			}
		}
	}

	public void OnLeave(WorldBuildRobot robot)
	{
		robot.animator.ResetTrigger(WorldBuildRobot.Building);
		hasOpenScan = false;
		robot.scan.gameObject.SetActive(value: false);
		getPathFinish = false;
		tempTime = 0f;
	}
}
