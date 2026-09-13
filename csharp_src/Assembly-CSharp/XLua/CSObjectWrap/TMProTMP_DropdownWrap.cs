using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_DropdownWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TMP_Dropdown);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 11, 12, 11);
		Utils.RegisterFunc(L, -3, "SetValueWithoutNotify", _m_SetValueWithoutNotify);
		Utils.RegisterFunc(L, -3, "RefreshShownValue", _m_RefreshShownValue);
		Utils.RegisterFunc(L, -3, "AddOptions", _m_AddOptions);
		Utils.RegisterFunc(L, -3, "ClearOptions", _m_ClearOptions);
		Utils.RegisterFunc(L, -3, "RegisterCreateDropdownListCallBack", _m_RegisterCreateDropdownListCallBack);
		Utils.RegisterFunc(L, -3, "UnregisterCreateDropdownListCallBack", _m_UnregisterCreateDropdownListCallBack);
		Utils.RegisterFunc(L, -3, "OnPointerClick", _m_OnPointerClick);
		Utils.RegisterFunc(L, -3, "OnSubmit", _m_OnSubmit);
		Utils.RegisterFunc(L, -3, "OnCancel", _m_OnCancel);
		Utils.RegisterFunc(L, -3, "Show", _m_Show);
		Utils.RegisterFunc(L, -3, "Hide", _m_Hide);
		Utils.RegisterFunc(L, -2, "template", _g_get_template);
		Utils.RegisterFunc(L, -2, "captionText", _g_get_captionText);
		Utils.RegisterFunc(L, -2, "captionImage", _g_get_captionImage);
		Utils.RegisterFunc(L, -2, "placeholder", _g_get_placeholder);
		Utils.RegisterFunc(L, -2, "itemText", _g_get_itemText);
		Utils.RegisterFunc(L, -2, "itemImage", _g_get_itemImage);
		Utils.RegisterFunc(L, -2, "options", _g_get_options);
		Utils.RegisterFunc(L, -2, "onValueChanged", _g_get_onValueChanged);
		Utils.RegisterFunc(L, -2, "isInvokeCbOnValueUnchanged", _g_get_isInvokeCbOnValueUnchanged);
		Utils.RegisterFunc(L, -2, "alphaFadeSpeed", _g_get_alphaFadeSpeed);
		Utils.RegisterFunc(L, -2, "value", _g_get_value);
		Utils.RegisterFunc(L, -2, "IsExpanded", _g_get_IsExpanded);
		Utils.RegisterFunc(L, -1, "template", _s_set_template);
		Utils.RegisterFunc(L, -1, "captionText", _s_set_captionText);
		Utils.RegisterFunc(L, -1, "captionImage", _s_set_captionImage);
		Utils.RegisterFunc(L, -1, "placeholder", _s_set_placeholder);
		Utils.RegisterFunc(L, -1, "itemText", _s_set_itemText);
		Utils.RegisterFunc(L, -1, "itemImage", _s_set_itemImage);
		Utils.RegisterFunc(L, -1, "options", _s_set_options);
		Utils.RegisterFunc(L, -1, "onValueChanged", _s_set_onValueChanged);
		Utils.RegisterFunc(L, -1, "isInvokeCbOnValueUnchanged", _s_set_isInvokeCbOnValueUnchanged);
		Utils.RegisterFunc(L, -1, "alphaFadeSpeed", _s_set_alphaFadeSpeed);
		Utils.RegisterFunc(L, -1, "value", _s_set_value);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "TMPro.TMP_Dropdown does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetValueWithoutNotify(IntPtr L)
	{
		try
		{
			TMP_Dropdown obj = (TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			((TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshShownValue();
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<List<TMP_Dropdown.OptionData>>(L, 2))
			{
				List<TMP_Dropdown.OptionData> options = (List<TMP_Dropdown.OptionData>)objectTranslator.GetObject(L, 2, typeof(List<TMP_Dropdown.OptionData>));
				tMP_Dropdown.AddOptions(options);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<List<string>>(L, 2))
			{
				List<string> options2 = (List<string>)objectTranslator.GetObject(L, 2, typeof(List<string>));
				tMP_Dropdown.AddOptions(options2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<List<Sprite>>(L, 2))
			{
				List<Sprite> options3 = (List<Sprite>)objectTranslator.GetObject(L, 2, typeof(List<Sprite>));
				tMP_Dropdown.AddOptions(options3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Dropdown.AddOptions!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearOptions(IntPtr L)
	{
		try
		{
			((TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearOptions();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterCreateDropdownListCallBack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			Action callback = objectTranslator.GetDelegate<Action>(L, 2);
			tMP_Dropdown.RegisterCreateDropdownListCallBack(callback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnregisterCreateDropdownListCallBack(IntPtr L)
	{
		try
		{
			((TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnregisterCreateDropdownListCallBack();
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			PointerEventData eventData = (PointerEventData)objectTranslator.GetObject(L, 2, typeof(PointerEventData));
			tMP_Dropdown.OnPointerClick(eventData);
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			tMP_Dropdown.OnSubmit(eventData);
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			BaseEventData eventData = (BaseEventData)objectTranslator.GetObject(L, 2, typeof(BaseEventData));
			tMP_Dropdown.OnCancel(eventData);
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
			((TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Show();
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
			((TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Hide();
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Dropdown.template);
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Dropdown.captionText);
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Dropdown.captionImage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_placeholder(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Dropdown.placeholder);
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Dropdown.itemText);
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Dropdown.itemImage);
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Dropdown.options);
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, tMP_Dropdown.onValueChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isInvokeCbOnValueUnchanged(IntPtr L)
	{
		try
		{
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Dropdown.isInvokeCbOnValueUnchanged);
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, tMP_Dropdown.alphaFadeSpeed);
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
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, tMP_Dropdown.value);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsExpanded(IntPtr L)
	{
		try
		{
			TMP_Dropdown tMP_Dropdown = (TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, tMP_Dropdown.IsExpanded);
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
			((TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1)).template = (RectTransform)objectTranslator.GetObject(L, 2, typeof(RectTransform));
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
			((TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1)).captionText = (TMP_Text)objectTranslator.GetObject(L, 2, typeof(TMP_Text));
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
			((TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1)).captionImage = (Image)objectTranslator.GetObject(L, 2, typeof(Image));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_placeholder(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1)).placeholder = (Graphic)objectTranslator.GetObject(L, 2, typeof(Graphic));
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
			((TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1)).itemText = (TMP_Text)objectTranslator.GetObject(L, 2, typeof(TMP_Text));
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
			((TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1)).itemImage = (Image)objectTranslator.GetObject(L, 2, typeof(Image));
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
			((TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1)).options = (List<TMP_Dropdown.OptionData>)objectTranslator.GetObject(L, 2, typeof(List<TMP_Dropdown.OptionData>));
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
			((TMP_Dropdown)objectTranslator.FastGetCSObj(L, 1)).onValueChanged = (TMP_Dropdown.DropdownEvent)objectTranslator.GetObject(L, 2, typeof(TMP_Dropdown.DropdownEvent));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isInvokeCbOnValueUnchanged(IntPtr L)
	{
		try
		{
			((TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isInvokeCbOnValueUnchanged = Lua.lua_toboolean(L, 2);
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
			((TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alphaFadeSpeed = (float)Lua.lua_tonumber(L, 2);
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
			((TMP_Dropdown)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).value = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
