using System;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIBaseMeshEffectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BaseMeshEffect);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 0, 0);
		Utils.RegisterFunc(L, -3, "ModifyMesh", _m_ModifyMesh);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.BaseMeshEffect does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ModifyMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BaseMeshEffect baseMeshEffect = (BaseMeshEffect)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Mesh>(L, 2))
			{
				Mesh mesh = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
				baseMeshEffect.ModifyMesh(mesh);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<VertexHelper>(L, 2))
			{
				VertexHelper vh = (VertexHelper)objectTranslator.GetObject(L, 2, typeof(VertexHelper));
				baseMeshEffect.ModifyMesh(vh);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.BaseMeshEffect.ModifyMesh!");
	}
}
