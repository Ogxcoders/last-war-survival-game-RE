using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using GameKit.Base;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineTransformWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Transform);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 105, 19, 13);
		Utils.RegisterFunc(L, -3, "SetParent", _m_SetParent);
		Utils.RegisterFunc(L, -3, "SetPositionAndRotation", _m_SetPositionAndRotation);
		Utils.RegisterFunc(L, -3, "Translate", _m_Translate);
		Utils.RegisterFunc(L, -3, "Rotate", _m_Rotate);
		Utils.RegisterFunc(L, -3, "RotateAround", _m_RotateAround);
		Utils.RegisterFunc(L, -3, "LookAt", _m_LookAt);
		Utils.RegisterFunc(L, -3, "TransformDirection", _m_TransformDirection);
		Utils.RegisterFunc(L, -3, "InverseTransformDirection", _m_InverseTransformDirection);
		Utils.RegisterFunc(L, -3, "TransformVector", _m_TransformVector);
		Utils.RegisterFunc(L, -3, "InverseTransformVector", _m_InverseTransformVector);
		Utils.RegisterFunc(L, -3, "TransformPoint", _m_TransformPoint);
		Utils.RegisterFunc(L, -3, "InverseTransformPoint", _m_InverseTransformPoint);
		Utils.RegisterFunc(L, -3, "DetachChildren", _m_DetachChildren);
		Utils.RegisterFunc(L, -3, "SetAsFirstSibling", _m_SetAsFirstSibling);
		Utils.RegisterFunc(L, -3, "SetAsLastSibling", _m_SetAsLastSibling);
		Utils.RegisterFunc(L, -3, "SetSiblingIndex", _m_SetSiblingIndex);
		Utils.RegisterFunc(L, -3, "GetSiblingIndex", _m_GetSiblingIndex);
		Utils.RegisterFunc(L, -3, "Find", _m_Find);
		Utils.RegisterFunc(L, -3, "IsChildOf", _m_IsChildOf);
		Utils.RegisterFunc(L, -3, "GetEnumerator", _m_GetEnumerator);
		Utils.RegisterFunc(L, -3, "GetChild", _m_GetChild);
		Utils.RegisterFunc(L, -3, "DOMove", _m_DOMove);
		Utils.RegisterFunc(L, -3, "DOMoveX", _m_DOMoveX);
		Utils.RegisterFunc(L, -3, "DOMoveY", _m_DOMoveY);
		Utils.RegisterFunc(L, -3, "DOMoveZ", _m_DOMoveZ);
		Utils.RegisterFunc(L, -3, "DOLocalMove", _m_DOLocalMove);
		Utils.RegisterFunc(L, -3, "DOLocalMoveX", _m_DOLocalMoveX);
		Utils.RegisterFunc(L, -3, "DOLocalMoveY", _m_DOLocalMoveY);
		Utils.RegisterFunc(L, -3, "DOLocalMoveZ", _m_DOLocalMoveZ);
		Utils.RegisterFunc(L, -3, "DORotate", _m_DORotate);
		Utils.RegisterFunc(L, -3, "DORotateQuaternion", _m_DORotateQuaternion);
		Utils.RegisterFunc(L, -3, "DOLocalRotate", _m_DOLocalRotate);
		Utils.RegisterFunc(L, -3, "DOLocalRotateQuaternion", _m_DOLocalRotateQuaternion);
		Utils.RegisterFunc(L, -3, "DOScale", _m_DOScale);
		Utils.RegisterFunc(L, -3, "DOScaleX", _m_DOScaleX);
		Utils.RegisterFunc(L, -3, "DOScaleY", _m_DOScaleY);
		Utils.RegisterFunc(L, -3, "DOScaleZ", _m_DOScaleZ);
		Utils.RegisterFunc(L, -3, "DOLookAt", _m_DOLookAt);
		Utils.RegisterFunc(L, -3, "DOPunchPosition", _m_DOPunchPosition);
		Utils.RegisterFunc(L, -3, "DOPunchScale", _m_DOPunchScale);
		Utils.RegisterFunc(L, -3, "DOPunchRotation", _m_DOPunchRotation);
		Utils.RegisterFunc(L, -3, "DOShakePosition", _m_DOShakePosition);
		Utils.RegisterFunc(L, -3, "DOShakeRotation", _m_DOShakeRotation);
		Utils.RegisterFunc(L, -3, "DOShakeScale", _m_DOShakeScale);
		Utils.RegisterFunc(L, -3, "DOJump", _m_DOJump);
		Utils.RegisterFunc(L, -3, "DOLocalJump", _m_DOLocalJump);
		Utils.RegisterFunc(L, -3, "DOPath", _m_DOPath);
		Utils.RegisterFunc(L, -3, "DOLocalPath", _m_DOLocalPath);
		Utils.RegisterFunc(L, -3, "DOBlendableMoveBy", _m_DOBlendableMoveBy);
		Utils.RegisterFunc(L, -3, "DOBlendableLocalMoveBy", _m_DOBlendableLocalMoveBy);
		Utils.RegisterFunc(L, -3, "DOBlendableRotateBy", _m_DOBlendableRotateBy);
		Utils.RegisterFunc(L, -3, "DOBlendableLocalRotateBy", _m_DOBlendableLocalRotateBy);
		Utils.RegisterFunc(L, -3, "DOBlendablePunchRotation", _m_DOBlendablePunchRotation);
		Utils.RegisterFunc(L, -3, "DOBlendableScaleBy", _m_DOBlendableScaleBy);
		Utils.RegisterFunc(L, -3, "GetOrAddTransform", _m_GetOrAddTransform);
		Utils.RegisterFunc(L, -3, "SetPositionX", _m_SetPositionX);
		Utils.RegisterFunc(L, -3, "SetPositionY", _m_SetPositionY);
		Utils.RegisterFunc(L, -3, "SetPositionZ", _m_SetPositionZ);
		Utils.RegisterFunc(L, -3, "AddPositionX", _m_AddPositionX);
		Utils.RegisterFunc(L, -3, "AddPositionY", _m_AddPositionY);
		Utils.RegisterFunc(L, -3, "AddPositionZ", _m_AddPositionZ);
		Utils.RegisterFunc(L, -3, "SetLocalPositionX", _m_SetLocalPositionX);
		Utils.RegisterFunc(L, -3, "SetLocalPositionY", _m_SetLocalPositionY);
		Utils.RegisterFunc(L, -3, "SetLocalPositionZ", _m_SetLocalPositionZ);
		Utils.RegisterFunc(L, -3, "AddLocalPositionX", _m_AddLocalPositionX);
		Utils.RegisterFunc(L, -3, "AddLocalPositionY", _m_AddLocalPositionY);
		Utils.RegisterFunc(L, -3, "AddLocalPositionZ", _m_AddLocalPositionZ);
		Utils.RegisterFunc(L, -3, "SetLocalScaleX", _m_SetLocalScaleX);
		Utils.RegisterFunc(L, -3, "SetLocalScaleY", _m_SetLocalScaleY);
		Utils.RegisterFunc(L, -3, "SetLocalScaleZ", _m_SetLocalScaleZ);
		Utils.RegisterFunc(L, -3, "AddLocalScaleX", _m_AddLocalScaleX);
		Utils.RegisterFunc(L, -3, "AddLocalScaleY", _m_AddLocalScaleY);
		Utils.RegisterFunc(L, -3, "AddLocalScaleZ", _m_AddLocalScaleZ);
		Utils.RegisterFunc(L, -3, "FindChildByName", _m_FindChildByName);
		Utils.RegisterFunc(L, -3, "FindAndGetComponent", _m_FindAndGetComponent);
		Utils.RegisterFunc(L, -3, "GetComponent_RectTransform", _m_GetComponent_RectTransform);
		Utils.RegisterFunc(L, -3, "GetComponent_Text", _m_GetComponent_Text);
		Utils.RegisterFunc(L, -3, "GetComponent_Image", _m_GetComponent_Image);
		Utils.RegisterFunc(L, -3, "GetComponent_Button", _m_GetComponent_Button);
		Utils.RegisterFunc(L, -3, "FindId", _m_FindId);
		Utils.RegisterFunc(L, -3, "Set_position", _m_Set_position);
		Utils.RegisterFunc(L, -3, "Set_positionX", _m_Set_positionX);
		Utils.RegisterFunc(L, -3, "Set_positionY", _m_Set_positionY);
		Utils.RegisterFunc(L, -3, "Set_positionZ", _m_Set_positionZ);
		Utils.RegisterFunc(L, -3, "Get_position", _m_Get_position);
		Utils.RegisterFunc(L, -3, "Set_localPosition", _m_Set_localPosition);
		Utils.RegisterFunc(L, -3, "Get_localPosition", _m_Get_localPosition);
		Utils.RegisterFunc(L, -3, "Set_localScale", _m_Set_localScale);
		Utils.RegisterFunc(L, -3, "Get_localScale", _m_Get_localScale);
		Utils.RegisterFunc(L, -3, "Get_lossyScale", _m_Get_lossyScale);
		Utils.RegisterFunc(L, -3, "Set_eulerAngles", _m_Set_eulerAngles);
		Utils.RegisterFunc(L, -3, "Get_eulerAngles", _m_Get_eulerAngles);
		Utils.RegisterFunc(L, -3, "Set_localEulerAngles", _m_Set_localEulerAngles);
		Utils.RegisterFunc(L, -3, "Get_localEulerAngles", _m_Get_localEulerAngles);
		Utils.RegisterFunc(L, -3, "Get_rotation", _m_Get_rotation);
		Utils.RegisterFunc(L, -3, "Set_rotation", _m_Set_rotation);
		Utils.RegisterFunc(L, -3, "Get_localRotation", _m_Get_localRotation);
		Utils.RegisterFunc(L, -3, "Set_localRotation", _m_Set_localRotation);
		Utils.RegisterFunc(L, -3, "Get_forward", _m_Get_forward);
		Utils.RegisterFunc(L, -3, "Set_forward", _m_Set_forward);
		Utils.RegisterFunc(L, -3, "Get_right", _m_Get_right);
		Utils.RegisterFunc(L, -3, "Set_right", _m_Set_right);
		Utils.RegisterFunc(L, -3, "Get_up", _m_Get_up);
		Utils.RegisterFunc(L, -3, "Set_up", _m_Set_up);
		Utils.RegisterFunc(L, -3, "Reset", _m_Reset);
		Utils.RegisterFunc(L, -2, "position", _g_get_position);
		Utils.RegisterFunc(L, -2, "localPosition", _g_get_localPosition);
		Utils.RegisterFunc(L, -2, "eulerAngles", _g_get_eulerAngles);
		Utils.RegisterFunc(L, -2, "localEulerAngles", _g_get_localEulerAngles);
		Utils.RegisterFunc(L, -2, "right", _g_get_right);
		Utils.RegisterFunc(L, -2, "up", _g_get_up);
		Utils.RegisterFunc(L, -2, "forward", _g_get_forward);
		Utils.RegisterFunc(L, -2, "rotation", _g_get_rotation);
		Utils.RegisterFunc(L, -2, "localRotation", _g_get_localRotation);
		Utils.RegisterFunc(L, -2, "localScale", _g_get_localScale);
		Utils.RegisterFunc(L, -2, "parent", _g_get_parent);
		Utils.RegisterFunc(L, -2, "worldToLocalMatrix", _g_get_worldToLocalMatrix);
		Utils.RegisterFunc(L, -2, "localToWorldMatrix", _g_get_localToWorldMatrix);
		Utils.RegisterFunc(L, -2, "root", _g_get_root);
		Utils.RegisterFunc(L, -2, "childCount", _g_get_childCount);
		Utils.RegisterFunc(L, -2, "lossyScale", _g_get_lossyScale);
		Utils.RegisterFunc(L, -2, "hasChanged", _g_get_hasChanged);
		Utils.RegisterFunc(L, -2, "hierarchyCapacity", _g_get_hierarchyCapacity);
		Utils.RegisterFunc(L, -2, "hierarchyCount", _g_get_hierarchyCount);
		Utils.RegisterFunc(L, -1, "position", _s_set_position);
		Utils.RegisterFunc(L, -1, "localPosition", _s_set_localPosition);
		Utils.RegisterFunc(L, -1, "eulerAngles", _s_set_eulerAngles);
		Utils.RegisterFunc(L, -1, "localEulerAngles", _s_set_localEulerAngles);
		Utils.RegisterFunc(L, -1, "right", _s_set_right);
		Utils.RegisterFunc(L, -1, "up", _s_set_up);
		Utils.RegisterFunc(L, -1, "forward", _s_set_forward);
		Utils.RegisterFunc(L, -1, "rotation", _s_set_rotation);
		Utils.RegisterFunc(L, -1, "localRotation", _s_set_localRotation);
		Utils.RegisterFunc(L, -1, "localScale", _s_set_localScale);
		Utils.RegisterFunc(L, -1, "parent", _s_set_parent);
		Utils.RegisterFunc(L, -1, "hasChanged", _s_set_hasChanged);
		Utils.RegisterFunc(L, -1, "hierarchyCapacity", _s_set_hierarchyCapacity);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.Transform does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetParent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Transform>(L, 2))
			{
				Transform parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				transform.SetParent(parent);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Transform>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Transform parent2 = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				bool worldPositionStays = Lua.lua_toboolean(L, 3);
				transform.SetParent(parent2, worldPositionStays);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.SetParent!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPositionAndRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Quaternion val2);
			transform.SetPositionAndRotation(val, val2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Translate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				transform.Translate(x, y, z);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				transform.Translate(val);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Space>(L, 5))
			{
				float x2 = (float)Lua.lua_tonumber(L, 2);
				float y2 = (float)Lua.lua_tonumber(L, 3);
				float z2 = (float)Lua.lua_tonumber(L, 4);
				objectTranslator.Get(L, 5, out Space val2);
				transform.Translate(x2, y2, z2, val2);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Transform>(L, 5))
			{
				float x3 = (float)Lua.lua_tonumber(L, 2);
				float y3 = (float)Lua.lua_tonumber(L, 3);
				float z3 = (float)Lua.lua_tonumber(L, 4);
				Transform relativeTo = (Transform)objectTranslator.GetObject(L, 5, typeof(Transform));
				transform.Translate(x3, y3, z3, relativeTo);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Space>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Space val4);
				transform.Translate(val3, val4);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Transform>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val5);
				Transform relativeTo2 = (Transform)objectTranslator.GetObject(L, 3, typeof(Transform));
				transform.Translate(val5, relativeTo2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.Translate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rotate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float xAngle = (float)Lua.lua_tonumber(L, 2);
				float yAngle = (float)Lua.lua_tonumber(L, 3);
				float zAngle = (float)Lua.lua_tonumber(L, 4);
				transform.Rotate(xAngle, yAngle, zAngle);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				transform.Rotate(val);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float angle = (float)Lua.lua_tonumber(L, 3);
				transform.Rotate(val2, angle);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Space>(L, 5))
			{
				float xAngle2 = (float)Lua.lua_tonumber(L, 2);
				float yAngle2 = (float)Lua.lua_tonumber(L, 3);
				float zAngle2 = (float)Lua.lua_tonumber(L, 4);
				objectTranslator.Get(L, 5, out Space val3);
				transform.Rotate(xAngle2, yAngle2, zAngle2, val3);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Space>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val4);
				objectTranslator.Get(L, 3, out Space val5);
				transform.Rotate(val4, val5);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Space>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val6);
				float angle2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Space val7);
				transform.Rotate(val6, angle2, val7);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.Rotate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RotateAround(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector3 val2);
			float angle = (float)Lua.lua_tonumber(L, 4);
			transform.RotateAround(val, val2, angle);
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
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Transform>(L, 2))
			{
				Transform target = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				transform.LookAt(target);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				transform.LookAt(val);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Transform>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				Transform target2 = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				objectTranslator.Get(L, 3, out Vector3 val2);
				transform.LookAt(target2, val2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Vector3 val4);
				transform.LookAt(val3, val4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.LookAt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TransformDirection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				Vector3 val = transform.TransformDirection(x, y, z);
				objectTranslator.PushUnityEngineVector3(L, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				Vector3 val3 = transform.TransformDirection(val2);
				objectTranslator.PushUnityEngineVector3(L, val3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.TransformDirection!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InverseTransformDirection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				Vector3 val = transform.InverseTransformDirection(x, y, z);
				objectTranslator.PushUnityEngineVector3(L, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				Vector3 val3 = transform.InverseTransformDirection(val2);
				objectTranslator.PushUnityEngineVector3(L, val3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.InverseTransformDirection!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TransformVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				Vector3 val = transform.TransformVector(x, y, z);
				objectTranslator.PushUnityEngineVector3(L, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				Vector3 val3 = transform.TransformVector(val2);
				objectTranslator.PushUnityEngineVector3(L, val3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.TransformVector!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InverseTransformVector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				Vector3 val = transform.InverseTransformVector(x, y, z);
				objectTranslator.PushUnityEngineVector3(L, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				Vector3 val3 = transform.InverseTransformVector(val2);
				objectTranslator.PushUnityEngineVector3(L, val3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.InverseTransformVector!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TransformPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				Vector3 val = transform.TransformPoint(x, y, z);
				objectTranslator.PushUnityEngineVector3(L, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				Vector3 val3 = transform.TransformPoint(val2);
				objectTranslator.PushUnityEngineVector3(L, val3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.TransformPoint!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InverseTransformPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				Vector3 val = transform.InverseTransformPoint(x, y, z);
				objectTranslator.PushUnityEngineVector3(L, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				Vector3 val3 = transform.InverseTransformPoint(val2);
				objectTranslator.PushUnityEngineVector3(L, val3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.InverseTransformPoint!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DetachChildren(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DetachChildren();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAsFirstSibling(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetAsFirstSibling();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAsLastSibling(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetAsLastSibling();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSiblingIndex(IntPtr L)
	{
		try
		{
			Transform obj = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int siblingIndex = Lua.xlua_tointeger(L, 2);
			obj.SetSiblingIndex(siblingIndex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSiblingIndex(IntPtr L)
	{
		try
		{
			int siblingIndex = ((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSiblingIndex();
			Lua.xlua_pushinteger(L, siblingIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Find(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform obj = (Transform)objectTranslator.FastGetCSObj(L, 1);
			string n = Lua.lua_tostring(L, 2);
			Transform o = obj.Find(n);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsChildOf(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			Transform parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			bool value = transform.IsChildOf(parent);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetEnumerator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			IEnumerator enumerator = ((Transform)objectTranslator.FastGetCSObj(L, 1)).GetEnumerator();
			objectTranslator.PushAny(L, enumerator);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetChild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform obj = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			Transform child = obj.GetChild(index);
			objectTranslator.Push(L, child);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOMove(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOMove(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOMove(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOMove!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOMoveX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOMoveX(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOMoveX(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOMoveX!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOMoveY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOMoveY(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOMoveY(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOMoveY!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOMoveZ(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOMoveZ(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOMoveZ(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOMoveZ!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOLocalMove(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOLocalMove(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOLocalMove(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOLocalMove!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOLocalMoveX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOLocalMoveX(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOLocalMoveX(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOLocalMoveX!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOLocalMoveY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOLocalMoveY(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOLocalMoveY(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOLocalMoveY!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOLocalMoveZ(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOLocalMoveZ(endValue, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue2 = (float)Lua.lua_tonumber(L, 2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOLocalMoveZ(endValue2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOLocalMoveZ!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORotate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<RotateMode>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out RotateMode v);
				TweenerCore<Quaternion, Vector3, QuaternionOptions> o = target.DORotate(val, duration, v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Quaternion, Vector3, QuaternionOptions> o2 = target.DORotate(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DORotate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORotateQuaternion(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			TweenerCore<Quaternion, Quaternion, NoOptions> o = ShortcutExtensions.DORotateQuaternion(duration: (float)Lua.lua_tonumber(L, 3), target: target, endValue: val);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOLocalRotate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<RotateMode>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out RotateMode v);
				TweenerCore<Quaternion, Vector3, QuaternionOptions> o = target.DOLocalRotate(val, duration, v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Quaternion, Vector3, QuaternionOptions> o2 = target.DOLocalRotate(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOLocalRotate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOLocalRotateQuaternion(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			TweenerCore<Quaternion, Quaternion, NoOptions> o = ShortcutExtensions.DOLocalRotateQuaternion(duration: (float)Lua.lua_tonumber(L, 3), target: target, endValue: val);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float endValue = (float)Lua.lua_tonumber(L, 2);
				float duration = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOScale(endValue, duration);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Vector3, VectorOptions> o2 = target.DOScale(val, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOScale!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOScaleX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOScaleX(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOScaleY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOScaleY(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOScaleZ(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<Vector3, Vector3, VectorOptions> o = target.DOScaleZ(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOLookAt(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<AxisConstraint>(L, 4) && objectTranslator.Assignable<Vector3?>(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out AxisConstraint v);
				objectTranslator.Get(L, 5, out Vector3? v2);
				Tweener o = target.DOLookAt(val, duration, v, v2);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<AxisConstraint>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out AxisConstraint v3);
				Tweener o2 = target.DOLookAt(val2, duration2, v3);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				Tweener o3 = target.DOLookAt(val3, duration3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOLookAt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPunchPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float elasticity = (float)Lua.lua_tonumber(L, 5);
				bool snapping = Lua.lua_toboolean(L, 6);
				Tweener o = target.DOPunchPosition(val, duration, vibrato, elasticity, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				float elasticity2 = (float)Lua.lua_tonumber(L, 5);
				Tweener o2 = target.DOPunchPosition(val2, duration2, vibrato2, elasticity2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				int vibrato3 = Lua.xlua_tointeger(L, 4);
				Tweener o3 = target.DOPunchPosition(val3, duration3, vibrato3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val4);
				float duration4 = (float)Lua.lua_tonumber(L, 3);
				Tweener o4 = target.DOPunchPosition(val4, duration4);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOPunchPosition!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPunchScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float elasticity = (float)Lua.lua_tonumber(L, 5);
				Tweener o = target.DOPunchScale(val, duration, vibrato, elasticity);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				Tweener o2 = target.DOPunchScale(val2, duration2, vibrato2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				Tweener o3 = target.DOPunchScale(val3, duration3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOPunchScale!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPunchRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float elasticity = (float)Lua.lua_tonumber(L, 5);
				Tweener o = target.DOPunchRotation(val, duration, vibrato, elasticity);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				Tweener o2 = target.DOPunchRotation(val2, duration2, vibrato2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				Tweener o3 = target.DOPunchRotation(val3, duration3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOPunchRotation!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOShakePosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				float duration = (float)Lua.lua_tonumber(L, 2);
				float strength = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float randomness = (float)Lua.lua_tonumber(L, 5);
				bool snapping = Lua.lua_toboolean(L, 6);
				bool fadeOut = Lua.lua_toboolean(L, 7);
				Tweener o = target.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration2 = (float)Lua.lua_tonumber(L, 2);
				float strength2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				float randomness2 = (float)Lua.lua_tonumber(L, 5);
				bool snapping2 = Lua.lua_toboolean(L, 6);
				Tweener o2 = target.DOShakePosition(duration2, strength2, vibrato2, randomness2, snapping2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration3 = (float)Lua.lua_tonumber(L, 2);
				float strength3 = (float)Lua.lua_tonumber(L, 3);
				int vibrato3 = Lua.xlua_tointeger(L, 4);
				float randomness3 = (float)Lua.lua_tonumber(L, 5);
				Tweener o3 = target.DOShakePosition(duration3, strength3, vibrato3, randomness3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration4 = (float)Lua.lua_tonumber(L, 2);
				float strength4 = (float)Lua.lua_tonumber(L, 3);
				int vibrato4 = Lua.xlua_tointeger(L, 4);
				Tweener o4 = target.DOShakePosition(duration4, strength4, vibrato4);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float duration5 = (float)Lua.lua_tonumber(L, 2);
				float strength5 = (float)Lua.lua_tonumber(L, 3);
				Tweener o5 = target.DOShakePosition(duration5, strength5);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float duration6 = (float)Lua.lua_tonumber(L, 2);
				Tweener o6 = target.DOShakePosition(duration6);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				float duration7 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				int vibrato5 = Lua.xlua_tointeger(L, 4);
				float randomness4 = (float)Lua.lua_tonumber(L, 5);
				bool snapping3 = Lua.lua_toboolean(L, 6);
				bool fadeOut2 = Lua.lua_toboolean(L, 7);
				Tweener o7 = target.DOShakePosition(duration7, val, vibrato5, randomness4, snapping3, fadeOut2);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration8 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				int vibrato6 = Lua.xlua_tointeger(L, 4);
				float randomness5 = (float)Lua.lua_tonumber(L, 5);
				bool snapping4 = Lua.lua_toboolean(L, 6);
				Tweener o8 = target.DOShakePosition(duration8, val2, vibrato6, randomness5, snapping4);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration9 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				int vibrato7 = Lua.xlua_tointeger(L, 4);
				float randomness6 = (float)Lua.lua_tonumber(L, 5);
				Tweener o9 = target.DOShakePosition(duration9, val3, vibrato7, randomness6);
				objectTranslator.Push(L, o9);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration10 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val4);
				int vibrato8 = Lua.xlua_tointeger(L, 4);
				Tweener o10 = target.DOShakePosition(duration10, val4, vibrato8);
				objectTranslator.Push(L, o10);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				float duration11 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val5);
				Tweener o11 = target.DOShakePosition(duration11, val5);
				objectTranslator.Push(L, o11);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOShakePosition!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOShakeRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration = (float)Lua.lua_tonumber(L, 2);
				float strength = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float randomness = (float)Lua.lua_tonumber(L, 5);
				bool fadeOut = Lua.lua_toboolean(L, 6);
				Tweener o = target.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration2 = (float)Lua.lua_tonumber(L, 2);
				float strength2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				float randomness2 = (float)Lua.lua_tonumber(L, 5);
				Tweener o2 = target.DOShakeRotation(duration2, strength2, vibrato2, randomness2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration3 = (float)Lua.lua_tonumber(L, 2);
				float strength3 = (float)Lua.lua_tonumber(L, 3);
				int vibrato3 = Lua.xlua_tointeger(L, 4);
				Tweener o3 = target.DOShakeRotation(duration3, strength3, vibrato3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float duration4 = (float)Lua.lua_tonumber(L, 2);
				float strength4 = (float)Lua.lua_tonumber(L, 3);
				Tweener o4 = target.DOShakeRotation(duration4, strength4);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float duration5 = (float)Lua.lua_tonumber(L, 2);
				Tweener o5 = target.DOShakeRotation(duration5);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration6 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				int vibrato4 = Lua.xlua_tointeger(L, 4);
				float randomness3 = (float)Lua.lua_tonumber(L, 5);
				bool fadeOut2 = Lua.lua_toboolean(L, 6);
				Tweener o6 = target.DOShakeRotation(duration6, val, vibrato4, randomness3, fadeOut2);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration7 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				int vibrato5 = Lua.xlua_tointeger(L, 4);
				float randomness4 = (float)Lua.lua_tonumber(L, 5);
				Tweener o7 = target.DOShakeRotation(duration7, val2, vibrato5, randomness4);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration8 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				int vibrato6 = Lua.xlua_tointeger(L, 4);
				Tweener o8 = target.DOShakeRotation(duration8, val3, vibrato6);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				float duration9 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val4);
				Tweener o9 = target.DOShakeRotation(duration9, val4);
				objectTranslator.Push(L, o9);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOShakeRotation!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOShakeScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration = (float)Lua.lua_tonumber(L, 2);
				float strength = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float randomness = (float)Lua.lua_tonumber(L, 5);
				bool fadeOut = Lua.lua_toboolean(L, 6);
				Tweener o = target.DOShakeScale(duration, strength, vibrato, randomness, fadeOut);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration2 = (float)Lua.lua_tonumber(L, 2);
				float strength2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				float randomness2 = (float)Lua.lua_tonumber(L, 5);
				Tweener o2 = target.DOShakeScale(duration2, strength2, vibrato2, randomness2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration3 = (float)Lua.lua_tonumber(L, 2);
				float strength3 = (float)Lua.lua_tonumber(L, 3);
				int vibrato3 = Lua.xlua_tointeger(L, 4);
				Tweener o3 = target.DOShakeScale(duration3, strength3, vibrato3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float duration4 = (float)Lua.lua_tonumber(L, 2);
				float strength4 = (float)Lua.lua_tonumber(L, 3);
				Tweener o4 = target.DOShakeScale(duration4, strength4);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float duration5 = (float)Lua.lua_tonumber(L, 2);
				Tweener o5 = target.DOShakeScale(duration5);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration6 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				int vibrato4 = Lua.xlua_tointeger(L, 4);
				float randomness3 = (float)Lua.lua_tonumber(L, 5);
				bool fadeOut2 = Lua.lua_toboolean(L, 6);
				Tweener o6 = target.DOShakeScale(duration6, val, vibrato4, randomness3, fadeOut2);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration7 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				int vibrato5 = Lua.xlua_tointeger(L, 4);
				float randomness4 = (float)Lua.lua_tonumber(L, 5);
				Tweener o7 = target.DOShakeScale(duration7, val2, vibrato5, randomness4);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration8 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				int vibrato6 = Lua.xlua_tointeger(L, 4);
				Tweener o8 = target.DOShakeScale(duration8, val3, vibrato6);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				float duration9 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val4);
				Tweener o9 = target.DOShakeScale(duration9, val4);
				objectTranslator.Push(L, o9);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOShakeScale!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOJump(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float jumpPower = (float)Lua.lua_tonumber(L, 3);
				int numJumps = Lua.xlua_tointeger(L, 4);
				float duration = (float)Lua.lua_tonumber(L, 5);
				bool snapping = Lua.lua_toboolean(L, 6);
				Sequence o = target.DOJump(val, jumpPower, numJumps, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float jumpPower2 = (float)Lua.lua_tonumber(L, 3);
				int numJumps2 = Lua.xlua_tointeger(L, 4);
				float duration2 = (float)Lua.lua_tonumber(L, 5);
				Sequence o2 = target.DOJump(val2, jumpPower2, numJumps2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOJump!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOLocalJump(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float jumpPower = (float)Lua.lua_tonumber(L, 3);
				int numJumps = Lua.xlua_tointeger(L, 4);
				float duration = (float)Lua.lua_tonumber(L, 5);
				bool snapping = Lua.lua_toboolean(L, 6);
				Sequence o = target.DOLocalJump(val, jumpPower, numJumps, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float jumpPower2 = (float)Lua.lua_tonumber(L, 3);
				int numJumps2 = Lua.xlua_tointeger(L, 4);
				float duration2 = (float)Lua.lua_tonumber(L, 5);
				Sequence o2 = target.DOLocalJump(val2, jumpPower2, numJumps2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOLocalJump!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPath(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Path>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PathMode>(L, 4))
			{
				Path path = (Path)objectTranslator.GetObject(L, 2, typeof(Path));
				float duration = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PathMode v);
				TweenerCore<Vector3, Path, PathOptions> o = target.DOPath(path, duration, v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Path>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Path path2 = (Path)objectTranslator.GetObject(L, 2, typeof(Path));
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Path, PathOptions> o2 = target.DOPath(path2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PathType>(L, 4) && objectTranslator.Assignable<PathMode>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Color?>(L, 7))
			{
				Vector3[] path3 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PathType v2);
				objectTranslator.Get(L, 5, out PathMode v3);
				int resolution = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out Color? v4);
				TweenerCore<Vector3, Path, PathOptions> o3 = target.DOPath(path3, duration3, v2, v3, resolution, v4);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PathType>(L, 4) && objectTranslator.Assignable<PathMode>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				Vector3[] path4 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				float duration4 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PathType v5);
				objectTranslator.Get(L, 5, out PathMode v6);
				int resolution2 = Lua.xlua_tointeger(L, 6);
				TweenerCore<Vector3, Path, PathOptions> o4 = target.DOPath(path4, duration4, v5, v6, resolution2);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PathType>(L, 4) && objectTranslator.Assignable<PathMode>(L, 5))
			{
				Vector3[] path5 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				float duration5 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PathType v7);
				objectTranslator.Get(L, 5, out PathMode v8);
				TweenerCore<Vector3, Path, PathOptions> o5 = target.DOPath(path5, duration5, v7, v8);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PathType>(L, 4))
			{
				Vector3[] path6 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				float duration6 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PathType v9);
				TweenerCore<Vector3, Path, PathOptions> o6 = target.DOPath(path6, duration6, v9);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Vector3[] path7 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				float duration7 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Path, PathOptions> o7 = target.DOPath(path7, duration7);
				objectTranslator.Push(L, o7);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOPath!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOLocalPath(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Path>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PathMode>(L, 4))
			{
				Path path = (Path)objectTranslator.GetObject(L, 2, typeof(Path));
				float duration = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PathMode v);
				TweenerCore<Vector3, Path, PathOptions> o = target.DOLocalPath(path, duration, v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Path>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Path path2 = (Path)objectTranslator.GetObject(L, 2, typeof(Path));
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Path, PathOptions> o2 = target.DOLocalPath(path2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PathType>(L, 4) && objectTranslator.Assignable<PathMode>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Color?>(L, 7))
			{
				Vector3[] path3 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PathType v2);
				objectTranslator.Get(L, 5, out PathMode v3);
				int resolution = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out Color? v4);
				TweenerCore<Vector3, Path, PathOptions> o3 = target.DOLocalPath(path3, duration3, v2, v3, resolution, v4);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PathType>(L, 4) && objectTranslator.Assignable<PathMode>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				Vector3[] path4 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				float duration4 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PathType v5);
				objectTranslator.Get(L, 5, out PathMode v6);
				int resolution2 = Lua.xlua_tointeger(L, 6);
				TweenerCore<Vector3, Path, PathOptions> o4 = target.DOLocalPath(path4, duration4, v5, v6, resolution2);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PathType>(L, 4) && objectTranslator.Assignable<PathMode>(L, 5))
			{
				Vector3[] path5 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				float duration5 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PathType v7);
				objectTranslator.Get(L, 5, out PathMode v8);
				TweenerCore<Vector3, Path, PathOptions> o5 = target.DOLocalPath(path5, duration5, v7, v8);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<PathType>(L, 4))
			{
				Vector3[] path6 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				float duration6 = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out PathType v9);
				TweenerCore<Vector3, Path, PathOptions> o6 = target.DOLocalPath(path6, duration6, v9);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Vector3[] path7 = (Vector3[])objectTranslator.GetObject(L, 2, typeof(Vector3[]));
				float duration7 = (float)Lua.lua_tonumber(L, 3);
				TweenerCore<Vector3, Path, PathOptions> o7 = target.DOLocalPath(path7, duration7);
				objectTranslator.Push(L, o7);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOLocalPath!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOBlendableMoveBy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				Tweener o = target.DOBlendableMoveBy(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				Tweener o2 = target.DOBlendableMoveBy(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOBlendableMoveBy!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOBlendableLocalMoveBy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				bool snapping = Lua.lua_toboolean(L, 4);
				Tweener o = target.DOBlendableLocalMoveBy(val, duration, snapping);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				Tweener o2 = target.DOBlendableLocalMoveBy(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOBlendableLocalMoveBy!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOBlendableRotateBy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<RotateMode>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out RotateMode v);
				Tweener o = target.DOBlendableRotateBy(val, duration, v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				Tweener o2 = target.DOBlendableRotateBy(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOBlendableRotateBy!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOBlendableLocalRotateBy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<RotateMode>(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out RotateMode v);
				Tweener o = target.DOBlendableLocalRotateBy(val, duration, v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				Tweener o2 = target.DOBlendableLocalRotateBy(val2, duration2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOBlendableLocalRotateBy!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOBlendablePunchRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float elasticity = (float)Lua.lua_tonumber(L, 5);
				Tweener o = target.DOBlendablePunchRotation(val, duration, vibrato, elasticity);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				Tweener o2 = target.DOBlendablePunchRotation(val2, duration2, vibrato2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float duration3 = (float)Lua.lua_tonumber(L, 3);
				Tweener o3 = target.DOBlendablePunchRotation(val3, duration3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.DOBlendablePunchRotation!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOBlendableScaleBy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform target = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Tweener o = ShortcutExtensions.DOBlendableScaleBy(duration: (float)Lua.lua_tonumber(L, 3), target: target, byValue: val);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOrAddTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform parent = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string childName = Lua.lua_tostring(L, 2);
				Transform orAddTransform = parent.GetOrAddTransform(childName);
				objectTranslator.Push(L, orAddTransform);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Vector3>(L, 4))
			{
				string childName2 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				objectTranslator.Get(L, 4, out Vector3 val2);
				Transform orAddTransform2 = parent.GetOrAddTransform(childName2, val, val2);
				objectTranslator.Push(L, orAddTransform2);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Quaternion>(L, 4))
			{
				string childName3 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				objectTranslator.Get(L, 4, out Quaternion val4);
				Transform orAddTransform3 = parent.GetOrAddTransform(childName3, val3, val4);
				objectTranslator.Push(L, orAddTransform3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.GetOrAddTransform!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPositionX(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float newValue = (float)Lua.lua_tonumber(L, 2);
			transform.SetPositionX(newValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPositionY(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float newValue = (float)Lua.lua_tonumber(L, 2);
			transform.SetPositionY(newValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPositionZ(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float newValue = (float)Lua.lua_tonumber(L, 2);
			transform.SetPositionZ(newValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddPositionX(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaValue = (float)Lua.lua_tonumber(L, 2);
			transform.AddPositionX(deltaValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddPositionY(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaValue = (float)Lua.lua_tonumber(L, 2);
			transform.AddPositionY(deltaValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddPositionZ(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaValue = (float)Lua.lua_tonumber(L, 2);
			transform.AddPositionZ(deltaValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalPositionX(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float newValue = (float)Lua.lua_tonumber(L, 2);
			transform.SetLocalPositionX(newValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalPositionY(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float newValue = (float)Lua.lua_tonumber(L, 2);
			transform.SetLocalPositionY(newValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalPositionZ(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float newValue = (float)Lua.lua_tonumber(L, 2);
			transform.SetLocalPositionZ(newValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddLocalPositionX(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaValue = (float)Lua.lua_tonumber(L, 2);
			transform.AddLocalPositionX(deltaValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddLocalPositionY(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaValue = (float)Lua.lua_tonumber(L, 2);
			transform.AddLocalPositionY(deltaValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddLocalPositionZ(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaValue = (float)Lua.lua_tonumber(L, 2);
			transform.AddLocalPositionZ(deltaValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalScaleX(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float newValue = (float)Lua.lua_tonumber(L, 2);
			transform.SetLocalScaleX(newValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalScaleY(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float newValue = (float)Lua.lua_tonumber(L, 2);
			transform.SetLocalScaleY(newValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLocalScaleZ(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float newValue = (float)Lua.lua_tonumber(L, 2);
			transform.SetLocalScaleZ(newValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddLocalScaleX(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaValue = (float)Lua.lua_tonumber(L, 2);
			transform.AddLocalScaleX(deltaValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddLocalScaleY(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaValue = (float)Lua.lua_tonumber(L, 2);
			transform.AddLocalScaleY(deltaValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddLocalScaleZ(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaValue = (float)Lua.lua_tonumber(L, 2);
			transform.AddLocalScaleZ(deltaValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindChildByName(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform parent = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string name = Lua.lua_tostring(L, 2);
				int maxFirstLevelChildren = Lua.xlua_tointeger(L, 3);
				int maxDepth = Lua.xlua_tointeger(L, 4);
				Transform o = parent.FindChildByName(name, maxFirstLevelChildren, maxDepth);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string name2 = Lua.lua_tostring(L, 2);
				int maxFirstLevelChildren2 = Lua.xlua_tointeger(L, 3);
				Transform o2 = parent.FindChildByName(name2, maxFirstLevelChildren2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name3 = Lua.lua_tostring(L, 2);
				Transform o3 = parent.FindChildByName(name3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Transform.FindChildByName!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindAndGetComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform tran = (Transform)objectTranslator.FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			Type type = (Type)objectTranslator.GetObject(L, 3, typeof(Type));
			Component o = tran.FindAndGetComponent(path, type);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponent_RectTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component_RectTransform = ((Transform)objectTranslator.FastGetCSObj(L, 1)).GetComponent_RectTransform();
			objectTranslator.Push(L, component_RectTransform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponent_Text(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component_Text = ((Transform)objectTranslator.FastGetCSObj(L, 1)).GetComponent_Text();
			objectTranslator.Push(L, component_Text);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponent_Image(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component_Image = ((Transform)objectTranslator.FastGetCSObj(L, 1)).GetComponent_Image();
			objectTranslator.Push(L, component_Image);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetComponent_Button(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Component component_Button = ((Transform)objectTranslator.FastGetCSObj(L, 1)).GetComponent_Button();
			objectTranslator.Push(L, component_Button);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform tran = (Transform)objectTranslator.FastGetCSObj(L, 1);
			int pathToId = Lua.xlua_tointeger(L, 2);
			Transform o = tran.FindId(pathToId);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_position(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			t.Set_position(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_positionX(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			t.Set_positionX(x);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_positionY(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			t.Set_positionY(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_positionZ(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			t.Set_positionZ(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_position(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_position(out var x, out var y, out var z);
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
	private static int _m_Set_localPosition(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			t.Set_localPosition(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_localPosition(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_localPosition(out var x, out var y, out var z);
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
	private static int _m_Set_localScale(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			t.Set_localScale(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_localScale(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_localScale(out var x, out var y, out var z);
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
	private static int _m_Get_lossyScale(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_lossyScale(out var x, out var y, out var z);
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
	private static int _m_Set_eulerAngles(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			t.Set_eulerAngles(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_eulerAngles(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_eulerAngles(out var x, out var y, out var z);
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
	private static int _m_Set_localEulerAngles(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			t.Set_localEulerAngles(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_localEulerAngles(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_localEulerAngles(out var x, out var y, out var z);
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
	private static int _m_Get_rotation(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_rotation(out var x, out var y, out var z, out var w);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			Lua.lua_pushnumber(L, w);
			return 4;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_rotation(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			float w = (float)Lua.lua_tonumber(L, 5);
			t.Set_rotation(x, y, z, w);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_localRotation(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_localRotation(out var x, out var y, out var z, out var w);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			Lua.lua_pushnumber(L, w);
			return 4;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Set_localRotation(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			float w = (float)Lua.lua_tonumber(L, 5);
			t.Set_localRotation(x, y, z, w);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_forward(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_forward(out var x, out var y, out var z);
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
	private static int _m_Set_forward(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			t.Set_forward(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_right(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_right(out var x, out var y, out var z);
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
	private static int _m_Set_right(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			t.Set_right(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_up(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_up(out var x, out var y, out var z);
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
	private static int _m_Set_up(IntPtr L)
	{
		try
		{
			Transform t = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			t.Set_up(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reset(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Reset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, transform.position);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_localPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, transform.localPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_eulerAngles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, transform.eulerAngles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_localEulerAngles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, transform.localEulerAngles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_right(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, transform.right);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_up(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, transform.up);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_forward(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, transform.forward);
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
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineQuaternion(L, transform.rotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_localRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineQuaternion(L, transform.localRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_localScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, transform.localScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_parent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, transform.parent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldToLocalMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, transform.worldToLocalMatrix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_localToWorldMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, transform.localToWorldMatrix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_root(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, transform.root);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_childCount(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, transform.childCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lossyScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, transform.lossyScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasChanged(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, transform.hasChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hierarchyCapacity(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, transform.hierarchyCapacity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hierarchyCount(IntPtr L)
	{
		try
		{
			Transform transform = (Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, transform.hierarchyCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_position(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			transform.position = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_localPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			transform.localPosition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_eulerAngles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			transform.eulerAngles = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_localEulerAngles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			transform.localEulerAngles = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_right(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			transform.right = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_up(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			transform.up = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_forward(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			transform.forward = val;
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
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			transform.rotation = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_localRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			transform.localRotation = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_localScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Transform transform = (Transform)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			transform.localScale = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_parent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Transform)objectTranslator.FastGetCSObj(L, 1)).parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_hasChanged(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).hasChanged = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_hierarchyCapacity(IntPtr L)
	{
		try
		{
			((Transform)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).hierarchyCapacity = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
