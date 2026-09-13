using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BattleFieldObjWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BattleFieldObj);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 3, 0);
		Utils.RegisterFunc(L, -3, "SetState", _m_SetState);
		Utils.RegisterFunc(L, -2, "SimpleAnimation", _g_get_SimpleAnimation);
		Utils.RegisterFunc(L, -2, "FirePoint", _g_get_FirePoint);
		Utils.RegisterFunc(L, -2, "UpPoint", _g_get_UpPoint);
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
				BattleFieldObj o = new BattleFieldObj();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BattleFieldObj constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetState(IntPtr L)
	{
		try
		{
			BattleFieldObj obj = (BattleFieldObj)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int state = Lua.xlua_tointeger(L, 2);
			obj.SetState(state);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SimpleAnimation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleFieldObj battleFieldObj = (BattleFieldObj)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, battleFieldObj.SimpleAnimation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FirePoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleFieldObj battleFieldObj = (BattleFieldObj)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, battleFieldObj.FirePoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UpPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleFieldObj battleFieldObj = (BattleFieldObj)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, battleFieldObj.UpPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
