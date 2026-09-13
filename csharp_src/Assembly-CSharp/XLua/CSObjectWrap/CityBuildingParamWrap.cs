using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CityBuildingParamWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CityBuilding.Param);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 8, 8);
		Utils.RegisterFunc(L, -2, "buildId", _g_get_buildId);
		Utils.RegisterFunc(L, -2, "buildUuid", _g_get_buildUuid);
		Utils.RegisterFunc(L, -2, "point", _g_get_point);
		Utils.RegisterFunc(L, -2, "BuildTopType", _g_get_BuildTopType);
		Utils.RegisterFunc(L, -2, "noPutPoint", _g_get_noPutPoint);
		Utils.RegisterFunc(L, -2, "buildSceneType", _g_get_buildSceneType);
		Utils.RegisterFunc(L, -2, "noDoAnim", _g_get_noDoAnim);
		Utils.RegisterFunc(L, -2, "visible", _g_get_visible);
		Utils.RegisterFunc(L, -1, "buildId", _s_set_buildId);
		Utils.RegisterFunc(L, -1, "buildUuid", _s_set_buildUuid);
		Utils.RegisterFunc(L, -1, "point", _s_set_point);
		Utils.RegisterFunc(L, -1, "BuildTopType", _s_set_BuildTopType);
		Utils.RegisterFunc(L, -1, "noPutPoint", _s_set_noPutPoint);
		Utils.RegisterFunc(L, -1, "buildSceneType", _s_set_buildSceneType);
		Utils.RegisterFunc(L, -1, "noDoAnim", _s_set_noDoAnim);
		Utils.RegisterFunc(L, -1, "visible", _s_set_visible);
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
				CityBuilding.Param o = new CityBuilding.Param();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityBuilding.Param constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_buildId(IntPtr L)
	{
		try
		{
			CityBuilding.Param param = (CityBuilding.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, param.buildId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_buildUuid(IntPtr L)
	{
		try
		{
			CityBuilding.Param param = (CityBuilding.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, param.buildUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_point(IntPtr L)
	{
		try
		{
			CityBuilding.Param param = (CityBuilding.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, param.point);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BuildTopType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityBuilding.Param param = (CityBuilding.Param)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushPlaceBuildType(L, param.BuildTopType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_noPutPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityBuilding.Param param = (CityBuilding.Param)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, param.noPutPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_buildSceneType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityBuilding.Param param = (CityBuilding.Param)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushCityBuildingBuildSceneType(L, param.buildSceneType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_noDoAnim(IntPtr L)
	{
		try
		{
			CityBuilding.Param param = (CityBuilding.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, param.noDoAnim);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_visible(IntPtr L)
	{
		try
		{
			CityBuilding.Param param = (CityBuilding.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, param.visible);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_buildId(IntPtr L)
	{
		try
		{
			((CityBuilding.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).buildId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_buildUuid(IntPtr L)
	{
		try
		{
			((CityBuilding.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).buildUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_point(IntPtr L)
	{
		try
		{
			((CityBuilding.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).point = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BuildTopType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityBuilding.Param param = (CityBuilding.Param)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out PlaceBuildType val);
			param.BuildTopType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_noPutPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CityBuilding.Param)objectTranslator.FastGetCSObj(L, 1)).noPutPoint = (LuaTable)objectTranslator.GetObject(L, 2, typeof(LuaTable));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_buildSceneType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityBuilding.Param param = (CityBuilding.Param)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CityBuilding.BuildSceneType val);
			param.buildSceneType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_noDoAnim(IntPtr L)
	{
		try
		{
			((CityBuilding.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).noDoAnim = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_visible(IntPtr L)
	{
		try
		{
			((CityBuilding.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).visible = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
