using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MeteoriteWorldEffectPlayerAbandonDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MeteoriteWorldEffectPlayer.AbandonData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 3, 3);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -2, "serverId", _g_get_serverId);
		Utils.RegisterFunc(L, -2, "flyTimeSec", _g_get_flyTimeSec);
		Utils.RegisterFunc(L, -2, "postEffectTimeSec", _g_get_postEffectTimeSec);
		Utils.RegisterFunc(L, -1, "serverId", _s_set_serverId);
		Utils.RegisterFunc(L, -1, "flyTimeSec", _s_set_flyTimeSec);
		Utils.RegisterFunc(L, -1, "postEffectTimeSec", _s_set_postEffectTimeSec);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "Create", _m_Create_xlua_st_);
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
				MeteoriteWorldEffectPlayer.AbandonData o = new MeteoriteWorldEffectPlayer.AbandonData();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MeteoriteWorldEffectPlayer.AbandonData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Create_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldScene scene = (WorldScene)objectTranslator.GetObject(L, 1, typeof(WorldScene));
			int serverId = Lua.xlua_tointeger(L, 2);
			int fromPoint = Lua.xlua_tointeger(L, 3);
			int toPoint = Lua.xlua_tointeger(L, 4);
			float elapsedSec = (float)Lua.lua_tonumber(L, 5);
			float flyTimeSec = (float)Lua.lua_tonumber(L, 6);
			float postEffectTimeSec = (float)Lua.lua_tonumber(L, 7);
			MeteoriteWorldEffectPlayer.AbandonData o = MeteoriteWorldEffectPlayer.AbandonData.Create(scene, serverId, fromPoint, toPoint, elapsedSec, flyTimeSec, postEffectTimeSec);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.AbandonData obj = (MeteoriteWorldEffectPlayer.AbandonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			float deltaTime = (float)Lua.lua_tonumber(L, 3);
			bool value = obj.Update(lod, deltaTime);
			Lua.lua_pushboolean(L, value);
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
			((MeteoriteWorldEffectPlayer.AbandonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serverId(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.AbandonData abandonData = (MeteoriteWorldEffectPlayer.AbandonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, abandonData.serverId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_flyTimeSec(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.AbandonData abandonData = (MeteoriteWorldEffectPlayer.AbandonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, abandonData.flyTimeSec);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_postEffectTimeSec(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.AbandonData abandonData = (MeteoriteWorldEffectPlayer.AbandonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, abandonData.postEffectTimeSec);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serverId(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.AbandonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).serverId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_flyTimeSec(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.AbandonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).flyTimeSec = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_postEffectTimeSec(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.AbandonData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).postEffectTimeSec = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
