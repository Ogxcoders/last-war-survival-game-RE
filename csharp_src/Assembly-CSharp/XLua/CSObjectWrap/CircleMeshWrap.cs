using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CircleMeshWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CircleMesh);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 2, 0);
		Utils.RegisterFunc(L, -3, "SetupTexture", _m_SetupTexture);
		Utils.RegisterFunc(L, -3, "SetupSprite", _m_SetupSprite);
		Utils.RegisterFunc(L, -3, "RebuildMesh", _m_RebuildMesh);
		Utils.RegisterFunc(L, -3, "Release", _m_Release);
		Utils.RegisterFunc(L, -3, "LoadSprite", _m_LoadSprite);
		Utils.RegisterFunc(L, -2, "MeshRenderer", _g_get_MeshRenderer);
		Utils.RegisterFunc(L, -2, "MainMaterial", _g_get_MainMaterial);
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
				CircleMesh o = new CircleMesh();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CircleMesh constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CircleMesh circleMesh = (CircleMesh)objectTranslator.FastGetCSObj(L, 1);
			Texture texture = (Texture)objectTranslator.GetObject(L, 2, typeof(Texture));
			circleMesh.SetupTexture(texture);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupSprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CircleMesh circleMesh = (CircleMesh)objectTranslator.FastGetCSObj(L, 1);
			Sprite sprite = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
			circleMesh.SetupSprite(sprite);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RebuildMesh(IntPtr L)
	{
		try
		{
			CircleMesh obj = (CircleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float radius = (float)Lua.lua_tonumber(L, 2);
			int segmentCount = Lua.xlua_tointeger(L, 3);
			obj.RebuildMesh(radius, segmentCount);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Release(IntPtr L)
	{
		try
		{
			((CircleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Release();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadSprite(IntPtr L)
	{
		try
		{
			CircleMesh circleMesh = (CircleMesh)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string spritePath = Lua.lua_tostring(L, 2);
				string defaultSprite = Lua.lua_tostring(L, 3);
				circleMesh.LoadSprite(spritePath, defaultSprite);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string spritePath2 = Lua.lua_tostring(L, 2);
				circleMesh.LoadSprite(spritePath2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CircleMesh.LoadSprite!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MeshRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CircleMesh circleMesh = (CircleMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, circleMesh.MeshRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MainMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CircleMesh circleMesh = (CircleMesh)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, circleMesh.MainMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
