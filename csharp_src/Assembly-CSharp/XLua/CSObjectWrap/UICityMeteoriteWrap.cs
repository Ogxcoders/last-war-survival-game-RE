using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UICityMeteoriteWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UICityMeteorite);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 6, 6);
		Utils.RegisterFunc(L, -3, "Refresh", _m_Refresh);
		Utils.RegisterFunc(L, -2, "crystalCount", _g_get_crystalCount);
		Utils.RegisterFunc(L, -2, "nucleusCount", _g_get_nucleusCount);
		Utils.RegisterFunc(L, -2, "goCrystal", _g_get_goCrystal);
		Utils.RegisterFunc(L, -2, "goNucleus", _g_get_goNucleus);
		Utils.RegisterFunc(L, -2, "leftPosition", _g_get_leftPosition);
		Utils.RegisterFunc(L, -2, "faceToCamera", _g_get_faceToCamera);
		Utils.RegisterFunc(L, -1, "crystalCount", _s_set_crystalCount);
		Utils.RegisterFunc(L, -1, "nucleusCount", _s_set_nucleusCount);
		Utils.RegisterFunc(L, -1, "goCrystal", _s_set_goCrystal);
		Utils.RegisterFunc(L, -1, "goNucleus", _s_set_goNucleus);
		Utils.RegisterFunc(L, -1, "leftPosition", _s_set_leftPosition);
		Utils.RegisterFunc(L, -1, "faceToCamera", _s_set_faceToCamera);
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
				UICityMeteorite o = new UICityMeteorite();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UICityMeteorite constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Refresh(IntPtr L)
	{
		try
		{
			UICityMeteorite obj = (UICityMeteorite)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int crystal = Lua.xlua_tointeger(L, 2);
			int nucleus = Lua.xlua_tointeger(L, 3);
			obj.Refresh(crystal, nucleus);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_crystalCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UICityMeteorite uICityMeteorite = (UICityMeteorite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uICityMeteorite.crystalCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_nucleusCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UICityMeteorite uICityMeteorite = (UICityMeteorite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uICityMeteorite.nucleusCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_goCrystal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UICityMeteorite uICityMeteorite = (UICityMeteorite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uICityMeteorite.goCrystal);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_goNucleus(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UICityMeteorite uICityMeteorite = (UICityMeteorite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uICityMeteorite.goNucleus);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_leftPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UICityMeteorite uICityMeteorite = (UICityMeteorite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, uICityMeteorite.leftPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_faceToCamera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UICityMeteorite uICityMeteorite = (UICityMeteorite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uICityMeteorite.faceToCamera);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_crystalCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UICityMeteorite)objectTranslator.FastGetCSObj(L, 1)).crystalCount = (SuperTextMesh)objectTranslator.GetObject(L, 2, typeof(SuperTextMesh));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_nucleusCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UICityMeteorite)objectTranslator.FastGetCSObj(L, 1)).nucleusCount = (SuperTextMesh)objectTranslator.GetObject(L, 2, typeof(SuperTextMesh));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_goCrystal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UICityMeteorite)objectTranslator.FastGetCSObj(L, 1)).goCrystal = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_goNucleus(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UICityMeteorite)objectTranslator.FastGetCSObj(L, 1)).goNucleus = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_leftPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UICityMeteorite uICityMeteorite = (UICityMeteorite)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			uICityMeteorite.leftPosition = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_faceToCamera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UICityMeteorite)objectTranslator.FastGetCSObj(L, 1)).faceToCamera = (AutoFaceToCamera)objectTranslator.GetObject(L, 2, typeof(AutoFaceToCamera));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
