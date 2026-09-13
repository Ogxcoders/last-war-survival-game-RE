using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldTroopManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldTroopManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 29, 1, 0);
		Utils.RegisterFunc(L, -3, "EditorDescription", _m_EditorDescription);
		Utils.RegisterFunc(L, -3, "RefreshOrCreateTroop", _m_RefreshOrCreateTroop);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "GetTroop", _m_GetTroop);
		Utils.RegisterFunc(L, -3, "CreateTroop", _m_CreateTroop);
		Utils.RegisterFunc(L, -3, "RefreshNeedCreateTroop", _m_RefreshNeedCreateTroop);
		Utils.RegisterFunc(L, -3, "DestroyTroop", _m_DestroyTroop);
		Utils.RegisterFunc(L, -3, "TryDestroyLittleSmartTroop", _m_TryDestroyLittleSmartTroop);
		Utils.RegisterFunc(L, -3, "UpdateTroop", _m_UpdateTroop);
		Utils.RegisterFunc(L, -3, "RefreshPosition", _m_RefreshPosition);
		Utils.RegisterFunc(L, -3, "OnMonsterIceBroken", _m_OnMonsterIceBroken);
		Utils.RegisterFunc(L, -3, "IsTroopCreate", _m_IsTroopCreate);
		Utils.RegisterFunc(L, -3, "OnDragUpdate", _m_OnDragUpdate);
		Utils.RegisterFunc(L, -3, "GetDestinationType", _m_GetDestinationType);
		Utils.RegisterFunc(L, -3, "GetTargetType", _m_GetTargetType);
		Utils.RegisterFunc(L, -3, "OnDragStop", _m_OnDragStop);
		Utils.RegisterFunc(L, -3, "OnLodChanged", _m_OnLodChanged);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "CreateBattleVFX", _m_CreateBattleVFX);
		Utils.RegisterFunc(L, -3, "OnDrawGizmos", _m_OnDrawGizmos);
		Utils.RegisterFunc(L, -3, "GetModelHeight", _m_GetModelHeight);
		Utils.RegisterFunc(L, -3, "CreateGroupTroop", _m_CreateGroupTroop);
		Utils.RegisterFunc(L, -3, "GetCurPosAndRotationTroopNum", _m_GetCurPosAndRotationTroopNum);
		Utils.RegisterFunc(L, -3, "RemovePosAndRotationDataByMarchUuid", _m_RemovePosAndRotationDataByMarchUuid);
		Utils.RegisterFunc(L, -3, "ShowWorldMarchByTypeSignal", _m_ShowWorldMarchByTypeSignal);
		Utils.RegisterFunc(L, -3, "HideWorldMarchByTypeSignal", _m_HideWorldMarchByTypeSignal);
		Utils.RegisterFunc(L, -3, "SetModelVisibleByMarchTypeForLua", _m_SetModelVisibleByMarchTypeForLua);
		Utils.RegisterFunc(L, -3, "CanShow", _m_CanShow);
		Utils.RegisterFunc(L, -2, "TroopCount", _g_get_TroopCount);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 4, 0);
		Utils.RegisterFunc(L, -2, "EditorInstance", _g_get_EditorInstance);
		Utils.RegisterFunc(L, -2, "__DEBUG_LOG__", _g_get___DEBUG_LOG__);
		Utils.RegisterFunc(L, -2, "time", _g_get_time);
		Utils.RegisterFunc(L, -2, "busy", _g_get_busy);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldScene>(L, 2))
			{
				WorldTroopManager o = new WorldTroopManager((WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroopManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorDescription(IntPtr L)
	{
		try
		{
			string str = ((WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EditorDescription();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshOrCreateTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			worldTroopManager.RefreshOrCreateTroop(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
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
			((WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager obj = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			WorldTroop troop = obj.GetTroop(marchUuid);
			objectTranslator.Push(L, troop);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			worldTroopManager.CreateTroop(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshNeedCreateTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			worldTroopManager.RefreshNeedCreateTroop(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyTroop(IntPtr L)
	{
		try
		{
			WorldTroopManager worldTroopManager = (WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				long marchUuid = Lua.lua_toint64(L, 2);
				bool isBattleFailed = Lua.lua_toboolean(L, 3);
				float num2 = worldTroopManager.DestroyTroop(marchUuid, isBattleFailed);
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long marchUuid2 = Lua.lua_toint64(L, 2);
				float num3 = worldTroopManager.DestroyTroop(marchUuid2);
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroopManager.DestroyTroop!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryDestroyLittleSmartTroop(IntPtr L)
	{
		try
		{
			WorldTroopManager obj = (WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			obj.TryDestroyLittleSmartTroop(marchUuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			worldTroopManager.UpdateTroop(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			worldTroopManager.RefreshPosition(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnMonsterIceBroken(IntPtr L)
	{
		try
		{
			WorldTroopManager obj = (WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			obj.OnMonsterIceBroken(uuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTroopCreate(IntPtr L)
	{
		try
		{
			WorldTroopManager obj = (WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			bool value = obj.IsTroopCreate(marchUuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDragUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && objectTranslator.Assignable<Vector3>(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				long marchUuid = Lua.lua_toint64(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				long targetMarchUuid = Lua.lua_toint64(L, 4);
				int startPointId = Lua.xlua_tointeger(L, 5);
				bool isFormation = Lua.lua_toboolean(L, 6);
				worldTroopManager.OnDragUpdate(marchUuid, val, targetMarchUuid, startPointId, isFormation);
				return 0;
			}
			if (num == 5 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && objectTranslator.Assignable<Vector3>(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				long marchUuid2 = Lua.lua_toint64(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				long targetMarchUuid2 = Lua.lua_toint64(L, 4);
				int startPointId2 = Lua.xlua_tointeger(L, 5);
				worldTroopManager.OnDragUpdate(marchUuid2, val2, targetMarchUuid2, startPointId2);
				return 0;
			}
			if (num == 4 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && objectTranslator.Assignable<Vector3>(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)))
			{
				long marchUuid3 = Lua.lua_toint64(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				long targetMarchUuid3 = Lua.lua_toint64(L, 4);
				worldTroopManager.OnDragUpdate(marchUuid3, val3, targetMarchUuid3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroopManager.OnDragUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDestinationType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			long targetMarchUuid = Lua.lua_toint64(L, 3);
			int endPos = Lua.xlua_tointeger(L, 4);
			objectTranslator.Get(L, 5, out MarchTargetType v);
			bool isFormation = Lua.lua_toboolean(L, 6);
			objectTranslator.Get(L, 7, out Vector3 val);
			int tileSize = Lua.xlua_tointeger(L, 8);
			EnumDestinationSignalType destinationType = worldTroopManager.GetDestinationType(marchUuid, targetMarchUuid, endPos, v, isFormation, ref val, ref tileSize);
			objectTranslator.Push(L, destinationType);
			objectTranslator.PushUnityEngineVector3(L, val);
			objectTranslator.UpdateUnityEngineVector3(L, 7, val);
			Lua.xlua_pushinteger(L, tileSize);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTargetType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager obj = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			long targetMarchUuid = Lua.lua_toint64(L, 2);
			int pointId = Lua.xlua_tointeger(L, 3);
			MarchTargetType targetType = obj.GetTargetType(targetMarchUuid, pointId);
			objectTranslator.Push(L, targetType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDragStop(IntPtr L)
	{
		try
		{
			WorldTroopManager worldTroopManager = (WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				long marchUuid = Lua.lua_toint64(L, 2);
				long targetMarchUuid = Lua.lua_toint64(L, 3);
				bool isFormation = Lua.lua_toboolean(L, 4);
				worldTroopManager.OnDragStop(marchUuid, targetMarchUuid, isFormation);
				return 0;
			}
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)))
			{
				long marchUuid2 = Lua.lua_toint64(L, 2);
				long targetMarchUuid2 = Lua.lua_toint64(L, 3);
				worldTroopManager.OnDragStop(marchUuid2, targetMarchUuid2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroopManager.OnDragStop!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnLodChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			object userdata = objectTranslator.GetObject(L, 2, typeof(object));
			worldTroopManager.OnLodChanged(userdata);
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
			WorldTroopManager obj = (WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_CreateBattleVFX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			string prefabPath = Lua.lua_tostring(L, 2);
			float life = (float)Lua.lua_tonumber(L, 3);
			Action<GameObject> onComplete = objectTranslator.GetDelegate<Action<GameObject>>(L, 4);
			worldTroopManager.CreateBattleVFX(prefabPath, life, onComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrawGizmos(IntPtr L)
	{
		try
		{
			((WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDrawGizmos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetModelHeight(IntPtr L)
	{
		try
		{
			WorldTroopManager obj = (WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			float modelHeight = obj.GetModelHeight(marchUuid);
			Lua.lua_pushnumber(L, modelHeight);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateGroupTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			WorldTroop o = worldTroopManager.CreateGroupTroop(march);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurPosAndRotationTroopNum(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			int pointId = Lua.xlua_tointeger(L, 3);
			objectTranslator.Get(L, 4, out Quaternion val);
			int curPosAndRotationTroopNum = worldTroopManager.GetCurPosAndRotationTroopNum(marchUuid, pointId, val);
			Lua.xlua_pushinteger(L, curPosAndRotationTroopNum);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemovePosAndRotationDataByMarchUuid(IntPtr L)
	{
		try
		{
			WorldTroopManager obj = (WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			obj.RemovePosAndRotationDataByMarchUuid(marchUuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowWorldMarchByTypeSignal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			object userData = objectTranslator.GetObject(L, 2, typeof(object));
			worldTroopManager.ShowWorldMarchByTypeSignal(userData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideWorldMarchByTypeSignal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			object userData = objectTranslator.GetObject(L, 2, typeof(object));
			worldTroopManager.HideWorldMarchByTypeSignal(userData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetModelVisibleByMarchTypeForLua(IntPtr L)
	{
		try
		{
			WorldTroopManager obj = (WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int marchType = Lua.xlua_tointeger(L, 2);
			bool visible = Lua.lua_toboolean(L, 3);
			obj.SetModelVisibleByMarchTypeForLua(marchType, visible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanShow(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopManager worldTroopManager = (WorldTroopManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch marchInfo = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			bool value = worldTroopManager.CanShow(marchInfo);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EditorInstance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, WorldTroopManager.EditorInstance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get___DEBUG_LOG__(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, WorldTroopManager.__DEBUG_LOG__);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_time(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, WorldTroopManager.time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_busy(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, WorldTroopManager.busy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TroopCount(IntPtr L)
	{
		try
		{
			WorldTroopManager worldTroopManager = (WorldTroopManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldTroopManager.TroopCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
