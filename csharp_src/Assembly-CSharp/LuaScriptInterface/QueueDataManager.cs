using XLua;

namespace LuaScriptInterface;

public interface QueueDataManager
{
	void UpdateQueueData(LuaTable message);

	QueueData GetQueueByType(int qType);

	void ResetAllQueue();

	QueueData GetQueueByBuildUuidForFarm(long bUuid);

	bool GetCanPlantForPastureByBuildUuid(long bUuid);
}
