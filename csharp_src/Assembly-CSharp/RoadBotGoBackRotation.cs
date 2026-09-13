using UnityEngine;

public class RoadBotGoBackRotation : IRoadBotState
{
	private bool getPathFinish;

	public void OnEnter(WorldRoadRobot robot)
	{
		Vector3 position = robot.transform.position;
		Vector3 eulerAngles = robot.transform.rotation.eulerAngles;
		Vector3 eulerAngles2 = Quaternion.LookRotation((robot.MoveStartPos - position).normalized).eulerAngles;
		Vector3 endRotation = new Vector3(eulerAngles.x, eulerAngles2.y, eulerAngles.z);
		robot.BotMove.InitRotation(eulerAngles, endRotation);
		getPathFinish = true;
	}

	public void OnUpdate(WorldRoadRobot robot, float deltaTime, Vector3 scanPos)
	{
		if (getPathFinish && !robot.BotMove.UpdateMove(deltaTime, scanPos))
		{
			robot.ChangeState(WorldRoadRobot.State.GoBack);
		}
	}

	public void OnLeave(WorldRoadRobot robot)
	{
		getPathFinish = false;
	}
}
