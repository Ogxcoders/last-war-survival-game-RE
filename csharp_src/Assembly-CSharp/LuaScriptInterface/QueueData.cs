using XLua;

namespace LuaScriptInterface;

[CSharpCallLua]
public interface QueueData
{
	long uuid { get; set; }

	string itemId { get; set; }

	long startTime { get; set; }

	long endTime { get; set; }

	string newItemId { get; set; }

	int type { get; set; }

	int GetQueueState();

	int GetParaState();
}
