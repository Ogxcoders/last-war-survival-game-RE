using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldMeteoritePointObjectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldMeteoritePointObject);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 1, 0);
		Utils.RegisterFunc(L, -3, "CreateGameObject", _m_CreateGameObject);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "TryDelayDestroy", _m_TryDelayDestroy);
		Utils.RegisterFunc(L, -3, "DestroyImmediate", _m_DestroyImmediate);
		Utils.RegisterFunc(L, -3, "SetCountDownShow", _m_SetCountDownShow);
		Utils.RegisterFunc(L, -3, "SetAutoAdjustLod", _m_SetAutoAdjustLod);
		Utils.RegisterFunc(L, -2, "CanDestroy", _g_get_CanDestroy);
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
			if (Lua.lua_gettop(L) == 4 && objectTranslator.Assignable<WorldScene>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				WorldScene worldScene = (WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene));
				int pointIndex = Lua.xlua_tointeger(L, 3);
				int pType = Lua.xlua_tointeger(L, 4);
				WorldMeteoritePointObject o = new WorldMeteoritePointObject(worldScene, pointIndex, pType);
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMeteoritePointObject constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateGameObject(IntPtr L)
	{
		try
		{
			((WorldMeteoritePointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateGameObject();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			((WorldMeteoritePointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryDelayDestroy(IntPtr L)
	{
		try
		{
			WorldMeteoritePointObject obj = (WorldMeteoritePointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long currentTime = Lua.lua_toint64(L, 2);
			obj.TryDelayDestroy(currentTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyImmediate(IntPtr L)
	{
		try
		{
			((WorldMeteoritePointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DestroyImmediate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCountDownShow(IntPtr L)
	{
		try
		{
			WorldMeteoritePointObject obj = (WorldMeteoritePointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool countDownShow = Lua.lua_toboolean(L, 2);
			obj.SetCountDownShow(countDownShow);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAutoAdjustLod(IntPtr L)
	{
		try
		{
			((WorldMeteoritePointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetAutoAdjustLod();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CanDestroy(IntPtr L)
	{
		try
		{
			WorldMeteoritePointObject worldMeteoritePointObject = (WorldMeteoritePointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMeteoritePointObject.CanDestroy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
