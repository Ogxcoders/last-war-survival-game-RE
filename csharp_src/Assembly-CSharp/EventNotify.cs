using Sfs2X.Entities.Data;
using Sfs2X.Util;
using XLua;

public static class EventNotify
{
	public static void Fire(EventId eventId)
	{
		GameEntry.Event.Fire(eventId);
	}

	public static void FireLong(EventId eventId, long userData)
	{
		GameEntry.Event.Fire(eventId, userData);
	}

	public static void FireBool(EventId eventId, bool userData)
	{
		GameEntry.Event.Fire(eventId, userData);
	}

	public static void FireString(EventId eventId, string userData)
	{
		GameEntry.Event.Fire(eventId, userData);
	}

	public static void FireLuaTable(EventId eventId, LuaTable userData)
	{
		GameEntry.Event.Fire(eventId, userData);
	}

	public static void FireSFSObject(EventId eventId, byte[] sfsObjBinary)
	{
		GameEntry.Event.Fire(eventId, SFSObject.NewFromBinaryData(new ByteArray(sfsObjBinary)));
	}
}
