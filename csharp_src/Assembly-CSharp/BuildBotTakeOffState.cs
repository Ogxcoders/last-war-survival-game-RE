using UnityEngine;

public class BuildBotTakeOffState : IBuildBotState
{
	private float takeOffTime;

	private float time;

	private bool getPathFinish;

	public void OnEnter(WorldBuildRobot robot)
	{
		time = 0f;
		takeOffTime = robot.stateMachineDic[WorldBuildRobot.State.TakeOff];
		robot.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
		robot.transform.position = robot.BotCenterPos;
		robot.animator.SetTrigger(WorldBuildRobot.TakeOff);
		getPathFinish = true;
	}

	public void OnUpdate(WorldBuildRobot robot, float deltaTime)
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
				robot.ChangeState(WorldBuildRobot.State.TakeOffRotation);
			}
		}
	}

	public void OnLeave(WorldBuildRobot robot)
	{
		getPathFinish = false;
		time = 0f;
		robot.animator.ResetTrigger(WorldBuildRobot.TakeOff);
	}
}
