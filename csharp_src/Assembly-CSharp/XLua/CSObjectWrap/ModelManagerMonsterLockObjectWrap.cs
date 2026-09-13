using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ModelManagerMonsterLockObjectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ModelManager.MonsterLockObject);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 0, 0);
		Utils.RegisterFunc(L, -3, "UpdateLod", _m_UpdateLod);
		Utils.RegisterFunc(L, -3, "UpdateFog", _m_UpdateFog);
		Utils.RegisterFunc(L, -3, "CreateGameObject", _m_CreateGameObject);
		Utils.RegisterFunc(L, -3, "UpdateGameObject", _m_UpdateGameObject);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "FadeOut", _m_FadeOut);
		Utils.RegisterFunc(L, -3, "DoGuideStartAnim", _m_DoGuideStartAnim);
		Utils.RegisterFunc(L, -3, "SetIsVisible", _m_SetIsVisible);
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
			if (Lua.lua_gettop(L) == 5 && objectTranslator.Assignable<ModelManager>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<ModelManager.ModelObjectType>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				ModelManager parent = (ModelManager)objectTranslator.GetObject(L, 2, typeof(ModelManager));
				int index = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out ModelManager.ModelObjectType val);
				ModelManager.MonsterLockObject o = new ModelManager.MonsterLockObject(monsterId: Lua.xlua_tointeger(L, 5), parent: parent, index: index, modelType: val);
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager.MonsterLockObject constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateLod(IntPtr L)
	{
		try
		{
			ModelManager.MonsterLockObject obj = (ModelManager.MonsterLockObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			ModelManager.MonsterLockObject obj = (ModelManager.MonsterLockObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_CreateGameObject(IntPtr L)
	{
		try
		{
			((ModelManager.MonsterLockObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateGameObject();
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
			ModelManager.MonsterLockObject monsterLockObject = (ModelManager.MonsterLockObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object param = objectTranslator.GetObject(L, 2, typeof(object));
				monsterLockObject.UpdateGameObject(param);
				return 0;
			}
			if (num == 1)
			{
				monsterLockObject.UpdateGameObject();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager.MonsterLockObject.UpdateGameObject!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			((ModelManager.MonsterLockObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FadeOut(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager.MonsterLockObject monsterLockObject = (ModelManager.MonsterLockObject)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Action>(L, 2))
			{
				Action callback = objectTranslator.GetDelegate<Action>(L, 2);
				monsterLockObject.FadeOut(callback);
				return 0;
			}
			if (num == 1)
			{
				monsterLockObject.FadeOut();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager.MonsterLockObject.FadeOut!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoGuideStartAnim(IntPtr L)
	{
		try
		{
			ModelManager.MonsterLockObject obj = (ModelManager.MonsterLockObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			ModelManager.MonsterLockObject obj = (ModelManager.MonsterLockObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isVisible = Lua.lua_toboolean(L, 2);
			obj.SetIsVisible(isVisible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
