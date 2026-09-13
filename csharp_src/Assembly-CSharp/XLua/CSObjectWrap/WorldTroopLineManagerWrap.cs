using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldTroopLineManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldTroopLineManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 12, 7, 0);
		Utils.RegisterFunc(L, -3, "EditorDescription", _m_EditorDescription);
		Utils.RegisterFunc(L, -3, "UpdateTroopLineColor", _m_UpdateTroopLineColor);
		Utils.RegisterFunc(L, -3, "OnSkinChange", _m_OnSkinChange);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "CreateTroopLine", _m_CreateTroopLine);
		Utils.RegisterFunc(L, -3, "DestroyTroopLine", _m_DestroyTroopLine);
		Utils.RegisterFunc(L, -3, "IsTroopLineCreate", _m_IsTroopLineCreate);
		Utils.RegisterFunc(L, -3, "IsTroopLineMarchOutOfData", _m_IsTroopLineMarchOutOfData);
		Utils.RegisterFunc(L, -3, "UpdateTroopLineNew", _m_UpdateTroopLineNew);
		Utils.RegisterFunc(L, -3, "HideDestination", _m_HideDestination);
		Utils.RegisterFunc(L, -2, "ShowTroopLineMask", _g_get_ShowTroopLineMask);
		Utils.RegisterFunc(L, -2, "ShowTroopMidSprite", _g_get_ShowTroopMidSprite);
		Utils.RegisterFunc(L, -2, "TroopLineWidthScale", _g_get_TroopLineWidthScale);
		Utils.RegisterFunc(L, -2, "TroopCircleScale", _g_get_TroopCircleScale);
		Utils.RegisterFunc(L, -2, "CurrentDisplayLevel", _g_get_CurrentDisplayLevel);
		Utils.RegisterFunc(L, -2, "LegacyTroopLineCount", _g_get_LegacyTroopLineCount);
		Utils.RegisterFunc(L, -2, "NewTroopLineCount", _g_get_NewTroopLineCount);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 1, 0);
		Utils.RegisterFunc(L, -4, "GetLineColor", _m_GetLineColor_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetLineColorVec4", _m_GetLineColorVec4_xlua_st_);
		Utils.RegisterFunc(L, -2, "EditorInstance", _g_get_EditorInstance);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldScene>(L, 2))
			{
				WorldTroopLineManager o = new WorldTroopLineManager((WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroopLineManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorDescription(IntPtr L)
	{
		try
		{
			string str = ((WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EditorDescription();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTroopLineColor(IntPtr L)
	{
		try
		{
			WorldTroopLineManager obj = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string troopLineColorArgs = Lua.lua_tostring(L, 2);
			obj.UpdateTroopLineColor(troopLineColorArgs);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnSkinChange(IntPtr L)
	{
		try
		{
			((WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnSkinChange();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLineColor_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Color lineColor = WorldTroopLineManager.GetLineColor((WorldMarch)objectTranslator.GetObject(L, 1, typeof(WorldMarch)));
			objectTranslator.PushUnityEngineColor(L, lineColor);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLineColorVec4_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector4 lineColorVec = WorldTroopLineManager.GetLineColorVec4((WorldMarch)objectTranslator.GetObject(L, 1, typeof(WorldMarch)));
			objectTranslator.PushUnityEngineVector4(L, lineColorVec);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInit(IntPtr L)
	{
		try
		{
			((WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			WorldTroopLineManager obj = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(deltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateTroopLine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineManager worldTroopLineManager = (WorldTroopLineManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			worldTroopLineManager.CreateTroopLine(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyTroopLine(IntPtr L)
	{
		try
		{
			WorldTroopLineManager obj = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			obj.DestroyTroopLine(marchUuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTroopLineCreate(IntPtr L)
	{
		try
		{
			WorldTroopLineManager obj = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			bool value = obj.IsTroopLineCreate(marchUuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTroopLineMarchOutOfData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineManager worldTroopLineManager = (WorldTroopLineManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			bool value = worldTroopLineManager.IsTroopLineMarchOutOfData(march);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTroopLineNew(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineManager worldTroopLineManager = (WorldTroopLineManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			objectTranslator.Get(L, 3, out Vector3 val);
			worldTroopLineManager.UpdateTroopLineNew(march, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideDestination(IntPtr L)
	{
		try
		{
			WorldTroopLineManager obj = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			obj.HideDestination(uuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EditorInstance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, WorldTroopLineManager.EditorInstance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ShowTroopLineMask(IntPtr L)
	{
		try
		{
			WorldTroopLineManager worldTroopLineManager = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldTroopLineManager.ShowTroopLineMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ShowTroopMidSprite(IntPtr L)
	{
		try
		{
			WorldTroopLineManager worldTroopLineManager = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldTroopLineManager.ShowTroopMidSprite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TroopLineWidthScale(IntPtr L)
	{
		try
		{
			WorldTroopLineManager worldTroopLineManager = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldTroopLineManager.TroopLineWidthScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TroopCircleScale(IntPtr L)
	{
		try
		{
			WorldTroopLineManager worldTroopLineManager = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldTroopLineManager.TroopCircleScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrentDisplayLevel(IntPtr L)
	{
		try
		{
			WorldTroopLineManager worldTroopLineManager = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldTroopLineManager.CurrentDisplayLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LegacyTroopLineCount(IntPtr L)
	{
		try
		{
			WorldTroopLineManager worldTroopLineManager = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldTroopLineManager.LegacyTroopLineCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_NewTroopLineCount(IntPtr L)
	{
		try
		{
			WorldTroopLineManager worldTroopLineManager = (WorldTroopLineManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldTroopLineManager.NewTroopLineCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
