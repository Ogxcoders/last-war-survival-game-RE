using UnityEngine;

public class RoadBotLandingRotationState : IRoadBotState
{
	private bool getPathFinish;

	public void OnEnter(WorldRoadRobot robot)
	{
		robot.transform.position = robot.MoveStartPos;
		Vector3 eulerAngles = robot.transform.rotation.eulerAngles;
		Vector3 vector = new Vector3(0f, 180f, 0f);
		Vector3 endRotation = new Vector3(eulerAngles.x, vector.y, eulerAngles.z);
		robot.BotMove.InitRotation(eulerAngles, endRotation);
		getPathFinish = true;
	}

	public void OnUpdate(WorldRoadRobot robot, float deltaTime, Vector3 scanPos)
	{
		if (getPathFinish && !robot.BotMove.UpdateMove(deltaTime, scanPos))
		{
			robot.ChangeState(WorldRoadRobot.State.Landing);
		}
	}

	public void OnLeave(WorldRoadRobot robot)
	{
		getPathFinish = false;
		robot.animator.ResetTrigger(WorldRoadRobot.Standby);
	}
}
