using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AutoAdjustScreenPosWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AutoAdjustScreenPos);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
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
				AutoAdjustScreenPos o = new AutoAdjustScreenPos();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AutoAdjustScreenPos constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoAdjustScreenPos autoAdjustScreenPos = (AutoAdjustScreenPos)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				autoAdjustScreenPos.Init(val);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Transform>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				Transform obj = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				objectTranslator.Get(L, 3, out Vector3 val2);
				autoAdjustScreenPos.Init(obj, val2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AutoAdjustScreenPos.Init!");
	}
}
