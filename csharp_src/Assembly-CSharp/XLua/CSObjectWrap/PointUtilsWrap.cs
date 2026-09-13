using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PointUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PointUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "ScreenPointToLocalPointInRectangle", _m_ScreenPointToLocalPointInRectangle_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "PointUtils does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenPointToLocalPointInRectangle_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RectTransform rectTransform = (RectTransform)objectTranslator.GetObject(L, 1, typeof(RectTransform));
			objectTranslator.Get(L, 2, out Vector2 val);
			Vector2 val2 = PointUtils.ScreenPointToLocalPointInRectangle(_camera: (Camera)objectTranslator.GetObject(L, 3, typeof(Camera)), _rectTransform: rectTransform, _screenPoint: val);
			objectTranslator.PushUnityEngineVector2(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
