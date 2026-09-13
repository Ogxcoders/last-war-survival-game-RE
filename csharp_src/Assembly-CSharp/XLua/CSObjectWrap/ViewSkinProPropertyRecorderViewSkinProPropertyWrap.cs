using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ViewSkinProPropertyRecorderViewSkinProPropertyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ViewSkinProPropertyRecorder.ViewSkinProProperty);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 4, 3);
		Utils.RegisterFunc(L, -2, "GameObject", _g_get_GameObject);
		Utils.RegisterFunc(L, -2, "Object", _g_get_Object);
		Utils.RegisterFunc(L, -2, "CustomTypeName", _g_get_CustomTypeName);
		Utils.RegisterFunc(L, -2, "CustomCodeName", _g_get_CustomCodeName);
		Utils.RegisterFunc(L, -1, "Object", _s_set_Object);
		Utils.RegisterFunc(L, -1, "CustomTypeName", _s_set_CustomTypeName);
		Utils.RegisterFunc(L, -1, "CustomCodeName", _s_set_CustomCodeName);
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
				ViewSkinProPropertyRecorder.ViewSkinProProperty o = new ViewSkinProPropertyRecorder.ViewSkinProProperty();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ViewSkinProPropertyRecorder.ViewSkinProProperty constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GameObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ViewSkinProPropertyRecorder.ViewSkinProProperty viewSkinProProperty = (ViewSkinProPropertyRecorder.ViewSkinProProperty)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, viewSkinProProperty.GameObject);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Object(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ViewSkinProPropertyRecorder.ViewSkinProProperty viewSkinProProperty = (ViewSkinProPropertyRecorder.ViewSkinProProperty)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, viewSkinProProperty.Object);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CustomTypeName(IntPtr L)
	{
		try
		{
			ViewSkinProPropertyRecorder.ViewSkinProProperty viewSkinProProperty = (ViewSkinProPropertyRecorder.ViewSkinProProperty)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, viewSkinProProperty.CustomTypeName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CustomCodeName(IntPtr L)
	{
		try
		{
			ViewSkinProPropertyRecorder.ViewSkinProProperty viewSkinProProperty = (ViewSkinProPropertyRecorder.ViewSkinProProperty)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, viewSkinProProperty.CustomCodeName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Object(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ViewSkinProPropertyRecorder.ViewSkinProProperty)objectTranslator.FastGetCSObj(L, 1)).Object = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CustomTypeName(IntPtr L)
	{
		try
		{
			((ViewSkinProPropertyRecorder.ViewSkinProProperty)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CustomTypeName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CustomCodeName(IntPtr L)
	{
		try
		{
			((ViewSkinProPropertyRecorder.ViewSkinProProperty)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CustomCodeName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
