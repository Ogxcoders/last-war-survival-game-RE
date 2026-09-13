using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 16, 31, 3);
		Utils.RegisterFunc(L, -3, "SetParticles", _m_SetParticles);
		Utils.RegisterFunc(L, -3, "GetParticles", _m_GetParticles);
		Utils.RegisterFunc(L, -3, "SetCustomParticleData", _m_SetCustomParticleData);
		Utils.RegisterFunc(L, -3, "GetCustomParticleData", _m_GetCustomParticleData);
		Utils.RegisterFunc(L, -3, "GetPlaybackState", _m_GetPlaybackState);
		Utils.RegisterFunc(L, -3, "SetPlaybackState", _m_SetPlaybackState);
		Utils.RegisterFunc(L, -3, "GetTrails", _m_GetTrails);
		Utils.RegisterFunc(L, -3, "SetTrails", _m_SetTrails);
		Utils.RegisterFunc(L, -3, "Simulate", _m_Simulate);
		Utils.RegisterFunc(L, -3, "Play", _m_Play);
		Utils.RegisterFunc(L, -3, "Pause", _m_Pause);
		Utils.RegisterFunc(L, -3, "Stop", _m_Stop);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "IsAlive", _m_IsAlive);
		Utils.RegisterFunc(L, -3, "Emit", _m_Emit);
		Utils.RegisterFunc(L, -3, "TriggerSubEmitter", _m_TriggerSubEmitter);
		Utils.RegisterFunc(L, -2, "isPlaying", _g_get_isPlaying);
		Utils.RegisterFunc(L, -2, "isEmitting", _g_get_isEmitting);
		Utils.RegisterFunc(L, -2, "isStopped", _g_get_isStopped);
		Utils.RegisterFunc(L, -2, "isPaused", _g_get_isPaused);
		Utils.RegisterFunc(L, -2, "particleCount", _g_get_particleCount);
		Utils.RegisterFunc(L, -2, "time", _g_get_time);
		Utils.RegisterFunc(L, -2, "randomSeed", _g_get_randomSeed);
		Utils.RegisterFunc(L, -2, "useAutoRandomSeed", _g_get_useAutoRandomSeed);
		Utils.RegisterFunc(L, -2, "proceduralSimulationSupported", _g_get_proceduralSimulationSupported);
		Utils.RegisterFunc(L, -2, "main", _g_get_main);
		Utils.RegisterFunc(L, -2, "emission", _g_get_emission);
		Utils.RegisterFunc(L, -2, "shape", _g_get_shape);
		Utils.RegisterFunc(L, -2, "velocityOverLifetime", _g_get_velocityOverLifetime);
		Utils.RegisterFunc(L, -2, "limitVelocityOverLifetime", _g_get_limitVelocityOverLifetime);
		Utils.RegisterFunc(L, -2, "inheritVelocity", _g_get_inheritVelocity);
		Utils.RegisterFunc(L, -2, "forceOverLifetime", _g_get_forceOverLifetime);
		Utils.RegisterFunc(L, -2, "colorOverLifetime", _g_get_colorOverLifetime);
		Utils.RegisterFunc(L, -2, "colorBySpeed", _g_get_colorBySpeed);
		Utils.RegisterFunc(L, -2, "sizeOverLifetime", _g_get_sizeOverLifetime);
		Utils.RegisterFunc(L, -2, "sizeBySpeed", _g_get_sizeBySpeed);
		Utils.RegisterFunc(L, -2, "rotationOverLifetime", _g_get_rotationOverLifetime);
		Utils.RegisterFunc(L, -2, "rotationBySpeed", _g_get_rotationBySpeed);
		Utils.RegisterFunc(L, -2, "externalForces", _g_get_externalForces);
		Utils.RegisterFunc(L, -2, "noise", _g_get_noise);
		Utils.RegisterFunc(L, -2, "collision", _g_get_collision);
		Utils.RegisterFunc(L, -2, "trigger", _g_get_trigger);
		Utils.RegisterFunc(L, -2, "subEmitters", _g_get_subEmitters);
		Utils.RegisterFunc(L, -2, "textureSheetAnimation", _g_get_textureSheetAnimation);
		Utils.RegisterFunc(L, -2, "lights", _g_get_lights);
		Utils.RegisterFunc(L, -2, "trails", _g_get_trails);
		Utils.RegisterFunc(L, -2, "customData", _g_get_customData);
		Utils.RegisterFunc(L, -1, "time", _s_set_time);
		Utils.RegisterFunc(L, -1, "randomSeed", _s_set_randomSeed);
		Utils.RegisterFunc(L, -1, "useAutoRandomSeed", _s_set_useAutoRandomSeed);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "ResetPreMappedBufferMemory", _m_ResetPreMappedBufferMemory_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetMaximumPreMappedBufferCounts", _m_SetMaximumPreMappedBufferCounts_xlua_st_);
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
				ParticleSystem o = new ParticleSystem();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetParticles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<ParticleSystem.Particle[]>(L, 2))
			{
				ParticleSystem.Particle[] particles = (ParticleSystem.Particle[])objectTranslator.GetObject(L, 2, typeof(ParticleSystem.Particle[]));
				particleSystem.SetParticles(particles);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<NativeArray<ParticleSystem.Particle>>(L, 2))
			{
				objectTranslator.Get(L, 2, out NativeArray<ParticleSystem.Particle> v);
				particleSystem.SetParticles(v);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<ParticleSystem.Particle[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				ParticleSystem.Particle[] particles2 = (ParticleSystem.Particle[])objectTranslator.GetObject(L, 2, typeof(ParticleSystem.Particle[]));
				int size = Lua.xlua_tointeger(L, 3);
				particleSystem.SetParticles(particles2, size);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<NativeArray<ParticleSystem.Particle>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out NativeArray<ParticleSystem.Particle> v2);
				int size2 = Lua.xlua_tointeger(L, 3);
				particleSystem.SetParticles(v2, size2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<ParticleSystem.Particle[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				ParticleSystem.Particle[] particles3 = (ParticleSystem.Particle[])objectTranslator.GetObject(L, 2, typeof(ParticleSystem.Particle[]));
				int size3 = Lua.xlua_tointeger(L, 3);
				int offset = Lua.xlua_tointeger(L, 4);
				particleSystem.SetParticles(particles3, size3, offset);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<NativeArray<ParticleSystem.Particle>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out NativeArray<ParticleSystem.Particle> v3);
				int size4 = Lua.xlua_tointeger(L, 3);
				int offset2 = Lua.xlua_tointeger(L, 4);
				particleSystem.SetParticles(v3, size4, offset2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.SetParticles!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetParticles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<ParticleSystem.Particle[]>(L, 2))
			{
				ParticleSystem.Particle[] particles = (ParticleSystem.Particle[])objectTranslator.GetObject(L, 2, typeof(ParticleSystem.Particle[]));
				int particles2 = particleSystem.GetParticles(particles);
				Lua.xlua_pushinteger(L, particles2);
				return 2;
			}
			if (num == 2 && objectTranslator.Assignable<NativeArray<ParticleSystem.Particle>>(L, 2))
			{
				objectTranslator.Get(L, 2, out NativeArray<ParticleSystem.Particle> v);
				int particles3 = particleSystem.GetParticles(v);
				Lua.xlua_pushinteger(L, particles3);
				return 2;
			}
			if (num == 3 && objectTranslator.Assignable<ParticleSystem.Particle[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				ParticleSystem.Particle[] particles4 = (ParticleSystem.Particle[])objectTranslator.GetObject(L, 2, typeof(ParticleSystem.Particle[]));
				int size = Lua.xlua_tointeger(L, 3);
				int particles5 = particleSystem.GetParticles(particles4, size);
				Lua.xlua_pushinteger(L, particles5);
				return 2;
			}
			if (num == 3 && objectTranslator.Assignable<NativeArray<ParticleSystem.Particle>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out NativeArray<ParticleSystem.Particle> v2);
				int size2 = Lua.xlua_tointeger(L, 3);
				int particles6 = particleSystem.GetParticles(v2, size2);
				Lua.xlua_pushinteger(L, particles6);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<ParticleSystem.Particle[]>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				ParticleSystem.Particle[] particles7 = (ParticleSystem.Particle[])objectTranslator.GetObject(L, 2, typeof(ParticleSystem.Particle[]));
				int size3 = Lua.xlua_tointeger(L, 3);
				int offset = Lua.xlua_tointeger(L, 4);
				int particles8 = particleSystem.GetParticles(particles7, size3, offset);
				Lua.xlua_pushinteger(L, particles8);
				return 2;
			}
			if (num == 4 && objectTranslator.Assignable<NativeArray<ParticleSystem.Particle>>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out NativeArray<ParticleSystem.Particle> v3);
				int size4 = Lua.xlua_tointeger(L, 3);
				int offset2 = Lua.xlua_tointeger(L, 4);
				int particles9 = particleSystem.GetParticles(v3, size4, offset2);
				Lua.xlua_pushinteger(L, particles9);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.GetParticles!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCustomParticleData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			List<Vector4> customData = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
			objectTranslator.Get(L, 3, out ParticleSystemCustomData v);
			particleSystem.SetCustomParticleData(customData, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCustomParticleData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			List<Vector4> customData = (List<Vector4>)objectTranslator.GetObject(L, 2, typeof(List<Vector4>));
			objectTranslator.Get(L, 3, out ParticleSystemCustomData v);
			int customParticleData = particleSystem.GetCustomParticleData(customData, v);
			Lua.xlua_pushinteger(L, customParticleData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPlaybackState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem.PlaybackState playbackState = ((ParticleSystem)objectTranslator.FastGetCSObj(L, 1)).GetPlaybackState();
			objectTranslator.Push(L, playbackState);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPlaybackState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ParticleSystem.PlaybackState v);
			particleSystem.SetPlaybackState(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTrails(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem.Trails trails = ((ParticleSystem)objectTranslator.FastGetCSObj(L, 1)).GetTrails();
			objectTranslator.Push(L, trails);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTrails(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ParticleSystem.Trails v);
			particleSystem.SetTrails(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Simulate(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float t = (float)Lua.lua_tonumber(L, 2);
				particleSystem.Simulate(t);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				float t2 = (float)Lua.lua_tonumber(L, 2);
				bool withChildren = Lua.lua_toboolean(L, 3);
				particleSystem.Simulate(t2, withChildren);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float t3 = (float)Lua.lua_tonumber(L, 2);
				bool withChildren2 = Lua.lua_toboolean(L, 3);
				bool restart = Lua.lua_toboolean(L, 4);
				particleSystem.Simulate(t3, withChildren2, restart);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				float t4 = (float)Lua.lua_tonumber(L, 2);
				bool withChildren3 = Lua.lua_toboolean(L, 3);
				bool restart2 = Lua.lua_toboolean(L, 4);
				bool fixedTimeStep = Lua.lua_toboolean(L, 5);
				particleSystem.Simulate(t4, withChildren3, restart2, fixedTimeStep);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.Simulate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Play(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				particleSystem.Play();
				return 0;
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool withChildren = Lua.lua_toboolean(L, 2);
					particleSystem.Play(withChildren);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.Play!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Pause(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				particleSystem.Pause();
				return 0;
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool withChildren = Lua.lua_toboolean(L, 2);
					particleSystem.Pause(withChildren);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.Pause!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Stop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			switch (num)
			{
			case 1:
				particleSystem.Stop();
				return 0;
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool withChildren = Lua.lua_toboolean(L, 2);
					particleSystem.Stop(withChildren);
					return 0;
				}
				break;
			}
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && objectTranslator.Assignable<ParticleSystemStopBehavior>(L, 3))
			{
				bool withChildren2 = Lua.lua_toboolean(L, 2);
				objectTranslator.Get(L, 3, out ParticleSystemStopBehavior v);
				particleSystem.Stop(withChildren2, v);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.Stop!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
				particleSystem.Clear();
				return 0;
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool withChildren = Lua.lua_toboolean(L, 2);
					particleSystem.Clear(withChildren);
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.Clear!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAlive(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				bool value2 = particleSystem.IsAlive();
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool withChildren = Lua.lua_toboolean(L, 2);
					bool value = particleSystem.IsAlive(withChildren);
					Lua.lua_pushboolean(L, value);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.IsAlive!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Emit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int count = Lua.xlua_tointeger(L, 2);
				particleSystem.Emit(count);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<ParticleSystem.EmitParams>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out ParticleSystem.EmitParams v);
				int count2 = Lua.xlua_tointeger(L, 3);
				particleSystem.Emit(v, count2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.Emit!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TriggerSubEmitter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int subEmitterIndex = Lua.xlua_tointeger(L, 2);
				particleSystem.TriggerSubEmitter(subEmitterIndex);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<ParticleSystem.Particle>(L, 3))
			{
				int subEmitterIndex2 = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out ParticleSystem.Particle v);
				particleSystem.TriggerSubEmitter(subEmitterIndex2, ref v);
				objectTranslator.Push(L, v);
				objectTranslator.Update(L, 3, v);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<List<ParticleSystem.Particle>>(L, 3))
			{
				int subEmitterIndex3 = Lua.xlua_tointeger(L, 2);
				List<ParticleSystem.Particle> particles = (List<ParticleSystem.Particle>)objectTranslator.GetObject(L, 3, typeof(List<ParticleSystem.Particle>));
				particleSystem.TriggerSubEmitter(subEmitterIndex3, particles);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.TriggerSubEmitter!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetPreMappedBufferMemory_xlua_st_(IntPtr L)
	{
		try
		{
			ParticleSystem.ResetPreMappedBufferMemory();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMaximumPreMappedBufferCounts_xlua_st_(IntPtr L)
	{
		try
		{
			int vertexBuffersCount = Lua.xlua_tointeger(L, 1);
			int indexBuffersCount = Lua.xlua_tointeger(L, 2);
			ParticleSystem.SetMaximumPreMappedBufferCounts(vertexBuffersCount, indexBuffersCount);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isPlaying(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, particleSystem.isPlaying);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isEmitting(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, particleSystem.isEmitting);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isStopped(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, particleSystem.isStopped);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isPaused(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, particleSystem.isPaused);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_particleCount(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, particleSystem.particleCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_time(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, particleSystem.time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_randomSeed(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushuint(L, particleSystem.randomSeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useAutoRandomSeed(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, particleSystem.useAutoRandomSeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_proceduralSimulationSupported(IntPtr L)
	{
		try
		{
			ParticleSystem particleSystem = (ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, particleSystem.proceduralSimulationSupported);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_main(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.main);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_emission(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.emission);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shape(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.shape);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_velocityOverLifetime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.velocityOverLifetime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_limitVelocityOverLifetime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.limitVelocityOverLifetime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inheritVelocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.inheritVelocity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_forceOverLifetime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.forceOverLifetime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colorOverLifetime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.colorOverLifetime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colorBySpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.colorBySpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sizeOverLifetime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.sizeOverLifetime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sizeBySpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.sizeBySpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rotationOverLifetime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.rotationOverLifetime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rotationBySpeed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.rotationBySpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_externalForces(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.externalForces);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_noise(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.noise);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_collision(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.collision);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_trigger(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.trigger);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_subEmitters(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.subEmitters);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureSheetAnimation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.textureSheetAnimation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.lights);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_trails(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.trails);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_customData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ParticleSystem particleSystem = (ParticleSystem)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, particleSystem.customData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_time(IntPtr L)
	{
		try
		{
			((ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).time = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_randomSeed(IntPtr L)
	{
		try
		{
			((ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).randomSeed = Lua.xlua_touint(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useAutoRandomSeed(IntPtr L)
	{
		try
		{
			((ParticleSystem)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useAutoRandomSeed = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
