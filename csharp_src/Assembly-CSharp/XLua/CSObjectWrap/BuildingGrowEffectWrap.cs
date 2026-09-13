using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BuildingGrowEffectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BuildingGrowEffect);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 3, 3);
		Utils.RegisterFunc(L, -3, "StartBuild", _m_StartBuild);
		Utils.RegisterFunc(L, -3, "DisappearBuild", _m_DisappearBuild);
		Utils.RegisterFunc(L, -3, "ShowNormal", _m_ShowNormal);
		Utils.RegisterFunc(L, -3, "ShowBuildGridSelection", _m_ShowBuildGridSelection);
		Utils.RegisterFunc(L, -3, "ShowCanPlace", _m_ShowCanPlace);
		Utils.RegisterFunc(L, -3, "GetHeight", _m_GetHeight);
		Utils.RegisterFunc(L, -3, "EndAnim", _m_EndAnim);
		Utils.RegisterFunc(L, -3, "SetAlphaValue", _m_SetAlphaValue);
		Utils.RegisterFunc(L, -3, "IsUseFakeShadow", _m_IsUseFakeShadow);
		Utils.RegisterFunc(L, -3, "SetRendererMaterials", _m_SetRendererMaterials);
		Utils.RegisterFunc(L, -2, "isWorking", _g_get_isWorking);
		Utils.RegisterFunc(L, -2, "isHiding", _g_get_isHiding);
		Utils.RegisterFunc(L, -2, "ProgressTime", _g_get_ProgressTime);
		Utils.RegisterFunc(L, -1, "isWorking", _s_set_isWorking);
		Utils.RegisterFunc(L, -1, "isHiding", _s_set_isHiding);
		Utils.RegisterFunc(L, -1, "ProgressTime", _s_set_ProgressTime);
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
				BuildingGrowEffect o = new BuildingGrowEffect();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BuildingGrowEffect constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartBuild(IntPtr L)
	{
		try
		{
			BuildingGrowEffect buildingGrowEffect = (BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 11 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && (Lua.lua_isnil(L, 8) || Lua.lua_type(L, 8) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 11))
			{
				long uuid = Lua.lua_toint64(L, 2);
				int buildId = Lua.xlua_tointeger(L, 3);
				int startTime = Lua.xlua_tointeger(L, 4);
				int endTime = Lua.xlua_tointeger(L, 5);
				int tileSizeX = Lua.xlua_tointeger(L, 6);
				int tileSizeY = Lua.xlua_tointeger(L, 7);
				string ownerUid = Lua.lua_tostring(L, 8);
				bool isDomeUpdate = Lua.lua_toboolean(L, 9);
				bool isShowRobet = Lua.lua_toboolean(L, 10);
				bool needGridAlpha = Lua.lua_toboolean(L, 11);
				buildingGrowEffect.StartBuild(uuid, buildId, startTime, endTime, tileSizeX, tileSizeY, ownerUid, isDomeUpdate, isShowRobet, needGridAlpha);
				return 0;
			}
			if (num == 10 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && (Lua.lua_isnil(L, 8) || Lua.lua_type(L, 8) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10))
			{
				long uuid2 = Lua.lua_toint64(L, 2);
				int buildId2 = Lua.xlua_tointeger(L, 3);
				int startTime2 = Lua.xlua_tointeger(L, 4);
				int endTime2 = Lua.xlua_tointeger(L, 5);
				int tileSizeX2 = Lua.xlua_tointeger(L, 6);
				int tileSizeY2 = Lua.xlua_tointeger(L, 7);
				string ownerUid2 = Lua.lua_tostring(L, 8);
				bool isDomeUpdate2 = Lua.lua_toboolean(L, 9);
				bool isShowRobet2 = Lua.lua_toboolean(L, 10);
				buildingGrowEffect.StartBuild(uuid2, buildId2, startTime2, endTime2, tileSizeX2, tileSizeY2, ownerUid2, isDomeUpdate2, isShowRobet2);
				return 0;
			}
			if (num == 9 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && (Lua.lua_isnil(L, 8) || Lua.lua_type(L, 8) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				long uuid3 = Lua.lua_toint64(L, 2);
				int buildId3 = Lua.xlua_tointeger(L, 3);
				int startTime3 = Lua.xlua_tointeger(L, 4);
				int endTime3 = Lua.xlua_tointeger(L, 5);
				int tileSizeX3 = Lua.xlua_tointeger(L, 6);
				int tileSizeY3 = Lua.xlua_tointeger(L, 7);
				string ownerUid3 = Lua.lua_tostring(L, 8);
				bool isDomeUpdate3 = Lua.lua_toboolean(L, 9);
				buildingGrowEffect.StartBuild(uuid3, buildId3, startTime3, endTime3, tileSizeX3, tileSizeY3, ownerUid3, isDomeUpdate3);
				return 0;
			}
			if (num == 8 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && (Lua.lua_isnil(L, 8) || Lua.lua_type(L, 8) == LuaTypes.LUA_TSTRING))
			{
				long uuid4 = Lua.lua_toint64(L, 2);
				int buildId4 = Lua.xlua_tointeger(L, 3);
				int startTime4 = Lua.xlua_tointeger(L, 4);
				int endTime4 = Lua.xlua_tointeger(L, 5);
				int tileSizeX4 = Lua.xlua_tointeger(L, 6);
				int tileSizeY4 = Lua.xlua_tointeger(L, 7);
				string ownerUid4 = Lua.lua_tostring(L, 8);
				buildingGrowEffect.StartBuild(uuid4, buildId4, startTime4, endTime4, tileSizeX4, tileSizeY4, ownerUid4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BuildingGrowEffect.StartBuild!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisappearBuild(IntPtr L)
	{
		try
		{
			BuildingGrowEffect obj = (BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			int buildId = Lua.xlua_tointeger(L, 3);
			int startTime = Lua.xlua_tointeger(L, 4);
			int endTime = Lua.xlua_tointeger(L, 5);
			int tileSizeX = Lua.xlua_tointeger(L, 6);
			int tileSizeY = Lua.xlua_tointeger(L, 7);
			string ownerUid = Lua.lua_tostring(L, 8);
			obj.DisappearBuild(uuid, buildId, startTime, endTime, tileSizeX, tileSizeY, ownerUid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowNormal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildingGrowEffect buildingGrowEffect = (BuildingGrowEffect)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<PlayerType>(L, 2))
			{
				objectTranslator.Get(L, 2, out PlayerType val);
				buildingGrowEffect.ShowNormal(val);
				return 0;
			}
			if (num == 1)
			{
				buildingGrowEffect.ShowNormal();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BuildingGrowEffect.ShowNormal!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowBuildGridSelection(IntPtr L)
	{
		try
		{
			((BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ShowBuildGridSelection();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowCanPlace(IntPtr L)
	{
		try
		{
			BuildingGrowEffect obj = (BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool canPlace = Lua.lua_toboolean(L, 2);
			obj.ShowCanPlace(canPlace);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHeight(IntPtr L)
	{
		try
		{
			float height = ((BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHeight();
			Lua.lua_pushnumber(L, height);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EndAnim(IntPtr L)
	{
		try
		{
			((BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EndAnim();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAlphaValue(IntPtr L)
	{
		try
		{
			BuildingGrowEffect obj = (BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float alphaValue = (float)Lua.lua_tonumber(L, 2);
			obj.SetAlphaValue(alphaValue);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsUseFakeShadow(IntPtr L)
	{
		try
		{
			bool value = ((BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsUseFakeShadow();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRendererMaterials(IntPtr L)
	{
		try
		{
			((BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetRendererMaterials();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isWorking(IntPtr L)
	{
		try
		{
			BuildingGrowEffect buildingGrowEffect = (BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, buildingGrowEffect.isWorking);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isHiding(IntPtr L)
	{
		try
		{
			BuildingGrowEffect buildingGrowEffect = (BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, buildingGrowEffect.isHiding);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ProgressTime(IntPtr L)
	{
		try
		{
			BuildingGrowEffect buildingGrowEffect = (BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, buildingGrowEffect.ProgressTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isWorking(IntPtr L)
	{
		try
		{
			((BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isWorking = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isHiding(IntPtr L)
	{
		try
		{
			((BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isHiding = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ProgressTime(IntPtr L)
	{
		try
		{
			((BuildingGrowEffect)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ProgressTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
