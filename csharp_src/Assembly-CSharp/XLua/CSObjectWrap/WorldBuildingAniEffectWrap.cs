using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldBuildingAniEffectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldBuildingAniEffect);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 4, 4);
		Utils.RegisterFunc(L, -3, "PlayAnimation", _m_PlayAnimation);
		Utils.RegisterFunc(L, -3, "StopAll", _m_StopAll);
		Utils.RegisterFunc(L, -3, "GetManagedEffectObjects", _m_GetManagedEffectObjects);
		Utils.RegisterFunc(L, -3, "GetStateEffectObjects", _m_GetStateEffectObjects);
		Utils.RegisterFunc(L, -3, "TestPlay", _m_TestPlay);
		Utils.RegisterFunc(L, -2, "effectNodeList", _g_get_effectNodeList);
		Utils.RegisterFunc(L, -2, "aniData", _g_get_aniData);
		Utils.RegisterFunc(L, -2, "startTime", _g_get_startTime);
		Utils.RegisterFunc(L, -2, "name", _g_get_name);
		Utils.RegisterFunc(L, -1, "effectNodeList", _s_set_effectNodeList);
		Utils.RegisterFunc(L, -1, "aniData", _s_set_aniData);
		Utils.RegisterFunc(L, -1, "startTime", _s_set_startTime);
		Utils.RegisterFunc(L, -1, "name", _s_set_name);
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
				WorldBuildingAniEffect o = new WorldBuildingAniEffect();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuildingAniEffect constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAnimation(IntPtr L)
	{
		try
		{
			WorldBuildingAniEffect obj = (WorldBuildingAniEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			((WorldBuildingAniEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopAll();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetManagedEffectObjects(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuildingAniEffect worldBuildingAniEffect = (WorldBuildingAniEffect)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<HashSet<UnityEngine.Object>>(L, 2))
			{
				HashSet<UnityEngine.Object> cache = (HashSet<UnityEngine.Object>)objectTranslator.GetObject(L, 2, typeof(HashSet<UnityEngine.Object>));
				HashSet<UnityEngine.Object> managedEffectObjects = worldBuildingAniEffect.GetManagedEffectObjects(cache);
				objectTranslator.Push(L, managedEffectObjects);
				return 1;
			}
			if (num == 1)
			{
				HashSet<UnityEngine.Object> managedEffectObjects2 = worldBuildingAniEffect.GetManagedEffectObjects();
				objectTranslator.Push(L, managedEffectObjects2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuildingAniEffect.GetManagedEffectObjects!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStateEffectObjects(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldBuildingAniEffect worldBuildingAniEffect = (WorldBuildingAniEffect)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<HashSet<UnityEngine.Object>>(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				HashSet<UnityEngine.Object> cache = (HashSet<UnityEngine.Object>)objectTranslator.GetObject(L, 3, typeof(HashSet<UnityEngine.Object>));
				HashSet<UnityEngine.Object> stateEffectObjects = worldBuildingAniEffect.GetStateEffectObjects(name, cache);
				objectTranslator.Push(L, stateEffectObjects);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name2 = Lua.lua_tostring(L, 2);
				HashSet<UnityEngine.Object> stateEffectObjects2 = worldBuildingAniEffect.GetStateEffectObjects(name2);
				objectTranslator.Push(L, stateEffectObjects2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldBuildingAniEffect.GetStateEffectObjects!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TestPlay(IntPtr L)
	{
		try
		{
			((WorldBuildingAniEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TestPlay();
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
			WorldBuildingAniEffect worldBuildingAniEffect = (WorldBuildingAniEffect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldBuildingAniEffect.effectNodeList);
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
			WorldBuildingAniEffect worldBuildingAniEffect = (WorldBuildingAniEffect)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldBuildingAniEffect.aniData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startTime(IntPtr L)
	{
		try
		{
			WorldBuildingAniEffect worldBuildingAniEffect = (WorldBuildingAniEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldBuildingAniEffect.startTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_name(IntPtr L)
	{
		try
		{
			WorldBuildingAniEffect worldBuildingAniEffect = (WorldBuildingAniEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldBuildingAniEffect.name);
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
			((WorldBuildingAniEffect)objectTranslator.FastGetCSObj(L, 1)).effectNodeList = (List<ParticleSystem>)objectTranslator.GetObject(L, 2, typeof(List<ParticleSystem>));
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
			((WorldBuildingAniEffect)objectTranslator.FastGetCSObj(L, 1)).aniData = (List<WorldBuildingAniEffect.Data>)objectTranslator.GetObject(L, 2, typeof(List<WorldBuildingAniEffect.Data>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startTime(IntPtr L)
	{
		try
		{
			((WorldBuildingAniEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).startTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_name(IntPtr L)
	{
		try
		{
			((WorldBuildingAniEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).name = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
