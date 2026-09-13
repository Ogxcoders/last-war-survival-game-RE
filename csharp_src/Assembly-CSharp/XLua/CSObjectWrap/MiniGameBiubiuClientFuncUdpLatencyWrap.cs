using System;
using System.Threading.Tasks;
using MiniGame.Biubiu.Client;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MiniGameBiubiuClientFuncUdpLatencyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FuncUdpLatency);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 0, 0);
		Utils.RegisterFunc(L, -4, "PingAll", _m_PingAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGameLiftServerPingValues", _m_GetGameLiftServerPingValues_xlua_st_);
		Utils.RegisterFunc(L, -4, "MeasureLatencyAsync", _m_MeasureLatencyAsync_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "MiniGame.Biubiu.Client.FuncUdpLatency does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PingAll_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int[] serverIds = (int[])objectTranslator.GetObject(L, 1, typeof(int[]));
			Action<string> complete = objectTranslator.GetDelegate<Action<string>>(L, 2);
			FuncUdpLatency.PingAll(serverIds, complete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGameLiftServerPingValues_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int[] serverIds = (int[])objectTranslator.GetObject(L, 1, typeof(int[]));
			Action<string> complete = objectTranslator.GetDelegate<Action<string>>(L, 2);
			FuncUdpLatency.GetGameLiftServerPingValues(serverIds, complete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MeasureLatencyAsync_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string domain = Lua.lua_tostring(L, 1);
			int port = Lua.xlua_tointeger(L, 2);
			string message = Lua.lua_tostring(L, 3);
			int numPings = Lua.xlua_tointeger(L, 4);
			int timeoutMs = Lua.xlua_tointeger(L, 5);
			Task<double> o = FuncUdpLatency.MeasureLatencyAsync(domain, port, message, numPings, timeoutMs);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
