using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_SpriteAssetWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TMP_SpriteAsset);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 8, 3);
		Utils.RegisterFunc(L, -3, "UpdateLookupTables", _m_UpdateLookupTables);
		Utils.RegisterFunc(L, -3, "GetSpriteIndexFromHashcode", _m_GetSpriteIndexFromHashcode);
		Utils.RegisterFunc(L, -3, "GetSpriteIndexFromUnicode", _m_GetSpriteIndexFromUnicode);
		Utils.RegisterFunc(L, -3, "GetSpriteIndexFromName", _m_GetSpriteIndexFromName);
		Utils.RegisterFunc(L, -3, "SortGlyphTable", _m_SortGlyphTable);
		Utils.RegisterFunc(L, -2, "version", _g_get_version);
		Utils.RegisterFunc(L, -2, "faceInfo", _g_get_faceInfo);
		Utils.RegisterFunc(L, -2, "spriteCharacterTable", _g_get_spriteCharacterTable);
		Utils.RegisterFunc(L, -2, "spriteCharacterLookupTable", _g_get_spriteCharacterLookupTable);
		Utils.RegisterFunc(L, -2, "spriteGlyphTable", _g_get_spriteGlyphTable);
		Utils.RegisterFunc(L, -2, "spriteSheet", _g_get_spriteSheet);
		Utils.RegisterFunc(L, -2, "spriteInfoList", _g_get_spriteInfoList);
		Utils.RegisterFunc(L, -2, "fallbackSpriteAssets", _g_get_fallbackSpriteAssets);
		Utils.RegisterFunc(L, -1, "spriteSheet", _s_set_spriteSheet);
		Utils.RegisterFunc(L, -1, "spriteInfoList", _s_set_spriteInfoList);
		Utils.RegisterFunc(L, -1, "fallbackSpriteAssets", _s_set_fallbackSpriteAssets);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "SearchForSpriteByUnicode", _m_SearchForSpriteByUnicode_xlua_st_);
		Utils.RegisterFunc(L, -4, "SearchForSpriteByHashCode", _m_SearchForSpriteByHashCode_xlua_st_);
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
				TMP_SpriteAsset o = new TMP_SpriteAsset();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_SpriteAsset constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateLookupTables(IntPtr L)
	{
		try
		{
			((TMP_SpriteAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateLookupTables();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSpriteIndexFromHashcode(IntPtr L)
	{
		try
		{
			TMP_SpriteAsset obj = (TMP_SpriteAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int hashCode = Lua.xlua_tointeger(L, 2);
			int spriteIndexFromHashcode = obj.GetSpriteIndexFromHashcode(hashCode);
			Lua.xlua_pushinteger(L, spriteIndexFromHashcode);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSpriteIndexFromUnicode(IntPtr L)
	{
		try
		{
			TMP_SpriteAsset obj = (TMP_SpriteAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			uint unicode = Lua.xlua_touint(L, 2);
			int spriteIndexFromUnicode = obj.GetSpriteIndexFromUnicode(unicode);
			Lua.xlua_pushinteger(L, spriteIndexFromUnicode);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSpriteIndexFromName(IntPtr L)
	{
		try
		{
			TMP_SpriteAsset obj = (TMP_SpriteAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			int spriteIndexFromName = obj.GetSpriteIndexFromName(name);
			Lua.xlua_pushinteger(L, spriteIndexFromName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SearchForSpriteByUnicode_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_SpriteAsset spriteAsset = (TMP_SpriteAsset)objectTranslator.GetObject(L, 1, typeof(TMP_SpriteAsset));
			uint unicode = Lua.xlua_touint(L, 2);
			bool includeFallbacks = Lua.lua_toboolean(L, 3);
			int spriteIndex;
			TMP_SpriteAsset o = TMP_SpriteAsset.SearchForSpriteByUnicode(spriteAsset, unicode, includeFallbacks, out spriteIndex);
			objectTranslator.Push(L, o);
			Lua.xlua_pushinteger(L, spriteIndex);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SearchForSpriteByHashCode_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_SpriteAsset spriteAsset = (TMP_SpriteAsset)objectTranslator.GetObject(L, 1, typeof(TMP_SpriteAsset));
			int hashCode = Lua.xlua_tointeger(L, 2);
			bool includeFallbacks = Lua.lua_toboolean(L, 3);
			int spriteIndex;
			TMP_SpriteAsset o = TMP_SpriteAsset.SearchForSpriteByHashCode(spriteAsset, hashCode, includeFallbacks, out spriteIndex);
			objectTranslator.Push(L, o);
			Lua.xlua_pushinteger(L, spriteIndex);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SortGlyphTable(IntPtr L)
	{
		try
		{
			((TMP_SpriteAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SortGlyphTable();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_version(IntPtr L)
	{
		try
		{
			TMP_SpriteAsset tMP_SpriteAsset = (TMP_SpriteAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, tMP_SpriteAsset.version);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_faceInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_SpriteAsset tMP_SpriteAsset = (TMP_SpriteAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_SpriteAsset.faceInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteCharacterTable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_SpriteAsset tMP_SpriteAsset = (TMP_SpriteAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_SpriteAsset.spriteCharacterTable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteCharacterLookupTable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_SpriteAsset tMP_SpriteAsset = (TMP_SpriteAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_SpriteAsset.spriteCharacterLookupTable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteGlyphTable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_SpriteAsset tMP_SpriteAsset = (TMP_SpriteAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_SpriteAsset.spriteGlyphTable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteSheet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_SpriteAsset tMP_SpriteAsset = (TMP_SpriteAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_SpriteAsset.spriteSheet);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteInfoList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_SpriteAsset tMP_SpriteAsset = (TMP_SpriteAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_SpriteAsset.spriteInfoList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fallbackSpriteAssets(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_SpriteAsset tMP_SpriteAsset = (TMP_SpriteAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_SpriteAsset.fallbackSpriteAssets);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spriteSheet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_SpriteAsset)objectTranslator.FastGetCSObj(L, 1)).spriteSheet = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spriteInfoList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_SpriteAsset)objectTranslator.FastGetCSObj(L, 1)).spriteInfoList = (List<TMP_Sprite>)objectTranslator.GetObject(L, 2, typeof(List<TMP_Sprite>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fallbackSpriteAssets(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_SpriteAsset)objectTranslator.FastGetCSObj(L, 1)).fallbackSpriteAssets = (List<TMP_SpriteAsset>)objectTranslator.GetObject(L, 2, typeof(List<TMP_SpriteAsset>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
