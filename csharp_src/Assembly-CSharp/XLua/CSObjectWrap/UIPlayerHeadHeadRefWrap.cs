using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIPlayerHeadHeadRefWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIPlayerHead.HeadRef);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 3, 3);
		Utils.RegisterFunc(L, -2, "assetKey", _g_get_assetKey);
		Utils.RegisterFunc(L, -2, "sprite", _g_get_sprite);
		Utils.RegisterFunc(L, -2, "refCount", _g_get_refCount);
		Utils.RegisterFunc(L, -1, "assetKey", _s_set_assetKey);
		Utils.RegisterFunc(L, -1, "sprite", _s_set_sprite);
		Utils.RegisterFunc(L, -1, "refCount", _s_set_refCount);
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
			if (Lua.lua_gettop(L) == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Texture2D>(L, 3))
			{
				string assetKey = Lua.lua_tostring(L, 2);
				Texture2D texture = (Texture2D)objectTranslator.GetObject(L, 3, typeof(Texture2D));
				UIPlayerHead.HeadRef o = new UIPlayerHead.HeadRef(assetKey, texture);
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIPlayerHead.HeadRef constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_assetKey(IntPtr L)
	{
		try
		{
			UIPlayerHead.HeadRef headRef = (UIPlayerHead.HeadRef)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, headRef.assetKey);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIPlayerHead.HeadRef headRef = (UIPlayerHead.HeadRef)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, headRef.sprite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_refCount(IntPtr L)
	{
		try
		{
			UIPlayerHead.HeadRef headRef = (UIPlayerHead.HeadRef)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, headRef.refCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_assetKey(IntPtr L)
	{
		try
		{
			((UIPlayerHead.HeadRef)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).assetKey = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIPlayerHead.HeadRef)objectTranslator.FastGetCSObj(L, 1)).sprite = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_refCount(IntPtr L)
	{
		try
		{
			((UIPlayerHead.HeadRef)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).refCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
