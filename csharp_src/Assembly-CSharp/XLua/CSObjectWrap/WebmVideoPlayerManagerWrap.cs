using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WebmVideoPlayerManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WebmVideoPlayerManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 7, 7);
		Utils.RegisterFunc(L, -3, "LoadVideo", _m_LoadVideo);
		Utils.RegisterFunc(L, -3, "PuseVideo", _m_PuseVideo);
		Utils.RegisterFunc(L, -3, "StopVideo", _m_StopVideo);
		Utils.RegisterFunc(L, -3, "CreateVideoPlayer", _m_CreateVideoPlayer);
		Utils.RegisterFunc(L, -3, "TryCreateRT", _m_TryCreateRT);
		Utils.RegisterFunc(L, -3, "ClearRT", _m_ClearRT);
		Utils.RegisterFunc(L, -3, "OnVideoLoaded", _m_OnVideoLoaded);
		Utils.RegisterFunc(L, -2, "mVideoPlayer", _g_get_mVideoPlayer);
		Utils.RegisterFunc(L, -2, "mCurVideoPath", _g_get_mCurVideoPath);
		Utils.RegisterFunc(L, -2, "mRawImage", _g_get_mRawImage);
		Utils.RegisterFunc(L, -2, "mRT", _g_get_mRT);
		Utils.RegisterFunc(L, -2, "mRTSize", _g_get_mRTSize);
		Utils.RegisterFunc(L, -2, "mIsVideoLoaded", _g_get_mIsVideoLoaded);
		Utils.RegisterFunc(L, -2, "mVideoURL", _g_get_mVideoURL);
		Utils.RegisterFunc(L, -1, "mVideoPlayer", _s_set_mVideoPlayer);
		Utils.RegisterFunc(L, -1, "mCurVideoPath", _s_set_mCurVideoPath);
		Utils.RegisterFunc(L, -1, "mRawImage", _s_set_mRawImage);
		Utils.RegisterFunc(L, -1, "mRT", _s_set_mRT);
		Utils.RegisterFunc(L, -1, "mRTSize", _s_set_mRTSize);
		Utils.RegisterFunc(L, -1, "mIsVideoLoaded", _s_set_mIsVideoLoaded);
		Utils.RegisterFunc(L, -1, "mVideoURL", _s_set_mVideoURL);
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
				WebmVideoPlayerManager o = new WebmVideoPlayerManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WebmVideoPlayerManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadVideo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WebmVideoPlayerManager webmVideoPlayerManager = (WebmVideoPlayerManager)objectTranslator.FastGetCSObj(L, 1);
			RawImage rawImage = (RawImage)objectTranslator.GetObject(L, 2, typeof(RawImage));
			string videoPath = Lua.lua_tostring(L, 3);
			int width = Lua.xlua_tointeger(L, 4);
			int height = Lua.xlua_tointeger(L, 5);
			webmVideoPlayerManager.LoadVideo(rawImage, videoPath, width, height);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PuseVideo(IntPtr L)
	{
		try
		{
			WebmVideoPlayerManager obj = (WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string videoPath = Lua.lua_tostring(L, 2);
			obj.PuseVideo(videoPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopVideo(IntPtr L)
	{
		try
		{
			WebmVideoPlayerManager obj = (WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string videoPath = Lua.lua_tostring(L, 2);
			obj.StopVideo(videoPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateVideoPlayer(IntPtr L)
	{
		try
		{
			((WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateVideoPlayer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryCreateRT(IntPtr L)
	{
		try
		{
			((WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TryCreateRT();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearRT(IntPtr L)
	{
		try
		{
			((WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearRT();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnVideoLoaded(IntPtr L)
	{
		try
		{
			WebmVideoPlayerManager obj = (WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			obj.OnVideoLoaded(path);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mVideoPlayer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WebmVideoPlayerManager webmVideoPlayerManager = (WebmVideoPlayerManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, webmVideoPlayerManager.mVideoPlayer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mCurVideoPath(IntPtr L)
	{
		try
		{
			WebmVideoPlayerManager webmVideoPlayerManager = (WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, webmVideoPlayerManager.mCurVideoPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mRawImage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WebmVideoPlayerManager webmVideoPlayerManager = (WebmVideoPlayerManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, webmVideoPlayerManager.mRawImage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mRT(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WebmVideoPlayerManager webmVideoPlayerManager = (WebmVideoPlayerManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, webmVideoPlayerManager.mRT);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mRTSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WebmVideoPlayerManager webmVideoPlayerManager = (WebmVideoPlayerManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, webmVideoPlayerManager.mRTSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mIsVideoLoaded(IntPtr L)
	{
		try
		{
			WebmVideoPlayerManager webmVideoPlayerManager = (WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, webmVideoPlayerManager.mIsVideoLoaded);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mVideoURL(IntPtr L)
	{
		try
		{
			WebmVideoPlayerManager webmVideoPlayerManager = (WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, webmVideoPlayerManager.mVideoURL);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mVideoPlayer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WebmVideoPlayerManager)objectTranslator.FastGetCSObj(L, 1)).mVideoPlayer = (VideoPlayer)objectTranslator.GetObject(L, 2, typeof(VideoPlayer));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mCurVideoPath(IntPtr L)
	{
		try
		{
			((WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mCurVideoPath = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mRawImage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WebmVideoPlayerManager)objectTranslator.FastGetCSObj(L, 1)).mRawImage = (RawImage)objectTranslator.GetObject(L, 2, typeof(RawImage));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mRT(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WebmVideoPlayerManager)objectTranslator.FastGetCSObj(L, 1)).mRT = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mRTSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WebmVideoPlayerManager webmVideoPlayerManager = (WebmVideoPlayerManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			webmVideoPlayerManager.mRTSize = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mIsVideoLoaded(IntPtr L)
	{
		try
		{
			((WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mIsVideoLoaded = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mVideoURL(IntPtr L)
	{
		try
		{
			((WebmVideoPlayerManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mVideoURL = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
