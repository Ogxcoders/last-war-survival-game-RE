using System;
using System.Collections.Generic;
using DG.Tweening;
using GameKit.Base;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineComponentWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Component);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 24, 3, 1);
		Utils.RegisterFunc(L, -3, "GetComponent", _m_GetComponent);
		Utils.RegisterFunc(L, -3, "TryGetComponent", _m_TryGetComponent);
		Utils.RegisterFunc(L, -3, "GetComponentInChildren", _m_GetComponentInChildren);
		Utils.RegisterFunc(L, -3, "GetComponentsInChildren", _m_GetComponentsInChildren);
		Utils.RegisterFunc(L, -3, "GetComponentInParent", _m_GetComponentInParent);
		Utils.RegisterFunc(L, -3, "GetComponentsInParent", _m_GetComponentsInParent);
		Utils.RegisterFunc(L, -3, "GetComponents", _m_GetComponents);
		Utils.RegisterFunc(L, -3, "CompareTag", _m_CompareTag);
		Utils.RegisterFunc(L, -3, "SendMessageUpwards", _m_SendMessageUpwards);
		Utils.RegisterFunc(L, -3, "SendMessage", _m_SendMessage);
		Utils.RegisterFunc(L, -3, "BroadcastMessage", _m_BroadcastMessage);
		Utils.RegisterFunc(L, -3, "DOComplete", _m_DOComplete);
		Utils.RegisterFunc(L, -3, "DOKill", _m_DOKill);
		Utils.RegisterFunc(L, -3, "DOFlip", _m_DOFlip);
		Utils.RegisterFunc(L, -3, "DOGoto", _m_DOGoto);
		Utils.RegisterFunc(L, -3, "DOPause", _m_DOPause);
		Utils.RegisterFunc(L, -3, "DOPlay", _m_DOPlay);
		Utils.RegisterFunc(L, -3, "DOPlayBackwards", _m_DOPlayBackwards);
		Utils.RegisterFunc(L, -3, "DOPlayForward", _m_DOPlayForward);
		Utils.RegisterFunc(L, -3, "DORestart", _m_DORestart);
		Utils.RegisterFunc(L, -3, "DORewind", _m_DORewind);
		Utils.RegisterFunc(L, -3, "DOSmoothRewind", _m_DOSmoothRewind);
		Utils.RegisterFunc(L, -3, "DOTogglePause", _m_DOTogglePause);
		Utils.RegisterFunc(L, -3, "GetOrAddComponent", _m_GetOrAddComponent);
		Utils.RegisterFunc(L, -2, "transform", _g_get_transform);
		Utils.RegisterFunc(L, -2, "gameObject", _g_get_gameObject);
		Utils.RegisterFunc(L, -2, "tag", _g_get_tag);
		Utils.RegisterFunc(L, -1, "tag", _s_set_tag);
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
				Component o = new Component();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component component2 = component.GetComponent(type);
				objectTranslator.Push(L, component2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string type2 = Lua.lua_tostring(L, 2);
				Component component3 = component.GetComponent(type2);
				objectTranslator.Push(L, component3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.GetComponent!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
			Component component2;
			bool value = component.TryGetComponent(type, out component2);
			Lua.lua_pushboolean(L, value);
			objectTranslator.Push(L, component2);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponentInChildren(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type t = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component componentInChildren = component.GetComponentInChildren(t);
				objectTranslator.Push(L, componentInChildren);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Type t2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				bool includeInactive = Lua.lua_toboolean(L, 3);
				Component componentInChildren2 = component.GetComponentInChildren(t2, includeInactive);
				objectTranslator.Push(L, componentInChildren2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.GetComponentInChildren!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponentsInChildren(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type t = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component[] componentsInChildren = component.GetComponentsInChildren(t);
				objectTranslator.Push(L, componentsInChildren);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Type t2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				bool includeInactive = Lua.lua_toboolean(L, 3);
				Component[] componentsInChildren2 = component.GetComponentsInChildren(t2, includeInactive);
				objectTranslator.Push(L, componentsInChildren2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.GetComponentsInChildren!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponentInParent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			Type t = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
			Component componentInParent = component.GetComponentInParent(t);
			objectTranslator.Push(L, componentInParent);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponentsInParent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type t = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component[] componentsInParent = component.GetComponentsInParent(t);
				objectTranslator.Push(L, componentsInParent);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Type t2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				bool includeInactive = Lua.lua_toboolean(L, 3);
				Component[] componentsInParent2 = component.GetComponentsInParent(t2, includeInactive);
				objectTranslator.Push(L, componentsInParent2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.GetComponentsInParent!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component[] components = component.GetComponents(type);
				objectTranslator.Push(L, components);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Type>(L, 2) && objectTranslator.Assignable<List<Component>>(L, 3))
			{
				Type type2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				List<Component> results = (List<Component>)objectTranslator.GetObject(L, 3, typeof(List<Component>));
				component.GetComponents(type2, results);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.GetComponents!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CompareTag(IntPtr L)
	{
		try
		{
			Component obj = (Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_SendMessageUpwards(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string methodName = Lua.lua_tostring(L, 2);
				component.SendMessageUpwards(methodName);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3))
			{
				string methodName2 = Lua.lua_tostring(L, 2);
				object value = objectTranslator.GetObject(L, 3, typeof(object));
				component.SendMessageUpwards(methodName2, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<SendMessageOptions>(L, 3))
			{
				string methodName3 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out SendMessageOptions v);
				component.SendMessageUpwards(methodName3, v);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<SendMessageOptions>(L, 4))
			{
				string methodName4 = Lua.lua_tostring(L, 2);
				object value2 = objectTranslator.GetObject(L, 3, typeof(object));
				objectTranslator.Get(L, 4, out SendMessageOptions v2);
				component.SendMessageUpwards(methodName4, value2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.SendMessageUpwards!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendMessage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string methodName = Lua.lua_tostring(L, 2);
				component.SendMessage(methodName);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3))
			{
				string methodName2 = Lua.lua_tostring(L, 2);
				object value = objectTranslator.GetObject(L, 3, typeof(object));
				component.SendMessage(methodName2, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<SendMessageOptions>(L, 3))
			{
				string methodName3 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out SendMessageOptions v);
				component.SendMessage(methodName3, v);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<SendMessageOptions>(L, 4))
			{
				string methodName4 = Lua.lua_tostring(L, 2);
				object value2 = objectTranslator.GetObject(L, 3, typeof(object));
				objectTranslator.Get(L, 4, out SendMessageOptions v2);
				component.SendMessage(methodName4, value2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.SendMessage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BroadcastMessage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string methodName = Lua.lua_tostring(L, 2);
				component.BroadcastMessage(methodName);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3))
			{
				string methodName2 = Lua.lua_tostring(L, 2);
				object parameter = objectTranslator.GetObject(L, 3, typeof(object));
				component.BroadcastMessage(methodName2, parameter);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<SendMessageOptions>(L, 3))
			{
				string methodName3 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out SendMessageOptions v);
				component.BroadcastMessage(methodName3, v);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<SendMessageOptions>(L, 4))
			{
				string methodName4 = Lua.lua_tostring(L, 2);
				object parameter2 = objectTranslator.GetObject(L, 3, typeof(object));
				objectTranslator.Get(L, 4, out SendMessageOptions v2);
				component.BroadcastMessage(methodName4, parameter2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.BroadcastMessage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOComplete(IntPtr L)
	{
		try
		{
			Component target = (Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool withCallbacks = Lua.lua_toboolean(L, 2);
				int value = target.DOComplete(withCallbacks);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1)
			{
				int value2 = target.DOComplete();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.DOComplete!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOKill(IntPtr L)
	{
		try
		{
			Component target = (Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool complete = Lua.lua_toboolean(L, 2);
				int value = target.DOKill(complete);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1)
			{
				int value2 = target.DOKill();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.DOKill!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOFlip(IntPtr L)
	{
		try
		{
			int value = ((Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOFlip();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOGoto(IntPtr L)
	{
		try
		{
			Component target = (Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				float to = (float)Lua.lua_tonumber(L, 2);
				bool andPlay = Lua.lua_toboolean(L, 3);
				int value = target.DOGoto(to, andPlay);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float to2 = (float)Lua.lua_tonumber(L, 2);
				int value2 = target.DOGoto(to2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.DOGoto!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPause(IntPtr L)
	{
		try
		{
			int value = ((Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPause();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlay(IntPtr L)
	{
		try
		{
			int value = ((Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPlay();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayBackwards(IntPtr L)
	{
		try
		{
			int value = ((Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPlayBackwards();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayForward(IntPtr L)
	{
		try
		{
			int value = ((Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPlayForward();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORestart(IntPtr L)
	{
		try
		{
			Component target = (Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool includeDelay = Lua.lua_toboolean(L, 2);
				int value = target.DORestart(includeDelay);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1)
			{
				int value2 = target.DORestart();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.DORestart!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORewind(IntPtr L)
	{
		try
		{
			Component target = (Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool includeDelay = Lua.lua_toboolean(L, 2);
				int value = target.DORewind(includeDelay);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 1)
			{
				int value2 = target.DORewind();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.DORewind!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOSmoothRewind(IntPtr L)
	{
		try
		{
			int value = ((Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOSmoothRewind();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOTogglePause(IntPtr L)
	{
		try
		{
			int value = ((Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOTogglePause();
			Lua.xlua_pushinteger(L, value);
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
			Component comp = (Component)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Type>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Type type = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				bool set_enable = Lua.lua_toboolean(L, 3);
				Component orAddComponent = comp.GetOrAddComponent(type, set_enable);
				objectTranslator.Push(L, orAddComponent);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Type>(L, 2))
			{
				Type type2 = (Type)objectTranslator.GetObject(L, 2, typeof(Type));
				Component orAddComponent2 = comp.GetOrAddComponent(type2);
				objectTranslator.Push(L, orAddComponent2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Component.GetOrAddComponent!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_transform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, component.transform);
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
			Component component = (Component)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, component.gameObject);
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
			Component component = (Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, component.tag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tag(IntPtr L)
	{
		try
		{
			((Component)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).tag = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
