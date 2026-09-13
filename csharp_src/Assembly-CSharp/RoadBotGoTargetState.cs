using UnityEngine;

public class RoadBotGoTargetState : IRoadBotState
{
	private bool getPathFinish;

	public void OnEnter(WorldRoadRobot robot)
	{
		robot.transform.position = robot.MoveStartPos;
		robot.animator.SetTrigger(WorldRoadRobot.Flying);
		Vector3 vector = robot.MoveEndPos - robot.MoveStartPos;
		robot.ApproachDir = new Vector3(vector.x, 0f, vector.z).normalized;
		robot.BotMove.InitPath(robot.MoveStartPos, robot.MoveEndPos, isGoTarget: true, PathUtils.CalRobotMoveNeedTime(robot.MoveStartPos, robot.MoveEndPos));
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
		robot.animator.ResetTrigger(WorldBuildRobot.Flying);
	}
}
