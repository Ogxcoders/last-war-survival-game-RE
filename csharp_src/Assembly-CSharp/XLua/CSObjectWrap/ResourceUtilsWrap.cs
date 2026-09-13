using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ResourceUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ResourceUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 12, 1, 1);
		Utils.RegisterFunc(L, -4, "GetResourceImagePath", _m_GetResourceImagePath_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetRewardTypeImagePath", _m_GetRewardTypeImagePath_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetRewardTypeName", _m_GetRewardTypeName_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetResourceTypeCityBuildingByType", _m_GetResourceTypeCityBuildingByType_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetResourcesTypeByCityBuildingType", _m_GetResourcesTypeByCityBuildingType_xlua_st_);
		Utils.RegisterFunc(L, -4, "RewardType2ResourceType", _m_RewardType2ResourceType_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetBuildTabTypeByResourceType", _m_GetBuildTabTypeByResourceType_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetResourceTypeByBuildTabType", _m_GetResourceTypeByBuildTabType_xlua_st_);
		Utils.RegisterFunc(L, -4, "ArmyConsume", _m_ArmyConsume_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetResourceItemCount", _m_GetResourceItemCount_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetOutBuildByResType", _m_GetOutBuildByResType_xlua_st_);
		Utils.RegisterFunc(L, -2, "pairStorage", _g_get_pairStorage);
		Utils.RegisterFunc(L, -1, "pairStorage", _s_set_pairStorage);
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
				ResourceUtils o = new ResourceUtils();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceUtils constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResourceImagePath_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ResourceType val);
			string resourceImagePath = ResourceUtils.GetResourceImagePath(val);
			Lua.lua_pushstring(L, resourceImagePath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRewardTypeImagePath_xlua_st_(IntPtr L)
	{
		try
		{
			string rewardTypeImagePath = ResourceUtils.GetRewardTypeImagePath(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushstring(L, rewardTypeImagePath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRewardTypeName_xlua_st_(IntPtr L)
	{
		try
		{
			string rewardTypeName = ResourceUtils.GetRewardTypeName(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushstring(L, rewardTypeName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResourceTypeCityBuildingByType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<int> resourceTypeCityBuildingByType = ResourceUtils.GetResourceTypeCityBuildingByType(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, resourceTypeCityBuildingByType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResourcesTypeByCityBuildingType_xlua_st_(IntPtr L)
	{
		try
		{
			int resourcesTypeByCityBuildingType = ResourceUtils.GetResourcesTypeByCityBuildingType(Lua.xlua_tointeger(L, 1));
			Lua.xlua_pushinteger(L, resourcesTypeByCityBuildingType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RewardType2ResourceType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out RewardType v);
			ResourceType val = ResourceUtils.RewardType2ResourceType(v);
			objectTranslator.PushResourceType(L, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildTabTypeByResourceType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ResourceType val);
			UIMainBottomBuildType buildTabTypeByResourceType = ResourceUtils.GetBuildTabTypeByResourceType(val);
			objectTranslator.Push(L, buildTabTypeByResourceType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResourceTypeByBuildTabType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out UIMainBottomBuildType v);
			ResourceType resourceTypeByBuildTabType = ResourceUtils.GetResourceTypeByBuildTabType(v);
			objectTranslator.PushResourceType(L, resourceTypeByBuildTabType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ArmyConsume_xlua_st_(IntPtr L)
	{
		try
		{
			float num = ResourceUtils.ArmyConsume(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResourceItemCount_xlua_st_(IntPtr L)
	{
		try
		{
			long resourceItemCount = ResourceUtils.GetResourceItemCount(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushint64(L, resourceItemCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOutBuildByResType_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ResourceType val);
			int outBuildByResType = ResourceUtils.GetOutBuildByResType(val);
			Lua.xlua_pushinteger(L, outBuildByResType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pairStorage(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, ResourceUtils.pairStorage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pairStorage(IntPtr L)
	{
		try
		{
			ResourceUtils.pairStorage = (Dictionary<ResourceType, List<int>>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Dictionary<ResourceType, List<int>>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
