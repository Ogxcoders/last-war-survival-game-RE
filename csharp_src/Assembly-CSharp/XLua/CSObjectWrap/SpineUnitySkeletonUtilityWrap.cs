using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SpineUnitySkeletonUtilityWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SkeletonUtility);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 12, 11, 7);
		Utils.RegisterFunc(L, -3, "ResubscribeEvents", _m_ResubscribeEvents);
		Utils.RegisterFunc(L, -3, "RegisterBone", _m_RegisterBone);
		Utils.RegisterFunc(L, -3, "UnregisterBone", _m_UnregisterBone);
		Utils.RegisterFunc(L, -3, "RegisterConstraint", _m_RegisterConstraint);
		Utils.RegisterFunc(L, -3, "UnregisterConstraint", _m_UnregisterConstraint);
		Utils.RegisterFunc(L, -3, "CollectBones", _m_CollectBones);
		Utils.RegisterFunc(L, -3, "GetBoneRoot", _m_GetBoneRoot);
		Utils.RegisterFunc(L, -3, "SpawnRoot", _m_SpawnRoot);
		Utils.RegisterFunc(L, -3, "SpawnHierarchy", _m_SpawnHierarchy);
		Utils.RegisterFunc(L, -3, "SpawnBoneRecursively", _m_SpawnBoneRecursively);
		Utils.RegisterFunc(L, -3, "SpawnBone", _m_SpawnBone);
		Utils.RegisterFunc(L, -3, "OnReset", _e_OnReset);
		Utils.RegisterFunc(L, -2, "SkeletonComponent", _g_get_SkeletonComponent);
		Utils.RegisterFunc(L, -2, "Skeleton", _g_get_Skeleton);
		Utils.RegisterFunc(L, -2, "IsValid", _g_get_IsValid);
		Utils.RegisterFunc(L, -2, "PositionScale", _g_get_PositionScale);
		Utils.RegisterFunc(L, -2, "boneRoot", _g_get_boneRoot);
		Utils.RegisterFunc(L, -2, "flipBy180DegreeRotation", _g_get_flipBy180DegreeRotation);
		Utils.RegisterFunc(L, -2, "skeletonRenderer", _g_get_skeletonRenderer);
		Utils.RegisterFunc(L, -2, "skeletonGraphic", _g_get_skeletonGraphic);
		Utils.RegisterFunc(L, -2, "skeletonAnimation", _g_get_skeletonAnimation);
		Utils.RegisterFunc(L, -2, "boneComponents", _g_get_boneComponents);
		Utils.RegisterFunc(L, -2, "constraintComponents", _g_get_constraintComponents);
		Utils.RegisterFunc(L, -1, "boneRoot", _s_set_boneRoot);
		Utils.RegisterFunc(L, -1, "flipBy180DegreeRotation", _s_set_flipBy180DegreeRotation);
		Utils.RegisterFunc(L, -1, "skeletonRenderer", _s_set_skeletonRenderer);
		Utils.RegisterFunc(L, -1, "skeletonGraphic", _s_set_skeletonGraphic);
		Utils.RegisterFunc(L, -1, "skeletonAnimation", _s_set_skeletonAnimation);
		Utils.RegisterFunc(L, -1, "boneComponents", _s_set_boneComponents);
		Utils.RegisterFunc(L, -1, "constraintComponents", _s_set_constraintComponents);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 6, 0, 0);
		Utils.RegisterFunc(L, -4, "AddBoundingBoxGameObject", _m_AddBoundingBoxGameObject_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddBoundingBoxAsComponent", _m_AddBoundingBoxAsComponent_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetColliderPointsLocal", _m_SetColliderPointsLocal_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetBoundingBoxBounds", _m_GetBoundingBoxBounds_xlua_st_);
		Utils.RegisterFunc(L, -4, "AddBoneRigidbody2D", _m_AddBoneRigidbody2D_xlua_st_);
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
				SkeletonUtility o = new SkeletonUtility();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonUtility constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddBoundingBoxGameObject_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<BoundingBoxAttachment>(L, 2) && objectTranslator.Assignable<Slot>(L, 3) && objectTranslator.Assignable<Transform>(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				string name = Lua.lua_tostring(L, 1);
				BoundingBoxAttachment box = (BoundingBoxAttachment)objectTranslator.GetObject(L, 2, typeof(BoundingBoxAttachment));
				Slot slot = (Slot)objectTranslator.GetObject(L, 3, typeof(Slot));
				Transform parent = (Transform)objectTranslator.GetObject(L, 4, typeof(Transform));
				bool isTrigger = Lua.lua_toboolean(L, 5);
				PolygonCollider2D o = SkeletonUtility.AddBoundingBoxGameObject(name, box, slot, parent, isTrigger);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<BoundingBoxAttachment>(L, 2) && objectTranslator.Assignable<Slot>(L, 3) && objectTranslator.Assignable<Transform>(L, 4))
			{
				string name2 = Lua.lua_tostring(L, 1);
				BoundingBoxAttachment box2 = (BoundingBoxAttachment)objectTranslator.GetObject(L, 2, typeof(BoundingBoxAttachment));
				Slot slot2 = (Slot)objectTranslator.GetObject(L, 3, typeof(Slot));
				Transform parent2 = (Transform)objectTranslator.GetObject(L, 4, typeof(Transform));
				PolygonCollider2D o2 = SkeletonUtility.AddBoundingBoxGameObject(name2, box2, slot2, parent2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Skeleton>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Transform>(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				Skeleton skeleton = (Skeleton)objectTranslator.GetObject(L, 1, typeof(Skeleton));
				string skinName = Lua.lua_tostring(L, 2);
				string slotName = Lua.lua_tostring(L, 3);
				string attachmentName = Lua.lua_tostring(L, 4);
				Transform parent3 = (Transform)objectTranslator.GetObject(L, 5, typeof(Transform));
				bool isTrigger2 = Lua.lua_toboolean(L, 6);
				PolygonCollider2D o3 = SkeletonUtility.AddBoundingBoxGameObject(skeleton, skinName, slotName, attachmentName, parent3, isTrigger2);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Skeleton>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Transform>(L, 5))
			{
				Skeleton skeleton2 = (Skeleton)objectTranslator.GetObject(L, 1, typeof(Skeleton));
				string skinName2 = Lua.lua_tostring(L, 2);
				string slotName2 = Lua.lua_tostring(L, 3);
				string attachmentName2 = Lua.lua_tostring(L, 4);
				Transform parent4 = (Transform)objectTranslator.GetObject(L, 5, typeof(Transform));
				PolygonCollider2D o4 = SkeletonUtility.AddBoundingBoxGameObject(skeleton2, skinName2, slotName2, attachmentName2, parent4);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonUtility.AddBoundingBoxGameObject!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddBoundingBoxAsComponent_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<BoundingBoxAttachment>(L, 1) && objectTranslator.Assignable<Slot>(L, 2) && objectTranslator.Assignable<GameObject>(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				BoundingBoxAttachment box = (BoundingBoxAttachment)objectTranslator.GetObject(L, 1, typeof(BoundingBoxAttachment));
				Slot slot = (Slot)objectTranslator.GetObject(L, 2, typeof(Slot));
				GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
				bool isTrigger = Lua.lua_toboolean(L, 4);
				PolygonCollider2D o = SkeletonUtility.AddBoundingBoxAsComponent(box, slot, gameObject, isTrigger);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<BoundingBoxAttachment>(L, 1) && objectTranslator.Assignable<Slot>(L, 2) && objectTranslator.Assignable<GameObject>(L, 3))
			{
				BoundingBoxAttachment box2 = (BoundingBoxAttachment)objectTranslator.GetObject(L, 1, typeof(BoundingBoxAttachment));
				Slot slot2 = (Slot)objectTranslator.GetObject(L, 2, typeof(Slot));
				GameObject gameObject2 = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
				PolygonCollider2D o2 = SkeletonUtility.AddBoundingBoxAsComponent(box2, slot2, gameObject2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonUtility.AddBoundingBoxAsComponent!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetColliderPointsLocal_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<PolygonCollider2D>(L, 1) && objectTranslator.Assignable<Slot>(L, 2) && objectTranslator.Assignable<BoundingBoxAttachment>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				PolygonCollider2D collider = (PolygonCollider2D)objectTranslator.GetObject(L, 1, typeof(PolygonCollider2D));
				Slot slot = (Slot)objectTranslator.GetObject(L, 2, typeof(Slot));
				BoundingBoxAttachment box = (BoundingBoxAttachment)objectTranslator.GetObject(L, 3, typeof(BoundingBoxAttachment));
				float scale = (float)Lua.lua_tonumber(L, 4);
				SkeletonUtility.SetColliderPointsLocal(collider, slot, box, scale);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<PolygonCollider2D>(L, 1) && objectTranslator.Assignable<Slot>(L, 2) && objectTranslator.Assignable<BoundingBoxAttachment>(L, 3))
			{
				PolygonCollider2D collider2 = (PolygonCollider2D)objectTranslator.GetObject(L, 1, typeof(PolygonCollider2D));
				Slot slot2 = (Slot)objectTranslator.GetObject(L, 2, typeof(Slot));
				BoundingBoxAttachment box2 = (BoundingBoxAttachment)objectTranslator.GetObject(L, 3, typeof(BoundingBoxAttachment));
				SkeletonUtility.SetColliderPointsLocal(collider2, slot2, box2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonUtility.SetColliderPointsLocal!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBoundingBoxBounds_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<BoundingBoxAttachment>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				BoundingBoxAttachment boundingBox = (BoundingBoxAttachment)objectTranslator.GetObject(L, 1, typeof(BoundingBoxAttachment));
				float depth = (float)Lua.lua_tonumber(L, 2);
				Bounds boundingBoxBounds = SkeletonUtility.GetBoundingBoxBounds(boundingBox, depth);
				objectTranslator.PushUnityEngineBounds(L, boundingBoxBounds);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<BoundingBoxAttachment>(L, 1))
			{
				Bounds boundingBoxBounds2 = SkeletonUtility.GetBoundingBoxBounds((BoundingBoxAttachment)objectTranslator.GetObject(L, 1, typeof(BoundingBoxAttachment)));
				objectTranslator.PushUnityEngineBounds(L, boundingBoxBounds2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonUtility.GetBoundingBoxBounds!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddBoneRigidbody2D_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<GameObject>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject));
				bool isKinematic = Lua.lua_toboolean(L, 2);
				float gravityScale = (float)Lua.lua_tonumber(L, 3);
				Rigidbody2D o = SkeletonUtility.AddBoneRigidbody2D(gameObject, isKinematic, gravityScale);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<GameObject>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				GameObject gameObject2 = (GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject));
				bool isKinematic2 = Lua.lua_toboolean(L, 2);
				Rigidbody2D o2 = SkeletonUtility.AddBoneRigidbody2D(gameObject2, isKinematic2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<GameObject>(L, 1))
			{
				Rigidbody2D o3 = SkeletonUtility.AddBoneRigidbody2D((GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject)));
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonUtility.AddBoneRigidbody2D!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResubscribeEvents(IntPtr L)
	{
		try
		{
			((SkeletonUtility)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResubscribeEvents();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterBone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			SkeletonUtilityBone bone = (SkeletonUtilityBone)objectTranslator.GetObject(L, 2, typeof(SkeletonUtilityBone));
			skeletonUtility.RegisterBone(bone);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnregisterBone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			SkeletonUtilityBone bone = (SkeletonUtilityBone)objectTranslator.GetObject(L, 2, typeof(SkeletonUtilityBone));
			skeletonUtility.UnregisterBone(bone);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterConstraint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			SkeletonUtilityConstraint constraint = (SkeletonUtilityConstraint)objectTranslator.GetObject(L, 2, typeof(SkeletonUtilityConstraint));
			skeletonUtility.RegisterConstraint(constraint);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnregisterConstraint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			SkeletonUtilityConstraint constraint = (SkeletonUtilityConstraint)objectTranslator.GetObject(L, 2, typeof(SkeletonUtilityConstraint));
			skeletonUtility.UnregisterConstraint(constraint);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CollectBones(IntPtr L)
	{
		try
		{
			((SkeletonUtility)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CollectBones();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBoneRoot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform boneRoot = ((SkeletonUtility)objectTranslator.FastGetCSObj(L, 1)).GetBoneRoot();
			objectTranslator.Push(L, boneRoot);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SpawnRoot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SkeletonUtilityBone.Mode v);
			GameObject o = skeletonUtility.SpawnRoot(pos: Lua.lua_toboolean(L, 3), rot: Lua.lua_toboolean(L, 4), sca: Lua.lua_toboolean(L, 5), mode: v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SpawnHierarchy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SkeletonUtilityBone.Mode v);
			GameObject o = skeletonUtility.SpawnHierarchy(pos: Lua.lua_toboolean(L, 3), rot: Lua.lua_toboolean(L, 4), sca: Lua.lua_toboolean(L, 5), mode: v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SpawnBoneRecursively(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			Bone bone = (Bone)objectTranslator.GetObject(L, 2, typeof(Bone));
			Transform parent = (Transform)objectTranslator.GetObject(L, 3, typeof(Transform));
			objectTranslator.Get(L, 4, out SkeletonUtilityBone.Mode v);
			GameObject o = skeletonUtility.SpawnBoneRecursively(pos: Lua.lua_toboolean(L, 5), rot: Lua.lua_toboolean(L, 6), sca: Lua.lua_toboolean(L, 7), bone: bone, parent: parent, mode: v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SpawnBone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			Bone bone = (Bone)objectTranslator.GetObject(L, 2, typeof(Bone));
			Transform parent = (Transform)objectTranslator.GetObject(L, 3, typeof(Transform));
			objectTranslator.Get(L, 4, out SkeletonUtilityBone.Mode v);
			GameObject o = skeletonUtility.SpawnBone(pos: Lua.lua_toboolean(L, 5), rot: Lua.lua_toboolean(L, 6), sca: Lua.lua_toboolean(L, 7), bone: bone, parent: parent, mode: v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SkeletonComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, skeletonUtility.SkeletonComponent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Skeleton(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonUtility.Skeleton);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsValid(IntPtr L)
	{
		try
		{
			SkeletonUtility skeletonUtility = (SkeletonUtility)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonUtility.IsValid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PositionScale(IntPtr L)
	{
		try
		{
			SkeletonUtility skeletonUtility = (SkeletonUtility)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, skeletonUtility.PositionScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_boneRoot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonUtility.boneRoot);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_flipBy180DegreeRotation(IntPtr L)
	{
		try
		{
			SkeletonUtility skeletonUtility = (SkeletonUtility)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonUtility.flipBy180DegreeRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skeletonRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonUtility.skeletonRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skeletonGraphic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonUtility.skeletonGraphic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skeletonAnimation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, skeletonUtility.skeletonAnimation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_boneComponents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonUtility.boneComponents);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_constraintComponents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonUtility.constraintComponents);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_boneRoot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonUtility)objectTranslator.FastGetCSObj(L, 1)).boneRoot = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_flipBy180DegreeRotation(IntPtr L)
	{
		try
		{
			((SkeletonUtility)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).flipBy180DegreeRotation = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skeletonRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonUtility)objectTranslator.FastGetCSObj(L, 1)).skeletonRenderer = (SkeletonRenderer)objectTranslator.GetObject(L, 2, typeof(SkeletonRenderer));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skeletonGraphic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonUtility)objectTranslator.FastGetCSObj(L, 1)).skeletonGraphic = (SkeletonGraphic)objectTranslator.GetObject(L, 2, typeof(SkeletonGraphic));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skeletonAnimation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonUtility)objectTranslator.FastGetCSObj(L, 1)).skeletonAnimation = (ISkeletonAnimation)objectTranslator.GetObject(L, 2, typeof(ISkeletonAnimation));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_boneComponents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonUtility)objectTranslator.FastGetCSObj(L, 1)).boneComponents = (List<SkeletonUtilityBone>)objectTranslator.GetObject(L, 2, typeof(List<SkeletonUtilityBone>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_constraintComponents(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonUtility)objectTranslator.FastGetCSObj(L, 1)).constraintComponents = (List<SkeletonUtilityConstraint>)objectTranslator.GetObject(L, 2, typeof(List<SkeletonUtilityConstraint>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnReset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonUtility skeletonUtility = (SkeletonUtility)objectTranslator.FastGetCSObj(L, 1);
			SkeletonUtility.SkeletonUtilityDelegate skeletonUtilityDelegate = objectTranslator.GetDelegate<SkeletonUtility.SkeletonUtilityDelegate>(L, 3);
			if (skeletonUtilityDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.SkeletonUtility.SkeletonUtilityDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonUtility.OnReset += skeletonUtilityDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonUtility.OnReset -= skeletonUtilityDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonUtility.OnReset!");
		return 0;
	}
}
