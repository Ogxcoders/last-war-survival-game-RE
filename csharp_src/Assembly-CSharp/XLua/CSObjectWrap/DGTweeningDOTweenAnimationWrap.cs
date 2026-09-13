using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DGTweeningDOTweenAnimationWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DOTweenAnimation);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 24, 36, 36);
		Utils.RegisterFunc(L, -3, "CreateTween", _m_CreateTween);
		Utils.RegisterFunc(L, -3, "DOPlay", _m_DOPlay);
		Utils.RegisterFunc(L, -3, "DOPlayBackwards", _m_DOPlayBackwards);
		Utils.RegisterFunc(L, -3, "DOPlayForward", _m_DOPlayForward);
		Utils.RegisterFunc(L, -3, "DOPause", _m_DOPause);
		Utils.RegisterFunc(L, -3, "DOTogglePause", _m_DOTogglePause);
		Utils.RegisterFunc(L, -3, "DORewind", _m_DORewind);
		Utils.RegisterFunc(L, -3, "DORestart", _m_DORestart);
		Utils.RegisterFunc(L, -3, "DOComplete", _m_DOComplete);
		Utils.RegisterFunc(L, -3, "DOKill", _m_DOKill);
		Utils.RegisterFunc(L, -3, "DOPlayById", _m_DOPlayById);
		Utils.RegisterFunc(L, -3, "DOPlayAllById", _m_DOPlayAllById);
		Utils.RegisterFunc(L, -3, "DOPauseAllById", _m_DOPauseAllById);
		Utils.RegisterFunc(L, -3, "DOPlayBackwardsById", _m_DOPlayBackwardsById);
		Utils.RegisterFunc(L, -3, "DOPlayBackwardsAllById", _m_DOPlayBackwardsAllById);
		Utils.RegisterFunc(L, -3, "DOPlayForwardById", _m_DOPlayForwardById);
		Utils.RegisterFunc(L, -3, "DOPlayForwardAllById", _m_DOPlayForwardAllById);
		Utils.RegisterFunc(L, -3, "DOPlayNext", _m_DOPlayNext);
		Utils.RegisterFunc(L, -3, "DORewindAndPlayNext", _m_DORewindAndPlayNext);
		Utils.RegisterFunc(L, -3, "DORewindAllById", _m_DORewindAllById);
		Utils.RegisterFunc(L, -3, "DORestartById", _m_DORestartById);
		Utils.RegisterFunc(L, -3, "DORestartAllById", _m_DORestartAllById);
		Utils.RegisterFunc(L, -3, "GetTweens", _m_GetTweens);
		Utils.RegisterFunc(L, -3, "CreateEditorPreview", _m_CreateEditorPreview);
		Utils.RegisterFunc(L, -2, "targetIsSelf", _g_get_targetIsSelf);
		Utils.RegisterFunc(L, -2, "targetGO", _g_get_targetGO);
		Utils.RegisterFunc(L, -2, "tweenTargetIsTargetGO", _g_get_tweenTargetIsTargetGO);
		Utils.RegisterFunc(L, -2, "delay", _g_get_delay);
		Utils.RegisterFunc(L, -2, "duration", _g_get_duration);
		Utils.RegisterFunc(L, -2, "easeType", _g_get_easeType);
		Utils.RegisterFunc(L, -2, "easeCurve", _g_get_easeCurve);
		Utils.RegisterFunc(L, -2, "loopType", _g_get_loopType);
		Utils.RegisterFunc(L, -2, "loops", _g_get_loops);
		Utils.RegisterFunc(L, -2, "id", _g_get_id);
		Utils.RegisterFunc(L, -2, "isRelative", _g_get_isRelative);
		Utils.RegisterFunc(L, -2, "isFrom", _g_get_isFrom);
		Utils.RegisterFunc(L, -2, "isIndependentUpdate", _g_get_isIndependentUpdate);
		Utils.RegisterFunc(L, -2, "autoKill", _g_get_autoKill);
		Utils.RegisterFunc(L, -2, "isActive", _g_get_isActive);
		Utils.RegisterFunc(L, -2, "isValid", _g_get_isValid);
		Utils.RegisterFunc(L, -2, "target", _g_get_target);
		Utils.RegisterFunc(L, -2, "animationType", _g_get_animationType);
		Utils.RegisterFunc(L, -2, "targetType", _g_get_targetType);
		Utils.RegisterFunc(L, -2, "forcedTargetType", _g_get_forcedTargetType);
		Utils.RegisterFunc(L, -2, "autoPlay", _g_get_autoPlay);
		Utils.RegisterFunc(L, -2, "useTargetAsV3", _g_get_useTargetAsV3);
		Utils.RegisterFunc(L, -2, "endValueFloat", _g_get_endValueFloat);
		Utils.RegisterFunc(L, -2, "endValueV3", _g_get_endValueV3);
		Utils.RegisterFunc(L, -2, "endValueV2", _g_get_endValueV2);
		Utils.RegisterFunc(L, -2, "endValueColor", _g_get_endValueColor);
		Utils.RegisterFunc(L, -2, "endValueString", _g_get_endValueString);
		Utils.RegisterFunc(L, -2, "endValueRect", _g_get_endValueRect);
		Utils.RegisterFunc(L, -2, "endValueTransform", _g_get_endValueTransform);
		Utils.RegisterFunc(L, -2, "optionalBool0", _g_get_optionalBool0);
		Utils.RegisterFunc(L, -2, "optionalFloat0", _g_get_optionalFloat0);
		Utils.RegisterFunc(L, -2, "optionalInt0", _g_get_optionalInt0);
		Utils.RegisterFunc(L, -2, "optionalRotationMode", _g_get_optionalRotationMode);
		Utils.RegisterFunc(L, -2, "optionalScrambleMode", _g_get_optionalScrambleMode);
		Utils.RegisterFunc(L, -2, "optionalString", _g_get_optionalString);
		Utils.RegisterFunc(L, -2, "isAutoMirrorUIProcessedInArabic", _g_get_isAutoMirrorUIProcessedInArabic);
		Utils.RegisterFunc(L, -1, "targetIsSelf", _s_set_targetIsSelf);
		Utils.RegisterFunc(L, -1, "targetGO", _s_set_targetGO);
		Utils.RegisterFunc(L, -1, "tweenTargetIsTargetGO", _s_set_tweenTargetIsTargetGO);
		Utils.RegisterFunc(L, -1, "delay", _s_set_delay);
		Utils.RegisterFunc(L, -1, "duration", _s_set_duration);
		Utils.RegisterFunc(L, -1, "easeType", _s_set_easeType);
		Utils.RegisterFunc(L, -1, "easeCurve", _s_set_easeCurve);
		Utils.RegisterFunc(L, -1, "loopType", _s_set_loopType);
		Utils.RegisterFunc(L, -1, "loops", _s_set_loops);
		Utils.RegisterFunc(L, -1, "id", _s_set_id);
		Utils.RegisterFunc(L, -1, "isRelative", _s_set_isRelative);
		Utils.RegisterFunc(L, -1, "isFrom", _s_set_isFrom);
		Utils.RegisterFunc(L, -1, "isIndependentUpdate", _s_set_isIndependentUpdate);
		Utils.RegisterFunc(L, -1, "autoKill", _s_set_autoKill);
		Utils.RegisterFunc(L, -1, "isActive", _s_set_isActive);
		Utils.RegisterFunc(L, -1, "isValid", _s_set_isValid);
		Utils.RegisterFunc(L, -1, "target", _s_set_target);
		Utils.RegisterFunc(L, -1, "animationType", _s_set_animationType);
		Utils.RegisterFunc(L, -1, "targetType", _s_set_targetType);
		Utils.RegisterFunc(L, -1, "forcedTargetType", _s_set_forcedTargetType);
		Utils.RegisterFunc(L, -1, "autoPlay", _s_set_autoPlay);
		Utils.RegisterFunc(L, -1, "useTargetAsV3", _s_set_useTargetAsV3);
		Utils.RegisterFunc(L, -1, "endValueFloat", _s_set_endValueFloat);
		Utils.RegisterFunc(L, -1, "endValueV3", _s_set_endValueV3);
		Utils.RegisterFunc(L, -1, "endValueV2", _s_set_endValueV2);
		Utils.RegisterFunc(L, -1, "endValueColor", _s_set_endValueColor);
		Utils.RegisterFunc(L, -1, "endValueString", _s_set_endValueString);
		Utils.RegisterFunc(L, -1, "endValueRect", _s_set_endValueRect);
		Utils.RegisterFunc(L, -1, "endValueTransform", _s_set_endValueTransform);
		Utils.RegisterFunc(L, -1, "optionalBool0", _s_set_optionalBool0);
		Utils.RegisterFunc(L, -1, "optionalFloat0", _s_set_optionalFloat0);
		Utils.RegisterFunc(L, -1, "optionalInt0", _s_set_optionalInt0);
		Utils.RegisterFunc(L, -1, "optionalRotationMode", _s_set_optionalRotationMode);
		Utils.RegisterFunc(L, -1, "optionalScrambleMode", _s_set_optionalScrambleMode);
		Utils.RegisterFunc(L, -1, "optionalString", _s_set_optionalString);
		Utils.RegisterFunc(L, -1, "isAutoMirrorUIProcessedInArabic", _s_set_isAutoMirrorUIProcessedInArabic);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "TypeToDOTargetType", _m_TypeToDOTargetType_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnReset", _e_OnReset);
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
				DOTweenAnimation o = new DOTweenAnimation();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTweenAnimation constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateTween(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateTween();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlay(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPlay();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayBackwards(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPlayBackwards();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayForward(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPlayForward();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPause(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPause();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOTogglePause(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOTogglePause();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORewind(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DORewind();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORestart(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				dOTweenAnimation.DORestart();
				return 0;
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool fromHere = Lua.lua_toboolean(L, 2);
					dOTweenAnimation.DORestart(fromHere);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTweenAnimation.DORestart!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOComplete(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOComplete();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOKill(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOKill();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayById(IntPtr L)
	{
		try
		{
			DOTweenAnimation obj = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string id = Lua.lua_tostring(L, 2);
			obj.DOPlayById(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayAllById(IntPtr L)
	{
		try
		{
			DOTweenAnimation obj = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string id = Lua.lua_tostring(L, 2);
			obj.DOPlayAllById(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPauseAllById(IntPtr L)
	{
		try
		{
			DOTweenAnimation obj = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string id = Lua.lua_tostring(L, 2);
			obj.DOPauseAllById(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayBackwardsById(IntPtr L)
	{
		try
		{
			DOTweenAnimation obj = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string id = Lua.lua_tostring(L, 2);
			obj.DOPlayBackwardsById(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayBackwardsAllById(IntPtr L)
	{
		try
		{
			DOTweenAnimation obj = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string id = Lua.lua_tostring(L, 2);
			obj.DOPlayBackwardsAllById(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayForwardById(IntPtr L)
	{
		try
		{
			DOTweenAnimation obj = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string id = Lua.lua_tostring(L, 2);
			obj.DOPlayForwardById(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayForwardAllById(IntPtr L)
	{
		try
		{
			DOTweenAnimation obj = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string id = Lua.lua_tostring(L, 2);
			obj.DOPlayForwardAllById(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPlayNext(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DOPlayNext();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORewindAndPlayNext(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DORewindAndPlayNext();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORewindAllById(IntPtr L)
	{
		try
		{
			DOTweenAnimation obj = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string id = Lua.lua_tostring(L, 2);
			obj.DORewindAllById(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORestartById(IntPtr L)
	{
		try
		{
			DOTweenAnimation obj = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string id = Lua.lua_tostring(L, 2);
			obj.DORestartById(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORestartAllById(IntPtr L)
	{
		try
		{
			DOTweenAnimation obj = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string id = Lua.lua_tostring(L, 2);
			obj.DORestartAllById(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTweens(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<Tween> tweens = ((DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1)).GetTweens();
			objectTranslator.Push(L, tweens);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TypeToDOTargetType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation.TargetType val = DOTweenAnimation.TypeToDOTargetType((Type)objectTranslator.GetObject(L, 1, typeof(Type)));
			objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateEditorPreview(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Tween o = ((DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1)).CreateEditorPreview();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetIsSelf(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.targetIsSelf);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetGO(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dOTweenAnimation.targetGO);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tweenTargetIsTargetGO(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.tweenTargetIsTargetGO);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_delay(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dOTweenAnimation.delay);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_duration(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dOTweenAnimation.duration);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_easeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushDGTweeningEase(L, dOTweenAnimation.easeType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_easeCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dOTweenAnimation.easeCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loopType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dOTweenAnimation.loopType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loops(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, dOTweenAnimation.loops);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_id(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, dOTweenAnimation.id);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isRelative(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.isRelative);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isFrom(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.isFrom);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isIndependentUpdate(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.isIndependentUpdate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autoKill(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.autoKill);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isActive(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.isActive);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isValid(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.isValid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dOTweenAnimation.target);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animationType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushDGTweeningDOTweenAnimationAnimationType(L, dOTweenAnimation.animationType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, dOTweenAnimation.targetType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_forcedTargetType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushDGTweeningDOTweenAnimationTargetType(L, dOTweenAnimation.forcedTargetType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autoPlay(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.autoPlay);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useTargetAsV3(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.useTargetAsV3);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endValueFloat(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dOTweenAnimation.endValueFloat);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endValueV3(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, dOTweenAnimation.endValueV3);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endValueV2(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, dOTweenAnimation.endValueV2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endValueColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, dOTweenAnimation.endValueColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endValueString(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, dOTweenAnimation.endValueString);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endValueRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dOTweenAnimation.endValueRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endValueTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dOTweenAnimation.endValueTransform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_optionalBool0(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.optionalBool0);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_optionalFloat0(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dOTweenAnimation.optionalFloat0);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_optionalInt0(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, dOTweenAnimation.optionalInt0);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_optionalRotationMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dOTweenAnimation.optionalRotationMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_optionalScrambleMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dOTweenAnimation.optionalScrambleMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_optionalString(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, dOTweenAnimation.optionalString);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isAutoMirrorUIProcessedInArabic(IntPtr L)
	{
		try
		{
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dOTweenAnimation.isAutoMirrorUIProcessedInArabic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetIsSelf(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetIsSelf = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetGO(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1)).targetGO = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tweenTargetIsTargetGO(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).tweenTargetIsTargetGO = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_delay(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).delay = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_duration(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).duration = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_easeType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Ease val);
			dOTweenAnimation.easeType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_easeCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1)).easeCurve = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loopType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LoopType v);
			dOTweenAnimation.loopType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loops(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loops = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_id(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).id = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isRelative(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isRelative = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isFrom(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isFrom = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isIndependentUpdate(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isIndependentUpdate = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autoKill(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).autoKill = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isActive(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isActive = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isValid(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isValid = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1)).target = (Component)objectTranslator.GetObject(L, 2, typeof(Component));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animationType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out DOTweenAnimation.AnimationType val);
			dOTweenAnimation.animationType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out DOTweenAnimation.TargetType val);
			dOTweenAnimation.targetType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_forcedTargetType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out DOTweenAnimation.TargetType val);
			dOTweenAnimation.forcedTargetType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autoPlay(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).autoPlay = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useTargetAsV3(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useTargetAsV3 = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endValueFloat(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).endValueFloat = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endValueV3(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			dOTweenAnimation.endValueV3 = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endValueV2(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			dOTweenAnimation.endValueV2 = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endValueColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			dOTweenAnimation.endValueColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endValueString(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).endValueString = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endValueRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			dOTweenAnimation.endValueRect = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endValueTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1)).endValueTransform = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_optionalBool0(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).optionalBool0 = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_optionalFloat0(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).optionalFloat0 = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_optionalInt0(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).optionalInt0 = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_optionalRotationMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RotateMode v);
			dOTweenAnimation.optionalRotationMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_optionalScrambleMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DOTweenAnimation dOTweenAnimation = (DOTweenAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScrambleMode v);
			dOTweenAnimation.optionalScrambleMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_optionalString(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).optionalString = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isAutoMirrorUIProcessedInArabic(IntPtr L)
	{
		try
		{
			((DOTweenAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isAutoMirrorUIProcessedInArabic = Lua.lua_toboolean(L, 2);
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
			Action<DOTweenAnimation> action = objectTranslator.GetDelegate<Action<DOTweenAnimation>>(L, 2);
			if (action == null)
			{
				return Lua.luaL_error(L, "#2 need System.Action<DG.Tweening.DOTweenAnimation>!");
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "+"))
			{
				DOTweenAnimation.OnReset += action;
				return 0;
			}
			if (num == 2 && Lua.xlua_is_eq_str(L, 1, "-"))
			{
				DOTweenAnimation.OnReset -= action;
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.DOTweenAnimation.OnReset!");
	}
}
