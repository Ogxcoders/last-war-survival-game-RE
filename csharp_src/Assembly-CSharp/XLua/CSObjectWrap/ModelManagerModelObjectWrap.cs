using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ModelManagerModelObjectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ModelManager.ModelObject);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 9, 6, 6);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "UpdateLod", _m_UpdateLod);
		Utils.RegisterFunc(L, -3, "UpdateFog", _m_UpdateFog);
		Utils.RegisterFunc(L, -3, "CreateGameObject", _m_CreateGameObject);
		Utils.RegisterFunc(L, -3, "UpdateGameObject", _m_UpdateGameObject);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "DoGuideStartAnim", _m_DoGuideStartAnim);
		Utils.RegisterFunc(L, -3, "SetIsVisible", _m_SetIsVisible);
		Utils.RegisterFunc(L, -3, "SetLabelActive", _m_SetLabelActive);
		Utils.RegisterFunc(L, -2, "instance", _g_get_instance);
		Utils.RegisterFunc(L, -2, "oldInstances", _g_get_oldInstances);
		Utils.RegisterFunc(L, -2, "pointIndex", _g_get_pointIndex);
		Utils.RegisterFunc(L, -2, "modelObjectType", _g_get_modelObjectType);
		Utils.RegisterFunc(L, -2, "isVisible", _g_get_isVisible);
		Utils.RegisterFunc(L, -2, "parent", _g_get_parent);
		Utils.RegisterFunc(L, -1, "instance", _s_set_instance);
		Utils.RegisterFunc(L, -1, "oldInstances", _s_set_oldInstances);
		Utils.RegisterFunc(L, -1, "pointIndex", _s_set_pointIndex);
		Utils.RegisterFunc(L, -1, "modelObjectType", _s_set_modelObjectType);
		Utils.RegisterFunc(L, -1, "isVisible", _s_set_isVisible);
		Utils.RegisterFunc(L, -1, "parent", _s_set_parent);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "ModelManager.ModelObject does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			((ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
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
			ModelManager.ModelObject obj = (ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			ModelManager.ModelObject obj = (ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_CreateGameObject(IntPtr L)
	{
		try
		{
			((ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateGameObject();
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
			ModelManager.ModelObject modelObject = (ModelManager.ModelObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object param = objectTranslator.GetObject(L, 2, typeof(object));
				modelObject.UpdateGameObject(param);
				return 0;
			}
			if (num == 1)
			{
				modelObject.UpdateGameObject();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager.ModelObject.UpdateGameObject!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			ModelManager.ModelObject obj = (ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(deltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoGuideStartAnim(IntPtr L)
	{
		try
		{
			ModelManager.ModelObject obj = (ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			ModelManager.ModelObject obj = (ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_SetLabelActive(IntPtr L)
	{
		try
		{
			ModelManager.ModelObject obj = (ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool labelActive = Lua.lua_toboolean(L, 2);
			obj.SetLabelActive(labelActive);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_instance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager.ModelObject modelObject = (ModelManager.ModelObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, modelObject.instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_oldInstances(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager.ModelObject modelObject = (ModelManager.ModelObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, modelObject.oldInstances);
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
			ModelManager.ModelObject modelObject = (ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, modelObject.pointIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_modelObjectType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager.ModelObject modelObject = (ModelManager.ModelObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushModelManagerModelObjectType(L, modelObject.modelObjectType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isVisible(IntPtr L)
	{
		try
		{
			ModelManager.ModelObject modelObject = (ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, modelObject.isVisible);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_parent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager.ModelObject modelObject = (ModelManager.ModelObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, modelObject.parent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_instance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ModelManager.ModelObject)objectTranslator.FastGetCSObj(L, 1)).instance = (InstanceRequest)objectTranslator.GetObject(L, 2, typeof(InstanceRequest));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_oldInstances(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ModelManager.ModelObject)objectTranslator.FastGetCSObj(L, 1)).oldInstances = (List<InstanceRequest>)objectTranslator.GetObject(L, 2, typeof(List<InstanceRequest>));
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
			((ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pointIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_modelObjectType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager.ModelObject modelObject = (ModelManager.ModelObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ModelManager.ModelObjectType val);
			modelObject.modelObjectType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isVisible(IntPtr L)
	{
		try
		{
			((ModelManager.ModelObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isVisible = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_parent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ModelManager.ModelObject)objectTranslator.FastGetCSObj(L, 1)).parent = (ModelManager)objectTranslator.GetObject(L, 2, typeof(ModelManager));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
