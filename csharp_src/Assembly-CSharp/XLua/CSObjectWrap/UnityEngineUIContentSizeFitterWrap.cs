using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIContentSizeFitterWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ContentSizeFitter);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 2, 2);
		Utils.RegisterFunc(L, -3, "SetLayoutHorizontal", _m_SetLayoutHorizontal);
		Utils.RegisterFunc(L, -3, "SetLayoutVertical", _m_SetLayoutVertical);
		Utils.RegisterFunc(L, -2, "horizontalFit", _g_get_horizontalFit);
		Utils.RegisterFunc(L, -2, "verticalFit", _g_get_verticalFit);
		Utils.RegisterFunc(L, -1, "horizontalFit", _s_set_horizontalFit);
		Utils.RegisterFunc(L, -1, "verticalFit", _s_set_verticalFit);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.ContentSizeFitter does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLayoutHorizontal(IntPtr L)
	{
		try
		{
			((ContentSizeFitter)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutHorizontal();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLayoutVertical(IntPtr L)
	{
		try
		{
			((ContentSizeFitter)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetLayoutVertical();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_horizontalFit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ContentSizeFitter contentSizeFitter = (ContentSizeFitter)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIContentSizeFitterFitMode(L, contentSizeFitter.horizontalFit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_verticalFit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ContentSizeFitter contentSizeFitter = (ContentSizeFitter)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineUIContentSizeFitterFitMode(L, contentSizeFitter.verticalFit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_horizontalFit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ContentSizeFitter contentSizeFitter = (ContentSizeFitter)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ContentSizeFitter.FitMode val);
			contentSizeFitter.horizontalFit = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_verticalFit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ContentSizeFitter contentSizeFitter = (ContentSizeFitter)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ContentSizeFitter.FitMode val);
			contentSizeFitter.verticalFit = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
