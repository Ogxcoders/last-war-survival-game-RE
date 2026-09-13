using UnityEngine;

public class BuildBotApproachTargetState : IBuildBotState
{
	private bool getPathFinish;

	private float approachTime;

	private float time;

	private Vector3 nowPos;

	private bool isGoTarget;

	public void OnEnter(WorldBuildRobot robot)
	{
		time = 0f;
		approachTime = 1f;
		nowPos = robot.transform.position;
		isGoTarget = robot._lastState == WorldBuildRobot.State.GoTarget;
		if (isGoTarget)
		{
			robot.animator.SetTrigger(WorldBuildRobot.Building);
		}
		getPathFinish = true;
	}

	public void OnUpdate(WorldBuildRobot robot, float deltaTime)
	{
		if (getPathFinish)
		{
			time += deltaTime;
			if (time < approachTime)
			{
				float num = time / approachTime;
				float num2 = (isGoTarget ? robot.curve.approachTargetDisCurve.Evaluate(num) : robot.curve.approachHomeDisCurve.Evaluate(num));
				Vector3 position = nowPos + robot.ApproachDir * num2;
				robot.transform.position = position;
			}
			else if (isGoTarget)
			{
				robot.ChangeState(WorldBuildRobot.State.WorkRotation);
			}
			else
			{
				robot.ChangeState(WorldBuildRobot.State.LandingRotation);
			}
		}
	}

	public void OnLeave(WorldBuildRobot robot)
	{
		if (isGoTarget)
		{
			robot.animator.ResetTrigger(WorldBuildRobot.Building);
		}
		getPathFinish = false;
	}
}
