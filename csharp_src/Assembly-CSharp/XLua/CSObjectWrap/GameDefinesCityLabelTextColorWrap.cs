using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesCityLabelTextColorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.CityLabelTextColor);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 12, 12);
		Utils.RegisterFunc(L, -2, "Green", _g_get_Green);
		Utils.RegisterFunc(L, -2, "Blue", _g_get_Blue);
		Utils.RegisterFunc(L, -2, "White", _g_get_White);
		Utils.RegisterFunc(L, -2, "Yellow", _g_get_Yellow);
		Utils.RegisterFunc(L, -2, "Red", _g_get_Red);
		Utils.RegisterFunc(L, -2, "Purple", _g_get_Purple);
		Utils.RegisterFunc(L, -2, "Black", _g_get_Black);
		Utils.RegisterFunc(L, -2, "AllianceEnemy", _g_get_AllianceEnemy);
		Utils.RegisterFunc(L, -2, "ZoneEnemy", _g_get_ZoneEnemy);
		Utils.RegisterFunc(L, -2, "SeasonEnemy", _g_get_SeasonEnemy);
		Utils.RegisterFunc(L, -2, "SeasonCamp", _g_get_SeasonCamp);
		Utils.RegisterFunc(L, -2, "SeasonAssist", _g_get_SeasonAssist);
		Utils.RegisterFunc(L, -1, "Green", _s_set_Green);
		Utils.RegisterFunc(L, -1, "Blue", _s_set_Blue);
		Utils.RegisterFunc(L, -1, "White", _s_set_White);
		Utils.RegisterFunc(L, -1, "Yellow", _s_set_Yellow);
		Utils.RegisterFunc(L, -1, "Red", _s_set_Red);
		Utils.RegisterFunc(L, -1, "Purple", _s_set_Purple);
		Utils.RegisterFunc(L, -1, "Black", _s_set_Black);
		Utils.RegisterFunc(L, -1, "AllianceEnemy", _s_set_AllianceEnemy);
		Utils.RegisterFunc(L, -1, "ZoneEnemy", _s_set_ZoneEnemy);
		Utils.RegisterFunc(L, -1, "SeasonEnemy", _s_set_SeasonEnemy);
		Utils.RegisterFunc(L, -1, "SeasonCamp", _s_set_SeasonCamp);
		Utils.RegisterFunc(L, -1, "SeasonAssist", _s_set_SeasonAssist);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameDefines.CityLabelTextColor does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Green(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.Green);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Blue(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.Blue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_White(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.White);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Yellow(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.Yellow);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Red(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.Red);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Purple(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.Purple);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Black(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.Black);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AllianceEnemy(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.AllianceEnemy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ZoneEnemy(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.ZoneEnemy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SeasonEnemy(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.SeasonEnemy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SeasonCamp(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.SeasonCamp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SeasonAssist(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.CityLabelTextColor.SeasonAssist);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Green(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.Green = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Blue(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.Blue = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_White(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.White = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Yellow(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.Yellow = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Red(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.Red = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Purple(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.Purple = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Black(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.Black = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_AllianceEnemy(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.AllianceEnemy = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ZoneEnemy(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.ZoneEnemy = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SeasonEnemy(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.SeasonEnemy = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SeasonCamp(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.SeasonCamp = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SeasonAssist(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color32 v);
			GameDefines.CityLabelTextColor.SeasonAssist = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
