using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BattleColliderUtilsMonsterColliderDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BattleColliderUtils.MonsterColliderData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 11, 11);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -2, "transform", _g_get_transform);
		Utils.RegisterFunc(L, -2, "colliderCenter", _g_get_colliderCenter);
		Utils.RegisterFunc(L, -2, "colliderType", _g_get_colliderType);
		Utils.RegisterFunc(L, -2, "uid", _g_get_uid);
		Utils.RegisterFunc(L, -2, "viewHandle", _g_get_viewHandle);
		Utils.RegisterFunc(L, -2, "radius", _g_get_radius);
		Utils.RegisterFunc(L, -2, "targetLayerMask", _g_get_targetLayerMask);
		Utils.RegisterFunc(L, -2, "capsule", _g_get_capsule);
		Utils.RegisterFunc(L, -2, "halfVector", _g_get_halfVector);
		Utils.RegisterFunc(L, -2, "endOffset", _g_get_endOffset);
		Utils.RegisterFunc(L, -2, "halfExtents", _g_get_halfExtents);
		Utils.RegisterFunc(L, -1, "transform", _s_set_transform);
		Utils.RegisterFunc(L, -1, "colliderCenter", _s_set_colliderCenter);
		Utils.RegisterFunc(L, -1, "colliderType", _s_set_colliderType);
		Utils.RegisterFunc(L, -1, "uid", _s_set_uid);
		Utils.RegisterFunc(L, -1, "viewHandle", _s_set_viewHandle);
		Utils.RegisterFunc(L, -1, "radius", _s_set_radius);
		Utils.RegisterFunc(L, -1, "targetLayerMask", _s_set_targetLayerMask);
		Utils.RegisterFunc(L, -1, "capsule", _s_set_capsule);
		Utils.RegisterFunc(L, -1, "halfVector", _s_set_halfVector);
		Utils.RegisterFunc(L, -1, "endOffset", _s_set_endOffset);
		Utils.RegisterFunc(L, -1, "halfExtents", _s_set_halfExtents);
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
				BattleColliderUtils.MonsterColliderData o = new BattleColliderUtils.MonsterColliderData();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BattleColliderUtils.MonsterColliderData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
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
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, monsterColliderData.transform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colliderCenter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, monsterColliderData.colliderCenter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colliderType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushBattleColliderUtilsColliderType(L, monsterColliderData.colliderType);
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
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, monsterColliderData.uid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_viewHandle(IntPtr L)
	{
		try
		{
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, monsterColliderData.viewHandle);
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
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, monsterColliderData.radius);
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
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, monsterColliderData.targetLayerMask);
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
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, monsterColliderData.capsule);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_halfVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, monsterColliderData.halfVector);
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
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, monsterColliderData.endOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_halfExtents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, monsterColliderData.halfExtents);
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
			((BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1)).transform = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colliderCenter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			monsterColliderData.colliderCenter = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colliderType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out BattleColliderUtils.ColliderType val);
			monsterColliderData.colliderType = val;
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
			((BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uid = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_viewHandle(IntPtr L)
	{
		try
		{
			((BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).viewHandle = Lua.xlua_tointeger(L, 2);
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
			((BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).radius = (float)Lua.lua_tonumber(L, 2);
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
			((BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetLayerMask = Lua.xlua_tointeger(L, 2);
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
			((BattleColliderUtils.MonsterColliderData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).capsule = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_halfVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			monsterColliderData.halfVector = val;
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
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			monsterColliderData.endOffset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_halfExtents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleColliderUtils.MonsterColliderData monsterColliderData = (BattleColliderUtils.MonsterColliderData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			monsterColliderData.halfExtents = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
