using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PVEStaticManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PVEStaticManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 0, 0);
		Utils.RegisterFunc(L, -3, "InitLightMapConfig", _m_InitLightMapConfig);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "InitLW", _m_InitLW);
		Utils.RegisterFunc(L, -3, "Append", _m_Append);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "OnUpdateData", _m_OnUpdateData);
		Utils.RegisterFunc(L, -3, "SetRenderOffsetZ", _m_SetRenderOffsetZ);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterObject(L, translator, -4, "PVEDecorationBytePath", "Assets/Main/Prefabs/PVELevel/{0}/decoration.bytes");
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
				PVEStaticManager o = new PVEStaticManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVEStaticManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitLightMapConfig(IntPtr L)
	{
		try
		{
			PVEStaticManager obj = (PVEStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string jsonStr = Lua.lua_tostring(L, 2);
			obj.InitLightMapConfig(jsonStr);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			PVEStaticManager obj = (PVEStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string sceneName = Lua.lua_tostring(L, 2);
			int tileCountPerChunk = Lua.xlua_tointeger(L, 3);
			int createCountPerFrame = Lua.xlua_tointeger(L, 4);
			obj.Init(sceneName, tileCountPerChunk, createCountPerFrame);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitLW(IntPtr L)
	{
		try
		{
			PVEStaticManager obj = (PVEStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int tileCountPerChunk = Lua.xlua_tointeger(L, 2);
			int createCountPerFrame = Lua.xlua_tointeger(L, 3);
			obj.InitLW(tileCountPerChunk, createCountPerFrame);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Append(IntPtr L)
	{
		try
		{
			PVEStaticManager obj = (PVEStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string sceneName = Lua.lua_tostring(L, 2);
			float offset = (float)Lua.lua_tonumber(L, 3);
			obj.Append(sceneName, offset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInit(IntPtr L)
	{
		try
		{
			((PVEStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			PVEStaticManager obj = (PVEStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int viewX = Lua.xlua_tointeger(L, 2);
			int viewY = Lua.xlua_tointeger(L, 3);
			obj.OnUpdate(viewX, viewY);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdateData(IntPtr L)
	{
		try
		{
			PVEStaticManager obj = (PVEStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int viewX = Lua.xlua_tointeger(L, 2);
			int viewY = Lua.xlua_tointeger(L, 3);
			int dataViewX = Lua.xlua_tointeger(L, 4);
			int dataViewY = Lua.xlua_tointeger(L, 5);
			obj.OnUpdateData(viewX, viewY, dataViewX, dataViewY);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRenderOffsetZ(IntPtr L)
	{
		try
		{
			PVEStaticManager obj = (PVEStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float renderOffsetZ = (float)Lua.lua_tonumber(L, 2);
			obj.SetRenderOffsetZ(renderOffsetZ);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
