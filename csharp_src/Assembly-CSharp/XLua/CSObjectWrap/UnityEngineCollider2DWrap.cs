using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineCollider2DWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UnityEngine.Collider2D);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 11, 12, 6);
		Utils.RegisterFunc(L, -3, "CreateMesh", _m_CreateMesh);
		Utils.RegisterFunc(L, -3, "GetShapeHash", _m_GetShapeHash);
		Utils.RegisterFunc(L, -3, "IsTouching", _m_IsTouching);
		Utils.RegisterFunc(L, -3, "IsTouchingLayers", _m_IsTouchingLayers);
		Utils.RegisterFunc(L, -3, "OverlapPoint", _m_OverlapPoint);
		Utils.RegisterFunc(L, -3, "Distance", _m_Distance);
		Utils.RegisterFunc(L, -3, "OverlapCollider", _m_OverlapCollider);
		Utils.RegisterFunc(L, -3, "GetContacts", _m_GetContacts);
		Utils.RegisterFunc(L, -3, "Cast", _m_Cast);
		Utils.RegisterFunc(L, -3, "Raycast", _m_Raycast);
		Utils.RegisterFunc(L, -3, "ClosestPoint", _m_ClosestPoint);
		Utils.RegisterFunc(L, -2, "density", _g_get_density);
		Utils.RegisterFunc(L, -2, "isTrigger", _g_get_isTrigger);
		Utils.RegisterFunc(L, -2, "usedByEffector", _g_get_usedByEffector);
		Utils.RegisterFunc(L, -2, "usedByComposite", _g_get_usedByComposite);
		Utils.RegisterFunc(L, -2, "composite", _g_get_composite);
		Utils.RegisterFunc(L, -2, "offset", _g_get_offset);
		Utils.RegisterFunc(L, -2, "attachedRigidbody", _g_get_attachedRigidbody);
		Utils.RegisterFunc(L, -2, "shapeCount", _g_get_shapeCount);
		Utils.RegisterFunc(L, -2, "bounds", _g_get_bounds);
		Utils.RegisterFunc(L, -2, "sharedMaterial", _g_get_sharedMaterial);
		Utils.RegisterFunc(L, -2, "friction", _g_get_friction);
		Utils.RegisterFunc(L, -2, "bounciness", _g_get_bounciness);
		Utils.RegisterFunc(L, -1, "density", _s_set_density);
		Utils.RegisterFunc(L, -1, "isTrigger", _s_set_isTrigger);
		Utils.RegisterFunc(L, -1, "usedByEffector", _s_set_usedByEffector);
		Utils.RegisterFunc(L, -1, "usedByComposite", _s_set_usedByComposite);
		Utils.RegisterFunc(L, -1, "offset", _s_set_offset);
		Utils.RegisterFunc(L, -1, "sharedMaterial", _s_set_sharedMaterial);
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
				UnityEngine.Collider2D o = new UnityEngine.Collider2D();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Collider2D constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D obj = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			bool useBodyPosition = Lua.lua_toboolean(L, 2);
			bool useBodyRotation = Lua.lua_toboolean(L, 3);
			Mesh o = obj.CreateMesh(useBodyPosition, useBodyRotation);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetShapeHash(IntPtr L)
	{
		try
		{
			uint shapeHash = ((UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetShapeHash();
			Lua.xlua_pushuint(L, shapeHash);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTouching(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<UnityEngine.Collider2D>(L, 2))
			{
				UnityEngine.Collider2D collider = (UnityEngine.Collider2D)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Collider2D));
				bool value = collider2D.IsTouching(collider);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<ContactFilter2D>(L, 2))
			{
				objectTranslator.Get(L, 2, out ContactFilter2D v);
				bool value2 = collider2D.IsTouching(v);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<UnityEngine.Collider2D>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3))
			{
				UnityEngine.Collider2D collider2 = (UnityEngine.Collider2D)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Collider2D));
				objectTranslator.Get(L, 3, out ContactFilter2D v2);
				bool value3 = collider2D.IsTouching(collider2, v2);
				Lua.lua_pushboolean(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Collider2D.IsTouching!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTouchingLayers(IntPtr L)
	{
		try
		{
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				bool value2 = collider2D.IsTouchingLayers();
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
				{
					int layerMask = Lua.xlua_tointeger(L, 2);
					bool value = collider2D.IsTouchingLayers(layerMask);
					Lua.lua_pushboolean(L, value);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Collider2D.IsTouchingLayers!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverlapPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			bool value = collider2D.OverlapPoint(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Distance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			UnityEngine.Collider2D collider = (UnityEngine.Collider2D)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Collider2D));
			ColliderDistance2D colliderDistance2D = collider2D.Distance(collider);
			objectTranslator.Push(L, colliderDistance2D);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OverlapCollider(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<ContactFilter2D>(L, 2) && objectTranslator.Assignable<UnityEngine.Collider2D[]>(L, 3))
			{
				objectTranslator.Get(L, 2, out ContactFilter2D v);
				UnityEngine.Collider2D[] results = (UnityEngine.Collider2D[])objectTranslator.GetObject(L, 3, typeof(UnityEngine.Collider2D[]));
				int value = collider2D.OverlapCollider(v, results);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<ContactFilter2D>(L, 2) && objectTranslator.Assignable<List<UnityEngine.Collider2D>>(L, 3))
			{
				objectTranslator.Get(L, 2, out ContactFilter2D v2);
				List<UnityEngine.Collider2D> results2 = (List<UnityEngine.Collider2D>)objectTranslator.GetObject(L, 3, typeof(List<UnityEngine.Collider2D>));
				int value2 = collider2D.OverlapCollider(v2, results2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Collider2D.OverlapCollider!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetContacts(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<ContactPoint2D[]>(L, 2))
			{
				ContactPoint2D[] contacts = (ContactPoint2D[])objectTranslator.GetObject(L, 2, typeof(ContactPoint2D[]));
				int contacts2 = collider2D.GetContacts(contacts);
				Lua.xlua_pushinteger(L, contacts2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<List<ContactPoint2D>>(L, 2))
			{
				List<ContactPoint2D> contacts3 = (List<ContactPoint2D>)objectTranslator.GetObject(L, 2, typeof(List<ContactPoint2D>));
				int contacts4 = collider2D.GetContacts(contacts3);
				Lua.xlua_pushinteger(L, contacts4);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<UnityEngine.Collider2D[]>(L, 2))
			{
				UnityEngine.Collider2D[] colliders = (UnityEngine.Collider2D[])objectTranslator.GetObject(L, 2, typeof(UnityEngine.Collider2D[]));
				int contacts5 = collider2D.GetContacts(colliders);
				Lua.xlua_pushinteger(L, contacts5);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<List<UnityEngine.Collider2D>>(L, 2))
			{
				List<UnityEngine.Collider2D> colliders2 = (List<UnityEngine.Collider2D>)objectTranslator.GetObject(L, 2, typeof(List<UnityEngine.Collider2D>));
				int contacts6 = collider2D.GetContacts(colliders2);
				Lua.xlua_pushinteger(L, contacts6);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<ContactFilter2D>(L, 2) && objectTranslator.Assignable<ContactPoint2D[]>(L, 3))
			{
				objectTranslator.Get(L, 2, out ContactFilter2D v);
				ContactPoint2D[] contacts7 = (ContactPoint2D[])objectTranslator.GetObject(L, 3, typeof(ContactPoint2D[]));
				int contacts8 = collider2D.GetContacts(v, contacts7);
				Lua.xlua_pushinteger(L, contacts8);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<ContactFilter2D>(L, 2) && objectTranslator.Assignable<List<ContactPoint2D>>(L, 3))
			{
				objectTranslator.Get(L, 2, out ContactFilter2D v2);
				List<ContactPoint2D> contacts9 = (List<ContactPoint2D>)objectTranslator.GetObject(L, 3, typeof(List<ContactPoint2D>));
				int contacts10 = collider2D.GetContacts(v2, contacts9);
				Lua.xlua_pushinteger(L, contacts10);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<ContactFilter2D>(L, 2) && objectTranslator.Assignable<UnityEngine.Collider2D[]>(L, 3))
			{
				objectTranslator.Get(L, 2, out ContactFilter2D v3);
				UnityEngine.Collider2D[] colliders3 = (UnityEngine.Collider2D[])objectTranslator.GetObject(L, 3, typeof(UnityEngine.Collider2D[]));
				int contacts11 = collider2D.GetContacts(v3, colliders3);
				Lua.xlua_pushinteger(L, contacts11);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<ContactFilter2D>(L, 2) && objectTranslator.Assignable<List<UnityEngine.Collider2D>>(L, 3))
			{
				objectTranslator.Get(L, 2, out ContactFilter2D v4);
				List<UnityEngine.Collider2D> colliders4 = (List<UnityEngine.Collider2D>)objectTranslator.GetObject(L, 3, typeof(List<UnityEngine.Collider2D>));
				int contacts12 = collider2D.GetContacts(v4, colliders4);
				Lua.xlua_pushinteger(L, contacts12);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Collider2D.GetContacts!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Cast(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<RaycastHit2D[]>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				RaycastHit2D[] results = (RaycastHit2D[])objectTranslator.GetObject(L, 3, typeof(RaycastHit2D[]));
				int value = collider2D.Cast(val, results);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<RaycastHit2D[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				RaycastHit2D[] results2 = (RaycastHit2D[])objectTranslator.GetObject(L, 3, typeof(RaycastHit2D[]));
				float distance = (float)Lua.lua_tonumber(L, 4);
				int value2 = collider2D.Cast(val2, results2, distance);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<RaycastHit2D[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector2 val3);
				RaycastHit2D[] results3 = (RaycastHit2D[])objectTranslator.GetObject(L, 3, typeof(RaycastHit2D[]));
				float distance2 = (float)Lua.lua_tonumber(L, 4);
				bool ignoreSiblingColliders = Lua.lua_toboolean(L, 5);
				int value3 = collider2D.Cast(val3, results3, distance2, ignoreSiblingColliders);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3) && objectTranslator.Assignable<RaycastHit2D[]>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val4);
				objectTranslator.Get(L, 3, out ContactFilter2D v);
				RaycastHit2D[] results4 = (RaycastHit2D[])objectTranslator.GetObject(L, 4, typeof(RaycastHit2D[]));
				int value4 = collider2D.Cast(val4, v, results4);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3) && objectTranslator.Assignable<RaycastHit2D[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector2 val5);
				objectTranslator.Get(L, 3, out ContactFilter2D v2);
				RaycastHit2D[] results5 = (RaycastHit2D[])objectTranslator.GetObject(L, 4, typeof(RaycastHit2D[]));
				float distance3 = (float)Lua.lua_tonumber(L, 5);
				int value5 = collider2D.Cast(val5, v2, results5, distance3);
				Lua.xlua_pushinteger(L, value5);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3) && objectTranslator.Assignable<RaycastHit2D[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector2 val6);
				objectTranslator.Get(L, 3, out ContactFilter2D v3);
				RaycastHit2D[] results6 = (RaycastHit2D[])objectTranslator.GetObject(L, 4, typeof(RaycastHit2D[]));
				float distance4 = (float)Lua.lua_tonumber(L, 5);
				bool ignoreSiblingColliders2 = Lua.lua_toboolean(L, 6);
				int value6 = collider2D.Cast(val6, v3, results6, distance4, ignoreSiblingColliders2);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3) && objectTranslator.Assignable<List<RaycastHit2D>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector2 val7);
				objectTranslator.Get(L, 3, out ContactFilter2D v4);
				List<RaycastHit2D> results7 = (List<RaycastHit2D>)objectTranslator.GetObject(L, 4, typeof(List<RaycastHit2D>));
				float distance5 = (float)Lua.lua_tonumber(L, 5);
				bool ignoreSiblingColliders3 = Lua.lua_toboolean(L, 6);
				int value7 = collider2D.Cast(val7, v4, results7, distance5, ignoreSiblingColliders3);
				Lua.xlua_pushinteger(L, value7);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3) && objectTranslator.Assignable<List<RaycastHit2D>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector2 val8);
				objectTranslator.Get(L, 3, out ContactFilter2D v5);
				List<RaycastHit2D> results8 = (List<RaycastHit2D>)objectTranslator.GetObject(L, 4, typeof(List<RaycastHit2D>));
				float distance6 = (float)Lua.lua_tonumber(L, 5);
				int value8 = collider2D.Cast(val8, v5, results8, distance6);
				Lua.xlua_pushinteger(L, value8);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3) && objectTranslator.Assignable<List<RaycastHit2D>>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val9);
				objectTranslator.Get(L, 3, out ContactFilter2D v6);
				List<RaycastHit2D> results9 = (List<RaycastHit2D>)objectTranslator.GetObject(L, 4, typeof(List<RaycastHit2D>));
				int value9 = collider2D.Cast(val9, v6, results9);
				Lua.xlua_pushinteger(L, value9);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Collider2D.Cast!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Raycast(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<RaycastHit2D[]>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				RaycastHit2D[] results = (RaycastHit2D[])objectTranslator.GetObject(L, 3, typeof(RaycastHit2D[]));
				int value = collider2D.Raycast(val, results);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<RaycastHit2D[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				RaycastHit2D[] results2 = (RaycastHit2D[])objectTranslator.GetObject(L, 3, typeof(RaycastHit2D[]));
				float distance = (float)Lua.lua_tonumber(L, 4);
				int value2 = collider2D.Raycast(val2, results2, distance);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<RaycastHit2D[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector2 val3);
				RaycastHit2D[] results3 = (RaycastHit2D[])objectTranslator.GetObject(L, 3, typeof(RaycastHit2D[]));
				float distance2 = (float)Lua.lua_tonumber(L, 4);
				int layerMask = Lua.xlua_tointeger(L, 5);
				int value3 = collider2D.Raycast(val3, results3, distance2, layerMask);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<RaycastHit2D[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector2 val4);
				RaycastHit2D[] results4 = (RaycastHit2D[])objectTranslator.GetObject(L, 3, typeof(RaycastHit2D[]));
				float distance3 = (float)Lua.lua_tonumber(L, 4);
				int layerMask2 = Lua.xlua_tointeger(L, 5);
				float minDepth = (float)Lua.lua_tonumber(L, 6);
				int value4 = collider2D.Raycast(val4, results4, distance3, layerMask2, minDepth);
				Lua.xlua_pushinteger(L, value4);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<RaycastHit2D[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				objectTranslator.Get(L, 2, out Vector2 val5);
				RaycastHit2D[] results5 = (RaycastHit2D[])objectTranslator.GetObject(L, 3, typeof(RaycastHit2D[]));
				float distance4 = (float)Lua.lua_tonumber(L, 4);
				int layerMask3 = Lua.xlua_tointeger(L, 5);
				float minDepth2 = (float)Lua.lua_tonumber(L, 6);
				float maxDepth = (float)Lua.lua_tonumber(L, 7);
				int value5 = collider2D.Raycast(val5, results5, distance4, layerMask3, minDepth2, maxDepth);
				Lua.xlua_pushinteger(L, value5);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3) && objectTranslator.Assignable<RaycastHit2D[]>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val6);
				objectTranslator.Get(L, 3, out ContactFilter2D v);
				RaycastHit2D[] results6 = (RaycastHit2D[])objectTranslator.GetObject(L, 4, typeof(RaycastHit2D[]));
				int value6 = collider2D.Raycast(val6, v, results6);
				Lua.xlua_pushinteger(L, value6);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3) && objectTranslator.Assignable<RaycastHit2D[]>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector2 val7);
				objectTranslator.Get(L, 3, out ContactFilter2D v2);
				RaycastHit2D[] results7 = (RaycastHit2D[])objectTranslator.GetObject(L, 4, typeof(RaycastHit2D[]));
				float distance5 = (float)Lua.lua_tonumber(L, 5);
				int value7 = collider2D.Raycast(val7, v2, results7, distance5);
				Lua.xlua_pushinteger(L, value7);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3) && objectTranslator.Assignable<List<RaycastHit2D>>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector2 val8);
				objectTranslator.Get(L, 3, out ContactFilter2D v3);
				List<RaycastHit2D> results8 = (List<RaycastHit2D>)objectTranslator.GetObject(L, 4, typeof(List<RaycastHit2D>));
				float distance6 = (float)Lua.lua_tonumber(L, 5);
				int value8 = collider2D.Raycast(val8, v3, results8, distance6);
				Lua.xlua_pushinteger(L, value8);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<ContactFilter2D>(L, 3) && objectTranslator.Assignable<List<RaycastHit2D>>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2 val9);
				objectTranslator.Get(L, 3, out ContactFilter2D v4);
				List<RaycastHit2D> results9 = (List<RaycastHit2D>)objectTranslator.GetObject(L, 4, typeof(List<RaycastHit2D>));
				int value9 = collider2D.Raycast(val9, v4, results9);
				Lua.xlua_pushinteger(L, value9);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Collider2D.Raycast!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClosestPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			Vector2 val2 = collider2D.ClosestPoint(val);
			objectTranslator.PushUnityEngineVector2(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_density(IntPtr L)
	{
		try
		{
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, collider2D.density);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isTrigger(IntPtr L)
	{
		try
		{
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, collider2D.isTrigger);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_usedByEffector(IntPtr L)
	{
		try
		{
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, collider2D.usedByEffector);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_usedByComposite(IntPtr L)
	{
		try
		{
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, collider2D.usedByComposite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_composite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, collider2D.composite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_offset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, collider2D.offset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_attachedRigidbody(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, collider2D.attachedRigidbody);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shapeCount(IntPtr L)
	{
		try
		{
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, collider2D.shapeCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, collider2D.bounds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sharedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, collider2D.sharedMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_friction(IntPtr L)
	{
		try
		{
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, collider2D.friction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bounciness(IntPtr L)
	{
		try
		{
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, collider2D.bounciness);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_density(IntPtr L)
	{
		try
		{
			((UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).density = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isTrigger(IntPtr L)
	{
		try
		{
			((UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isTrigger = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_usedByEffector(IntPtr L)
	{
		try
		{
			((UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).usedByEffector = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_usedByComposite(IntPtr L)
	{
		try
		{
			((UnityEngine.Collider2D)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).usedByComposite = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_offset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Collider2D collider2D = (UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			collider2D.offset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sharedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UnityEngine.Collider2D)objectTranslator.FastGetCSObj(L, 1)).sharedMaterial = (PhysicsMaterial2D)objectTranslator.GetObject(L, 2, typeof(PhysicsMaterial2D));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
