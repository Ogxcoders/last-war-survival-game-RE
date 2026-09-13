using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CatmullRomUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CatmullRomUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterFunc(L, -4, "CalcCatmullRomCurve", _m_CalcCatmullRomCurve_xlua_st_);
		Utils.RegisterFunc(L, -4, "CalcCurve", _m_CalcCurve_xlua_st_);
		Utils.RegisterFunc(L, -4, "CalcCurveXZPlane", _m_CalcCurveXZPlane_xlua_st_);
		Utils.RegisterFunc(L, -4, "CalcCurveByPoints", _m_CalcCurveByPoints_xlua_st_);
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
				CatmullRomUtils o = new CatmullRomUtils();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CatmullRomUtils constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalcCatmullRomCurve_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<Vector3>>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				List<Vector3> ctrlPoints = (List<Vector3>)objectTranslator.GetObject(L, 1, typeof(List<Vector3>));
				bool loop = Lua.lua_toboolean(L, 2);
				List<Vector3> o = CatmullRomUtils.CalcCatmullRomCurve(ctrlPoints, loop);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<List<Vector3>>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				List<Vector3> ctrlPoints2 = (List<Vector3>)objectTranslator.GetObject(L, 1, typeof(List<Vector3>));
				int frameCount = Lua.xlua_tointeger(L, 2);
				List<Vector3> o2 = CatmullRomUtils.CalcCatmullRomCurve(ctrlPoints2, frameCount);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CatmullRomUtils.CalcCatmullRomCurve!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalcCurve_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			List<Vector3> o = CatmullRomUtils.CalcCurve(hwRatio: (float)Lua.lua_tonumber(L, 3), start: val, end: val2);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalcCurveXZPlane_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			List<Vector3> o = CatmullRomUtils.CalcCurveXZPlane(ctrPointRatio: (float)Lua.lua_tonumber(L, 3), isLeftSide: Lua.lua_toboolean(L, 4), start: val, end: val2);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalcCurveByPoints_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			objectTranslator.Get(L, 3, out Vector3 val3);
			List<Vector3> o = CatmullRomUtils.CalcCurveByPoints(val, val2, val3);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
