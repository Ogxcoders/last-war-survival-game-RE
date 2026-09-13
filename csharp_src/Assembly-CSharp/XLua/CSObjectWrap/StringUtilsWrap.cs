using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class StringUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(StringUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 8, 0, 0);
		Utils.RegisterFunc(L, -4, "FormatPassedTime", _m_FormatPassedTime_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetLWEmojiName", _m_GetLWEmojiName_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTimerString", _m_GetTimerString_xlua_st_);
		Utils.RegisterFunc(L, -4, "VersionCompare", _m_VersionCompare_xlua_st_);
		Utils.RegisterFunc(L, -4, "FirstLaunchUpdateLanguageCompare", _m_FirstLaunchUpdateLanguageCompare_xlua_st_);
		Utils.RegisterFunc(L, -4, "FirstLaunchUpdateVersionCompare", _m_FirstLaunchUpdateVersionCompare_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetNotEmojiText", _m_GetNotEmojiText_xlua_st_);
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
				StringUtils o = new StringUtils();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to StringUtils constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FormatPassedTime_xlua_st_(IntPtr L)
	{
		try
		{
			string str = StringUtils.FormatPassedTime(Lua.lua_tonumber(L, 1));
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLWEmojiName_xlua_st_(IntPtr L)
	{
		try
		{
			string input = Lua.lua_tostring(L, 1);
			int uncodeStart = Lua.xlua_tointeger(L, 2);
			int uncodeEnd = Lua.xlua_tointeger(L, 3);
			string lWEmojiName = StringUtils.GetLWEmojiName(input, uncodeStart, uncodeEnd);
			Lua.lua_pushstring(L, lWEmojiName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTimerString_xlua_st_(IntPtr L)
	{
		try
		{
			string timerString = StringUtils.GetTimerString(Lua.lua_toint64(L, 1));
			Lua.lua_pushstring(L, timerString);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_VersionCompare_xlua_st_(IntPtr L)
	{
		try
		{
			string a = Lua.lua_tostring(L, 1);
			string b = Lua.lua_tostring(L, 2);
			int value = StringUtils.VersionCompare(a, b);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FirstLaunchUpdateLanguageCompare_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = StringUtils.FirstLaunchUpdateLanguageCompare(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FirstLaunchUpdateVersionCompare_xlua_st_(IntPtr L)
	{
		try
		{
			string hotUpdateMsg = Lua.lua_tostring(L, 1);
			string lwfile = Lua.lua_tostring(L, 2);
			string forceUpdateMsg = Lua.lua_tostring(L, 3);
			int versionDiff = Lua.xlua_tointeger(L, 4);
			int remoteTableVersion = Lua.xlua_tointeger(L, 5);
			bool newestVersion;
			bool value = StringUtils.FirstLaunchUpdateVersionCompare(hotUpdateMsg, lwfile, forceUpdateMsg, versionDiff, remoteTableVersion, out newestVersion);
			Lua.lua_pushboolean(L, value);
			Lua.lua_pushboolean(L, newestVersion);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNotEmojiText_xlua_st_(IntPtr L)
	{
		try
		{
			string notEmojiText = StringUtils.GetNotEmojiText(Lua.lua_tostring(L, 1));
			Lua.lua_pushstring(L, notEmojiText);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
