using UnityEngine;

public class BuildingBotWorkRotation : IBuildBotState
{
	private bool getPathFinish;

	public void OnEnter(WorldBuildRobot robot)
	{
		robot.transform.position = robot.MoveEndPos;
		Vector3 eulerAngles = robot.transform.rotation.eulerAngles;
		Vector3 eulerAngles2 = Quaternion.LookRotation((robot.posCenter - robot.MoveEndPos).normalized).eulerAngles;
		Vector3 endRotation = new Vector3(eulerAngles.x, eulerAngles2.y, eulerAngles.z);
		robot.BotMove.InitRotation(eulerAngles, endRotation);
		getPathFinish = true;
	}

	public void OnUpdate(WorldBuildRobot robot, float deltaTime)
	{
		if (getPathFinish && !robot.BotMove.UpdateMove(deltaTime))
		{
			robot.ChangeState(WorldBuildRobot.State.Work);
		}
	}

	public void OnLeave(WorldBuildRobot robot)
	{
		getPathFinish = false;
	}
}
