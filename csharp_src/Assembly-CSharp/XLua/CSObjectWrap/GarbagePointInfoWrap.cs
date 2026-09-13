using System;
using Protobuf;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GarbagePointInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GarbagePointInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 4, 4);
		Utils.RegisterFunc(L, -3, "Clone", _m_Clone);
		Utils.RegisterFunc(L, -2, "ownerUid", _g_get_ownerUid);
		Utils.RegisterFunc(L, -2, "uuid", _g_get_uuid);
		Utils.RegisterFunc(L, -2, "eventId", _g_get_eventId);
		Utils.RegisterFunc(L, -2, "endTime", _g_get_endTime);
		Utils.RegisterFunc(L, -1, "ownerUid", _s_set_ownerUid);
		Utils.RegisterFunc(L, -1, "uuid", _s_set_uuid);
		Utils.RegisterFunc(L, -1, "eventId", _s_set_eventId);
		Utils.RegisterFunc(L, -1, "endTime", _s_set_endTime);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 1)
			{
				GarbagePointInfo o = new GarbagePointInfo();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldPointInfo>(L, 2))
			{
				GarbagePointInfo o2 = new GarbagePointInfo((WorldPointInfo)objectTranslator.GetObject(L, 2, typeof(WorldPointInfo)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GarbagePointInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo o = ((GarbagePointInfo)objectTranslator.FastGetCSObj(L, 1)).Clone();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ownerUid(IntPtr L)
	{
		try
		{
			GarbagePointInfo garbagePointInfo = (GarbagePointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, garbagePointInfo.ownerUid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uuid(IntPtr L)
	{
		try
		{
			GarbagePointInfo garbagePointInfo = (GarbagePointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, garbagePointInfo.uuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_eventId(IntPtr L)
	{
		try
		{
			GarbagePointInfo garbagePointInfo = (GarbagePointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, garbagePointInfo.eventId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endTime(IntPtr L)
	{
		try
		{
			GarbagePointInfo garbagePointInfo = (GarbagePointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, garbagePointInfo.endTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ownerUid(IntPtr L)
	{
		try
		{
			((GarbagePointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ownerUid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uuid(IntPtr L)
	{
		try
		{
			((GarbagePointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_eventId(IntPtr L)
	{
		try
		{
			((GarbagePointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).eventId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endTime(IntPtr L)
	{
		try
		{
			((GarbagePointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).endTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
