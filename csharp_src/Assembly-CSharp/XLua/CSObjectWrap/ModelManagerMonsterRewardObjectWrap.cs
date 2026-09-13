using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ModelManagerMonsterRewardObjectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ModelManager.MonsterRewardObject);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 9, 0, 0);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "CreateGameObject", _m_CreateGameObject);
		Utils.RegisterFunc(L, -3, "UpdateGameObject", _m_UpdateGameObject);
		Utils.RegisterFunc(L, -3, "UpdateLod", _m_UpdateLod);
		Utils.RegisterFunc(L, -3, "UpdateFog", _m_UpdateFog);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "DoGuideStartAnim", _m_DoGuideStartAnim);
		Utils.RegisterFunc(L, -3, "SetIsVisible", _m_SetIsVisible);
		Utils.RegisterFunc(L, -3, "GetObject", _m_GetObject);
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
			if (Lua.lua_gettop(L) == 4 && objectTranslator.Assignable<ModelManager>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<ModelManager.ModelObjectType>(L, 4))
			{
				ModelManager parent = (ModelManager)objectTranslator.GetObject(L, 2, typeof(ModelManager));
				int pointId = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out ModelManager.ModelObjectType val);
				ModelManager.MonsterRewardObject o = new ModelManager.MonsterRewardObject(parent, pointId, val);
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager.MonsterRewardObject constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			((ModelManager.MonsterRewardObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateGameObject(IntPtr L)
	{
		try
		{
			((ModelManager.MonsterRewardObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateGameObject();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateGameObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager.MonsterRewardObject monsterRewardObject = (ModelManager.MonsterRewardObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object param = objectTranslator.GetObject(L, 2, typeof(object));
				monsterRewardObject.UpdateGameObject(param);
				return 0;
			}
			if (num == 1)
			{
				monsterRewardObject.UpdateGameObject();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager.MonsterRewardObject.UpdateGameObject!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateLod(IntPtr L)
	{
		try
		{
			ModelManager.MonsterRewardObject obj = (ModelManager.MonsterRewardObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			obj.UpdateLod(lod);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateFog(IntPtr L)
	{
		try
		{
			ModelManager.MonsterRewardObject obj = (ModelManager.MonsterRewardObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int fogId = Lua.xlua_tointeger(L, 2);
			obj.UpdateFog(fogId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			ModelManager.MonsterRewardObject obj = (ModelManager.MonsterRewardObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(deltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoGuideStartAnim(IntPtr L)
	{
		try
		{
			ModelManager.MonsterRewardObject obj = (ModelManager.MonsterRewardObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int time = Lua.xlua_tointeger(L, 2);
			obj.DoGuideStartAnim(time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIsVisible(IntPtr L)
	{
		try
		{
			ModelManager.MonsterRewardObject obj = (ModelManager.MonsterRewardObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isVisible = Lua.lua_toboolean(L, 2);
			obj.SetIsVisible(isVisible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject o = ((ModelManager.MonsterRewardObject)objectTranslator.FastGetCSObj(L, 1)).GetObject();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
