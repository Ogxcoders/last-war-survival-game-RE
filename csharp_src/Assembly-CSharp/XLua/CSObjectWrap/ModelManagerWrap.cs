using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ModelManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ModelManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 20, 2, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "InitLoadModel", _m_InitLoadModel);
		Utils.RegisterFunc(L, -3, "GetFormationUuid", _m_GetFormationUuid);
		Utils.RegisterFunc(L, -3, "LoadOneObject", _m_LoadOneObject);
		Utils.RegisterFunc(L, -3, "RemoveOneObject", _m_RemoveOneObject);
		Utils.RegisterFunc(L, -3, "RemoveOneObjectByPointType", _m_RemoveOneObjectByPointType);
		Utils.RegisterFunc(L, -3, "GetObjectByPointId", _m_GetObjectByPointId);
		Utils.RegisterFunc(L, -3, "ClearReInitObject", _m_ClearReInitObject);
		Utils.RegisterFunc(L, -3, "ReInitObject", _m_ReInitObject);
		Utils.RegisterFunc(L, -3, "ClearReInitObjectByFilter", _m_ClearReInitObjectByFilter);
		Utils.RegisterFunc(L, -3, "IsCanShowBuild", _m_IsCanShowBuild);
		Utils.RegisterFunc(L, -3, "LoadCityTroop", _m_LoadCityTroop);
		Utils.RegisterFunc(L, -3, "DestroyCityTroop", _m_DestroyCityTroop);
		Utils.RegisterFunc(L, -3, "CreateCitySpaceMan", _m_CreateCitySpaceMan);
		Utils.RegisterFunc(L, -3, "DestroyCitySpaceMan", _m_DestroyCitySpaceMan);
		Utils.RegisterFunc(L, -3, "GetCityTroop", _m_GetCityTroop);
		Utils.RegisterFunc(L, -3, "SetVisibleByPointType", _m_SetVisibleByPointType);
		Utils.RegisterFunc(L, -3, "IsNoDoBuildAnim", _m_IsNoDoBuildAnim);
		Utils.RegisterFunc(L, -2, "UsePveReturnOpt", _g_get_UsePveReturnOpt);
		Utils.RegisterFunc(L, -2, "UseBuildArrayOpt", _g_get_UseBuildArrayOpt);
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
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<CityScene>(L, 2))
			{
				ModelManager o = new ModelManager((CityScene)objectTranslator.GetObject(L, 2, typeof(CityScene)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInit(IntPtr L)
	{
		try
		{
			((ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
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
			ModelManager obj = (ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_InitLoadModel(IntPtr L)
	{
		try
		{
			((ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitLoadModel();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFormationUuid(IntPtr L)
	{
		try
		{
			long formationUuid = ((ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetFormationUuid();
			Lua.lua_pushint64(L, formationUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadOneObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager modelManager = (ModelManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<object>(L, 4))
			{
				int index = Lua.xlua_tointeger(L, 2);
				int modelObjectType = Lua.xlua_tointeger(L, 3);
				object param = objectTranslator.GetObject(L, 4, typeof(object));
				ModelManager.ModelObject o = modelManager.LoadOneObject(index, modelObjectType, param);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int index2 = Lua.xlua_tointeger(L, 2);
				int modelObjectType2 = Lua.xlua_tointeger(L, 3);
				ModelManager.ModelObject o2 = modelManager.LoadOneObject(index2, modelObjectType2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager.LoadOneObject!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveOneObject(IntPtr L)
	{
		try
		{
			ModelManager obj = (ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			obj.RemoveOneObject(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveOneObjectByPointType(IntPtr L)
	{
		try
		{
			ModelManager obj = (ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int pointType = Lua.xlua_tointeger(L, 3);
			obj.RemoveOneObjectByPointType(index, pointType);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetObjectByPointId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ModelManager obj = (ModelManager)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			ModelManager.ModelObject objectByPointId = obj.GetObjectByPointId(index);
			objectTranslator.Push(L, objectByPointId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearReInitObject(IntPtr L)
	{
		try
		{
			((ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearReInitObject();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReInitObject(IntPtr L)
	{
		try
		{
			((ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReInitObject();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearReInitObjectByFilter(IntPtr L)
	{
		try
		{
			((ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearReInitObjectByFilter();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCanShowBuild(IntPtr L)
	{
		try
		{
			bool value = ((ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsCanShowBuild();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadCityTroop(IntPtr L)
	{
		try
		{
			ModelManager modelManager = (ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int createPos = Lua.xlua_tointeger(L, 2);
				int targetPos = Lua.xlua_tointeger(L, 3);
				modelManager.LoadCityTroop(createPos, targetPos);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int createPos2 = Lua.xlua_tointeger(L, 2);
				modelManager.LoadCityTroop(createPos2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ModelManager.LoadCityTroop!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyCityTroop(IntPtr L)
	{
		try
		{
			((ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DestroyCityTroop();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateCitySpaceMan(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceMan o = ((ModelManager)objectTranslator.FastGetCSObj(L, 1)).CreateCitySpaceMan();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyCitySpaceMan(IntPtr L)
	{
		try
		{
			((ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DestroyCitySpaceMan();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCityTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = ((ModelManager)objectTranslator.FastGetCSObj(L, 1)).GetCityTroop();
			objectTranslator.Push(L, cityTroop);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVisibleByPointType(IntPtr L)
	{
		try
		{
			ModelManager obj = (ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointType = Lua.xlua_tointeger(L, 2);
			bool isVisible = Lua.lua_toboolean(L, 3);
			obj.SetVisibleByPointType(pointType, isVisible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNoDoBuildAnim(IntPtr L)
	{
		try
		{
			ModelManager obj = (ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			bool value = obj.IsNoDoBuildAnim(uuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UsePveReturnOpt(IntPtr L)
	{
		try
		{
			ModelManager modelManager = (ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, modelManager.UsePveReturnOpt);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UseBuildArrayOpt(IntPtr L)
	{
		try
		{
			ModelManager modelManager = (ModelManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, modelManager.UseBuildArrayOpt);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
