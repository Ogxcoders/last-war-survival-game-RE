using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UILoadingComponentWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UILoadingComponent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 9, 40, 40);
		Utils.RegisterFunc(L, -3, "OnStart", _m_OnStart);
		Utils.RegisterFunc(L, -3, "CSOpen", _m_CSOpen);
		Utils.RegisterFunc(L, -3, "CSClose", _m_CSClose);
		Utils.RegisterFunc(L, -3, "GetLogoAnimLength", _m_GetLogoAnimLength);
		Utils.RegisterFunc(L, -3, "FetchNoticeData", _m_FetchNoticeData);
		Utils.RegisterFunc(L, -3, "ShowAccountSelect", _m_ShowAccountSelect);
		Utils.RegisterFunc(L, -3, "ShowNewGame", _m_ShowNewGame);
		Utils.RegisterFunc(L, -3, "RegistNewGameAction", _m_RegistNewGameAction);
		Utils.RegisterFunc(L, -3, "RegistSelectAccountAction", _m_RegistSelectAccountAction);
		Utils.RegisterFunc(L, -2, "versionText", _g_get_versionText);
		Utils.RegisterFunc(L, -2, "background", _g_get_background);
		Utils.RegisterFunc(L, -2, "videoPlayer", _g_get_videoPlayer);
		Utils.RegisterFunc(L, -2, "animator", _g_get_animator);
		Utils.RegisterFunc(L, -2, "progressBar", _g_get_progressBar);
		Utils.RegisterFunc(L, -2, "loadingText", _g_get_loadingText);
		Utils.RegisterFunc(L, -2, "tipText", _g_get_tipText);
		Utils.RegisterFunc(L, -2, "downloadText", _g_get_downloadText);
		Utils.RegisterFunc(L, -2, "logoImage", _g_get_logoImage);
		Utils.RegisterFunc(L, -2, "wifiObj", _g_get_wifiObj);
		Utils.RegisterFunc(L, -2, "btnNotice", _g_get_btnNotice);
		Utils.RegisterFunc(L, -2, "txtNotice", _g_get_txtNotice);
		Utils.RegisterFunc(L, -2, "btnServe", _g_get_btnServe);
		Utils.RegisterFunc(L, -2, "txtServe", _g_get_txtServe);
		Utils.RegisterFunc(L, -2, "serverRedDot", _g_get_serverRedDot);
		Utils.RegisterFunc(L, -2, "serverRedDotCount", _g_get_serverRedDotCount);
		Utils.RegisterFunc(L, -2, "btnHelp", _g_get_btnHelp);
		Utils.RegisterFunc(L, -2, "txtHelp", _g_get_txtHelp);
		Utils.RegisterFunc(L, -2, "maintenanceDialog", _g_get_maintenanceDialog);
		Utils.RegisterFunc(L, -2, "maintenanceDialogTitle", _g_get_maintenanceDialogTitle);
		Utils.RegisterFunc(L, -2, "maintenanceDialogContent", _g_get_maintenanceDialogContent);
		Utils.RegisterFunc(L, -2, "maintenanceBtn", _g_get_maintenanceBtn);
		Utils.RegisterFunc(L, -2, "maintenanceBtnText", _g_get_maintenanceBtnText);
		Utils.RegisterFunc(L, -2, "handleSliderArea", _g_get_handleSliderArea);
		Utils.RegisterFunc(L, -2, "koreanDecorate", _g_get_koreanDecorate);
		Utils.RegisterFunc(L, -2, "subLoadingContainer", _g_get_subLoadingContainer);
		Utils.RegisterFunc(L, -2, "btnAccountSelect", _g_get_btnAccountSelect);
		Utils.RegisterFunc(L, -2, "btnNewGame", _g_get_btnNewGame);
		Utils.RegisterFunc(L, -2, "txtBtnLogin", _g_get_txtBtnLogin);
		Utils.RegisterFunc(L, -2, "txtBtnNewGame", _g_get_txtBtnNewGame);
		Utils.RegisterFunc(L, -2, "btnClear", _g_get_btnClear);
		Utils.RegisterFunc(L, -2, "btnClearText", _g_get_btnClearText);
		Utils.RegisterFunc(L, -2, "clearConfirm", _g_get_clearConfirm);
		Utils.RegisterFunc(L, -2, "clearConfirmTitle", _g_get_clearConfirmTitle);
		Utils.RegisterFunc(L, -2, "clearConfirmContent", _g_get_clearConfirmContent);
		Utils.RegisterFunc(L, -2, "btnClearConfirmOk", _g_get_btnClearConfirmOk);
		Utils.RegisterFunc(L, -2, "btnClearConfirmNo", _g_get_btnClearConfirmNo);
		Utils.RegisterFunc(L, -2, "btnClearConfirmOk_Text", _g_get_btnClearConfirmOk_Text);
		Utils.RegisterFunc(L, -2, "btnClearConfirmNo_Text", _g_get_btnClearConfirmNo_Text);
		Utils.RegisterFunc(L, -2, "safeModeObj", _g_get_safeModeObj);
		Utils.RegisterFunc(L, -1, "versionText", _s_set_versionText);
		Utils.RegisterFunc(L, -1, "background", _s_set_background);
		Utils.RegisterFunc(L, -1, "videoPlayer", _s_set_videoPlayer);
		Utils.RegisterFunc(L, -1, "animator", _s_set_animator);
		Utils.RegisterFunc(L, -1, "progressBar", _s_set_progressBar);
		Utils.RegisterFunc(L, -1, "loadingText", _s_set_loadingText);
		Utils.RegisterFunc(L, -1, "tipText", _s_set_tipText);
		Utils.RegisterFunc(L, -1, "downloadText", _s_set_downloadText);
		Utils.RegisterFunc(L, -1, "logoImage", _s_set_logoImage);
		Utils.RegisterFunc(L, -1, "wifiObj", _s_set_wifiObj);
		Utils.RegisterFunc(L, -1, "btnNotice", _s_set_btnNotice);
		Utils.RegisterFunc(L, -1, "txtNotice", _s_set_txtNotice);
		Utils.RegisterFunc(L, -1, "btnServe", _s_set_btnServe);
		Utils.RegisterFunc(L, -1, "txtServe", _s_set_txtServe);
		Utils.RegisterFunc(L, -1, "serverRedDot", _s_set_serverRedDot);
		Utils.RegisterFunc(L, -1, "serverRedDotCount", _s_set_serverRedDotCount);
		Utils.RegisterFunc(L, -1, "btnHelp", _s_set_btnHelp);
		Utils.RegisterFunc(L, -1, "txtHelp", _s_set_txtHelp);
		Utils.RegisterFunc(L, -1, "maintenanceDialog", _s_set_maintenanceDialog);
		Utils.RegisterFunc(L, -1, "maintenanceDialogTitle", _s_set_maintenanceDialogTitle);
		Utils.RegisterFunc(L, -1, "maintenanceDialogContent", _s_set_maintenanceDialogContent);
		Utils.RegisterFunc(L, -1, "maintenanceBtn", _s_set_maintenanceBtn);
		Utils.RegisterFunc(L, -1, "maintenanceBtnText", _s_set_maintenanceBtnText);
		Utils.RegisterFunc(L, -1, "handleSliderArea", _s_set_handleSliderArea);
		Utils.RegisterFunc(L, -1, "koreanDecorate", _s_set_koreanDecorate);
		Utils.RegisterFunc(L, -1, "subLoadingContainer", _s_set_subLoadingContainer);
		Utils.RegisterFunc(L, -1, "btnAccountSelect", _s_set_btnAccountSelect);
		Utils.RegisterFunc(L, -1, "btnNewGame", _s_set_btnNewGame);
		Utils.RegisterFunc(L, -1, "txtBtnLogin", _s_set_txtBtnLogin);
		Utils.RegisterFunc(L, -1, "txtBtnNewGame", _s_set_txtBtnNewGame);
		Utils.RegisterFunc(L, -1, "btnClear", _s_set_btnClear);
		Utils.RegisterFunc(L, -1, "btnClearText", _s_set_btnClearText);
		Utils.RegisterFunc(L, -1, "clearConfirm", _s_set_clearConfirm);
		Utils.RegisterFunc(L, -1, "clearConfirmTitle", _s_set_clearConfirmTitle);
		Utils.RegisterFunc(L, -1, "clearConfirmContent", _s_set_clearConfirmContent);
		Utils.RegisterFunc(L, -1, "btnClearConfirmOk", _s_set_btnClearConfirmOk);
		Utils.RegisterFunc(L, -1, "btnClearConfirmNo", _s_set_btnClearConfirmNo);
		Utils.RegisterFunc(L, -1, "btnClearConfirmOk_Text", _s_set_btnClearConfirmOk_Text);
		Utils.RegisterFunc(L, -1, "btnClearConfirmNo_Text", _s_set_btnClearConfirmNo_Text);
		Utils.RegisterFunc(L, -1, "safeModeObj", _s_set_safeModeObj);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "IsSystemLanguageSimplifiedChinese", _m_IsSystemLanguageSimplifiedChinese_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetWeightedRandomIndex", _m_GetWeightedRandomIndex_xlua_st_);
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
				UILoadingComponent o = new UILoadingComponent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UILoadingComponent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnStart(IntPtr L)
	{
		try
		{
			((UILoadingComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnStart();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CSOpen(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<object>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				object userData = objectTranslator.GetObject(L, 2, typeof(object));
				bool initLuaState = Lua.lua_toboolean(L, 3);
				uILoadingComponent.CSOpen(userData, initLuaState);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object userData2 = objectTranslator.GetObject(L, 2, typeof(object));
				uILoadingComponent.CSOpen(userData2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UILoadingComponent.CSOpen!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSystemLanguageSimplifiedChinese_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = UILoadingComponent.IsSystemLanguageSimplifiedChinese();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWeightedRandomIndex_xlua_st_(IntPtr L)
	{
		try
		{
			int weightedRandomIndex = UILoadingComponent.GetWeightedRandomIndex((int[])ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(int[])));
			Lua.xlua_pushinteger(L, weightedRandomIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CSClose(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			object userData = objectTranslator.GetObject(L, 2, typeof(object));
			uILoadingComponent.CSClose(userData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLogoAnimLength(IntPtr L)
	{
		try
		{
			float logoAnimLength = ((UILoadingComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLogoAnimLength();
			Lua.lua_pushnumber(L, logoAnimLength);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FetchNoticeData(IntPtr L)
	{
		try
		{
			((UILoadingComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FetchNoticeData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowAccountSelect(IntPtr L)
	{
		try
		{
			UILoadingComponent obj = (UILoadingComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool show = Lua.lua_toboolean(L, 2);
			obj.ShowAccountSelect(show);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowNewGame(IntPtr L)
	{
		try
		{
			UILoadingComponent obj = (UILoadingComponent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool show = Lua.lua_toboolean(L, 2);
			obj.ShowNewGame(show);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegistNewGameAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			UnityAction action = objectTranslator.GetDelegate<UnityAction>(L, 2);
			uILoadingComponent.RegistNewGameAction(action);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegistSelectAccountAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			UnityAction action = objectTranslator.GetDelegate<UnityAction>(L, 2);
			uILoadingComponent.RegistSelectAccountAction(action);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_versionText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.versionText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_background(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.background);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_videoPlayer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.videoPlayer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.animator);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_progressBar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.progressBar);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loadingText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.loadingText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tipText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.tipText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_downloadText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.downloadText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_logoImage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.logoImage);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_wifiObj(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.wifiObj);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnNotice(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnNotice);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_txtNotice(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.txtNotice);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnServe(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnServe);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_txtServe(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.txtServe);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serverRedDot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.serverRedDot);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serverRedDotCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.serverRedDotCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnHelp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnHelp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_txtHelp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.txtHelp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maintenanceDialog(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.maintenanceDialog);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maintenanceDialogTitle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.maintenanceDialogTitle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maintenanceDialogContent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.maintenanceDialogContent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maintenanceBtn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.maintenanceBtn);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maintenanceBtnText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.maintenanceBtnText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_handleSliderArea(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.handleSliderArea);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_koreanDecorate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.koreanDecorate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_subLoadingContainer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.subLoadingContainer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnAccountSelect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnAccountSelect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnNewGame(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnNewGame);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_txtBtnLogin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.txtBtnLogin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_txtBtnNewGame(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.txtBtnNewGame);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnClear(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnClear);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnClearText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnClearText);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clearConfirm(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.clearConfirm);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clearConfirmTitle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.clearConfirmTitle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clearConfirmContent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.clearConfirmContent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnClearConfirmOk(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnClearConfirmOk);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnClearConfirmNo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnClearConfirmNo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnClearConfirmOk_Text(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnClearConfirmOk_Text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_btnClearConfirmNo_Text(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.btnClearConfirmNo_Text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_safeModeObj(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UILoadingComponent uILoadingComponent = (UILoadingComponent)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, uILoadingComponent.safeModeObj);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_versionText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).versionText = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_background(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).background = (Image)objectTranslator.GetObject(L, 2, typeof(Image));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_videoPlayer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).videoPlayer = (VideoPlayer)objectTranslator.GetObject(L, 2, typeof(VideoPlayer));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).animator = (Animator)objectTranslator.GetObject(L, 2, typeof(Animator));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_progressBar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).progressBar = (Slider)objectTranslator.GetObject(L, 2, typeof(Slider));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loadingText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).loadingText = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tipText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).tipText = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_downloadText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).downloadText = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_logoImage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).logoImage = (Image)objectTranslator.GetObject(L, 2, typeof(Image));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_wifiObj(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).wifiObj = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnNotice(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnNotice = (Button)objectTranslator.GetObject(L, 2, typeof(Button));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_txtNotice(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).txtNotice = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnServe(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnServe = (Button)objectTranslator.GetObject(L, 2, typeof(Button));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_txtServe(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).txtServe = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serverRedDot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).serverRedDot = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serverRedDotCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).serverRedDotCount = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnHelp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnHelp = (Button)objectTranslator.GetObject(L, 2, typeof(Button));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_txtHelp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).txtHelp = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maintenanceDialog(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).maintenanceDialog = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maintenanceDialogTitle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).maintenanceDialogTitle = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maintenanceDialogContent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).maintenanceDialogContent = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maintenanceBtn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).maintenanceBtn = (Button)objectTranslator.GetObject(L, 2, typeof(Button));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maintenanceBtnText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).maintenanceBtnText = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_handleSliderArea(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).handleSliderArea = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_koreanDecorate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).koreanDecorate = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_subLoadingContainer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).subLoadingContainer = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnAccountSelect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnAccountSelect = (Button)objectTranslator.GetObject(L, 2, typeof(Button));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnNewGame(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnNewGame = (Button)objectTranslator.GetObject(L, 2, typeof(Button));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_txtBtnLogin(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).txtBtnLogin = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_txtBtnNewGame(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).txtBtnNewGame = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnClear(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnClear = (Button)objectTranslator.GetObject(L, 2, typeof(Button));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnClearText(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnClearText = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clearConfirm(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).clearConfirm = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clearConfirmTitle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).clearConfirmTitle = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clearConfirmContent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).clearConfirmContent = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnClearConfirmOk(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnClearConfirmOk = (Button)objectTranslator.GetObject(L, 2, typeof(Button));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnClearConfirmNo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnClearConfirmNo = (Button)objectTranslator.GetObject(L, 2, typeof(Button));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnClearConfirmOk_Text(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnClearConfirmOk_Text = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_btnClearConfirmNo_Text(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).btnClearConfirmNo_Text = (TextMeshProUGUIEx)objectTranslator.GetObject(L, 2, typeof(TextMeshProUGUIEx));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_safeModeObj(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UILoadingComponent)objectTranslator.FastGetCSObj(L, 1)).safeModeObj = (UILoadingComponentSafeMode)objectTranslator.GetObject(L, 2, typeof(UILoadingComponentSafeMode));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
