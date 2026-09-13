using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class RendererSorterWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RendererSorter);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 1, 1);
		Utils.RegisterFunc(L, -2, "BaseOrder", _g_get_BaseOrder);
		Utils.RegisterFunc(L, -1, "BaseOrder", _s_set_BaseOrder);
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
				RendererSorter o = new RendererSorter();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to RendererSorter constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BaseOrder(IntPtr L)
	{
		try
		{
			RendererSorter rendererSorter = (RendererSorter)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rendererSorter.BaseOrder);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BaseOrder(IntPtr L)
	{
		try
		{
			((RendererSorter)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).BaseOrder = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
