using UnityEngine;

public class BuildBotGoTargetState : IBuildBotState
{
	private bool getPathFinish;

	public void OnEnter(WorldBuildRobot robot)
	{
		robot.transform.position = robot.MoveStartPos;
		robot.animator.SetTrigger(WorldBuildRobot.Flying);
		Vector3 vector = robot.MoveEndPos - robot.MoveStartPos;
		robot.ApproachDir = new Vector3(vector.x, 0f, vector.z).normalized;
		robot.BotMove.InitPath(robot.MoveStartPos, robot.MoveEndPos, isGoTarget: true, robot.moveTargetTime);
		getPathFinish = true;
	}

	public void OnUpdate(WorldBuildRobot robot, float deltaTime)
	{
		if (getPathFinish && !robot.BotMove.UpdateMove(deltaTime))
		{
			robot.ChangeState(WorldBuildRobot.State.ApproachTarget);
		}
	}

	public void OnLeave(WorldBuildRobot robot)
	{
		getPathFinish = false;
		robot.animator.ResetTrigger(WorldBuildRobot.Flying);
	}
}
