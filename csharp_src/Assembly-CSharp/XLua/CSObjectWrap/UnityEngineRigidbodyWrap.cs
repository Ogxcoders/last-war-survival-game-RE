using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRigidbodyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Rigidbody);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 19, 23, 22);
		Utils.RegisterFunc(L, -3, "SetDensity", _m_SetDensity);
		Utils.RegisterFunc(L, -3, "MovePosition", _m_MovePosition);
		Utils.RegisterFunc(L, -3, "MoveRotation", _m_MoveRotation);
		Utils.RegisterFunc(L, -3, "Sleep", _m_Sleep);
		Utils.RegisterFunc(L, -3, "IsSleeping", _m_IsSleeping);
		Utils.RegisterFunc(L, -3, "WakeUp", _m_WakeUp);
		Utils.RegisterFunc(L, -3, "ResetCenterOfMass", _m_ResetCenterOfMass);
		Utils.RegisterFunc(L, -3, "ResetInertiaTensor", _m_ResetInertiaTensor);
		Utils.RegisterFunc(L, -3, "GetRelativePointVelocity", _m_GetRelativePointVelocity);
		Utils.RegisterFunc(L, -3, "GetPointVelocity", _m_GetPointVelocity);
		Utils.RegisterFunc(L, -3, "AddForce", _m_AddForce);
		Utils.RegisterFunc(L, -3, "AddRelativeForce", _m_AddRelativeForce);
		Utils.RegisterFunc(L, -3, "AddTorque", _m_AddTorque);
		Utils.RegisterFunc(L, -3, "AddRelativeTorque", _m_AddRelativeTorque);
		Utils.RegisterFunc(L, -3, "AddForceAtPosition", _m_AddForceAtPosition);
		Utils.RegisterFunc(L, -3, "AddExplosionForce", _m_AddExplosionForce);
		Utils.RegisterFunc(L, -3, "ClosestPointOnBounds", _m_ClosestPointOnBounds);
		Utils.RegisterFunc(L, -3, "SweepTest", _m_SweepTest);
		Utils.RegisterFunc(L, -3, "SweepTestAll", _m_SweepTestAll);
		Utils.RegisterFunc(L, -2, "velocity", _g_get_velocity);
		Utils.RegisterFunc(L, -2, "angularVelocity", _g_get_angularVelocity);
		Utils.RegisterFunc(L, -2, "drag", _g_get_drag);
		Utils.RegisterFunc(L, -2, "angularDrag", _g_get_angularDrag);
		Utils.RegisterFunc(L, -2, "mass", _g_get_mass);
		Utils.RegisterFunc(L, -2, "useGravity", _g_get_useGravity);
		Utils.RegisterFunc(L, -2, "maxDepenetrationVelocity", _g_get_maxDepenetrationVelocity);
		Utils.RegisterFunc(L, -2, "isKinematic", _g_get_isKinematic);
		Utils.RegisterFunc(L, -2, "freezeRotation", _g_get_freezeRotation);
		Utils.RegisterFunc(L, -2, "constraints", _g_get_constraints);
		Utils.RegisterFunc(L, -2, "collisionDetectionMode", _g_get_collisionDetectionMode);
		Utils.RegisterFunc(L, -2, "centerOfMass", _g_get_centerOfMass);
		Utils.RegisterFunc(L, -2, "worldCenterOfMass", _g_get_worldCenterOfMass);
		Utils.RegisterFunc(L, -2, "inertiaTensorRotation", _g_get_inertiaTensorRotation);
		Utils.RegisterFunc(L, -2, "inertiaTensor", _g_get_inertiaTensor);
		Utils.RegisterFunc(L, -2, "detectCollisions", _g_get_detectCollisions);
		Utils.RegisterFunc(L, -2, "position", _g_get_position);
		Utils.RegisterFunc(L, -2, "rotation", _g_get_rotation);
		Utils.RegisterFunc(L, -2, "interpolation", _g_get_interpolation);
		Utils.RegisterFunc(L, -2, "solverIterations", _g_get_solverIterations);
		Utils.RegisterFunc(L, -2, "sleepThreshold", _g_get_sleepThreshold);
		Utils.RegisterFunc(L, -2, "maxAngularVelocity", _g_get_maxAngularVelocity);
		Utils.RegisterFunc(L, -2, "solverVelocityIterations", _g_get_solverVelocityIterations);
		Utils.RegisterFunc(L, -1, "velocity", _s_set_velocity);
		Utils.RegisterFunc(L, -1, "angularVelocity", _s_set_angularVelocity);
		Utils.RegisterFunc(L, -1, "drag", _s_set_drag);
		Utils.RegisterFunc(L, -1, "angularDrag", _s_set_angularDrag);
		Utils.RegisterFunc(L, -1, "mass", _s_set_mass);
		Utils.RegisterFunc(L, -1, "useGravity", _s_set_useGravity);
		Utils.RegisterFunc(L, -1, "maxDepenetrationVelocity", _s_set_maxDepenetrationVelocity);
		Utils.RegisterFunc(L, -1, "isKinematic", _s_set_isKinematic);
		Utils.RegisterFunc(L, -1, "freezeRotation", _s_set_freezeRotation);
		Utils.RegisterFunc(L, -1, "constraints", _s_set_constraints);
		Utils.RegisterFunc(L, -1, "collisionDetectionMode", _s_set_collisionDetectionMode);
		Utils.RegisterFunc(L, -1, "centerOfMass", _s_set_centerOfMass);
		Utils.RegisterFunc(L, -1, "inertiaTensorRotation", _s_set_inertiaTensorRotation);
		Utils.RegisterFunc(L, -1, "inertiaTensor", _s_set_inertiaTensor);
		Utils.RegisterFunc(L, -1, "detectCollisions", _s_set_detectCollisions);
		Utils.RegisterFunc(L, -1, "position", _s_set_position);
		Utils.RegisterFunc(L, -1, "rotation", _s_set_rotation);
		Utils.RegisterFunc(L, -1, "interpolation", _s_set_interpolation);
		Utils.RegisterFunc(L, -1, "solverIterations", _s_set_solverIterations);
		Utils.RegisterFunc(L, -1, "sleepThreshold", _s_set_sleepThreshold);
		Utils.RegisterFunc(L, -1, "maxAngularVelocity", _s_set_maxAngularVelocity);
		Utils.RegisterFunc(L, -1, "solverVelocityIterations", _s_set_solverVelocityIterations);
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
				Rigidbody o = new Rigidbody();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rigidbody constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDensity(IntPtr L)
	{
		try
		{
			Rigidbody obj = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float density = (float)Lua.lua_tonumber(L, 2);
			obj.SetDensity(density);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MovePosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			rigidbody.MovePosition(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			rigidbody.MoveRotation(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Sleep(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Sleep();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSleeping(IntPtr L)
	{
		try
		{
			bool value = ((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsSleeping();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WakeUp(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).WakeUp();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetCenterOfMass(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetCenterOfMass();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetInertiaTensor(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetInertiaTensor();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRelativePointVelocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 relativePointVelocity = rigidbody.GetRelativePointVelocity(val);
			objectTranslator.PushUnityEngineVector3(L, relativePointVelocity);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointVelocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 pointVelocity = rigidbody.GetPointVelocity(val);
			objectTranslator.PushUnityEngineVector3(L, pointVelocity);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddForce(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				rigidbody.AddForce(x, y, z);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				rigidbody.AddForce(val);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<ForceMode>(L, 5))
			{
				float x2 = (float)Lua.lua_tonumber(L, 2);
				float y2 = (float)Lua.lua_tonumber(L, 3);
				float z2 = (float)Lua.lua_tonumber(L, 4);
				objectTranslator.Get(L, 5, out ForceMode v);
				rigidbody.AddForce(x2, y2, z2, v);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<ForceMode>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.Get(L, 3, out ForceMode v2);
				rigidbody.AddForce(val2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rigidbody.AddForce!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddRelativeForce(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				rigidbody.AddRelativeForce(x, y, z);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				rigidbody.AddRelativeForce(val);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<ForceMode>(L, 5))
			{
				float x2 = (float)Lua.lua_tonumber(L, 2);
				float y2 = (float)Lua.lua_tonumber(L, 3);
				float z2 = (float)Lua.lua_tonumber(L, 4);
				objectTranslator.Get(L, 5, out ForceMode v);
				rigidbody.AddRelativeForce(x2, y2, z2, v);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<ForceMode>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.Get(L, 3, out ForceMode v2);
				rigidbody.AddRelativeForce(val2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rigidbody.AddRelativeForce!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddTorque(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				rigidbody.AddTorque(x, y, z);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				rigidbody.AddTorque(val);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<ForceMode>(L, 5))
			{
				float x2 = (float)Lua.lua_tonumber(L, 2);
				float y2 = (float)Lua.lua_tonumber(L, 3);
				float z2 = (float)Lua.lua_tonumber(L, 4);
				objectTranslator.Get(L, 5, out ForceMode v);
				rigidbody.AddTorque(x2, y2, z2, v);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<ForceMode>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.Get(L, 3, out ForceMode v2);
				rigidbody.AddTorque(val2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rigidbody.AddTorque!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddRelativeTorque(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				rigidbody.AddRelativeTorque(x, y, z);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				rigidbody.AddRelativeTorque(val);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<ForceMode>(L, 5))
			{
				float x2 = (float)Lua.lua_tonumber(L, 2);
				float y2 = (float)Lua.lua_tonumber(L, 3);
				float z2 = (float)Lua.lua_tonumber(L, 4);
				objectTranslator.Get(L, 5, out ForceMode v);
				rigidbody.AddRelativeTorque(x2, y2, z2, v);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<ForceMode>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.Get(L, 3, out ForceMode v2);
				rigidbody.AddRelativeTorque(val2, v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rigidbody.AddRelativeTorque!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddForceAtPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				objectTranslator.Get(L, 3, out Vector3 val2);
				rigidbody.AddForceAtPosition(val, val2);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<ForceMode>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Vector3 val4);
				objectTranslator.Get(L, 4, out ForceMode v);
				rigidbody.AddForceAtPosition(val3, val4, v);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rigidbody.AddForceAtPosition!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddExplosionForce(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float explosionForce = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				float explosionRadius = (float)Lua.lua_tonumber(L, 4);
				rigidbody.AddExplosionForce(explosionForce, val, explosionRadius);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float explosionForce2 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				float explosionRadius2 = (float)Lua.lua_tonumber(L, 4);
				float upwardsModifier = (float)Lua.lua_tonumber(L, 5);
				rigidbody.AddExplosionForce(explosionForce2, val2, explosionRadius2, upwardsModifier);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<ForceMode>(L, 6))
			{
				float explosionForce3 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				float explosionRadius3 = (float)Lua.lua_tonumber(L, 4);
				float upwardsModifier2 = (float)Lua.lua_tonumber(L, 5);
				objectTranslator.Get(L, 6, out ForceMode v);
				rigidbody.AddExplosionForce(explosionForce3, val3, explosionRadius3, upwardsModifier2, v);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rigidbody.AddExplosionForce!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClosestPointOnBounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = rigidbody.ClosestPointOnBounds(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SweepTest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				RaycastHit hitInfo;
				bool value = rigidbody.SweepTest(val, out hitInfo);
				Lua.lua_pushboolean(L, value);
				objectTranslator.Push(L, hitInfo);
				return 2;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float maxDistance = (float)Lua.lua_tonumber(L, 3);
				RaycastHit hitInfo2;
				bool value2 = rigidbody.SweepTest(val2, out hitInfo2, maxDistance);
				Lua.lua_pushboolean(L, value2);
				objectTranslator.Push(L, hitInfo2);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out QueryTriggerInteraction val4);
				RaycastHit hitInfo3;
				bool value3 = rigidbody.SweepTest(val3, out hitInfo3, maxDistance2, val4);
				Lua.lua_pushboolean(L, value3);
				objectTranslator.Push(L, hitInfo3);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rigidbody.SweepTest!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SweepTestAll(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				RaycastHit[] o = rigidbody.SweepTestAll(val);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float maxDistance = (float)Lua.lua_tonumber(L, 3);
				RaycastHit[] o2 = rigidbody.SweepTestAll(val2, maxDistance);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<QueryTriggerInteraction>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float maxDistance2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out QueryTriggerInteraction val4);
				RaycastHit[] o3 = rigidbody.SweepTestAll(val3, maxDistance2, val4);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rigidbody.SweepTestAll!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_velocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, rigidbody.velocity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_angularVelocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, rigidbody.angularVelocity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_drag(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rigidbody.drag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_angularDrag(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rigidbody.angularDrag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mass(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rigidbody.mass);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useGravity(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, rigidbody.useGravity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxDepenetrationVelocity(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rigidbody.maxDepenetrationVelocity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isKinematic(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, rigidbody.isKinematic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_freezeRotation(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, rigidbody.freezeRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_constraints(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rigidbody.constraints);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_collisionDetectionMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rigidbody.collisionDetectionMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_centerOfMass(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, rigidbody.centerOfMass);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldCenterOfMass(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, rigidbody.worldCenterOfMass);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inertiaTensorRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineQuaternion(L, rigidbody.inertiaTensorRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inertiaTensor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, rigidbody.inertiaTensor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_detectCollisions(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, rigidbody.detectCollisions);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, rigidbody.position);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineQuaternion(L, rigidbody.rotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_interpolation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rigidbody.interpolation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_solverIterations(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rigidbody.solverIterations);
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
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rigidbody.sleepThreshold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxAngularVelocity(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, rigidbody.maxAngularVelocity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_solverVelocityIterations(IntPtr L)
	{
		try
		{
			Rigidbody rigidbody = (Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rigidbody.solverVelocityIterations);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_velocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			rigidbody.velocity = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_angularVelocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			rigidbody.angularVelocity = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_drag(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).drag = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_angularDrag(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).angularDrag = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mass(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mass = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useGravity(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useGravity = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxDepenetrationVelocity(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxDepenetrationVelocity = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isKinematic(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isKinematic = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_freezeRotation(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).freezeRotation = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_constraints(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RigidbodyConstraints v);
			rigidbody.constraints = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_collisionDetectionMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CollisionDetectionMode v);
			rigidbody.collisionDetectionMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_centerOfMass(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			rigidbody.centerOfMass = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_inertiaTensorRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			rigidbody.inertiaTensorRotation = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_inertiaTensor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			rigidbody.inertiaTensor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_detectCollisions(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).detectCollisions = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			rigidbody.position = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			rigidbody.rotation = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_interpolation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Rigidbody rigidbody = (Rigidbody)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RigidbodyInterpolation v);
			rigidbody.interpolation = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_solverIterations(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).solverIterations = Lua.xlua_tointeger(L, 2);
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
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sleepThreshold = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxAngularVelocity(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxAngularVelocity = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_solverVelocityIterations(IntPtr L)
	{
		try
		{
			((Rigidbody)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).solverVelocityIterations = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
