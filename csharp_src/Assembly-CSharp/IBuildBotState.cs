public interface IBuildBotState
{
	void OnEnter(WorldBuildRobot robot);

	void OnUpdate(WorldBuildRobot robot, float deltaTime);

	void OnLeave(WorldBuildRobot robot);
}
