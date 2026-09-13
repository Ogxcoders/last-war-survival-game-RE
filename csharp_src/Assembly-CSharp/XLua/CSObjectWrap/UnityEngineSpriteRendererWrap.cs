using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineSpriteRendererWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SpriteRenderer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 15, 10, 10);
		Utils.RegisterFunc(L, -3, "LoadSprite", _m_LoadSprite);
		Utils.RegisterFunc(L, -3, "LoadSpriteAsync", _m_LoadSpriteAsync);
		Utils.RegisterFunc(L, -3, "ConvertToSpriteMeshRender", _m_ConvertToSpriteMeshRender);
		Utils.RegisterFunc(L, -3, "Set_size", _m_Set_size);
		Utils.RegisterFunc(L, -3, "Get_size", _m_Get_size);
		Utils.RegisterFunc(L, -3, "Set_color", _m_Set_color);
		Utils.RegisterFunc(L, -3, "Set_color_r", _m_Set_color_r);
		Utils.RegisterFunc(L, -3, "Set_color_g", _m_Set_color_g);
		Utils.RegisterFunc(L, -3, "Set_color_b", _m_Set_color_b);
		Utils.RegisterFunc(L, -3, "Set_color_a", _m_Set_color_a);
		Utils.RegisterFunc(L, -3, "Get_color", _m_Get_color);
		Utils.RegisterFunc(L, -3, "Get_color_r", _m_Get_color_r);
		Utils.RegisterFunc(L, -3, "Get_color_g", _m_Get_color_g);
		Utils.RegisterFunc(L, -3, "Get_color_b", _m_Get_color_b);
		Utils.RegisterFunc(L, -3, "Get_color_a", _m_Get_color_a);
		Utils.RegisterFunc(L, -2, "sprite", _g_get_sprite);
		Utils.RegisterFunc(L, -2, "drawMode", _g_get_drawMode);
		Utils.RegisterFunc(L, -2, "size", _g_get_size);
		Utils.RegisterFunc(L, -2, "adaptiveModeThreshold", _g_get_adaptiveModeThreshold);
		Utils.RegisterFunc(L, -2, "tileMode", _g_get_tileMode);
		Utils.RegisterFunc(L, -2, "color", _g_get_color);
		Utils.RegisterFunc(L, -2, "maskInteraction", _g_get_maskInteraction);
		Utils.RegisterFunc(L, -2, "flipX", _g_get_flipX);
		Utils.RegisterFunc(L, -2, "flipY", _g_get_flipY);
		Utils.RegisterFunc(L, -2, "spriteSortPoint", _g_get_spriteSortPoint);
		Utils.RegisterFunc(L, -1, "sprite", _s_set_sprite);
		Utils.RegisterFunc(L, -1, "drawMode", _s_set_drawMode);
		Utils.RegisterFunc(L, -1, "size", _s_set_size);
		Utils.RegisterFunc(L, -1, "adaptiveModeThreshold", _s_set_adaptiveModeThreshold);
		Utils.RegisterFunc(L, -1, "tileMode", _s_set_tileMode);
		Utils.RegisterFunc(L, -1, "color", _s_set_color);
		Utils.RegisterFunc(L, -1, "maskInteraction", _s_set_maskInteraction);
		Utils.RegisterFunc(L, -1, "flipX", _s_set_flipX);
		Utils.RegisterFunc(L, -1, "flipY", _s_set_flipY);
		Utils.RegisterFunc(L, -1, "spriteSortPoint", _s_set_spriteSortPoint);
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
				SpriteRenderer o = new SpriteRenderer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SpriteRenderer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadSprite(IntPtr L)
	{
		try
		{
			SpriteRenderer spriteRenderer = (SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string spritePath = Lua.lua_tostring(L, 2);
				string defaultSprite = Lua.lua_tostring(L, 3);
				spriteRenderer.LoadSprite(spritePath, defaultSprite);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string spritePath2 = Lua.lua_tostring(L, 2);
				spriteRenderer.LoadSprite(spritePath2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SpriteRenderer.LoadSprite!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadSpriteAsync(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string spritePath = Lua.lua_tostring(L, 2);
				string defaultSprite = Lua.lua_tostring(L, 3);
				spriteRenderer.LoadSpriteAsync(spritePath, defaultSprite);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string spritePath2 = Lua.lua_tostring(L, 2);
				spriteRenderer.LoadSpriteAsync(spritePath2);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<Sprite>>(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				string spritePath3 = Lua.lua_tostring(L, 2);
				Action<Sprite> completeCallback = objectTranslator.GetDelegate<Action<Sprite>>(L, 3);
				string defaultSprite2 = Lua.lua_tostring(L, 4);
				spriteRenderer.LoadSpriteAsync(spritePath3, completeCallback, defaultSprite2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Action<Sprite>>(L, 3))
			{
				string spritePath4 = Lua.lua_tostring(L, 2);
				Action<Sprite> completeCallback2 = objectTranslator.GetDelegate<Action<Sprite>>(L, 3);
				spriteRenderer.LoadSpriteAsync(spritePath4, completeCallback2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SpriteRenderer.LoadSpriteAsync!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConvertToSpriteMeshRender(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteMeshRenderer o = ((SpriteRenderer)objectTranslator.FastGetCSObj(L, 1)).ConvertToSpriteMeshRender();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_size(IntPtr L)
	{
		try
		{
			SpriteRenderer r = (SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			r.Set_size(x, y);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_size(IntPtr L)
	{
		try
		{
			((SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_size(out var x, out var y);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color(IntPtr L)
	{
		try
		{
			SpriteRenderer sr = (SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float r = (float)Lua.lua_tonumber(L, 2);
			float g = (float)Lua.lua_tonumber(L, 3);
			float b = (float)Lua.lua_tonumber(L, 4);
			float a = (float)Lua.lua_tonumber(L, 5);
			sr.Set_color(r, g, b, a);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color_r(IntPtr L)
	{
		try
		{
			SpriteRenderer sr = (SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float r = (float)Lua.lua_tonumber(L, 2);
			sr.Set_color_r(r);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color_g(IntPtr L)
	{
		try
		{
			SpriteRenderer sr = (SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float g = (float)Lua.lua_tonumber(L, 2);
			sr.Set_color_g(g);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color_b(IntPtr L)
	{
		try
		{
			SpriteRenderer sr = (SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float b = (float)Lua.lua_tonumber(L, 2);
			sr.Set_color_b(b);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_color_a(IntPtr L)
	{
		try
		{
			SpriteRenderer sr = (SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_Get_color(IntPtr L)
	{
		try
		{
			((SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_color(out var r, out var g, out var b, out var a);
			Lua.lua_pushnumber(L, r);
			Lua.lua_pushnumber(L, g);
			Lua.lua_pushnumber(L, b);
			Lua.lua_pushnumber(L, a);
			return 4;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_color_r(IntPtr L)
	{
		try
		{
			((SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_color_r(out var r);
			Lua.lua_pushnumber(L, r);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_color_g(IntPtr L)
	{
		try
		{
			((SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_color_g(out var g);
			Lua.lua_pushnumber(L, g);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_color_b(IntPtr L)
	{
		try
		{
			((SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_color_b(out var b);
			Lua.lua_pushnumber(L, b);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_color_a(IntPtr L)
	{
		try
		{
			((SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_color_a(out var a);
			Lua.lua_pushnumber(L, a);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, spriteRenderer.sprite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_drawMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, spriteRenderer.drawMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_size(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, spriteRenderer.size);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_adaptiveModeThreshold(IntPtr L)
	{
		try
		{
			SpriteRenderer spriteRenderer = (SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, spriteRenderer.adaptiveModeThreshold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tileMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, spriteRenderer.tileMode);
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
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, spriteRenderer.color);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maskInteraction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, spriteRenderer.maskInteraction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_flipX(IntPtr L)
	{
		try
		{
			SpriteRenderer spriteRenderer = (SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, spriteRenderer.flipX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_flipY(IntPtr L)
	{
		try
		{
			SpriteRenderer spriteRenderer = (SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, spriteRenderer.flipY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteSortPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, spriteRenderer.spriteSortPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SpriteRenderer)objectTranslator.FastGetCSObj(L, 1)).sprite = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_drawMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SpriteDrawMode v);
			spriteRenderer.drawMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_size(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			spriteRenderer.size = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_adaptiveModeThreshold(IntPtr L)
	{
		try
		{
			((SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).adaptiveModeThreshold = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tileMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SpriteTileMode v);
			spriteRenderer.tileMode = v;
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
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			spriteRenderer.color = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maskInteraction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SpriteMaskInteraction v);
			spriteRenderer.maskInteraction = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_flipX(IntPtr L)
	{
		try
		{
			((SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).flipX = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_flipY(IntPtr L)
	{
		try
		{
			((SpriteRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).flipY = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spriteSortPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SpriteRenderer spriteRenderer = (SpriteRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SpriteSortPoint v);
			spriteRenderer.spriteSortPoint = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
