using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SceneSkinMetaWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SceneSkinMeta);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 37, 37);
		Utils.RegisterFunc(L, -3, "GetMapType", _m_GetMapType);
		Utils.RegisterFunc(L, -3, "GetPreviewType", _m_GetPreviewType);
		Utils.RegisterFunc(L, -3, "IsCityStrongholdMode", _m_IsCityStrongholdMode);
		Utils.RegisterFunc(L, -3, "IsSnowMode", _m_IsSnowMode);
		Utils.RegisterFunc(L, -3, "IsDesertMode", _m_IsDesertMode);
		Utils.RegisterFunc(L, -3, "IsMummyMode", _m_IsMummyMode);
		Utils.RegisterFunc(L, -3, "IsDarknessMode", _m_IsDarknessMode);
		Utils.RegisterFunc(L, -3, "IsNineNationMode", _m_IsNineNationMode);
		Utils.RegisterFunc(L, -3, "IsNotSeason", _m_IsNotSeason);
		Utils.RegisterFunc(L, -3, "GetWorldTerrainOffset", _m_GetWorldTerrainOffset);
		Utils.RegisterFunc(L, -2, "id", _g_get_id);
		Utils.RegisterFunc(L, -2, "mapType", _g_get_mapType);
		Utils.RegisterFunc(L, -2, "loading_bg", _g_get_loading_bg);
		Utils.RegisterFunc(L, -2, "loading_logo", _g_get_loading_logo);
		Utils.RegisterFunc(L, -2, "world_deco_byte", _g_get_world_deco_byte);
		Utils.RegisterFunc(L, -2, "world_deco_asset", _g_get_world_deco_asset);
		Utils.RegisterFunc(L, -2, "world_block", _g_get_world_block);
		Utils.RegisterFunc(L, -2, "world_terrain", _g_get_world_terrain);
		Utils.RegisterFunc(L, -2, "world_terrain_low", _g_get_world_terrain_low);
		Utils.RegisterFunc(L, -2, "world_terrain_control", _g_get_world_terrain_control);
		Utils.RegisterFunc(L, -2, "world_terrain_black", _g_get_world_terrain_black);
		Utils.RegisterFunc(L, -2, "world_map", _g_get_world_map);
		Utils.RegisterFunc(L, -2, "world_city_color", _g_get_world_city_color);
		Utils.RegisterFunc(L, -2, "splash_fill_mode", _g_get_splash_fill_mode);
		Utils.RegisterFunc(L, -2, "world_zone_line", _g_get_world_zone_line);
		Utils.RegisterFunc(L, -2, "city_terrain", _g_get_city_terrain);
		Utils.RegisterFunc(L, -2, "city_deco", _g_get_city_deco);
		Utils.RegisterFunc(L, -2, "city_fog", _g_get_city_fog);
		Utils.RegisterFunc(L, -2, "radar_bg", _g_get_radar_bg);
		Utils.RegisterFunc(L, -2, "world_city_table_name", _g_get_world_city_table_name);
		Utils.RegisterFunc(L, -2, "world_terrain_mode", _g_get_world_terrain_mode);
		Utils.RegisterFunc(L, -2, "world_terrain_mode_mat", _g_get_world_terrain_mode_mat);
		Utils.RegisterFunc(L, -2, "troop_line_color", _g_get_troop_line_color);
		Utils.RegisterFunc(L, -2, "camera_scaling", _g_get_camera_scaling);
		Utils.RegisterFunc(L, -2, "seasonType", _g_get_seasonType);
		Utils.RegisterFunc(L, -2, "seasonNext", _g_get_seasonNext);
		Utils.RegisterFunc(L, -2, "world_fog", _g_get_world_fog);
		Utils.RegisterFunc(L, -2, "world_fog_bloody", _g_get_world_fog_bloody);
		Utils.RegisterFunc(L, -2, "light_monster", _g_get_light_monster);
		Utils.RegisterFunc(L, -2, "edge_performance", _g_get_edge_performance);
		Utils.RegisterFunc(L, -2, "loading_bgm", _g_get_loading_bgm);
		Utils.RegisterFunc(L, -2, "home_bgm", _g_get_home_bgm);
		Utils.RegisterFunc(L, -2, "world_bgm", _g_get_world_bgm);
		Utils.RegisterFunc(L, -2, "city_sound", _g_get_city_sound);
		Utils.RegisterFunc(L, -2, "world_sound", _g_get_world_sound);
		Utils.RegisterFunc(L, -2, "edge_world_fog", _g_get_edge_world_fog);
		Utils.RegisterFunc(L, -2, "cityPostProcessVolume", _g_get_cityPostProcessVolume);
		Utils.RegisterFunc(L, -1, "id", _s_set_id);
		Utils.RegisterFunc(L, -1, "mapType", _s_set_mapType);
		Utils.RegisterFunc(L, -1, "loading_bg", _s_set_loading_bg);
		Utils.RegisterFunc(L, -1, "loading_logo", _s_set_loading_logo);
		Utils.RegisterFunc(L, -1, "world_deco_byte", _s_set_world_deco_byte);
		Utils.RegisterFunc(L, -1, "world_deco_asset", _s_set_world_deco_asset);
		Utils.RegisterFunc(L, -1, "world_block", _s_set_world_block);
		Utils.RegisterFunc(L, -1, "world_terrain", _s_set_world_terrain);
		Utils.RegisterFunc(L, -1, "world_terrain_low", _s_set_world_terrain_low);
		Utils.RegisterFunc(L, -1, "world_terrain_control", _s_set_world_terrain_control);
		Utils.RegisterFunc(L, -1, "world_terrain_black", _s_set_world_terrain_black);
		Utils.RegisterFunc(L, -1, "world_map", _s_set_world_map);
		Utils.RegisterFunc(L, -1, "world_city_color", _s_set_world_city_color);
		Utils.RegisterFunc(L, -1, "splash_fill_mode", _s_set_splash_fill_mode);
		Utils.RegisterFunc(L, -1, "world_zone_line", _s_set_world_zone_line);
		Utils.RegisterFunc(L, -1, "city_terrain", _s_set_city_terrain);
		Utils.RegisterFunc(L, -1, "city_deco", _s_set_city_deco);
		Utils.RegisterFunc(L, -1, "city_fog", _s_set_city_fog);
		Utils.RegisterFunc(L, -1, "radar_bg", _s_set_radar_bg);
		Utils.RegisterFunc(L, -1, "world_city_table_name", _s_set_world_city_table_name);
		Utils.RegisterFunc(L, -1, "world_terrain_mode", _s_set_world_terrain_mode);
		Utils.RegisterFunc(L, -1, "world_terrain_mode_mat", _s_set_world_terrain_mode_mat);
		Utils.RegisterFunc(L, -1, "troop_line_color", _s_set_troop_line_color);
		Utils.RegisterFunc(L, -1, "camera_scaling", _s_set_camera_scaling);
		Utils.RegisterFunc(L, -1, "seasonType", _s_set_seasonType);
		Utils.RegisterFunc(L, -1, "seasonNext", _s_set_seasonNext);
		Utils.RegisterFunc(L, -1, "world_fog", _s_set_world_fog);
		Utils.RegisterFunc(L, -1, "world_fog_bloody", _s_set_world_fog_bloody);
		Utils.RegisterFunc(L, -1, "light_monster", _s_set_light_monster);
		Utils.RegisterFunc(L, -1, "edge_performance", _s_set_edge_performance);
		Utils.RegisterFunc(L, -1, "loading_bgm", _s_set_loading_bgm);
		Utils.RegisterFunc(L, -1, "home_bgm", _s_set_home_bgm);
		Utils.RegisterFunc(L, -1, "world_bgm", _s_set_world_bgm);
		Utils.RegisterFunc(L, -1, "city_sound", _s_set_city_sound);
		Utils.RegisterFunc(L, -1, "world_sound", _s_set_world_sound);
		Utils.RegisterFunc(L, -1, "edge_world_fog", _s_set_edge_world_fog);
		Utils.RegisterFunc(L, -1, "cityPostProcessVolume", _s_set_cityPostProcessVolume);
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
				SceneSkinMeta o = new SceneSkinMeta();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SceneSkinMeta constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMapType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonType mapType = ((SceneSkinMeta)objectTranslator.FastGetCSObj(L, 1)).GetMapType();
			objectTranslator.Push(L, mapType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPreviewType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonType previewType = ((SceneSkinMeta)objectTranslator.FastGetCSObj(L, 1)).GetPreviewType();
			objectTranslator.Push(L, previewType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCityStrongholdMode(IntPtr L)
	{
		try
		{
			bool value = ((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsCityStrongholdMode();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSnowMode(IntPtr L)
	{
		try
		{
			bool value = ((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsSnowMode();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDesertMode(IntPtr L)
	{
		try
		{
			bool value = ((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDesertMode();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMummyMode(IntPtr L)
	{
		try
		{
			bool value = ((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMummyMode();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDarknessMode(IntPtr L)
	{
		try
		{
			bool value = ((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDarknessMode();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNineNationMode(IntPtr L)
	{
		try
		{
			bool value = ((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsNineNationMode();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNotSeason(IntPtr L)
	{
		try
		{
			bool value = ((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsNotSeason();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldTerrainOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<Vector4> worldTerrainOffset = ((SceneSkinMeta)objectTranslator.FastGetCSObj(L, 1)).GetWorldTerrainOffset();
			objectTranslator.Push(L, worldTerrainOffset);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_id(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.id);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mapType(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.mapType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loading_bg(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.loading_bg);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loading_logo(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.loading_logo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_deco_byte(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_deco_byte);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_deco_asset(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_deco_asset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_block(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_block);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_terrain(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_terrain);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_terrain_low(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_terrain_low);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_terrain_control(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_terrain_control);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_terrain_black(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_terrain_black);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_map(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_map);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_city_color(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_city_color);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_splash_fill_mode(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.splash_fill_mode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_zone_line(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_zone_line);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_city_terrain(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.city_terrain);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_city_deco(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.city_deco);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_city_fog(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.city_fog);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radar_bg(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.radar_bg);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_city_table_name(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_city_table_name);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_terrain_mode(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.world_terrain_mode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_terrain_mode_mat(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_terrain_mode_mat);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_troop_line_color(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.troop_line_color);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_camera_scaling(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, sceneSkinMeta.camera_scaling);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_seasonType(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.seasonType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_seasonNext(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.seasonNext);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_fog(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_fog);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_fog_bloody(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.world_fog_bloody);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_light_monster(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.light_monster);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_edge_performance(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.edge_performance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loading_bgm(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.loading_bgm);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_home_bgm(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.home_bgm);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_bgm(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.world_bgm);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_city_sound(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.city_sound);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_world_sound(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, sceneSkinMeta.world_sound);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_edge_world_fog(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.edge_world_fog);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cityPostProcessVolume(IntPtr L)
	{
		try
		{
			SceneSkinMeta sceneSkinMeta = (SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, sceneSkinMeta.cityPostProcessVolume);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_id(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).id = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mapType(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mapType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loading_bg(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loading_bg = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loading_logo(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loading_logo = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_deco_byte(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_deco_byte = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_deco_asset(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_deco_asset = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_block(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_block = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_terrain(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_terrain = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_terrain_low(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_terrain_low = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_terrain_control(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_terrain_control = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_terrain_black(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_terrain_black = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_map(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_map = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_city_color(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_city_color = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_splash_fill_mode(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).splash_fill_mode = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_zone_line(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_zone_line = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_city_terrain(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).city_terrain = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_city_deco(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).city_deco = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_city_fog(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).city_fog = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_radar_bg(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).radar_bg = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_city_table_name(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_city_table_name = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_terrain_mode(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_terrain_mode = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_terrain_mode_mat(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_terrain_mode_mat = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_troop_line_color(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).troop_line_color = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_camera_scaling(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).camera_scaling = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_seasonType(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).seasonType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_seasonNext(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).seasonNext = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_fog(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_fog = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_fog_bloody(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_fog_bloody = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_light_monster(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).light_monster = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_edge_performance(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).edge_performance = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loading_bgm(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loading_bgm = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_home_bgm(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).home_bgm = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_bgm(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_bgm = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_city_sound(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).city_sound = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_world_sound(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).world_sound = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_edge_world_fog(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).edge_world_fog = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cityPostProcessVolume(IntPtr L)
	{
		try
		{
			((SceneSkinMeta)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cityPostProcessVolume = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
