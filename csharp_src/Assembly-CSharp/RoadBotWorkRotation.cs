using UnityEngine;

public class RoadBotWorkRotation : IRoadBotState
{
	private bool getPathFinish;

	public void OnEnter(WorldRoadRobot robot)
	{
		robot.transform.position = robot.MoveEndPos;
		Vector3 eulerAngles = robot.transform.rotation.eulerAngles;
		Vector3 eulerAngles2 = Quaternion.LookRotation((robot.FirstRoadPos - robot.MoveEndPos).normalized).eulerAngles;
		Vector3 endRotation = new Vector3(eulerAngles.x, eulerAngles2.y, eulerAngles.z);
		robot.BotMove.InitRotation(eulerAngles, endRotation);
		getPathFinish = true;
	}

	public void OnUpdate(WorldRoadRobot robot, float deltaTime, Vector3 scanPos)
	{
		if (getPathFinish && !robot.BotMove.UpdateMove(deltaTime, scanPos))
		{
			robot.ChangeState(WorldRoadRobot.State.Work);
		}
	}

	public void OnLeave(WorldRoadRobot robot)
	{
		getPathFinish = false;
	}
}
