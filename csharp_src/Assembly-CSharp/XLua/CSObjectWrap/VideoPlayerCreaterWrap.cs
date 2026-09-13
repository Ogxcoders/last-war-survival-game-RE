using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VideoPlayerCreaterWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(VideoPlayerCreater);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 1, 1);
		Utils.RegisterFunc(L, -3, "Start", _m_Start);
		Utils.RegisterFunc(L, -3, "OnDestroy", _m_OnDestroy);
		Utils.RegisterFunc(L, -2, "mCurentVideoPath", _g_get_mCurentVideoPath);
		Utils.RegisterFunc(L, -1, "mCurentVideoPath", _s_set_mCurentVideoPath);
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
				VideoPlayerCreater o = new VideoPlayerCreater();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to VideoPlayerCreater constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Start(IntPtr L)
	{
		try
		{
			((VideoPlayerCreater)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Start();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDestroy(IntPtr L)
	{
		try
		{
			((VideoPlayerCreater)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDestroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mCurentVideoPath(IntPtr L)
	{
		try
		{
			VideoPlayerCreater videoPlayerCreater = (VideoPlayerCreater)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, videoPlayerCreater.mCurentVideoPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mCurentVideoPath(IntPtr L)
	{
		try
		{
			((VideoPlayerCreater)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mCurentVideoPath = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
