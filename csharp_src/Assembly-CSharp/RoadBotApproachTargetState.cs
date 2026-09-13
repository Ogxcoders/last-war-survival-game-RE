using UnityEngine;

public class RoadBotApproachTargetState : IRoadBotState
{
	private bool getPathFinish;

	private float approachTime;

	private float time;

	private Vector3 nowPos;

	private bool isGoTarget;

	public void OnEnter(WorldRoadRobot robot)
	{
		time = 0f;
		approachTime = 1f;
		nowPos = robot.transform.position;
		isGoTarget = robot._lastState == WorldRoadRobot.State.GoTarget;
		if (isGoTarget)
		{
			robot.animator.SetTrigger(WorldRoadRobot.Building);
		}
		getPathFinish = true;
	}

	public void OnUpdate(WorldRoadRobot robot, float deltaTime, Vector3 scanPos)
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
				robot.ChangeState(WorldRoadRobot.State.WorkRotation);
			}
			else
			{
				robot.ChangeState(WorldRoadRobot.State.LandingRotation);
			}
		}
	}

	public void OnLeave(WorldRoadRobot robot)
	{
		if (isGoTarget)
		{
			robot.animator.ResetTrigger(WorldRoadRobot.Building);
		}
		getPathFinish = false;
	}
}
