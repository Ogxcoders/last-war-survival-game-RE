using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SystemCollectionsGenericList_1Enumerator_UnityEngineRenderingUniversalScriptableRendererFeature_Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>.Enumerator);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 1, 0);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "MoveNext", _m_MoveNext);
		Utils.RegisterFunc(L, -2, "Current", _g_get_Current);
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
				objectTranslator.Push(L, default(List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>.Enumerator));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to System.Collections.Generic.List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>.Enumerator constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>.Enumerator v);
			v.Dispose();
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveNext(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>.Enumerator v);
			bool value = v.MoveNext();
			Lua.lua_pushboolean(L, value);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Current(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out List<UnityEngine.Rendering.Universal.ScriptableRendererFeature>.Enumerator v);
			objectTranslator.Push(L, v.Current);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
