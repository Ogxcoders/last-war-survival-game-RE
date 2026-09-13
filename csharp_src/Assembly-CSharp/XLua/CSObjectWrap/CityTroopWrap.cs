using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CityTroopWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CityTroop);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 50, 6, 3);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "GetPointInfo", _m_GetPointInfo);
		Utils.RegisterFunc(L, -3, "OnDrag", _m_OnDrag);
		Utils.RegisterFunc(L, -3, "OnBeginDrag", _m_OnBeginDrag);
		Utils.RegisterFunc(L, -3, "OnCreateDragLine", _m_OnCreateDragLine);
		Utils.RegisterFunc(L, -3, "OnDragLineUpdate", _m_OnDragLineUpdate);
		Utils.RegisterFunc(L, -3, "DealGarbageQueueDataWhenDragEnd", _m_DealGarbageQueueDataWhenDragEnd);
		Utils.RegisterFunc(L, -3, "OnDragLineStop", _m_OnDragLineStop);
		Utils.RegisterFunc(L, -3, "OnCreateMoveLine", _m_OnCreateMoveLine);
		Utils.RegisterFunc(L, -3, "DoWhenUseTmpEndPos", _m_DoWhenUseTmpEndPos);
		Utils.RegisterFunc(L, -3, "DoWhenSetTmpEndPos", _m_DoWhenSetTmpEndPos);
		Utils.RegisterFunc(L, -3, "OnMoveLineUpdate", _m_OnMoveLineUpdate);
		Utils.RegisterFunc(L, -3, "OnMoveLineStop", _m_OnMoveLineStop);
		Utils.RegisterFunc(L, -3, "EnterWorkIdle", _m_EnterWorkIdle);
		Utils.RegisterFunc(L, -3, "PlayAnim", _m_PlayAnim);
		Utils.RegisterFunc(L, -3, "EnterWorkMove", _m_EnterWorkMove);
		Utils.RegisterFunc(L, -3, "MoveStartRotation", _m_MoveStartRotation);
		Utils.RegisterFunc(L, -3, "MoveEndRotation", _m_MoveEndRotation);
		Utils.RegisterFunc(L, -3, "Move", _m_Move);
		Utils.RegisterFunc(L, -3, "EnterWorkGarbage", _m_EnterWorkGarbage);
		Utils.RegisterFunc(L, -3, "EnterWorkAttack", _m_EnterWorkAttack);
		Utils.RegisterFunc(L, -3, "GetRealEndPos", _m_GetRealEndPos);
		Utils.RegisterFunc(L, -3, "EnterWorkOpenFog", _m_EnterWorkOpenFog);
		Utils.RegisterFunc(L, -3, "PlayGarbageSuccess", _m_PlayGarbageSuccess);
		Utils.RegisterFunc(L, -3, "PlayGarbageFail", _m_PlayGarbageFail);
		Utils.RegisterFunc(L, -3, "ClearTroopUnits", _m_ClearTroopUnits);
		Utils.RegisterFunc(L, -3, "MoveAfterCreate", _m_MoveAfterCreate);
		Utils.RegisterFunc(L, -3, "IsTruck", _m_IsTruck);
		Utils.RegisterFunc(L, -3, "CanLongTap", _m_CanLongTap);
		Utils.RegisterFunc(L, -3, "GetTransform", _m_GetTransform);
		Utils.RegisterFunc(L, -3, "PointInPick", _m_PointInPick);
		Utils.RegisterFunc(L, -3, "Drag", _m_Drag);
		Utils.RegisterFunc(L, -3, "Select", _m_Select);
		Utils.RegisterFunc(L, -3, "Click", _m_Click);
		Utils.RegisterFunc(L, -3, "ShowHeadUI", _m_ShowHeadUI);
		Utils.RegisterFunc(L, -3, "IsOutRange", _m_IsOutRange);
		Utils.RegisterFunc(L, -3, "ChangeTouchPos", _m_ChangeTouchPos);
		Utils.RegisterFunc(L, -3, "GetClosestPoint", _m_GetClosestPoint);
		Utils.RegisterFunc(L, -3, "OnEndDrag", _m_OnEndDrag);
		Utils.RegisterFunc(L, -3, "OnClick", _m_OnClick);
		Utils.RegisterFunc(L, -3, "OnEndLongTap", _m_OnEndLongTap);
		Utils.RegisterFunc(L, -3, "OnBeginLongTap", _m_OnBeginLongTap);
		Utils.RegisterFunc(L, -3, "IsTruckPickGarbageTroop", _m_IsTruckPickGarbageTroop);
		Utils.RegisterFunc(L, -3, "IsWorkmanPickGarbageTroop", _m_IsWorkmanPickGarbageTroop);
		Utils.RegisterFunc(L, -3, "GetTrunkPosition", _m_GetTrunkPosition);
		Utils.RegisterFunc(L, -3, "GetPickGarbageMoveDistance", _m_GetPickGarbageMoveDistance);
		Utils.RegisterFunc(L, -3, "GetOpenFogMoveDistance", _m_GetOpenFogMoveDistance);
		Utils.RegisterFunc(L, -3, "TroopUnitsBirthThenPickGarbage", _m_TroopUnitsBirthThenPickGarbage);
		Utils.RegisterFunc(L, -3, "TroopUnitPickSuccess", _m_TroopUnitPickSuccess);
		Utils.RegisterFunc(L, -2, "PreviewType", _g_get_PreviewType);
		Utils.RegisterFunc(L, -2, "EndPos", _g_get_EndPos);
		Utils.RegisterFunc(L, -2, "TmpEndPos", _g_get_TmpEndPos);
		Utils.RegisterFunc(L, -2, "OpenFogPointIndex", _g_get_OpenFogPointIndex);
		Utils.RegisterFunc(L, -2, "Priority", _g_get_Priority);
		Utils.RegisterFunc(L, -2, "TilePos", _g_get_TilePos);
		Utils.RegisterFunc(L, -1, "EndPos", _s_set_EndPos);
		Utils.RegisterFunc(L, -1, "TmpEndPos", _s_set_TmpEndPos);
		Utils.RegisterFunc(L, -1, "OpenFogPointIndex", _s_set_OpenFogPointIndex);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterObject(L, translator, -4, "City_Troop_Anim_Idle", "idle");
		Utils.RegisterObject(L, translator, -4, "City_Troop_Anim_Run", "run");
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
				CityTroop o = new CityTroop();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityTroop constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
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
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo pointInfo = ((CityTroop)objectTranslator.FastGetCSObj(L, 1)).GetPointInfo();
			objectTranslator.Push(L, pointInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector3 val2);
			bool value = cityTroop.OnDrag(val, val2);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeginDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = cityTroop.OnBeginDrag(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnCreateDragLine(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnCreateDragLine();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDragLineUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			cityTroop.OnDragLineUpdate(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DealGarbageQueueDataWhenDragEnd(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = cityTroop.DealGarbageQueueDataWhenDragEnd(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDragLineStop(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDragLineStop();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnCreateMoveLine(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnCreateMoveLine();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoWhenUseTmpEndPos(IntPtr L)
	{
		try
		{
			bool value = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoWhenUseTmpEndPos();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoWhenSetTmpEndPos(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DoWhenSetTmpEndPos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnMoveLineUpdate(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnMoveLineUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnMoveLineStop(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnMoveLineStop();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnterWorkIdle(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnterWorkIdle();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAnim(IntPtr L)
	{
		try
		{
			CityTroop obj = (CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string animName = Lua.lua_tostring(L, 2);
			obj.PlayAnim(animName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnterWorkMove(IntPtr L)
	{
		try
		{
			CityTroop obj = (CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float moveSpeed = (float)Lua.lua_tonumber(L, 2);
			float num = obj.EnterWorkMove(moveSpeed);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveStartRotation(IntPtr L)
	{
		try
		{
			CityTroop obj = (CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float time = (float)Lua.lua_tonumber(L, 2);
			obj.MoveStartRotation(time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveEndRotation(IntPtr L)
	{
		try
		{
			CityTroop obj = (CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float time = (float)Lua.lua_tonumber(L, 2);
			obj.MoveEndRotation(time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Move(IntPtr L)
	{
		try
		{
			CityTroop obj = (CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float time = (float)Lua.lua_tonumber(L, 2);
			obj.Move(time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnterWorkGarbage(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnterWorkGarbage();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnterWorkAttack(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnterWorkAttack();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRealEndPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 realEndPos = ((CityTroop)objectTranslator.FastGetCSObj(L, 1)).GetRealEndPos();
			objectTranslator.PushUnityEngineVector3(L, realEndPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnterWorkOpenFog(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnterWorkOpenFog();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayGarbageSuccess(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayGarbageSuccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayGarbageFail(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayGarbageFail();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearTroopUnits(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearTroopUnits();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MoveAfterCreate(IntPtr L)
	{
		try
		{
			CityTroop obj = (CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int targetPosition = Lua.xlua_tointeger(L, 2);
			obj.MoveAfterCreate(targetPosition);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTruck(IntPtr L)
	{
		try
		{
			bool value = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsTruck();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanLongTap(IntPtr L)
	{
		try
		{
			bool value = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CanLongTap();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = ((CityTroop)objectTranslator.FastGetCSObj(L, 1)).GetTransform();
			objectTranslator.Push(L, transform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PointInPick(IntPtr L)
	{
		try
		{
			bool value = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PointInPick();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Drag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			cityTroop.Drag(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Select(IntPtr L)
	{
		try
		{
			bool value = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Select();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Click(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Click();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowHeadUI(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowHeadUI();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOutRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = cityTroop.IsOutRange(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeTouchPos(IntPtr L)
	{
		try
		{
			CityTroop obj = (CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			obj.ChangeTouchPos(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetClosestPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 closestPoint = cityTroop.GetClosestPoint(val);
			objectTranslator.PushUnityEngineVector3(L, closestPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEndDrag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = cityTroop.OnEndDrag(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnClick(IntPtr L)
	{
		try
		{
			bool value = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnClick();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnEndLongTap(IntPtr L)
	{
		try
		{
			bool value = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnEndLongTap();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeginLongTap(IntPtr L)
	{
		try
		{
			bool value = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBeginLongTap();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTruckPickGarbageTroop(IntPtr L)
	{
		try
		{
			bool value = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsTruckPickGarbageTroop();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsWorkmanPickGarbageTroop(IntPtr L)
	{
		try
		{
			bool value = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsWorkmanPickGarbageTroop();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTrunkPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 trunkPosition = ((CityTroop)objectTranslator.FastGetCSObj(L, 1)).GetTrunkPosition();
			objectTranslator.PushUnityEngineVector3(L, trunkPosition);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPickGarbageMoveDistance(IntPtr L)
	{
		try
		{
			float pickGarbageMoveDistance = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPickGarbageMoveDistance();
			Lua.lua_pushnumber(L, pickGarbageMoveDistance);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOpenFogMoveDistance(IntPtr L)
	{
		try
		{
			float openFogMoveDistance = ((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetOpenFogMoveDistance();
			Lua.lua_pushnumber(L, openFogMoveDistance);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TroopUnitsBirthThenPickGarbage(IntPtr L)
	{
		try
		{
			CityTroop cityTroop = (CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool appear = Lua.lua_toboolean(L, 2);
				cityTroop.TroopUnitsBirthThenPickGarbage(appear);
				return 0;
			}
			if (num == 1)
			{
				cityTroop.TroopUnitsBirthThenPickGarbage();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityTroop.TroopUnitsBirthThenPickGarbage!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TroopUnitPickSuccess(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TroopUnitPickSuccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PreviewType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityTroop.PreviewType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EndPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, cityTroop.EndPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TmpEndPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, cityTroop.TmpEndPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OpenFogPointIndex(IntPtr L)
	{
		try
		{
			CityTroop cityTroop = (CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityTroop.OpenFogPointIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Priority(IntPtr L)
	{
		try
		{
			CityTroop cityTroop = (CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, cityTroop.Priority);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TilePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityTroop.TilePos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_EndPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			cityTroop.EndPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_TmpEndPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = (CityTroop)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			cityTroop.TmpEndPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OpenFogPointIndex(IntPtr L)
	{
		try
		{
			((CityTroop)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OpenFogPointIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
