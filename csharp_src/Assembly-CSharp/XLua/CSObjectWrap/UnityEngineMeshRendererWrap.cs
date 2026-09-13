using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineMeshRendererWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MeshRenderer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 2, 1);
		Utils.RegisterFunc(L, -2, "additionalVertexStreams", _g_get_additionalVertexStreams);
		Utils.RegisterFunc(L, -2, "subMeshStartIndex", _g_get_subMeshStartIndex);
		Utils.RegisterFunc(L, -1, "additionalVertexStreams", _s_set_additionalVertexStreams);
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
				MeshRenderer o = new MeshRenderer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.MeshRenderer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_additionalVertexStreams(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MeshRenderer meshRenderer = (MeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, meshRenderer.additionalVertexStreams);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_subMeshStartIndex(IntPtr L)
	{
		try
		{
			MeshRenderer meshRenderer = (MeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, meshRenderer.subMeshStartIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_additionalVertexStreams(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((MeshRenderer)objectTranslator.FastGetCSObj(L, 1)).additionalVertexStreams = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
