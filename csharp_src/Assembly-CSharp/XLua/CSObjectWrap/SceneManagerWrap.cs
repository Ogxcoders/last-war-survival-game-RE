using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SceneManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SceneManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 27, 7, 3);
		Utils.RegisterFunc(L, -4, "ToggleBlendDepth", _m_ToggleBlendDepth_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetupSceneInEditor", _m_SetupSceneInEditor_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateWorld", _m_CreateWorld_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateCity", _m_CreateCity_xlua_st_);
		Utils.RegisterFunc(L, -4, "ChangeScene", _m_ChangeScene_xlua_st_);
		Utils.RegisterFunc(L, -4, "DestroyCurScene", _m_DestroyCurScene_xlua_st_);
		Utils.RegisterFunc(L, -4, "Destroy", _m_Destroy_xlua_st_);
		Utils.RegisterFunc(L, -4, "DestroyScene", _m_DestroyScene_xlua_st_);
		Utils.RegisterFunc(L, -4, "Update", _m_Update_xlua_st_);
		Utils.RegisterFunc(L, -4, "DebugClick", _m_DebugClick_xlua_st_);
		Utils.RegisterFunc(L, -4, "LateUpdate", _m_LateUpdate_xlua_st_);
		Utils.RegisterFunc(L, -4, "FixedUpdate", _m_FixedUpdate_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsInWorld", _m_IsInWorld_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsInCity", _m_IsInCity_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsInPVE", _m_IsInPVE_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsSceneNone", _m_IsSceneNone_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsSceneBuildFninsh", _m_IsSceneBuildFninsh_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCurZoneId", _m_GetCurZoneId_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetNightColor", _m_SetNightColor_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetNightColor", _m_ResetNightColor_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetRollStrengthZ", _m_SetRollStrengthZ_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetRollStrengthX", _m_SetRollStrengthX_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetRollStrengthZ", _m_ResetRollStrengthZ_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetSceneShadow", _m_SetSceneShadow_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetSceneShadow", _m_ResetSceneShadow_xlua_st_);
		Utils.RegisterFunc(L, -4, "EditorFocusGameObject", _m_EditorFocusGameObject_xlua_st_);
		Utils.RegisterFunc(L, -2, "World", _g_get_World);
		Utils.RegisterFunc(L, -2, "IsEditorMode", _g_get_IsEditorMode);
		Utils.RegisterFunc(L, -2, "MarchDataMgr", _g_get_MarchDataMgr);
		Utils.RegisterFunc(L, -2, "CurrSceneID", _g_get_CurrSceneID);
		Utils.RegisterFunc(L, -2, "CurrentSceneSubType", _g_get_CurrentSceneSubType);
		Utils.RegisterFunc(L, -2, "WorldTerrainAssetHandler", _g_get_WorldTerrainAssetHandler);
		Utils.RegisterFunc(L, -2, "DISABLE_UNIFORM_EVENT_DISPATCH", _g_get_DISABLE_UNIFORM_EVENT_DISPATCH);
		Utils.RegisterFunc(L, -1, "CurrSceneID", _s_set_CurrSceneID);
		Utils.RegisterFunc(L, -1, "CurrentSceneSubType", _s_set_CurrentSceneSubType);
		Utils.RegisterFunc(L, -1, "DISABLE_UNIFORM_EVENT_DISPATCH", _s_set_DISABLE_UNIFORM_EVENT_DISPATCH);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "SceneManager does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToggleBlendDepth_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.ToggleBlendDepth(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupSceneInEditor_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneInterface scene = (SceneInterface)objectTranslator.GetObject(L, 1, typeof(SceneInterface));
			objectTranslator.Get(L, 2, out SceneManager.SceneID val);
			SceneManager.SetupSceneInEditor(scene, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateWorld_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.CreateWorld();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateCity_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.CreateCity();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeScene_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out SceneManager.SceneID val);
			SceneManager.ChangeScene(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyCurScene_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.DestroyCurScene();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.Destroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyScene_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.DestroyScene((MonoBehaviour)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(MonoBehaviour)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugClick_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.DebugClick();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LateUpdate_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.LateUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FixedUpdate_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.FixedUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInWorld_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SceneManager.IsInWorld();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInCity_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SceneManager.IsInCity();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInPVE_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SceneManager.IsInPVE();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSceneNone_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SceneManager.IsSceneNone();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSceneBuildFninsh_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SceneManager.IsSceneBuildFninsh();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurZoneId_xlua_st_(IntPtr L)
	{
		try
		{
			int curZoneId = SceneManager.GetCurZoneId();
			Lua.xlua_pushinteger(L, curZoneId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNightColor_xlua_st_(IntPtr L)
	{
		try
		{
			float r = (float)Lua.lua_tonumber(L, 1);
			float g = (float)Lua.lua_tonumber(L, 2);
			float b = (float)Lua.lua_tonumber(L, 3);
			float intensity = (float)Lua.lua_tonumber(L, 4);
			SceneManager.SetNightColor(r, g, b, intensity);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetNightColor_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.ResetNightColor();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRollStrengthZ_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.SetRollStrengthZ((float)Lua.lua_tonumber(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRollStrengthX_xlua_st_(IntPtr L)
	{
		try
		{
			float posX = (float)Lua.lua_tonumber(L, 1);
			float posZ = (float)Lua.lua_tonumber(L, 2);
			float x = (float)Lua.lua_tonumber(L, 3);
			SceneManager.SetRollStrengthX(posX, posZ, x);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetRollStrengthZ_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.ResetRollStrengthZ();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSceneShadow_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color val);
			float shadowFalloff = (float)Lua.lua_tonumber(L, 2);
			bool value = SceneManager.SetSceneShadow(val, shadowFalloff);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetSceneShadow_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.ResetSceneShadow();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorFocusGameObject_xlua_st_(IntPtr L)
	{
		try
		{
			SceneManager.EditorFocusGameObject((GameObject)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(GameObject)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_World(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushAny(L, SceneManager.World);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsEditorMode(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SceneManager.IsEditorMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MarchDataMgr(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SceneManager.MarchDataMgr);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrSceneID(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SceneManager.CurrSceneID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrentSceneSubType(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SceneManager.CurrentSceneSubType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_WorldTerrainAssetHandler(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SceneManager.WorldTerrainAssetHandler);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DISABLE_UNIFORM_EVENT_DISPATCH(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CurrSceneID(IntPtr L)
	{
		try
		{
			SceneManager.CurrSceneID = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CurrentSceneSubType(IntPtr L)
	{
		try
		{
			SceneManager.CurrentSceneSubType = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DISABLE_UNIFORM_EVENT_DISPATCH(IntPtr L)
	{
		try
		{
			SceneManager.DISABLE_UNIFORM_EVENT_DISPATCH = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
