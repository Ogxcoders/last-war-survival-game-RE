using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FOWSystemWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FOWSystem);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 11, 19, 16);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "ClearRevealers", _m_ClearRevealers);
		Utils.RegisterFunc(L, -3, "GetRenderScale", _m_GetRenderScale);
		Utils.RegisterFunc(L, -3, "InitRender", _m_InitRender);
		Utils.RegisterFunc(L, -3, "ResetOrigin", _m_ResetOrigin);
		Utils.RegisterFunc(L, -3, "InitAllBuffer", _m_InitAllBuffer);
		Utils.RegisterFunc(L, -3, "RegisterCompleteAction", _m_RegisterCompleteAction);
		Utils.RegisterFunc(L, -3, "WorldToGridHeight", _m_WorldToGridHeight);
		Utils.RegisterFunc(L, -3, "IsVisible", _m_IsVisible);
		Utils.RegisterFunc(L, -3, "IsExplored", _m_IsExplored);
		Utils.RegisterFunc(L, -3, "CanShowWhiteFog", _m_CanShowWhiteFog);
		Utils.RegisterFunc(L, -2, "texture0", _g_get_texture0);
		Utils.RegisterFunc(L, -2, "texture1", _g_get_texture1);
		Utils.RegisterFunc(L, -2, "blendFactor", _g_get_blendFactor);
		Utils.RegisterFunc(L, -2, "isRevealRect", _g_get_isRevealRect);
		Utils.RegisterFunc(L, -2, "worldSize", _g_get_worldSize);
		Utils.RegisterFunc(L, -2, "textureSize", _g_get_textureSize);
		Utils.RegisterFunc(L, -2, "updateFrequency", _g_get_updateFrequency);
		Utils.RegisterFunc(L, -2, "textureBlendTime", _g_get_textureBlendTime);
		Utils.RegisterFunc(L, -2, "blurIterations", _g_get_blurIterations);
		Utils.RegisterFunc(L, -2, "heightRange", _g_get_heightRange);
		Utils.RegisterFunc(L, -2, "raycastMask", _g_get_raycastMask);
		Utils.RegisterFunc(L, -2, "raycastRadius", _g_get_raycastRadius);
		Utils.RegisterFunc(L, -2, "margin", _g_get_margin);
		Utils.RegisterFunc(L, -2, "debug", _g_get_debug);
		Utils.RegisterFunc(L, -2, "NeedDrawFogUpdateRange", _g_get_NeedDrawFogUpdateRange);
		Utils.RegisterFunc(L, -2, "WorldFogUpdateRange", _g_get_WorldFogUpdateRange);
		Utils.RegisterFunc(L, -2, "needUpdateCenterPos", _g_get_needUpdateCenterPos);
		Utils.RegisterFunc(L, -2, "subThreadUpdateFinish", _g_get_subThreadUpdateFinish);
		Utils.RegisterFunc(L, -2, "newCenterPos", _g_get_newCenterPos);
		Utils.RegisterFunc(L, -1, "isRevealRect", _s_set_isRevealRect);
		Utils.RegisterFunc(L, -1, "worldSize", _s_set_worldSize);
		Utils.RegisterFunc(L, -1, "textureSize", _s_set_textureSize);
		Utils.RegisterFunc(L, -1, "updateFrequency", _s_set_updateFrequency);
		Utils.RegisterFunc(L, -1, "textureBlendTime", _s_set_textureBlendTime);
		Utils.RegisterFunc(L, -1, "blurIterations", _s_set_blurIterations);
		Utils.RegisterFunc(L, -1, "heightRange", _s_set_heightRange);
		Utils.RegisterFunc(L, -1, "raycastMask", _s_set_raycastMask);
		Utils.RegisterFunc(L, -1, "raycastRadius", _s_set_raycastRadius);
		Utils.RegisterFunc(L, -1, "margin", _s_set_margin);
		Utils.RegisterFunc(L, -1, "debug", _s_set_debug);
		Utils.RegisterFunc(L, -1, "NeedDrawFogUpdateRange", _s_set_NeedDrawFogUpdateRange);
		Utils.RegisterFunc(L, -1, "WorldFogUpdateRange", _s_set_WorldFogUpdateRange);
		Utils.RegisterFunc(L, -1, "needUpdateCenterPos", _s_set_needUpdateCenterPos);
		Utils.RegisterFunc(L, -1, "subThreadUpdateFinish", _s_set_subThreadUpdateFinish);
		Utils.RegisterFunc(L, -1, "newCenterPos", _s_set_newCenterPos);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 3, 3);
		Utils.RegisterFunc(L, -4, "CreateRevealer", _m_CreateRevealer_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddRevealer", _m_AddRevealer_xlua_st_);
		Utils.RegisterFunc(L, -4, "DeleteRevealer", _m_DeleteRevealer_xlua_st_);
		Utils.RegisterFunc(L, -2, "screenScale", _g_get_screenScale);
		Utils.RegisterFunc(L, -2, "instance", _g_get_instance);
		Utils.RegisterFunc(L, -2, "mRevealers", _g_get_mRevealers);
		Utils.RegisterFunc(L, -1, "screenScale", _s_set_screenScale);
		Utils.RegisterFunc(L, -1, "instance", _s_set_instance);
		Utils.RegisterFunc(L, -1, "mRevealers", _s_set_mRevealers);
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
				FOWSystem o = new FOWSystem();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FOWSystem constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateRevealer_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem.Revealer o = FOWSystem.CreateRevealer();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddRevealer_xlua_st_(IntPtr L)
	{
		try
		{
			FOWSystem.AddRevealer((FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(FOWSystem.Revealer)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteRevealer_xlua_st_(IntPtr L)
	{
		try
		{
			FOWSystem.DeleteRevealer((FOWSystem.Revealer)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(FOWSystem.Revealer)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearRevealers(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearRevealers();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderScale(IntPtr L)
	{
		try
		{
			float renderScale = ((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetRenderScale();
			Lua.lua_pushnumber(L, renderScale);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitRender(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitRender();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetOrigin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			fOWSystem.ResetOrigin(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitAllBuffer(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitAllBuffer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterCompleteAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			Action onComplete = objectTranslator.GetDelegate<Action>(L, 2);
			fOWSystem.RegisterCompleteAction(onComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldToGridHeight(IntPtr L)
	{
		try
		{
			FOWSystem obj = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float height = (float)Lua.lua_tonumber(L, 2);
			int value = obj.WorldToGridHeight(height);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsVisible(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = fOWSystem.IsVisible(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsExplored(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = fOWSystem.IsExplored(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanShowWhiteFog(IntPtr L)
	{
		try
		{
			bool value = ((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CanShowWhiteFog();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_texture0(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, fOWSystem.texture0);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_texture1(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, fOWSystem.texture1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_blendFactor(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fOWSystem.blendFactor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isRevealRect(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, fOWSystem.isRevealRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_screenScale(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, FOWSystem.screenScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, FOWSystem.instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mRevealers(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, FOWSystem.mRevealers);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldSize(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, fOWSystem.worldSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureSize(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, fOWSystem.textureSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_updateFrequency(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fOWSystem.updateFrequency);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureBlendTime(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fOWSystem.textureBlendTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_blurIterations(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, fOWSystem.blurIterations);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_heightRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, fOWSystem.heightRange);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_raycastMask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, fOWSystem.raycastMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_raycastRadius(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fOWSystem.raycastRadius);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_margin(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, fOWSystem.margin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_debug(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, fOWSystem.debug);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_NeedDrawFogUpdateRange(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, fOWSystem.NeedDrawFogUpdateRange);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_WorldFogUpdateRange(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, fOWSystem.WorldFogUpdateRange);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_needUpdateCenterPos(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, fOWSystem.needUpdateCenterPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_subThreadUpdateFinish(IntPtr L)
	{
		try
		{
			FOWSystem fOWSystem = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, fOWSystem.subThreadUpdateFinish);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_newCenterPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, fOWSystem.newCenterPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isRevealRect(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isRevealRect = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_screenScale(IntPtr L)
	{
		try
		{
			FOWSystem.screenScale = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_instance(IntPtr L)
	{
		try
		{
			FOWSystem.instance = (FOWSystem)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(FOWSystem));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mRevealers(IntPtr L)
	{
		try
		{
			FOWSystem.mRevealers = (List<FOWSystem.Revealer>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(List<FOWSystem.Revealer>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_worldSize(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).worldSize = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureSize(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).textureSize = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_updateFrequency(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).updateFrequency = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureBlendTime(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).textureBlendTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_blurIterations(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).blurIterations = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_heightRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			fOWSystem.heightRange = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_raycastMask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LayerMask v);
			fOWSystem.raycastMask = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_raycastRadius(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).raycastRadius = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_margin(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).margin = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_debug(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).debug = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_NeedDrawFogUpdateRange(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).NeedDrawFogUpdateRange = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_WorldFogUpdateRange(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).WorldFogUpdateRange = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_needUpdateCenterPos(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).needUpdateCenterPos = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_subThreadUpdateFinish(IntPtr L)
	{
		try
		{
			((FOWSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).subThreadUpdateFinish = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_newCenterPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FOWSystem fOWSystem = (FOWSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			fOWSystem.newCenterPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
