namespace Leopotam.EcsLite;

public interface IEcsDebugger
{
	void LogDebug(string log);

	void LogInfo(string log);

	void LogError(string log);

	void LogWarning(string log);
}
