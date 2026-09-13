using UnityEngine;

public class BuildBotTakeOffRotationState : IBuildBotState
{
	private bool getPathFinish;

	public void OnEnter(WorldBuildRobot robot)
	{
		robot.animator.SetTrigger(WorldBuildRobot.Standby);
		robot.transform.position = robot.MoveStartPos;
		Vector3 eulerAngles = robot.transform.rotation.eulerAngles;
		Vector3 eulerAngles2 = Quaternion.LookRotation((robot.MoveEndPos - robot.MoveStartPos).normalized).eulerAngles;
		Vector3 endRotation = new Vector3(eulerAngles.x, eulerAngles2.y, eulerAngles.z);
		robot.BotMove.InitRotation(eulerAngles, endRotation);
		getPathFinish = true;
	}

	public void OnUpdate(WorldBuildRobot robot, float deltaTime)
	{
		if (getPathFinish && !robot.BotMove.UpdateMove(deltaTime))
		{
			robot.ChangeState(WorldBuildRobot.State.GoTarget);
		}
	}

	public void OnLeave(WorldBuildRobot robot)
	{
		getPathFinish = false;
		robot.animator.ResetTrigger(WorldBuildRobot.Standby);
	}
}
