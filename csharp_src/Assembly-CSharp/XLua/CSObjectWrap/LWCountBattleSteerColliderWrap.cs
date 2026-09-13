using System;
using LW.CountBattle;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LWCountBattleSteerColliderWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SteerCollider);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 2, 2);
		Utils.RegisterFunc(L, -2, "id", _g_get_id);
		Utils.RegisterFunc(L, -2, "shape", _g_get_shape);
		Utils.RegisterFunc(L, -1, "id", _s_set_id);
		Utils.RegisterFunc(L, -1, "shape", _s_set_shape);
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
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Shape>(L, 3))
			{
				int id = Lua.xlua_tointeger(L, 2);
				Shape shape = (Shape)objectTranslator.GetObject(L, 3, typeof(Shape));
				SteerCollider o = new SteerCollider(id, shape);
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LW.CountBattle.SteerCollider constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_id(IntPtr L)
	{
		try
		{
			SteerCollider steerCollider = (SteerCollider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, steerCollider.id);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shape(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerCollider steerCollider = (SteerCollider)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerCollider.shape);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_id(IntPtr L)
	{
		try
		{
			((SteerCollider)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).id = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shape(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SteerCollider)objectTranslator.FastGetCSObj(L, 1)).shape = (Shape)objectTranslator.GetObject(L, 2, typeof(Shape));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
