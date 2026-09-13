using System;
using UnityEngine;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineParticleSystemTextureSheetAnimationModuleWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ParticleSystem.TextureSheetAnimationModule);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 17, 16);
		Utils.RegisterFunc(L, -3, "AddSprite", _m_AddSprite);
		Utils.RegisterFunc(L, -3, "RemoveSprite", _m_RemoveSprite);
		Utils.RegisterFunc(L, -3, "SetSprite", _m_SetSprite);
		Utils.RegisterFunc(L, -3, "GetSprite", _m_GetSprite);
		Utils.RegisterFunc(L, -2, "enabled", _g_get_enabled);
		Utils.RegisterFunc(L, -2, "mode", _g_get_mode);
		Utils.RegisterFunc(L, -2, "timeMode", _g_get_timeMode);
		Utils.RegisterFunc(L, -2, "fps", _g_get_fps);
		Utils.RegisterFunc(L, -2, "numTilesX", _g_get_numTilesX);
		Utils.RegisterFunc(L, -2, "numTilesY", _g_get_numTilesY);
		Utils.RegisterFunc(L, -2, "animation", _g_get_animation);
		Utils.RegisterFunc(L, -2, "rowMode", _g_get_rowMode);
		Utils.RegisterFunc(L, -2, "frameOverTime", _g_get_frameOverTime);
		Utils.RegisterFunc(L, -2, "frameOverTimeMultiplier", _g_get_frameOverTimeMultiplier);
		Utils.RegisterFunc(L, -2, "startFrame", _g_get_startFrame);
		Utils.RegisterFunc(L, -2, "startFrameMultiplier", _g_get_startFrameMultiplier);
		Utils.RegisterFunc(L, -2, "cycleCount", _g_get_cycleCount);
		Utils.RegisterFunc(L, -2, "rowIndex", _g_get_rowIndex);
		Utils.RegisterFunc(L, -2, "uvChannelMask", _g_get_uvChannelMask);
		Utils.RegisterFunc(L, -2, "spriteCount", _g_get_spriteCount);
		Utils.RegisterFunc(L, -2, "speedRange", _g_get_speedRange);
		Utils.RegisterFunc(L, -1, "enabled", _s_set_enabled);
		Utils.RegisterFunc(L, -1, "mode", _s_set_mode);
		Utils.RegisterFunc(L, -1, "timeMode", _s_set_timeMode);
		Utils.RegisterFunc(L, -1, "fps", _s_set_fps);
		Utils.RegisterFunc(L, -1, "numTilesX", _s_set_numTilesX);
		Utils.RegisterFunc(L, -1, "numTilesY", _s_set_numTilesY);
		Utils.RegisterFunc(L, -1, "animation", _s_set_animation);
		Utils.RegisterFunc(L, -1, "rowMode", _s_set_rowMode);
		Utils.RegisterFunc(L, -1, "frameOverTime", _s_set_frameOverTime);
		Utils.RegisterFunc(L, -1, "frameOverTimeMultiplier", _s_set_frameOverTimeMultiplier);
		Utils.RegisterFunc(L, -1, "startFrame", _s_set_startFrame);
		Utils.RegisterFunc(L, -1, "startFrameMultiplier", _s_set_startFrameMultiplier);
		Utils.RegisterFunc(L, -1, "cycleCount", _s_set_cycleCount);
		Utils.RegisterFunc(L, -1, "rowIndex", _s_set_rowIndex);
		Utils.RegisterFunc(L, -1, "uvChannelMask", _s_set_uvChannelMask);
		Utils.RegisterFunc(L, -1, "speedRange", _s_set_speedRange);
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
				objectTranslator.Push(L, default(ParticleSystem.TextureSheetAnimationModule));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.ParticleSystem.TextureSheetAnimationModule constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddSprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			Sprite sprite = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
			v.AddSprite(sprite);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveSprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			int index = Lua.xlua_tointeger(L, 2);
			v.RemoveSprite(index);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			int index = Lua.xlua_tointeger(L, 2);
			Sprite sprite = (Sprite)objectTranslator.GetObject(L, 3, typeof(Sprite));
			v.SetSprite(index, sprite);
			objectTranslator.Update(L, 1, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSprite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			int index = Lua.xlua_tointeger(L, 2);
			Sprite sprite = v.GetSprite(index);
			objectTranslator.Push(L, sprite);
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
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			Lua.lua_pushboolean(L, v.enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Push(L, v.mode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timeMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Push(L, v.timeMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fps(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			Lua.lua_pushnumber(L, v.fps);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_numTilesX(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			Lua.xlua_pushinteger(L, v.numTilesX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_numTilesY(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			Lua.xlua_pushinteger(L, v.numTilesY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Push(L, v.animation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rowMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Push(L, v.rowMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frameOverTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Push(L, v.frameOverTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frameOverTimeMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			Lua.lua_pushnumber(L, v.frameOverTimeMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startFrame(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Push(L, v.startFrame);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startFrameMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			Lua.lua_pushnumber(L, v.startFrameMultiplier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cycleCount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			Lua.xlua_pushinteger(L, v.cycleCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rowIndex(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			Lua.xlua_pushinteger(L, v.rowIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uvChannelMask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Push(L, v.uvChannelMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spriteCount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			Lua.xlua_pushinteger(L, v.spriteCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_speedRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.PushUnityEngineVector2(L, v.speedRange);
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
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			v.enabled = Lua.lua_toboolean(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Get(L, 2, out ParticleSystemAnimationMode v2);
			v.mode = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_timeMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Get(L, 2, out ParticleSystemAnimationTimeMode v2);
			v.timeMode = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fps(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			v.fps = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_numTilesX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			v.numTilesX = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_numTilesY(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			v.numTilesY = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Get(L, 2, out ParticleSystemAnimationType v2);
			v.animation = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rowMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Get(L, 2, out ParticleSystemAnimationRowMode v2);
			v.rowMode = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_frameOverTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.frameOverTime = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_frameOverTimeMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			v.frameOverTimeMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startFrame(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Get(L, 2, out ParticleSystem.MinMaxCurve v2);
			v.startFrame = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startFrameMultiplier(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			v.startFrameMultiplier = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cycleCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			v.cycleCount = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rowIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			v.rowIndex = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uvChannelMask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Get(L, 2, out UVChannelFlags v2);
			v.uvChannelMask = v2;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_speedRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out ParticleSystem.TextureSheetAnimationModule v);
			objectTranslator.Get(L, 2, out Vector2 val);
			v.speedRange = val;
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
