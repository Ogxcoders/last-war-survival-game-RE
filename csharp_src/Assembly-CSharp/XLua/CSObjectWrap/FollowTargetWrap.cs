using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FollowTargetWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FollowTarget);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 3, 3);
		Utils.RegisterFunc(L, -3, "SetTarget", _m_SetTarget);
		Utils.RegisterFunc(L, -3, "SetTargetAndOffset", _m_SetTargetAndOffset);
		Utils.RegisterFunc(L, -3, "SetTargetAndOffsetXYZ", _m_SetTargetAndOffsetXYZ);
		Utils.RegisterFunc(L, -2, "target", _g_get_target);
		Utils.RegisterFunc(L, -2, "offset", _g_get_offset);
		Utils.RegisterFunc(L, -2, "globalOffset", _g_get_globalOffset);
		Utils.RegisterFunc(L, -1, "target", _s_set_target);
		Utils.RegisterFunc(L, -1, "offset", _s_set_offset);
		Utils.RegisterFunc(L, -1, "globalOffset", _s_set_globalOffset);
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
				FollowTarget o = new FollowTarget();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FollowTarget constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FollowTarget followTarget = (FollowTarget)objectTranslator.FastGetCSObj(L, 1);
			Transform target = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			followTarget.SetTarget(target);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTargetAndOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FollowTarget followTarget = (FollowTarget)objectTranslator.FastGetCSObj(L, 1);
			Transform target = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			objectTranslator.Get(L, 3, out Vector3 val);
			followTarget.SetTargetAndOffset(target, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTargetAndOffsetXYZ(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FollowTarget followTarget = (FollowTarget)objectTranslator.FastGetCSObj(L, 1);
			Transform target = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			float offsetX = (float)Lua.lua_tonumber(L, 3);
			float offsetY = (float)Lua.lua_tonumber(L, 4);
			float offsetZ = (float)Lua.lua_tonumber(L, 5);
			followTarget.SetTargetAndOffsetXYZ(target, offsetX, offsetY, offsetZ);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FollowTarget followTarget = (FollowTarget)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, followTarget.target);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_offset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FollowTarget followTarget = (FollowTarget)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, followTarget.offset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_globalOffset(IntPtr L)
	{
		try
		{
			FollowTarget followTarget = (FollowTarget)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, followTarget.globalOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((FollowTarget)objectTranslator.FastGetCSObj(L, 1)).target = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_offset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FollowTarget followTarget = (FollowTarget)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			followTarget.offset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_globalOffset(IntPtr L)
	{
		try
		{
			((FollowTarget)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).globalOffset = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
