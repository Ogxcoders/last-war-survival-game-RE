using System;
using System.Collections;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineMonoBehaviourWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MonoBehaviour);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 1, 1);
		Utils.RegisterFunc(L, -3, "IsInvoking", _m_IsInvoking);
		Utils.RegisterFunc(L, -3, "CancelInvoke", _m_CancelInvoke);
		Utils.RegisterFunc(L, -3, "Invoke", _m_Invoke);
		Utils.RegisterFunc(L, -3, "InvokeRepeating", _m_InvokeRepeating);
		Utils.RegisterFunc(L, -3, "StartCoroutine", _m_StartCoroutine);
		Utils.RegisterFunc(L, -3, "StopCoroutine", _m_StopCoroutine);
		Utils.RegisterFunc(L, -3, "StopAllCoroutines", _m_StopAllCoroutines);
		Utils.RegisterFunc(L, -2, "useGUILayout", _g_get_useGUILayout);
		Utils.RegisterFunc(L, -1, "useGUILayout", _s_set_useGUILayout);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "print", _m_print_xlua_st_);
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
				MonoBehaviour o = new MonoBehaviour();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MonoBehaviour constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInvoking(IntPtr L)
	{
		try
		{
			MonoBehaviour monoBehaviour = (MonoBehaviour)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				bool value2 = monoBehaviour.IsInvoking();
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string methodName = Lua.lua_tostring(L, 2);
					bool value = monoBehaviour.IsInvoking(methodName);
					Lua.lua_pushboolean(L, value);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MonoBehaviour.IsInvoking!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CancelInvoke(IntPtr L)
	{
		try
		{
			MonoBehaviour monoBehaviour = (MonoBehaviour)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				monoBehaviour.CancelInvoke();
				return 0;
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string methodName = Lua.lua_tostring(L, 2);
					monoBehaviour.CancelInvoke(methodName);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MonoBehaviour.CancelInvoke!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Invoke(IntPtr L)
	{
		try
		{
			MonoBehaviour obj = (MonoBehaviour)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string methodName = Lua.lua_tostring(L, 2);
			float time = (float)Lua.lua_tonumber(L, 3);
			obj.Invoke(methodName, time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InvokeRepeating(IntPtr L)
	{
		try
		{
			MonoBehaviour obj = (MonoBehaviour)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string methodName = Lua.lua_tostring(L, 2);
			float time = (float)Lua.lua_tonumber(L, 3);
			float repeatRate = (float)Lua.lua_tonumber(L, 4);
			obj.InvokeRepeating(methodName, time, repeatRate);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartCoroutine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MonoBehaviour monoBehaviour = (MonoBehaviour)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string methodName = Lua.lua_tostring(L, 2);
				Coroutine o = monoBehaviour.StartCoroutine(methodName);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<IEnumerator>(L, 2))
			{
				IEnumerator routine = (IEnumerator)objectTranslator.GetObject(L, 2, typeof(IEnumerator));
				Coroutine o2 = monoBehaviour.StartCoroutine(routine);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 3))
			{
				string methodName2 = Lua.lua_tostring(L, 2);
				object value = objectTranslator.GetObject(L, 3, typeof(object));
				Coroutine o3 = monoBehaviour.StartCoroutine(methodName2, value);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MonoBehaviour.StartCoroutine!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopCoroutine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MonoBehaviour monoBehaviour = (MonoBehaviour)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<IEnumerator>(L, 2))
			{
				IEnumerator routine = (IEnumerator)objectTranslator.GetObject(L, 2, typeof(IEnumerator));
				monoBehaviour.StopCoroutine(routine);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Coroutine>(L, 2))
			{
				Coroutine routine2 = (Coroutine)objectTranslator.GetObject(L, 2, typeof(Coroutine));
				monoBehaviour.StopCoroutine(routine2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string methodName = Lua.lua_tostring(L, 2);
				monoBehaviour.StopCoroutine(methodName);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MonoBehaviour.StopCoroutine!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopAllCoroutines(IntPtr L)
	{
		try
		{
			((MonoBehaviour)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopAllCoroutines();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_print_xlua_st_(IntPtr L)
	{
		try
		{
			MonoBehaviour.print(ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(object)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useGUILayout(IntPtr L)
	{
		try
		{
			MonoBehaviour monoBehaviour = (MonoBehaviour)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, monoBehaviour.useGUILayout);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useGUILayout(IntPtr L)
	{
		try
		{
			((MonoBehaviour)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useGUILayout = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
