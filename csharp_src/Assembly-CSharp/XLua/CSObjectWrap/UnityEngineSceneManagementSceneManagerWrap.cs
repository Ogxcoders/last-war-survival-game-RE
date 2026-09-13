using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineSceneManagementSceneManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UnityEngine.SceneManagement.SceneManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 16, 2, 0);
		Utils.RegisterFunc(L, -4, "GetActiveScene", _m_GetActiveScene_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetActiveScene", _m_SetActiveScene_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSceneByPath", _m_GetSceneByPath_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSceneByName", _m_GetSceneByName_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSceneByBuildIndex", _m_GetSceneByBuildIndex_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSceneAt", _m_GetSceneAt_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateScene", _m_CreateScene_xlua_st_);
		Utils.RegisterFunc(L, -4, "MergeScenes", _m_MergeScenes_xlua_st_);
		Utils.RegisterFunc(L, -4, "MoveGameObjectToScene", _m_MoveGameObjectToScene_xlua_st_);
		Utils.RegisterFunc(L, -4, "LoadScene", _m_LoadScene_xlua_st_);
		Utils.RegisterFunc(L, -4, "LoadSceneAsync", _m_LoadSceneAsync_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnloadSceneAsync", _m_UnloadSceneAsync_xlua_st_);
		Utils.RegisterFunc(L, -4, "sceneLoaded", _e_sceneLoaded);
		Utils.RegisterFunc(L, -4, "sceneUnloaded", _e_sceneUnloaded);
		Utils.RegisterFunc(L, -4, "activeSceneChanged", _e_activeSceneChanged);
		Utils.RegisterFunc(L, -2, "sceneCount", _g_get_sceneCount);
		Utils.RegisterFunc(L, -2, "sceneCountInBuildSettings", _g_get_sceneCountInBuildSettings);
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
				UnityEngine.SceneManagement.SceneManager o = new UnityEngine.SceneManagement.SceneManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SceneManagement.SceneManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetActiveScene_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
			objectTranslator.Push(L, activeScene);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetActiveScene_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Scene v);
			bool value = UnityEngine.SceneManagement.SceneManager.SetActiveScene(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSceneByPath_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Scene sceneByPath = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, sceneByPath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSceneByName_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Scene sceneByName = UnityEngine.SceneManagement.SceneManager.GetSceneByName(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, sceneByName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSceneByBuildIndex_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Scene sceneByBuildIndex = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, sceneByBuildIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSceneAt_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Scene sceneAt = UnityEngine.SceneManagement.SceneManager.GetSceneAt(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, sceneAt);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateScene_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Scene scene = UnityEngine.SceneManagement.SceneManager.CreateScene(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, scene);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<CreateSceneParameters>(L, 2))
			{
				string sceneName = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out CreateSceneParameters v);
				Scene scene2 = UnityEngine.SceneManagement.SceneManager.CreateScene(sceneName, v);
				objectTranslator.Push(L, scene2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SceneManagement.SceneManager.CreateScene!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MergeScenes_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Scene v);
			objectTranslator.Get(L, 2, out Scene v2);
			UnityEngine.SceneManagement.SceneManager.MergeScenes(v, v2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveGameObjectToScene_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject go = (GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject));
			objectTranslator.Get(L, 2, out Scene v);
			UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(go, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadScene_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(Lua.xlua_tointeger(L, 1));
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(Lua.lua_tostring(L, 1));
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<LoadSceneMode>(L, 2))
			{
				int sceneBuildIndex = Lua.xlua_tointeger(L, 1);
				objectTranslator.Get(L, 2, out LoadSceneMode v);
				UnityEngine.SceneManagement.SceneManager.LoadScene(sceneBuildIndex, v);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<LoadSceneParameters>(L, 2))
			{
				int sceneBuildIndex2 = Lua.xlua_tointeger(L, 1);
				objectTranslator.Get(L, 2, out LoadSceneParameters v2);
				Scene scene = UnityEngine.SceneManagement.SceneManager.LoadScene(sceneBuildIndex2, v2);
				objectTranslator.Push(L, scene);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<LoadSceneMode>(L, 2))
			{
				string sceneName = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out LoadSceneMode v3);
				UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, v3);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<LoadSceneParameters>(L, 2))
			{
				string sceneName2 = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out LoadSceneParameters v4);
				Scene scene2 = UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName2, v4);
				objectTranslator.Push(L, scene2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SceneManagement.SceneManager.LoadScene!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadSceneAsync_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				AsyncOperation o = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Lua.xlua_tointeger(L, 1));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				AsyncOperation o2 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<LoadSceneMode>(L, 2))
			{
				int sceneBuildIndex = Lua.xlua_tointeger(L, 1);
				objectTranslator.Get(L, 2, out LoadSceneMode v);
				AsyncOperation o3 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex, v);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<LoadSceneParameters>(L, 2))
			{
				int sceneBuildIndex2 = Lua.xlua_tointeger(L, 1);
				objectTranslator.Get(L, 2, out LoadSceneParameters v2);
				AsyncOperation o4 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex2, v2);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<LoadSceneMode>(L, 2))
			{
				string sceneName = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out LoadSceneMode v3);
				AsyncOperation o5 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, v3);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<LoadSceneParameters>(L, 2))
			{
				string sceneName2 = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out LoadSceneParameters v4);
				AsyncOperation o6 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName2, v4);
				objectTranslator.Push(L, o6);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SceneManagement.SceneManager.LoadSceneAsync!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadSceneAsync_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				AsyncOperation o = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(Lua.xlua_tointeger(L, 1));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				AsyncOperation o2 = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<Scene>(L, 1))
			{
				objectTranslator.Get(L, 1, out Scene v);
				AsyncOperation o3 = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(v);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<UnloadSceneOptions>(L, 2))
			{
				int sceneBuildIndex = Lua.xlua_tointeger(L, 1);
				objectTranslator.Get(L, 2, out UnloadSceneOptions v2);
				AsyncOperation o4 = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneBuildIndex, v2);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<UnloadSceneOptions>(L, 2))
			{
				string sceneName = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out UnloadSceneOptions v3);
				AsyncOperation o5 = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName, v3);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Scene>(L, 1) && objectTranslator.Assignable<UnloadSceneOptions>(L, 2))
			{
				objectTranslator.Get(L, 1, out Scene v4);
				objectTranslator.Get(L, 2, out UnloadSceneOptions v5);
				AsyncOperation o6 = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(v4, v5);
				objectTranslator.Push(L, o6);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sceneCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, UnityEngine.SceneManagement.SceneManager.sceneCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sceneCountInBuildSettings(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_sceneLoaded(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			UnityAction<Scene, LoadSceneMode> unityAction = objectTranslator.GetDelegate<UnityAction<Scene, LoadSceneMode>>(L, 2);
			if (unityAction == null)
			{
				return Lua.luaL_error(L, "#2 need UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				UnityEngine.SceneManagement.SceneManager.sceneLoaded += unityAction;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				UnityEngine.SceneManagement.SceneManager.sceneLoaded -= unityAction;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SceneManagement.SceneManager.sceneLoaded!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_sceneUnloaded(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			UnityAction<Scene> unityAction = objectTranslator.GetDelegate<UnityAction<Scene>>(L, 2);
			if (unityAction == null)
			{
				return Lua.luaL_error(L, "#2 need UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene>!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				UnityEngine.SceneManagement.SceneManager.sceneUnloaded += unityAction;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= unityAction;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SceneManagement.SceneManager.sceneUnloaded!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_activeSceneChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			UnityAction<Scene, Scene> unityAction = objectTranslator.GetDelegate<UnityAction<Scene, Scene>>(L, 2);
			if (unityAction == null)
			{
				return Lua.luaL_error(L, "#2 need UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.Scene>!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				UnityEngine.SceneManagement.SceneManager.activeSceneChanged += unityAction;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= unityAction;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SceneManagement.SceneManager.activeSceneChanged!");
	}
}
