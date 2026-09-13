using UnityEngine;

public class BuildBotLandingRotationState : IBuildBotState
{
	private bool getPathFinish;

	public void OnEnter(WorldBuildRobot robot)
	{
		robot.transform.position = robot.MoveStartPos;
		Vector3 eulerAngles = robot.transform.rotation.eulerAngles;
		Vector3 vector = new Vector3(0f, 180f, 0f);
		Vector3 endRotation = new Vector3(eulerAngles.x, vector.y, eulerAngles.z);
		robot.BotMove.InitRotation(eulerAngles, endRotation);
		getPathFinish = true;
	}

	public void OnUpdate(WorldBuildRobot robot, float deltaTime)
	{
		if (getPathFinish && !robot.BotMove.UpdateMove(deltaTime))
		{
			robot.ChangeState(WorldBuildRobot.State.Landing);
		}
	}

	public void OnLeave(WorldBuildRobot robot)
	{
		getPathFinish = false;
		robot.animator.ResetTrigger(WorldBuildRobot.Standby);
	}
}
