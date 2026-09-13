using System;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityUIExtensionWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UnityUIExtension);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 1, 0);
		Utils.RegisterFunc(L, -4, "OverrideImageSpriteByDynamicAtlas", _m_OverrideImageSpriteByDynamicAtlas_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsSpriteSmallEnoughToFitInDynamicAtlas", _m_IsSpriteSmallEnoughToFitInDynamicAtlas_xlua_st_);
		Utils.RegisterFunc(L, -4, "Update", _m_Update_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "k_SpriteMaxTexelCountFitIntoDynamicAtlas", UnityUIExtension.k_SpriteMaxTexelCountFitIntoDynamicAtlas);
		Utils.RegisterFunc(L, -2, "EnableAsyncLoadImageCallbackCheck", _g_get_EnableAsyncLoadImageCallbackCheck);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityUIExtension does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverrideImageSpriteByDynamicAtlas_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Image image = (Image)objectTranslator.GetObject(L, 1, typeof(Image));
			Sprite sprite = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
			UnityUIExtension.OverrideImageSpriteByDynamicAtlas(image, sprite);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSpriteSmallEnoughToFitInDynamicAtlas_xlua_st_(IntPtr L)
	{
		try
		{
			int width = Lua.xlua_tointeger(L, 1);
			int height = Lua.xlua_tointeger(L, 2);
			bool value = UnityUIExtension.IsSpriteSmallEnoughToFitInDynamicAtlas(width, height);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update_xlua_st_(IntPtr L)
	{
		try
		{
			UnityUIExtension.Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EnableAsyncLoadImageCallbackCheck(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, UnityUIExtension.EnableAsyncLoadImageCallbackCheck);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
