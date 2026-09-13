using UnityEngine;

public interface IRoadBotState
{
	void OnEnter(WorldRoadRobot robot);

	void OnUpdate(WorldRoadRobot robot, float deltaTime, Vector3 scanPos);

	void OnLeave(WorldRoadRobot robot);
}
