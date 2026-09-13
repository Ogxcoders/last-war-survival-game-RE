using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SpriteMeshRendererWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SpriteMeshRenderer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 5, 5);
		Utils.RegisterFunc(L, -3, "LoadSprite", _m_LoadSprite);
		Utils.RegisterFunc(L, -3, "Set_color_a", _m_Set_color_a);
		Utils.RegisterFunc(L, -2, "sharedMaterial", _g_get_sharedMaterial);
		Utils.RegisterFunc(L, -2, "sortingLayerID", _g_get_sortingLayerID);
		Utils.RegisterFunc(L, -2, "sortingOrder", _g_get_sortingOrder);
		Utils.RegisterFunc(L, -2, "color", _g_get_color);
		Utils.RegisterFunc(L, -2, "sprite", _g_get_sprite);
		Utils.RegisterFunc(L, -1, "sharedMaterial", _s_set_sharedMaterial);
		Utils.RegisterFunc(L, -1, "sortingLayerID", _s_set_sortingLayerID);
		Utils.RegisterFunc(L, -1, "sortingOrder", _s_set_sortingOrder);
		Utils.RegisterFunc(L, -1, "color", _s_set_color);
		Utils.RegisterFunc(L, -1, "sprite", _s_set_sprite);
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
				SpriteMeshRenderer o = new SpriteMeshRenderer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SpriteMeshRenderer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadSprite(IntPtr L)
	{
		try
		{
			SpriteMeshRenderer meshRenderer = (SpriteMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				string spritePath = Lua.lua_tostring(L, 2);
				string defaultSprite = Lua.lua_tostring(L, 3);
				bool isAsync = Lua.lua_toboolean(L, 4);
				meshRenderer.LoadSprite(spritePath, defaultSprite, isAsync);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string spritePath2 = Lua.lua_tostring(L, 2);
				string defaultSprite2 = Lua.lua_tostring(L, 3);
				meshRenderer.LoadSprite(spritePath2, defaultSprite2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string spritePath3 = Lua.lua_tostring(L, 2);
				meshRenderer.LoadSprite(spritePath3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SpriteMeshRenderer.LoadSprite!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color_a(IntPtr L)
	{
		try
		{
			SpriteMeshRenderer sr = (SpriteMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float a = (float)Lua.lua_tonumber(L, 2);
			sr.Set_color_a(a);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sharedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteMeshRenderer spriteMeshRenderer = (SpriteMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, spriteMeshRenderer.sharedMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sortingLayerID(IntPtr L)
	{
		try
		{
			SpriteMeshRenderer spriteMeshRenderer = (SpriteMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, spriteMeshRenderer.sortingLayerID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sortingOrder(IntPtr L)
	{
		try
		{
			SpriteMeshRenderer spriteMeshRenderer = (SpriteMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, spriteMeshRenderer.sortingOrder);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_color(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteMeshRenderer spriteMeshRenderer = (SpriteMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, spriteMeshRenderer.color);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteMeshRenderer spriteMeshRenderer = (SpriteMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, spriteMeshRenderer.sprite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sharedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SpriteMeshRenderer)objectTranslator.FastGetCSObj(L, 1)).sharedMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sortingLayerID(IntPtr L)
	{
		try
		{
			((SpriteMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sortingLayerID = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sortingOrder(IntPtr L)
	{
		try
		{
			((SpriteMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sortingOrder = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_color(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteMeshRenderer spriteMeshRenderer = (SpriteMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			spriteMeshRenderer.color = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SpriteMeshRenderer)objectTranslator.FastGetCSObj(L, 1)).sprite = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
