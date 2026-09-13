using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesSpritePathWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.SpritePath);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "HeroIconSmall", "Assets/Main/Sprites/HeroIconsSmall/");
		Utils.RegisterObject(L, translator, -4, "UITitleTag", "Assets/Main/Sprites/UI/UITitleTag/");
		Utils.RegisterObject(L, translator, -4, "ContryFlag", "Assets/Main/Sprites/CountryFlag/");
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameDefines.SpritePath does not have a constructor!");
	}
}
