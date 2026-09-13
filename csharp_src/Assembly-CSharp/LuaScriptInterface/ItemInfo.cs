namespace LuaScriptInterface;

public interface ItemInfo
{
	string uuid { get; set; }

	string itemId { get; set; }

	int count { get; set; }
}
