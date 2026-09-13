using UnityEngine;

public class RoadBotIdleState : IRoadBotState
{
	public void OnEnter(WorldRoadRobot bot)
	{
		bot.go.SetActive(value: false);
		bot.scan.gameObject.SetActive(value: false);
		bot.transform.position = bot.BotCenterPos;
	}

	public void OnUpdate(WorldRoadRobot bot, float deltaTime, Vector3 scanPos)
	{
	}

	public void OnLeave(WorldRoadRobot bot)
	{
		bot.go.SetActive(value: true);
	}
}
