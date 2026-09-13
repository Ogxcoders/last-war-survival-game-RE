using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineColorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Color);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 5, 3, 8, 4);
		Utils.RegisterFunc(L, -4, "__add", __AddMeta);
		Utils.RegisterFunc(L, -4, "__sub", __SubMeta);
		Utils.RegisterFunc(L, -4, "__mul", __MulMeta);
		Utils.RegisterFunc(L, -4, "__div", __DivMeta);
		Utils.RegisterFunc(L, -4, "__eq", __EqMeta);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -3, "GetHashCode", _m_GetHashCode);
		Utils.RegisterFunc(L, -3, "Equals", _m_Equals);
		Utils.RegisterFunc(L, -2, "grayscale", _g_get_grayscale);
		Utils.RegisterFunc(L, -2, "linear", _g_get_linear);
		Utils.RegisterFunc(L, -2, "gamma", _g_get_gamma);
		Utils.RegisterFunc(L, -2, "maxColorComponent", _g_get_maxColorComponent);
		Utils.RegisterFunc(L, -2, "r", _g_get_r);
		Utils.RegisterFunc(L, -2, "g", _g_get_g);
		Utils.RegisterFunc(L, -2, "b", _g_get_b);
		Utils.RegisterFunc(L, -2, "a", _g_get_a);
		Utils.RegisterFunc(L, -1, "r", _s_set_r);
		Utils.RegisterFunc(L, -1, "g", _s_set_g);
		Utils.RegisterFunc(L, -1, "b", _s_set_b);
		Utils.RegisterFunc(L, -1, "a", _s_set_a);
		Utils.EndObjectRegister(typeFromHandle, L, translator, __CSIndexer, __NewIndexer, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 11, 0);
		Utils.RegisterFunc(L, -4, "Lerp", _m_Lerp_xlua_st_);
		Utils.RegisterFunc(L, -4, "LerpUnclamped", _m_LerpUnclamped_xlua_st_);
		Utils.RegisterFunc(L, -4, "RGBToHSV", _m_RGBToHSV_xlua_st_);
		Utils.RegisterFunc(L, -4, "HSVToRGB", _m_HSVToRGB_xlua_st_);
		Utils.RegisterFunc(L, -2, "red", _g_get_red);
		Utils.RegisterFunc(L, -2, "green", _g_get_green);
		Utils.RegisterFunc(L, -2, "blue", _g_get_blue);
		Utils.RegisterFunc(L, -2, "white", _g_get_white);
		Utils.RegisterFunc(L, -2, "black", _g_get_black);
		Utils.RegisterFunc(L, -2, "yellow", _g_get_yellow);
		Utils.RegisterFunc(L, -2, "cyan", _g_get_cyan);
		Utils.RegisterFunc(L, -2, "magenta", _g_get_magenta);
		Utils.RegisterFunc(L, -2, "gray", _g_get_gray);
		Utils.RegisterFunc(L, -2, "grey", _g_get_grey);
		Utils.RegisterFunc(L, -2, "clear", _g_get_clear);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float r = (float)Lua.lua_tonumber(L, 2);
				float g = (float)Lua.lua_tonumber(L, 3);
				float b = (float)Lua.lua_tonumber(L, 4);
				float a = (float)Lua.lua_tonumber(L, 5);
				Color val = new Color(r, g, b, a);
				objectTranslator.PushUnityEngineColor(L, val);
				return 1;
			}
			if (Lua.lua_gettop(L) == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float r2 = (float)Lua.lua_tonumber(L, 2);
				float g2 = (float)Lua.lua_tonumber(L, 3);
				float b2 = (float)Lua.lua_tonumber(L, 4);
				Color val2 = new Color(r2, g2, b2);
				objectTranslator.PushUnityEngineColor(L, val2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 1)
			{
				objectTranslator.PushUnityEngineColor(L, default(Color));
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Color constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int __CSIndexer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Color>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Color val);
				int index = Lua.xlua_tointeger(L, 2);
				Lua.lua_pushboolean(L, value: true);
				Lua.lua_pushnumber(L, val[index]);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.lua_pushboolean(L, value: false);
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int __NewIndexer(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		try
		{
			if (objectTranslator.Assignable<Color>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 1, out Color val);
				int index = Lua.xlua_tointeger(L, 2);
				val[index] = (float)Lua.lua_tonumber(L, 3);
				Lua.lua_pushboolean(L, value: true);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.lua_pushboolean(L, value: false);
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __AddMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Color>(L, 1) && objectTranslator.Assignable<Color>(L, 2))
			{
				objectTranslator.Get(L, 1, out Color val);
				objectTranslator.Get(L, 2, out Color val2);
				objectTranslator.PushUnityEngineColor(L, val + val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of + operator, need UnityEngine.Color!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __SubMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Color>(L, 1) && objectTranslator.Assignable<Color>(L, 2))
			{
				objectTranslator.Get(L, 1, out Color val);
				objectTranslator.Get(L, 2, out Color val2);
				objectTranslator.PushUnityEngineColor(L, val - val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of - operator, need UnityEngine.Color!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __MulMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Color>(L, 1) && objectTranslator.Assignable<Color>(L, 2))
			{
				objectTranslator.Get(L, 1, out Color val);
				objectTranslator.Get(L, 2, out Color val2);
				objectTranslator.PushUnityEngineColor(L, val * val2);
				return 1;
			}
			if (objectTranslator.Assignable<Color>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Color val3);
				float num = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.PushUnityEngineColor(L, val3 * num);
				return 1;
			}
			if (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Color>(L, 2))
			{
				float num2 = (float)Lua.lua_tonumber(L, 1);
				objectTranslator.Get(L, 2, out Color val4);
				objectTranslator.PushUnityEngineColor(L, num2 * val4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of * operator, need UnityEngine.Color!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __DivMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Color>(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out Color val);
				float num = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.PushUnityEngineColor(L, val / num);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of / operator, need UnityEngine.Color!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __EqMeta(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (objectTranslator.Assignable<Color>(L, 1) && objectTranslator.Assignable<Color>(L, 2))
			{
				objectTranslator.Get(L, 1, out Color val);
				objectTranslator.Get(L, 2, out Color val2);
				Lua.lua_pushboolean(L, val == val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to right hand of == operator, need UnityEngine.Color!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToString(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				string str2 = val.ToString();
				Lua.lua_pushstring(L, str2);
				objectTranslator.UpdateUnityEngineColor(L, 1, val);
				return 1;
			}
			case 2:
				if (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
				{
					string text = Lua.lua_tostring(L, 2);
					string str = val.ToString(text);
					Lua.lua_pushstring(L, str);
					objectTranslator.UpdateUnityEngineColor(L, 1, val);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Color.ToString!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHashCode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			int hashCode = val.GetHashCode();
			Lua.xlua_pushinteger(L, hashCode);
			objectTranslator.UpdateUnityEngineColor(L, 1, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Equals(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object obj = objectTranslator.GetObject(L, 2, typeof(object));
				bool value = val.Equals(obj);
				Lua.lua_pushboolean(L, value);
				objectTranslator.UpdateUnityEngineColor(L, 1, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Color>(L, 2))
			{
				objectTranslator.Get(L, 2, out Color val2);
				bool value2 = val.Equals(val2);
				Lua.lua_pushboolean(L, value2);
				objectTranslator.UpdateUnityEngineColor(L, 1, val);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Color.Equals!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Lerp_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			objectTranslator.Get(L, 2, out Color val2);
			Color val3 = Color.Lerp(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineColor(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LerpUnclamped_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			objectTranslator.Get(L, 2, out Color val2);
			Color val3 = Color.LerpUnclamped(t: (float)Lua.lua_tonumber(L, 3), a: val, b: val2);
			objectTranslator.PushUnityEngineColor(L, val3);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RGBToHSV_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color val);
			Color.RGBToHSV(val, out var H, out var S, out var V);
			Lua.lua_pushnumber(L, H);
			Lua.lua_pushnumber(L, S);
			Lua.lua_pushnumber(L, V);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HSVToRGB_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float h = (float)Lua.lua_tonumber(L, 1);
				float s = (float)Lua.lua_tonumber(L, 2);
				float v = (float)Lua.lua_tonumber(L, 3);
				Color val = Color.HSVToRGB(h, s, v);
				objectTranslator.PushUnityEngineColor(L, val);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				float h2 = (float)Lua.lua_tonumber(L, 1);
				float s2 = (float)Lua.lua_tonumber(L, 2);
				float v2 = (float)Lua.lua_tonumber(L, 3);
				bool hdr = Lua.lua_toboolean(L, 4);
				Color val2 = Color.HSVToRGB(h2, s2, v2, hdr);
				objectTranslator.PushUnityEngineColor(L, val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Color.HSVToRGB!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_red(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.red);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_green(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.green);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_blue(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.blue);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_white(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.white);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_black(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.black);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_yellow(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.yellow);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cyan(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.cyan);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_magenta(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.magenta);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gray(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.gray);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_grey(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.grey);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clear(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineColor(L, Color.clear);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_grayscale(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color val);
			Lua.lua_pushnumber(L, val.grayscale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_linear(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			objectTranslator.PushUnityEngineColor(L, val.linear);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gamma(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			objectTranslator.PushUnityEngineColor(L, val.gamma);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxColorComponent(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color val);
			Lua.lua_pushnumber(L, val.maxColorComponent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_r(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color val);
			Lua.lua_pushnumber(L, val.r);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_g(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color val);
			Lua.lua_pushnumber(L, val.g);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_b(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color val);
			Lua.lua_pushnumber(L, val.b);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_a(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Color val);
			Lua.lua_pushnumber(L, val.a);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_r(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			val.r = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineColor(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_g(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			val.g = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineColor(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_b(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			val.b = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineColor(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_a(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Color val);
			val.a = (float)Lua.lua_tonumber(L, 2);
			objectTranslator.UpdateUnityEngineColor(L, 1, val);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
