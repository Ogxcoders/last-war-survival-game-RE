using System;
using VEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VEngineAssetWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Asset);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 3, 1);
		Utils.RegisterFunc(L, -3, "MoveNext", _m_MoveNext);
		Utils.RegisterFunc(L, -3, "Reset", _m_Reset);
		Utils.RegisterFunc(L, -2, "asset", _g_get_asset);
		Utils.RegisterFunc(L, -2, "Current", _g_get_Current);
		Utils.RegisterFunc(L, -2, "completed", _g_get_completed);
		Utils.RegisterFunc(L, -1, "completed", _s_set_completed);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 9, 1, 1);
		Utils.RegisterFunc(L, -4, "KeepAliveOnLoad", _m_KeepAliveOnLoad_xlua_st_);
		Utils.RegisterFunc(L, -4, "LoadAsync", _m_LoadAsync_xlua_st_);
		Utils.RegisterFunc(L, -4, "Load", _m_Load_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateAssets", _m_UpdateAssets_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveCachedUnusedAssets", _m_RemoveCachedUnusedAssets_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnloadUnusedAssets", _m_UnloadUnusedAssets_xlua_st_);
		Utils.RegisterFunc(L, -4, "DebugOutputCache", _m_DebugOutputCache_xlua_st_);
		Utils.RegisterFunc(L, -4, "DebugLoadCount", _m_DebugLoadCount_xlua_st_);
		Utils.RegisterFunc(L, -2, "EditorHookBeforeRequestAsset", _g_get_EditorHookBeforeRequestAsset);
		Utils.RegisterFunc(L, -1, "EditorHookBeforeRequestAsset", _s_set_EditorHookBeforeRequestAsset);
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
				Asset o = new Asset();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to VEngine.Asset constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveNext(IntPtr L)
	{
		try
		{
			bool value = ((Asset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MoveNext();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reset(IntPtr L)
	{
		try
		{
			((Asset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Reset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_KeepAliveOnLoad_xlua_st_(IntPtr L)
	{
		try
		{
			Asset.KeepAliveOnLoad((Asset)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Asset)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadAsync_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Type>(L, 2) && objectTranslator.Assignable<Action<Asset>>(L, 3))
			{
				string path = Lua.lua_tostring(L, 1);
				Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Action<Asset> completed = objectTranslator.GetDelegate<Action<Asset>>(L, 3);
				Asset o = Asset.LoadAsync(path, type, completed);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Type>(L, 2))
			{
				string path2 = Lua.lua_tostring(L, 1);
				Type type2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Asset o2 = Asset.LoadAsync(path2, type2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to VEngine.Asset.LoadAsync!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Load_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string path = Lua.lua_tostring(L, 1);
			Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
			Asset o = Asset.Load(path, type);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateAssets_xlua_st_(IntPtr L)
	{
		try
		{
			Asset.UpdateAssets();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveCachedUnusedAssets_xlua_st_(IntPtr L)
	{
		try
		{
			Asset.RemoveCachedUnusedAssets();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadUnusedAssets_xlua_st_(IntPtr L)
	{
		try
		{
			Asset.UnloadUnusedAssets();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugOutputCache_xlua_st_(IntPtr L)
	{
		try
		{
			Asset.DebugOutputCache();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugLoadCount_xlua_st_(IntPtr L)
	{
		try
		{
			Asset.DebugLoadCount();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Asset asset = (Asset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, asset.asset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Current(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Asset asset = (Asset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, asset.Current);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_completed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Asset asset = (Asset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, asset.completed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EditorHookBeforeRequestAsset(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Asset.EditorHookBeforeRequestAsset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_completed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Asset)objectTranslator.FastGetCSObj(L, 1)).completed = objectTranslator.GetDelegate<Action<Asset>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_EditorHookBeforeRequestAsset(IntPtr L)
	{
		try
		{
			Asset.EditorHookBeforeRequestAsset = ObjectTranslatorPool.Instance.Find(L).GetDelegate<Action<string>>(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
