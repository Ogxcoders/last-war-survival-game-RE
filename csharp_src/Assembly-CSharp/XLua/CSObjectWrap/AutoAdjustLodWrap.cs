using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AutoAdjustLodWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AutoAdjustLod);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 14, 1, 0);
		Utils.RegisterFunc(L, -3, "GetSettings", _m_GetSettings);
		Utils.RegisterFunc(L, -3, "getLodType", _m_getLodType);
		Utils.RegisterFunc(L, -3, "SetLodType", _m_SetLodType);
		Utils.RegisterFunc(L, -3, "UpdateLod", _m_UpdateLod);
		Utils.RegisterFunc(L, -3, "SetBeforeLodFadeCallback", _m_SetBeforeLodFadeCallback);
		Utils.RegisterFunc(L, -3, "SetLodUpdateCallback", _m_SetLodUpdateCallback);
		Utils.RegisterFunc(L, -3, "DoUpdate", _m_DoUpdate);
		Utils.RegisterFunc(L, -3, "DoEnable", _m_DoEnable);
		Utils.RegisterFunc(L, -3, "DoDisable", _m_DoDisable);
		Utils.RegisterFunc(L, -3, "IsMainShow", _m_IsMainShow);
		Utils.RegisterFunc(L, -3, "IsShow", _m_IsShow);
		Utils.RegisterFunc(L, -3, "SetNoOptimizeActivate", _m_SetNoOptimizeActivate);
		Utils.RegisterFunc(L, -3, "AppendLod", _m_AppendLod);
		Utils.RegisterFunc(L, -3, "ClearAllAppend", _m_ClearAllAppend);
		Utils.RegisterFunc(L, -2, "LowLodLevelHasShowed", _g_get_LowLodLevelHasShowed);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterFunc(L, -4, "SetTargetOriginPos", _m_SetTargetOriginPos_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "FADE_DURATION", AutoAdjustLod.FADE_DURATION);
		Utils.RegisterObject(L, translator, -4, "OUT_POS", AutoAdjustLod.OUT_POS);
		Utils.RegisterObject(L, translator, -4, "targetOriginPos", AutoAdjustLod.targetOriginPos);
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
				AutoAdjustLod o = new AutoAdjustLod();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AutoAdjustLod constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTargetOriginPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject key = (GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject));
			objectTranslator.Get(L, 2, out Vector3 val);
			AutoAdjustLod.SetTargetOriginPos(key, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSettings(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoAdjustLod.Setting[] settings = ((AutoAdjustLod)objectTranslator.FastGetCSObj(L, 1)).GetSettings();
			objectTranslator.Push(L, settings);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_getLodType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LodType lodType = ((AutoAdjustLod)objectTranslator.FastGetCSObj(L, 1)).getLodType();
			objectTranslator.Push(L, lodType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLodType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoAdjustLod autoAdjustLod = (AutoAdjustLod)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LodType v);
			autoAdjustLod.SetLodType(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateLod(IntPtr L)
	{
		try
		{
			AutoAdjustLod obj = (AutoAdjustLod)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			obj.UpdateLod(lod);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBeforeLodFadeCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoAdjustLod autoAdjustLod = (AutoAdjustLod)objectTranslator.FastGetCSObj(L, 1);
			Action<bool, bool> beforeLodFadeCallback = objectTranslator.GetDelegate<Action<bool, bool>>(L, 2);
			autoAdjustLod.SetBeforeLodFadeCallback(beforeLodFadeCallback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLodUpdateCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoAdjustLod autoAdjustLod = (AutoAdjustLod)objectTranslator.FastGetCSObj(L, 1);
			Action<int> lodUpdateCallback = objectTranslator.GetDelegate<Action<int>>(L, 2);
			autoAdjustLod.SetLodUpdateCallback(lodUpdateCallback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoUpdate(IntPtr L)
	{
		try
		{
			((AutoAdjustLod)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoEnable(IntPtr L)
	{
		try
		{
			((AutoAdjustLod)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoEnable();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoDisable(IntPtr L)
	{
		try
		{
			((AutoAdjustLod)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoDisable();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMainShow(IntPtr L)
	{
		try
		{
			bool value = ((AutoAdjustLod)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMainShow();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsShow(IntPtr L)
	{
		try
		{
			bool value = ((AutoAdjustLod)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsShow();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNoOptimizeActivate(IntPtr L)
	{
		try
		{
			AutoAdjustLod obj = (AutoAdjustLod)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool noOptimizeActivate = Lua.lua_toboolean(L, 2);
			obj.SetNoOptimizeActivate(noOptimizeActivate);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AppendLod(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoAdjustLod autoAdjustLod = (AutoAdjustLod)objectTranslator.FastGetCSObj(L, 1);
			GameObject target = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			string lodRange = Lua.lua_tostring(L, 3);
			autoAdjustLod.AppendLod(target, lodRange);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllAppend(IntPtr L)
	{
		try
		{
			((AutoAdjustLod)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllAppend();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LowLodLevelHasShowed(IntPtr L)
	{
		try
		{
			AutoAdjustLod autoAdjustLod = (AutoAdjustLod)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, autoAdjustLod.LowLodLevelHasShowed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
