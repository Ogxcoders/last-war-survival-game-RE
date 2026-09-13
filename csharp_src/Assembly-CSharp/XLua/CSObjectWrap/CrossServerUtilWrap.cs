using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CrossServerUtilWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CrossServerUtil);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterFunc(L, -4, "SilentLogin", _m_SilentLogin_xlua_st_);
		Utils.RegisterFunc(L, -4, "Update", _m_Update_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsCrossing", _m_IsCrossing_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowCloud", _m_ShowCloud_xlua_st_);
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
				CrossServerUtil o = new CrossServerUtil();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CrossServerUtil constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SilentLogin_xlua_st_(IntPtr L)
	{
		try
		{
			CrossServerUtil.SilentLogin();
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
			CrossServerUtil.Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCrossing_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = CrossServerUtil.IsCrossing();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowCloud_xlua_st_(IntPtr L)
	{
		try
		{
			CrossServerUtil.ShowCloud(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
