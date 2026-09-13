using UnityEngine;

public class RoadBotLandingState : IRoadBotState
{
	private float landingTime;

	private float time;

	private bool getPathFinish;

	private float waitTime;

	private bool hasLanding;

	public void OnEnter(WorldRoadRobot robot)
	{
		hasLanding = false;
		time = 0f;
		waitTime = 0.2f;
		landingTime = robot.stateMachineDic[WorldRoadRobot.State.Landing];
		robot.animator.SetTrigger(WorldRoadRobot.Standby);
		getPathFinish = true;
	}

	public void OnUpdate(WorldRoadRobot robot, float deltaTime, Vector3 scanPos)
	{
		if (!getPathFinish)
		{
			return;
		}
		if (waitTime > 0f)
		{
			waitTime -= deltaTime;
			return;
		}
		if (!hasLanding)
		{
			robot.animator.SetTrigger(WorldBuildRobot.Landing);
			hasLanding = true;
		}
		time += deltaTime;
		if (time < landingTime)
		{
			float num = time / landingTime;
			float y = robot.flyHeight - robot.flyHeight * robot.curve.landingPosYCurve.Evaluate(num);
			Vector3 position = robot.transform.position;
			position = new Vector3(position.x, y, position.z);
			robot.transform.position = position;
		}
		else
		{
			robot.ChangeState(WorldRoadRobot.State.Idle);
			SceneManager.World.AddToNeedRemoveList(robot.uuid);
		}
	}

	public void OnLeave(WorldRoadRobot robot)
	{
		getPathFinish = false;
		time = 0f;
		robot.animator.ResetTrigger(WorldRoadRobot.Landing);
	}
}
