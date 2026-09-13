using System;
using Sfs2XLw.Entities.Data;
using UnityGameFramework.Runtime;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameEntryWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameEntry);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 10, 23, 2);
		Utils.RegisterFunc(L, -4, "Init", _m_Init_xlua_st_);
		Utils.RegisterFunc(L, -4, "RegisterAction", _m_RegisterAction_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetCrossServerComponent", _m_ResetCrossServerComponent_xlua_st_);
		Utils.RegisterFunc(L, -4, "RegisterComponent", _m_RegisterComponent_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnDisconnectRetry", _m_OnDisconnectRetry_xlua_st_);
		Utils.RegisterFunc(L, -4, "Shutdown", _m_Shutdown_xlua_st_);
		Utils.RegisterFunc(L, -4, "DisposeAction", _m_DisposeAction_xlua_st_);
		Utils.RegisterFunc(L, -4, "Update", _m_Update_xlua_st_);
		Utils.RegisterFunc(L, -4, "DebugLuaFile", _m_DebugLuaFile_xlua_st_);
		Utils.RegisterFunc(L, -2, "GameBase", _g_get_GameBase);
		Utils.RegisterFunc(L, -2, "Event", _g_get_Event);
		Utils.RegisterFunc(L, -2, "UIContainer", _g_get_UIContainer);
		Utils.RegisterFunc(L, -2, "UICamera", _g_get_UICamera);
		Utils.RegisterFunc(L, -2, "Resource", _g_get_Resource);
		Utils.RegisterFunc(L, -2, "Localization", _g_get_Localization);
		Utils.RegisterFunc(L, -2, "Network", _g_get_Network);
		Utils.RegisterFunc(L, -2, "NetworkCross", _g_get_NetworkCross);
		Utils.RegisterFunc(L, -2, "Sound", _g_get_Sound);
		Utils.RegisterFunc(L, -2, "Setting", _g_get_Setting);
		Utils.RegisterFunc(L, -2, "Lua", _g_get_Lua);
		Utils.RegisterFunc(L, -2, "Data", _g_get_Data);
		Utils.RegisterFunc(L, -2, "pb", _g_get_pb);
		Utils.RegisterFunc(L, -2, "Timer", _g_get_Timer);
		Utils.RegisterFunc(L, -2, "GlobalData", _g_get_GlobalData);
		Utils.RegisterFunc(L, -2, "Sdk", _g_get_Sdk);
		Utils.RegisterFunc(L, -2, "Device", _g_get_Device);
		Utils.RegisterFunc(L, -2, "BuildAnimatorManager", _g_get_BuildAnimatorManager);
		Utils.RegisterFunc(L, -2, "ConfigCache", _g_get_ConfigCache);
		Utils.RegisterFunc(L, -2, "LOD", _g_get_LOD);
		Utils.RegisterFunc(L, -2, "PayOrderData", _g_get_PayOrderData);
		Utils.RegisterFunc(L, -2, "pvrExtension", _g_get_pvrExtension);
		Utils.RegisterFunc(L, -2, "_placeHolderProp", _g_get__placeHolderProp);
		Utils.RegisterFunc(L, -1, "pvrExtension", _s_set_pvrExtension);
		Utils.RegisterFunc(L, -1, "_placeHolderProp", _s_set__placeHolderProp);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameEntry does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init_xlua_st_(IntPtr L)
	{
		try
		{
			GameEntry.Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterAction_xlua_st_(IntPtr L)
	{
		try
		{
			GameEntry.RegisterAction();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetCrossServerComponent_xlua_st_(IntPtr L)
	{
		try
		{
			GameEntry.ResetCrossServerComponent();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterComponent_xlua_st_(IntPtr L)
	{
		try
		{
			GameEntry.RegisterComponent((GameFrameworkComponent)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(GameFrameworkComponent)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDisconnectRetry_xlua_st_(IntPtr L)
	{
		try
		{
			GameEntry.OnDisconnectRetry();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Shutdown_xlua_st_(IntPtr L)
	{
		try
		{
			GameEntry.Shutdown();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisposeAction_xlua_st_(IntPtr L)
	{
		try
		{
			GameEntry.DisposeAction();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update_xlua_st_(IntPtr L)
	{
		try
		{
			GameEntry.Update((float)Lua.lua_tonumber(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugLuaFile_xlua_st_(IntPtr L)
	{
		try
		{
			GameEntry.DebugLuaFile(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GameBase(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.GameBase);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Event(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Event);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UIContainer(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.UIContainer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UICamera(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.UICamera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Resource(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Resource);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Localization(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Localization);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Network(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Network);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_NetworkCross(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.NetworkCross);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Sound(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Sound);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Setting(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Setting);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Lua(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Lua);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Data(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Data);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pb(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.pb);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Timer(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Timer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GlobalData(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.GlobalData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Sdk(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Sdk);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Device(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.Device);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BuildAnimatorManager(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.BuildAnimatorManager);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ConfigCache(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.ConfigCache);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LOD(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.LOD);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PayOrderData(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.PayOrderData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pvrExtension(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameEntry.pvrExtension);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__placeHolderProp(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushAny(L, GameEntry._placeHolderProp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pvrExtension(IntPtr L)
	{
		try
		{
			GameEntry.pvrExtension = (PowerVRExtensions)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(PowerVRExtensions));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__placeHolderProp(IntPtr L)
	{
		try
		{
			GameEntry._placeHolderProp = (ISFSObject)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(ISFSObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
