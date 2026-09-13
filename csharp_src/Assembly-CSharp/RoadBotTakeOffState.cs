using UnityEngine;

public class RoadBotTakeOffState : IRoadBotState
{
	private float takeOffTime;

	private float time;

	private bool getPathFinish;

	public void OnEnter(WorldRoadRobot robot)
	{
		time = 0f;
		takeOffTime = robot.stateMachineDic[WorldRoadRobot.State.TakeOff];
		robot.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
		robot.transform.position = robot.BotCenterPos;
		robot.animator.SetTrigger(WorldRoadRobot.TakeOff);
		getPathFinish = true;
	}

	public void OnUpdate(WorldRoadRobot robot, float deltaTime, Vector3 scanPos)
	{
		if (getPathFinish)
		{
			time += deltaTime;
			if (time < takeOffTime)
			{
				float num = time / takeOffTime;
				float y = robot.flyHeight * robot.curve.takeOffPosYCurve.Evaluate(num);
				Vector3 position = robot.transform.position;
				position = new Vector3(position.x, y, position.z);
				robot.transform.position = position;
			}
			else
			{
				robot.ChangeState(WorldRoadRobot.State.TakeOffRotation);
			}
		}
	}

	public void OnLeave(WorldRoadRobot robot)
	{
		getPathFinish = false;
		time = 0f;
		robot.animator.ResetTrigger(WorldRoadRobot.TakeOff);
	}
}
