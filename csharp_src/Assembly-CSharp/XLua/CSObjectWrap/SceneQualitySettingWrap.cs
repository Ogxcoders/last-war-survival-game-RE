using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SceneQualitySettingWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SceneQualitySetting);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 9, 0, 0);
		Utils.RegisterFunc(L, -4, "ChangeQualitySetting", _m_ChangeQualitySetting_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetScale", _m_GetScale_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetResolutionQuality", _m_SetResolutionQuality_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetPixelHeightMax", _m_SetPixelHeightMax_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGraphicLevel", _m_GetGraphicLevel_xlua_st_);
		Utils.RegisterFunc(L, -4, "ApplySavedGraphicsLevel", _m_ApplySavedGraphicsLevel_xlua_st_);
		Utils.RegisterFunc(L, -4, "TryChangeFeatureToGpuSkinCompatable", _m_TryChangeFeatureToGpuSkinCompatable_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTerrainLevel", _m_GetTerrainLevel_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "SceneQualitySetting does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeQualitySetting_xlua_st_(IntPtr L)
	{
		try
		{
			SceneQualitySetting.ChangeQualitySetting();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetScale_xlua_st_(IntPtr L)
	{
		try
		{
			float scale = SceneQualitySetting.GetScale();
			Lua.lua_pushnumber(L, scale);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetResolutionQuality_xlua_st_(IntPtr L)
	{
		try
		{
			SceneQualitySetting.SetResolutionQuality();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPixelHeightMax_xlua_st_(IntPtr L)
	{
		try
		{
			SceneQualitySetting.SetPixelHeightMax(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGraphicLevel_xlua_st_(IntPtr L)
	{
		try
		{
			int graphicLevel = SceneQualitySetting.GetGraphicLevel();
			Lua.xlua_pushinteger(L, graphicLevel);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ApplySavedGraphicsLevel_xlua_st_(IntPtr L)
	{
		try
		{
			SceneQualitySetting.ApplySavedGraphicsLevel();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryChangeFeatureToGpuSkinCompatable_xlua_st_(IntPtr L)
	{
		try
		{
			SceneQualitySetting.TryChangeFeatureToGpuSkinCompatable();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTerrainLevel_xlua_st_(IntPtr L)
	{
		try
		{
			int terrainLevel = SceneQualitySetting.GetTerrainLevel();
			Lua.xlua_pushinteger(L, terrainLevel);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
