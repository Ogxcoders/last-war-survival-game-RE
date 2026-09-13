using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEnginePhysicsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Physics);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 36, 17, 16);
		Utils.RegisterFunc(L, -4, "IgnoreCollision", _m_IgnoreCollision_xlua_st_);
		Utils.RegisterFunc(L, -4, "IgnoreLayerCollision", _m_IgnoreLayerCollision_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetIgnoreLayerCollision", _m_GetIgnoreLayerCollision_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetIgnoreCollision", _m_GetIgnoreCollision_xlua_st_);
		Utils.RegisterFunc(L, -4, "Raycast", _m_Raycast_xlua_st_);
		Utils.RegisterFunc(L, -4, "Linecast", _m_Linecast_xlua_st_);
		Utils.RegisterFunc(L, -4, "CapsuleCast", _m_CapsuleCast_xlua_st_);
		Utils.RegisterFunc(L, -4, "SphereCast", _m_SphereCast_xlua_st_);
		Utils.RegisterFunc(L, -4, "BoxCast", _m_BoxCast_xlua_st_);
		Utils.RegisterFunc(L, -4, "RaycastAll", _m_RaycastAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "RaycastNonAlloc", _m_RaycastNonAlloc_xlua_st_);
		Utils.RegisterFunc(L, -4, "CapsuleCastAll", _m_CapsuleCastAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "SphereCastAll", _m_SphereCastAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "OverlapCapsule", _m_OverlapCapsule_xlua_st_);
		Utils.RegisterFunc(L, -4, "OverlapSphere", _m_OverlapSphere_xlua_st_);
		Utils.RegisterFunc(L, -4, "Simulate", _m_Simulate_xlua_st_);
		Utils.RegisterFunc(L, -4, "SyncTransforms", _m_SyncTransforms_xlua_st_);
		Utils.RegisterFunc(L, -4, "ComputePenetration", _m_ComputePenetration_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClosestPoint", _m_ClosestPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "OverlapSphereNonAlloc", _m_OverlapSphereNonAlloc_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckSphere", _m_CheckSphere_xlua_st_);
		Utils.RegisterFunc(L, -4, "CapsuleCastNonAlloc", _m_CapsuleCastNonAlloc_xlua_st_);
		Utils.RegisterFunc(L, -4, "SphereCastNonAlloc", _m_SphereCastNonAlloc_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckCapsule", _m_CheckCapsule_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckBox", _m_CheckBox_xlua_st_);
		Utils.RegisterFunc(L, -4, "OverlapBox", _m_OverlapBox_xlua_st_);
		Utils.RegisterFunc(L, -4, "OverlapBoxNonAlloc", _m_OverlapBoxNonAlloc_xlua_st_);
		Utils.RegisterFunc(L, -4, "BoxCastNonAlloc", _m_BoxCastNonAlloc_xlua_st_);
		Utils.RegisterFunc(L, -4, "BoxCastAll", _m_BoxCastAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "OverlapCapsuleNonAlloc", _m_OverlapCapsuleNonAlloc_xlua_st_);
		Utils.RegisterFunc(L, -4, "RebuildBroadphaseRegions", _m_RebuildBroadphaseRegions_xlua_st_);
		Utils.RegisterFunc(L, -4, "BakeMesh", _m_BakeMesh_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "IgnoreRaycastLayer", 4);
		Utils.RegisterObject(L, translator, -4, "DefaultRaycastLayers", -5);
		Utils.RegisterObject(L, translator, -4, "AllLayers", -1);
		Utils.RegisterFunc(L, -2, "gravity", _g_get_gravity);
		Utils.RegisterFunc(L, -2, "defaultContactOffset", _g_get_defaultContactOffset);
		Utils.RegisterFunc(L, -2, "sleepThreshold", _g_get_sleepThreshold);
		Utils.RegisterFunc(L, -2, "queriesHitTriggers", _g_get_queriesHitTriggers);
		Utils.RegisterFunc(L, -2, "queriesHitBackfaces", _g_get_queriesHitBackfaces);
		Utils.RegisterFunc(L, -2, "bounceThreshold", _g_get_bounceThreshold);
		Utils.RegisterFunc(L, -2, "defaultSolverIterations", _g_get_defaultSolverIterations);
		Utils.RegisterFunc(L, -2, "defaultSolverVelocityIterations", _g_get_defaultSolverVelocityIterations);
		Utils.RegisterFunc(L, -2, "defaultMaxAngularSpeed", _g_get_defaultMaxAngularSpeed);
		Utils.RegisterFunc(L, -2, "defaultPhysicsScene", _g_get_defaultPhysicsScene);
		Utils.RegisterFunc(L, -2, "autoSimulation", _g_get_autoSimulation);
		Utils.RegisterFunc(L, -2, "autoSyncTransforms", _g_get_autoSyncTransforms);
		Utils.RegisterFunc(L, -2, "reuseCollisionCallbacks", _g_get_reuseCollisionCallbacks);
		Utils.RegisterFunc(L, -2, "interCollisionDistance", _g_get_interCollisionDistance);
		Utils.RegisterFunc(L, -2, "interCollisionStiffness", _g_get_interCollisionStiffness);
		Utils.RegisterFunc(L, -2, "interCollisionSettingsToggle", _g_get_interCollisionSettingsToggle);
		Utils.RegisterFunc(L, -2, "clothGravity", _g_get_clothGravity);
		Utils.RegisterFunc(L, -1, "gravity", _s_set_gravity);
		Utils.RegisterFunc(L, -1, "defaultContactOffset", _s_set_defaultContactOffset);
		Utils.RegisterFunc(L, -1, "sleepThreshold", _s_set_sleepThreshold);
		Utils.RegisterFunc(L, -1, "queriesHitTriggers", _s_set_queriesHitTriggers);
		Utils.RegisterFunc(L, -1, "queriesHitBackfaces", _s_set_queriesHitBackfaces);
		Utils.RegisterFunc(L, -1, "bounceThreshold", _s_set_bounceThreshold);
		Utils.RegisterFunc(L, -1, "defaultSolverIterations", _s_set_defaultSolverIterations);
		Utils.RegisterFunc(L, -1, "defaultSolverVelocityIterations", _s_set_defaultSolverVelocityIterations);
		Utils.RegisterFunc(L, -1, "defaultMaxAngularSpeed", _s_set_defaultMaxAngularSpeed);
		Utils.RegisterFunc(L, -1, "autoSimulation", _s_set_autoSimulation);
		Utils.RegisterFunc(L, -1, "autoSyncTransforms", _s_set_autoSyncTransforms);
		Utils.RegisterFunc(L, -1, "reuseCollisionCallbacks", _s_set_reuseCollisionCallbacks);
		Utils.RegisterFunc(L, -1, "interCollisionDistance", _s_set_interCollisionDistance);
		Utils.RegisterFunc(L, -1, "interCollisionStiffness", _s_set_interCollisionStiffness);
		Utils.RegisterFunc(L, -1, "interCollisionSettingsToggle", _s_set_interCollisionSettingsToggle);
		Utils.RegisterFunc(L, -1, "clothGravity", _s_set_clothGravity);
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
				Physics o = new Physics();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IgnoreCollision_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Collider>(L, 1) && objectTranslator.Assignable<Collider>(L, 2))
			{
				Collider collider = (Collider)objectTranslator.GetObject(L, 1, typeof(Collider));
				Collider collider2 = (Collider)objectTranslator.GetObject(L, 2, typeof(Collider));
				Physics.IgnoreCollision(collider, collider2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Collider>(L, 1) && objectTranslator.Assignable<Collider>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Collider collider3 = (Collider)objectTranslator.GetObject(L, 1, typeof(Collider));
				Collider collider4 = (Collider)objectTranslator.GetObject(L, 2, typeof(Collider));
				bool ignore = Lua.lua_toboolean(L, 3);
				Physics.IgnoreCollision(collider3, collider4, ignore);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.IgnoreCollision!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IgnoreLayerCollision_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int layer = Lua.xlua_tointeger(L, 1);
				int layer2 = Lua.xlua_tointeger(L, 2);
				Physics.IgnoreLayerCollision(layer, layer2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int layer3 = Lua.xlua_tointeger(L, 1);
				int layer4 = Lua.xlua_tointeger(L, 2);
				bool ignore = Lua.lua_toboolean(L, 3);
				Physics.IgnoreLayerCollision(layer3, layer4, ignore);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.IgnoreLayerCollision!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIgnoreLayerCollision_xlua_st_(IntPtr L)
	{
		try
		{
			int layer = Lua.xlua_tointeger(L, 1);
			int layer2 = Lua.xlua_tointeger(L, 2);
			bool ignoreLayerCollision = Physics.GetIgnoreLayerCollision(layer, layer2);
			Lua.lua_pushboolean(L, ignoreLayerCollision);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIgnoreCollision_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Collider collider = (Collider)objectTranslator.GetObject(L, 1, typeof(Collider));
			Collider collider2 = (Collider)objectTranslator.GetObject(L, 2, typeof(Collider));
			bool ignoreCollision = Physics.GetIgnoreCollision(collider, collider2);
			Lua.lua_pushboolean(L, ignoreCollision);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Raycast_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<Ray>(L, 1))
			{
				objectTranslator.Get(L, 1, out Ray val);
				bool value = Physics.Raycast(val);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Ray val2);
				float maxDistance = (float)Lua.lua_tonumber(L, 2);
				bool value2 = Physics.Raycast(val2, maxDistance);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<Ray>(L, 1))
			{
				objectTranslator.Get(L, 1, out Ray val3);
				RaycastHit hitInfo;
				bool value3 = Physics.Raycast(val3, out hitInfo);
				Lua.lua_pushboolean(L, value3);
				objectTranslator.Push(L, hitInfo);
				return 2;
			}
			if (num == 3 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Ray val4);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 2);
				int layerMask = Lua.xlua_tointeger(L, 3);
				bool value4 = Physics.Raycast(val4, maxDistance2, layerMask);
				Lua.lua_pushboolean(L, value4);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Ray val5);
				float maxDistance3 = (float)Lua.lua_tonumber(L, 2);
				RaycastHit hitInfo2;
				bool value5 = Physics.Raycast(val5, out hitInfo2, maxDistance3);
				Lua.lua_pushboolean(L, value5);
				objectTranslator.Push(L, hitInfo2);
				return 2;
			}
			if (num == 3 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Ray val6);
				float maxDistance4 = (float)Lua.lua_tonumber(L, 2);
				int layerMask2 = Lua.xlua_tointeger(L, 3);
				RaycastHit hitInfo3;
				bool value6 = Physics.Raycast(val6, out hitInfo3, maxDistance4, layerMask2);
				Lua.lua_pushboolean(L, value6);
				objectTranslator.Push(L, hitInfo3);
				return 2;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val7);
				objectTranslator.Get(L, 2, out Vector3 val8);
				bool value7 = Physics.Raycast(val7, val8);
				Lua.lua_pushboolean(L, value7);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val9);
				objectTranslator.Get(L, 2, out Vector3 val10);
				float maxDistance5 = (float)Lua.lua_tonumber(L, 3);
				bool value8 = Physics.Raycast(val9, val10, maxDistance5);
				Lua.lua_pushboolean(L, value8);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val11);
				objectTranslator.Get(L, 2, out Vector3 val12);
				RaycastHit hitInfo4;
				bool value9 = Physics.Raycast(val11, val12, out hitInfo4);
				Lua.lua_pushboolean(L, value9);
				objectTranslator.Push(L, hitInfo4);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val13);
				objectTranslator.Get(L, 2, out Vector3 val14);
				float maxDistance6 = (float)Lua.lua_tonumber(L, 3);
				int layerMask3 = Lua.xlua_tointeger(L, 4);
				bool value10 = Physics.Raycast(val13, val14, maxDistance6, layerMask3);
				Lua.lua_pushboolean(L, value10);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val15);
				objectTranslator.Get(L, 2, out Vector3 val16);
				float maxDistance7 = (float)Lua.lua_tonumber(L, 3);
				RaycastHit hitInfo5;
				bool value11 = Physics.Raycast(val15, val16, out hitInfo5, maxDistance7);
				Lua.lua_pushboolean(L, value11);
				objectTranslator.Push(L, hitInfo5);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 4))
			{
				objectTranslator.Get(L, 1, out Ray val17);
				float maxDistance8 = (float)Lua.lua_tonumber(L, 2);
				int layerMask4 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out QueryTriggerInteraction val18);
				bool value12 = Physics.Raycast(val17, maxDistance8, layerMask4, val18);
				Lua.lua_pushboolean(L, value12);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val19);
				objectTranslator.Get(L, 2, out Vector3 val20);
				float maxDistance9 = (float)Lua.lua_tonumber(L, 3);
				int layerMask5 = Lua.xlua_tointeger(L, 4);
				RaycastHit hitInfo6;
				bool value13 = Physics.Raycast(val19, val20, out hitInfo6, maxDistance9, layerMask5);
				Lua.lua_pushboolean(L, value13);
				objectTranslator.Push(L, hitInfo6);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 4))
			{
				objectTranslator.Get(L, 1, out Ray val21);
				float maxDistance10 = (float)Lua.lua_tonumber(L, 2);
				int layerMask6 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out QueryTriggerInteraction val22);
				RaycastHit hitInfo7;
				bool value14 = Physics.Raycast(val21, out hitInfo7, maxDistance10, layerMask6, val22);
				Lua.lua_pushboolean(L, value14);
				objectTranslator.Push(L, hitInfo7);
				return 2;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val23);
				objectTranslator.Get(L, 2, out Vector3 val24);
				float maxDistance11 = (float)Lua.lua_tonumber(L, 3);
				int layerMask7 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val25);
				bool value15 = Physics.Raycast(val23, val24, maxDistance11, layerMask7, val25);
				Lua.lua_pushboolean(L, value15);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val26);
				objectTranslator.Get(L, 2, out Vector3 val27);
				float maxDistance12 = (float)Lua.lua_tonumber(L, 3);
				int layerMask8 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val28);
				RaycastHit hitInfo8;
				bool value16 = Physics.Raycast(val26, val27, out hitInfo8, maxDistance12, layerMask8, val28);
				Lua.lua_pushboolean(L, value16);
				objectTranslator.Push(L, hitInfo8);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.Raycast!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Linecast_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				bool value = Physics.Linecast(val, val2);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				int layerMask = Lua.xlua_tointeger(L, 3);
				bool value2 = Physics.Linecast(val3, val4, layerMask);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val5);
				objectTranslator.Get(L, 2, out Vector3 val6);
				RaycastHit hitInfo;
				bool value3 = Physics.Linecast(val5, val6, out hitInfo);
				Lua.lua_pushboolean(L, value3);
				objectTranslator.Push(L, hitInfo);
				return 2;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val7);
				objectTranslator.Get(L, 2, out Vector3 val8);
				int layerMask2 = Lua.xlua_tointeger(L, 3);
				RaycastHit hitInfo2;
				bool value4 = Physics.Linecast(val7, val8, out hitInfo2, layerMask2);
				Lua.lua_pushboolean(L, value4);
				objectTranslator.Push(L, hitInfo2);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val9);
				objectTranslator.Get(L, 2, out Vector3 val10);
				int layerMask3 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out QueryTriggerInteraction val11);
				bool value5 = Physics.Linecast(val9, val10, layerMask3, val11);
				Lua.lua_pushboolean(L, value5);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val12);
				objectTranslator.Get(L, 2, out Vector3 val13);
				int layerMask4 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out QueryTriggerInteraction val14);
				RaycastHit hitInfo3;
				bool value6 = Physics.Linecast(val12, val13, out hitInfo3, layerMask4, val14);
				Lua.lua_pushboolean(L, value6);
				objectTranslator.Push(L, hitInfo3);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.Linecast!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CapsuleCast_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				float radius = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val3);
				bool value = Physics.CapsuleCast(val, val2, radius, val3);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val4);
				objectTranslator.Get(L, 2, out Vector3 val5);
				float radius2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val6);
				float maxDistance = (float)Lua.lua_tonumber(L, 5);
				bool value2 = Physics.CapsuleCast(val4, val5, radius2, val6, maxDistance);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val7);
				objectTranslator.Get(L, 2, out Vector3 val8);
				float radius3 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val9);
				RaycastHit hitInfo;
				bool value3 = Physics.CapsuleCast(val7, val8, radius3, val9, out hitInfo);
				Lua.lua_pushboolean(L, value3);
				objectTranslator.Push(L, hitInfo);
				return 2;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val10);
				objectTranslator.Get(L, 2, out Vector3 val11);
				float radius4 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val12);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 5);
				int layerMask = Lua.xlua_tointeger(L, 6);
				bool value4 = Physics.CapsuleCast(val10, val11, radius4, val12, maxDistance2, layerMask);
				Lua.lua_pushboolean(L, value4);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val13);
				objectTranslator.Get(L, 2, out Vector3 val14);
				float radius5 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val15);
				float maxDistance3 = (float)Lua.lua_tonumber(L, 5);
				RaycastHit hitInfo2;
				bool value5 = Physics.CapsuleCast(val13, val14, radius5, val15, out hitInfo2, maxDistance3);
				Lua.lua_pushboolean(L, value5);
				objectTranslator.Push(L, hitInfo2);
				return 2;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val16);
				objectTranslator.Get(L, 2, out Vector3 val17);
				float radius6 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val18);
				float maxDistance4 = (float)Lua.lua_tonumber(L, 5);
				int layerMask2 = Lua.xlua_tointeger(L, 6);
				RaycastHit hitInfo3;
				bool value6 = Physics.CapsuleCast(val16, val17, radius6, val18, out hitInfo3, maxDistance4, layerMask2);
				Lua.lua_pushboolean(L, value6);
				objectTranslator.Push(L, hitInfo3);
				return 2;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 7))
			{
				objectTranslator.Get(L, 1, out Vector3 val19);
				objectTranslator.Get(L, 2, out Vector3 val20);
				float radius7 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val21);
				float maxDistance5 = (float)Lua.lua_tonumber(L, 5);
				int layerMask3 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out QueryTriggerInteraction val22);
				bool value7 = Physics.CapsuleCast(val19, val20, radius7, val21, maxDistance5, layerMask3, val22);
				Lua.lua_pushboolean(L, value7);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 7))
			{
				objectTranslator.Get(L, 1, out Vector3 val23);
				objectTranslator.Get(L, 2, out Vector3 val24);
				float radius8 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val25);
				float maxDistance6 = (float)Lua.lua_tonumber(L, 5);
				int layerMask4 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out QueryTriggerInteraction val26);
				RaycastHit hitInfo4;
				bool value8 = Physics.CapsuleCast(val23, val24, radius8, val25, out hitInfo4, maxDistance6, layerMask4, val26);
				Lua.lua_pushboolean(L, value8);
				objectTranslator.Push(L, hitInfo4);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.CapsuleCast!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SphereCast_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Ray val);
				float radius = (float)Lua.lua_tonumber(L, 2);
				bool value = Physics.SphereCast(val, radius);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Ray val2);
				float radius2 = (float)Lua.lua_tonumber(L, 2);
				float maxDistance = (float)Lua.lua_tonumber(L, 3);
				bool value2 = Physics.SphereCast(val2, radius2, maxDistance);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Ray val3);
				float radius3 = (float)Lua.lua_tonumber(L, 2);
				RaycastHit hitInfo;
				bool value3 = Physics.SphereCast(val3, radius3, out hitInfo);
				Lua.lua_pushboolean(L, value3);
				objectTranslator.Push(L, hitInfo);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Ray val4);
				float radius4 = (float)Lua.lua_tonumber(L, 2);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 3);
				int layerMask = Lua.xlua_tointeger(L, 4);
				bool value4 = Physics.SphereCast(val4, radius4, maxDistance2, layerMask);
				Lua.lua_pushboolean(L, value4);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Ray val5);
				float radius5 = (float)Lua.lua_tonumber(L, 2);
				float maxDistance3 = (float)Lua.lua_tonumber(L, 3);
				RaycastHit hitInfo2;
				bool value5 = Physics.SphereCast(val5, radius5, out hitInfo2, maxDistance3);
				Lua.lua_pushboolean(L, value5);
				objectTranslator.Push(L, hitInfo2);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Ray val6);
				float radius6 = (float)Lua.lua_tonumber(L, 2);
				float maxDistance4 = (float)Lua.lua_tonumber(L, 3);
				int layerMask2 = Lua.xlua_tointeger(L, 4);
				RaycastHit hitInfo3;
				bool value6 = Physics.SphereCast(val6, radius6, out hitInfo3, maxDistance4, layerMask2);
				Lua.lua_pushboolean(L, value6);
				objectTranslator.Push(L, hitInfo3);
				return 2;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val7);
				float radius7 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val8);
				RaycastHit hitInfo4;
				bool value7 = Physics.SphereCast(val7, radius7, val8, out hitInfo4);
				Lua.lua_pushboolean(L, value7);
				objectTranslator.Push(L, hitInfo4);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val9);
				float radius8 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val10);
				float maxDistance5 = (float)Lua.lua_tonumber(L, 4);
				RaycastHit hitInfo5;
				bool value8 = Physics.SphereCast(val9, radius8, val10, out hitInfo5, maxDistance5);
				Lua.lua_pushboolean(L, value8);
				objectTranslator.Push(L, hitInfo5);
				return 2;
			}
			if (num == 5 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Ray val11);
				float radius9 = (float)Lua.lua_tonumber(L, 2);
				float maxDistance6 = (float)Lua.lua_tonumber(L, 3);
				int layerMask3 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val12);
				bool value9 = Physics.SphereCast(val11, radius9, maxDistance6, layerMask3, val12);
				Lua.lua_pushboolean(L, value9);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val13);
				float radius10 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val14);
				float maxDistance7 = (float)Lua.lua_tonumber(L, 4);
				int layerMask4 = Lua.xlua_tointeger(L, 5);
				RaycastHit hitInfo6;
				bool value10 = Physics.SphereCast(val13, radius10, val14, out hitInfo6, maxDistance7, layerMask4);
				Lua.lua_pushboolean(L, value10);
				objectTranslator.Push(L, hitInfo6);
				return 2;
			}
			if (num == 5 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Ray val15);
				float radius11 = (float)Lua.lua_tonumber(L, 2);
				float maxDistance8 = (float)Lua.lua_tonumber(L, 3);
				int layerMask5 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val16);
				RaycastHit hitInfo7;
				bool value11 = Physics.SphereCast(val15, radius11, out hitInfo7, maxDistance8, layerMask5, val16);
				Lua.lua_pushboolean(L, value11);
				objectTranslator.Push(L, hitInfo7);
				return 2;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val17);
				float radius12 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val18);
				float maxDistance9 = (float)Lua.lua_tonumber(L, 4);
				int layerMask6 = Lua.xlua_tointeger(L, 5);
				objectTranslator.Get(L, 6, out QueryTriggerInteraction val19);
				RaycastHit hitInfo8;
				bool value12 = Physics.SphereCast(val17, radius12, val18, out hitInfo8, maxDistance9, layerMask6, val19);
				Lua.lua_pushboolean(L, value12);
				objectTranslator.Push(L, hitInfo8);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.SphereCast!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BoxCast_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				bool value = Physics.BoxCast(val, val2, val3);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val4);
				objectTranslator.Get(L, 2, out Vector3 val5);
				objectTranslator.Get(L, 3, out Vector3 val6);
				RaycastHit hitInfo;
				bool value2 = Physics.BoxCast(val4, val5, val6, out hitInfo);
				Lua.lua_pushboolean(L, value2);
				objectTranslator.Push(L, hitInfo);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val7);
				objectTranslator.Get(L, 2, out Vector3 val8);
				objectTranslator.Get(L, 3, out Vector3 val9);
				objectTranslator.Get(L, 4, out Quaternion val10);
				bool value3 = Physics.BoxCast(val7, val8, val9, val10);
				Lua.lua_pushboolean(L, value3);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val11);
				objectTranslator.Get(L, 2, out Vector3 val12);
				objectTranslator.Get(L, 3, out Vector3 val13);
				objectTranslator.Get(L, 4, out Quaternion val14);
				float maxDistance = (float)Lua.lua_tonumber(L, 5);
				bool value4 = Physics.BoxCast(val11, val12, val13, val14, maxDistance);
				Lua.lua_pushboolean(L, value4);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val15);
				objectTranslator.Get(L, 2, out Vector3 val16);
				objectTranslator.Get(L, 3, out Vector3 val17);
				objectTranslator.Get(L, 4, out Quaternion val18);
				RaycastHit hitInfo2;
				bool value5 = Physics.BoxCast(val15, val16, val17, out hitInfo2, val18);
				Lua.lua_pushboolean(L, value5);
				objectTranslator.Push(L, hitInfo2);
				return 2;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val19);
				objectTranslator.Get(L, 2, out Vector3 val20);
				objectTranslator.Get(L, 3, out Vector3 val21);
				objectTranslator.Get(L, 4, out Quaternion val22);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 5);
				int layerMask = Lua.xlua_tointeger(L, 6);
				bool value6 = Physics.BoxCast(val19, val20, val21, val22, maxDistance2, layerMask);
				Lua.lua_pushboolean(L, value6);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val23);
				objectTranslator.Get(L, 2, out Vector3 val24);
				objectTranslator.Get(L, 3, out Vector3 val25);
				objectTranslator.Get(L, 4, out Quaternion val26);
				float maxDistance3 = (float)Lua.lua_tonumber(L, 5);
				RaycastHit hitInfo3;
				bool value7 = Physics.BoxCast(val23, val24, val25, out hitInfo3, val26, maxDistance3);
				Lua.lua_pushboolean(L, value7);
				objectTranslator.Push(L, hitInfo3);
				return 2;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val27);
				objectTranslator.Get(L, 2, out Vector3 val28);
				objectTranslator.Get(L, 3, out Vector3 val29);
				objectTranslator.Get(L, 4, out Quaternion val30);
				float maxDistance4 = (float)Lua.lua_tonumber(L, 5);
				int layerMask2 = Lua.xlua_tointeger(L, 6);
				RaycastHit hitInfo4;
				bool value8 = Physics.BoxCast(val27, val28, val29, out hitInfo4, val30, maxDistance4, layerMask2);
				Lua.lua_pushboolean(L, value8);
				objectTranslator.Push(L, hitInfo4);
				return 2;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 7))
			{
				objectTranslator.Get(L, 1, out Vector3 val31);
				objectTranslator.Get(L, 2, out Vector3 val32);
				objectTranslator.Get(L, 3, out Vector3 val33);
				objectTranslator.Get(L, 4, out Quaternion val34);
				float maxDistance5 = (float)Lua.lua_tonumber(L, 5);
				int layerMask3 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out QueryTriggerInteraction val35);
				bool value9 = Physics.BoxCast(val31, val32, val33, val34, maxDistance5, layerMask3, val35);
				Lua.lua_pushboolean(L, value9);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 7))
			{
				objectTranslator.Get(L, 1, out Vector3 val36);
				objectTranslator.Get(L, 2, out Vector3 val37);
				objectTranslator.Get(L, 3, out Vector3 val38);
				objectTranslator.Get(L, 4, out Quaternion val39);
				float maxDistance6 = (float)Lua.lua_tonumber(L, 5);
				int layerMask4 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out QueryTriggerInteraction val40);
				RaycastHit hitInfo5;
				bool value10 = Physics.BoxCast(val36, val37, val38, out hitInfo5, val39, maxDistance6, layerMask4, val40);
				Lua.lua_pushboolean(L, value10);
				objectTranslator.Push(L, hitInfo5);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.BoxCast!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RaycastAll_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<Ray>(L, 1))
			{
				objectTranslator.Get(L, 1, out Ray val);
				RaycastHit[] o = Physics.RaycastAll(val);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Ray val2);
				float maxDistance = (float)Lua.lua_tonumber(L, 2);
				RaycastHit[] o2 = Physics.RaycastAll(val2, maxDistance);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Ray val3);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 2);
				int layerMask = Lua.xlua_tointeger(L, 3);
				RaycastHit[] o3 = Physics.RaycastAll(val3, maxDistance2, layerMask);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val4);
				objectTranslator.Get(L, 2, out Vector3 val5);
				RaycastHit[] o4 = Physics.RaycastAll(val4, val5);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val6);
				objectTranslator.Get(L, 2, out Vector3 val7);
				float maxDistance3 = (float)Lua.lua_tonumber(L, 3);
				RaycastHit[] o5 = Physics.RaycastAll(val6, val7, maxDistance3);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val8);
				objectTranslator.Get(L, 2, out Vector3 val9);
				float maxDistance4 = (float)Lua.lua_tonumber(L, 3);
				int layerMask2 = Lua.xlua_tointeger(L, 4);
				RaycastHit[] o6 = Physics.RaycastAll(val8, val9, maxDistance4, layerMask2);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 4))
			{
				objectTranslator.Get(L, 1, out Ray val10);
				float maxDistance5 = (float)Lua.lua_tonumber(L, 2);
				int layerMask3 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out QueryTriggerInteraction val11);
				RaycastHit[] o7 = Physics.RaycastAll(val10, maxDistance5, layerMask3, val11);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val12);
				objectTranslator.Get(L, 2, out Vector3 val13);
				float maxDistance6 = (float)Lua.lua_tonumber(L, 3);
				int layerMask4 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val14);
				RaycastHit[] o8 = Physics.RaycastAll(val12, val13, maxDistance6, layerMask4, val14);
				objectTranslator.Push(L, o8);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.RaycastAll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RaycastNonAlloc_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Ray>(L, 1) && objectTranslator.Assignable<RaycastHit[]>(L, 2))
			{
				objectTranslator.Get(L, 1, out Ray val);
				RaycastHit[] results = (RaycastHit[])objectTranslator.GetObject(L, 2, typeof(RaycastHit[]));
				int value = Physics.RaycastNonAlloc(val, results);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Ray>(L, 1) && objectTranslator.Assignable<RaycastHit[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Ray val2);
				RaycastHit[] results2 = (RaycastHit[])objectTranslator.GetObject(L, 2, typeof(RaycastHit[]));
				float maxDistance = (float)Lua.lua_tonumber(L, 3);
				int value2 = Physics.RaycastNonAlloc(val2, results2, maxDistance);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Ray>(L, 1) && objectTranslator.Assignable<RaycastHit[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Ray val3);
				RaycastHit[] results3 = (RaycastHit[])objectTranslator.GetObject(L, 2, typeof(RaycastHit[]));
				float maxDistance2 = (float)Lua.lua_tonumber(L, 3);
				int layerMask = Lua.xlua_tointeger(L, 4);
				int value3 = Physics.RaycastNonAlloc(val3, results3, maxDistance2, layerMask);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<RaycastHit[]>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val4);
				objectTranslator.Get(L, 2, out Vector3 val5);
				RaycastHit[] results4 = (RaycastHit[])objectTranslator.GetObject(L, 3, typeof(RaycastHit[]));
				int value4 = Physics.RaycastNonAlloc(val4, val5, results4);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<RaycastHit[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val6);
				objectTranslator.Get(L, 2, out Vector3 val7);
				RaycastHit[] results5 = (RaycastHit[])objectTranslator.GetObject(L, 3, typeof(RaycastHit[]));
				float maxDistance3 = (float)Lua.lua_tonumber(L, 4);
				int value5 = Physics.RaycastNonAlloc(val6, val7, results5, maxDistance3);
				Lua.xlua_pushinteger(L, value5);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Ray>(L, 1) && objectTranslator.Assignable<RaycastHit[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Ray val8);
				RaycastHit[] results6 = (RaycastHit[])objectTranslator.GetObject(L, 2, typeof(RaycastHit[]));
				float maxDistance4 = (float)Lua.lua_tonumber(L, 3);
				int layerMask2 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val9);
				int value6 = Physics.RaycastNonAlloc(val8, results6, maxDistance4, layerMask2, val9);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<RaycastHit[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val10);
				objectTranslator.Get(L, 2, out Vector3 val11);
				RaycastHit[] results7 = (RaycastHit[])objectTranslator.GetObject(L, 3, typeof(RaycastHit[]));
				float maxDistance5 = (float)Lua.lua_tonumber(L, 4);
				int layerMask3 = Lua.xlua_tointeger(L, 5);
				int value7 = Physics.RaycastNonAlloc(val10, val11, results7, maxDistance5, layerMask3);
				Lua.xlua_pushinteger(L, value7);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<RaycastHit[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val12);
				objectTranslator.Get(L, 2, out Vector3 val13);
				RaycastHit[] results8 = (RaycastHit[])objectTranslator.GetObject(L, 3, typeof(RaycastHit[]));
				float maxDistance6 = (float)Lua.lua_tonumber(L, 4);
				int layerMask4 = Lua.xlua_tointeger(L, 5);
				objectTranslator.Get(L, 6, out QueryTriggerInteraction val14);
				int value8 = Physics.RaycastNonAlloc(val12, val13, results8, maxDistance6, layerMask4, val14);
				Lua.xlua_pushinteger(L, value8);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.RaycastNonAlloc!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CapsuleCastAll_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				float radius = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val3);
				RaycastHit[] o = Physics.CapsuleCastAll(val, val2, radius, val3);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val4);
				objectTranslator.Get(L, 2, out Vector3 val5);
				float radius2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val6);
				float maxDistance = (float)Lua.lua_tonumber(L, 5);
				RaycastHit[] o2 = Physics.CapsuleCastAll(val4, val5, radius2, val6, maxDistance);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val7);
				objectTranslator.Get(L, 2, out Vector3 val8);
				float radius3 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val9);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 5);
				int layerMask = Lua.xlua_tointeger(L, 6);
				RaycastHit[] o3 = Physics.CapsuleCastAll(val7, val8, radius3, val9, maxDistance2, layerMask);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 7))
			{
				objectTranslator.Get(L, 1, out Vector3 val10);
				objectTranslator.Get(L, 2, out Vector3 val11);
				float radius4 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val12);
				float maxDistance3 = (float)Lua.lua_tonumber(L, 5);
				int layerMask2 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out QueryTriggerInteraction val13);
				RaycastHit[] o4 = Physics.CapsuleCastAll(val10, val11, radius4, val12, maxDistance3, layerMask2, val13);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.CapsuleCastAll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SphereCastAll_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Ray val);
				float radius = (float)Lua.lua_tonumber(L, 2);
				RaycastHit[] o = Physics.SphereCastAll(val, radius);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Ray val2);
				float radius2 = (float)Lua.lua_tonumber(L, 2);
				float maxDistance = (float)Lua.lua_tonumber(L, 3);
				RaycastHit[] o2 = Physics.SphereCastAll(val2, radius2, maxDistance);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Ray val3);
				float radius3 = (float)Lua.lua_tonumber(L, 2);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 3);
				int layerMask = Lua.xlua_tointeger(L, 4);
				RaycastHit[] o3 = Physics.SphereCastAll(val3, radius3, maxDistance2, layerMask);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val4);
				float radius4 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val5);
				RaycastHit[] o4 = Physics.SphereCastAll(val4, radius4, val5);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val6);
				float radius5 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val7);
				float maxDistance3 = (float)Lua.lua_tonumber(L, 4);
				RaycastHit[] o5 = Physics.SphereCastAll(val6, radius5, val7, maxDistance3);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val8);
				float radius6 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val9);
				float maxDistance4 = (float)Lua.lua_tonumber(L, 4);
				int layerMask2 = Lua.xlua_tointeger(L, 5);
				RaycastHit[] o6 = Physics.SphereCastAll(val8, radius6, val9, maxDistance4, layerMask2);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Ray val10);
				float radius7 = (float)Lua.lua_tonumber(L, 2);
				float maxDistance5 = (float)Lua.lua_tonumber(L, 3);
				int layerMask3 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val11);
				RaycastHit[] o7 = Physics.SphereCastAll(val10, radius7, maxDistance5, layerMask3, val11);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val12);
				float radius8 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val13);
				float maxDistance6 = (float)Lua.lua_tonumber(L, 4);
				int layerMask4 = Lua.xlua_tointeger(L, 5);
				objectTranslator.Get(L, 6, out QueryTriggerInteraction val14);
				RaycastHit[] o8 = Physics.SphereCastAll(val12, radius8, val13, maxDistance6, layerMask4, val14);
				objectTranslator.Push(L, o8);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.SphereCastAll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverlapCapsule_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				float radius = (float)Lua.lua_tonumber(L, 3);
				Collider[] o = Physics.OverlapCapsule(val, val2, radius);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				float radius2 = (float)Lua.lua_tonumber(L, 3);
				int layerMask = Lua.xlua_tointeger(L, 4);
				Collider[] o2 = Physics.OverlapCapsule(val3, val4, radius2, layerMask);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val5);
				objectTranslator.Get(L, 2, out Vector3 val6);
				float radius3 = (float)Lua.lua_tonumber(L, 3);
				int layerMask2 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val7);
				Collider[] o3 = Physics.OverlapCapsule(val5, val6, radius3, layerMask2, val7);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapCapsule!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverlapSphere_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				float radius = (float)Lua.lua_tonumber(L, 2);
				Collider[] o = Physics.OverlapSphere(val, radius);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val2);
				float radius2 = (float)Lua.lua_tonumber(L, 2);
				int layerMask = Lua.xlua_tointeger(L, 3);
				Collider[] o2 = Physics.OverlapSphere(val2, radius2, layerMask);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				float radius3 = (float)Lua.lua_tonumber(L, 2);
				int layerMask2 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out QueryTriggerInteraction val4);
				Collider[] o3 = Physics.OverlapSphere(val3, radius3, layerMask2, val4);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapSphere!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Simulate_xlua_st_(IntPtr L)
	{
		try
		{
			Physics.Simulate((float)Lua.lua_tonumber(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncTransforms_xlua_st_(IntPtr L)
	{
		try
		{
			Physics.SyncTransforms();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ComputePenetration_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Collider colliderA = (Collider)objectTranslator.GetObject(L, 1, typeof(Collider));
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Quaternion val2);
			Collider colliderB = (Collider)objectTranslator.GetObject(L, 4, typeof(Collider));
			objectTranslator.Get(L, 5, out Vector3 val3);
			objectTranslator.Get(L, 6, out Quaternion val4);
			Vector3 direction;
			float distance;
			bool value = Physics.ComputePenetration(colliderA, val, val2, colliderB, val3, val4, out direction, out distance);
			Lua.lua_pushboolean(L, value);
			objectTranslator.PushUnityEngineVector3(L, direction);
			Lua.lua_pushnumber(L, distance);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClosestPoint_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			Collider collider = (Collider)objectTranslator.GetObject(L, 2, typeof(Collider));
			objectTranslator.Get(L, 3, out Vector3 val2);
			objectTranslator.Get(L, 4, out Quaternion val3);
			Vector3 val4 = Physics.ClosestPoint(val, collider, val2, val3);
			objectTranslator.PushUnityEngineVector3(L, val4);
			return 1;
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Collider[]>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				float radius = (float)Lua.lua_tonumber(L, 2);
				Collider[] results = (Collider[])objectTranslator.GetObject(L, 3, typeof(Collider[]));
				int value = Physics.OverlapSphereNonAlloc(val, radius, results);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Collider[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val2);
				float radius2 = (float)Lua.lua_tonumber(L, 2);
				Collider[] results2 = (Collider[])objectTranslator.GetObject(L, 3, typeof(Collider[]));
				int layerMask = Lua.xlua_tointeger(L, 4);
				int value2 = Physics.OverlapSphereNonAlloc(val2, radius2, results2, layerMask);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Collider[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				float radius3 = (float)Lua.lua_tonumber(L, 2);
				Collider[] results3 = (Collider[])objectTranslator.GetObject(L, 3, typeof(Collider[]));
				int layerMask2 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val4);
				int value3 = Physics.OverlapSphereNonAlloc(val3, radius3, results3, layerMask2, val4);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapSphereNonAlloc!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckSphere_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				float radius = (float)Lua.lua_tonumber(L, 2);
				bool value = Physics.CheckSphere(val, radius);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val2);
				float radius2 = (float)Lua.lua_tonumber(L, 2);
				int layerMask = Lua.xlua_tointeger(L, 3);
				bool value2 = Physics.CheckSphere(val2, radius2, layerMask);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				float radius3 = (float)Lua.lua_tonumber(L, 2);
				int layerMask2 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out QueryTriggerInteraction val4);
				bool value3 = Physics.CheckSphere(val3, radius3, layerMask2, val4);
				Lua.lua_pushboolean(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.CheckSphere!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CapsuleCastNonAlloc_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && objectTranslator.Assignable<RaycastHit[]>(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				float radius = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val3);
				RaycastHit[] results = (RaycastHit[])objectTranslator.GetObject(L, 5, typeof(RaycastHit[]));
				int value = Physics.CapsuleCastNonAlloc(val, val2, radius, val3, results);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && objectTranslator.Assignable<RaycastHit[]>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val4);
				objectTranslator.Get(L, 2, out Vector3 val5);
				float radius2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val6);
				RaycastHit[] results2 = (RaycastHit[])objectTranslator.GetObject(L, 5, typeof(RaycastHit[]));
				float maxDistance = (float)Lua.lua_tonumber(L, 6);
				int value2 = Physics.CapsuleCastNonAlloc(val4, val5, radius2, val6, results2, maxDistance);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && objectTranslator.Assignable<RaycastHit[]>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				objectTranslator.Get(L, 1, out Vector3 val7);
				objectTranslator.Get(L, 2, out Vector3 val8);
				float radius3 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val9);
				RaycastHit[] results3 = (RaycastHit[])objectTranslator.GetObject(L, 5, typeof(RaycastHit[]));
				float maxDistance2 = (float)Lua.lua_tonumber(L, 6);
				int layerMask = Lua.xlua_tointeger(L, 7);
				int value3 = Physics.CapsuleCastNonAlloc(val7, val8, radius3, val9, results3, maxDistance2, layerMask);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 8 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && objectTranslator.Assignable<RaycastHit[]>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 8))
			{
				objectTranslator.Get(L, 1, out Vector3 val10);
				objectTranslator.Get(L, 2, out Vector3 val11);
				float radius4 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val12);
				RaycastHit[] results4 = (RaycastHit[])objectTranslator.GetObject(L, 5, typeof(RaycastHit[]));
				float maxDistance3 = (float)Lua.lua_tonumber(L, 6);
				int layerMask2 = Lua.xlua_tointeger(L, 7);
				objectTranslator.Get(L, 8, out QueryTriggerInteraction val13);
				int value4 = Physics.CapsuleCastNonAlloc(val10, val11, radius4, val12, results4, maxDistance3, layerMask2, val13);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.CapsuleCastNonAlloc!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SphereCastNonAlloc_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<RaycastHit[]>(L, 3))
			{
				objectTranslator.Get(L, 1, out Ray val);
				float radius = (float)Lua.lua_tonumber(L, 2);
				RaycastHit[] results = (RaycastHit[])objectTranslator.GetObject(L, 3, typeof(RaycastHit[]));
				int value = Physics.SphereCastNonAlloc(val, radius, results);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<RaycastHit[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Ray val2);
				float radius2 = (float)Lua.lua_tonumber(L, 2);
				RaycastHit[] results2 = (RaycastHit[])objectTranslator.GetObject(L, 3, typeof(RaycastHit[]));
				float maxDistance = (float)Lua.lua_tonumber(L, 4);
				int value2 = Physics.SphereCastNonAlloc(val2, radius2, results2, maxDistance);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<RaycastHit[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Ray val3);
				float radius3 = (float)Lua.lua_tonumber(L, 2);
				RaycastHit[] results3 = (RaycastHit[])objectTranslator.GetObject(L, 3, typeof(RaycastHit[]));
				float maxDistance2 = (float)Lua.lua_tonumber(L, 4);
				int layerMask = Lua.xlua_tointeger(L, 5);
				int value3 = Physics.SphereCastNonAlloc(val3, radius3, results3, maxDistance2, layerMask);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<RaycastHit[]>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val4);
				float radius4 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val5);
				RaycastHit[] results4 = (RaycastHit[])objectTranslator.GetObject(L, 4, typeof(RaycastHit[]));
				int value4 = Physics.SphereCastNonAlloc(val4, radius4, val5, results4);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<RaycastHit[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val6);
				float radius5 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val7);
				RaycastHit[] results5 = (RaycastHit[])objectTranslator.GetObject(L, 4, typeof(RaycastHit[]));
				float maxDistance3 = (float)Lua.lua_tonumber(L, 5);
				int value5 = Physics.SphereCastNonAlloc(val6, radius5, val7, results5, maxDistance3);
				Lua.xlua_pushinteger(L, value5);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<RaycastHit[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val8);
				float radius6 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val9);
				RaycastHit[] results6 = (RaycastHit[])objectTranslator.GetObject(L, 4, typeof(RaycastHit[]));
				float maxDistance4 = (float)Lua.lua_tonumber(L, 5);
				int layerMask2 = Lua.xlua_tointeger(L, 6);
				int value6 = Physics.SphereCastNonAlloc(val8, radius6, val9, results6, maxDistance4, layerMask2);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Ray>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<RaycastHit[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 6))
			{
				objectTranslator.Get(L, 1, out Ray val10);
				float radius7 = (float)Lua.lua_tonumber(L, 2);
				RaycastHit[] results7 = (RaycastHit[])objectTranslator.GetObject(L, 3, typeof(RaycastHit[]));
				float maxDistance5 = (float)Lua.lua_tonumber(L, 4);
				int layerMask3 = Lua.xlua_tointeger(L, 5);
				objectTranslator.Get(L, 6, out QueryTriggerInteraction val11);
				int value7 = Physics.SphereCastNonAlloc(val10, radius7, results7, maxDistance5, layerMask3, val11);
				Lua.xlua_pushinteger(L, value7);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<RaycastHit[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 7))
			{
				objectTranslator.Get(L, 1, out Vector3 val12);
				float radius8 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val13);
				RaycastHit[] results8 = (RaycastHit[])objectTranslator.GetObject(L, 4, typeof(RaycastHit[]));
				float maxDistance6 = (float)Lua.lua_tonumber(L, 5);
				int layerMask4 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out QueryTriggerInteraction val14);
				int value8 = Physics.SphereCastNonAlloc(val12, radius8, val13, results8, maxDistance6, layerMask4, val14);
				Lua.xlua_pushinteger(L, value8);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.SphereCastNonAlloc!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckCapsule_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				float radius = (float)Lua.lua_tonumber(L, 3);
				bool value = Physics.CheckCapsule(val, val2, radius);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				float radius2 = (float)Lua.lua_tonumber(L, 3);
				int layerMask = Lua.xlua_tointeger(L, 4);
				bool value2 = Physics.CheckCapsule(val3, val4, radius2, layerMask);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val5);
				objectTranslator.Get(L, 2, out Vector3 val6);
				float radius3 = (float)Lua.lua_tonumber(L, 3);
				int layerMask2 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val7);
				bool value3 = Physics.CheckCapsule(val5, val6, radius3, layerMask2, val7);
				Lua.lua_pushboolean(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.CheckCapsule!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckBox_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				bool value = Physics.CheckBox(val, val2);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				objectTranslator.Get(L, 3, out Quaternion val5);
				bool value2 = Physics.CheckBox(val3, val4, val5);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val6);
				objectTranslator.Get(L, 2, out Vector3 val7);
				objectTranslator.Get(L, 3, out Quaternion val8);
				int layerMask = Lua.xlua_tointeger(L, 4);
				bool value3 = Physics.CheckBox(val6, val7, val8, layerMask);
				Lua.lua_pushboolean(L, value3);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val9);
				objectTranslator.Get(L, 2, out Vector3 val10);
				objectTranslator.Get(L, 3, out Quaternion val11);
				int layermask = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val12);
				bool value4 = Physics.CheckBox(val9, val10, val11, layermask, val12);
				Lua.lua_pushboolean(L, value4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.CheckBox!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverlapBox_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				Collider[] o = Physics.OverlapBox(val, val2);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				objectTranslator.Get(L, 3, out Quaternion val5);
				Collider[] o2 = Physics.OverlapBox(val3, val4, val5);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val6);
				objectTranslator.Get(L, 2, out Vector3 val7);
				objectTranslator.Get(L, 3, out Quaternion val8);
				int layerMask = Lua.xlua_tointeger(L, 4);
				Collider[] o3 = Physics.OverlapBox(val6, val7, val8, layerMask);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val9);
				objectTranslator.Get(L, 2, out Vector3 val10);
				objectTranslator.Get(L, 3, out Quaternion val11);
				int layerMask2 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out QueryTriggerInteraction val12);
				Collider[] o4 = Physics.OverlapBox(val9, val10, val11, layerMask2, val12);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapBox!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverlapBoxNonAlloc_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Collider[]>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				Collider[] results = (Collider[])objectTranslator.GetObject(L, 3, typeof(Collider[]));
				int value = Physics.OverlapBoxNonAlloc(val, val2, results);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Collider[]>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				Collider[] results2 = (Collider[])objectTranslator.GetObject(L, 3, typeof(Collider[]));
				objectTranslator.Get(L, 4, out Quaternion val5);
				int value2 = Physics.OverlapBoxNonAlloc(val3, val4, results2, val5);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Collider[]>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val6);
				objectTranslator.Get(L, 2, out Vector3 val7);
				Collider[] results3 = (Collider[])objectTranslator.GetObject(L, 3, typeof(Collider[]));
				objectTranslator.Get(L, 4, out Quaternion val8);
				int mask = Lua.xlua_tointeger(L, 5);
				int value3 = Physics.OverlapBoxNonAlloc(val6, val7, results3, val8, mask);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Collider[]>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val9);
				objectTranslator.Get(L, 2, out Vector3 val10);
				Collider[] results4 = (Collider[])objectTranslator.GetObject(L, 3, typeof(Collider[]));
				objectTranslator.Get(L, 4, out Quaternion val11);
				int mask2 = Lua.xlua_tointeger(L, 5);
				objectTranslator.Get(L, 6, out QueryTriggerInteraction val12);
				int value4 = Physics.OverlapBoxNonAlloc(val9, val10, results4, val11, mask2, val12);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapBoxNonAlloc!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BoxCastNonAlloc_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<RaycastHit[]>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				RaycastHit[] results = (RaycastHit[])objectTranslator.GetObject(L, 4, typeof(RaycastHit[]));
				int value = Physics.BoxCastNonAlloc(val, val2, val3, results);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<RaycastHit[]>(L, 4) && objectTranslator.Assignable<Quaternion>(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val4);
				objectTranslator.Get(L, 2, out Vector3 val5);
				objectTranslator.Get(L, 3, out Vector3 val6);
				RaycastHit[] results2 = (RaycastHit[])objectTranslator.GetObject(L, 4, typeof(RaycastHit[]));
				objectTranslator.Get(L, 5, out Quaternion val7);
				int value2 = Physics.BoxCastNonAlloc(val4, val5, val6, results2, val7);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<RaycastHit[]>(L, 4) && objectTranslator.Assignable<Quaternion>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val8);
				objectTranslator.Get(L, 2, out Vector3 val9);
				objectTranslator.Get(L, 3, out Vector3 val10);
				RaycastHit[] results3 = (RaycastHit[])objectTranslator.GetObject(L, 4, typeof(RaycastHit[]));
				objectTranslator.Get(L, 5, out Quaternion val11);
				float maxDistance = (float)Lua.lua_tonumber(L, 6);
				int value3 = Physics.BoxCastNonAlloc(val8, val9, val10, results3, val11, maxDistance);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<RaycastHit[]>(L, 4) && objectTranslator.Assignable<Quaternion>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				objectTranslator.Get(L, 1, out Vector3 val12);
				objectTranslator.Get(L, 2, out Vector3 val13);
				objectTranslator.Get(L, 3, out Vector3 val14);
				RaycastHit[] results4 = (RaycastHit[])objectTranslator.GetObject(L, 4, typeof(RaycastHit[]));
				objectTranslator.Get(L, 5, out Quaternion val15);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 6);
				int layerMask = Lua.xlua_tointeger(L, 7);
				int value4 = Physics.BoxCastNonAlloc(val12, val13, val14, results4, val15, maxDistance2, layerMask);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 8 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<RaycastHit[]>(L, 4) && objectTranslator.Assignable<Quaternion>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 8))
			{
				objectTranslator.Get(L, 1, out Vector3 val16);
				objectTranslator.Get(L, 2, out Vector3 val17);
				objectTranslator.Get(L, 3, out Vector3 val18);
				RaycastHit[] results5 = (RaycastHit[])objectTranslator.GetObject(L, 4, typeof(RaycastHit[]));
				objectTranslator.Get(L, 5, out Quaternion val19);
				float maxDistance3 = (float)Lua.lua_tonumber(L, 6);
				int layerMask2 = Lua.xlua_tointeger(L, 7);
				objectTranslator.Get(L, 8, out QueryTriggerInteraction val20);
				int value5 = Physics.BoxCastNonAlloc(val16, val17, val18, results5, val19, maxDistance3, layerMask2, val20);
				Lua.xlua_pushinteger(L, value5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.BoxCastNonAlloc!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BoxCastAll_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				RaycastHit[] o = Physics.BoxCastAll(val, val2, val3);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val4);
				objectTranslator.Get(L, 2, out Vector3 val5);
				objectTranslator.Get(L, 3, out Vector3 val6);
				objectTranslator.Get(L, 4, out Quaternion val7);
				RaycastHit[] o2 = Physics.BoxCastAll(val4, val5, val6, val7);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val8);
				objectTranslator.Get(L, 2, out Vector3 val9);
				objectTranslator.Get(L, 3, out Vector3 val10);
				objectTranslator.Get(L, 4, out Quaternion val11);
				float maxDistance = (float)Lua.lua_tonumber(L, 5);
				RaycastHit[] o3 = Physics.BoxCastAll(val8, val9, val10, val11, maxDistance);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val12);
				objectTranslator.Get(L, 2, out Vector3 val13);
				objectTranslator.Get(L, 3, out Vector3 val14);
				objectTranslator.Get(L, 4, out Quaternion val15);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 5);
				int layerMask = Lua.xlua_tointeger(L, 6);
				RaycastHit[] o4 = Physics.BoxCastAll(val12, val13, val14, val15, maxDistance2, layerMask);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 7))
			{
				objectTranslator.Get(L, 1, out Vector3 val16);
				objectTranslator.Get(L, 2, out Vector3 val17);
				objectTranslator.Get(L, 3, out Vector3 val18);
				objectTranslator.Get(L, 4, out Quaternion val19);
				float maxDistance3 = (float)Lua.lua_tonumber(L, 5);
				int layerMask2 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out QueryTriggerInteraction val20);
				RaycastHit[] o5 = Physics.BoxCastAll(val16, val17, val18, val19, maxDistance3, layerMask2, val20);
				objectTranslator.Push(L, o5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.BoxCastAll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverlapCapsuleNonAlloc_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Collider[]>(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				float radius = (float)Lua.lua_tonumber(L, 3);
				Collider[] results = (Collider[])objectTranslator.GetObject(L, 4, typeof(Collider[]));
				int value = Physics.OverlapCapsuleNonAlloc(val, val2, radius, results);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Collider[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				float radius2 = (float)Lua.lua_tonumber(L, 3);
				Collider[] results2 = (Collider[])objectTranslator.GetObject(L, 4, typeof(Collider[]));
				int layerMask = Lua.xlua_tointeger(L, 5);
				int value2 = Physics.OverlapCapsuleNonAlloc(val3, val4, radius2, results2, layerMask);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Collider[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 6))
			{
				objectTranslator.Get(L, 1, out Vector3 val5);
				objectTranslator.Get(L, 2, out Vector3 val6);
				float radius3 = (float)Lua.lua_tonumber(L, 3);
				Collider[] results3 = (Collider[])objectTranslator.GetObject(L, 4, typeof(Collider[]));
				int layerMask2 = Lua.xlua_tointeger(L, 5);
				objectTranslator.Get(L, 6, out QueryTriggerInteraction val7);
				int value3 = Physics.OverlapCapsuleNonAlloc(val5, val6, radius3, results3, layerMask2, val7);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Physics.OverlapCapsuleNonAlloc!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RebuildBroadphaseRegions_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Bounds val);
			int subdivisions = Lua.xlua_tointeger(L, 2);
			Physics.RebuildBroadphaseRegions(val, subdivisions);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BakeMesh_xlua_st_(IntPtr L)
	{
		try
		{
			int meshID = Lua.xlua_tointeger(L, 1);
			bool convex = Lua.lua_toboolean(L, 2);
			Physics.BakeMesh(meshID, convex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gravity(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Physics.gravity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultContactOffset(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Physics.defaultContactOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sleepThreshold(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Physics.sleepThreshold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_queriesHitTriggers(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Physics.queriesHitTriggers);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_queriesHitBackfaces(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Physics.queriesHitBackfaces);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bounceThreshold(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Physics.bounceThreshold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultSolverIterations(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Physics.defaultSolverIterations);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultSolverVelocityIterations(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Physics.defaultSolverVelocityIterations);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultMaxAngularSpeed(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Physics.defaultMaxAngularSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultPhysicsScene(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Physics.defaultPhysicsScene);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autoSimulation(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Physics.autoSimulation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autoSyncTransforms(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Physics.autoSyncTransforms);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_reuseCollisionCallbacks(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Physics.reuseCollisionCallbacks);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_interCollisionDistance(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Physics.interCollisionDistance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_interCollisionStiffness(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Physics.interCollisionStiffness);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_interCollisionSettingsToggle(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Physics.interCollisionSettingsToggle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clothGravity(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, Physics.clothGravity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gravity(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
			Physics.gravity = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultContactOffset(IntPtr L)
	{
		try
		{
			Physics.defaultContactOffset = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sleepThreshold(IntPtr L)
	{
		try
		{
			Physics.sleepThreshold = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_queriesHitTriggers(IntPtr L)
	{
		try
		{
			Physics.queriesHitTriggers = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_queriesHitBackfaces(IntPtr L)
	{
		try
		{
			Physics.queriesHitBackfaces = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bounceThreshold(IntPtr L)
	{
		try
		{
			Physics.bounceThreshold = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultSolverIterations(IntPtr L)
	{
		try
		{
			Physics.defaultSolverIterations = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultSolverVelocityIterations(IntPtr L)
	{
		try
		{
			Physics.defaultSolverVelocityIterations = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_defaultMaxAngularSpeed(IntPtr L)
	{
		try
		{
			Physics.defaultMaxAngularSpeed = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autoSimulation(IntPtr L)
	{
		try
		{
			Physics.autoSimulation = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autoSyncTransforms(IntPtr L)
	{
		try
		{
			Physics.autoSyncTransforms = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_reuseCollisionCallbacks(IntPtr L)
	{
		try
		{
			Physics.reuseCollisionCallbacks = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_interCollisionDistance(IntPtr L)
	{
		try
		{
			Physics.interCollisionDistance = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_interCollisionStiffness(IntPtr L)
	{
		try
		{
			Physics.interCollisionStiffness = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_interCollisionSettingsToggle(IntPtr L)
	{
		try
		{
			Physics.interCollisionSettingsToggle = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clothGravity(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
			Physics.clothGravity = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
