using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesSpecialItemIDWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.SpecialItemID);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 9, 0, 0);
		Utils.RegisterObject(L, translator, -4, "ITEM_MOVE_RANDOM", 200001);
		Utils.RegisterObject(L, translator, -4, "ITEM_MOVE_CITY", 200002);
		Utils.RegisterObject(L, translator, -4, "ITEM_FREE_MOVE_CITY", 200005);
		Utils.RegisterObject(L, translator, -4, "LW_ITEM_ALLY_MOVE_CITY", 200008);
		Utils.RegisterObject(L, translator, -4, "ITEM_MERGESERVER_MOVECITY", 200453);
		Utils.RegisterObject(L, translator, -4, "ITEM_CROSS_MOVE_CITY", 200002);
		Utils.RegisterObject(L, translator, -4, "ITEM_CROSS_FREE_CITY", 200005);
		Utils.RegisterObject(L, translator, -4, "RECRUIT_TYPE_HERO_ACTIVITY", 200070);
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
				GameDefines.SpecialItemID o = new GameDefines.SpecialItemID();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameDefines.SpecialItemID constructor!");
	}
}
