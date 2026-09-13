namespace LuaScriptInterface;

public interface EventManager
{
	void DispatchCSEvent(int eventId, object userData);

	void DispatchCSEventSFSObject(int eventId, byte[] sfsObjBinary);
}
