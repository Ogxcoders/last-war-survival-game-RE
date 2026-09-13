using System;
using DG.Tweening;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AnimationHelperWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AnimationHelper);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "GetState", _m_GetState_xlua_st_);
		Utils.RegisterFunc(L, -4, "DOBezierCurve3D", _m_DOBezierCurve3D_xlua_st_);
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
				AnimationHelper o = new AnimationHelper();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AnimationHelper constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetState_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Animation animation = (Animation)objectTranslator.GetObject(L, 1, typeof(Animation));
			string name = Lua.lua_tostring(L, 2);
			AnimationState state = AnimationHelper.GetState(animation, name);
			objectTranslator.Push(L, state);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOBezierCurve3D_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 6 && objectTranslator.Assignable<Transform>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				Transform transform = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				objectTranslator.Get(L, 2, out Vector3 val);
				objectTranslator.Get(L, 3, out Vector3 val2);
				Tween o = AnimationHelper.DOBezierCurve3D(duration: (float)Lua.lua_tonumber(L, 4), controller: (float)Lua.lua_tonumber(L, 5), forward: Lua.lua_toboolean(L, 6), transform: transform, startPos: val, destPos: val2);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<Transform>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				Transform transform2 = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Vector3 val4);
				Tween o2 = AnimationHelper.DOBezierCurve3D(duration: (float)Lua.lua_tonumber(L, 4), controller: (float)Lua.lua_tonumber(L, 5), transform: transform2, startPos: val3, destPos: val4);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<Transform>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				Transform transform3 = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				objectTranslator.Get(L, 2, out Vector3 val5);
				objectTranslator.Get(L, 3, out Vector3 val6);
				Tween o3 = AnimationHelper.DOBezierCurve3D(duration: (float)Lua.lua_tonumber(L, 4), transform: transform3, startPos: val5, destPos: val6);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AnimationHelper.DOBezierCurve3D!");
	}
}
