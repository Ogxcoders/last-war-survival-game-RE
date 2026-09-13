using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MeteoriteWorldEffectPlayerBattleStateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MeteoriteWorldEffectPlayer.BattleState);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 0, 0);
		Utils.RegisterFunc(L, -3, "EnterState", _m_EnterState);
		Utils.RegisterFunc(L, -3, "LeaveState", _m_LeaveState);
		Utils.RegisterFunc(L, -3, "UpdateState", _m_UpdateState);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "MeteoriteWorldEffectPlayer.BattleState does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnterState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeteoriteWorldEffectPlayer.BattleState battleState = (MeteoriteWorldEffectPlayer.BattleState)objectTranslator.FastGetCSObj(L, 1);
			MeteoriteWorldEffectPlayer player = (MeteoriteWorldEffectPlayer)objectTranslator.GetObject(L, 2, typeof(MeteoriteWorldEffectPlayer));
			battleState.EnterState(player);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LeaveState(IntPtr L)
	{
		try
		{
			((MeteoriteWorldEffectPlayer.BattleState)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LeaveState();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateState(IntPtr L)
	{
		try
		{
			MeteoriteWorldEffectPlayer.BattleState obj = (MeteoriteWorldEffectPlayer.BattleState)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			obj.UpdateState(deltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
