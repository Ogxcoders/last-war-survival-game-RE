using System;
using System.Collections.Generic;
using GameKit.Base;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineGameObjectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameObject);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 31, 9, 3);
		Utils.RegisterFunc(L, -3, "GetComponent", _m_GetComponent);
		Utils.RegisterFunc(L, -3, "GetComponentInChildren", _m_GetComponentInChildren);
		Utils.RegisterFunc(L, -3, "GetComponentInParent", _m_GetComponentInParent);
		Utils.RegisterFunc(L, -3, "GetComponents", _m_GetComponents);
		Utils.RegisterFunc(L, -3, "GetComponentsInChildren", _m_GetComponentsInChildren);
		Utils.RegisterFunc(L, -3, "GetComponentsInParent", _m_GetComponentsInParent);
		Utils.RegisterFunc(L, -3, "TryGetComponent", _m_TryGetComponent);
		Utils.RegisterFunc(L, -3, "SendMessageUpwards", _m_SendMessageUpwards);
		Utils.RegisterFunc(L, -3, "SendMessage", _m_SendMessage);
		Utils.RegisterFunc(L, -3, "BroadcastMessage", _m_BroadcastMessage);
		Utils.RegisterFunc(L, -3, "AddComponent", _m_AddComponent);
		Utils.RegisterFunc(L, -3, "SetActive", _m_SetActive);
		Utils.RegisterFunc(L, -3, "CompareTag", _m_CompareTag);
		Utils.RegisterFunc(L, -3, "GameObjectCreatePool", _m_GameObjectCreatePool);
		Utils.RegisterFunc(L, -3, "GameObjectSpawn", _m_GameObjectSpawn);
		Utils.RegisterFunc(L, -3, "GameObjectRecycle", _m_GameObjectRecycle);
		Utils.RegisterFunc(L, -3, "GameObjectRecycleAll", _m_GameObjectRecycleAll);
		Utils.RegisterFunc(L, -3, "GameObjectDestroyAll", _m_GameObjectDestroyAll);
		Utils.RegisterFunc(L, -3, "CountPooled", _m_CountPooled);
		Utils.RegisterFunc(L, -3, "GetRootParent", _m_GetRootParent);
		Utils.RegisterFunc(L, -3, "GetOrAddComponent", _m_GetOrAddComponent);
		Utils.RegisterFunc(L, -3, "Instantiate", _m_Instantiate);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "InScene", _m_InScene);
		Utils.RegisterFunc(L, -3, "SetLayerRecursively", _m_SetLayerRecursively);
		Utils.RegisterFunc(L, -3, "DestroyEx", _m_DestroyEx);
		Utils.RegisterFunc(L, -3, "GetComponent_RectTransform", _m_GetComponent_RectTransform);
		Utils.RegisterFunc(L, -3, "GetComponent_Text", _m_GetComponent_Text);
		Utils.RegisterFunc(L, -3, "GetComponent_Image", _m_GetComponent_Image);
		Utils.RegisterFunc(L, -3, "GetComponent_Button", _m_GetComponent_Button);
		Utils.RegisterFunc(L, -3, "TryActive", _m_TryActive);
		Utils.RegisterFunc(L, -2, "transform", _g_get_transform);
		Utils.RegisterFunc(L, -2, "layer", _g_get_layer);
		Utils.RegisterFunc(L, -2, "activeSelf", _g_get_activeSelf);
		Utils.RegisterFunc(L, -2, "activeInHierarchy", _g_get_activeInHierarchy);
		Utils.RegisterFunc(L, -2, "isStatic", _g_get_isStatic);
		Utils.RegisterFunc(L, -2, "tag", _g_get_tag);
		Utils.RegisterFunc(L, -2, "scene", _g_get_scene);
		Utils.RegisterFunc(L, -2, "sceneCullingMask", _g_get_sceneCullingMask);
		Utils.RegisterFunc(L, -2, "gameObject", _g_get_gameObject);
		Utils.RegisterFunc(L, -1, "layer", _s_set_layer);
		Utils.RegisterFunc(L, -1, "isStatic", _s_set_isStatic);
		Utils.RegisterFunc(L, -1, "tag", _s_set_tag);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 6, 0, 0);
		Utils.RegisterFunc(L, -4, "CreatePrimitive", _m_CreatePrimitive_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindWithTag", _m_FindWithTag_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindGameObjectWithTag", _m_FindGameObjectWithTag_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindGameObjectsWithTag", _m_FindGameObjectsWithTag_xlua_st_);
		Utils.RegisterFunc(L, -4, "Find", _m_Find_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				GameObject o = new GameObject(Lua.lua_tostring(L, 2));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				GameObject o2 = new GameObject();
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (Lua.lua_gettop(L) >= 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 3) || objectTranslator.Assignable<Type>(L, 3)))
			{
				string name = Lua.lua_tostring(L, 2);
				Type[] components = objectTranslator.GetParams<Type>(L, 3);
				GameObject o3 = new GameObject(name, components);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreatePrimitive_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out PrimitiveType v);
			GameObject o = GameObject.CreatePrimitive(v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component component = gameObject.GetComponent(type);
				objectTranslator.Push(L, component);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string type2 = Lua.lua_tostring(L, 2);
				Component component2 = gameObject.GetComponent(type2);
				objectTranslator.Push(L, component2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetComponent!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponentInChildren(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component componentInChildren = gameObject.GetComponentInChildren(type);
				objectTranslator.Push(L, componentInChildren);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Type type2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				bool includeInactive = Lua.lua_toboolean(L, 3);
				Component componentInChildren2 = gameObject.GetComponentInChildren(type2, includeInactive);
				objectTranslator.Push(L, componentInChildren2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetComponentInChildren!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponentInParent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
			Component componentInParent = gameObject.GetComponentInParent(type);
			objectTranslator.Push(L, componentInParent);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component[] components = gameObject.GetComponents(type);
				objectTranslator.Push(L, components);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 2) && objectTranslator.Assignable<List<Component>>(L, 3))
			{
				Type type2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				List<Component> results = (List<Component>)objectTranslator.GetObject(L, 3, typeof(List<Component>));
				gameObject.GetComponents(type2, results);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetComponents!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponentsInChildren(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component[] componentsInChildren = gameObject.GetComponentsInChildren(type);
				objectTranslator.Push(L, componentsInChildren);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Type type2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				bool includeInactive = Lua.lua_toboolean(L, 3);
				Component[] componentsInChildren2 = gameObject.GetComponentsInChildren(type2, includeInactive);
				objectTranslator.Push(L, componentsInChildren2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetComponentsInChildren!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponentsInParent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component[] componentsInParent = gameObject.GetComponentsInParent(type);
				objectTranslator.Push(L, componentsInParent);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Type type2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				bool includeInactive = Lua.lua_toboolean(L, 3);
				Component[] componentsInParent2 = gameObject.GetComponentsInParent(type2, includeInactive);
				objectTranslator.Push(L, componentsInParent2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetComponentsInParent!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
			Component component;
			bool value = gameObject.TryGetComponent(type, out component);
			Lua.lua_pushboolean(L, value);
			objectTranslator.Push(L, component);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindWithTag_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject o = GameObject.FindWithTag(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendMessageUpwards(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string methodName = Lua.lua_tostring(L, 2);
				gameObject.SendMessageUpwards(methodName);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<SendMessageOptions>(L, 3))
			{
				string methodName2 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out SendMessageOptions v);
				gameObject.SendMessageUpwards(methodName2, v);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3))
			{
				string methodName3 = Lua.lua_tostring(L, 2);
				object value = objectTranslator.GetObject(L, 3, typeof(object));
				gameObject.SendMessageUpwards(methodName3, value);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<SendMessageOptions>(L, 4))
			{
				string methodName4 = Lua.lua_tostring(L, 2);
				object value2 = objectTranslator.GetObject(L, 3, typeof(object));
				objectTranslator.Get(L, 4, out SendMessageOptions v2);
				gameObject.SendMessageUpwards(methodName4, value2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject.SendMessageUpwards!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendMessage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string methodName = Lua.lua_tostring(L, 2);
				gameObject.SendMessage(methodName);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<SendMessageOptions>(L, 3))
			{
				string methodName2 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out SendMessageOptions v);
				gameObject.SendMessage(methodName2, v);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3))
			{
				string methodName3 = Lua.lua_tostring(L, 2);
				object value = objectTranslator.GetObject(L, 3, typeof(object));
				gameObject.SendMessage(methodName3, value);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<SendMessageOptions>(L, 4))
			{
				string methodName4 = Lua.lua_tostring(L, 2);
				object value2 = objectTranslator.GetObject(L, 3, typeof(object));
				objectTranslator.Get(L, 4, out SendMessageOptions v2);
				gameObject.SendMessage(methodName4, value2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject.SendMessage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BroadcastMessage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string methodName = Lua.lua_tostring(L, 2);
				gameObject.BroadcastMessage(methodName);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<SendMessageOptions>(L, 3))
			{
				string methodName2 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out SendMessageOptions v);
				gameObject.BroadcastMessage(methodName2, v);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3))
			{
				string methodName3 = Lua.lua_tostring(L, 2);
				object parameter = objectTranslator.GetObject(L, 3, typeof(object));
				gameObject.BroadcastMessage(methodName3, parameter);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<SendMessageOptions>(L, 4))
			{
				string methodName4 = Lua.lua_tostring(L, 2);
				object parameter2 = objectTranslator.GetObject(L, 3, typeof(object));
				objectTranslator.Get(L, 4, out SendMessageOptions v2);
				gameObject.BroadcastMessage(methodName4, parameter2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject.BroadcastMessage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			Type componentType = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
			Component o = gameObject.AddComponent(componentType);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetActive(IntPtr L)
	{
		try
		{
			GameObject obj = (GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool active = Lua.lua_toboolean(L, 2);
			obj.SetActive(active);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CompareTag(IntPtr L)
	{
		try
		{
			GameObject obj = (GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string tag = Lua.lua_tostring(L, 2);
			bool value = obj.CompareTag(tag);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindGameObjectWithTag_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject o = GameObject.FindGameObjectWithTag(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindGameObjectsWithTag_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject[] o = GameObject.FindGameObjectsWithTag(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Find_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject o = GameObject.Find(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GameObjectCreatePool(IntPtr L)
	{
		try
		{
			((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GameObjectCreatePool();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GameObjectSpawn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject prefab = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				GameObject o2 = prefab.GameObjectSpawn();
				objectTranslator.Push(L, o2);
				return 1;
			}
			case 2:
				if (objectTranslator.Assignable<Transform>(L, 2))
				{
					Transform parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
					GameObject o = prefab.GameObjectSpawn(parent);
					objectTranslator.Push(L, o);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GameObjectSpawn!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GameObjectRecycle(IntPtr L)
	{
		try
		{
			((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GameObjectRecycle();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GameObjectRecycleAll(IntPtr L)
	{
		try
		{
			((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GameObjectRecycleAll();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GameObjectDestroyAll(IntPtr L)
	{
		try
		{
			((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GameObjectDestroyAll();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CountPooled(IntPtr L)
	{
		try
		{
			int value = ((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CountPooled();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRootParent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject rootParent = ((GameObject)objectTranslator.FastGetCSObj(L, 1)).GetRootParent();
			objectTranslator.Push(L, rootParent);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOrAddComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject go = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Type>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				bool set_enable = Lua.lua_toboolean(L, 3);
				Component orAddComponent = go.GetOrAddComponent(type, set_enable);
				objectTranslator.Push(L, orAddComponent);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type type2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component orAddComponent2 = go.GetOrAddComponent(type2);
				objectTranslator.Push(L, orAddComponent2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.GameObject.GetOrAddComponent!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Instantiate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject o = ((GameObject)objectTranslator.FastGetCSObj(L, 1)).Instantiate();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InScene(IntPtr L)
	{
		try
		{
			bool value = ((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InScene();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLayerRecursively(IntPtr L)
	{
		try
		{
			GameObject gameObject = (GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int layer = Lua.xlua_tointeger(L, 2);
			gameObject.SetLayerRecursively(layer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyEx(IntPtr L)
	{
		try
		{
			((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DestroyEx();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponent_RectTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component_RectTransform = ((GameObject)objectTranslator.FastGetCSObj(L, 1)).GetComponent_RectTransform();
			objectTranslator.Push(L, component_RectTransform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponent_Text(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component_Text = ((GameObject)objectTranslator.FastGetCSObj(L, 1)).GetComponent_Text();
			objectTranslator.Push(L, component_Text);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponent_Image(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component_Image = ((GameObject)objectTranslator.FastGetCSObj(L, 1)).GetComponent_Image();
			objectTranslator.Push(L, component_Image);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponent_Button(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component_Button = ((GameObject)objectTranslator.FastGetCSObj(L, 1)).GetComponent_Button();
			objectTranslator.Push(L, component_Button);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryActive(IntPtr L)
	{
		try
		{
			GameObject obj = (GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool active = Lua.lua_toboolean(L, 2);
			obj.TryActive(active);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_transform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gameObject.transform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_layer(IntPtr L)
	{
		try
		{
			GameObject gameObject = (GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, gameObject.layer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_activeSelf(IntPtr L)
	{
		try
		{
			GameObject gameObject = (GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, gameObject.activeSelf);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_activeInHierarchy(IntPtr L)
	{
		try
		{
			GameObject gameObject = (GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, gameObject.activeInHierarchy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isStatic(IntPtr L)
	{
		try
		{
			GameObject gameObject = (GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, gameObject.isStatic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tag(IntPtr L)
	{
		try
		{
			GameObject gameObject = (GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gameObject.tag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scene(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gameObject.scene);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sceneCullingMask(IntPtr L)
	{
		try
		{
			GameObject gameObject = (GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, gameObject.sceneCullingMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gameObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = (GameObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gameObject.gameObject);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_layer(IntPtr L)
	{
		try
		{
			((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).layer = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isStatic(IntPtr L)
	{
		try
		{
			((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isStatic = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tag(IntPtr L)
	{
		try
		{
			((GameObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).tag = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
