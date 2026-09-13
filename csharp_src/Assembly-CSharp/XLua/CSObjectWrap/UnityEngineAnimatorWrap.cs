using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineAnimatorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Animator);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 57, 43, 19);
		Utils.RegisterFunc(L, -3, "GetFloat", _m_GetFloat);
		Utils.RegisterFunc(L, -3, "SetFloat", _m_SetFloat);
		Utils.RegisterFunc(L, -3, "GetBool", _m_GetBool);
		Utils.RegisterFunc(L, -3, "SetBool", _m_SetBool);
		Utils.RegisterFunc(L, -3, "GetInteger", _m_GetInteger);
		Utils.RegisterFunc(L, -3, "SetInteger", _m_SetInteger);
		Utils.RegisterFunc(L, -3, "SetTrigger", _m_SetTrigger);
		Utils.RegisterFunc(L, -3, "ResetTrigger", _m_ResetTrigger);
		Utils.RegisterFunc(L, -3, "IsParameterControlledByCurve", _m_IsParameterControlledByCurve);
		Utils.RegisterFunc(L, -3, "GetIKPosition", _m_GetIKPosition);
		Utils.RegisterFunc(L, -3, "SetIKPosition", _m_SetIKPosition);
		Utils.RegisterFunc(L, -3, "GetIKRotation", _m_GetIKRotation);
		Utils.RegisterFunc(L, -3, "SetIKRotation", _m_SetIKRotation);
		Utils.RegisterFunc(L, -3, "GetIKPositionWeight", _m_GetIKPositionWeight);
		Utils.RegisterFunc(L, -3, "SetIKPositionWeight", _m_SetIKPositionWeight);
		Utils.RegisterFunc(L, -3, "GetIKRotationWeight", _m_GetIKRotationWeight);
		Utils.RegisterFunc(L, -3, "SetIKRotationWeight", _m_SetIKRotationWeight);
		Utils.RegisterFunc(L, -3, "GetIKHintPosition", _m_GetIKHintPosition);
		Utils.RegisterFunc(L, -3, "SetIKHintPosition", _m_SetIKHintPosition);
		Utils.RegisterFunc(L, -3, "GetIKHintPositionWeight", _m_GetIKHintPositionWeight);
		Utils.RegisterFunc(L, -3, "SetIKHintPositionWeight", _m_SetIKHintPositionWeight);
		Utils.RegisterFunc(L, -3, "SetLookAtPosition", _m_SetLookAtPosition);
		Utils.RegisterFunc(L, -3, "SetLookAtWeight", _m_SetLookAtWeight);
		Utils.RegisterFunc(L, -3, "SetBoneLocalRotation", _m_SetBoneLocalRotation);
		Utils.RegisterFunc(L, -3, "GetBehaviours", _m_GetBehaviours);
		Utils.RegisterFunc(L, -3, "GetLayerName", _m_GetLayerName);
		Utils.RegisterFunc(L, -3, "GetLayerIndex", _m_GetLayerIndex);
		Utils.RegisterFunc(L, -3, "GetLayerWeight", _m_GetLayerWeight);
		Utils.RegisterFunc(L, -3, "SetLayerWeight", _m_SetLayerWeight);
		Utils.RegisterFunc(L, -3, "GetCurrentAnimatorStateInfo", _m_GetCurrentAnimatorStateInfo);
		Utils.RegisterFunc(L, -3, "GetNextAnimatorStateInfo", _m_GetNextAnimatorStateInfo);
		Utils.RegisterFunc(L, -3, "GetAnimatorTransitionInfo", _m_GetAnimatorTransitionInfo);
		Utils.RegisterFunc(L, -3, "GetCurrentAnimatorClipInfoCount", _m_GetCurrentAnimatorClipInfoCount);
		Utils.RegisterFunc(L, -3, "GetNextAnimatorClipInfoCount", _m_GetNextAnimatorClipInfoCount);
		Utils.RegisterFunc(L, -3, "GetCurrentAnimatorClipInfo", _m_GetCurrentAnimatorClipInfo);
		Utils.RegisterFunc(L, -3, "GetNextAnimatorClipInfo", _m_GetNextAnimatorClipInfo);
		Utils.RegisterFunc(L, -3, "IsInTransition", _m_IsInTransition);
		Utils.RegisterFunc(L, -3, "GetParameter", _m_GetParameter);
		Utils.RegisterFunc(L, -3, "MatchTarget", _m_MatchTarget);
		Utils.RegisterFunc(L, -3, "InterruptMatchTarget", _m_InterruptMatchTarget);
		Utils.RegisterFunc(L, -3, "CrossFadeInFixedTime", _m_CrossFadeInFixedTime);
		Utils.RegisterFunc(L, -3, "WriteDefaultValues", _m_WriteDefaultValues);
		Utils.RegisterFunc(L, -3, "CrossFade", _m_CrossFade);
		Utils.RegisterFunc(L, -3, "PlayInFixedTime", _m_PlayInFixedTime);
		Utils.RegisterFunc(L, -3, "Play", _m_Play);
		Utils.RegisterFunc(L, -3, "SetTarget", _m_SetTarget);
		Utils.RegisterFunc(L, -3, "GetBoneTransform", _m_GetBoneTransform);
		Utils.RegisterFunc(L, -3, "StartPlayback", _m_StartPlayback);
		Utils.RegisterFunc(L, -3, "StopPlayback", _m_StopPlayback);
		Utils.RegisterFunc(L, -3, "StartRecording", _m_StartRecording);
		Utils.RegisterFunc(L, -3, "StopRecording", _m_StopRecording);
		Utils.RegisterFunc(L, -3, "HasState", _m_HasState);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "Rebind", _m_Rebind);
		Utils.RegisterFunc(L, -3, "ApplyBuiltinRootMotion", _m_ApplyBuiltinRootMotion);
		Utils.RegisterFunc(L, -3, "PlayId", _m_PlayId);
		Utils.RegisterFunc(L, -3, "SetTriggerId", _m_SetTriggerId);
		Utils.RegisterFunc(L, -2, "isOptimizable", _g_get_isOptimizable);
		Utils.RegisterFunc(L, -2, "isHuman", _g_get_isHuman);
		Utils.RegisterFunc(L, -2, "hasRootMotion", _g_get_hasRootMotion);
		Utils.RegisterFunc(L, -2, "humanScale", _g_get_humanScale);
		Utils.RegisterFunc(L, -2, "isInitialized", _g_get_isInitialized);
		Utils.RegisterFunc(L, -2, "deltaPosition", _g_get_deltaPosition);
		Utils.RegisterFunc(L, -2, "deltaRotation", _g_get_deltaRotation);
		Utils.RegisterFunc(L, -2, "velocity", _g_get_velocity);
		Utils.RegisterFunc(L, -2, "angularVelocity", _g_get_angularVelocity);
		Utils.RegisterFunc(L, -2, "rootPosition", _g_get_rootPosition);
		Utils.RegisterFunc(L, -2, "rootRotation", _g_get_rootRotation);
		Utils.RegisterFunc(L, -2, "applyRootMotion", _g_get_applyRootMotion);
		Utils.RegisterFunc(L, -2, "updateMode", _g_get_updateMode);
		Utils.RegisterFunc(L, -2, "hasTransformHierarchy", _g_get_hasTransformHierarchy);
		Utils.RegisterFunc(L, -2, "gravityWeight", _g_get_gravityWeight);
		Utils.RegisterFunc(L, -2, "bodyPosition", _g_get_bodyPosition);
		Utils.RegisterFunc(L, -2, "bodyRotation", _g_get_bodyRotation);
		Utils.RegisterFunc(L, -2, "stabilizeFeet", _g_get_stabilizeFeet);
		Utils.RegisterFunc(L, -2, "layerCount", _g_get_layerCount);
		Utils.RegisterFunc(L, -2, "parameters", _g_get_parameters);
		Utils.RegisterFunc(L, -2, "parameterCount", _g_get_parameterCount);
		Utils.RegisterFunc(L, -2, "feetPivotActive", _g_get_feetPivotActive);
		Utils.RegisterFunc(L, -2, "pivotWeight", _g_get_pivotWeight);
		Utils.RegisterFunc(L, -2, "pivotPosition", _g_get_pivotPosition);
		Utils.RegisterFunc(L, -2, "isMatchingTarget", _g_get_isMatchingTarget);
		Utils.RegisterFunc(L, -2, "speed", _g_get_speed);
		Utils.RegisterFunc(L, -2, "targetPosition", _g_get_targetPosition);
		Utils.RegisterFunc(L, -2, "targetRotation", _g_get_targetRotation);
		Utils.RegisterFunc(L, -2, "cullingMode", _g_get_cullingMode);
		Utils.RegisterFunc(L, -2, "playbackTime", _g_get_playbackTime);
		Utils.RegisterFunc(L, -2, "recorderStartTime", _g_get_recorderStartTime);
		Utils.RegisterFunc(L, -2, "recorderStopTime", _g_get_recorderStopTime);
		Utils.RegisterFunc(L, -2, "recorderMode", _g_get_recorderMode);
		Utils.RegisterFunc(L, -2, "runtimeAnimatorController", _g_get_runtimeAnimatorController);
		Utils.RegisterFunc(L, -2, "hasBoundPlayables", _g_get_hasBoundPlayables);
		Utils.RegisterFunc(L, -2, "avatar", _g_get_avatar);
		Utils.RegisterFunc(L, -2, "playableGraph", _g_get_playableGraph);
		Utils.RegisterFunc(L, -2, "layersAffectMassCenter", _g_get_layersAffectMassCenter);
		Utils.RegisterFunc(L, -2, "leftFeetBottomHeight", _g_get_leftFeetBottomHeight);
		Utils.RegisterFunc(L, -2, "rightFeetBottomHeight", _g_get_rightFeetBottomHeight);
		Utils.RegisterFunc(L, -2, "logWarnings", _g_get_logWarnings);
		Utils.RegisterFunc(L, -2, "fireEvents", _g_get_fireEvents);
		Utils.RegisterFunc(L, -2, "keepAnimatorControllerStateOnDisable", _g_get_keepAnimatorControllerStateOnDisable);
		Utils.RegisterFunc(L, -1, "rootPosition", _s_set_rootPosition);
		Utils.RegisterFunc(L, -1, "rootRotation", _s_set_rootRotation);
		Utils.RegisterFunc(L, -1, "applyRootMotion", _s_set_applyRootMotion);
		Utils.RegisterFunc(L, -1, "updateMode", _s_set_updateMode);
		Utils.RegisterFunc(L, -1, "bodyPosition", _s_set_bodyPosition);
		Utils.RegisterFunc(L, -1, "bodyRotation", _s_set_bodyRotation);
		Utils.RegisterFunc(L, -1, "stabilizeFeet", _s_set_stabilizeFeet);
		Utils.RegisterFunc(L, -1, "feetPivotActive", _s_set_feetPivotActive);
		Utils.RegisterFunc(L, -1, "speed", _s_set_speed);
		Utils.RegisterFunc(L, -1, "cullingMode", _s_set_cullingMode);
		Utils.RegisterFunc(L, -1, "playbackTime", _s_set_playbackTime);
		Utils.RegisterFunc(L, -1, "recorderStartTime", _s_set_recorderStartTime);
		Utils.RegisterFunc(L, -1, "recorderStopTime", _s_set_recorderStopTime);
		Utils.RegisterFunc(L, -1, "runtimeAnimatorController", _s_set_runtimeAnimatorController);
		Utils.RegisterFunc(L, -1, "avatar", _s_set_avatar);
		Utils.RegisterFunc(L, -1, "layersAffectMassCenter", _s_set_layersAffectMassCenter);
		Utils.RegisterFunc(L, -1, "logWarnings", _s_set_logWarnings);
		Utils.RegisterFunc(L, -1, "fireEvents", _s_set_fireEvents);
		Utils.RegisterFunc(L, -1, "keepAnimatorControllerStateOnDisable", _s_set_keepAnimatorControllerStateOnDisable);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "StringToHash", _m_StringToHash_xlua_st_);
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
				Animator o = new Animator();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFloat(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int id = Lua.xlua_tointeger(L, 2);
				float num2 = animator.GetFloat(id);
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				float num3 = animator.GetFloat(name);
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.GetFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFloat(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int id = Lua.xlua_tointeger(L, 2);
				float value = (float)Lua.lua_tonumber(L, 3);
				animator.SetFloat(id, value);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int id2 = Lua.xlua_tointeger(L, 2);
				float value2 = (float)Lua.lua_tonumber(L, 3);
				float dampTime = (float)Lua.lua_tonumber(L, 4);
				float deltaTime = (float)Lua.lua_tonumber(L, 5);
				animator.SetFloat(id2, value2, dampTime, deltaTime);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				float value3 = (float)Lua.lua_tonumber(L, 3);
				animator.SetFloat(name, value3);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string name2 = Lua.lua_tostring(L, 2);
				float value4 = (float)Lua.lua_tonumber(L, 3);
				float dampTime2 = (float)Lua.lua_tonumber(L, 4);
				float deltaTime2 = (float)Lua.lua_tonumber(L, 5);
				animator.SetFloat(name2, value4, dampTime2, deltaTime2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.SetFloat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBool(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int id = Lua.xlua_tointeger(L, 2);
				bool value = animator.GetBool(id);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				bool value2 = animator.GetBool(name);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.GetBool!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBool(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int id = Lua.xlua_tointeger(L, 2);
				bool value = Lua.lua_toboolean(L, 3);
				animator.SetBool(id, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				bool value2 = Lua.lua_toboolean(L, 3);
				animator.SetBool(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.SetBool!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInteger(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int id = Lua.xlua_tointeger(L, 2);
				int integer = animator.GetInteger(id);
				Lua.xlua_pushinteger(L, integer);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				int integer2 = animator.GetInteger(name);
				Lua.xlua_pushinteger(L, integer2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.GetInteger!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetInteger(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int id = Lua.xlua_tointeger(L, 2);
				int value = Lua.xlua_tointeger(L, 3);
				animator.SetInteger(id, value);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				int value2 = Lua.xlua_tointeger(L, 3);
				animator.SetInteger(name, value2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.SetInteger!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTrigger(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int trigger = Lua.xlua_tointeger(L, 2);
				animator.SetTrigger(trigger);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string trigger2 = Lua.lua_tostring(L, 2);
				animator.SetTrigger(trigger2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.SetTrigger!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetTrigger(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int id = Lua.xlua_tointeger(L, 2);
				animator.ResetTrigger(id);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				animator.ResetTrigger(name);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.ResetTrigger!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsParameterControlledByCurve(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int id = Lua.xlua_tointeger(L, 2);
				bool value = animator.IsParameterControlledByCurve(id);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				bool value2 = animator.IsParameterControlledByCurve(name);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.IsParameterControlledByCurve!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIKPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKGoal v);
			Vector3 iKPosition = animator.GetIKPosition(v);
			objectTranslator.PushUnityEngineVector3(L, iKPosition);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIKPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKGoal v);
			objectTranslator.Get(L, 3, out Vector3 val);
			animator.SetIKPosition(v, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIKRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKGoal v);
			Quaternion iKRotation = animator.GetIKRotation(v);
			objectTranslator.PushUnityEngineQuaternion(L, iKRotation);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIKRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKGoal v);
			objectTranslator.Get(L, 3, out Quaternion val);
			animator.SetIKRotation(v, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIKPositionWeight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKGoal v);
			float iKPositionWeight = animator.GetIKPositionWeight(v);
			Lua.lua_pushnumber(L, iKPositionWeight);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIKPositionWeight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKGoal v);
			float value = (float)Lua.lua_tonumber(L, 3);
			animator.SetIKPositionWeight(v, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIKRotationWeight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKGoal v);
			float iKRotationWeight = animator.GetIKRotationWeight(v);
			Lua.lua_pushnumber(L, iKRotationWeight);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIKRotationWeight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKGoal v);
			float value = (float)Lua.lua_tonumber(L, 3);
			animator.SetIKRotationWeight(v, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIKHintPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKHint v);
			Vector3 iKHintPosition = animator.GetIKHintPosition(v);
			objectTranslator.PushUnityEngineVector3(L, iKHintPosition);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIKHintPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKHint v);
			objectTranslator.Get(L, 3, out Vector3 val);
			animator.SetIKHintPosition(v, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIKHintPositionWeight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKHint v);
			float iKHintPositionWeight = animator.GetIKHintPositionWeight(v);
			Lua.lua_pushnumber(L, iKHintPositionWeight);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIKHintPositionWeight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarIKHint v);
			float value = (float)Lua.lua_tonumber(L, 3);
			animator.SetIKHintPositionWeight(v, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLookAtPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			animator.SetLookAtPosition(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLookAtWeight(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float lookAtWeight = (float)Lua.lua_tonumber(L, 2);
				animator.SetLookAtWeight(lookAtWeight);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float weight = (float)Lua.lua_tonumber(L, 2);
				float bodyWeight = (float)Lua.lua_tonumber(L, 3);
				animator.SetLookAtWeight(weight, bodyWeight);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float weight2 = (float)Lua.lua_tonumber(L, 2);
				float bodyWeight2 = (float)Lua.lua_tonumber(L, 3);
				float headWeight = (float)Lua.lua_tonumber(L, 4);
				animator.SetLookAtWeight(weight2, bodyWeight2, headWeight);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float weight3 = (float)Lua.lua_tonumber(L, 2);
				float bodyWeight3 = (float)Lua.lua_tonumber(L, 3);
				float headWeight2 = (float)Lua.lua_tonumber(L, 4);
				float eyesWeight = (float)Lua.lua_tonumber(L, 5);
				animator.SetLookAtWeight(weight3, bodyWeight3, headWeight2, eyesWeight);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				float weight4 = (float)Lua.lua_tonumber(L, 2);
				float bodyWeight4 = (float)Lua.lua_tonumber(L, 3);
				float headWeight3 = (float)Lua.lua_tonumber(L, 4);
				float eyesWeight2 = (float)Lua.lua_tonumber(L, 5);
				float clampWeight = (float)Lua.lua_tonumber(L, 6);
				animator.SetLookAtWeight(weight4, bodyWeight4, headWeight3, eyesWeight2, clampWeight);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.SetLookAtWeight!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBoneLocalRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out HumanBodyBones v);
			objectTranslator.Get(L, 3, out Quaternion val);
			animator.SetBoneLocalRotation(v, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBehaviours(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator obj = (Animator)objectTranslator.FastGetCSObj(L, 1);
			int fullPathHash = Lua.xlua_tointeger(L, 2);
			int layerIndex = Lua.xlua_tointeger(L, 3);
			StateMachineBehaviour[] behaviours = obj.GetBehaviours(fullPathHash, layerIndex);
			objectTranslator.Push(L, behaviours);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLayerName(IntPtr L)
	{
		try
		{
			Animator obj = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int layerIndex = Lua.xlua_tointeger(L, 2);
			string layerName = obj.GetLayerName(layerIndex);
			Lua.lua_pushstring(L, layerName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLayerIndex(IntPtr L)
	{
		try
		{
			Animator obj = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string layerName = Lua.lua_tostring(L, 2);
			int layerIndex = obj.GetLayerIndex(layerName);
			Lua.xlua_pushinteger(L, layerIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLayerWeight(IntPtr L)
	{
		try
		{
			Animator obj = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int layerIndex = Lua.xlua_tointeger(L, 2);
			float layerWeight = obj.GetLayerWeight(layerIndex);
			Lua.lua_pushnumber(L, layerWeight);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLayerWeight(IntPtr L)
	{
		try
		{
			Animator obj = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int layerIndex = Lua.xlua_tointeger(L, 2);
			float weight = (float)Lua.lua_tonumber(L, 3);
			obj.SetLayerWeight(layerIndex, weight);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentAnimatorStateInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator obj = (Animator)objectTranslator.FastGetCSObj(L, 1);
			int layerIndex = Lua.xlua_tointeger(L, 2);
			AnimatorStateInfo currentAnimatorStateInfo = obj.GetCurrentAnimatorStateInfo(layerIndex);
			objectTranslator.Push(L, currentAnimatorStateInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNextAnimatorStateInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator obj = (Animator)objectTranslator.FastGetCSObj(L, 1);
			int layerIndex = Lua.xlua_tointeger(L, 2);
			AnimatorStateInfo nextAnimatorStateInfo = obj.GetNextAnimatorStateInfo(layerIndex);
			objectTranslator.Push(L, nextAnimatorStateInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAnimatorTransitionInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator obj = (Animator)objectTranslator.FastGetCSObj(L, 1);
			int layerIndex = Lua.xlua_tointeger(L, 2);
			AnimatorTransitionInfo animatorTransitionInfo = obj.GetAnimatorTransitionInfo(layerIndex);
			objectTranslator.Push(L, animatorTransitionInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentAnimatorClipInfoCount(IntPtr L)
	{
		try
		{
			Animator obj = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int layerIndex = Lua.xlua_tointeger(L, 2);
			int currentAnimatorClipInfoCount = obj.GetCurrentAnimatorClipInfoCount(layerIndex);
			Lua.xlua_pushinteger(L, currentAnimatorClipInfoCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNextAnimatorClipInfoCount(IntPtr L)
	{
		try
		{
			Animator obj = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int layerIndex = Lua.xlua_tointeger(L, 2);
			int nextAnimatorClipInfoCount = obj.GetNextAnimatorClipInfoCount(layerIndex);
			Lua.xlua_pushinteger(L, nextAnimatorClipInfoCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentAnimatorClipInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int layerIndex = Lua.xlua_tointeger(L, 2);
				AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(layerIndex);
				objectTranslator.Push(L, currentAnimatorClipInfo);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<AnimatorClipInfo>>(L, 3))
			{
				int layerIndex2 = Lua.xlua_tointeger(L, 2);
				List<AnimatorClipInfo> clips = (List<AnimatorClipInfo>)objectTranslator.GetObject(L, 3, typeof(List<AnimatorClipInfo>));
				animator.GetCurrentAnimatorClipInfo(layerIndex2, clips);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.GetCurrentAnimatorClipInfo!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNextAnimatorClipInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int layerIndex = Lua.xlua_tointeger(L, 2);
				AnimatorClipInfo[] nextAnimatorClipInfo = animator.GetNextAnimatorClipInfo(layerIndex);
				objectTranslator.Push(L, nextAnimatorClipInfo);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<AnimatorClipInfo>>(L, 3))
			{
				int layerIndex2 = Lua.xlua_tointeger(L, 2);
				List<AnimatorClipInfo> clips = (List<AnimatorClipInfo>)objectTranslator.GetObject(L, 3, typeof(List<AnimatorClipInfo>));
				animator.GetNextAnimatorClipInfo(layerIndex2, clips);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.GetNextAnimatorClipInfo!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInTransition(IntPtr L)
	{
		try
		{
			Animator obj = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int layerIndex = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsInTransition(layerIndex);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetParameter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator obj = (Animator)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			AnimatorControllerParameter parameter = obj.GetParameter(index);
			objectTranslator.Push(L, parameter);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MatchTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<AvatarTarget>(L, 4) && objectTranslator.Assignable<MatchTargetWeightMask>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				objectTranslator.Get(L, 3, out Quaternion val2);
				objectTranslator.Get(L, 4, out AvatarTarget v);
				objectTranslator.Get(L, 5, out MatchTargetWeightMask v2);
				float startNormalizedTime = (float)Lua.lua_tonumber(L, 6);
				animator.MatchTarget(val, val2, v, v2, startNormalizedTime);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<AvatarTarget>(L, 4) && objectTranslator.Assignable<MatchTargetWeightMask>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Quaternion val4);
				objectTranslator.Get(L, 4, out AvatarTarget v3);
				objectTranslator.Get(L, 5, out MatchTargetWeightMask v4);
				float startNormalizedTime2 = (float)Lua.lua_tonumber(L, 6);
				float targetNormalizedTime = (float)Lua.lua_tonumber(L, 7);
				animator.MatchTarget(val3, val4, v3, v4, startNormalizedTime2, targetNormalizedTime);
				return 0;
			}
			if (num == 8 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Quaternion>(L, 3) && objectTranslator.Assignable<AvatarTarget>(L, 4) && objectTranslator.Assignable<MatchTargetWeightMask>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				objectTranslator.Get(L, 2, out Vector3 val5);
				objectTranslator.Get(L, 3, out Quaternion val6);
				objectTranslator.Get(L, 4, out AvatarTarget v5);
				objectTranslator.Get(L, 5, out MatchTargetWeightMask v6);
				float startNormalizedTime3 = (float)Lua.lua_tonumber(L, 6);
				float targetNormalizedTime2 = (float)Lua.lua_tonumber(L, 7);
				bool completeMatch = Lua.lua_toboolean(L, 8);
				animator.MatchTarget(val5, val6, v5, v6, startNormalizedTime3, targetNormalizedTime2, completeMatch);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.MatchTarget!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InterruptMatchTarget(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				animator.InterruptMatchTarget();
				return 0;
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool completeMatch = Lua.lua_toboolean(L, 2);
					animator.InterruptMatchTarget(completeMatch);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.InterruptMatchTarget!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrossFadeInFixedTime(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int stateHashName = Lua.xlua_tointeger(L, 2);
				float fixedTransitionDuration = (float)Lua.lua_tonumber(L, 3);
				animator.CrossFadeInFixedTime(stateHashName, fixedTransitionDuration);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int stateHashName2 = Lua.xlua_tointeger(L, 2);
				float fixedTransitionDuration2 = (float)Lua.lua_tonumber(L, 3);
				int layer = Lua.xlua_tointeger(L, 4);
				animator.CrossFadeInFixedTime(stateHashName2, fixedTransitionDuration2, layer);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int stateHashName3 = Lua.xlua_tointeger(L, 2);
				float fixedTransitionDuration3 = (float)Lua.lua_tonumber(L, 3);
				int layer2 = Lua.xlua_tointeger(L, 4);
				float fixedTimeOffset = (float)Lua.lua_tonumber(L, 5);
				animator.CrossFadeInFixedTime(stateHashName3, fixedTransitionDuration3, layer2, fixedTimeOffset);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				int stateHashName4 = Lua.xlua_tointeger(L, 2);
				float fixedTransitionDuration4 = (float)Lua.lua_tonumber(L, 3);
				int layer3 = Lua.xlua_tointeger(L, 4);
				float fixedTimeOffset2 = (float)Lua.lua_tonumber(L, 5);
				float normalizedTransitionTime = (float)Lua.lua_tonumber(L, 6);
				animator.CrossFadeInFixedTime(stateHashName4, fixedTransitionDuration4, layer3, fixedTimeOffset2, normalizedTransitionTime);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string stateName = Lua.lua_tostring(L, 2);
				float fixedTransitionDuration5 = (float)Lua.lua_tonumber(L, 3);
				animator.CrossFadeInFixedTime(stateName, fixedTransitionDuration5);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string stateName2 = Lua.lua_tostring(L, 2);
				float fixedTransitionDuration6 = (float)Lua.lua_tonumber(L, 3);
				int layer4 = Lua.xlua_tointeger(L, 4);
				animator.CrossFadeInFixedTime(stateName2, fixedTransitionDuration6, layer4);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string stateName3 = Lua.lua_tostring(L, 2);
				float fixedTransitionDuration7 = (float)Lua.lua_tonumber(L, 3);
				int layer5 = Lua.xlua_tointeger(L, 4);
				float fixedTimeOffset3 = (float)Lua.lua_tonumber(L, 5);
				animator.CrossFadeInFixedTime(stateName3, fixedTransitionDuration7, layer5, fixedTimeOffset3);
				return 0;
			}
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				string stateName4 = Lua.lua_tostring(L, 2);
				float fixedTransitionDuration8 = (float)Lua.lua_tonumber(L, 3);
				int layer6 = Lua.xlua_tointeger(L, 4);
				float fixedTimeOffset4 = (float)Lua.lua_tonumber(L, 5);
				float normalizedTransitionTime2 = (float)Lua.lua_tonumber(L, 6);
				animator.CrossFadeInFixedTime(stateName4, fixedTransitionDuration8, layer6, fixedTimeOffset4, normalizedTransitionTime2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.CrossFadeInFixedTime!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WriteDefaultValues(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).WriteDefaultValues();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrossFade(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int stateHashName = Lua.xlua_tointeger(L, 2);
				float normalizedTransitionDuration = (float)Lua.lua_tonumber(L, 3);
				animator.CrossFade(stateHashName, normalizedTransitionDuration);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int stateHashName2 = Lua.xlua_tointeger(L, 2);
				float normalizedTransitionDuration2 = (float)Lua.lua_tonumber(L, 3);
				int layer = Lua.xlua_tointeger(L, 4);
				animator.CrossFade(stateHashName2, normalizedTransitionDuration2, layer);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int stateHashName3 = Lua.xlua_tointeger(L, 2);
				float normalizedTransitionDuration3 = (float)Lua.lua_tonumber(L, 3);
				int layer2 = Lua.xlua_tointeger(L, 4);
				float normalizedTimeOffset = (float)Lua.lua_tonumber(L, 5);
				animator.CrossFade(stateHashName3, normalizedTransitionDuration3, layer2, normalizedTimeOffset);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				int stateHashName4 = Lua.xlua_tointeger(L, 2);
				float normalizedTransitionDuration4 = (float)Lua.lua_tonumber(L, 3);
				int layer3 = Lua.xlua_tointeger(L, 4);
				float normalizedTimeOffset2 = (float)Lua.lua_tonumber(L, 5);
				float normalizedTransitionTime = (float)Lua.lua_tonumber(L, 6);
				animator.CrossFade(stateHashName4, normalizedTransitionDuration4, layer3, normalizedTimeOffset2, normalizedTransitionTime);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string stateName = Lua.lua_tostring(L, 2);
				float normalizedTransitionDuration5 = (float)Lua.lua_tonumber(L, 3);
				animator.CrossFade(stateName, normalizedTransitionDuration5);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string stateName2 = Lua.lua_tostring(L, 2);
				float normalizedTransitionDuration6 = (float)Lua.lua_tonumber(L, 3);
				int layer4 = Lua.xlua_tointeger(L, 4);
				animator.CrossFade(stateName2, normalizedTransitionDuration6, layer4);
				return 0;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string stateName3 = Lua.lua_tostring(L, 2);
				float normalizedTransitionDuration7 = (float)Lua.lua_tonumber(L, 3);
				int layer5 = Lua.xlua_tointeger(L, 4);
				float normalizedTimeOffset3 = (float)Lua.lua_tonumber(L, 5);
				animator.CrossFade(stateName3, normalizedTransitionDuration7, layer5, normalizedTimeOffset3);
				return 0;
			}
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				string stateName4 = Lua.lua_tostring(L, 2);
				float normalizedTransitionDuration8 = (float)Lua.lua_tonumber(L, 3);
				int layer6 = Lua.xlua_tointeger(L, 4);
				float normalizedTimeOffset4 = (float)Lua.lua_tonumber(L, 5);
				float normalizedTransitionTime2 = (float)Lua.lua_tonumber(L, 6);
				animator.CrossFade(stateName4, normalizedTransitionDuration8, layer6, normalizedTimeOffset4, normalizedTransitionTime2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.CrossFade!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayInFixedTime(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int stateNameHash = Lua.xlua_tointeger(L, 2);
				animator.PlayInFixedTime(stateNameHash);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int stateNameHash2 = Lua.xlua_tointeger(L, 2);
				int layer = Lua.xlua_tointeger(L, 3);
				animator.PlayInFixedTime(stateNameHash2, layer);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int stateNameHash3 = Lua.xlua_tointeger(L, 2);
				int layer2 = Lua.xlua_tointeger(L, 3);
				float fixedTime = (float)Lua.lua_tonumber(L, 4);
				animator.PlayInFixedTime(stateNameHash3, layer2, fixedTime);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string stateName = Lua.lua_tostring(L, 2);
				animator.PlayInFixedTime(stateName);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string stateName2 = Lua.lua_tostring(L, 2);
				int layer3 = Lua.xlua_tointeger(L, 3);
				animator.PlayInFixedTime(stateName2, layer3);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string stateName3 = Lua.lua_tostring(L, 2);
				int layer4 = Lua.xlua_tointeger(L, 3);
				float fixedTime2 = (float)Lua.lua_tonumber(L, 4);
				animator.PlayInFixedTime(stateName3, layer4, fixedTime2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.PlayInFixedTime!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Play(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int stateNameHash = Lua.xlua_tointeger(L, 2);
				animator.Play(stateNameHash);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int stateNameHash2 = Lua.xlua_tointeger(L, 2);
				int layer = Lua.xlua_tointeger(L, 3);
				animator.Play(stateNameHash2, layer);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int stateNameHash3 = Lua.xlua_tointeger(L, 2);
				int layer2 = Lua.xlua_tointeger(L, 3);
				float normalizedTime = (float)Lua.lua_tonumber(L, 4);
				animator.Play(stateNameHash3, layer2, normalizedTime);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string stateName = Lua.lua_tostring(L, 2);
				animator.Play(stateName);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				string stateName2 = Lua.lua_tostring(L, 2);
				int layer3 = Lua.xlua_tointeger(L, 3);
				animator.Play(stateName2, layer3);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string stateName3 = Lua.lua_tostring(L, 2);
				int layer4 = Lua.xlua_tointeger(L, 3);
				float normalizedTime2 = (float)Lua.lua_tonumber(L, 4);
				animator.Play(stateName3, layer4, normalizedTime2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Animator.Play!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AvatarTarget v);
			float targetNormalizedTime = (float)Lua.lua_tonumber(L, 3);
			animator.SetTarget(v, targetNormalizedTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBoneTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out HumanBodyBones v);
			Transform boneTransform = animator.GetBoneTransform(v);
			objectTranslator.Push(L, boneTransform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartPlayback(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StartPlayback();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopPlayback(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopPlayback();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartRecording(IntPtr L)
	{
		try
		{
			Animator obj = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int frameCount = Lua.xlua_tointeger(L, 2);
			obj.StartRecording(frameCount);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopRecording(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopRecording();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasState(IntPtr L)
	{
		try
		{
			Animator obj = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int layerIndex = Lua.xlua_tointeger(L, 2);
			int stateID = Lua.xlua_tointeger(L, 3);
			bool value = obj.HasState(layerIndex, stateID);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StringToHash_xlua_st_(IntPtr L)
	{
		try
		{
			int value = Animator.StringToHash(Lua.lua_tostring(L, 1));
			Lua.xlua_pushinteger(L, value);
			return 1;
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
			Animator obj = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			obj.Update(deltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Rebind(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Rebind();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ApplyBuiltinRootMotion(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ApplyBuiltinRootMotion();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayId(IntPtr L)
	{
		try
		{
			Animator ani = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int stateNameToId = Lua.xlua_tointeger(L, 2);
			int layerIdx = Lua.xlua_tointeger(L, 3);
			float normalizedTime = (float)Lua.lua_tonumber(L, 4);
			ani.PlayId(stateNameToId, layerIdx, normalizedTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTriggerId(IntPtr L)
	{
		try
		{
			Animator ani = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int triggerNameToId = Lua.xlua_tointeger(L, 2);
			ani.SetTriggerId(triggerNameToId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isOptimizable(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.isOptimizable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isHuman(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.isHuman);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasRootMotion(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.hasRootMotion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_humanScale(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animator.humanScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isInitialized(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.isInitialized);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_deltaPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, animator.deltaPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_deltaRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineQuaternion(L, animator.deltaRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_velocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, animator.velocity);
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
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, animator.angularVelocity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rootPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, animator.rootPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rootRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineQuaternion(L, animator.rootRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_applyRootMotion(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.applyRootMotion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_updateMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animator.updateMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasTransformHierarchy(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.hasTransformHierarchy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gravityWeight(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animator.gravityWeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bodyPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, animator.bodyPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bodyRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineQuaternion(L, animator.bodyRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stabilizeFeet(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.stabilizeFeet);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_layerCount(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, animator.layerCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_parameters(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animator.parameters);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_parameterCount(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, animator.parameterCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_feetPivotActive(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animator.feetPivotActive);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pivotWeight(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animator.pivotWeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pivotPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, animator.pivotPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isMatchingTarget(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.isMatchingTarget);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_speed(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animator.speed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, animator.targetPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineQuaternion(L, animator.targetRotation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cullingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineAnimatorCullingMode(L, animator.cullingMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playbackTime(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animator.playbackTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_recorderStartTime(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animator.recorderStartTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_recorderStopTime(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animator.recorderStopTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_recorderMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animator.recorderMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_runtimeAnimatorController(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animator.runtimeAnimatorController);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasBoundPlayables(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.hasBoundPlayables);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_avatar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animator.avatar);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playableGraph(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, animator.playableGraph);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_layersAffectMassCenter(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.layersAffectMassCenter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_leftFeetBottomHeight(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animator.leftFeetBottomHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rightFeetBottomHeight(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, animator.rightFeetBottomHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_logWarnings(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.logWarnings);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fireEvents(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.fireEvents);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_keepAnimatorControllerStateOnDisable(IntPtr L)
	{
		try
		{
			Animator animator = (Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, animator.keepAnimatorControllerStateOnDisable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rootPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			animator.rootPosition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rootRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			animator.rootRotation = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_applyRootMotion(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).applyRootMotion = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_updateMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AnimatorUpdateMode v);
			animator.updateMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bodyPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			animator.bodyPosition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bodyRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			animator.bodyRotation = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stabilizeFeet(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).stabilizeFeet = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_feetPivotActive(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).feetPivotActive = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_speed(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).speed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cullingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animator animator = (Animator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AnimatorCullingMode val);
			animator.cullingMode = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playbackTime(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playbackTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_recorderStartTime(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).recorderStartTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_recorderStopTime(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).recorderStopTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_runtimeAnimatorController(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Animator)objectTranslator.FastGetCSObj(L, 1)).runtimeAnimatorController = (RuntimeAnimatorController)objectTranslator.GetObject(L, 2, typeof(RuntimeAnimatorController));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_avatar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Animator)objectTranslator.FastGetCSObj(L, 1)).avatar = (Avatar)objectTranslator.GetObject(L, 2, typeof(Avatar));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_layersAffectMassCenter(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).layersAffectMassCenter = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_logWarnings(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).logWarnings = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fireEvents(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fireEvents = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_keepAnimatorControllerStateOnDisable(IntPtr L)
	{
		try
		{
			((Animator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).keepAnimatorControllerStateOnDisable = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
