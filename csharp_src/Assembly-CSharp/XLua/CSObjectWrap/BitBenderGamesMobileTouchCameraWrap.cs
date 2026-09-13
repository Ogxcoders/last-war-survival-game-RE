using System;
using System.Collections.Generic;
using BitBenderGames;
using BitBenderGames.CameraState;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BitBenderGamesMobileTouchCameraWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MobileTouchCamera);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 44, 39, 17);
		Utils.RegisterFunc(L, -3, "HideCamera", _m_HideCamera);
		Utils.RegisterFunc(L, -3, "ShowCamera", _m_ShowCamera);
		Utils.RegisterFunc(L, -3, "SetRotRangeOverride", _m_SetRotRangeOverride);
		Utils.RegisterFunc(L, -3, "SetZoomRangeOverride", _m_SetZoomRangeOverride);
		Utils.RegisterFunc(L, -3, "CalcZoom", _m_CalcZoom);
		Utils.RegisterFunc(L, -3, "SetIsCamZoomChangeBlock", _m_SetIsCamZoomChangeBlock);
		Utils.RegisterFunc(L, -3, "GetIsCamZoomChangeBlock", _m_GetIsCamZoomChangeBlock);
		Utils.RegisterFunc(L, -3, "GetZoomParams", _m_GetZoomParams);
		Utils.RegisterFunc(L, -3, "SetZoomParams", _m_SetZoomParams);
		Utils.RegisterFunc(L, -3, "Awake", _m_Awake);
		Utils.RegisterFunc(L, -3, "ResetCamera", _m_ResetCamera);
		Utils.RegisterFunc(L, -3, "OnDestroy", _m_OnDestroy);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "Follow", _m_Follow);
		Utils.RegisterFunc(L, -3, "BeginSyncWithTimeline", _m_BeginSyncWithTimeline);
		Utils.RegisterFunc(L, -3, "EndSyncWithTimeline", _m_EndSyncWithTimeline);
		Utils.RegisterFunc(L, -3, "LockCamera", _m_LockCamera);
		Utils.RegisterFunc(L, -3, "FreeCamera", _m_FreeCamera);
		Utils.RegisterFunc(L, -3, "AutoLookat", _m_AutoLookat);
		Utils.RegisterFunc(L, -3, "AutoZoom", _m_AutoZoom);
		Utils.RegisterFunc(L, -3, "AutoFocus", _m_AutoFocus);
		Utils.RegisterFunc(L, -3, "QuitFocus", _m_QuitFocus);
		Utils.RegisterFunc(L, -3, "StopMove", _m_StopMove);
		Utils.RegisterFunc(L, -3, "LookAt", _m_LookAt);
		Utils.RegisterFunc(L, -3, "GetState", _m_GetState);
		Utils.RegisterFunc(L, -3, "SetState", _m_SetState);
		Utils.RegisterFunc(L, -3, "InputDonwOnUI", _m_InputDonwOnUI);
		Utils.RegisterFunc(L, -3, "GetIntersectionPoint", _m_GetIntersectionPoint);
		Utils.RegisterFunc(L, -3, "RaycastGround", _m_RaycastGround);
		Utils.RegisterFunc(L, -3, "UnprojectVector2", _m_UnprojectVector2);
		Utils.RegisterFunc(L, -3, "ProjectVector3", _m_ProjectVector3);
		Utils.RegisterFunc(L, -3, "GetCameraTargetPos", _m_GetCameraTargetPos);
		Utils.RegisterFunc(L, -3, "GetCameraPos", _m_GetCameraPos);
		Utils.RegisterFunc(L, -3, "SetCameraPos", _m_SetCameraPos);
		Utils.RegisterFunc(L, -3, "GetRotation", _m_GetRotation);
		Utils.RegisterFunc(L, -3, "SetRotation", _m_SetRotation);
		Utils.RegisterFunc(L, -3, "SetFov", _m_SetFov);
		Utils.RegisterFunc(L, -3, "GetFov", _m_GetFov);
		Utils.RegisterFunc(L, -3, "ScreenPointToRay", _m_ScreenPointToRay);
		Utils.RegisterFunc(L, -3, "GetTouchTerrainPos", _m_GetTouchTerrainPos);
		Utils.RegisterFunc(L, -3, "WorldToScreenPoint", _m_WorldToScreenPoint);
		Utils.RegisterFunc(L, -3, "GetZoomSensitivity", _m_GetZoomSensitivity);
		Utils.RegisterFunc(L, -3, "AdjustTarget", _m_AdjustTarget);
		Utils.RegisterFunc(L, -3, "OnForceChangePositionEvent", _e_OnForceChangePositionEvent);
		Utils.RegisterFunc(L, -2, "CurrentState", _g_get_CurrentState);
		Utils.RegisterFunc(L, -2, "CurrentCameraState", _g_get_CurrentCameraState);
		Utils.RegisterFunc(L, -2, "RefPlane", _g_get_RefPlane);
		Utils.RegisterFunc(L, -2, "LodLevel", _g_get_LodLevel);
		Utils.RegisterFunc(L, -2, "use45XYCamera", _g_get_use45XYCamera);
		Utils.RegisterFunc(L, -2, "use45XCamera", _g_get_use45XCamera);
		Utils.RegisterFunc(L, -2, "CamZoomInit", _g_get_CamZoomInit);
		Utils.RegisterFunc(L, -2, "CamZoomMin", _g_get_CamZoomMin);
		Utils.RegisterFunc(L, -2, "CamZoomMax", _g_get_CamZoomMax);
		Utils.RegisterFunc(L, -2, "CamZoomMaxCity", _g_get_CamZoomMaxCity);
		Utils.RegisterFunc(L, -2, "CamZoomMinCity", _g_get_CamZoomMinCity);
		Utils.RegisterFunc(L, -2, "CamZoomMaxWorld", _g_get_CamZoomMaxWorld);
		Utils.RegisterFunc(L, -2, "CamZoomMinWorld", _g_get_CamZoomMinWorld);
		Utils.RegisterFunc(L, -2, "CamOverZoomMargin", _g_get_CamOverZoomMargin);
		Utils.RegisterFunc(L, -2, "CamZoom", _g_get_CamZoom);
		Utils.RegisterFunc(L, -2, "CanMoveing", _g_get_CanMoveing);
		Utils.RegisterFunc(L, -2, "DampFactorTimeMultiplier", _g_get_DampFactorTimeMultiplier);
		Utils.RegisterFunc(L, -2, "AutoScrollVelocityMax", _g_get_AutoScrollVelocityMax);
		Utils.RegisterFunc(L, -2, "AutoScrollDamp", _g_get_AutoScrollDamp);
		Utils.RegisterFunc(L, -2, "AutoScrollDamps", _g_get_AutoScrollDamps);
		Utils.RegisterFunc(L, -2, "AutoScrollDampCurve", _g_get_AutoScrollDampCurve);
		Utils.RegisterFunc(L, -2, "CamFollowFactor", _g_get_CamFollowFactor);
		Utils.RegisterFunc(L, -2, "CamZoomFarmPlant", _g_get_CamZoomFarmPlant);
		Utils.RegisterFunc(L, -2, "CamZoomFarmPlantRotation", _g_get_CamZoomFarmPlantRotation);
		Utils.RegisterFunc(L, -2, "CamZoomBuild", _g_get_CamZoomBuild);
		Utils.RegisterFunc(L, -2, "CamZoomFocusRotation", _g_get_CamZoomFocusRotation);
		Utils.RegisterFunc(L, -2, "CamZoomFormation", _g_get_CamZoomFormation);
		Utils.RegisterFunc(L, -2, "CamZoomFocusFormationRotation", _g_get_CamZoomFocusFormationRotation);
		Utils.RegisterFunc(L, -2, "CamZoomEarthOrder", _g_get_CamZoomEarthOrder);
		Utils.RegisterFunc(L, -2, "CamZoomDome", _g_get_CamZoomDome);
		Utils.RegisterFunc(L, -2, "CamZoomMoveCity", _g_get_CamZoomMoveCity);
		Utils.RegisterFunc(L, -2, "CamZoomFocusEarthOrderRotation", _g_get_CamZoomFocusEarthOrderRotation);
		Utils.RegisterFunc(L, -2, "CamZoomFocusMoveCityRotation", _g_get_CamZoomFocusMoveCityRotation);
		Utils.RegisterFunc(L, -2, "CamZoomInitRotation", _g_get_CamZoomInitRotation);
		Utils.RegisterFunc(L, -2, "CameraFocusCurve", _g_get_CameraFocusCurve);
		Utils.RegisterFunc(L, -2, "CameraFocusEarthCurve", _g_get_CameraFocusEarthCurve);
		Utils.RegisterFunc(L, -2, "CameraFocusDomeCurve", _g_get_CameraFocusDomeCurve);
		Utils.RegisterFunc(L, -2, "CameraFocusMoveCityCurve", _g_get_CameraFocusMoveCityCurve);
		Utils.RegisterFunc(L, -2, "touchInput", _g_get_touchInput);
		Utils.RegisterFunc(L, -1, "LodLevel", _s_set_LodLevel);
		Utils.RegisterFunc(L, -1, "use45XYCamera", _s_set_use45XYCamera);
		Utils.RegisterFunc(L, -1, "use45XCamera", _s_set_use45XCamera);
		Utils.RegisterFunc(L, -1, "CamZoomInit", _s_set_CamZoomInit);
		Utils.RegisterFunc(L, -1, "CamZoomMin", _s_set_CamZoomMin);
		Utils.RegisterFunc(L, -1, "CamZoomMax", _s_set_CamZoomMax);
		Utils.RegisterFunc(L, -1, "CamZoom", _s_set_CamZoom);
		Utils.RegisterFunc(L, -1, "CanMoveing", _s_set_CanMoveing);
		Utils.RegisterFunc(L, -1, "CamZoomFarmPlant", _s_set_CamZoomFarmPlant);
		Utils.RegisterFunc(L, -1, "CamZoomFarmPlantRotation", _s_set_CamZoomFarmPlantRotation);
		Utils.RegisterFunc(L, -1, "CamZoomBuild", _s_set_CamZoomBuild);
		Utils.RegisterFunc(L, -1, "CamZoomFocusRotation", _s_set_CamZoomFocusRotation);
		Utils.RegisterFunc(L, -1, "CamZoomFormation", _s_set_CamZoomFormation);
		Utils.RegisterFunc(L, -1, "CamZoomFocusFormationRotation", _s_set_CamZoomFocusFormationRotation);
		Utils.RegisterFunc(L, -1, "BeforeUpdate", _s_set_BeforeUpdate);
		Utils.RegisterFunc(L, -1, "AfterUpdate", _s_set_AfterUpdate);
		Utils.RegisterFunc(L, -1, "touchInput", _s_set_touchInput);
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
				MobileTouchCamera o = new MobileTouchCamera();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideCamera(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HideCamera();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowCamera(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowCamera();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRotRangeOverride(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			bool overrideValue = Lua.lua_toboolean(L, 2);
			objectTranslator.Get(L, 3, out Vector2 val);
			mobileTouchCamera.SetRotRangeOverride(overrideValue, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetZoomRangeOverride(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			bool overrideValue = Lua.lua_toboolean(L, 2);
			objectTranslator.Get(L, 3, out Vector2 val);
			mobileTouchCamera.SetZoomRangeOverride(overrideValue, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalcZoom(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			float cameraY = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Get(L, 3, out Vector3 val);
			Vector3 outPos;
			Quaternion outRot;
			bool value = mobileTouchCamera.CalcZoom(cameraY, val, out outPos, out outRot);
			Lua.lua_pushboolean(L, value);
			objectTranslator.PushUnityEngineVector3(L, outPos);
			objectTranslator.PushUnityEngineQuaternion(L, outRot);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIsCamZoomChangeBlock(IntPtr L)
	{
		try
		{
			MobileTouchCamera obj = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isCamZoomChangeBlock = Lua.lua_toboolean(L, 2);
			obj.SetIsCamZoomChangeBlock(isCamZoomChangeBlock);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIsCamZoomChangeBlock(IntPtr L)
	{
		try
		{
			bool isCamZoomChangeBlock = ((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetIsCamZoomChangeBlock();
			Lua.lua_pushboolean(L, isCamZoomChangeBlock);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetZoomParams(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<MobileTouchCamera.ZoomParam> zoomParams = ((MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1)).GetZoomParams();
			objectTranslator.Push(L, zoomParams);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetZoomParams(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int level = Lua.xlua_tointeger(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float offsetZ = (float)Lua.lua_tonumber(L, 4);
				float sensitivity = (float)Lua.lua_tonumber(L, 5);
				mobileTouchCamera.SetZoomParams(level, y, offsetZ, sensitivity);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<List<MobileTouchCamera.ZoomParam>>(L, 2))
			{
				List<MobileTouchCamera.ZoomParam> zoomParams = (List<MobileTouchCamera.ZoomParam>)objectTranslator.GetObject(L, 2, typeof(List<MobileTouchCamera.ZoomParam>));
				mobileTouchCamera.SetZoomParams(zoomParams);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.SetZoomParams!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Awake(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Awake();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetCamera(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetCamera();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDestroy(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDestroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Follow(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<GameObject>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				GameObject go = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
				float time = (float)Lua.lua_tonumber(L, 3);
				float offset = (float)Lua.lua_tonumber(L, 4);
				mobileTouchCamera.Follow(go, time, offset);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<GameObject>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				GameObject go2 = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
				float time2 = (float)Lua.lua_tonumber(L, 3);
				mobileTouchCamera.Follow(go2, time2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.Follow!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BeginSyncWithTimeline(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			Camera camInTimeline = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
			float transitionTime = (float)Lua.lua_tonumber(L, 3);
			mobileTouchCamera.BeginSyncWithTimeline(camInTimeline, transitionTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EndSyncWithTimeline(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EndSyncWithTimeline();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LockCamera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			float duration = (float)Lua.lua_tonumber(L, 3);
			mobileTouchCamera.LockCamera(val, duration);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FreeCamera(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FreeCamera();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AutoLookat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			float zoom = (float)Lua.lua_tonumber(L, 3);
			float time = (float)Lua.lua_tonumber(L, 4);
			Action onCompete = objectTranslator.GetDelegate<Action>(L, 5);
			mobileTouchCamera.AutoLookat(val, zoom, time, onCompete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AutoZoom(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			float zoom = (float)Lua.lua_tonumber(L, 2);
			float time = (float)Lua.lua_tonumber(L, 3);
			Action onComplete = objectTranslator.GetDelegate<Action>(L, 4);
			mobileTouchCamera.AutoZoom(zoom, time, onComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AutoFocus(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			float zoom = (float)Lua.lua_tonumber(L, 3);
			float time = (float)Lua.lua_tonumber(L, 4);
			float rotation = (float)Lua.lua_tonumber(L, 5);
			bool focusToCenter = Lua.lua_toboolean(L, 6);
			bool lockView = Lua.lua_toboolean(L, 7);
			AnimationCurve curve = (AnimationCurve)objectTranslator.GetObject(L, 8, typeof(AnimationCurve));
			Action onCompete = objectTranslator.GetDelegate<Action>(L, 9);
			mobileTouchCamera.AutoFocus(val, zoom, time, rotation, focusToCenter, lockView, curve, onCompete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_QuitFocus(IntPtr L)
	{
		try
		{
			MobileTouchCamera obj = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float time = (float)Lua.lua_tonumber(L, 2);
			obj.QuitFocus(time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopMove(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopMove();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LookAt(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			mobileTouchCamera.LookAt(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out MobileTouchCamera.State val);
			CameraStateBase state = mobileTouchCamera.GetState(val);
			objectTranslator.Push(L, state);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num >= 3 && objectTranslator.Assignable<MobileTouchCamera.State>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 4) || objectTranslator.Assignable<object>(L, 4)))
			{
				objectTranslator.Get(L, 2, out MobileTouchCamera.State val);
				bool canReEnterCurrState = Lua.lua_toboolean(L, 3);
				object[] enterParams = objectTranslator.GetParams<object>(L, 4);
				mobileTouchCamera.SetState(val, canReEnterCurrState, enterParams);
				return 0;
			}
			if (num >= 2 && objectTranslator.Assignable<MobileTouchCamera.State>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out MobileTouchCamera.State val2);
				bool canReEnterCurrState2 = Lua.lua_toboolean(L, 3);
				mobileTouchCamera.SetState(val2, canReEnterCurrState2);
				return 0;
			}
			if (num >= 1 && objectTranslator.Assignable<MobileTouchCamera.State>(L, 2))
			{
				objectTranslator.Get(L, 2, out MobileTouchCamera.State val3);
				mobileTouchCamera.SetState(val3, false);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.SetState!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InputDonwOnUI(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = mobileTouchCamera.InputDonwOnUI(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIntersectionPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Ray val);
			Vector3 intersectionPoint = mobileTouchCamera.GetIntersectionPoint(val);
			objectTranslator.PushUnityEngineVector3(L, intersectionPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RaycastGround(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Ray val);
			Vector3 hitPoint;
			bool value = mobileTouchCamera.RaycastGround(val, out hitPoint);
			Lua.lua_pushboolean(L, value);
			objectTranslator.PushUnityEngineVector3(L, hitPoint);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnprojectVector2(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector2>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2 val);
				float offset = (float)Lua.lua_tonumber(L, 3);
				Vector3 val2 = mobileTouchCamera.UnprojectVector2(val, offset);
				objectTranslator.PushUnityEngineVector3(L, val2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector2>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector2 val3);
				Vector3 val4 = mobileTouchCamera.UnprojectVector2(val3);
				objectTranslator.PushUnityEngineVector3(L, val4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.UnprojectVector2!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProjectVector3(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector2 val2 = mobileTouchCamera.ProjectVector3(val);
			objectTranslator.PushUnityEngineVector2(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCameraTargetPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 cameraTargetPos = ((MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1)).GetCameraTargetPos();
			objectTranslator.PushUnityEngineVector3(L, cameraTargetPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCameraPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 cameraPos = ((MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1)).GetCameraPos();
			objectTranslator.PushUnityEngineVector3(L, cameraPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCameraPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			mobileTouchCamera.SetCameraPos(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Quaternion rotation = ((MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1)).GetRotation();
			objectTranslator.PushUnityEngineQuaternion(L, rotation);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			mobileTouchCamera.SetRotation(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFov(IntPtr L)
	{
		try
		{
			MobileTouchCamera obj = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float fov = (float)Lua.lua_tonumber(L, 2);
			obj.SetFov(fov);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFov(IntPtr L)
	{
		try
		{
			float fov = ((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetFov();
			Lua.lua_pushnumber(L, fov);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenPointToRay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Ray val2 = mobileTouchCamera.ScreenPointToRay(val);
			objectTranslator.PushUnityEngineRay(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTouchTerrainPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera obj = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			Vector3 pos;
			bool touchTerrainPos = obj.GetTouchTerrainPos(x, y, out pos);
			Lua.lua_pushboolean(L, touchTerrainPos);
			objectTranslator.PushUnityEngineVector3(L, pos);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldToScreenPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = mobileTouchCamera.WorldToScreenPoint(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetZoomSensitivity(IntPtr L)
	{
		try
		{
			float zoomSensitivity = ((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetZoomSensitivity();
			Lua.lua_pushnumber(L, zoomSensitivity);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AdjustTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = mobileTouchCamera.AdjustTarget(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrentState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, mobileTouchCamera.CurrentState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrentCameraState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileTouchCamera.CurrentCameraState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RefPlane(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileTouchCamera.RefPlane);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LodLevel(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, mobileTouchCamera.LodLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_use45XYCamera(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, mobileTouchCamera.use45XYCamera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_use45XCamera(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, mobileTouchCamera.use45XCamera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomInit(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomInit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomMin(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomMax(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomMaxCity(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomMaxCity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomMinCity(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomMinCity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomMaxWorld(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomMaxWorld);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomMinWorld(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomMinWorld);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamOverZoomMargin(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamOverZoomMargin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoom(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoom);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CanMoveing(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, mobileTouchCamera.CanMoveing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DampFactorTimeMultiplier(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.DampFactorTimeMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AutoScrollVelocityMax(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.AutoScrollVelocityMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AutoScrollDamp(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.AutoScrollDamp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AutoScrollDamps(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileTouchCamera.AutoScrollDamps);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AutoScrollDampCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileTouchCamera.AutoScrollDampCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamFollowFactor(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamFollowFactor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomFarmPlant(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomFarmPlant);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomFarmPlantRotation(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomFarmPlantRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomBuild(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomBuild);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomFocusRotation(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomFocusRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomFormation(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomFormation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomFocusFormationRotation(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomFocusFormationRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomEarthOrder(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomEarthOrder);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomDome(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomDome);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomMoveCity(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomMoveCity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomFocusEarthOrderRotation(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomFocusEarthOrderRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomFocusMoveCityRotation(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomFocusMoveCityRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CamZoomInitRotation(IntPtr L)
	{
		try
		{
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, mobileTouchCamera.CamZoomInitRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CameraFocusCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileTouchCamera.CameraFocusCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CameraFocusEarthCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileTouchCamera.CameraFocusEarthCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CameraFocusDomeCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileTouchCamera.CameraFocusDomeCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CameraFocusMoveCityCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileTouchCamera.CameraFocusMoveCityCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_touchInput(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, mobileTouchCamera.touchInput);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LodLevel(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LodLevel = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_use45XYCamera(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).use45XYCamera = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_use45XCamera(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).use45XCamera = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CamZoomInit(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CamZoomInit = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CamZoomMin(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CamZoomMin = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CamZoomMax(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CamZoomMax = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CamZoom(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CamZoom = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CanMoveing(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CanMoveing = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CamZoomFarmPlant(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CamZoomFarmPlant = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CamZoomFarmPlantRotation(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CamZoomFarmPlantRotation = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CamZoomBuild(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CamZoomBuild = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CamZoomFocusRotation(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CamZoomFocusRotation = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CamZoomFormation(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CamZoomFormation = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CamZoomFocusFormationRotation(IntPtr L)
	{
		try
		{
			((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CamZoomFocusFormationRotation = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BeforeUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1)).BeforeUpdate = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_AfterUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1)).AfterUpdate = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_touchInput(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1)).touchInput = (TouchInputController)objectTranslator.GetObject(L, 2, typeof(TouchInputController));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnForceChangePositionEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			MobileTouchCamera mobileTouchCamera = (MobileTouchCamera)objectTranslator.FastGetCSObj(L, 1);
			MobileTouchCamera.OnForceChangePosition onForceChangePosition = objectTranslator.GetDelegate<MobileTouchCamera.OnForceChangePosition>(L, 3);
			if (onForceChangePosition == null)
			{
				return Lua.luaL_error(L, "#3 need BitBenderGames.MobileTouchCamera.OnForceChangePosition!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					mobileTouchCamera.OnForceChangePositionEvent += onForceChangePosition;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					mobileTouchCamera.OnForceChangePositionEvent -= onForceChangePosition;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.OnForceChangePositionEvent!");
		return 0;
	}
}
