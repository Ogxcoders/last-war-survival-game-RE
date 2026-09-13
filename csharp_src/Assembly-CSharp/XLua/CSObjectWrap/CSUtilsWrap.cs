using System;
using BitBenderGames;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CSUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CSUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 33, 0, 0);
		Utils.RegisterFunc(L, -4, "SetPositionFromInput", _m_SetPositionFromInput_xlua_st_);
		Utils.RegisterFunc(L, -4, "WorldPositionToUISpacePosition", _m_WorldPositionToUISpacePosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "WorldPositionToUITransform", _m_WorldPositionToUITransform_xlua_st_);
		Utils.RegisterFunc(L, -4, "WorldTransformToUIPosition", _m_WorldTransformToUIPosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "UIWorldToUIPos", _m_UIWorldToUIPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "WorldToUIPos", _m_WorldToUIPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsPointerOverUIObject", _m_IsPointerOverUIObject_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetTransformUISpacePosition", _m_SetTransformUISpacePosition_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPositionAndEulerAngleY", _m_GetPositionAndEulerAngleY_xlua_st_);
		Utils.RegisterFunc(L, -4, "DOTweenTo_RectTransformPos_X", _m_DOTweenTo_RectTransformPos_X_xlua_st_);
		Utils.RegisterFunc(L, -4, "DOTweenTo_RectTransformPos_Y", _m_DOTweenTo_RectTransformPos_Y_xlua_st_);
		Utils.RegisterFunc(L, -4, "DOTweenTo_MinWidth", _m_DOTweenTo_MinWidth_xlua_st_);
		Utils.RegisterFunc(L, -4, "DOTweenTo_ScrollRect_Horizontal", _m_DOTweenTo_ScrollRect_Horizontal_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTriggerIds", _m_GetTriggerIds_xlua_st_);
		Utils.RegisterFunc(L, -4, "Hit", _m_Hit_xlua_st_);
		Utils.RegisterFunc(L, -4, "Hit2", _m_Hit2_xlua_st_);
		Utils.RegisterFunc(L, -4, "PoissonDiscSampler", _m_PoissonDiscSampler_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCameraTouchWorldPos", _m_GetCameraTouchWorldPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCameraTouchScreenPos", _m_GetCameraTouchScreenPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "DoCommonVibration", _m_DoCommonVibration_xlua_st_);
		Utils.RegisterFunc(L, -4, "StringToCurve", _m_StringToCurve_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdatePVEStaticMgr", _m_UpdatePVEStaticMgr_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdatePVEStaticMgrWithRealPos", _m_UpdatePVEStaticMgrWithRealPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdatePVEStaticMgrDrawMesh", _m_UpdatePVEStaticMgrDrawMesh_xlua_st_);
		Utils.RegisterFunc(L, -4, "ModifySaveGirlAnimationStartPos", _m_ModifySaveGirlAnimationStartPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetMemRecord", _m_GetMemRecord_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckiOSSystemVersion", _m_CheckiOSSystemVersion_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClickInUIRect", _m_ClickInUIRect_xlua_st_);
		Utils.RegisterFunc(L, -4, "LoadPveShotTex", _m_LoadPveShotTex_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetTimelineExtOpen", _m_SetTimelineExtOpen_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnTimelineInteractionPlotDone", _m_OnTimelineInteractionPlotDone_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnTimelineInteractionQTE1Done", _m_OnTimelineInteractionQTE1Done_xlua_st_);
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
				CSUtils o = new CSUtils();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CSUtils constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPositionFromInput_xlua_st_(IntPtr L)
	{
		try
		{
			CSUtils.SetPositionFromInput((Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldPositionToUISpacePosition_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			Vector3 val2 = CSUtils.WorldPositionToUISpacePosition(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldPositionToUITransform_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Transform>(L, 1) && objectTranslator.Assignable<Camera>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Transform worldTransform = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				Camera mainCamera = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
				RectTransform uiRectTransform = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				float offsetY = (float)Lua.lua_tonumber(L, 4);
				float offsetZ = (float)Lua.lua_tonumber(L, 5);
				CSUtils.WorldPositionToUITransform(worldTransform, mainCamera, uiRectTransform, offsetY, offsetZ);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Transform>(L, 1) && objectTranslator.Assignable<Camera>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Transform worldTransform2 = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				Camera mainCamera2 = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
				RectTransform uiRectTransform2 = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				float offsetY2 = (float)Lua.lua_tonumber(L, 4);
				CSUtils.WorldPositionToUITransform(worldTransform2, mainCamera2, uiRectTransform2, offsetY2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Transform>(L, 1) && objectTranslator.Assignable<Camera>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3))
			{
				Transform worldTransform3 = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				Camera mainCamera3 = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
				RectTransform uiRectTransform3 = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				CSUtils.WorldPositionToUITransform(worldTransform3, mainCamera3, uiRectTransform3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CSUtils.WorldPositionToUITransform!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldTransformToUIPosition_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Transform>(L, 1) && objectTranslator.Assignable<Camera>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Transform worldTransform = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				Camera mainCamera = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
				RectTransform parentRectTransform = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				float offsetY = (float)Lua.lua_tonumber(L, 4);
				float offsetZ = (float)Lua.lua_tonumber(L, 5);
				Vector2 val = CSUtils.WorldTransformToUIPosition(worldTransform, mainCamera, parentRectTransform, offsetY, offsetZ);
				objectTranslator.PushUnityEngineVector2(L, val);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Transform>(L, 1) && objectTranslator.Assignable<Camera>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Transform worldTransform2 = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				Camera mainCamera2 = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
				RectTransform parentRectTransform2 = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				float offsetY2 = (float)Lua.lua_tonumber(L, 4);
				Vector2 val2 = CSUtils.WorldTransformToUIPosition(worldTransform2, mainCamera2, parentRectTransform2, offsetY2);
				objectTranslator.PushUnityEngineVector2(L, val2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Transform>(L, 1) && objectTranslator.Assignable<Camera>(L, 2) && objectTranslator.Assignable<RectTransform>(L, 3))
			{
				Transform worldTransform3 = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				Camera mainCamera3 = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
				RectTransform parentRectTransform3 = (RectTransform)objectTranslator.GetObject(L, 3, typeof(RectTransform));
				Vector2 val3 = CSUtils.WorldTransformToUIPosition(worldTransform3, mainCamera3, parentRectTransform3);
				objectTranslator.PushUnityEngineVector2(L, val3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CSUtils.WorldTransformToUIPosition!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIWorldToUIPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			Vector2 val2 = CSUtils.UIWorldToUIPos(rect: (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform)), worldPos: val);
			objectTranslator.PushUnityEngineVector2(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldToUIPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			Vector2 val2 = CSUtils.WorldToUIPos(rect: (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform)), worldPos: val);
			objectTranslator.PushUnityEngineVector2(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPointerOverUIObject_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = CSUtils.IsPointerOverUIObject();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTransformUISpacePosition_xlua_st_(IntPtr L)
	{
		try
		{
			Transform targetTransform = (Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform));
			float posX = (float)Lua.lua_tonumber(L, 2);
			float posY = (float)Lua.lua_tonumber(L, 3);
			float posZ = (float)Lua.lua_tonumber(L, 4);
			CSUtils.SetTransformUISpacePosition(targetTransform, posX, posY, posZ);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPositionAndEulerAngleY_xlua_st_(IntPtr L)
	{
		try
		{
			CSUtils.GetPositionAndEulerAngleY((Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform)), out var x, out var y, out var z, out var angle);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			Lua.lua_pushnumber(L, angle);
			return 4;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOTweenTo_RectTransformPos_X_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<RectTransform>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Action>(L, 4))
			{
				RectTransform rectTransform = (RectTransform)objectTranslator.GetObject(L, 1, typeof(RectTransform));
				float endX = (float)Lua.lua_tonumber(L, 2);
				float time = (float)Lua.lua_tonumber(L, 3);
				Action callback = objectTranslator.GetDelegate<Action>(L, 4);
				CSUtils.DOTweenTo_RectTransformPos_X(rectTransform, endX, time, callback);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<RectTransform>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				RectTransform rectTransform2 = (RectTransform)objectTranslator.GetObject(L, 1, typeof(RectTransform));
				float endX2 = (float)Lua.lua_tonumber(L, 2);
				float time2 = (float)Lua.lua_tonumber(L, 3);
				CSUtils.DOTweenTo_RectTransformPos_X(rectTransform2, endX2, time2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CSUtils.DOTweenTo_RectTransformPos_X!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOTweenTo_RectTransformPos_Y_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<RectTransform>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Action>(L, 4))
			{
				RectTransform rectTransform = (RectTransform)objectTranslator.GetObject(L, 1, typeof(RectTransform));
				float endY = (float)Lua.lua_tonumber(L, 2);
				float time = (float)Lua.lua_tonumber(L, 3);
				Action callback = objectTranslator.GetDelegate<Action>(L, 4);
				CSUtils.DOTweenTo_RectTransformPos_Y(rectTransform, endY, time, callback);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<RectTransform>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				RectTransform rectTransform2 = (RectTransform)objectTranslator.GetObject(L, 1, typeof(RectTransform));
				float endY2 = (float)Lua.lua_tonumber(L, 2);
				float time2 = (float)Lua.lua_tonumber(L, 3);
				CSUtils.DOTweenTo_RectTransformPos_Y(rectTransform2, endY2, time2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CSUtils.DOTweenTo_RectTransformPos_Y!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOTweenTo_MinWidth_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<LayoutElement>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Action>(L, 4))
			{
				LayoutElement rectTransform = (LayoutElement)objectTranslator.GetObject(L, 1, typeof(LayoutElement));
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float time = (float)Lua.lua_tonumber(L, 3);
				Action callback = objectTranslator.GetDelegate<Action>(L, 4);
				CSUtils.DOTweenTo_MinWidth(rectTransform, endValue, time, callback);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<LayoutElement>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				LayoutElement rectTransform2 = (LayoutElement)objectTranslator.GetObject(L, 1, typeof(LayoutElement));
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float time2 = (float)Lua.lua_tonumber(L, 3);
				CSUtils.DOTweenTo_MinWidth(rectTransform2, endValue2, time2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CSUtils.DOTweenTo_MinWidth!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOTweenTo_ScrollRect_Horizontal_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<ScrollRect>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Action>(L, 4))
			{
				ScrollRect scrollRect = (ScrollRect)objectTranslator.GetObject(L, 1, typeof(ScrollRect));
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float time = (float)Lua.lua_tonumber(L, 3);
				Action callback = objectTranslator.GetDelegate<Action>(L, 4);
				CSUtils.DOTweenTo_ScrollRect_Horizontal(scrollRect, endValue, time, callback);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<ScrollRect>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				ScrollRect scrollRect2 = (ScrollRect)objectTranslator.GetObject(L, 1, typeof(ScrollRect));
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float time2 = (float)Lua.lua_tonumber(L, 3);
				CSUtils.DOTweenTo_ScrollRect_Horizontal(scrollRect2, endValue2, time2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CSUtils.DOTweenTo_ScrollRect_Horizontal!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTriggerIds_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			float tx = (float)Lua.lua_tonumber(L, 1);
			float ty = (float)Lua.lua_tonumber(L, 2);
			float tz = (float)Lua.lua_tonumber(L, 3);
			float dx = (float)Lua.lua_tonumber(L, 4);
			float dy = (float)Lua.lua_tonumber(L, 5);
			float dz = (float)Lua.lua_tonumber(L, 6);
			float radius = (float)Lua.lua_tonumber(L, 7);
			GameObject srcObj = (GameObject)objectTranslator.GetObject(L, 8, typeof(GameObject));
			LuaTable outTable = (LuaTable)objectTranslator.GetObject(L, 9, typeof(LuaTable));
			string layerName = Lua.lua_tostring(L, 10);
			int triggerIds = CSUtils.GetTriggerIds(tx, ty, tz, dx, dy, dz, radius, srcObj, outTable, layerName);
			Lua.xlua_pushinteger(L, triggerIds);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Hit_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
			float radius = (float)Lua.lua_tonumber(L, 2);
			float speed = (float)Lua.lua_tonumber(L, 3);
			Vector3 oritation;
			bool value = CSUtils.Hit(transform, radius, speed, out oritation);
			Lua.lua_pushboolean(L, value);
			objectTranslator.PushUnityEngineVector3(L, oritation);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Hit2_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			Vector3 oritation;
			bool value = CSUtils.Hit2(radius: (float)Lua.lua_tonumber(L, 3), speed: (float)Lua.lua_tonumber(L, 4), pos: val, forward: val2, oritation: out oritation);
			Lua.lua_pushboolean(L, value);
			objectTranslator.PushUnityEngineVector3(L, oritation);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PoissonDiscSampler_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			float radius = (float)Lua.lua_tonumber(L, 1);
			float width = (float)Lua.lua_tonumber(L, 2);
			float height = (float)Lua.lua_tonumber(L, 3);
			Vector2[] o = CSUtils.PoissonDiscSampler(radius, width, height);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCameraTouchWorldPos_xlua_st_(IntPtr L)
	{
		try
		{
			float hitX;
			float hitZ;
			bool cameraTouchWorldPos = CSUtils.GetCameraTouchWorldPos((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(MobileTouchCamera)), out hitX, out hitZ);
			Lua.lua_pushboolean(L, cameraTouchWorldPos);
			Lua.lua_pushnumber(L, hitX);
			Lua.lua_pushnumber(L, hitZ);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCameraTouchScreenPos_xlua_st_(IntPtr L)
	{
		try
		{
			float screenX;
			float screenY;
			bool cameraTouchScreenPos = CSUtils.GetCameraTouchScreenPos((MobileTouchCamera)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(MobileTouchCamera)), out screenX, out screenY);
			Lua.lua_pushboolean(L, cameraTouchScreenPos);
			Lua.lua_pushnumber(L, screenX);
			Lua.lua_pushnumber(L, screenY);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoCommonVibration_xlua_st_(IntPtr L)
	{
		try
		{
			float intensity = (float)Lua.lua_tonumber(L, 1);
			float sharpness = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			CSUtils.DoCommonVibration(intensity, sharpness, duration);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StringToCurve_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AnimationCurve o = CSUtils.StringToCurve(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdatePVEStaticMgr_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera touchCamera = (MobileTouchCamera)objectTranslator.GetObject(L, 1, typeof(MobileTouchCamera));
			PVEDecorationManagerBase pveStaticManager = (PVEDecorationManagerBase)objectTranslator.GetObject(L, 2, typeof(PVEDecorationManagerBase));
			CSUtils.UpdatePVEStaticMgr(touchCamera, pveStaticManager);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdatePVEStaticMgrWithRealPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MobileTouchCamera touchCamera = (MobileTouchCamera)objectTranslator.GetObject(L, 1, typeof(MobileTouchCamera));
			PVEDecorationManagerBase pveStaticManager = (PVEDecorationManagerBase)objectTranslator.GetObject(L, 2, typeof(PVEDecorationManagerBase));
			float offsetZ = (float)Lua.lua_tonumber(L, 3);
			float renderOffsetZ = (float)Lua.lua_tonumber(L, 4);
			CSUtils.UpdatePVEStaticMgrWithRealPos(touchCamera, pveStaticManager, offsetZ, renderOffsetZ);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdatePVEStaticMgrDrawMesh_xlua_st_(IntPtr L)
	{
		try
		{
			CSUtils.UpdatePVEStaticMgrDrawMesh((PVEDecorationManagerBase)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(PVEDecorationManagerBase)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ModifySaveGirlAnimationStartPos_xlua_st_(IntPtr L)
	{
		try
		{
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(SkeletonGraphic));
			string animationName = Lua.lua_tostring(L, 2);
			string boneName = Lua.lua_tostring(L, 3);
			float startX = (float)Lua.lua_tonumber(L, 4);
			float startY = (float)Lua.lua_tonumber(L, 5);
			CSUtils.ModifySaveGirlAnimationStartPos(skeletonGraphic, animationName, boneName, startX, startY);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMemRecord_xlua_st_(IntPtr L)
	{
		try
		{
			CSUtils.GetMemRecord(out var nrsv, out var nuse, out var mrsv, out var muse);
			Lua.lua_pushnumber(L, nrsv);
			Lua.lua_pushnumber(L, nuse);
			Lua.lua_pushnumber(L, mrsv);
			Lua.lua_pushnumber(L, muse);
			return 4;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckiOSSystemVersion_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = CSUtils.CheckiOSSystemVersion(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClickInUIRect_xlua_st_(IntPtr L)
	{
		try
		{
			RectTransform rectTransform = (RectTransform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(RectTransform));
			float screenPointX = (float)Lua.lua_tonumber(L, 2);
			float screenPointY = (float)Lua.lua_tonumber(L, 3);
			bool value = CSUtils.ClickInUIRect(rectTransform, screenPointX, screenPointY);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadPveShotTex_xlua_st_(IntPtr L)
	{
		try
		{
			CSUtils.LoadPveShotTex(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTimelineExtOpen_xlua_st_(IntPtr L)
	{
		try
		{
			CSUtils.SetTimelineExtOpen(Lua.lua_toboolean(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnTimelineInteractionPlotDone_xlua_st_(IntPtr L)
	{
		try
		{
			CSUtils.OnTimelineInteractionPlotDone(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnTimelineInteractionQTE1Done_xlua_st_(IntPtr L)
	{
		try
		{
			CSUtils.OnTimelineInteractionQTE1Done();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
