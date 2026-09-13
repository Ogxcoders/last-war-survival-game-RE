using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SceneSkinManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SceneSkinManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 0, 0);
		Utils.RegisterFunc(L, -3, "SetCurSkinMeta", _m_SetCurSkinMeta);
		Utils.RegisterFunc(L, -3, "SetViewModeSkinMeta", _m_SetViewModeSkinMeta);
		Utils.RegisterFunc(L, -3, "GetCurSkinMeta", _m_GetCurSkinMeta);
		Utils.RegisterFunc(L, -3, "GetBaseSkinMeta", _m_GetBaseSkinMeta);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 1, 0);
		Utils.RegisterFunc(L, -4, "Purge", _m_Purge_xlua_st_);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
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
				SceneSkinManager o = new SceneSkinManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SceneSkinManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Purge_xlua_st_(IntPtr L)
	{
		try
		{
			SceneSkinManager.Purge();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCurSkinMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneSkinManager sceneSkinManager = (SceneSkinManager)objectTranslator.FastGetCSObj(L, 1);
			SceneSkinMeta curSkinMeta = (SceneSkinMeta)objectTranslator.GetObject(L, 2, typeof(SceneSkinMeta));
			sceneSkinManager.SetCurSkinMeta(curSkinMeta);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetViewModeSkinMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneSkinManager sceneSkinManager = (SceneSkinManager)objectTranslator.FastGetCSObj(L, 1);
			SceneSkinMeta viewModeSkinMeta = (SceneSkinMeta)objectTranslator.GetObject(L, 2, typeof(SceneSkinMeta));
			sceneSkinManager.SetViewModeSkinMeta(viewModeSkinMeta);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurSkinMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneSkinMeta curSkinMeta = ((SceneSkinManager)objectTranslator.FastGetCSObj(L, 1)).GetCurSkinMeta();
			objectTranslator.Push(L, curSkinMeta);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBaseSkinMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneSkinMeta baseSkinMeta = ((SceneSkinManager)objectTranslator.FastGetCSObj(L, 1)).GetBaseSkinMeta();
			objectTranslator.Push(L, baseSkinMeta);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SceneSkinManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
