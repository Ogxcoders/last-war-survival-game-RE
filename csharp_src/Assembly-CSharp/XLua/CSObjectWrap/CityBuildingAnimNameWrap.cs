using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CityBuildingAnimNameWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CityBuilding.AnimName);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 10, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Idle", "idle");
		Utils.RegisterObject(L, translator, -4, "Click", "trigger");
		Utils.RegisterObject(L, translator, -4, "Place", "placed");
		Utils.RegisterObject(L, translator, -4, "Work", "working");
		Utils.RegisterObject(L, translator, -4, "StartWork", "start");
		Utils.RegisterObject(L, translator, -4, "EndWork", "end");
		Utils.RegisterObject(L, translator, -4, "WorkIdle", "work_idle");
		Utils.RegisterObject(L, translator, -4, "SelfWork", "self_working");
		Utils.RegisterObject(L, translator, -4, "SelfEndWork", "self_end");
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
				CityBuilding.AnimName o = new CityBuilding.AnimName();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityBuilding.AnimName constructor!");
	}
}
