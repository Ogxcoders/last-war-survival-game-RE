using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineSpriteWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Sprite);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 16, 0);
		Utils.RegisterFunc(L, -3, "GetPhysicsShapeCount", _m_GetPhysicsShapeCount);
		Utils.RegisterFunc(L, -3, "GetPhysicsShapePointCount", _m_GetPhysicsShapePointCount);
		Utils.RegisterFunc(L, -3, "GetPhysicsShape", _m_GetPhysicsShape);
		Utils.RegisterFunc(L, -3, "OverridePhysicsShape", _m_OverridePhysicsShape);
		Utils.RegisterFunc(L, -3, "OverrideGeometry", _m_OverrideGeometry);
		Utils.RegisterFunc(L, -2, "bounds", _g_get_bounds);
		Utils.RegisterFunc(L, -2, "rect", _g_get_rect);
		Utils.RegisterFunc(L, -2, "border", _g_get_border);
		Utils.RegisterFunc(L, -2, "texture", _g_get_texture);
		Utils.RegisterFunc(L, -2, "pixelsPerUnit", _g_get_pixelsPerUnit);
		Utils.RegisterFunc(L, -2, "spriteAtlasTextureScale", _g_get_spriteAtlasTextureScale);
		Utils.RegisterFunc(L, -2, "associatedAlphaSplitTexture", _g_get_associatedAlphaSplitTexture);
		Utils.RegisterFunc(L, -2, "pivot", _g_get_pivot);
		Utils.RegisterFunc(L, -2, "packed", _g_get_packed);
		Utils.RegisterFunc(L, -2, "packingMode", _g_get_packingMode);
		Utils.RegisterFunc(L, -2, "packingRotation", _g_get_packingRotation);
		Utils.RegisterFunc(L, -2, "textureRect", _g_get_textureRect);
		Utils.RegisterFunc(L, -2, "textureRectOffset", _g_get_textureRectOffset);
		Utils.RegisterFunc(L, -2, "vertices", _g_get_vertices);
		Utils.RegisterFunc(L, -2, "triangles", _g_get_triangles);
		Utils.RegisterFunc(L, -2, "uv", _g_get_uv);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "Create", _m_Create_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.Sprite does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPhysicsShapeCount(IntPtr L)
	{
		try
		{
			int physicsShapeCount = ((Sprite)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPhysicsShapeCount();
			Lua.xlua_pushinteger(L, physicsShapeCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPhysicsShapePointCount(IntPtr L)
	{
		try
		{
			Sprite obj = (Sprite)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int shapeIdx = Lua.xlua_tointeger(L, 2);
			int physicsShapePointCount = obj.GetPhysicsShapePointCount(shapeIdx);
			Lua.xlua_pushinteger(L, physicsShapePointCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPhysicsShape(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			int shapeIdx = Lua.xlua_tointeger(L, 2);
			List<Vector2> physicsShape = (List<Vector2>)objectTranslator.GetObject(L, 3, typeof(List<Vector2>));
			int physicsShape2 = sprite.GetPhysicsShape(shapeIdx, physicsShape);
			Lua.xlua_pushinteger(L, physicsShape2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverridePhysicsShape(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			IList<Vector2[]> physicsShapes = (IList<Vector2[]>)objectTranslator.GetObject(L, 2, typeof(IList<Vector2[]>));
			sprite.OverridePhysicsShape(physicsShapes);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverrideGeometry(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			Vector2[] vertices = (Vector2[])objectTranslator.GetObject(L, 2, typeof(Vector2[]));
			ushort[] triangles = (ushort[])objectTranslator.GetObject(L, 3, typeof(ushort[]));
			sprite.OverrideGeometry(vertices, triangles);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Create_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Texture2D>(L, 1) && objectTranslator.Assignable<Rect>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3))
			{
				Texture2D texture = (Texture2D)objectTranslator.GetObject(L, 1, typeof(Texture2D));
				objectTranslator.Get(L, 2, out Rect v);
				objectTranslator.Get(L, 3, out Vector2 val);
				Sprite o = Sprite.Create(texture, v, val);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Texture2D>(L, 1) && objectTranslator.Assignable<Rect>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Texture2D texture2 = (Texture2D)objectTranslator.GetObject(L, 1, typeof(Texture2D));
				objectTranslator.Get(L, 2, out Rect v2);
				objectTranslator.Get(L, 3, out Vector2 val2);
				Sprite o2 = Sprite.Create(pixelsPerUnit: (float)Lua.lua_tonumber(L, 4), texture: texture2, rect: v2, pivot: val2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Texture2D>(L, 1) && objectTranslator.Assignable<Rect>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Texture2D texture3 = (Texture2D)objectTranslator.GetObject(L, 1, typeof(Texture2D));
				objectTranslator.Get(L, 2, out Rect v3);
				objectTranslator.Get(L, 3, out Vector2 val3);
				Sprite o3 = Sprite.Create(pixelsPerUnit: (float)Lua.lua_tonumber(L, 4), extrude: Lua.xlua_touint(L, 5), texture: texture3, rect: v3, pivot: val3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Texture2D>(L, 1) && objectTranslator.Assignable<Rect>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<SpriteMeshType>(L, 6))
			{
				Texture2D texture4 = (Texture2D)objectTranslator.GetObject(L, 1, typeof(Texture2D));
				objectTranslator.Get(L, 2, out Rect v4);
				objectTranslator.Get(L, 3, out Vector2 val4);
				float pixelsPerUnit = (float)Lua.lua_tonumber(L, 4);
				uint extrude = Lua.xlua_touint(L, 5);
				objectTranslator.Get(L, 6, out SpriteMeshType v5);
				Sprite o4 = Sprite.Create(texture4, v4, val4, pixelsPerUnit, extrude, v5);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Texture2D>(L, 1) && objectTranslator.Assignable<Rect>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<SpriteMeshType>(L, 6) && objectTranslator.Assignable<Vector4>(L, 7))
			{
				Texture2D texture5 = (Texture2D)objectTranslator.GetObject(L, 1, typeof(Texture2D));
				objectTranslator.Get(L, 2, out Rect v6);
				objectTranslator.Get(L, 3, out Vector2 val5);
				float pixelsPerUnit2 = (float)Lua.lua_tonumber(L, 4);
				uint extrude2 = Lua.xlua_touint(L, 5);
				objectTranslator.Get(L, 6, out SpriteMeshType v7);
				objectTranslator.Get(L, 7, out Vector4 val6);
				Sprite o5 = Sprite.Create(texture5, v6, val5, pixelsPerUnit2, extrude2, v7, val6);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 8 && objectTranslator.Assignable<Texture2D>(L, 1) && objectTranslator.Assignable<Rect>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<SpriteMeshType>(L, 6) && objectTranslator.Assignable<Vector4>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				Texture2D texture6 = (Texture2D)objectTranslator.GetObject(L, 1, typeof(Texture2D));
				objectTranslator.Get(L, 2, out Rect v8);
				objectTranslator.Get(L, 3, out Vector2 val7);
				float pixelsPerUnit3 = (float)Lua.lua_tonumber(L, 4);
				uint extrude3 = Lua.xlua_touint(L, 5);
				objectTranslator.Get(L, 6, out SpriteMeshType v9);
				objectTranslator.Get(L, 7, out Vector4 val8);
				Sprite o6 = Sprite.Create(generateFallbackPhysicsShape: Lua.lua_toboolean(L, 8), texture: texture6, rect: v8, pivot: val7, pixelsPerUnit: pixelsPerUnit3, extrude: extrude3, meshType: v9, border: val8);
				objectTranslator.Push(L, o6);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Sprite.Create!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, sprite.bounds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, sprite.rect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_border(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector4(L, sprite.border);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_texture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, sprite.texture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelsPerUnit(IntPtr L)
	{
		try
		{
			Sprite sprite = (Sprite)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, sprite.pixelsPerUnit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteAtlasTextureScale(IntPtr L)
	{
		try
		{
			Sprite sprite = (Sprite)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, sprite.spriteAtlasTextureScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_associatedAlphaSplitTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, sprite.associatedAlphaSplitTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pivot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, sprite.pivot);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_packed(IntPtr L)
	{
		try
		{
			Sprite sprite = (Sprite)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, sprite.packed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_packingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, sprite.packingMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_packingRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, sprite.packingRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, sprite.textureRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureRectOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, sprite.textureRectOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_vertices(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, sprite.vertices);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_triangles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, sprite.triangles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uv(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Sprite sprite = (Sprite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, sprite.uv);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
