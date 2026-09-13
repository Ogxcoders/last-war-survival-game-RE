using XLua;

namespace LuaScriptInterface;

public interface BuildQueueManager
{
	void UpdateQueueData(LuaTable message);
}
