namespace LuaScriptInterface;

public interface UIManager
{
	void OpenWindow(string uiName, params object[] args);

	void DestroyWindow(string uiName, params object[] args);

	bool IsWindowOpen(string uiName);
}
