using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BattlefieldBuildingObjectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BattlefieldBuildingObject);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 3, 3);
		Utils.RegisterFunc(L, -3, "SetStyleIndex", _m_SetStyleIndex);
		Utils.RegisterFunc(L, -3, "SetIcon", _m_SetIcon);
		Utils.RegisterFunc(L, -2, "renderer", _g_get_renderer);
		Utils.RegisterFunc(L, -2, "iconRenderer", _g_get_iconRenderer);
		Utils.RegisterFunc(L, -2, "iconNode", _g_get_iconNode);
		Utils.RegisterFunc(L, -1, "renderer", _s_set_renderer);
		Utils.RegisterFunc(L, -1, "iconRenderer", _s_set_iconRenderer);
		Utils.RegisterFunc(L, -1, "iconNode", _s_set_iconNode);
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
				BattlefieldBuildingObject o = new BattlefieldBuildingObject();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BattlefieldBuildingObject constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetStyleIndex(IntPtr L)
	{
		try
		{
			BattlefieldBuildingObject obj = (BattlefieldBuildingObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int styleIndex = Lua.xlua_tointeger(L, 2);
			obj.SetStyleIndex(styleIndex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIcon(IntPtr L)
	{
		try
		{
			BattlefieldBuildingObject obj = (BattlefieldBuildingObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string icon = Lua.lua_tostring(L, 2);
			obj.SetIcon(icon);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattlefieldBuildingObject battlefieldBuildingObject = (BattlefieldBuildingObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, battlefieldBuildingObject.renderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_iconRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattlefieldBuildingObject battlefieldBuildingObject = (BattlefieldBuildingObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, battlefieldBuildingObject.iconRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_iconNode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattlefieldBuildingObject battlefieldBuildingObject = (BattlefieldBuildingObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, battlefieldBuildingObject.iconNode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BattlefieldBuildingObject)objectTranslator.FastGetCSObj(L, 1)).renderer = (Renderer)objectTranslator.GetObject(L, 2, typeof(Renderer));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_iconRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BattlefieldBuildingObject)objectTranslator.FastGetCSObj(L, 1)).iconRenderer = (SpriteRenderer)objectTranslator.GetObject(L, 2, typeof(SpriteRenderer));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_iconNode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BattlefieldBuildingObject)objectTranslator.FastGetCSObj(L, 1)).iconNode = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
