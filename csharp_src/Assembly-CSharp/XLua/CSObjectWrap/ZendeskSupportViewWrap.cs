using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ZendeskSupportViewWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ZendeskSupportView);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterFunc(L, -4, "ShowMessaging", _m_ShowMessaging_xlua_st_);
		Utils.RegisterFunc(L, -4, "Show", _m_Show_xlua_st_);
		Utils.RegisterFunc(L, -4, "Close", _m_Close_xlua_st_);
		Utils.RegisterFunc(L, -4, "ShowWebView", _m_ShowWebView_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "ZendeskSupportView does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowMessaging_xlua_st_(IntPtr L)
	{
		try
		{
			ZendeskSupportView.ShowMessaging();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Show_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<GameObject>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				GameObject obj = (GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject));
				string url = Lua.lua_tostring(L, 2);
				Action whenDone = objectTranslator.GetDelegate<Action>(L, 3);
				string startCallJs = Lua.lua_tostring(L, 4);
				ZendeskSupportView.Show(obj, url, whenDone, startCallJs);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<GameObject>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action>(L, 3))
			{
				GameObject obj2 = (GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject));
				string url2 = Lua.lua_tostring(L, 2);
				Action whenDone2 = objectTranslator.GetDelegate<Action>(L, 3);
				ZendeskSupportView.Show(obj2, url2, whenDone2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<GameObject>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				GameObject obj3 = (GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject));
				string url3 = Lua.lua_tostring(L, 2);
				ZendeskSupportView.Show(obj3, url3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ZendeskSupportView.Show!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Close_xlua_st_(IntPtr L)
	{
		try
		{
			ZendeskSupportView.Close();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowWebView_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<ZendeskSupportView.ViewReturn>>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3) && objectTranslator.Assignable<MonoBehaviour>(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING))
			{
				string url = Lua.lua_tostring(L, 1);
				Action<ZendeskSupportView.ViewReturn> done = objectTranslator.GetDelegate<Action<ZendeskSupportView.ViewReturn>>(L, 2);
				RectTransform con = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				MonoBehaviour launcher = (MonoBehaviour)objectTranslator.GetObject(L, 4, typeof(MonoBehaviour));
				string startJsCode = Lua.lua_tostring(L, 5);
				ZendeskSupportView.ShowWebView(url, done, con, launcher, startJsCode);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<ZendeskSupportView.ViewReturn>>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3) && objectTranslator.Assignable<MonoBehaviour>(L, 4))
			{
				string url2 = Lua.lua_tostring(L, 1);
				Action<ZendeskSupportView.ViewReturn> done2 = objectTranslator.GetDelegate<Action<ZendeskSupportView.ViewReturn>>(L, 2);
				RectTransform con2 = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				MonoBehaviour launcher2 = (MonoBehaviour)objectTranslator.GetObject(L, 4, typeof(MonoBehaviour));
				ZendeskSupportView.ShowWebView(url2, done2, con2, launcher2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ZendeskSupportView.ShowWebView!");
	}
}
