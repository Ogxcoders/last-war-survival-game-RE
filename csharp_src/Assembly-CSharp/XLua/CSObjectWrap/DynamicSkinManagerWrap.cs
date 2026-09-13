using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DynamicSkinManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DynamicSkinManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 0, 0);
		Utils.RegisterFunc(L, -3, "Awake", _m_Awake);
		Utils.RegisterFunc(L, -3, "SetAsync", _m_SetAsync);
		Utils.RegisterFunc(L, -3, "ActiveSkin", _m_ActiveSkin);
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
				DynamicSkinManager o = new DynamicSkinManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DynamicSkinManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Awake(IntPtr L)
	{
		try
		{
			((DynamicSkinManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Awake();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAsync(IntPtr L)
	{
		try
		{
			DynamicSkinManager obj = (DynamicSkinManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool async = Lua.lua_toboolean(L, 2);
			obj.SetAsync(async);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ActiveSkin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicSkinManager dynamicSkinManager = (DynamicSkinManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nSeasonType = Lua.xlua_tointeger(L, 2);
				dynamicSkinManager.ActiveSkin(nSeasonType);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<SeasonType>(L, 2))
			{
				objectTranslator.Get(L, 2, out SeasonType v);
				dynamicSkinManager.ActiveSkin(v);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DynamicSkinManager.ActiveSkin!");
	}
}
