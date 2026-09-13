using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BattleColliderUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BattleColliderUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 57, 1, 1);
		Utils.RegisterFunc(L, -4, "GhostPlayerColliderLuaArray", _m_GhostPlayerColliderLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "TryGetGhostPlayerCollideDir", _m_TryGetGhostPlayerCollideDir_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddMonsterColliderTransform", _m_AddMonsterColliderTransform_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddMonsterCollider", _m_AddMonsterCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveMonsterCollider", _m_RemoveMonsterCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitMonsterColliderResultAccess", _m_InitMonsterColliderResultAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnInitMonsterColliderResultAccess", _m_UnInitMonsterColliderResultAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "MonsterColliderLuaArray", _m_MonsterColliderLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearMonsterColliderData", _m_ClearMonsterColliderData_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddObstacleCollider", _m_AddObstacleCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveObstacleCollider", _m_RemoveObstacleCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitObstacleColliderResultAccess", _m_InitObstacleColliderResultAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnInitObstacleColliderResultAccess", _m_UnInitObstacleColliderResultAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "ObstacleColliderLuaArray", _m_ObstacleColliderLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "HandleLuaArray", _m_HandleLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetObstacleColliderData", _m_ResetObstacleColliderData_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearObstacleColliderData", _m_ClearObstacleColliderData_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddPlayerCollider", _m_AddPlayerCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "ChangePlayerCollider", _m_ChangePlayerCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemovePlayerCollider", _m_RemovePlayerCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitPlayerColliderResultAccess", _m_InitPlayerColliderResultAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnInitPlayerColliderResultAccess", _m_UnInitPlayerColliderResultAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "PlayerColliderLuaArray", _m_PlayerColliderLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetPlayerColliderData", _m_ResetPlayerColliderData_xlua_st_);
		Utils.RegisterFunc(L, -4, "TryGetSurfingPlayerCollideZ", _m_TryGetSurfingPlayerCollideZ_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearPlayerColliderData", _m_ClearPlayerColliderData_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddUnitCollider", _m_AddUnitCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveUnitCollider", _m_RemoveUnitCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitUnitColliderResultAccess", _m_InitUnitColliderResultAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnInitUnitColliderResultAccess", _m_UnInitUnitColliderResultAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnitColliderLuaArray", _m_UnitColliderLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearUnitColliderData", _m_ClearUnitColliderData_xlua_st_);
		Utils.RegisterFunc(L, -4, "EnableCollider2D", _m_EnableCollider2D_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsCollider2D", _m_IsCollider2D_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddBullet", _m_AddBullet_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddSphereBullet", _m_AddSphereBullet_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddCapsuleBullet", _m_AddCapsuleBullet_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetBulletDotCD", _m_SetBulletDotCD_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveBullet", _m_RemoveBullet_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitBulletColliderResultAccess", _m_InitBulletColliderResultAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnInitBulletColliderResultAccess", _m_UnInitBulletColliderResultAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "BulletColliderTest", _m_BulletColliderTest_xlua_st_);
		Utils.RegisterFunc(L, -4, "BulletCollider", _m_BulletCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "EnterBattle", _m_EnterBattle_xlua_st_);
		Utils.RegisterFunc(L, -4, "ExitBattle", _m_ExitBattle_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateCollider", _m_UpdateCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "BulletColliderLuaArray", _m_BulletColliderLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddCollider2DAgent", _m_AddCollider2DAgent_xlua_st_);
		Utils.RegisterFunc(L, -4, "RemoveCollider2DAgent", _m_RemoveCollider2DAgent_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearBulletData", _m_ClearBulletData_xlua_st_);
		Utils.RegisterFunc(L, -4, "Dispose", _m_Dispose_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetColliderClosestPoint", _m_GetColliderClosestPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetInverseTransformPoint", _m_GetInverseTransformPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitOverlapSphereNonAllocAccess", _m_InitOverlapSphereNonAllocAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnInitOverlapSphereNonAllocAccess", _m_UnInitOverlapSphereNonAllocAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "OverlapSphereNonAlloc", _m_OverlapSphereNonAlloc_xlua_st_);
		Utils.RegisterFunc(L, -2, "_ColliderList", _g_get__ColliderList);
		Utils.RegisterFunc(L, -1, "_ColliderList", _s_set__ColliderList);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "BattleColliderUtils does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GhostPlayerColliderLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BattleColliderUtils.GhostPlayerColliderLuaArray();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetGhostPlayerCollideDir_xlua_st_(IntPtr L)
	{
		try
		{
			float num = BattleColliderUtils.TryGetGhostPlayerCollideDir(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddMonsterColliderTransform_xlua_st_(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform));
			int objId = Lua.xlua_tointeger(L, 2);
			int layerMask = Lua.xlua_tointeger(L, 3);
			int viewHandle = Lua.xlua_tointeger(L, 4);
			BattleColliderUtils.AddMonsterColliderTransform(transform, objId, layerMask, viewHandle);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddMonsterCollider_xlua_st_(IntPtr L)
	{
		try
		{
			int viewHandle = Lua.xlua_tointeger(L, 1);
			int objId = Lua.xlua_tointeger(L, 2);
			int layerMask = Lua.xlua_tointeger(L, 3);
			BattleColliderUtils.AddMonsterCollider(viewHandle, objId, layerMask);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveMonsterCollider_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.RemoveMonsterCollider(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitMonsterColliderResultAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.InitMonsterColliderResultAccess((LuaArrAccess)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaArrAccess)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInitMonsterColliderResultAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.UnInitMonsterColliderResultAccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MonsterColliderLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BattleColliderUtils.MonsterColliderLuaArray();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearMonsterColliderData_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.ClearMonsterColliderData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddObstacleCollider_xlua_st_(IntPtr L)
	{
		try
		{
			Transform cam = (Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform));
			int viewHandle = Lua.xlua_tointeger(L, 2);
			int objId = Lua.xlua_tointeger(L, 3);
			int layerMask = Lua.xlua_tointeger(L, 4);
			BattleColliderUtils.AddObstacleCollider(cam, viewHandle, objId, layerMask);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveObstacleCollider_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.RemoveObstacleCollider();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitObstacleColliderResultAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.InitObstacleColliderResultAccess((LuaArrAccess)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaArrAccess)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInitObstacleColliderResultAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.UnInitObstacleColliderResultAccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ObstacleColliderLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BattleColliderUtils.ObstacleColliderLuaArray();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			int index = Lua.xlua_tointeger(L, 2);
			int cap = Lua.xlua_tointeger(L, 3);
			objectTranslator.Get(L, 4, out Vector3 val2);
			objectTranslator.Get(L, 5, out Vector3 val3);
			bool value = BattleColliderUtils.HandleLuaArray(val, ref index, cap, val2, val3);
			Lua.lua_pushboolean(L, value);
			Lua.xlua_pushinteger(L, index);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetObstacleColliderData_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.ResetObstacleColliderData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearObstacleColliderData_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.ClearObstacleColliderData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddPlayerCollider_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<GameObject>(L, 4))
			{
				int viewHandle = Lua.xlua_tointeger(L, 1);
				int objId = Lua.xlua_tointeger(L, 2);
				int layerMask = Lua.xlua_tointeger(L, 3);
				GameObject colliderRoot = (GameObject)objectTranslator.GetObject(L, 4, typeof(GameObject));
				BattleColliderUtils.AddPlayerCollider(viewHandle, objId, layerMask, colliderRoot);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int viewHandle2 = Lua.xlua_tointeger(L, 1);
				int objId2 = Lua.xlua_tointeger(L, 2);
				int layerMask2 = Lua.xlua_tointeger(L, 3);
				BattleColliderUtils.AddPlayerCollider(viewHandle2, objId2, layerMask2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BattleColliderUtils.AddPlayerCollider!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangePlayerCollider_xlua_st_(IntPtr L)
	{
		try
		{
			int objId = Lua.xlua_tointeger(L, 1);
			float heightScale = (float)Lua.lua_tonumber(L, 2);
			BattleColliderUtils.ChangePlayerCollider(objId, heightScale);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemovePlayerCollider_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.RemovePlayerCollider();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitPlayerColliderResultAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.InitPlayerColliderResultAccess((LuaArrAccess)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaArrAccess)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInitPlayerColliderResultAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.UnInitPlayerColliderResultAccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayerColliderLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BattleColliderUtils.PlayerColliderLuaArray();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetPlayerColliderData_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.ResetPlayerColliderData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetSurfingPlayerCollideZ_xlua_st_(IntPtr L)
	{
		try
		{
			float num = BattleColliderUtils.TryGetSurfingPlayerCollideZ(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearPlayerColliderData_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.ClearPlayerColliderData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddUnitCollider_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<GameObject>(L, 4))
			{
				int viewHandle = Lua.xlua_tointeger(L, 1);
				int objId = Lua.xlua_tointeger(L, 2);
				int layerMask = Lua.xlua_tointeger(L, 3);
				GameObject colliderRoot = (GameObject)objectTranslator.GetObject(L, 4, typeof(GameObject));
				BattleColliderUtils.AddUnitCollider(viewHandle, objId, layerMask, colliderRoot);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int viewHandle2 = Lua.xlua_tointeger(L, 1);
				int objId2 = Lua.xlua_tointeger(L, 2);
				int layerMask2 = Lua.xlua_tointeger(L, 3);
				BattleColliderUtils.AddUnitCollider(viewHandle2, objId2, layerMask2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BattleColliderUtils.AddUnitCollider!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveUnitCollider_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.RemoveUnitCollider();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitUnitColliderResultAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.InitUnitColliderResultAccess((LuaArrAccess)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaArrAccess)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInitUnitColliderResultAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.UnInitUnitColliderResultAccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnitColliderLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BattleColliderUtils.UnitColliderLuaArray();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearUnitColliderData_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.ClearUnitColliderData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnableCollider2D_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.EnableCollider2D(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCollider2D_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BattleColliderUtils.IsCollider2D();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddBullet_xlua_st_(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform));
			long uid = Lua.lua_toint64(L, 2);
			float radius = (float)Lua.lua_tonumber(L, 3);
			int targetLayerMask = Lua.xlua_tointeger(L, 4);
			BattleColliderUtils.AddBullet(transform, uid, radius, targetLayerMask);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddSphereBullet_xlua_st_(IntPtr L)
	{
		try
		{
			int viewHandle = Lua.xlua_tointeger(L, 1);
			long uid = Lua.lua_toint64(L, 2);
			float radius = (float)Lua.lua_tonumber(L, 3);
			int targetLayerMask = Lua.xlua_tointeger(L, 4);
			BattleColliderUtils.AddSphereBullet(viewHandle, uid, radius, targetLayerMask);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddCapsuleBullet_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 10 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				int viewHandle = Lua.xlua_tointeger(L, 1);
				long uid = Lua.lua_toint64(L, 2);
				float radius = (float)Lua.lua_tonumber(L, 3);
				int targetLayerMask = Lua.xlua_tointeger(L, 4);
				float startOffsetX = (float)Lua.lua_tonumber(L, 5);
				float startOffsetY = (float)Lua.lua_tonumber(L, 6);
				float startOffsetZ = (float)Lua.lua_tonumber(L, 7);
				float endOffsetX = (float)Lua.lua_tonumber(L, 8);
				float endOffsetY = (float)Lua.lua_tonumber(L, 9);
				float endOffsetZ = (float)Lua.lua_tonumber(L, 10);
				BattleColliderUtils.AddCapsuleBullet(viewHandle, uid, radius, targetLayerMask, startOffsetX, startOffsetY, startOffsetZ, endOffsetX, endOffsetY, endOffsetZ);
				return 0;
			}
			if (num == 10 && objectTranslator.Assignable<Transform>(L, 1) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				Transform transform = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				long uid2 = Lua.lua_toint64(L, 2);
				float radius2 = (float)Lua.lua_tonumber(L, 3);
				int targetLayerMask2 = Lua.xlua_tointeger(L, 4);
				float startOffsetX2 = (float)Lua.lua_tonumber(L, 5);
				float startOffsetY2 = (float)Lua.lua_tonumber(L, 6);
				float startOffsetZ2 = (float)Lua.lua_tonumber(L, 7);
				float endOffsetX2 = (float)Lua.lua_tonumber(L, 8);
				float endOffsetY2 = (float)Lua.lua_tonumber(L, 9);
				float endOffsetZ2 = (float)Lua.lua_tonumber(L, 10);
				BattleColliderUtils.AddCapsuleBullet(transform, uid2, radius2, targetLayerMask2, startOffsetX2, startOffsetY2, startOffsetZ2, endOffsetX2, endOffsetY2, endOffsetZ2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BattleColliderUtils.AddCapsuleBullet!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBulletDotCD_xlua_st_(IntPtr L)
	{
		try
		{
			long uid = Lua.lua_toint64(L, 1);
			float dotCD = (float)Lua.lua_tonumber(L, 2);
			BattleColliderUtils.SetBulletDotCD(uid, dotCD);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveBullet_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.RemoveBullet(Lua.lua_toint64(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitBulletColliderResultAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.InitBulletColliderResultAccess((LuaArrAccess)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaArrAccess)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInitBulletColliderResultAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.UnInitBulletColliderResultAccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BulletColliderTest_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BattleColliderUtils.BulletColliderTest();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BulletCollider_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LuaTable o = BattleColliderUtils.BulletCollider((float)Lua.lua_tonumber(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnterBattle_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
			{
				BattleColliderUtils.EnterBattle(Lua.lua_toboolean(L, 1));
				return 0;
			}
			if (num == 0)
			{
				BattleColliderUtils.EnterBattle();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BattleColliderUtils.EnterBattle!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExitBattle_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.ExitBattle();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateCollider_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.UpdateCollider();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BulletColliderLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BattleColliderUtils.BulletColliderLuaArray((float)Lua.lua_tonumber(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddCollider2DAgent_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int uid = Lua.xlua_tointeger(L, 1);
			int colliderId = Lua.xlua_tointeger(L, 2);
			Collider unityCollider = (Collider)objectTranslator.GetObject(L, 3, typeof(Collider));
			BattleColliderUtils.AddCollider2DAgent(uid, colliderId, unityCollider);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveCollider2DAgent_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.RemoveCollider2DAgent(Lua.lua_toint64(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearBulletData_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.ClearBulletData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetColliderClosestPoint_xlua_st_(IntPtr L)
	{
		try
		{
			Collider collider = (Collider)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Collider));
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			BattleColliderUtils.GetColliderClosestPoint(collider, x, y, z, out var pointX, out var pointY, out var pointZ);
			Lua.lua_pushnumber(L, pointX);
			Lua.lua_pushnumber(L, pointY);
			Lua.lua_pushnumber(L, pointZ);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInverseTransformPoint_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			Vector3 inverseTransformPoint = BattleColliderUtils.GetInverseTransformPoint(transform, x, y, z);
			objectTranslator.PushUnityEngineVector3(L, inverseTransformPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitOverlapSphereNonAllocAccess_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int arrayCount = Lua.xlua_tointeger(L, 1);
			LuaArrAccess luaArrAccess = (LuaArrAccess)objectTranslator.GetObject(L, 2, typeof(LuaArrAccess));
			LuaArrAccess resultLuaAccess = (LuaArrAccess)objectTranslator.GetObject(L, 3, typeof(LuaArrAccess));
			BattleColliderUtils.InitOverlapSphereNonAllocAccess(arrayCount, luaArrAccess, resultLuaAccess);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInitOverlapSphereNonAllocAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BattleColliderUtils.UnInitOverlapSphereNonAllocAccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverlapSphereNonAlloc_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
			{
				BattleColliderUtils.OverlapSphereNonAlloc(Lua.lua_toboolean(L, 1));
				return 0;
			}
			if (num == 0)
			{
				BattleColliderUtils.OverlapSphereNonAlloc();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BattleColliderUtils.OverlapSphereNonAlloc!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__ColliderList(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, BattleColliderUtils._ColliderList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__ColliderList(IntPtr L)
	{
		try
		{
			BattleColliderUtils._ColliderList = (int[])ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(int[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
