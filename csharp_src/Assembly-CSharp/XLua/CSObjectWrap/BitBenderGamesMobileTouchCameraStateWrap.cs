using System;
using BitBenderGames;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BitBenderGamesMobileTouchCameraStateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(MobileTouchCamera.State), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(MobileTouchCamera.State), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(MobileTouchCamera.State), L, null, 9, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Idle", MobileTouchCamera.State.Idle);
		Utils.RegisterObject(L, translator, -4, "FreeLook", MobileTouchCamera.State.FreeLook);
		Utils.RegisterObject(L, translator, -4, "MoveTo", MobileTouchCamera.State.MoveTo);
		Utils.RegisterObject(L, translator, -4, "Focus", MobileTouchCamera.State.Focus);
		Utils.RegisterObject(L, translator, -4, "QuitFocus", MobileTouchCamera.State.QuitFocus);
		Utils.RegisterObject(L, translator, -4, "Follow", MobileTouchCamera.State.Follow);
		Utils.RegisterObject(L, translator, -4, "SyncWithTimeline", MobileTouchCamera.State.SyncWithTimeline);
		Utils.RegisterObject(L, translator, -4, "Lock", MobileTouchCamera.State.Lock);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(MobileTouchCamera.State), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, (MobileTouchCamera.State)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Idle"))
			{
				objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, MobileTouchCamera.State.Idle);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "FreeLook"))
			{
				objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, MobileTouchCamera.State.FreeLook);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "MoveTo"))
			{
				objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, MobileTouchCamera.State.MoveTo);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Focus"))
			{
				objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, MobileTouchCamera.State.Focus);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "QuitFocus"))
			{
				objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, MobileTouchCamera.State.QuitFocus);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Follow"))
			{
				objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, MobileTouchCamera.State.Follow);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "SyncWithTimeline"))
			{
				objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, MobileTouchCamera.State.SyncWithTimeline);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Lock"))
			{
				objectTranslator.PushBitBenderGamesMobileTouchCameraState(L, MobileTouchCamera.State.Lock);
				break;
			}
			return Lua.luaL_error(L, "invalid string for BitBenderGames.MobileTouchCamera.State!");
		default:
			return Lua.luaL_error(L, "invalid lua type for BitBenderGames.MobileTouchCamera.State! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
