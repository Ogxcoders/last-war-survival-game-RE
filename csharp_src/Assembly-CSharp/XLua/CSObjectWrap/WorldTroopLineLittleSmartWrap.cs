using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldTroopLineLittleSmartWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldTroopLineLittleSmart);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 3, 2);
		Utils.RegisterFunc(L, -3, "RefreshByMarch", _m_RefreshByMarch);
		Utils.RegisterFunc(L, -3, "UpdateSpeed", _m_UpdateSpeed);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "OnDrawGizmos", _m_OnDrawGizmos);
		Utils.RegisterFunc(L, -2, "Lines", _g_get_Lines);
		Utils.RegisterFunc(L, -2, "marchInfo", _g_get_marchInfo);
		Utils.RegisterFunc(L, -2, "uuid", _g_get_uuid);
		Utils.RegisterFunc(L, -1, "marchInfo", _s_set_marchInfo);
		Utils.RegisterFunc(L, -1, "uuid", _s_set_uuid);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "Create", _m_Create_xlua_st_);
		Utils.RegisterFunc(L, -4, "Recycle", _m_Recycle_xlua_st_);
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
				WorldTroopLineLittleSmart o = new WorldTroopLineLittleSmart();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroopLineLittleSmart constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Create_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldGpuInstancingRenderer renderer = (WorldGpuInstancingRenderer)objectTranslator.GetObject(L, 1, typeof(WorldGpuInstancingRenderer));
			long uuid = Lua.lua_toint64(L, 2);
			WorldTroopLineLittleSmart o = WorldTroopLineLittleSmart.Create(renderer, uuid);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Recycle_xlua_st_(IntPtr L)
	{
		try
		{
			WorldTroopLineLittleSmart.Recycle((WorldTroopLineLittleSmart)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(WorldTroopLineLittleSmart)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshByMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart worldTroopLineLittleSmart = (WorldTroopLineLittleSmart)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch marchInfo = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			worldTroopLineLittleSmart.RefreshByMarch(marchInfo);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateSpeed(IntPtr L)
	{
		try
		{
			WorldTroopLineLittleSmart obj = (WorldTroopLineLittleSmart)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float speed = (float)Lua.lua_tonumber(L, 2);
			float num = obj.UpdateSpeed(speed);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose(IntPtr L)
	{
		try
		{
			((WorldTroopLineLittleSmart)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrawGizmos(IntPtr L)
	{
		try
		{
			((WorldTroopLineLittleSmart)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDrawGizmos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Lines(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart worldTroopLineLittleSmart = (WorldTroopLineLittleSmart)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldTroopLineLittleSmart.Lines);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_marchInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart worldTroopLineLittleSmart = (WorldTroopLineLittleSmart)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldTroopLineLittleSmart.marchInfo);
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
			WorldTroopLineLittleSmart worldTroopLineLittleSmart = (WorldTroopLineLittleSmart)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldTroopLineLittleSmart.uuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_marchInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldTroopLineLittleSmart)objectTranslator.FastGetCSObj(L, 1)).marchInfo = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
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
			((WorldTroopLineLittleSmart)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
