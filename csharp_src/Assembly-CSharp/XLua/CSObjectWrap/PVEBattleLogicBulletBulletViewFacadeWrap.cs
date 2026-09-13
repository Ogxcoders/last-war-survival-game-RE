using System;
using PVEBattleLogic.Bullet;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PVEBattleLogicBulletBulletViewFacadeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BulletViewFacade);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 54, 0, 0);
		Utils.RegisterFunc(L, -4, "Init", _m_Init_xlua_st_);
		Utils.RegisterFunc(L, -4, "SyncNameId", _m_SyncNameId_xlua_st_);
		Utils.RegisterFunc(L, -4, "SyncAnimationCurve", _m_SyncAnimationCurve_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateBulletView", _m_CreateBulletView_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsLoaded", _m_IsLoaded_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetVisible", _m_SetVisible_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetParent", _m_SetParent_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetLocalPosition", _m_SetLocalPosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetLocalPosition", _m_ResetLocalPosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetPosition", _m_SetPosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetLocalScale", _m_SetLocalScale_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetEulerAngles", _m_SetEulerAngles_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetEulerAnglesY", _m_GetEulerAnglesY_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetLocalRotation", _m_ResetLocalRotation_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPosition", _m_GetPosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPositionXYZ", _m_GetPositionXYZ_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetForward", _m_SetForward_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetForward", _m_GetForward_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetRight", _m_GetRight_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetUp", _m_GetUp_xlua_st_);
		Utils.RegisterFunc(L, -4, "Translate", _m_Translate_xlua_st_);
		Utils.RegisterFunc(L, -4, "LookAt", _m_LookAt_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTransformPoint", _m_GetTransformPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetColliderCenterWorldPos", _m_GetColliderCenterWorldPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetLineRendererArray", _m_GetLineRendererArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "TryGetSphereCollider", _m_TryGetSphereCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "TryGetCapsuleCollider", _m_TryGetCapsuleCollider_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearTrailRenderer", _m_ClearTrailRenderer_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetBulletViewTransform", _m_GetBulletViewTransform_xlua_st_);
		Utils.RegisterFunc(L, -4, "SyncBulletTransform", _m_SyncBulletTransform_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckLoaded", _m_CheckLoaded_xlua_st_);
		Utils.RegisterFunc(L, -4, "DestroyBulletView", _m_DestroyBulletView_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateBulletViewStraight", _m_CreateBulletViewStraight_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateBulletViewStraightListLuaArray", _m_CreateBulletViewStraightListLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateBulletViewStraightLuaArray", _m_CreateBulletViewStraightLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateBulletViewStraightParam", _m_CreateBulletViewStraightParam_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateBulletViewStraightParamWithInertiaVelocity", _m_CreateBulletViewStraightParamWithInertiaVelocity_xlua_st_);
		Utils.RegisterFunc(L, -4, "SyncStraightGatlingViewDataLuaArray", _m_SyncStraightGatlingViewDataLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateStraightGatlingViewListLuaArray", _m_CreateStraightGatlingViewListLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "CreateStraightGatlingViewLuaArray", _m_CreateStraightGatlingViewLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "DestroyBulletViewStraight", _m_DestroyBulletViewStraight_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetAnimationCurve", _m_GetAnimationCurve_xlua_st_);
		Utils.RegisterFunc(L, -4, "TryGetStraightViewParam", _m_TryGetStraightViewParam_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateStraight", _m_UpdateStraight_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateStraightLuaArray", _m_UpdateStraightLuaArray_xlua_st_);
		Utils.RegisterFunc(L, -4, "PreloadBullet", _m_PreloadBullet_xlua_st_);
		Utils.RegisterFunc(L, -4, "PreloadStraight", _m_PreloadStraight_xlua_st_);
		Utils.RegisterFunc(L, -4, "StraightLogicDie", _m_StraightLogicDie_xlua_st_);
		Utils.RegisterFunc(L, -4, "InitBulletViewArrayAccess", _m_InitBulletViewArrayAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnInitBulletViewArrayAccess", _m_UnInitBulletViewArrayAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearAll", _m_ClearAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Dispose", _m_Dispose_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "INVALID_HANDLE", -1);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "PVEBattleLogic.Bullet.BulletViewFacade does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncNameId_xlua_st_(IntPtr L)
	{
		try
		{
			string bulletViewName = Lua.lua_tostring(L, 1);
			int bulletViewNameId = Lua.xlua_tointeger(L, 2);
			BulletViewFacade.SyncNameId(bulletViewName, bulletViewNameId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncAnimationCurve_xlua_st_(IntPtr L)
	{
		try
		{
			string animationCurve = Lua.lua_tostring(L, 1);
			int curveId = Lua.xlua_tointeger(L, 2);
			BulletViewFacade.SyncAnimationCurve(animationCurve, curveId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateBulletView_xlua_st_(IntPtr L)
	{
		try
		{
			bool loaded;
			int value = BulletViewFacade.CreateBulletView(Lua.xlua_tointeger(L, 1), out loaded);
			Lua.xlua_pushinteger(L, value);
			Lua.lua_pushboolean(L, loaded);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsLoaded_xlua_st_(IntPtr L)
	{
		try
		{
			float dotMaxCD;
			bool value = BulletViewFacade.IsLoaded(Lua.xlua_tointeger(L, 1), out dotMaxCD);
			Lua.lua_pushboolean(L, value);
			Lua.lua_pushnumber(L, dotMaxCD);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVisible_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			bool visible = Lua.lua_toboolean(L, 2);
			BulletViewFacade.SetVisible(handle, visible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetParent_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int handle = Lua.xlua_tointeger(L, 1);
			Transform parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			BulletViewFacade.SetParent(handle, parent);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalPosition_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			BulletViewFacade.SetLocalPosition(handle, x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetLocalPosition_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.ResetLocalPosition(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPosition_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			BulletViewFacade.SetPosition(handle, x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalScale_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			BulletViewFacade.SetLocalScale(handle, x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetEulerAngles_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			BulletViewFacade.SetEulerAngles(handle, x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetEulerAnglesY_xlua_st_(IntPtr L)
	{
		try
		{
			float eulerAnglesY = BulletViewFacade.GetEulerAnglesY(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushnumber(L, eulerAnglesY);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetLocalRotation_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.ResetLocalRotation(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPosition_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 position = BulletViewFacade.GetPosition(Lua.xlua_tointeger(L, 1));
			objectTranslator.PushUnityEngineVector3(L, position);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPositionXYZ_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.GetPositionXYZ(Lua.xlua_tointeger(L, 1), out var x, out var y, out var z);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetForward_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			BulletViewFacade.SetForward(handle, x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetForward_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.GetForward(Lua.xlua_tointeger(L, 1), out var x, out var y, out var z);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRight_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.GetRight(Lua.xlua_tointeger(L, 1), out var x, out var y, out var z);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUp_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.GetUp(Lua.xlua_tointeger(L, 1), out var x, out var y, out var z);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Translate_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int handle = Lua.xlua_tointeger(L, 1);
				float translationZ = (float)Lua.lua_tonumber(L, 2);
				BulletViewFacade.Translate(handle, translationZ);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int handle2 = Lua.xlua_tointeger(L, 1);
				float translationX = (float)Lua.lua_tonumber(L, 2);
				float translationY = (float)Lua.lua_tonumber(L, 3);
				float translationZ2 = (float)Lua.lua_tonumber(L, 4);
				BulletViewFacade.Translate(handle2, translationX, translationY, translationZ2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVEBattleLogic.Bullet.BulletViewFacade.Translate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LookAt_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			BulletViewFacade.LookAt(handle, x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTransformPoint_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float posX = (float)Lua.lua_tonumber(L, 2);
			float posY = (float)Lua.lua_tonumber(L, 3);
			float posZ = (float)Lua.lua_tonumber(L, 4);
			BulletViewFacade.GetTransformPoint(handle, posX, posY, posZ, out var x, out var y, out var z);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetColliderCenterWorldPos_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.GetColliderCenterWorldPos(Lua.xlua_tointeger(L, 1), out var x, out var y, out var z);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLineRendererArray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LineRenderer[] lineRendererArray = BulletViewFacade.GetLineRendererArray(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, lineRendererArray);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetSphereCollider_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SphereCollider sphereCollider;
			bool value = BulletViewFacade.TryGetSphereCollider(Lua.xlua_tointeger(L, 1), out sphereCollider);
			Lua.lua_pushboolean(L, value);
			objectTranslator.Push(L, sphereCollider);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetCapsuleCollider_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CapsuleCollider capsuleCollider;
			bool value = BulletViewFacade.TryGetCapsuleCollider(Lua.xlua_tointeger(L, 1), out capsuleCollider);
			Lua.lua_pushboolean(L, value);
			objectTranslator.Push(L, capsuleCollider);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearTrailRenderer_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.ClearTrailRenderer(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBulletViewTransform_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform bulletViewTransform = BulletViewFacade.GetBulletViewTransform(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, bulletViewTransform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncBulletTransform_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.SyncBulletTransform((LuaTable)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaTable)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckLoaded_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			switch (Lua.lua_gettop(L))
			{
			case 0:
				BulletViewFacade.CheckLoaded();
				return 0;
			case 1:
				if (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TTABLE)
				{
					LuaTable o = BulletViewFacade.CheckLoaded((LuaTable)objectTranslator.GetObject(L, 1, typeof(LuaTable)));
					objectTranslator.Push(L, o);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PVEBattleLogic.Bullet.BulletViewFacade.CheckLoaded!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyBulletView_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			BulletViewFacade.DestroyBulletView(ref handle);
			Lua.xlua_pushinteger(L, handle);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateBulletViewStraight_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string bulletViewName = Lua.lua_tostring(L, 1);
			LuaTable paramTable = (LuaTable)objectTranslator.GetObject(L, 2, typeof(LuaTable));
			bool loaded;
			float dotMaxCD;
			int value = BulletViewFacade.CreateBulletViewStraight(bulletViewName, paramTable, out loaded, out dotMaxCD);
			Lua.xlua_pushinteger(L, value);
			Lua.lua_pushboolean(L, loaded);
			Lua.lua_pushnumber(L, dotMaxCD);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateBulletViewStraightListLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BulletViewFacade.CreateBulletViewStraightListLuaArray(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateBulletViewStraightLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			bool loaded;
			float dotMaxCD;
			int value = BulletViewFacade.CreateBulletViewStraightLuaArray(out loaded, out dotMaxCD);
			Lua.xlua_pushinteger(L, value);
			Lua.lua_pushboolean(L, loaded);
			Lua.lua_pushnumber(L, dotMaxCD);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateBulletViewStraightParam_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string bulletViewName = Lua.lua_tostring(L, 1);
			long bulletObjId = Lua.lua_toint64(L, 2);
			float startX = (float)Lua.lua_tonumber(L, 3);
			float startY = (float)Lua.lua_tonumber(L, 4);
			float startZ = (float)Lua.lua_tonumber(L, 5);
			float rotY = (float)Lua.lua_tonumber(L, 6);
			int targetLayerMask = Lua.xlua_tointeger(L, 7);
			int metaId = Lua.xlua_tointeger(L, 8);
			LuaTable paramTable = (LuaTable)objectTranslator.GetObject(L, 9, typeof(LuaTable));
			bool loaded;
			float dotMaxCD;
			int value = BulletViewFacade.CreateBulletViewStraightParam(bulletViewName, bulletObjId, startX, startY, startZ, rotY, targetLayerMask, metaId, paramTable, out loaded, out dotMaxCD);
			Lua.xlua_pushinteger(L, value);
			Lua.lua_pushboolean(L, loaded);
			Lua.lua_pushnumber(L, dotMaxCD);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateBulletViewStraightParamWithInertiaVelocity_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string bulletViewName = Lua.lua_tostring(L, 1);
			long bulletObjId = Lua.lua_toint64(L, 2);
			float startX = (float)Lua.lua_tonumber(L, 3);
			float startY = (float)Lua.lua_tonumber(L, 4);
			float startZ = (float)Lua.lua_tonumber(L, 5);
			float rotY = (float)Lua.lua_tonumber(L, 6);
			int targetLayerMask = Lua.xlua_tointeger(L, 7);
			float inertiaX = (float)Lua.lua_tonumber(L, 8);
			float inertiaY = (float)Lua.lua_tonumber(L, 9);
			float inertiaZ = (float)Lua.lua_tonumber(L, 10);
			int metaId = Lua.xlua_tointeger(L, 11);
			LuaTable paramTable = (LuaTable)objectTranslator.GetObject(L, 12, typeof(LuaTable));
			bool loaded;
			float dotMaxCD;
			int value = BulletViewFacade.CreateBulletViewStraightParamWithInertiaVelocity(bulletViewName, bulletObjId, startX, startY, startZ, rotY, targetLayerMask, inertiaX, inertiaY, inertiaZ, metaId, paramTable, out loaded, out dotMaxCD);
			Lua.xlua_pushinteger(L, value);
			Lua.lua_pushboolean(L, loaded);
			Lua.lua_pushnumber(L, dotMaxCD);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncStraightGatlingViewDataLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.SyncStraightGatlingViewDataLuaArray();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateStraightGatlingViewListLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BulletViewFacade.CreateStraightGatlingViewListLuaArray(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateStraightGatlingViewLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			int value = BulletViewFacade.CreateStraightGatlingViewLuaArray();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyBulletViewStraight_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			BulletViewFacade.DestroyBulletViewStraight(ref handle);
			Lua.xlua_pushinteger(L, handle);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAnimationCurve_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationCurve animationCurve = BulletViewFacade.GetAnimationCurve(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, animationCurve);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetStraightViewParam_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BulletStraightViewParam bulletStraightViewParam;
			bool value = BulletViewFacade.TryGetStraightViewParam(Lua.xlua_tointeger(L, 1), out bulletStraightViewParam);
			Lua.lua_pushboolean(L, value);
			objectTranslator.Push(L, bulletStraightViewParam);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateStraight_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LuaTable o = BulletViewFacade.UpdateStraight((float)Lua.lua_tonumber(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateStraightLuaArray_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = BulletViewFacade.UpdateStraightLuaArray((float)Lua.lua_tonumber(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PreloadBullet_xlua_st_(IntPtr L)
	{
		try
		{
			string bulletView = Lua.lua_tostring(L, 1);
			int count = Lua.xlua_tointeger(L, 2);
			BulletViewFacade.PreloadBullet(bulletView, count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PreloadStraight_xlua_st_(IntPtr L)
	{
		try
		{
			string bulletView = Lua.lua_tostring(L, 1);
			int count = Lua.xlua_tointeger(L, 2);
			BulletViewFacade.PreloadStraight(bulletView, count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StraightLogicDie_xlua_st_(IntPtr L)
	{
		try
		{
			int handle = Lua.xlua_tointeger(L, 1);
			float delayTime = (float)Lua.lua_tonumber(L, 2);
			BulletViewFacade.StraightLogicDie(handle, delayTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitBulletViewArrayAccess_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LuaArrAccess luaArrAccess = (LuaArrAccess)objectTranslator.GetObject(L, 1, typeof(LuaArrAccess));
			LuaArrAccess createStraightLuaArrAccess = (LuaArrAccess)objectTranslator.GetObject(L, 2, typeof(LuaArrAccess));
			LuaArrAccess createStraightListArrAccess = (LuaArrAccess)objectTranslator.GetObject(L, 3, typeof(LuaArrAccess));
			LuaArrAccess createGatlingStraightLuaArrAccess = (LuaArrAccess)objectTranslator.GetObject(L, 4, typeof(LuaArrAccess));
			BulletViewFacade.InitBulletViewArrayAccess(luaArrAccess, createStraightLuaArrAccess, createStraightListArrAccess, createGatlingStraightLuaArrAccess);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInitBulletViewArrayAccess_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.UnInitBulletViewArrayAccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAll_xlua_st_(IntPtr L)
	{
		try
		{
			BulletViewFacade.ClearAll();
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
			BulletViewFacade.Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
