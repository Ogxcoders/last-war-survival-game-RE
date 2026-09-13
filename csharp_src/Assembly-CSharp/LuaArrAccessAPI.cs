using System;
using GameFramework;
using XLua;
using XLua.LuaDLL;

[LuaCallCSharp(GenFlag.No)]
public class LuaArrAccessAPI
{
	public static bool IsLuajit;

	public static void RegisterPinFunc(IntPtr L)
	{
		string name = "lua_safe_pin_bind";
		Lua.lua_pushstdcallcfunction(L, PinFunction);
		if (Lua.xlua_setglobal(L, name) != 0)
		{
			throw new Exception("call xlua_setglobal fail!");
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int PinFunction(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		IntPtr intPtr = Lua.lua_topointer(L, 1);
		LuaArrAccess luaArrAccess = (LuaArrAccess)objectTranslator.FastGetCSObj(L, 2);
		if (intPtr != IntPtr.Zero && Lua.lua_istable(L, 1))
		{
			luaArrAccess.OnPin(intPtr);
		}
		return 0;
	}

	public static void Init(bool IsJit)
	{
		IsLuajit = IsJit;
	}

	public static LuaArrAccess CreateLuaShareAccess()
	{
		if (IsLuajit)
		{
			Log.Warning("LuaAdapter: create LuaJitArrAccess");
			return new LuaJitArrAccess();
		}
		return new LuaArrAccess64();
	}
}
