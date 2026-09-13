public class BuildBotIdleState : IBuildBotState
{
	public void OnEnter(WorldBuildRobot bot)
	{
		bot.gameObject.SetActive(value: false);
		bot.scan.gameObject.SetActive(value: false);
		bot.transform.position = bot.BotCenterPos;
	}

	public void OnUpdate(WorldBuildRobot bot, float deltaTime)
	{
	}

	public void OnLeave(WorldBuildRobot bot)
	{
		bot.gameObject.SetActive(value: true);
	}
}
