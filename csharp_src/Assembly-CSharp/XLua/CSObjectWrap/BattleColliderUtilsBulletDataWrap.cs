using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BattleColliderUtilsBulletDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BattleColliderUtils.BulletData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 10, 10);
		Utils.RegisterFunc(L, -3, "CheckCD", _m_CheckCD);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -2, "transform", _g_get_transform);
		Utils.RegisterFunc(L, -2, "uid", _g_get_uid);
		Utils.RegisterFunc(L, -2, "radius", _g_get_radius);
		Utils.RegisterFunc(L, -2, "targetLayerMask", _g_get_targetLayerMask);
		Utils.RegisterFunc(L, -2, "capsule", _g_get_capsule);
		Utils.RegisterFunc(L, -2, "startOffset", _g_get_startOffset);
		Utils.RegisterFunc(L, -2, "endOffset", _g_get_endOffset);
		Utils.RegisterFunc(L, -2, "lastPos", _g_get_lastPos);
		Utils.RegisterFunc(L, -2, "dotCD", _g_get_dotCD);
		Utils.RegisterFunc(L, -2, "dotCDMax", _g_get_dotCDMax);
		Utils.RegisterFunc(L, -1, "transform", _s_set_transform);
		Utils.RegisterFunc(L, -1, "uid", _s_set_uid);
		Utils.RegisterFunc(L, -1, "radius", _s_set_radius);
		Utils.RegisterFunc(L, -1, "targetLayerMask", _s_set_targetLayerMask);
		Utils.RegisterFunc(L, -1, "capsule", _s_set_capsule);
		Utils.RegisterFunc(L, -1, "startOffset", _s_set_startOffset);
		Utils.RegisterFunc(L, -1, "endOffset", _s_set_endOffset);
		Utils.RegisterFunc(L, -1, "lastPos", _s_set_lastPos);
		Utils.RegisterFunc(L, -1, "dotCD", _s_set_dotCD);
		Utils.RegisterFunc(L, -1, "dotCDMax", _s_set_dotCDMax);
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
				BattleColliderUtils.BulletData o = new BattleColliderUtils.BulletData();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BattleColliderUtils.BulletData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckCD(IntPtr L)
	{
		try
		{
			BattleColliderUtils.BulletData obj = (BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			bool value = obj.CheckCD(deltaTime);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_transform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, bulletData.transform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uid(IntPtr L)
	{
		try
		{
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, bulletData.uid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radius(IntPtr L)
	{
		try
		{
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, bulletData.radius);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetLayerMask(IntPtr L)
	{
		try
		{
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, bulletData.targetLayerMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_capsule(IntPtr L)
	{
		try
		{
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, bulletData.capsule);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, bulletData.startOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, bulletData.endOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lastPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, bulletData.lastPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_dotCD(IntPtr L)
	{
		try
		{
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, bulletData.dotCD);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_dotCDMax(IntPtr L)
	{
		try
		{
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, bulletData.dotCDMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_transform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BattleColliderUtils.BulletData)objectTranslator.FastGetCSObj(L, 1)).transform = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uid(IntPtr L)
	{
		try
		{
			((BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uid = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radius(IntPtr L)
	{
		try
		{
			((BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).radius = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetLayerMask(IntPtr L)
	{
		try
		{
			((BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetLayerMask = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_capsule(IntPtr L)
	{
		try
		{
			((BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).capsule = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bulletData.startOffset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bulletData.endOffset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lastPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.BulletData bulletData = (BattleColliderUtils.BulletData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bulletData.lastPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_dotCD(IntPtr L)
	{
		try
		{
			((BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).dotCD = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_dotCDMax(IntPtr L)
	{
		try
		{
			((BattleColliderUtils.BulletData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).dotCDMax = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
