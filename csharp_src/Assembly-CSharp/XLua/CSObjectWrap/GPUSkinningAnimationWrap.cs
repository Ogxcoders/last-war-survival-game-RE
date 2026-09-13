using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GPUSkinningAnimationWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GPUSkinningAnimation);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 8, 8);
		Utils.RegisterFunc(L, -3, "GetBoneByTransform", _m_GetBoneByTransform);
		Utils.RegisterFunc(L, -3, "GetBoneIndex", _m_GetBoneIndex);
		Utils.RegisterFunc(L, -2, "guid", _g_get_guid);
		Utils.RegisterFunc(L, -2, "name", _g_get_name);
		Utils.RegisterFunc(L, -2, "bones", _g_get_bones);
		Utils.RegisterFunc(L, -2, "rootBoneIndex", _g_get_rootBoneIndex);
		Utils.RegisterFunc(L, -2, "clips", _g_get_clips);
		Utils.RegisterFunc(L, -2, "bounds", _g_get_bounds);
		Utils.RegisterFunc(L, -2, "textureWidth", _g_get_textureWidth);
		Utils.RegisterFunc(L, -2, "textureHeight", _g_get_textureHeight);
		Utils.RegisterFunc(L, -1, "guid", _s_set_guid);
		Utils.RegisterFunc(L, -1, "name", _s_set_name);
		Utils.RegisterFunc(L, -1, "bones", _s_set_bones);
		Utils.RegisterFunc(L, -1, "rootBoneIndex", _s_set_rootBoneIndex);
		Utils.RegisterFunc(L, -1, "clips", _s_set_clips);
		Utils.RegisterFunc(L, -1, "bounds", _s_set_bounds);
		Utils.RegisterFunc(L, -1, "textureWidth", _s_set_textureWidth);
		Utils.RegisterFunc(L, -1, "textureHeight", _s_set_textureHeight);
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
				GPUSkinningAnimation o = new GPUSkinningAnimation();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GPUSkinningAnimation constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBoneByTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)objectTranslator.FastGetCSObj(L, 1);
			Transform transform = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			GPUSkinningBone boneByTransform = gPUSkinningAnimation.GetBoneByTransform(transform);
			objectTranslator.Push(L, boneByTransform);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBoneIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)objectTranslator.FastGetCSObj(L, 1);
			GPUSkinningBone bone = (GPUSkinningBone)objectTranslator.GetObject(L, 2, typeof(GPUSkinningBone));
			int boneIndex = gPUSkinningAnimation.GetBoneIndex(bone);
			Lua.xlua_pushinteger(L, boneIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_guid(IntPtr L)
	{
		try
		{
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gPUSkinningAnimation.guid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_name(IntPtr L)
	{
		try
		{
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, gPUSkinningAnimation.name);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bones(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gPUSkinningAnimation.bones);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rootBoneIndex(IntPtr L)
	{
		try
		{
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, gPUSkinningAnimation.rootBoneIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clips(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gPUSkinningAnimation.clips);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_bounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineBounds(L, gPUSkinningAnimation.bounds);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureWidth(IntPtr L)
	{
		try
		{
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, gPUSkinningAnimation.textureWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textureHeight(IntPtr L)
	{
		try
		{
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, gPUSkinningAnimation.textureHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_guid(IntPtr L)
	{
		try
		{
			((GPUSkinningAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).guid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_name(IntPtr L)
	{
		try
		{
			((GPUSkinningAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).name = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bones(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((GPUSkinningAnimation)objectTranslator.FastGetCSObj(L, 1)).bones = (GPUSkinningBone[])objectTranslator.GetObject(L, 2, typeof(GPUSkinningBone[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rootBoneIndex(IntPtr L)
	{
		try
		{
			((GPUSkinningAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).rootBoneIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clips(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((GPUSkinningAnimation)objectTranslator.FastGetCSObj(L, 1)).clips = (GPUSkinningClip[])objectTranslator.GetObject(L, 2, typeof(GPUSkinningClip[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_bounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinningAnimation gPUSkinningAnimation = (GPUSkinningAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Bounds val);
			gPUSkinningAnimation.bounds = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureWidth(IntPtr L)
	{
		try
		{
			((GPUSkinningAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).textureWidth = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textureHeight(IntPtr L)
	{
		try
		{
			((GPUSkinningAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).textureHeight = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
