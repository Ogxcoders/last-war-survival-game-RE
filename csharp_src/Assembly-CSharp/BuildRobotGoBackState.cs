using UnityEngine;

public class BuildRobotGoBackState : IBuildBotState
{
	private bool getPathFinish;

	public void OnEnter(WorldBuildRobot robot)
	{
		robot.animator.SetTrigger(WorldBuildRobot.Flying);
		robot.BackStartPos = robot.transform.position;
		Vector3 vector = robot.MoveStartPos - robot.BackStartPos;
		robot.ApproachDir = new Vector3(vector.x, 0f, vector.z).normalized;
		robot.BotMove.InitPath(robot.BackStartPos, robot.MoveStartPos, isGoTarget: false, PathUtils.CalRobotMoveNeedTime(robot.BackStartPos, robot.MoveStartPos));
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
		robot.animator.SetTrigger(WorldBuildRobot.Standby);
		robot.animator.ResetTrigger(WorldBuildRobot.Flying);
	}
}
