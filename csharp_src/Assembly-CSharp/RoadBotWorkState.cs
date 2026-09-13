using UnityEngine;

public class RoadBotWorkState : IRoadBotState
{
	private bool getPathFinish;

	private float tempTime;

	private bool hasOpenScan;

	public void OnEnter(WorldRoadRobot robot)
	{
		tempTime = 0f;
		robot.transform.position = robot.MoveEndPos;
		robot.transform.LookAt(robot.FirstRoadPos);
		robot.animator.SetTrigger(WorldRoadRobot.Building);
		robot.BotMove.InitWorkMove(robot.WorkingPath);
		getPathFinish = true;
	}

	public void OnUpdate(WorldRoadRobot robot, float deltaTime, Vector3 scanPos)
	{
		if (!getPathFinish)
		{
			return;
		}
		tempTime += deltaTime;
		if (!hasOpenScan && tempTime > 0.1f)
		{
			robot.scan.gameObject.SetActive(value: true);
			hasOpenScan = true;
		}
		if (!robot.BotMove.UpdateMove(deltaTime, scanPos))
		{
			if (robot.isOther)
			{
				robot.ChangeState(WorldRoadRobot.State.Idle);
				SceneManager.World.AddToNeedRemoveList(robot.uuid);
			}
			else
			{
				robot.ChangeState(WorldRoadRobot.State.GoBackRotation);
			}
		}
	}

	public void OnLeave(WorldRoadRobot robot)
	{
		robot.animator.ResetTrigger(WorldRoadRobot.Building);
		hasOpenScan = false;
		robot.scan.gameObject.SetActive(value: false);
		getPathFinish = false;
		tempTime = 0f;
	}
}
