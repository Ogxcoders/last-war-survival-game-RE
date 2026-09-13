using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BuildSelectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BuildSelect);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 0, 0);
		Utils.RegisterFunc(L, -3, "ChangeColor", _m_ChangeColor);
		Utils.RegisterFunc(L, -3, "CleanCloneRange", _m_CleanCloneRange);
		Utils.RegisterFunc(L, -3, "SetCloneRange", _m_SetCloneRange);
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
				BuildSelect o = new BuildSelect();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BuildSelect constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeColor(IntPtr L)
	{
		try
		{
			BuildSelect obj = (BuildSelect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isOk = Lua.lua_toboolean(L, 2);
			obj.ChangeColor(isOk);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CleanCloneRange(IntPtr L)
	{
		try
		{
			((BuildSelect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CleanCloneRange();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCloneRange(IntPtr L)
	{
		try
		{
			BuildSelect obj = (BuildSelect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int cloneRange = Lua.xlua_tointeger(L, 2);
			obj.SetCloneRange(cloneRange);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
