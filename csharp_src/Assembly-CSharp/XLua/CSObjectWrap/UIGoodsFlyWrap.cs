using System;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIGoodsFlyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIGoodsFly);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 14, 14);
		Utils.RegisterFunc(L, -3, "DoAnimForLua", _m_DoAnimForLua);
		Utils.RegisterFunc(L, -3, "DoAnim", _m_DoAnim);
		Utils.RegisterFunc(L, -3, "DoAnimBox", _m_DoAnimBox);
		Utils.RegisterFunc(L, -3, "DoAnimWithoutLogic", _m_DoAnimWithoutLogic);
		Utils.RegisterFunc(L, -3, "DoParabolaAnim", _m_DoParabolaAnim);
		Utils.RegisterFunc(L, -3, "DoParabolaAnimLocal", _m_DoParabolaAnimLocal);
		Utils.RegisterFunc(L, -2, "isRun", _g_get_isRun);
		Utils.RegisterFunc(L, -2, "targetPos", _g_get_targetPos);
		Utils.RegisterFunc(L, -2, "minSize", _g_get_minSize);
		Utils.RegisterFunc(L, -2, "maxSize", _g_get_maxSize);
		Utils.RegisterFunc(L, -2, "isReset", _g_get_isReset);
		Utils.RegisterFunc(L, -2, "oldPos", _g_get_oldPos);
		Utils.RegisterFunc(L, -2, "moveTime", _g_get_moveTime);
		Utils.RegisterFunc(L, -2, "firstCurve", _g_get_firstCurve);
		Utils.RegisterFunc(L, -2, "secondCurve", _g_get_secondCurve);
		Utils.RegisterFunc(L, -2, "thirdCurve", _g_get_thirdCurve);
		Utils.RegisterFunc(L, -2, "controlPointOffset", _g_get_controlPointOffset);
		Utils.RegisterFunc(L, -2, "img", _g_get_img);
		Utils.RegisterFunc(L, -2, "txt", _g_get_txt);
		Utils.RegisterFunc(L, -2, "middleValue", _g_get_middleValue);
		Utils.RegisterFunc(L, -1, "isRun", _s_set_isRun);
		Utils.RegisterFunc(L, -1, "targetPos", _s_set_targetPos);
		Utils.RegisterFunc(L, -1, "minSize", _s_set_minSize);
		Utils.RegisterFunc(L, -1, "maxSize", _s_set_maxSize);
		Utils.RegisterFunc(L, -1, "isReset", _s_set_isReset);
		Utils.RegisterFunc(L, -1, "oldPos", _s_set_oldPos);
		Utils.RegisterFunc(L, -1, "moveTime", _s_set_moveTime);
		Utils.RegisterFunc(L, -1, "firstCurve", _s_set_firstCurve);
		Utils.RegisterFunc(L, -1, "secondCurve", _s_set_secondCurve);
		Utils.RegisterFunc(L, -1, "thirdCurve", _s_set_thirdCurve);
		Utils.RegisterFunc(L, -1, "controlPointOffset", _s_set_controlPointOffset);
		Utils.RegisterFunc(L, -1, "img", _s_set_img);
		Utils.RegisterFunc(L, -1, "txt", _s_set_txt);
		Utils.RegisterFunc(L, -1, "middleValue", _s_set_middleValue);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "Bezier2", _m_Bezier2_xlua_st_);
		Utils.RegisterFunc(L, -4, "Bezier3", _m_Bezier3_xlua_st_);
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
				UIGoodsFly o = new UIGoodsFly();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIGoodsFly constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoAnimForLua(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			float minRange = (float)Lua.lua_tonumber(L, 2);
			float maxRange = (float)Lua.lua_tonumber(L, 3);
			int rewardType = Lua.xlua_tointeger(L, 4);
			string pic = Lua.lua_tostring(L, 5);
			int num = Lua.xlua_tointeger(L, 6);
			objectTranslator.Get(L, 7, out Vector3 val);
			Action onComplete = objectTranslator.GetDelegate<Action>(L, 8);
			bool isOnlyDisperse = Lua.lua_toboolean(L, 9);
			uIGoodsFly.DoAnimForLua(minRange, maxRange, rewardType, pic, num, val, onComplete, isOnlyDisperse);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoAnim(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && objectTranslator.Assignable<Vector3>(L, 5) && objectTranslator.Assignable<Action>(L, 6))
			{
				float minRange = (float)Lua.lua_tonumber(L, 2);
				float maxRange = (float)Lua.lua_tonumber(L, 3);
				objectTranslator.Get(L, 4, out Vector3 val);
				objectTranslator.Get(L, 5, out Vector3 val2);
				Action onComplete = objectTranslator.GetDelegate<Action>(L, 6);
				uIGoodsFly.DoAnim(minRange, maxRange, val, val2, onComplete);
				return 0;
			}
			if (num == 8 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector3>(L, 6) && objectTranslator.Assignable<Action>(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				float minRange2 = (float)Lua.lua_tonumber(L, 2);
				float maxRange2 = (float)Lua.lua_tonumber(L, 3);
				int rewardType = Lua.xlua_tointeger(L, 4);
				string pic = Lua.lua_tostring(L, 5);
				objectTranslator.Get(L, 6, out Vector3 val3);
				Action onComplete2 = objectTranslator.GetDelegate<Action>(L, 7);
				bool isOnlyDisperse = Lua.lua_toboolean(L, 8);
				uIGoodsFly.DoAnim(minRange2, maxRange2, rewardType, pic, val3, onComplete2, isOnlyDisperse);
				return 0;
			}
			if (num == 9 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Vector3>(L, 7) && objectTranslator.Assignable<Action>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				float minRange3 = (float)Lua.lua_tonumber(L, 2);
				float maxRange3 = (float)Lua.lua_tonumber(L, 3);
				int rewardType2 = Lua.xlua_tointeger(L, 4);
				string pic2 = Lua.lua_tostring(L, 5);
				int num2 = Lua.xlua_tointeger(L, 6);
				objectTranslator.Get(L, 7, out Vector3 val4);
				Action onComplete3 = objectTranslator.GetDelegate<Action>(L, 8);
				bool isOnlyDisperse2 = Lua.lua_toboolean(L, 9);
				uIGoodsFly.DoAnim(minRange3, maxRange3, rewardType2, pic2, num2, val4, onComplete3, isOnlyDisperse2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIGoodsFly.DoAnim!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoAnimBox(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			float minRange = (float)Lua.lua_tonumber(L, 2);
			float maxRange = (float)Lua.lua_tonumber(L, 3);
			objectTranslator.Get(L, 4, out Vector3 val);
			objectTranslator.Get(L, 5, out Vector3 val2);
			Action onComplete = objectTranslator.GetDelegate<Action>(L, 6);
			uIGoodsFly.DoAnimBox(minRange, maxRange, val, val2, onComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoAnimWithoutLogic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			float minRange = (float)Lua.lua_tonumber(L, 2);
			float maxRange = (float)Lua.lua_tonumber(L, 3);
			string pic = Lua.lua_tostring(L, 4);
			objectTranslator.Get(L, 5, out Vector3 val);
			Action onComplete = objectTranslator.GetDelegate<Action>(L, 6);
			float flyTime = (float)Lua.lua_tonumber(L, 7);
			float flyTime2 = (float)Lua.lua_tonumber(L, 8);
			uIGoodsFly.DoAnimWithoutLogic(minRange, maxRange, pic, val, onComplete, flyTime, flyTime2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoParabolaAnim(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector3 val2);
			Action onComplete = objectTranslator.GetDelegate<Action>(L, 4);
			uIGoodsFly.DoParabolaAnim(val, val2, onComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoParabolaAnimLocal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out Vector3 val2);
			Action onComplete = objectTranslator.GetDelegate<Action>(L, 4);
			uIGoodsFly.DoParabolaAnimLocal(val, val2, onComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Bezier2_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			objectTranslator.Get(L, 3, out Vector3 val3);
			Vector3 val4 = UIGoodsFly.Bezier2(t: (float)Lua.lua_tonumber(L, 4), startPos: val, controlPos: val2, endPos: val3);
			objectTranslator.PushUnityEngineVector3(L, val4);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Bezier3_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			objectTranslator.Get(L, 3, out Vector3 val3);
			objectTranslator.Get(L, 4, out Vector3 val4);
			Vector3 val5 = UIGoodsFly.Bezier3(t: (float)Lua.lua_tonumber(L, 5), startPos: val, controlPos1: val2, controlPos2: val3, endPos: val4);
			objectTranslator.PushUnityEngineVector3(L, val5);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isRun(IntPtr L)
	{
		try
		{
			UIGoodsFly uIGoodsFly = (UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, uIGoodsFly.isRun);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, uIGoodsFly.targetPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minSize(IntPtr L)
	{
		try
		{
			UIGoodsFly uIGoodsFly = (UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, uIGoodsFly.minSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxSize(IntPtr L)
	{
		try
		{
			UIGoodsFly uIGoodsFly = (UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, uIGoodsFly.maxSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isReset(IntPtr L)
	{
		try
		{
			UIGoodsFly uIGoodsFly = (UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, uIGoodsFly.isReset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_oldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, uIGoodsFly.oldPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_moveTime(IntPtr L)
	{
		try
		{
			UIGoodsFly uIGoodsFly = (UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, uIGoodsFly.moveTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_firstCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIGoodsFly.firstCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_secondCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIGoodsFly.secondCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_thirdCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIGoodsFly.thirdCurve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_controlPointOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, uIGoodsFly.controlPointOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_img(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIGoodsFly.img);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_txt(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uIGoodsFly.txt);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_middleValue(IntPtr L)
	{
		try
		{
			UIGoodsFly uIGoodsFly = (UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, uIGoodsFly.middleValue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isRun(IntPtr L)
	{
		try
		{
			((UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isRun = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			uIGoodsFly.targetPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_minSize(IntPtr L)
	{
		try
		{
			((UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).minSize = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxSize(IntPtr L)
	{
		try
		{
			((UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxSize = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isReset(IntPtr L)
	{
		try
		{
			((UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isReset = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_oldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			uIGoodsFly.oldPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_moveTime(IntPtr L)
	{
		try
		{
			((UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).moveTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_firstCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIGoodsFly)objectTranslator.FastGetCSObj(L, 1)).firstCurve = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_secondCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIGoodsFly)objectTranslator.FastGetCSObj(L, 1)).secondCurve = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_thirdCurve(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIGoodsFly)objectTranslator.FastGetCSObj(L, 1)).thirdCurve = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_controlPointOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGoodsFly uIGoodsFly = (UIGoodsFly)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			uIGoodsFly.controlPointOffset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_img(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIGoodsFly)objectTranslator.FastGetCSObj(L, 1)).img = (Image)objectTranslator.GetObject(L, 2, typeof(Image));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_txt(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UIGoodsFly)objectTranslator.FastGetCSObj(L, 1)).txt = (Text)objectTranslator.GetObject(L, 2, typeof(Text));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_middleValue(IntPtr L)
	{
		try
		{
			((UIGoodsFly)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).middleValue = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
