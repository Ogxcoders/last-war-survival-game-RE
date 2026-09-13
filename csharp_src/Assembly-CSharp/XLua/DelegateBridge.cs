using System;
using System.Collections;
using BitBenderGames;
using DG.Tweening;
using DG.Tweening.Core;
using GameFramework;
using GameKit.Base;
using LW.CountBattle;
using Mopsicus.Plugins;
using Sfs2X.Requests;
using Spine.Unity;
using SuperScrollView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua;

public class DelegateBridge : DelegateBridgeBase
{
	internal static DelegateBridge[] DelegateBridgeList;

	public static bool Gen_Flag;

	public DelegateBridge(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	public void PCall(IntPtr L, int nArgs, int nResults, int errFunc)
	{
		if (Lua.lua_pcall(L, nArgs, nResults, errFunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(errFunc - 1);
		}
	}

	public void Action()
	{
		IntPtr l = luaEnv.L;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		if (Lua.lua_pcall(l, 0, 0, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		Lua.lua_settop(l, num);
	}

	public void Action<T1>(T1 p1)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, p1);
		if (Lua.lua_pcall(l, 1, 0, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		Lua.lua_settop(l, num);
	}

	public void Action<T1, T2>(T1 p1, T2 p2)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, p1);
		translator.PushByType(l, p2);
		if (Lua.lua_pcall(l, 2, 0, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		Lua.lua_settop(l, num);
	}

	public void Action<T1, T2, T3>(T1 p1, T2 p2, T3 p3)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, p1);
		translator.PushByType(l, p2);
		translator.PushByType(l, p3);
		if (Lua.lua_pcall(l, 3, 0, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		Lua.lua_settop(l, num);
	}

	public void Action<T1, T2, T3, T4>(T1 p1, T2 p2, T3 p3, T4 p4)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, p1);
		translator.PushByType(l, p2);
		translator.PushByType(l, p3);
		translator.PushByType(l, p4);
		if (Lua.lua_pcall(l, 4, 0, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		Lua.lua_settop(l, num);
	}

	public TResult Func<TResult>()
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		if (Lua.lua_pcall(l, 0, 1, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		try
		{
			translator.Get(l, -1, out TResult v);
			return v;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			Lua.lua_settop(l, num);
		}
	}

	public TResult Func<T1, TResult>(T1 p1)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, p1);
		if (Lua.lua_pcall(l, 1, 1, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		try
		{
			translator.Get(l, -1, out TResult v);
			return v;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			Lua.lua_settop(l, num);
		}
	}

	public TResult Func<T1, T2, TResult>(T1 p1, T2 p2)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, p1);
		translator.PushByType(l, p2);
		if (Lua.lua_pcall(l, 2, 1, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		try
		{
			translator.Get(l, -1, out TResult v);
			return v;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			Lua.lua_settop(l, num);
		}
	}

	public TResult Func<T1, T2, T3, TResult>(T1 p1, T2 p2, T3 p3)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, p1);
		translator.PushByType(l, p2);
		translator.PushByType(l, p3);
		if (Lua.lua_pcall(l, 3, 1, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		try
		{
			translator.Get(l, -1, out TResult v);
			return v;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			Lua.lua_settop(l, num);
		}
	}

	public TResult Func<T1, T2, T3, T4, TResult>(T1 p1, T2 p2, T3 p3, T4 p4)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, p1);
		translator.PushByType(l, p2);
		translator.PushByType(l, p3);
		translator.PushByType(l, p4);
		if (Lua.lua_pcall(l, 4, 1, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		try
		{
			translator.Get(l, -1, out TResult v);
			return v;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			Lua.lua_settop(l, num);
		}
	}

	public void __Gen_Delegate_Imp0(float p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushnumber(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp1(float p0, float p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushnumber(rawL, p0);
		Lua.lua_pushnumber(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp2()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		PCall(rawL, 0, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp3(long p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushint64(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp4(string p0, string p1, string p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushstring(rawL, p0);
		Lua.lua_pushstring(rawL, p1);
		Lua.lua_pushstring(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp5(string p0, string p1, string p2, string p3)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushstring(rawL, p0);
		Lua.lua_pushstring(rawL, p1);
		Lua.lua_pushstring(rawL, p2);
		Lua.lua_pushstring(rawL, p3);
		PCall(rawL, 4, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp6(string p0, int p1, string p2, string p3, Action p4, Action p5, Action p6, string p7, bool p8)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_pushstring(rawL, p0);
		Lua.xlua_pushinteger(rawL, p1);
		Lua.lua_pushstring(rawL, p2);
		Lua.lua_pushstring(rawL, p3);
		translator.Push(rawL, p4);
		translator.Push(rawL, p5);
		translator.Push(rawL, p6);
		Lua.lua_pushstring(rawL, p7);
		Lua.lua_pushboolean(rawL, p8);
		PCall(rawL, 9, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp7(string p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushstring(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public string __Gen_Delegate_Imp8(string p0, int p1, string p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushstring(rawL, p0);
		Lua.xlua_pushinteger(rawL, p1);
		Lua.lua_pushstring(rawL, p2);
		PCall(rawL, 3, 1, num);
		string result = Lua.lua_tostring(rawL, num + 1);
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public void __Gen_Delegate_Imp9(byte[] p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushstring(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public int __Gen_Delegate_Imp10(long p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return 0;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushint64(rawL, p0);
		PCall(rawL, 1, 1, num);
		int result = Lua.xlua_tointeger(rawL, num + 1);
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public void __Gen_Delegate_Imp11(UnityWebRequest p0, bool p1, object p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.Push(rawL, p0);
		Lua.lua_pushboolean(rawL, p1);
		translator.PushAny(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp12(int p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.xlua_pushinteger(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp13(GameObject p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp14(Vector2 p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.PushUnityEngineVector2(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp15(bool p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushboolean(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp16(bool p0, string p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushboolean(rawL, p0);
		Lua.lua_pushstring(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp17(bool p0, string p1, string p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushboolean(rawL, p0);
		Lua.lua_pushstring(rawL, p1);
		Lua.lua_pushstring(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp18(bool p0, GameCenterAuthData p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_pushboolean(rawL, p0);
		translator.Push(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp19(PlayGamesAuthData p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp20(Vector3 p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.PushUnityEngineVector3(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp21(Vector3 p0, float p1, float p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.PushUnityEngineVector3(rawL, p0);
		Lua.lua_pushnumber(rawL, p1);
		Lua.lua_pushnumber(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp22(long p0, int p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushint64(rawL, p0);
		Lua.xlua_pushinteger(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp23(int p0, int p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.xlua_pushinteger(rawL, p0);
		Lua.xlua_pushinteger(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp24(LuaTable p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp25(GameObject p0, int p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		Lua.xlua_pushinteger(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp26(GameObject p0, int p1, int p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		Lua.xlua_pushinteger(rawL, p1);
		Lua.xlua_pushinteger(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp27(Image p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp28(Sprite p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp29(string p0, string p1, int p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushstring(rawL, p0);
		Lua.lua_pushstring(rawL, p1);
		Lua.xlua_pushinteger(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp30(string p0, string p1, int p2, string p3)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushstring(rawL, p0);
		Lua.lua_pushstring(rawL, p1);
		Lua.xlua_pushinteger(rawL, p2);
		Lua.lua_pushstring(rawL, p3);
		PCall(rawL, 4, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp31(string p0, string p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushstring(rawL, p0);
		Lua.lua_pushstring(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp32(float p0, string p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushnumber(rawL, p0);
		Lua.lua_pushstring(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp33(string p0, int p1, string p2, string p3, int p4)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushstring(rawL, p0);
		Lua.xlua_pushinteger(rawL, p1);
		Lua.lua_pushstring(rawL, p2);
		Lua.lua_pushstring(rawL, p3);
		Lua.xlua_pushinteger(rawL, p4);
		PCall(rawL, 5, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public float __Gen_Delegate_Imp34()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return 0f;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		PCall(rawL, 0, 1, num);
		float result = (float)Lua.lua_tonumber(rawL, num + 1);
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public BasePool __Gen_Delegate_Imp35(string p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_pushstring(rawL, p0);
		PCall(rawL, 1, 1, num);
		BasePool result = (BasePool)translator.GetObject(rawL, num + 1, typeof(BasePool));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public void __Gen_Delegate_Imp36(InstanceRequest p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp37(PointerEventData p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp38(LoopListViewItem2 p0, bool p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		Lua.lua_pushboolean(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public Vector2 __Gen_Delegate_Imp39()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return default(Vector2);
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		PCall(rawL, 0, 1, num);
		translator.Get(rawL, num + 1, out Vector2 val);
		Lua.lua_settop(rawL, num - 1);
		return val;
	}

	public Vector3 __Gen_Delegate_Imp40()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return default(Vector3);
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		PCall(rawL, 0, 1, num);
		translator.Get(rawL, num + 1, out Vector3 val);
		Lua.lua_settop(rawL, num - 1);
		return val;
	}

	public void __Gen_Delegate_Imp41(int p0, bool p1, int p2, int p3, int p4)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.xlua_pushinteger(rawL, p0);
		Lua.lua_pushboolean(rawL, p1);
		Lua.xlua_pushinteger(rawL, p2);
		Lua.xlua_pushinteger(rawL, p3);
		Lua.xlua_pushinteger(rawL, p4);
		PCall(rawL, 5, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public LoopListViewItem2 __Gen_Delegate_Imp42(LoopListView2 p0, int p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.Push(rawL, p0);
		Lua.xlua_pushinteger(rawL, p1);
		PCall(rawL, 2, 1, num);
		LoopListViewItem2 result = (LoopListViewItem2)translator.GetObject(rawL, num + 1, typeof(LoopListViewItem2));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public LoopGridViewItem __Gen_Delegate_Imp43(LoopGridView p0, int p1, int p2, int p3)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.Push(rawL, p0);
		Lua.xlua_pushinteger(rawL, p1);
		Lua.xlua_pushinteger(rawL, p2);
		Lua.xlua_pushinteger(rawL, p3);
		PCall(rawL, 4, 1, num);
		LoopGridViewItem result = (LoopGridViewItem)translator.GetObject(rawL, num + 1, typeof(LoopGridViewItem));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public void __Gen_Delegate_Imp44(LoopListViewItem2 p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp45(SteerUnit p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp46(int p0, SteerUnit p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		Lua.xlua_pushinteger(rawL, p0);
		translator.Push(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp47(string p0, bool p1, string p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		Lua.lua_pushstring(rawL, p0);
		Lua.lua_pushboolean(rawL, p1);
		Lua.lua_pushstring(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp48(string p0, string p1, LogType p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_pushstring(rawL, p0);
		Lua.lua_pushstring(rawL, p1);
		translator.Push(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp49(SkeletonGraphic p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp50(string p0, UnityEngine.Object p1, object p2, bool p3)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_pushstring(rawL, p0);
		translator.Push(rawL, p1);
		translator.PushAny(rawL, p2);
		Lua.lua_pushboolean(rawL, p3);
		PCall(rawL, 4, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp51(GameObject p0, object p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.Push(rawL, p0);
		translator.PushAny(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp52(Vector3 p0, bool p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.PushUnityEngineVector3(rawL, p0);
		Lua.lua_pushboolean(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp53(Vector3 p0, Vector3 p1, Vector3 p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.PushUnityEngineVector3(rawL, p0);
		translator.PushUnityEngineVector3(rawL, p1);
		translator.PushUnityEngineVector3(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp54(Vector3 p0, Vector3 p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.PushUnityEngineVector3(rawL, p0);
		translator.PushUnityEngineVector3(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp55(Vector3 p0, float p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.PushUnityEngineVector3(rawL, p0);
		Lua.lua_pushnumber(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp56(PinchUpdateData p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.Push(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp57(Vector3 p0, bool p1, bool p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.PushUnityEngineVector3(rawL, p0);
		Lua.lua_pushboolean(rawL, p1);
		Lua.lua_pushboolean(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp58(Vector3 p0, bool p1, Vector3 p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.PushUnityEngineVector3(rawL, p0);
		Lua.lua_pushboolean(rawL, p1);
		translator.PushUnityEngineVector3(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public UserBindGaidMessage __Gen_Delegate_Imp59()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		PCall(rawL, 0, 1, num);
		UserBindGaidMessage result = (UserBindGaidMessage)translator.GetObject(rawL, num + 1, typeof(UserBindGaidMessage));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public string __Gen_Delegate_Imp60(object p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.PushAny(rawL, p0);
		PCall(rawL, 1, 1, num);
		string result = Lua.lua_tostring(rawL, num + 1);
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public IRequest __Gen_Delegate_Imp61(object p0, object[] p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.PushAny(rawL, p0);
		if (p1 != null)
		{
			for (int i = 0; i < p1.Length; i++)
			{
				translator.PushAny(rawL, p1[i]);
			}
		}
		PCall(rawL, 1 + ((p1 != null) ? p1.Length : 0), 1, num);
		IRequest result = (IRequest)translator.GetObject(rawL, num + 1, typeof(IRequest));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public void __Gen_Delegate_Imp62(object p0, object p1)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.PushAny(rawL, p0);
		translator.PushAny(rawL, p1);
		PCall(rawL, 2, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public void __Gen_Delegate_Imp63(object p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.PushAny(rawL, p0);
		PCall(rawL, 1, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public BuildMainCityMessage __Gen_Delegate_Imp64()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		PCall(rawL, 0, 1, num);
		BuildMainCityMessage result = (BuildMainCityMessage)translator.GetObject(rawL, num + 1, typeof(BuildMainCityMessage));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public CrossWorldMoveMessage __Gen_Delegate_Imp65()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		PCall(rawL, 0, 1, num);
		CrossWorldMoveMessage result = (CrossWorldMoveMessage)translator.GetObject(rawL, num + 1, typeof(CrossWorldMoveMessage));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public FSTaskCommand __Gen_Delegate_Imp66()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		PCall(rawL, 0, 1, num);
		FSTaskCommand result = (FSTaskCommand)translator.GetObject(rawL, num + 1, typeof(FSTaskCommand));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public PushUserOffMessage __Gen_Delegate_Imp67()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		PCall(rawL, 0, 1, num);
		PushUserOffMessage result = (PushUserOffMessage)translator.GetObject(rawL, num + 1, typeof(PushUserOffMessage));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public PushWorldMarchMessage __Gen_Delegate_Imp68()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		PCall(rawL, 0, 1, num);
		PushWorldMarchMessage result = (PushWorldMarchMessage)translator.GetObject(rawL, num + 1, typeof(PushWorldMarchMessage));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public WorldMarchFormationMessage __Gen_Delegate_Imp69()
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		PCall(rawL, 0, 1, num);
		WorldMarchFormationMessage result = (WorldMarchFormationMessage)translator.GetObject(rawL, num + 1, typeof(WorldMarchFormationMessage));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public bool __Gen_Delegate_Imp70(object p0)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return false;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		luaEnv.translator.PushAny(rawL, p0);
		PCall(rawL, 1, 1, num);
		bool result = Lua.lua_toboolean(rawL, num + 1);
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	public void __Gen_Delegate_Imp71(object p0, object p1, object p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.PushAny(rawL, p0);
		translator.PushAny(rawL, p1);
		translator.PushAny(rawL, p2);
		PCall(rawL, 3, 0, num);
		Lua.lua_settop(rawL, num - 1);
	}

	public IEnumerator __Gen_Delegate_Imp72(object p0, object p1, object p2)
	{
		if (base.Disposed)
		{
			Log.Error("Call delegate when lua env is released");
			return null;
		}
		IntPtr rawL = luaEnv.rawL;
		int num = Lua.pcall_prepare(rawL, errorFuncRef, luaReference);
		ObjectTranslator translator = luaEnv.translator;
		translator.PushAny(rawL, p0);
		translator.PushAny(rawL, p1);
		translator.PushAny(rawL, p2);
		PCall(rawL, 3, 1, num);
		IEnumerator result = (IEnumerator)translator.GetObject(rawL, num + 1, typeof(IEnumerator));
		Lua.lua_settop(rawL, num - 1);
		return result;
	}

	static DelegateBridge()
	{
		DelegateBridgeList = new DelegateBridge[0];
		Gen_Flag = false;
		Gen_Flag = true;
	}

	public override Delegate GetDelegateByType(Type type)
	{
		if (type == typeof(Action<float>))
		{
			return new Action<float>(__Gen_Delegate_Imp0);
		}
		if (type == typeof(DOSetter<float>))
		{
			return new DOSetter<float>(__Gen_Delegate_Imp0);
		}
		if (type == typeof(TouchInputController.InputLongTapProgress))
		{
			return new TouchInputController.InputLongTapProgress(__Gen_Delegate_Imp0);
		}
		if (type == typeof(Action<float, float>))
		{
			return new Action<float, float>(__Gen_Delegate_Imp1);
		}
		if (type == typeof(XLuaManager.DataCenterInitDelegate))
		{
			return new XLuaManager.DataCenterInitDelegate(__Gen_Delegate_Imp2);
		}
		if (type == typeof(XLuaManager.UpdateResourceItemMaxValueDelegate))
		{
			return new XLuaManager.UpdateResourceItemMaxValueDelegate(__Gen_Delegate_Imp2);
		}
		if (type == typeof(XLuaManager.PreloadAssetsDelegate))
		{
			return new XLuaManager.PreloadAssetsDelegate(__Gen_Delegate_Imp2);
		}
		if (type == typeof(Action))
		{
			return new Action(__Gen_Delegate_Imp2);
		}
		if (type == typeof(UnityAction))
		{
			return new UnityAction(__Gen_Delegate_Imp2);
		}
		if (type == typeof(TweenCallback))
		{
			return new TweenCallback(__Gen_Delegate_Imp2);
		}
		if (type == typeof(Application.LowMemoryCallback))
		{
			return new Application.LowMemoryCallback(__Gen_Delegate_Imp2);
		}
		if (type == typeof(SkeletonUtility.SkeletonUtilityDelegate))
		{
			return new SkeletonUtility.SkeletonUtilityDelegate(__Gen_Delegate_Imp2);
		}
		if (type == typeof(UnlimitedScrollView.OnPointerDownScrollViewDelegate))
		{
			return new UnlimitedScrollView.OnPointerDownScrollViewDelegate(__Gen_Delegate_Imp2);
		}
		if (type == typeof(XLuaManager.UITimeForLuaDelegate))
		{
			return new XLuaManager.UITimeForLuaDelegate(__Gen_Delegate_Imp3);
		}
		if (type == typeof(XLuaManager.UIShowTipsDelegate))
		{
			return new XLuaManager.UIShowTipsDelegate(__Gen_Delegate_Imp4);
		}
		if (type == typeof(XLuaManager.UIShowFoldUpBuild))
		{
			return new XLuaManager.UIShowFoldUpBuild(__Gen_Delegate_Imp5);
		}
		if (type == typeof(XLuaManager.UIShowMessageDelegate))
		{
			return new XLuaManager.UIShowMessageDelegate(__Gen_Delegate_Imp6);
		}
		if (type == typeof(XLuaManager.WebSocketProtocalDelegate))
		{
			return new XLuaManager.WebSocketProtocalDelegate(__Gen_Delegate_Imp7);
		}
		if (type == typeof(XLuaManager.GetTemplateDataFromLua))
		{
			return new XLuaManager.GetTemplateDataFromLua(__Gen_Delegate_Imp8);
		}
		if (type == typeof(XLuaManager.LuaLoadPbConfig))
		{
			return new XLuaManager.LuaLoadPbConfig(__Gen_Delegate_Imp9);
		}
		if (type == typeof(XLuaManager.ArmyFormationDataManager))
		{
			return new XLuaManager.ArmyFormationDataManager(__Gen_Delegate_Imp10);
		}
		if (type == typeof(WebRequestManager.OnWebRequestCallback))
		{
			return new WebRequestManager.OnWebRequestCallback(__Gen_Delegate_Imp11);
		}
		if (type == typeof(Action<int>))
		{
			return new Action<int>(__Gen_Delegate_Imp12);
		}
		if (type == typeof(UnlimitedScrollView.DragOnHeadOrTailOfTheScrollViewDelegate))
		{
			return new UnlimitedScrollView.DragOnHeadOrTailOfTheScrollViewDelegate(__Gen_Delegate_Imp12);
		}
		if (type == typeof(Action<GameObject>))
		{
			return new Action<GameObject>(__Gen_Delegate_Imp13);
		}
		if (type == typeof(Action<Vector2>))
		{
			return new Action<Vector2>(__Gen_Delegate_Imp14);
		}
		if (type == typeof(DOSetter<Vector2>))
		{
			return new DOSetter<Vector2>(__Gen_Delegate_Imp14);
		}
		if (type == typeof(Action<bool>))
		{
			return new Action<bool>(__Gen_Delegate_Imp15);
		}
		if (type == typeof(UnityAction<bool>))
		{
			return new UnityAction<bool>(__Gen_Delegate_Imp15);
		}
		if (type == typeof(Action<bool, string>))
		{
			return new Action<bool, string>(__Gen_Delegate_Imp16);
		}
		if (type == typeof(Action<bool, string, string>))
		{
			return new Action<bool, string, string>(__Gen_Delegate_Imp17);
		}
		if (type == typeof(Action<bool, GameCenterAuthData>))
		{
			return new Action<bool, GameCenterAuthData>(__Gen_Delegate_Imp18);
		}
		if (type == typeof(Action<PlayGamesAuthData>))
		{
			return new Action<PlayGamesAuthData>(__Gen_Delegate_Imp19);
		}
		if (type == typeof(Action<Vector3>))
		{
			return new Action<Vector3>(__Gen_Delegate_Imp20);
		}
		if (type == typeof(DOSetter<Vector3>))
		{
			return new DOSetter<Vector3>(__Gen_Delegate_Imp20);
		}
		if (type == typeof(TouchInputController.Input1PositionDelegate))
		{
			return new TouchInputController.Input1PositionDelegate(__Gen_Delegate_Imp20);
		}
		if (type == typeof(TouchInputController2.Input1PositionDelegate))
		{
			return new TouchInputController2.Input1PositionDelegate(__Gen_Delegate_Imp20);
		}
		if (type == typeof(Action<Vector3, float, float>))
		{
			return new Action<Vector3, float, float>(__Gen_Delegate_Imp21);
		}
		if (type == typeof(TouchInputController.PinchUpdateDelegate))
		{
			return new TouchInputController.PinchUpdateDelegate(__Gen_Delegate_Imp21);
		}
		if (type == typeof(TouchInputController2.PinchUpdateDelegate))
		{
			return new TouchInputController2.PinchUpdateDelegate(__Gen_Delegate_Imp21);
		}
		if (type == typeof(Action<long, int>))
		{
			return new Action<long, int>(__Gen_Delegate_Imp22);
		}
		if (type == typeof(Action<int, int>))
		{
			return new Action<int, int>(__Gen_Delegate_Imp23);
		}
		if (type == typeof(UnlimitedScrollView.BeginDragDelegate))
		{
			return new UnlimitedScrollView.BeginDragDelegate(__Gen_Delegate_Imp23);
		}
		if (type == typeof(Action<LuaTable>))
		{
			return new Action<LuaTable>(__Gen_Delegate_Imp24);
		}
		if (type == typeof(Action<GameObject, int>))
		{
			return new Action<GameObject, int>(__Gen_Delegate_Imp25);
		}
		if (type == typeof(ScrollView.MoveItemDelegate))
		{
			return new ScrollView.MoveItemDelegate(__Gen_Delegate_Imp25);
		}
		if (type == typeof(Action<GameObject, int, int>))
		{
			return new Action<GameObject, int, int>(__Gen_Delegate_Imp26);
		}
		if (type == typeof(Action<Image>))
		{
			return new Action<Image>(__Gen_Delegate_Imp27);
		}
		if (type == typeof(Action<Sprite>))
		{
			return new Action<Sprite>(__Gen_Delegate_Imp28);
		}
		if (type == typeof(Action<string, string, int>))
		{
			return new Action<string, string, int>(__Gen_Delegate_Imp29);
		}
		if (type == typeof(Action<string, string, int, string>))
		{
			return new Action<string, string, int, string>(__Gen_Delegate_Imp30);
		}
		if (type == typeof(Action<string, string>))
		{
			return new Action<string, string>(__Gen_Delegate_Imp31);
		}
		if (type == typeof(Action<float, string>))
		{
			return new Action<float, string>(__Gen_Delegate_Imp32);
		}
		if (type == typeof(Action<string, int, string, string, int>))
		{
			return new Action<string, int, string, string, int>(__Gen_Delegate_Imp33);
		}
		if (type == typeof(Func<float>))
		{
			return new Func<float>(__Gen_Delegate_Imp34);
		}
		if (type == typeof(DOGetter<float>))
		{
			return new DOGetter<float>(__Gen_Delegate_Imp34);
		}
		if (type == typeof(Func<string, BasePool>))
		{
			return new Func<string, BasePool>(__Gen_Delegate_Imp35);
		}
		if (type == typeof(Action<InstanceRequest>))
		{
			return new Action<InstanceRequest>(__Gen_Delegate_Imp36);
		}
		if (type == typeof(Action<PointerEventData>))
		{
			return new Action<PointerEventData>(__Gen_Delegate_Imp37);
		}
		if (type == typeof(ScrollView.PointerEventDelegate))
		{
			return new ScrollView.PointerEventDelegate(__Gen_Delegate_Imp37);
		}
		if (type == typeof(Action<LoopListViewItem2, bool>))
		{
			return new Action<LoopListViewItem2, bool>(__Gen_Delegate_Imp38);
		}
		if (type == typeof(DOGetter<Vector2>))
		{
			return new DOGetter<Vector2>(__Gen_Delegate_Imp39);
		}
		if (type == typeof(DOGetter<Vector3>))
		{
			return new DOGetter<Vector3>(__Gen_Delegate_Imp40);
		}
		if (type == typeof(MobileInputReceiver.ShowDelegate))
		{
			return new MobileInputReceiver.ShowDelegate(__Gen_Delegate_Imp41);
		}
		if (type == typeof(MobileInput.ShowDelegate))
		{
			return new MobileInput.ShowDelegate(__Gen_Delegate_Imp41);
		}
		if (type == typeof(Func<LoopListView2, int, LoopListViewItem2>))
		{
			return new Func<LoopListView2, int, LoopListViewItem2>(__Gen_Delegate_Imp42);
		}
		if (type == typeof(Func<LoopGridView, int, int, int, LoopGridViewItem>))
		{
			return new Func<LoopGridView, int, int, int, LoopGridViewItem>(__Gen_Delegate_Imp43);
		}
		if (type == typeof(Action<LoopListViewItem2>))
		{
			return new Action<LoopListViewItem2>(__Gen_Delegate_Imp44);
		}
		if (type == typeof(Action<SteerUnit>))
		{
			return new Action<SteerUnit>(__Gen_Delegate_Imp45);
		}
		if (type == typeof(Action<int, SteerUnit>))
		{
			return new Action<int, SteerUnit>(__Gen_Delegate_Imp46);
		}
		if (type == typeof(Application.AdvertisingIdentifierCallback))
		{
			return new Application.AdvertisingIdentifierCallback(__Gen_Delegate_Imp47);
		}
		if (type == typeof(Application.LogCallback))
		{
			return new Application.LogCallback(__Gen_Delegate_Imp48);
		}
		if (type == typeof(SkeletonGraphic.SkeletonRendererDelegate))
		{
			return new SkeletonGraphic.SkeletonRendererDelegate(__Gen_Delegate_Imp49);
		}
		if (type == typeof(DynamicResourceManager.OnLoadComplete))
		{
			return new DynamicResourceManager.OnLoadComplete(__Gen_Delegate_Imp50);
		}
		if (type == typeof(UnlimitedScrollView.ItemMoveInDelegate))
		{
			return new UnlimitedScrollView.ItemMoveInDelegate(__Gen_Delegate_Imp51);
		}
		if (type == typeof(UnlimitedScrollView.ItemMoveOutDelegate))
		{
			return new UnlimitedScrollView.ItemMoveOutDelegate(__Gen_Delegate_Imp51);
		}
		if (type == typeof(TouchInputController.InputDragStartDelegate))
		{
			return new TouchInputController.InputDragStartDelegate(__Gen_Delegate_Imp52);
		}
		if (type == typeof(TouchInputController.DragUpdateDelegate))
		{
			return new TouchInputController.DragUpdateDelegate(__Gen_Delegate_Imp53);
		}
		if (type == typeof(TouchInputController2.DragUpdateDelegate))
		{
			return new TouchInputController2.DragUpdateDelegate(__Gen_Delegate_Imp53);
		}
		if (type == typeof(TouchInputController.DragStopDelegate))
		{
			return new TouchInputController.DragStopDelegate(__Gen_Delegate_Imp54);
		}
		if (type == typeof(TouchInputController2.DragStopDelegate))
		{
			return new TouchInputController2.DragStopDelegate(__Gen_Delegate_Imp54);
		}
		if (type == typeof(TouchInputController.PinchStartDelegate))
		{
			return new TouchInputController.PinchStartDelegate(__Gen_Delegate_Imp55);
		}
		if (type == typeof(TouchInputController2.PinchStartDelegate))
		{
			return new TouchInputController2.PinchStartDelegate(__Gen_Delegate_Imp55);
		}
		if (type == typeof(TouchInputController.PinchUpdateExtendedDelegate))
		{
			return new TouchInputController.PinchUpdateExtendedDelegate(__Gen_Delegate_Imp56);
		}
		if (type == typeof(TouchInputController2.PinchUpdateExtendedDelegate))
		{
			return new TouchInputController2.PinchUpdateExtendedDelegate(__Gen_Delegate_Imp56);
		}
		if (type == typeof(TouchInputController.InputClickDelegate))
		{
			return new TouchInputController.InputClickDelegate(__Gen_Delegate_Imp57);
		}
		if (type == typeof(TouchInputController2.InputClickDelegate))
		{
			return new TouchInputController2.InputClickDelegate(__Gen_Delegate_Imp57);
		}
		if (type == typeof(TouchInputController2.InputDragStartDelegate))
		{
			return new TouchInputController2.InputDragStartDelegate(__Gen_Delegate_Imp58);
		}
		return null;
	}
}
