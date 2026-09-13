using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesUILayerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.UILayer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 10, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Scene", "Scene");
		Utils.RegisterObject(L, translator, -4, "Background", "Background");
		Utils.RegisterObject(L, translator, -4, "UIResource", "UIResource");
		Utils.RegisterObject(L, translator, -4, "Normal", "Normal");
		Utils.RegisterObject(L, translator, -4, "Info", "Info");
		Utils.RegisterObject(L, translator, -4, "Dialog", "Dialog");
		Utils.RegisterObject(L, translator, -4, "Guide", "Guide");
		Utils.RegisterObject(L, translator, -4, "TopMost", "TopMost");
		Utils.RegisterObject(L, translator, -4, "Battle3D", "3DUIContainer");
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameDefines.UILayer does not have a constructor!");
	}
}
