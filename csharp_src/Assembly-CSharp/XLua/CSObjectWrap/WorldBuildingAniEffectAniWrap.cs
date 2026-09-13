using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldBuildingAniEffectAniWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldBuildingAniEffectAni);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 4, 4);
		Utils.RegisterFunc(L, -3, "PlayAnimation", _m_PlayAnimation);
		Utils.RegisterFunc(L, -3, "StopAll", _m_StopAll);
		Utils.RegisterFunc(L, -2, "effectNodeList", _g_get_effectNodeList);
		Utils.RegisterFunc(L, -2, "aniData", _g_get_aniData);
		Utils.RegisterFunc(L, -2, "loopAniName", _g_get_loopAniName);
		Utils.RegisterFunc(L, -2, "loopAni", _g_get_loopAni);
		Utils.RegisterFunc(L, -1, "effectNodeList", _s_set_effectNodeList);
		Utils.RegisterFunc(L, -1, "aniData", _s_set_aniData);
		Utils.RegisterFunc(L, -1, "loopAniName", _s_set_loopAniName);
		Utils.RegisterFunc(L, -1, "loopAni", _s_set_loopAni);
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
				WorldBuildingAniEffectAni o = new WorldBuildingAniEffectAni();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuildingAniEffectAni constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAnimation(IntPtr L)
	{
		try
		{
			WorldBuildingAniEffectAni obj = (WorldBuildingAniEffectAni)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			float startTime = (float)Lua.lua_tonumber(L, 3);
			obj.PlayAnimation(name, startTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopAll(IntPtr L)
	{
		try
		{
			((WorldBuildingAniEffectAni)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopAll();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_effectNodeList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuildingAniEffectAni worldBuildingAniEffectAni = (WorldBuildingAniEffectAni)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldBuildingAniEffectAni.effectNodeList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_aniData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuildingAniEffectAni worldBuildingAniEffectAni = (WorldBuildingAniEffectAni)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldBuildingAniEffectAni.aniData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loopAniName(IntPtr L)
	{
		try
		{
			WorldBuildingAniEffectAni worldBuildingAniEffectAni = (WorldBuildingAniEffectAni)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldBuildingAniEffectAni.loopAniName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loopAni(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuildingAniEffectAni worldBuildingAniEffectAni = (WorldBuildingAniEffectAni)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldBuildingAniEffectAni.loopAni);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_effectNodeList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldBuildingAniEffectAni)objectTranslator.FastGetCSObj(L, 1)).effectNodeList = (List<SimpleAnimation>)objectTranslator.GetObject(L, 2, typeof(List<SimpleAnimation>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_aniData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldBuildingAniEffectAni)objectTranslator.FastGetCSObj(L, 1)).aniData = (List<WorldBuildingAniEffectAni.Data>)objectTranslator.GetObject(L, 2, typeof(List<WorldBuildingAniEffectAni.Data>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loopAniName(IntPtr L)
	{
		try
		{
			((WorldBuildingAniEffectAni)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loopAniName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loopAni(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldBuildingAniEffectAni)objectTranslator.FastGetCSObj(L, 1)).loopAni = (SimpleAnimation)objectTranslator.GetObject(L, 2, typeof(SimpleAnimation));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
