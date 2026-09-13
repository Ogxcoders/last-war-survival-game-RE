using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIWorldPosHelperWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIWorldPosHelper);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "CalcConstructMilePointerSimple", _m_CalcConstructMilePointerSimple);
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
				UIWorldPosHelper o = new UIWorldPosHelper();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIWorldPosHelper constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			UIWorldPosHelper obj = (UIWorldPosHelper)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float scale = (float)Lua.lua_tonumber(L, 2);
			obj.Init(scale);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalcConstructMilePointerSimple(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIWorldPosHelper uIWorldPosHelper = (UIWorldPosHelper)objectTranslator.FastGetCSObj(L, 1);
			float leftPadding = (float)Lua.lua_tonumber(L, 2);
			float topPadding = (float)Lua.lua_tonumber(L, 3);
			objectTranslator.Get(L, 4, out Vector3 val);
			uIWorldPosHelper.CalcConstructMilePointerSimple(leftPadding, topPadding, val, out var show, out var dist, out var posX, out var posY, out var eulerAngleZ);
			Lua.lua_pushboolean(L, show);
			Lua.lua_pushnumber(L, dist);
			Lua.lua_pushnumber(L, posX);
			Lua.lua_pushnumber(L, posY);
			Lua.lua_pushnumber(L, eulerAngleZ);
			return 5;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
