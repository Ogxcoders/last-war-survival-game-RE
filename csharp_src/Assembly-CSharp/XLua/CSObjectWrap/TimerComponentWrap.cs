using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TimerComponentWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TimerComponent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 45, 2, 2);
		Utils.RegisterFunc(L, -3, "Shutdown", _m_Shutdown);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "SetServerOffset", _m_SetServerOffset);
		Utils.RegisterFunc(L, -3, "SetWorldTime", _m_SetWorldTime);
		Utils.RegisterFunc(L, -3, "ChangeTime", _m_ChangeTime);
		Utils.RegisterFunc(L, -3, "UpdateServerMilliseconds", _m_UpdateServerMilliseconds);
		Utils.RegisterFunc(L, -3, "GetLocalMilliseconds", _m_GetLocalMilliseconds);
		Utils.RegisterFunc(L, -3, "GetLocalSeconds", _m_GetLocalSeconds);
		Utils.RegisterFunc(L, -3, "GetServerTime", _m_GetServerTime);
		Utils.RegisterFunc(L, -3, "GetServerTimeSeconds", _m_GetServerTimeSeconds);
		Utils.RegisterFunc(L, -3, "MillisecondToSecondString", _m_MillisecondToSecondString);
		Utils.RegisterFunc(L, -3, "MillisecondsToStringWithoutHour", _m_MillisecondsToStringWithoutHour);
		Utils.RegisterFunc(L, -3, "SecondsToStringWithoutHour", _m_SecondsToStringWithoutHour);
		Utils.RegisterFunc(L, -3, "SecondsToSecondString", _m_SecondsToSecondString);
		Utils.RegisterFunc(L, -3, "MilliSecondToFmtString", _m_MilliSecondToFmtString);
		Utils.RegisterFunc(L, -3, "SecondToFmtString", _m_SecondToFmtString);
		Utils.RegisterFunc(L, -3, "MilliSecondToFmtStringHighestTime", _m_MilliSecondToFmtStringHighestTime);
		Utils.RegisterFunc(L, -3, "SecondToFmtStringHighestTime", _m_SecondToFmtStringHighestTime);
		Utils.RegisterFunc(L, -3, "RefixTimebyZone", _m_RefixTimebyZone);
		Utils.RegisterFunc(L, -3, "PassTimeSecondString", _m_PassTimeSecondString);
		Utils.RegisterFunc(L, -3, "GetDateTime", _m_GetDateTime);
		Utils.RegisterFunc(L, -3, "TimeStampToTimeSimple", _m_TimeStampToTimeSimple);
		Utils.RegisterFunc(L, -3, "TimeStampToTimeDate", _m_TimeStampToTimeDate);
		Utils.RegisterFunc(L, -3, "TimeStampToTimeDateMd", _m_TimeStampToTimeDateMd);
		Utils.RegisterFunc(L, -3, "TimeStampToTime", _m_TimeStampToTime);
		Utils.RegisterFunc(L, -3, "TimeStampToHMS", _m_TimeStampToHMS);
		Utils.RegisterFunc(L, -3, "GetUniversalTime", _m_GetUniversalTime);
		Utils.RegisterFunc(L, -3, "GetUniversalChatTime", _m_GetUniversalChatTime);
		Utils.RegisterFunc(L, -3, "GetCurZoneTimestamp", _m_GetCurZoneTimestamp);
		Utils.RegisterFunc(L, -3, "GetUTCTimestamp", _m_GetUTCTimestamp);
		Utils.RegisterFunc(L, -3, "GetServerWeekDay", _m_GetServerWeekDay);
		Utils.RegisterFunc(L, -3, "WeekDay", _m_WeekDay);
		Utils.RegisterFunc(L, -3, "GetWeekDay", _m_GetWeekDay);
		Utils.RegisterFunc(L, -3, "GetResSecondsTo24", _m_GetResSecondsTo24);
		Utils.RegisterFunc(L, -3, "TimeStampToServerTime", _m_TimeStampToServerTime);
		Utils.RegisterFunc(L, -3, "TimeStampToLocalTime", _m_TimeStampToLocalTime);
		Utils.RegisterFunc(L, -3, "GetLocalTimeFromUtcOffset", _m_GetLocalTimeFromUtcOffset);
		Utils.RegisterFunc(L, -3, "RegisterTimer", _m_RegisterTimer);
		Utils.RegisterFunc(L, -3, "RegisterTimerRepeat", _m_RegisterTimerRepeat);
		Utils.RegisterFunc(L, -3, "CancelTimer", _m_CancelTimer);
		Utils.RegisterFunc(L, -3, "CancelAllTimers", _m_CancelAllTimers);
		Utils.RegisterFunc(L, -3, "SyncServerTime", _m_SyncServerTime);
		Utils.RegisterFunc(L, -3, "__GetServerTime", _m___GetServerTime);
		Utils.RegisterFunc(L, -3, "GetServerTimeWithoutOffset", _m_GetServerTimeWithoutOffset);
		Utils.RegisterFunc(L, -3, "GetServerTimeCS", _m_GetServerTimeCS);
		Utils.RegisterFunc(L, -2, "Light", _g_get_Light);
		Utils.RegisterFunc(L, -2, "Tomorrow", _g_get_Tomorrow);
		Utils.RegisterFunc(L, -1, "Light", _s_set_Light);
		Utils.RegisterFunc(L, -1, "Tomorrow", _s_set_Tomorrow);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 1);
		Utils.RegisterFunc(L, -2, "useRealTime", _g_get_useRealTime);
		Utils.RegisterFunc(L, -1, "useRealTime", _s_set_useRealTime);
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
				TimerComponent o = new TimerComponent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimerComponent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Shutdown(IntPtr L)
	{
		try
		{
			((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Shutdown();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float elapseSeconds = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(elapseSeconds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetServerOffset(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serverOffset = Lua.xlua_tointeger(L, 2);
			obj.SetServerOffset(serverOffset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetWorldTime(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int t = Lua.xlua_tointeger(L, 2);
			int tz = Lua.xlua_tointeger(L, 3);
			obj.SetWorldTime(t, tz);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeTime(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long t = Lua.lua_toint64(L, 2);
			long n = obj.ChangeTime(t);
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateServerMilliseconds(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long ms = Lua.lua_toint64(L, 2);
			obj.UpdateServerMilliseconds(ms);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLocalMilliseconds(IntPtr L)
	{
		try
		{
			long localMilliseconds = ((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLocalMilliseconds();
			Lua.lua_pushint64(L, localMilliseconds);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLocalSeconds(IntPtr L)
	{
		try
		{
			long localSeconds = ((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLocalSeconds();
			Lua.lua_pushint64(L, localSeconds);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerTime(IntPtr L)
	{
		try
		{
			long serverTime = ((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetServerTime();
			Lua.lua_pushint64(L, serverTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerTimeSeconds(IntPtr L)
	{
		try
		{
			int serverTimeSeconds = ((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetServerTimeSeconds();
			Lua.xlua_pushinteger(L, serverTimeSeconds);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MillisecondToSecondString(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long mstime = Lua.lua_toint64(L, 2);
			string separator = Lua.lua_tostring(L, 3);
			string str = obj.MillisecondToSecondString(mstime, separator);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MillisecondsToStringWithoutHour(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long mstime = Lua.lua_toint64(L, 2);
			string separator = Lua.lua_tostring(L, 3);
			string str = obj.MillisecondsToStringWithoutHour(mstime, separator);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SecondsToStringWithoutHour(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long second = Lua.lua_toint64(L, 2);
			string separator = Lua.lua_tostring(L, 3);
			string str = obj.SecondsToStringWithoutHour(second, separator);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SecondsToSecondString(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long second = Lua.lua_toint64(L, 2);
			string separator = Lua.lua_tostring(L, 3);
			string str = obj.SecondsToSecondString(second, separator);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MilliSecondToFmtString(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long milliSecond = Lua.lua_toint64(L, 2);
			string str = obj.MilliSecondToFmtString(milliSecond);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SecondToFmtString(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long secs = Lua.lua_toint64(L, 2);
			string str = obj.SecondToFmtString(secs);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MilliSecondToFmtStringHighestTime(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long milliSecond = Lua.lua_toint64(L, 2);
			string str = obj.MilliSecondToFmtStringHighestTime(milliSecond);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SecondToFmtStringHighestTime(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long secs = Lua.lua_toint64(L, 2);
			string str = obj.SecondToFmtStringHighestTime(secs);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefixTimebyZone(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long time = Lua.lua_toint64(L, 2);
			long n = obj.RefixTimebyZone(time);
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PassTimeSecondString(IntPtr L)
	{
		try
		{
			TimerComponent timerComponent = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				long passTime = Lua.lua_toint64(L, 2);
				bool isMstime = Lua.lua_toboolean(L, 3);
				string str = timerComponent.PassTimeSecondString(passTime, isMstime);
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long passTime2 = Lua.lua_toint64(L, 2);
				string str2 = timerComponent.PassTimeSecondString(passTime2);
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimerComponent.PassTimeSecondString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDateTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimerComponent obj = (TimerComponent)objectTranslator.FastGetCSObj(L, 1);
			long timeStamp = Lua.lua_toint64(L, 2);
			DateTime dateTime = obj.GetDateTime(timeStamp);
			objectTranslator.Push(L, dateTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TimeStampToTimeSimple(IntPtr L)
	{
		try
		{
			TimerComponent timerComponent = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				long timestamp = Lua.lua_toint64(L, 2);
				string format = Lua.lua_tostring(L, 3);
				string str = timerComponent.TimeStampToTimeSimple(timestamp, format);
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long timestamp2 = Lua.lua_toint64(L, 2);
				string str2 = timerComponent.TimeStampToTimeSimple(timestamp2);
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimerComponent.TimeStampToTimeSimple!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TimeStampToTimeDate(IntPtr L)
	{
		try
		{
			TimerComponent timerComponent = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				long timestamp = Lua.lua_toint64(L, 2);
				string format = Lua.lua_tostring(L, 3);
				string str = timerComponent.TimeStampToTimeDate(timestamp, format);
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long timestamp2 = Lua.lua_toint64(L, 2);
				string str2 = timerComponent.TimeStampToTimeDate(timestamp2);
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimerComponent.TimeStampToTimeDate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TimeStampToTimeDateMd(IntPtr L)
	{
		try
		{
			TimerComponent timerComponent = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				long timestamp = Lua.lua_toint64(L, 2);
				string format = Lua.lua_tostring(L, 3);
				string str = timerComponent.TimeStampToTimeDateMd(timestamp, format);
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long timestamp2 = Lua.lua_toint64(L, 2);
				string str2 = timerComponent.TimeStampToTimeDateMd(timestamp2);
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimerComponent.TimeStampToTimeDateMd!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TimeStampToTime(IntPtr L)
	{
		try
		{
			TimerComponent timerComponent = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				long timestamp = Lua.lua_toint64(L, 2);
				string format = Lua.lua_tostring(L, 3);
				string str = timerComponent.TimeStampToTime(timestamp, format);
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long timestamp2 = Lua.lua_toint64(L, 2);
				string str2 = timerComponent.TimeStampToTime(timestamp2);
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimerComponent.TimeStampToTime!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TimeStampToHMS(IntPtr L)
	{
		try
		{
			TimerComponent timerComponent = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				long timestamp = Lua.lua_toint64(L, 2);
				string format = Lua.lua_tostring(L, 3);
				string str = timerComponent.TimeStampToHMS(timestamp, format);
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long timestamp2 = Lua.lua_toint64(L, 2);
				string str2 = timerComponent.TimeStampToHMS(timestamp2);
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimerComponent.TimeStampToHMS!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUniversalTime(IntPtr L)
	{
		try
		{
			TimerComponent timerComponent = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				long timeStamp = Lua.lua_toint64(L, 2);
				string format = Lua.lua_tostring(L, 3);
				string universalTime = timerComponent.GetUniversalTime(timeStamp, format);
				Lua.lua_pushstring(L, universalTime);
				return 1;
			}
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long timeStamp2 = Lua.lua_toint64(L, 2);
				string universalTime2 = timerComponent.GetUniversalTime(timeStamp2);
				Lua.lua_pushstring(L, universalTime2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimerComponent.GetUniversalTime!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUniversalChatTime(IntPtr L)
	{
		try
		{
			TimerComponent timerComponent = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				long timeStamp = Lua.lua_toint64(L, 2);
				string format = Lua.lua_tostring(L, 3);
				string universalChatTime = timerComponent.GetUniversalChatTime(timeStamp, format);
				Lua.lua_pushstring(L, universalChatTime);
				return 1;
			}
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long timeStamp2 = Lua.lua_toint64(L, 2);
				string universalChatTime2 = timerComponent.GetUniversalChatTime(timeStamp2);
				Lua.lua_pushstring(L, universalChatTime2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimerComponent.GetUniversalChatTime!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurZoneTimestamp(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int year = Lua.xlua_tointeger(L, 2);
			int month = Lua.xlua_tointeger(L, 3);
			int day = Lua.xlua_tointeger(L, 4);
			int hour = Lua.xlua_tointeger(L, 5);
			int min = Lua.xlua_tointeger(L, 6);
			int sec = Lua.xlua_tointeger(L, 7);
			long curZoneTimestamp = obj.GetCurZoneTimestamp(year, month, day, hour, min, sec);
			Lua.lua_pushint64(L, curZoneTimestamp);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUTCTimestamp(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int year = Lua.xlua_tointeger(L, 2);
			int month = Lua.xlua_tointeger(L, 3);
			int day = Lua.xlua_tointeger(L, 4);
			int hour = Lua.xlua_tointeger(L, 5);
			int min = Lua.xlua_tointeger(L, 6);
			int sec = Lua.xlua_tointeger(L, 7);
			long uTCTimestamp = obj.GetUTCTimestamp(year, month, day, hour, min, sec);
			Lua.lua_pushint64(L, uTCTimestamp);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerWeekDay(IntPtr L)
	{
		try
		{
			int serverWeekDay = ((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetServerWeekDay();
			Lua.xlua_pushinteger(L, serverWeekDay);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WeekDay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimerComponent timerComponent = (TimerComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out DateTime v);
			int value = timerComponent.WeekDay(v);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWeekDay(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long milliSecond = Lua.lua_toint64(L, 2);
			int weekDay = obj.GetWeekDay(milliSecond);
			Lua.xlua_pushinteger(L, weekDay);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResSecondsTo24(IntPtr L)
	{
		try
		{
			int resSecondsTo = ((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetResSecondsTo24();
			Lua.xlua_pushinteger(L, resSecondsTo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TimeStampToServerTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimerComponent obj = (TimerComponent)objectTranslator.FastGetCSObj(L, 1);
			long timeStamp = Lua.lua_toint64(L, 2);
			DateTime dateTime = obj.TimeStampToServerTime(timeStamp);
			objectTranslator.Push(L, dateTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TimeStampToLocalTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimerComponent obj = (TimerComponent)objectTranslator.FastGetCSObj(L, 1);
			long timeStamp = Lua.lua_toint64(L, 2);
			DateTime dateTime = obj.TimeStampToLocalTime(timeStamp);
			objectTranslator.Push(L, dateTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLocalTimeFromUtcOffset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimerComponent obj = (TimerComponent)objectTranslator.FastGetCSObj(L, 1);
			long timeStamp = Lua.lua_toint64(L, 2);
			float offset = (float)Lua.lua_tonumber(L, 3);
			DateTime localTimeFromUtcOffset = obj.GetLocalTimeFromUtcOffset(timeStamp, offset);
			objectTranslator.Push(L, localTimeFromUtcOffset);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterTimer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimerComponent timerComponent = (TimerComponent)objectTranslator.FastGetCSObj(L, 1);
			float delaySec = (float)Lua.lua_tonumber(L, 2);
			Action onComplete = objectTranslator.GetDelegate<Action>(L, 3);
			ITimer o = timerComponent.RegisterTimer(delaySec, onComplete);
			objectTranslator.PushAny(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterTimerRepeat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimerComponent timerComponent = (TimerComponent)objectTranslator.FastGetCSObj(L, 1);
			float delaySec = (float)Lua.lua_tonumber(L, 2);
			float repeatSec = (float)Lua.lua_tonumber(L, 3);
			Action onComplete = objectTranslator.GetDelegate<Action>(L, 4);
			ITimer o = timerComponent.RegisterTimerRepeat(delaySec, repeatSec, onComplete);
			objectTranslator.PushAny(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CancelTimer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimerComponent timerComponent = (TimerComponent)objectTranslator.FastGetCSObj(L, 1);
			ITimer timer = (ITimer)objectTranslator.GetObject(L, 2, typeof(ITimer));
			timerComponent.CancelTimer(timer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CancelAllTimers(IntPtr L)
	{
		try
		{
			((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CancelAllTimers();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncServerTime(IntPtr L)
	{
		try
		{
			TimerComponent obj = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long recvTime = Lua.lua_toint64(L, 2);
			long serverTime = Lua.lua_toint64(L, 3);
			long clientTime = Lua.lua_toint64(L, 4);
			bool reset = Lua.lua_toboolean(L, 5);
			obj.SyncServerTime(recvTime, serverTime, clientTime, reset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m___GetServerTime(IntPtr L)
	{
		try
		{
			long n = ((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).__GetServerTime();
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerTimeWithoutOffset(IntPtr L)
	{
		try
		{
			long serverTimeWithoutOffset = ((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetServerTimeWithoutOffset();
			Lua.lua_pushint64(L, serverTimeWithoutOffset);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerTimeCS(IntPtr L)
	{
		try
		{
			long serverTimeCS = ((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetServerTimeCS();
			Lua.lua_pushint64(L, serverTimeCS);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Light(IntPtr L)
	{
		try
		{
			TimerComponent timerComponent = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, timerComponent.Light);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Tomorrow(IntPtr L)
	{
		try
		{
			TimerComponent timerComponent = (TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, timerComponent.Tomorrow);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useRealTime(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, TimerComponent.useRealTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Light(IntPtr L)
	{
		try
		{
			((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Light = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Tomorrow(IntPtr L)
	{
		try
		{
			((TimerComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Tomorrow = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useRealTime(IntPtr L)
	{
		try
		{
			TimerComponent.useRealTime = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
