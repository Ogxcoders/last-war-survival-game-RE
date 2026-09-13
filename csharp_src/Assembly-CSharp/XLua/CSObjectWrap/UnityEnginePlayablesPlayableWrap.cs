using System;
using UnityEngine.Playables;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEnginePlayablesPlayableWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Playable);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 37, 0, 0);
		Utils.RegisterFunc(L, -3, "GetHandle", _m_GetHandle);
		Utils.RegisterFunc(L, -3, "GetPlayableType", _m_GetPlayableType);
		Utils.RegisterFunc(L, -3, "Equals", _m_Equals);
		Utils.RegisterFunc(L, -3, "IsNull", _m_IsNull);
		Utils.RegisterFunc(L, -3, "IsValid", _m_IsValid);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "GetGraph", _m_GetGraph);
		Utils.RegisterFunc(L, -3, "GetPlayState", _m_GetPlayState);
		Utils.RegisterFunc(L, -3, "Play", _m_Play);
		Utils.RegisterFunc(L, -3, "Pause", _m_Pause);
		Utils.RegisterFunc(L, -3, "SetSpeed", _m_SetSpeed);
		Utils.RegisterFunc(L, -3, "GetSpeed", _m_GetSpeed);
		Utils.RegisterFunc(L, -3, "SetDuration", _m_SetDuration);
		Utils.RegisterFunc(L, -3, "GetDuration", _m_GetDuration);
		Utils.RegisterFunc(L, -3, "SetTime", _m_SetTime);
		Utils.RegisterFunc(L, -3, "GetTime", _m_GetTime);
		Utils.RegisterFunc(L, -3, "GetPreviousTime", _m_GetPreviousTime);
		Utils.RegisterFunc(L, -3, "SetDone", _m_SetDone);
		Utils.RegisterFunc(L, -3, "IsDone", _m_IsDone);
		Utils.RegisterFunc(L, -3, "SetPropagateSetTime", _m_SetPropagateSetTime);
		Utils.RegisterFunc(L, -3, "GetPropagateSetTime", _m_GetPropagateSetTime);
		Utils.RegisterFunc(L, -3, "CanChangeInputs", _m_CanChangeInputs);
		Utils.RegisterFunc(L, -3, "CanSetWeights", _m_CanSetWeights);
		Utils.RegisterFunc(L, -3, "CanDestroy", _m_CanDestroy);
		Utils.RegisterFunc(L, -3, "SetInputCount", _m_SetInputCount);
		Utils.RegisterFunc(L, -3, "GetInputCount", _m_GetInputCount);
		Utils.RegisterFunc(L, -3, "SetOutputCount", _m_SetOutputCount);
		Utils.RegisterFunc(L, -3, "GetOutputCount", _m_GetOutputCount);
		Utils.RegisterFunc(L, -3, "GetInput", _m_GetInput);
		Utils.RegisterFunc(L, -3, "GetOutput", _m_GetOutput);
		Utils.RegisterFunc(L, -3, "SetInputWeight", _m_SetInputWeight);
		Utils.RegisterFunc(L, -3, "GetInputWeight", _m_GetInputWeight);
		Utils.RegisterFunc(L, -3, "DisconnectInput", _m_DisconnectInput);
		Utils.RegisterFunc(L, -3, "SetLeadTime", _m_SetLeadTime);
		Utils.RegisterFunc(L, -3, "GetLeadTime", _m_GetLeadTime);
		Utils.RegisterFunc(L, -3, "GetTraversalMode", _m_GetTraversalMode);
		Utils.RegisterFunc(L, -3, "SetTraversalMode", _m_SetTraversalMode);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 1, 0);
		Utils.RegisterFunc(L, -4, "Create", _m_Create_xlua_st_);
		Utils.RegisterFunc(L, -2, "Null", _g_get_Null);
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
				objectTranslator.Push(L, default(Playable));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Playables.Playable constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Create_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<PlayableGraph>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out PlayableGraph v);
				int inputCount = Lua.xlua_tointeger(L, 2);
				Playable playable = Playable.Create(v, inputCount);
				objectTranslator.Push(L, playable);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<PlayableGraph>(L, 1))
			{
				objectTranslator.Get(L, 1, out PlayableGraph v2);
				Playable playable2 = Playable.Create(v2);
				objectTranslator.Push(L, playable2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Playables.Playable.Create!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHandle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableHandle handle = v.GetHandle();
			objectTranslator.Push(L, handle);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPlayableType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			Type playableType = v.GetPlayableType();
			objectTranslator.Push(L, playableType);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Equals(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			objectTranslator.Get(L, 2, out Playable v2);
			bool value = v.Equals(v2);
			Lua.lua_pushboolean(L, value);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNull(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			bool value = v.IsNull();
			Lua.lua_pushboolean(L, value);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsValid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			bool value = v.IsValid();
			Lua.lua_pushboolean(L, value);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			v.Destroy();
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGraph(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableGraph graph = v.GetGraph();
			objectTranslator.Push(L, graph);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPlayState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayState playState = v.GetPlayState();
			objectTranslator.Push(L, playState);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Play(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			v.Play();
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Pause(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			v.Pause();
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableExtensions.SetSpeed(value: Lua.lua_tonumber(L, 2), playable: v);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			double speed = v.GetSpeed();
			Lua.lua_pushnumber(L, speed);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDuration(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableExtensions.SetDuration(value: Lua.lua_tonumber(L, 2), playable: v);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDuration(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			double duration = v.GetDuration();
			Lua.lua_pushnumber(L, duration);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableExtensions.SetTime(value: Lua.lua_tonumber(L, 2), playable: v);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			double time = v.GetTime();
			Lua.lua_pushnumber(L, time);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPreviousTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			double previousTime = v.GetPreviousTime();
			Lua.lua_pushnumber(L, previousTime);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableExtensions.SetDone(value: Lua.lua_toboolean(L, 2), playable: v);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			bool value = v.IsDone();
			Lua.lua_pushboolean(L, value);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPropagateSetTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableExtensions.SetPropagateSetTime(value: Lua.lua_toboolean(L, 2), playable: v);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPropagateSetTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			bool propagateSetTime = v.GetPropagateSetTime();
			Lua.lua_pushboolean(L, propagateSetTime);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanChangeInputs(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			bool value = v.CanChangeInputs();
			Lua.lua_pushboolean(L, value);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanSetWeights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			bool value = v.CanSetWeights();
			Lua.lua_pushboolean(L, value);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanDestroy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			bool value = v.CanDestroy();
			Lua.lua_pushboolean(L, value);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetInputCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableExtensions.SetInputCount(value: Lua.xlua_tointeger(L, 2), playable: v);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInputCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			int inputCount = v.GetInputCount();
			Lua.xlua_pushinteger(L, inputCount);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetOutputCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableExtensions.SetOutputCount(value: Lua.xlua_tointeger(L, 2), playable: v);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOutputCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			int outputCount = v.GetOutputCount();
			Lua.xlua_pushinteger(L, outputCount);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInput(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			Playable playable = PlayableExtensions.GetInput(inputPort: Lua.xlua_tointeger(L, 2), playable: v);
			objectTranslator.Push(L, playable);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOutput(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			Playable playable = PlayableExtensions.GetOutput(outputPort: Lua.xlua_tointeger(L, 2), playable: v);
			objectTranslator.Push(L, playable);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetInputWeight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableExtensions.SetInputWeight(inputIndex: Lua.xlua_tointeger(L, 2), weight: (float)Lua.lua_tonumber(L, 3), playable: v);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInputWeight(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			float num = PlayableExtensions.GetInputWeight(inputIndex: Lua.xlua_tointeger(L, 2), playable: v);
			Lua.lua_pushnumber(L, num);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisconnectInput(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableExtensions.DisconnectInput(inputPort: Lua.xlua_tointeger(L, 2), playable: v);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLeadTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableExtensions.SetLeadTime(value: (float)Lua.lua_tonumber(L, 2), playable: v);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLeadTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			float leadTime = v.GetLeadTime();
			Lua.lua_pushnumber(L, leadTime);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTraversalMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			PlayableTraversalMode traversalMode = v.GetTraversalMode();
			objectTranslator.Push(L, traversalMode);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTraversalMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Playable v);
			objectTranslator.Get(L, 2, out PlayableTraversalMode v2);
			v.SetTraversalMode(v2);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Null(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Playable.Null);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
