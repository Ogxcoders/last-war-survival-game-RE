using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MeteoriteWorldEffectPlayerLastKillDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MeteoriteWorldEffectPlayer.LastKillData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 9, 9);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -2, "position", _g_get_position);
		Utils.RegisterFunc(L, -2, "iconPic", _g_get_iconPic);
		Utils.RegisterFunc(L, -2, "iconPic2", _g_get_iconPic2);
		Utils.RegisterFunc(L, -2, "uuid", _g_get_uuid);
		Utils.RegisterFunc(L, -2, "headPic", _g_get_headPic);
		Utils.RegisterFunc(L, -2, "picVer", _g_get_picVer);
		Utils.RegisterFunc(L, -2, "score", _g_get_score);
		Utils.RegisterFunc(L, -2, "score2", _g_get_score2);
		Utils.RegisterFunc(L, -2, "worldScene", _g_get_worldScene);
		Utils.RegisterFunc(L, -1, "position", _s_set_position);
		Utils.RegisterFunc(L, -1, "iconPic", _s_set_iconPic);
		Utils.RegisterFunc(L, -1, "iconPic2", _s_set_iconPic2);
		Utils.RegisterFunc(L, -1, "uuid", _s_set_uuid);
		Utils.RegisterFunc(L, -1, "headPic", _s_set_headPic);
		Utils.RegisterFunc(L, -1, "picVer", _s_set_picVer);
		Utils.RegisterFunc(L, -1, "score", _s_set_score);
		Utils.RegisterFunc(L, -1, "score2", _s_set_score2);
		Utils.RegisterFunc(L, -1, "worldScene", _s_set_worldScene);
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
				MeteoriteWorldEffectPlayer.LastKillData o = new MeteoriteWorldEffectPlayer.LastKillData();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MeteoriteWorldEffectPlayer.LastKillData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.LastKillData obj = (MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			float deltaTime = (float)Lua.lua_tonumber(L, 3);
			bool value = obj.Update(lod, deltaTime);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.LastKillData lastKillData = (MeteoriteWorldEffectPlayer.LastKillData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, lastKillData.position);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_iconPic(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.LastKillData lastKillData = (MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, lastKillData.iconPic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_iconPic2(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.LastKillData lastKillData = (MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, lastKillData.iconPic2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uuid(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.LastKillData lastKillData = (MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, lastKillData.uuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_headPic(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.LastKillData lastKillData = (MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, lastKillData.headPic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_picVer(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.LastKillData lastKillData = (MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, lastKillData.picVer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_score(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.LastKillData lastKillData = (MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, lastKillData.score);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_score2(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.LastKillData lastKillData = (MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, lastKillData.score2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldScene(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.LastKillData lastKillData = (MeteoriteWorldEffectPlayer.LastKillData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lastKillData.worldScene);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.LastKillData lastKillData = (MeteoriteWorldEffectPlayer.LastKillData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			lastKillData.position = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_iconPic(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).iconPic = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_iconPic2(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).iconPic2 = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uuid(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uuid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_headPic(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).headPic = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_picVer(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).picVer = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_score(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).score = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_score2(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.LastKillData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).score2 = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_worldScene(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MeteoriteWorldEffectPlayer.LastKillData)objectTranslator.FastGetCSObj(L, 1)).worldScene = (WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
