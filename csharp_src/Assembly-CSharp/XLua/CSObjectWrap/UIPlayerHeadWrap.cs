using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIPlayerHeadWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIPlayerHead);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 5, 2);
		Utils.RegisterFunc(L, -3, "SetData", _m_SetData);
		Utils.RegisterFunc(L, -3, "SetBigData", _m_SetBigData);
		Utils.RegisterFunc(L, -3, "SetFakeData", _m_SetFakeData);
		Utils.RegisterFunc(L, -3, "SetCustomLoadCallback", _m_SetCustomLoadCallback);
		Utils.RegisterFunc(L, -3, "UseSystemHead", _m_UseSystemHead);
		Utils.RegisterFunc(L, -3, "UseSpecifiedRes", _m_UseSpecifiedRes);
		Utils.RegisterFunc(L, -2, "IsValidated", _g_get_IsValidated);
		Utils.RegisterFunc(L, -2, "IsEnabled", _g_get_IsEnabled);
		Utils.RegisterFunc(L, -2, "IsSystemHead", _g_get_IsSystemHead);
		Utils.RegisterFunc(L, -2, "circleImage", _g_get_circleImage);
		Utils.RegisterFunc(L, -2, "spriteRenderer", _g_get_spriteRenderer);
		Utils.RegisterFunc(L, -1, "circleImage", _s_set_circleImage);
		Utils.RegisterFunc(L, -1, "spriteRenderer", _s_set_spriteRenderer);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 7, 5, 5);
		Utils.RegisterFunc(L, -4, "OnStaticUpdate", _m_OnStaticUpdate_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnLowMemory", _m_OnLowMemory_xlua_st_);
		Utils.RegisterFunc(L, -4, "DumpCache", _m_DumpCache_xlua_st_);
		Utils.RegisterFunc(L, -4, "DumpDynamicAssets", _m_DumpDynamicAssets_xlua_st_);
		Utils.RegisterFunc(L, -4, "DumpCollections", _m_DumpCollections_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "DefaultUserHead", "Assets/Main/Sprites/UI/LWCommon/Sprite/zyf_touxiang_da_hui");
		Utils.RegisterFunc(L, -2, "CollectOnlineHeadsSwitch", _g_get_CollectOnlineHeadsSwitch);
		Utils.RegisterFunc(L, -2, "CACHE_CAPACITY", _g_get_CACHE_CAPACITY);
		Utils.RegisterFunc(L, -2, "_cache", _g_get__cache);
		Utils.RegisterFunc(L, -2, "_waitingCount", _g_get__waitingCount);
		Utils.RegisterFunc(L, -2, "_loadingCount", _g_get__loadingCount);
		Utils.RegisterFunc(L, -1, "CollectOnlineHeadsSwitch", _s_set_CollectOnlineHeadsSwitch);
		Utils.RegisterFunc(L, -1, "CACHE_CAPACITY", _s_set_CACHE_CAPACITY);
		Utils.RegisterFunc(L, -1, "_cache", _s_set__cache);
		Utils.RegisterFunc(L, -1, "_waitingCount", _s_set__waitingCount);
		Utils.RegisterFunc(L, -1, "_loadingCount", _s_set__loadingCount);
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
				UIPlayerHead o = new UIPlayerHead();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIPlayerHead constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnStaticUpdate_xlua_st_(IntPtr L)
	{
		try
		{
			UIPlayerHead.OnStaticUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnLowMemory_xlua_st_(IntPtr L)
	{
		try
		{
			UIPlayerHead.OnLowMemory();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DumpCache_xlua_st_(IntPtr L)
	{
		try
		{
			string str = UIPlayerHead.DumpCache();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DumpDynamicAssets_xlua_st_(IntPtr L)
	{
		try
		{
			string str = UIPlayerHead.DumpDynamicAssets();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DumpCollections_xlua_st_(IntPtr L)
	{
		try
		{
			string str = UIPlayerHead.DumpCollections();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetData(IntPtr L)
	{
		try
		{
			UIPlayerHead uIPlayerHead = (UIPlayerHead)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				string uid = Lua.lua_tostring(L, 2);
				string pic = Lua.lua_tostring(L, 3);
				int picVer = Lua.xlua_tointeger(L, 4);
				bool useBig = Lua.lua_toboolean(L, 5);
				uIPlayerHead.SetData(uid, pic, picVer, useBig);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string uid2 = Lua.lua_tostring(L, 2);
				string pic2 = Lua.lua_tostring(L, 3);
				int picVer2 = Lua.xlua_tointeger(L, 4);
				uIPlayerHead.SetData(uid2, pic2, picVer2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIPlayerHead.SetData!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBigData(IntPtr L)
	{
		try
		{
			UIPlayerHead uIPlayerHead = (UIPlayerHead)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				string uid = Lua.lua_tostring(L, 2);
				string pic = Lua.lua_tostring(L, 3);
				int picVer = Lua.xlua_tointeger(L, 4);
				bool useBig = Lua.lua_toboolean(L, 5);
				uIPlayerHead.SetBigData(uid, pic, picVer, useBig);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string uid2 = Lua.lua_tostring(L, 2);
				string pic2 = Lua.lua_tostring(L, 3);
				int picVer2 = Lua.xlua_tointeger(L, 4);
				uIPlayerHead.SetBigData(uid2, pic2, picVer2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIPlayerHead.SetBigData!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFakeData(IntPtr L)
	{
		try
		{
			((UIPlayerHead)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetFakeData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCustomLoadCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIPlayerHead uIPlayerHead = (UIPlayerHead)objectTranslator.FastGetCSObj(L, 1);
			Action customLoadCallback = objectTranslator.GetDelegate<Action>(L, 2);
			uIPlayerHead.SetCustomLoadCallback(customLoadCallback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UseSystemHead(IntPtr L)
	{
		try
		{
			((UIPlayerHead)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UseSystemHead();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UseSpecifiedRes(IntPtr L)
	{
		try
		{
			UIPlayerHead obj = (UIPlayerHead)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string spResPath = Lua.lua_tostring(L, 2);
			obj.UseSpecifiedRes(spResPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CollectOnlineHeadsSwitch(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, UIPlayerHead.CollectOnlineHeadsSwitch);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsValidated(IntPtr L)
	{
		try
		{
			UIPlayerHead uIPlayerHead = (UIPlayerHead)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, uIPlayerHead.IsValidated);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsEnabled(IntPtr L)
	{
		try
		{
			UIPlayerHead uIPlayerHead = (UIPlayerHead)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, uIPlayerHead.IsEnabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsSystemHead(IntPtr L)
	{
		try
		{
			UIPlayerHead uIPlayerHead = (UIPlayerHead)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, uIPlayerHead.IsSystemHead);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CACHE_CAPACITY(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, UIPlayerHead.CACHE_CAPACITY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__cache(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, UIPlayerHead._cache);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__waitingCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, UIPlayerHead._waitingCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__loadingCount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, UIPlayerHead._loadingCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_circleImage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIPlayerHead uIPlayerHead = (UIPlayerHead)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIPlayerHead.circleImage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIPlayerHead uIPlayerHead = (UIPlayerHead)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIPlayerHead.spriteRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CollectOnlineHeadsSwitch(IntPtr L)
	{
		try
		{
			UIPlayerHead.CollectOnlineHeadsSwitch = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CACHE_CAPACITY(IntPtr L)
	{
		try
		{
			UIPlayerHead.CACHE_CAPACITY = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__cache(IntPtr L)
	{
		try
		{
			UIPlayerHead._cache = (Dictionary<string, UIPlayerHead.HeadRef>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Dictionary<string, UIPlayerHead.HeadRef>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__waitingCount(IntPtr L)
	{
		try
		{
			UIPlayerHead._waitingCount = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__loadingCount(IntPtr L)
	{
		try
		{
			UIPlayerHead._loadingCount = (Dictionary<string, int>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Dictionary<string, int>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_circleImage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIPlayerHead)objectTranslator.FastGetCSObj(L, 1)).circleImage = (CircleImage)objectTranslator.GetObject(L, 2, typeof(CircleImage));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spriteRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIPlayerHead)objectTranslator.FastGetCSObj(L, 1)).spriteRenderer = (SpriteRenderer)objectTranslator.GetObject(L, 2, typeof(SpriteRenderer));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
