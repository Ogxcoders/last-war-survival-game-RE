using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ModelManagerRoadObjectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ModelManager.RoadObject);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 1, 1);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "CreateGameObject", _m_CreateGameObject);
		Utils.RegisterFunc(L, -3, "UpdateGameObject", _m_UpdateGameObject);
		Utils.RegisterFunc(L, -3, "DoGuideStartAnim", _m_DoGuideStartAnim);
		Utils.RegisterFunc(L, -3, "SetIsVisible", _m_SetIsVisible);
		Utils.RegisterFunc(L, -3, "UpdateLod", _m_UpdateLod);
		Utils.RegisterFunc(L, -3, "UpdateFog", _m_UpdateFog);
		Utils.RegisterFunc(L, -3, "StartPrint", _m_StartPrint);
		Utils.RegisterFunc(L, -3, "UpdatePrintProgress", _m_UpdatePrintProgress);
		Utils.RegisterFunc(L, -3, "FinishPrint", _m_FinishPrint);
		Utils.RegisterFunc(L, -2, "uuid", _g_get_uuid);
		Utils.RegisterFunc(L, -1, "uuid", _s_set_uuid);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 2, 2);
		Utils.RegisterFunc(L, -2, "PrintRoadLeft", _g_get_PrintRoadLeft);
		Utils.RegisterFunc(L, -2, "PrintRoadRight", _g_get_PrintRoadRight);
		Utils.RegisterFunc(L, -1, "PrintRoadLeft", _s_set_PrintRoadLeft);
		Utils.RegisterFunc(L, -1, "PrintRoadRight", _s_set_PrintRoadRight);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 4 && objectTranslator.Assignable<ModelManager>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<ModelManager.ModelObjectType>(L, 4))
			{
				ModelManager parent = (ModelManager)objectTranslator.GetObject(L, 2, typeof(ModelManager));
				int pointId = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out ModelManager.ModelObjectType val);
				ModelManager.RoadObject o = new ModelManager.RoadObject(parent, pointId, val);
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager.RoadObject constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			((ModelManager.RoadObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateGameObject(IntPtr L)
	{
		try
		{
			((ModelManager.RoadObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateGameObject();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateGameObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager.RoadObject roadObject = (ModelManager.RoadObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object param = objectTranslator.GetObject(L, 2, typeof(object));
				roadObject.UpdateGameObject(param);
				return 0;
			}
			if (num == 1)
			{
				roadObject.UpdateGameObject();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager.RoadObject.UpdateGameObject!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoGuideStartAnim(IntPtr L)
	{
		try
		{
			ModelManager.RoadObject obj = (ModelManager.RoadObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int time = Lua.xlua_tointeger(L, 2);
			obj.DoGuideStartAnim(time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIsVisible(IntPtr L)
	{
		try
		{
			ModelManager.RoadObject obj = (ModelManager.RoadObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isVisible = Lua.lua_toboolean(L, 2);
			obj.SetIsVisible(isVisible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateLod(IntPtr L)
	{
		try
		{
			ModelManager.RoadObject obj = (ModelManager.RoadObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			obj.UpdateLod(lod);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateFog(IntPtr L)
	{
		try
		{
			ModelManager.RoadObject obj = (ModelManager.RoadObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int fogId = Lua.xlua_tointeger(L, 2);
			obj.UpdateFog(fogId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartPrint(IntPtr L)
	{
		try
		{
			ModelManager.RoadObject obj = (ModelManager.RoadObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			obj.StartPrint(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdatePrintProgress(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager.RoadObject roadObject = (ModelManager.RoadObject)objectTranslator.FastGetCSObj(L, 1);
			float progress = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Get(L, 3, out Vector4 val);
			roadObject.UpdatePrintProgress(progress, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FinishPrint(IntPtr L)
	{
		try
		{
			((ModelManager.RoadObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FinishPrint();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PrintRoadLeft(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, ModelManager.RoadObject.PrintRoadLeft);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PrintRoadRight(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, ModelManager.RoadObject.PrintRoadRight);
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
			ModelManager.RoadObject roadObject = (ModelManager.RoadObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, roadObject.uuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_PrintRoadLeft(IntPtr L)
	{
		try
		{
			ModelManager.RoadObject.PrintRoadLeft = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_PrintRoadRight(IntPtr L)
	{
		try
		{
			ModelManager.RoadObject.PrintRoadRight = (float)Lua.lua_tonumber(L, 1);
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
			((ModelManager.RoadObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
