using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MeteoriteWorldEffectPlayerFragmentDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MeteoriteWorldEffectPlayer.FragmentData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 16, 16);
		Utils.RegisterFunc(L, -3, "IsVisible", _m_IsVisible);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -2, "fragmentType", _g_get_fragmentType);
		Utils.RegisterFunc(L, -2, "targetPosition", _g_get_targetPosition);
		Utils.RegisterFunc(L, -2, "serverId", _g_get_serverId);
		Utils.RegisterFunc(L, -2, "pointIndex", _g_get_pointIndex);
		Utils.RegisterFunc(L, -2, "tilePosition", _g_get_tilePosition);
		Utils.RegisterFunc(L, -2, "scale", _g_get_scale);
		Utils.RegisterFunc(L, -2, "height", _g_get_height);
		Utils.RegisterFunc(L, -2, "degree", _g_get_degree);
		Utils.RegisterFunc(L, -2, "dropTime", _g_get_dropTime);
		Utils.RegisterFunc(L, -2, "exploreTime", _g_get_exploreTime);
		Utils.RegisterFunc(L, -2, "crackTime", _g_get_crackTime);
		Utils.RegisterFunc(L, -2, "startTime", _g_get_startTime);
		Utils.RegisterFunc(L, -2, "rotateSpeed", _g_get_rotateSpeed);
		Utils.RegisterFunc(L, -2, "delayTimeSec", _g_get_delayTimeSec);
		Utils.RegisterFunc(L, -2, "worldScene", _g_get_worldScene);
		Utils.RegisterFunc(L, -2, "maxLod", _g_get_maxLod);
		Utils.RegisterFunc(L, -1, "fragmentType", _s_set_fragmentType);
		Utils.RegisterFunc(L, -1, "targetPosition", _s_set_targetPosition);
		Utils.RegisterFunc(L, -1, "serverId", _s_set_serverId);
		Utils.RegisterFunc(L, -1, "pointIndex", _s_set_pointIndex);
		Utils.RegisterFunc(L, -1, "tilePosition", _s_set_tilePosition);
		Utils.RegisterFunc(L, -1, "scale", _s_set_scale);
		Utils.RegisterFunc(L, -1, "height", _s_set_height);
		Utils.RegisterFunc(L, -1, "degree", _s_set_degree);
		Utils.RegisterFunc(L, -1, "dropTime", _s_set_dropTime);
		Utils.RegisterFunc(L, -1, "exploreTime", _s_set_exploreTime);
		Utils.RegisterFunc(L, -1, "crackTime", _s_set_crackTime);
		Utils.RegisterFunc(L, -1, "startTime", _s_set_startTime);
		Utils.RegisterFunc(L, -1, "rotateSpeed", _s_set_rotateSpeed);
		Utils.RegisterFunc(L, -1, "delayTimeSec", _s_set_delayTimeSec);
		Utils.RegisterFunc(L, -1, "worldScene", _s_set_worldScene);
		Utils.RegisterFunc(L, -1, "maxLod", _s_set_maxLod);
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
				MeteoriteWorldEffectPlayer.FragmentData o = new MeteoriteWorldEffectPlayer.FragmentData();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MeteoriteWorldEffectPlayer.FragmentData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsVisible(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData obj = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsVisible(lod);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData obj = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fragmentType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushMeteoriteWorldEffectPlayerFragmentDataFragmentType(L, fragmentData.fragmentType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, fragmentData.targetPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serverId(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, fragmentData.serverId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointIndex(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, fragmentData.pointIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tilePosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, fragmentData.tilePosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, fragmentData.scale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_height(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fragmentData.height);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_degree(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fragmentData.degree);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_dropTime(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fragmentData.dropTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_exploreTime(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fragmentData.exploreTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_crackTime(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fragmentData.crackTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startTime(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, fragmentData.startTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rotateSpeed(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fragmentData.rotateSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_delayTimeSec(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fragmentData.delayTimeSec);
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
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, fragmentData.worldScene);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxLod(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, fragmentData.maxLod);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fragmentType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out MeteoriteWorldEffectPlayer.FragmentData.FragmentType val);
			fragmentData.fragmentType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			fragmentData.targetPosition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serverId(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).serverId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointIndex(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pointIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tilePosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			fragmentData.tilePosition = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_scale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.FragmentData fragmentData = (MeteoriteWorldEffectPlayer.FragmentData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			fragmentData.scale = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_height(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).height = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_degree(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).degree = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_dropTime(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).dropTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_exploreTime(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).exploreTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_crackTime(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).crackTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startTime(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).startTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rotateSpeed(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).rotateSpeed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_delayTimeSec(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).delayTimeSec = (float)Lua.lua_tonumber(L, 2);
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
			((MeteoriteWorldEffectPlayer.FragmentData)objectTranslator.FastGetCSObj(L, 1)).worldScene = (WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxLod(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.FragmentData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxLod = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
