using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIDropdownWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Dropdown);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 9, 9, 9);
		Utils.RegisterFunc(L, -3, "SetValueWithoutNotify", _m_SetValueWithoutNotify);
		Utils.RegisterFunc(L, -3, "RefreshShownValue", _m_RefreshShownValue);
		Utils.RegisterFunc(L, -3, "AddOptions", _m_AddOptions);
		Utils.RegisterFunc(L, -3, "ClearOptions", _m_ClearOptions);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -3, "OnSubmit", _m_OnSubmit);
		Utils.RegisterFunc(L, -3, "OnCancel", _m_OnCancel);
		Utils.RegisterFunc(L, -3, "Show", _m_Show);
		Utils.RegisterFunc(L, -3, "Hide", _m_Hide);
		Utils.RegisterFunc(L, -2, "template", _g_get_template);
		Utils.RegisterFunc(L, -2, "captionText", _g_get_captionText);
		Utils.RegisterFunc(L, -2, "captionImage", _g_get_captionImage);
		Utils.RegisterFunc(L, -2, "itemText", _g_get_itemText);
		Utils.RegisterFunc(L, -2, "itemImage", _g_get_itemImage);
		Utils.RegisterFunc(L, -2, "options", _g_get_options);
		Utils.RegisterFunc(L, -2, "onValueChanged", _g_get_onValueChanged);
		Utils.RegisterFunc(L, -2, "alphaFadeSpeed", _g_get_alphaFadeSpeed);
		Utils.RegisterFunc(L, -2, "value", _g_get_value);
		Utils.RegisterFunc(L, -1, "template", _s_set_template);
		Utils.RegisterFunc(L, -1, "captionText", _s_set_captionText);
		Utils.RegisterFunc(L, -1, "captionImage", _s_set_captionImage);
		Utils.RegisterFunc(L, -1, "itemText", _s_set_itemText);
		Utils.RegisterFunc(L, -1, "itemImage", _s_set_itemImage);
		Utils.RegisterFunc(L, -1, "options", _s_set_options);
		Utils.RegisterFunc(L, -1, "onValueChanged", _s_set_onValueChanged);
		Utils.RegisterFunc(L, -1, "alphaFadeSpeed", _s_set_alphaFadeSpeed);
		Utils.RegisterFunc(L, -1, "value", _s_set_value);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.Dropdown does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetValueWithoutNotify(IntPtr L)
	{
		try
		{
			Dropdown obj = (Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int valueWithoutNotify = Lua.xlua_tointeger(L, 2);
			obj.SetValueWithoutNotify(valueWithoutNotify);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshShownValue(IntPtr L)
	{
		try
		{
			((Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshShownValue();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddOptions(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<Dropdown.OptionData>>(L, 2))
			{
				List<Dropdown.OptionData> options = (List<Dropdown.OptionData>)objectTranslator.GetObject(L, 2, typeof(List<Dropdown.OptionData>));
				dropdown.AddOptions(options);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<List<string>>(L, 2))
			{
				List<string> options2 = (List<string>)objectTranslator.GetObject(L, 2, typeof(List<string>));
				dropdown.AddOptions(options2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<List<Sprite>>(L, 2))
			{
				List<Sprite> options3 = (List<Sprite>)objectTranslator.GetObject(L, 2, typeof(List<Sprite>));
				dropdown.AddOptions(options3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.Dropdown.AddOptions!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearOptions(IntPtr L)
	{
		try
		{
			((Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearOptions();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPointerClick(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			dropdown.OnPointerClick(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnSubmit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			dropdown.OnSubmit(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnCancel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			dropdown.OnCancel(eventData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Show(IntPtr L)
	{
		try
		{
			((Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Show();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Hide(IntPtr L)
	{
		try
		{
			((Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Hide();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_template(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dropdown.template);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_captionText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dropdown.captionText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_captionImage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dropdown.captionImage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_itemText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dropdown.itemText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_itemImage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dropdown.itemImage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_options(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dropdown.options);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onValueChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dropdown dropdown = (Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, dropdown.onValueChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alphaFadeSpeed(IntPtr L)
	{
		try
		{
			Dropdown dropdown = (Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, dropdown.alphaFadeSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_value(IntPtr L)
	{
		try
		{
			Dropdown dropdown = (Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, dropdown.value);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_template(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Dropdown)objectTranslator.FastGetCSObj(L, 1)).template = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_captionText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Dropdown)objectTranslator.FastGetCSObj(L, 1)).captionText = (Text)objectTranslator.GetObject(L, 2, typeof(Text));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_captionImage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Dropdown)objectTranslator.FastGetCSObj(L, 1)).captionImage = (Image)objectTranslator.GetObject(L, 2, typeof(Image));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_itemText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Dropdown)objectTranslator.FastGetCSObj(L, 1)).itemText = (Text)objectTranslator.GetObject(L, 2, typeof(Text));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_itemImage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Dropdown)objectTranslator.FastGetCSObj(L, 1)).itemImage = (Image)objectTranslator.GetObject(L, 2, typeof(Image));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_options(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Dropdown)objectTranslator.FastGetCSObj(L, 1)).options = (List<Dropdown.OptionData>)objectTranslator.GetObject(L, 2, typeof(List<Dropdown.OptionData>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onValueChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Dropdown)objectTranslator.FastGetCSObj(L, 1)).onValueChanged = (Dropdown.DropdownEvent)objectTranslator.GetObject(L, 2, typeof(Dropdown.DropdownEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alphaFadeSpeed(IntPtr L)
	{
		try
		{
			((Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alphaFadeSpeed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_value(IntPtr L)
	{
		try
		{
			((Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).value = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
