using System;
using BitBenderGames;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldCameraWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldCamera);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 47, 19, 8);
		Utils.RegisterFunc(L, -3, "InCamera", _m_InCamera);
		Utils.RegisterFunc(L, -3, "SetRotRangeOverride", _m_SetRotRangeOverride);
		Utils.RegisterFunc(L, -3, "SetLodRange", _m_SetLodRange);
		Utils.RegisterFunc(L, -3, "IsInMoveToState", _m_IsInMoveToState);
		Utils.RegisterFunc(L, -3, "IsInFreeLookStateWithAtLeastSpeed", _m_IsInFreeLookStateWithAtLeastSpeed);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "InitCameraInfoOnSceneLoaded", _m_InitCameraInfoOnSceneLoaded);
		Utils.RegisterFunc(L, -3, "GetLodLevel", _m_GetLodLevel);
		Utils.RegisterFunc(L, -3, "GetLodDistance", _m_GetLodDistance);
		Utils.RegisterFunc(L, -3, "GetPreviousLodDistance", _m_GetPreviousLodDistance);
		Utils.RegisterFunc(L, -3, "GetMinLodDistance", _m_GetMinLodDistance);
		Utils.RegisterFunc(L, -3, "MarkLodChanged", _m_MarkLodChanged);
		Utils.RegisterFunc(L, -3, "GetCurrentCameraState", _m_GetCurrentCameraState);
		Utils.RegisterFunc(L, -3, "AutoLookat", _m_AutoLookat);
		Utils.RegisterFunc(L, -3, "AutoZoom", _m_AutoZoom);
		Utils.RegisterFunc(L, -3, "AutoFocus", _m_AutoFocus);
		Utils.RegisterFunc(L, -3, "QuitFocus", _m_QuitFocus);
		Utils.RegisterFunc(L, -3, "StopMove", _m_StopMove);
		Utils.RegisterFunc(L, -3, "Lookat", _m_Lookat);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "RefreshCameraAnchor", _m_RefreshCameraAnchor);
		Utils.RegisterFunc(L, -3, "GetRaycastGroundPoint", _m_GetRaycastGroundPoint);
		Utils.RegisterFunc(L, -3, "WorldToViewportPoint", _m_WorldToViewportPoint);
		Utils.RegisterFunc(L, -3, "WorldToScreenPoint", _m_WorldToScreenPoint);
		Utils.RegisterFunc(L, -3, "ScreenPointToWorld", _m_ScreenPointToWorld);
		Utils.RegisterFunc(L, -3, "ScreenPointToRay", _m_ScreenPointToRay);
		Utils.RegisterFunc(L, -3, "TrackMarch", _m_TrackMarch);
		Utils.RegisterFunc(L, -3, "TrackHSR", _m_TrackHSR);
		Utils.RegisterFunc(L, -3, "BeginSyncWithTimeline", _m_BeginSyncWithTimeline);
		Utils.RegisterFunc(L, -3, "EndSyncWithTimeline", _m_EndSyncWithTimeline);
		Utils.RegisterFunc(L, -3, "LockCamera", _m_LockCamera);
		Utils.RegisterFunc(L, -3, "FreeCamera", _m_FreeCamera);
		Utils.RegisterFunc(L, -3, "ClampToEdge", _m_ClampToEdge);
		Utils.RegisterFunc(L, -3, "GetRotation", _m_GetRotation);
		Utils.RegisterFunc(L, -3, "GetPosition", _m_GetPosition);
		Utils.RegisterFunc(L, -3, "GetMapIconScale", _m_GetMapIconScale);
		Utils.RegisterFunc(L, -3, "GetMapLabelScale", _m_GetMapLabelScale);
		Utils.RegisterFunc(L, -3, "EnablePostProcess", _m_EnablePostProcess);
		Utils.RegisterFunc(L, -3, "DisablePostProcess", _m_DisablePostProcess);
		Utils.RegisterFunc(L, -3, "OnDrawGizmos", _m_OnDrawGizmos);
		Utils.RegisterFunc(L, -3, "SetTouchInputControllerEnable", _m_SetTouchInputControllerEnable);
		Utils.RegisterFunc(L, -3, "GetTouchInputControllerEnable", _m_GetTouchInputControllerEnable);
		Utils.RegisterFunc(L, -3, "SetResolution", _m_SetResolution);
		Utils.RegisterFunc(L, -3, "SetZoomParams", _m_SetZoomParams);
		Utils.RegisterFunc(L, -3, "SetFOV", _m_SetFOV);
		Utils.RegisterFunc(L, -3, "AfterUpdate", _e_AfterUpdate);
		Utils.RegisterFunc(L, -2, "__camera", _g_get___camera);
		Utils.RegisterFunc(L, -2, "TrackMarchId", _g_get_TrackMarchId);
		Utils.RegisterFunc(L, -2, "ViewSize", _g_get_ViewSize);
		Utils.RegisterFunc(L, -2, "InitZoom", _g_get_InitZoom);
		Utils.RegisterFunc(L, -2, "ZoomMin", _g_get_ZoomMin);
		Utils.RegisterFunc(L, -2, "ZoomMax", _g_get_ZoomMax);
		Utils.RegisterFunc(L, -2, "CurTarget", _g_get_CurTarget);
		Utils.RegisterFunc(L, -2, "CurTilePos", _g_get_CurTilePos);
		Utils.RegisterFunc(L, -2, "CurTilePosClamped", _g_get_CurTilePosClamped);
		Utils.RegisterFunc(L, -2, "Zoom", _g_get_Zoom);
		Utils.RegisterFunc(L, -2, "AutoMove", _g_get_AutoMove);
		Utils.RegisterFunc(L, -2, "IsFocus", _g_get_IsFocus);
		Utils.RegisterFunc(L, -2, "CanMoving", _g_get_CanMoving);
		Utils.RegisterFunc(L, -2, "Enabled", _g_get_Enabled);
		Utils.RegisterFunc(L, -2, "TouchInputController", _g_get_TouchInputController);
		Utils.RegisterFunc(L, -2, "CurrentLodLevel", _g_get_CurrentLodLevel);
		Utils.RegisterFunc(L, -2, "frameBufferWidth", _g_get_frameBufferWidth);
		Utils.RegisterFunc(L, -2, "frameBufferHeight", _g_get_frameBufferHeight);
		Utils.RegisterFunc(L, -2, "cameraAnchor", _g_get_cameraAnchor);
		Utils.RegisterFunc(L, -1, "ZoomMin", _s_set_ZoomMin);
		Utils.RegisterFunc(L, -1, "ZoomMax", _s_set_ZoomMax);
		Utils.RegisterFunc(L, -1, "Zoom", _s_set_Zoom);
		Utils.RegisterFunc(L, -1, "AutoMove", _s_set_AutoMove);
		Utils.RegisterFunc(L, -1, "IsFocus", _s_set_IsFocus);
		Utils.RegisterFunc(L, -1, "CanMoving", _s_set_CanMoving);
		Utils.RegisterFunc(L, -1, "Enabled", _s_set_Enabled);
		Utils.RegisterFunc(L, -1, "cameraAnchor", _s_set_cameraAnchor);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 11, 2, 1);
		Utils.RegisterObject(L, translator, -4, "MoveTime", 0.3f);
		Utils.RegisterObject(L, translator, -4, "MainBuildingUpgradeZoom", 54.96f);
		Utils.RegisterObject(L, translator, -4, "ShowBaseFireCameraZoom", 62.07f);
		Utils.RegisterObject(L, translator, -4, "PointMoveDetal", 0.1f);
		Utils.RegisterObject(L, translator, -4, "MAX_POS_Y", WorldCamera.MAX_POS_Y);
		Utils.RegisterObject(L, translator, -4, "MAX_PADDING_Y", WorldCamera.MAX_PADDING_Y);
		Utils.RegisterObject(L, translator, -4, "LeftBottom", 0);
		Utils.RegisterObject(L, translator, -4, "LeftTop", 1);
		Utils.RegisterObject(L, translator, -4, "RightTop", 2);
		Utils.RegisterObject(L, translator, -4, "RightBottom", 3);
		Utils.RegisterFunc(L, -2, "LodArray", _g_get_LodArray);
		Utils.RegisterFunc(L, -2, "NeedLoadFocusCurve", _g_get_NeedLoadFocusCurve);
		Utils.RegisterFunc(L, -1, "NeedLoadFocusCurve", _s_set_NeedLoadFocusCurve);
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
				WorldCamera o = new WorldCamera((WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldCamera constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InCamera(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float z = (float)Lua.lua_tonumber(L, 3);
				bool value = worldCamera.InCamera(x, z);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x2 = (float)Lua.lua_tonumber(L, 2);
				float z2 = (float)Lua.lua_tonumber(L, 3);
				float distance = (float)Lua.lua_tonumber(L, 4);
				bool value2 = worldCamera.InCamera(x2, z2, distance);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldCamera.InCamera!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRotRangeOverride(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			bool overrideRange = Lua.lua_toboolean(L, 2);
			objectTranslator.Get(L, 3, out Vector2 val);
			worldCamera.SetRotRangeOverride(overrideRange, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLodRange(IntPtr L)
	{
		try
		{
			WorldCamera obj = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool overrideRange = Lua.lua_toboolean(L, 2);
			int minLod = Lua.xlua_tointeger(L, 3);
			int maxLod = Lua.xlua_tointeger(L, 4);
			obj.SetLodRange(overrideRange, minLod, maxLod);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInMoveToState(IntPtr L)
	{
		try
		{
			bool value = ((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsInMoveToState();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInFreeLookStateWithAtLeastSpeed(IntPtr L)
	{
		try
		{
			WorldCamera obj = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float speed = (float)Lua.lua_tonumber(L, 2);
			bool value = obj.IsInFreeLookStateWithAtLeastSpeed(speed);
			Lua.lua_pushboolean(L, value);
			return 1;
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
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
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
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitCameraInfoOnSceneLoaded(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitCameraInfoOnSceneLoaded();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLodLevel(IntPtr L)
	{
		try
		{
			int lodLevel = ((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLodLevel();
			Lua.xlua_pushinteger(L, lodLevel);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLodDistance(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				float lodDistance2 = worldCamera.GetLodDistance();
				Lua.lua_pushnumber(L, lodDistance2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
				{
					int lod = Lua.xlua_tointeger(L, 2);
					float lodDistance = worldCamera.GetLodDistance(lod);
					Lua.lua_pushnumber(L, lodDistance);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldCamera.GetLodDistance!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPreviousLodDistance(IntPtr L)
	{
		try
		{
			float previousLodDistance = ((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPreviousLodDistance();
			Lua.lua_pushnumber(L, previousLodDistance);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMinLodDistance(IntPtr L)
	{
		try
		{
			float minLodDistance = ((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMinLodDistance();
			Lua.lua_pushnumber(L, minLodDistance);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MarkLodChanged(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).MarkLodChanged();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentCameraState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera.State currentCameraState = ((WorldCamera)objectTranslator.FastGetCSObj(L, 1)).GetCurrentCameraState();
			objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, currentCameraState);
			return 1;
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
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Action>(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float zoom = (float)Lua.lua_tonumber(L, 3);
				float time = (float)Lua.lua_tonumber(L, 4);
				Action onComplete = objectTranslator.GetDelegate<Action>(L, 5);
				worldCamera.AutoLookat(val, zoom, time, onComplete);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float zoom2 = (float)Lua.lua_tonumber(L, 3);
				float time2 = (float)Lua.lua_tonumber(L, 4);
				worldCamera.AutoLookat(val2, zoom2, time2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float zoom3 = (float)Lua.lua_tonumber(L, 3);
				worldCamera.AutoLookat(val3, zoom3);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val4);
				worldCamera.AutoLookat(val4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldCamera.AutoLookat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AutoZoom(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Action>(L, 4))
			{
				float zoom = (float)Lua.lua_tonumber(L, 2);
				float time = (float)Lua.lua_tonumber(L, 3);
				Action onComplete = objectTranslator.GetDelegate<Action>(L, 4);
				worldCamera.AutoZoom(zoom, time, onComplete);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float zoom2 = (float)Lua.lua_tonumber(L, 2);
				float time2 = (float)Lua.lua_tonumber(L, 3);
				worldCamera.AutoZoom(zoom2, time2);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float zoom3 = (float)Lua.lua_tonumber(L, 2);
				worldCamera.AutoZoom(zoom3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldCamera.AutoZoom!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AutoFocus(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<LookAtFocusState>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && objectTranslator.Assignable<Action>(L, 7))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				objectTranslator.Get(L, 3, out LookAtFocusState v);
				float time = (float)Lua.lua_tonumber(L, 4);
				bool focusToCenter = Lua.lua_toboolean(L, 5);
				bool lockView = Lua.lua_toboolean(L, 6);
				Action onComplete = objectTranslator.GetDelegate<Action>(L, 7);
				worldCamera.AutoFocus(val, v, time, focusToCenter, lockView, onComplete);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<LookAtFocusState>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.Get(L, 3, out LookAtFocusState v2);
				float time2 = (float)Lua.lua_tonumber(L, 4);
				bool focusToCenter2 = Lua.lua_toboolean(L, 5);
				bool lockView2 = Lua.lua_toboolean(L, 6);
				worldCamera.AutoFocus(val2, v2, time2, focusToCenter2, lockView2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldCamera.AutoFocus!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_QuitFocus(IntPtr L)
	{
		try
		{
			WorldCamera obj = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopMove();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Lookat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			worldCamera.Lookat(val);
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
			WorldCamera obj = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_RefreshCameraAnchor(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshCameraAnchor();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRaycastGroundPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 raycastGroundPoint = worldCamera.GetRaycastGroundPoint(val);
			objectTranslator.PushUnityEngineVector3(L, raycastGroundPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldToViewportPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = worldCamera.WorldToViewportPoint(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
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
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = worldCamera.WorldToScreenPoint(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenPointToWorld(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float disPlane = (float)Lua.lua_tonumber(L, 3);
				Vector3 val2 = worldCamera.ScreenPointToWorld(val, disPlane);
				objectTranslator.PushUnityEngineVector3(L, val2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				Vector3 val4 = worldCamera.ScreenPointToWorld(val3);
				objectTranslator.PushUnityEngineVector3(L, val4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldCamera.ScreenPointToWorld!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenPointToRay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Ray val2 = worldCamera.ScreenPointToRay(val);
			objectTranslator.PushUnityEngineRay(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TrackMarch(IntPtr L)
	{
		try
		{
			WorldCamera obj = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchId = Lua.lua_toint64(L, 2);
			obj.TrackMarch(marchId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TrackHSR(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			long marchId = Lua.lua_toint64(L, 2);
			GameObject go = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			worldCamera.TrackHSR(marchId, go);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BeginSyncWithTimeline(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			Camera camInTimeline = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
			float transitionTime = (float)Lua.lua_tonumber(L, 3);
			worldCamera.BeginSyncWithTimeline(camInTimeline, transitionTime);
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
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EndSyncWithTimeline();
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
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			float duration = (float)Lua.lua_tonumber(L, 3);
			worldCamera.LockCamera(val, duration);
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
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FreeCamera();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClampToEdge(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClampToEdge();
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
			Quaternion rotation = ((WorldCamera)objectTranslator.FastGetCSObj(L, 1)).GetRotation();
			objectTranslator.PushUnityEngineQuaternion(L, rotation);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 position = ((WorldCamera)objectTranslator.FastGetCSObj(L, 1)).GetPosition();
			objectTranslator.PushUnityEngineVector3(L, position);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMapIconScale(IntPtr L)
	{
		try
		{
			float mapIconScale = ((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMapIconScale();
			Lua.lua_pushnumber(L, mapIconScale);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMapLabelScale(IntPtr L)
	{
		try
		{
			float mapLabelScale = ((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMapLabelScale();
			Lua.lua_pushnumber(L, mapLabelScale);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnablePostProcess(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnablePostProcess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisablePostProcess(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DisablePostProcess();
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
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDrawGizmos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTouchInputControllerEnable(IntPtr L)
	{
		try
		{
			WorldCamera obj = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool touchInputControllerEnable = Lua.lua_toboolean(L, 2);
			obj.SetTouchInputControllerEnable(touchInputControllerEnable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTouchInputControllerEnable(IntPtr L)
	{
		try
		{
			bool touchInputControllerEnable = ((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetTouchInputControllerEnable();
			Lua.lua_pushboolean(L, touchInputControllerEnable);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetResolution(IntPtr L)
	{
		try
		{
			WorldCamera obj = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int resolution = Lua.xlua_tointeger(L, 2);
			obj.SetResolution(resolution);
			return 0;
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
			WorldCamera obj = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int level = Lua.xlua_tointeger(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float offsetZ = (float)Lua.lua_tonumber(L, 4);
			float sensitivity = (float)Lua.lua_tonumber(L, 5);
			obj.SetZoomParams(level, y, offsetZ, sensitivity);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFOV(IntPtr L)
	{
		try
		{
			WorldCamera obj = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float fOV = (float)Lua.lua_tonumber(L, 2);
			obj.SetFOV(fOV);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get___camera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldCamera.__camera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TrackMarchId(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldCamera.TrackMarchId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LodArray(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, WorldCamera.LodArray);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ViewSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, worldCamera.ViewSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_InitZoom(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldCamera.InitZoom);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ZoomMin(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldCamera.ZoomMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ZoomMax(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldCamera.ZoomMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, worldCamera.CurTarget);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTilePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldCamera.CurTilePos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTilePosClamped(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldCamera.CurTilePosClamped);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Zoom(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldCamera.Zoom);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AutoMove(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldCamera.AutoMove);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsFocus(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldCamera.IsFocus);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CanMoving(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldCamera.CanMoving);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Enabled(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldCamera.Enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TouchInputController(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldCamera.TouchInputController);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrentLodLevel(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldCamera.CurrentLodLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frameBufferWidth(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldCamera.frameBufferWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frameBufferHeight(IntPtr L)
	{
		try
		{
			WorldCamera worldCamera = (WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldCamera.frameBufferHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_NeedLoadFocusCurve(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, WorldCamera.NeedLoadFocusCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cameraAnchor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldCamera.cameraAnchor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ZoomMin(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ZoomMin = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ZoomMax(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ZoomMax = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Zoom(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Zoom = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_AutoMove(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).AutoMove = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsFocus(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsFocus = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CanMoving(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CanMoving = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Enabled(IntPtr L)
	{
		try
		{
			((WorldCamera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Enabled = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_NeedLoadFocusCurve(IntPtr L)
	{
		try
		{
			WorldCamera.NeedLoadFocusCurve = (LookAtFocusState[])ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LookAtFocusState[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cameraAnchor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldCamera)objectTranslator.FastGetCSObj(L, 1)).cameraAnchor = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_AfterUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			WorldCamera worldCamera = (WorldCamera)objectTranslator.FastGetCSObj(L, 1);
			Action action = objectTranslator.GetDelegate<Action>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					worldCamera.AfterUpdate += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					worldCamera.AfterUpdate -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to WorldCamera.AfterUpdate!");
		return 0;
	}
}
