using System;
using UnityEngine;
using UnityEngine.U2D;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineU2DSpriteAtlasWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SpriteAtlas);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 3, 0);
		Utils.RegisterFunc(L, -3, "CanBindTo", _m_CanBindTo);
		Utils.RegisterFunc(L, -3, "GetSprite", _m_GetSprite);
		Utils.RegisterFunc(L, -3, "GetSprites", _m_GetSprites);
		Utils.RegisterFunc(L, -2, "isVariant", _g_get_isVariant);
		Utils.RegisterFunc(L, -2, "tag", _g_get_tag);
		Utils.RegisterFunc(L, -2, "spriteCount", _g_get_spriteCount);
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
				SpriteAtlas o = new SpriteAtlas();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.U2D.SpriteAtlas constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanBindTo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteAtlas spriteAtlas = (SpriteAtlas)objectTranslator.FastGetCSObj(L, 1);
			Sprite sprite = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
			bool value = spriteAtlas.CanBindTo(sprite);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteAtlas obj = (SpriteAtlas)objectTranslator.FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			Sprite sprite = obj.GetSprite(name);
			objectTranslator.Push(L, sprite);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSprites(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteAtlas spriteAtlas = (SpriteAtlas)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Sprite[]>(L, 2))
			{
				Sprite[] sprites = (Sprite[])objectTranslator.GetObject(L, 2, typeof(Sprite[]));
				int sprites2 = spriteAtlas.GetSprites(sprites);
				Lua.xlua_pushinteger(L, sprites2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Sprite[]>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				Sprite[] sprites3 = (Sprite[])objectTranslator.GetObject(L, 2, typeof(Sprite[]));
				string name = Lua.lua_tostring(L, 3);
				int sprites4 = spriteAtlas.GetSprites(sprites3, name);
				Lua.xlua_pushinteger(L, sprites4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.U2D.SpriteAtlas.GetSprites!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isVariant(IntPtr L)
	{
		try
		{
			SpriteAtlas spriteAtlas = (SpriteAtlas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, spriteAtlas.isVariant);
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
			SpriteAtlas spriteAtlas = (SpriteAtlas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, spriteAtlas.tag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteCount(IntPtr L)
	{
		try
		{
			SpriteAtlas spriteAtlas = (SpriteAtlas)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, spriteAtlas.spriteCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
