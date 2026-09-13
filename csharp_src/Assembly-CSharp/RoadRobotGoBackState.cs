using UnityEngine;

public class RoadRobotGoBackState : IRoadBotState
{
	private bool getPathFinish;

	public void OnEnter(WorldRoadRobot robot)
	{
		robot.animator.SetTrigger(WorldRoadRobot.Flying);
		robot.BackStartPos = robot.transform.position;
		Vector3 vector = robot.MoveStartPos - robot.BackStartPos;
		robot.ApproachDir = new Vector3(vector.x, 0f, vector.z).normalized;
		robot.BotMove.InitPath(robot.BackStartPos, robot.MoveStartPos, isGoTarget: false, PathUtils.CalRobotMoveNeedTime(robot.BackStartPos, robot.MoveStartPos));
		getPathFinish = true;
	}

	public void OnUpdate(WorldRoadRobot robot, float deltaTime, Vector3 scanPos)
	{
		if (getPathFinish && !robot.BotMove.UpdateMove(deltaTime, scanPos))
		{
			robot.ChangeState(WorldRoadRobot.State.ApproachTarget);
		}
	}

	public void OnLeave(WorldRoadRobot robot)
	{
		getPathFinish = false;
		robot.animator.SetTrigger(WorldBuildRobot.Standby);
		robot.animator.ResetTrigger(WorldBuildRobot.Flying);
	}
}
