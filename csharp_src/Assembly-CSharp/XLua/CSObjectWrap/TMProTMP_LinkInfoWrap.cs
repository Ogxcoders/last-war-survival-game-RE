using System;
using TMPro;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_LinkInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TMP_LinkInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 6, 6);
		Utils.RegisterFunc(L, -3, "GetLinkText", _m_GetLinkText);
		Utils.RegisterFunc(L, -3, "GetLinkID", _m_GetLinkID);
		Utils.RegisterFunc(L, -2, "textComponent", _g_get_textComponent);
		Utils.RegisterFunc(L, -2, "hashCode", _g_get_hashCode);
		Utils.RegisterFunc(L, -2, "linkIdFirstCharacterIndex", _g_get_linkIdFirstCharacterIndex);
		Utils.RegisterFunc(L, -2, "linkIdLength", _g_get_linkIdLength);
		Utils.RegisterFunc(L, -2, "linkTextfirstCharacterIndex", _g_get_linkTextfirstCharacterIndex);
		Utils.RegisterFunc(L, -2, "linkTextLength", _g_get_linkTextLength);
		Utils.RegisterFunc(L, -1, "textComponent", _s_set_textComponent);
		Utils.RegisterFunc(L, -1, "hashCode", _s_set_hashCode);
		Utils.RegisterFunc(L, -1, "linkIdFirstCharacterIndex", _s_set_linkIdFirstCharacterIndex);
		Utils.RegisterFunc(L, -1, "linkIdLength", _s_set_linkIdLength);
		Utils.RegisterFunc(L, -1, "linkTextfirstCharacterIndex", _s_set_linkTextfirstCharacterIndex);
		Utils.RegisterFunc(L, -1, "linkTextLength", _s_set_linkTextLength);
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
				objectTranslator.Push(L, default(TMP_LinkInfo));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_LinkInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLinkText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TMP_LinkInfo v);
			string linkText = v.GetLinkText();
			Lua.lua_pushstring(L, linkText);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLinkID(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TMP_LinkInfo v);
			string linkID = v.GetLinkID();
			Lua.lua_pushstring(L, linkID);
			objectTranslator.Update(L, 1, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_textComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TMP_LinkInfo v);
			objectTranslator.Push(L, v.textComponent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hashCode(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out TMP_LinkInfo v);
			Lua.xlua_pushinteger(L, v.hashCode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_linkIdFirstCharacterIndex(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out TMP_LinkInfo v);
			Lua.xlua_pushinteger(L, v.linkIdFirstCharacterIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_linkIdLength(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out TMP_LinkInfo v);
			Lua.xlua_pushinteger(L, v.linkIdLength);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_linkTextfirstCharacterIndex(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out TMP_LinkInfo v);
			Lua.xlua_pushinteger(L, v.linkTextfirstCharacterIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_linkTextLength(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out TMP_LinkInfo v);
			Lua.xlua_pushinteger(L, v.linkTextLength);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_textComponent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TMP_LinkInfo v);
			v.textComponent = (TMP_Text)objectTranslator.GetObject(L, 2, typeof(TMP_Text));
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_hashCode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TMP_LinkInfo v);
			v.hashCode = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_linkIdFirstCharacterIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TMP_LinkInfo v);
			v.linkIdFirstCharacterIndex = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_linkIdLength(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TMP_LinkInfo v);
			v.linkIdLength = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_linkTextfirstCharacterIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TMP_LinkInfo v);
			v.linkTextfirstCharacterIndex = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_linkTextLength(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out TMP_LinkInfo v);
			v.linkTextLength = Lua.xlua_tointeger(L, 2);
			objectTranslator.Update(L, 1, v);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
