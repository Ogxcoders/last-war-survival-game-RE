using System;
using DG.Tweening;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DGTweeningEaseFactoryWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(EaseFactory);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "StopMotion", _m_StopMotion_xlua_st_);
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
				EaseFactory o = new EaseFactory();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.EaseFactory constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopMotion_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Ease?>(L, 2))
			{
				int motionFps = Lua.xlua_tointeger(L, 1);
				objectTranslator.Get(L, 2, out Ease? v);
				EaseFunction o = EaseFactory.StopMotion(motionFps, v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				EaseFunction o2 = EaseFactory.StopMotion(Lua.xlua_tointeger(L, 1));
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<AnimationCurve>(L, 2))
			{
				int motionFps2 = Lua.xlua_tointeger(L, 1);
				AnimationCurve animCurve = (AnimationCurve)objectTranslator.GetObject(L, 2, typeof(AnimationCurve));
				EaseFunction o3 = EaseFactory.StopMotion(motionFps2, animCurve);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<EaseFunction>(L, 2))
			{
				int motionFps3 = Lua.xlua_tointeger(L, 1);
				EaseFunction customEase = objectTranslator.GetDelegate<EaseFunction>(L, 2);
				EaseFunction o4 = EaseFactory.StopMotion(motionFps3, customEase);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DG.Tweening.EaseFactory.StopMotion!");
	}
}
