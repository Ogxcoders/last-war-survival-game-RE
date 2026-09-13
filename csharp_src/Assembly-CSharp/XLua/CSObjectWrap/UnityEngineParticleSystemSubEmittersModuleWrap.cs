using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemSubEmittersModuleWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.SubEmittersModule);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 2, 1);
		Utils.RegisterFunc(L, -3, "AddSubEmitter", _m_AddSubEmitter);
		Utils.RegisterFunc(L, -3, "RemoveSubEmitter", _m_RemoveSubEmitter);
		Utils.RegisterFunc(L, -3, "SetSubEmitterSystem", _m_SetSubEmitterSystem);
		Utils.RegisterFunc(L, -3, "SetSubEmitterType", _m_SetSubEmitterType);
		Utils.RegisterFunc(L, -3, "SetSubEmitterProperties", _m_SetSubEmitterProperties);
		Utils.RegisterFunc(L, -3, "SetSubEmitterEmitProbability", _m_SetSubEmitterEmitProbability);
		Utils.RegisterFunc(L, -3, "GetSubEmitterSystem", _m_GetSubEmitterSystem);
		Utils.RegisterFunc(L, -3, "GetSubEmitterType", _m_GetSubEmitterType);
		Utils.RegisterFunc(L, -3, "GetSubEmitterProperties", _m_GetSubEmitterProperties);
		Utils.RegisterFunc(L, -3, "GetSubEmitterEmitProbability", _m_GetSubEmitterEmitProbability);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "subEmittersCount", _g_get_subEmittersCount);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
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
				objectTranslator.Push(L, default(ParticleSystem.SubEmittersModule));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.SubEmittersModule constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddSubEmitter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<ParticleSystem>(L, 2) && objectTranslator.Assignable<ParticleSystemSubEmitterType>(L, 3) && objectTranslator.Assignable<ParticleSystemSubEmitterProperties>(L, 4))
			{
				ParticleSystem subEmitter = (ParticleSystem)objectTranslator.GetObject(L, 2, typeof(ParticleSystem));
				objectTranslator.Get(L, 3, out ParticleSystemSubEmitterType v2);
				objectTranslator.Get(L, 4, out ParticleSystemSubEmitterProperties v3);
				v.AddSubEmitter(subEmitter, v2, v3);
				objectTranslator.Update(L, 1, v);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<ParticleSystem>(L, 2) && objectTranslator.Assignable<ParticleSystemSubEmitterType>(L, 3) && objectTranslator.Assignable<ParticleSystemSubEmitterProperties>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				ParticleSystem subEmitter2 = (ParticleSystem)objectTranslator.GetObject(L, 2, typeof(ParticleSystem));
				objectTranslator.Get(L, 3, out ParticleSystemSubEmitterType v4);
				objectTranslator.Get(L, 4, out ParticleSystemSubEmitterProperties v5);
				float emitProbability = (float)Lua.lua_tonumber(L, 5);
				v.AddSubEmitter(subEmitter2, v4, v5, emitProbability);
				objectTranslator.Update(L, 1, v);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.SubEmittersModule.AddSubEmitter!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveSubEmitter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			int index = Lua.xlua_tointeger(L, 2);
			v.RemoveSubEmitter(index);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSubEmitterSystem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			int index = Lua.xlua_tointeger(L, 2);
			ParticleSystem subEmitter = (ParticleSystem)objectTranslator.GetObject(L, 3, typeof(ParticleSystem));
			v.SetSubEmitterSystem(index, subEmitter);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSubEmitterType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			int index = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out ParticleSystemSubEmitterType v2);
			v.SetSubEmitterType(index, v2);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSubEmitterProperties(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			int index = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out ParticleSystemSubEmitterProperties v2);
			v.SetSubEmitterProperties(index, v2);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSubEmitterEmitProbability(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			int index = Lua.xlua_tointeger(L, 2);
			float emitProbability = (float)Lua.lua_tonumber(L, 3);
			v.SetSubEmitterEmitProbability(index, emitProbability);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSubEmitterSystem(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			int index = Lua.xlua_tointeger(L, 2);
			ParticleSystem subEmitterSystem = v.GetSubEmitterSystem(index);
			objectTranslator.Push(L, subEmitterSystem);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSubEmitterType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			int index = Lua.xlua_tointeger(L, 2);
			ParticleSystemSubEmitterType subEmitterType = v.GetSubEmitterType(index);
			objectTranslator.Push(L, subEmitterType);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSubEmitterProperties(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			int index = Lua.xlua_tointeger(L, 2);
			ParticleSystemSubEmitterProperties subEmitterProperties = v.GetSubEmitterProperties(index);
			objectTranslator.Push(L, subEmitterProperties);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSubEmitterEmitProbability(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			int index = Lua.xlua_tointeger(L, 2);
			float subEmitterEmitProbability = v.GetSubEmitterEmitProbability(index);
			Lua.lua_pushnumber(L, subEmitterEmitProbability);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enabled(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.SubEmittersModule v);
			Lua.lua_pushboolean(L, v.enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_subEmittersCount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.SubEmittersModule v);
			Lua.xlua_pushinteger(L, v.subEmittersCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enabled(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.SubEmittersModule v);
			v.enabled = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
