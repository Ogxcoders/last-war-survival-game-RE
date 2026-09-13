using XLua;

namespace LuaScriptInterface;

public interface ItemData
{
	void UpdateItems(LuaTable data);

	void UpdateOneItem(LuaTable data);

	ItemInfo GetItemById(string itemId);

	StatusItemData GetStatusItem(int type);
}
