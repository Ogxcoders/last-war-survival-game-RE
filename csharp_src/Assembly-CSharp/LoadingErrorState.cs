using System.Collections.Generic;
using AIHelp;
using GameFramework;
using UnityEngine;

public class LoadingErrorState : LoadingStateBase
{
	public LoadingErrorState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		GameEntry.Event.Fire(EventId.CloseDisconnectView);
		string text = (string)args[0];
		if (text != "E000" && text != "E999")
		{
			Log.Error("Loading error : {0}", text);
			PostEventLog.TrackMap("loading_error_state", SDKManager.AddBILaunchTimeProperty(new Dictionary<string, object> { { "errMsg", text } }));
		}
		string text2 = "";
		switch (text)
		{
		case "error connect lost":
		case "E101":
		case "E102":
		case "E105":
		case "E106":
		case "E107":
		case "E108":
		case "E112":
		case "E113":
		case "E114":
		case "E118":
		case "E119":
			text2 = "login_error_connectFail";
			break;
		case "error force update":
		{
			string text5 = "Google Play";
			string mdownLoadUrl = null;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				text5 = "AppStore";
			}
			string message = GameEntry.Localization.GetString("129013", text5);
			if (args.Length >= 2)
			{
				mdownLoadUrl = (string)args[1];
			}
			UIUtils.ShowMessage(message, 1, "110003", "", delegate
			{
				if (!mdownLoadUrl.IsNullOrEmpty())
				{
					Application.OpenURL(mdownLoadUrl);
				}
			}, delegate
			{
				if (!mdownLoadUrl.IsNullOrEmpty())
				{
					Application.OpenURL(mdownLoadUrl);
				}
			});
			break;
		}
		case "E103":
		case "E104":
		case "E111":
			text2 = "login_error_fileErr";
			break;
		case "E117":
		case "E120":
			text2 = "login_error_accountErr";
			break;
		case "E121":
		case "E122":
			text2 = "login_error_updateFail";
			break;
		case "E116":
			text2 = "login_error_serverErr";
			break;
		case "E124":
		{
			string text4 = GameEntry.Localization.GetString("129063") + "\n" + text;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				text4 = text4 + "\n" + GameEntry.Localization.GetString("login_alert_tips_1001");
			}
			UIUtils.ShowMessage3Action(text4, "129091", "280014", delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, delegate
			{
				AIHelpProxy.Show("E005", GameEntry.Localization.GetString("2700006"));
			}, delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, isChangeImg: false);
			break;
		}
		case "E116_201":
			UIUtils.ShowMessage(GameEntry.Localization.GetString("not_available_description_01", text), delegate
			{
				ApplicationLaunch.Instance.Quit();
			});
			break;
		case "E109":
			UIUtils.ShowMessage(GameEntry.Localization.GetString("login_err_tips_maintenance_new"), delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, delegate
			{
				ApplicationLaunch.Instance.Quit();
			});
			break;
		case "E115":
			UIUtils.ShowMessage3Action(GetTipsMessage(text, "129014"), "129091", "280014", delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, delegate
			{
				AIHelpProxy.Show("E005", GameEntry.Localization.GetString("2700006"));
			}, delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, isChangeImg: false);
			break;
		case "E123":
			UIUtils.ShowMessage3Action(GetTipsMessage(text, "login_100001_tip"), "110006", "280014", delegate
			{
				ApplicationLaunch.Instance.Quit();
			}, delegate
			{
				AIHelpProxy.Show("E005", GameEntry.Localization.GetString("2700006"));
			}, delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, isChangeImg: false);
			break;
		case "E999":
			PlayerPrefs.Save();
			if (SDKManager.IS_UNITY_ANDROID())
			{
				ApplicationLaunch.Instance.ReloadGameInOtherProcess();
				break;
			}
			UIUtils.ShowMessage(GameEntry.Localization.GetString("458290"), 1, "110006", "", delegate
			{
				ApplicationLaunch.Instance.ReloadGameInOtherProcess();
			}, delegate
			{
				ApplicationLaunch.Instance.ReloadGameInOtherProcess();
			});
			break;
		case "E001":
			ApplicationLaunch.Instance.Loading.ClearGameServerSetting(clearZone: true);
			ApplicationLaunch.Instance.ReloadGame();
			break;
		case "4":
		{
			string text3 = null;
			if (args.Length >= 2)
			{
				text3 = (string)args[1];
			}
			if (text3.IsNullOrEmpty())
			{
				UIUtils.ShowMessage(GameEntry.Localization.GetString("E100085"), 1, null);
				return;
			}
			GameEntry.Lua.UIManager.OpenWindow("UIAccountBan", text3);
			break;
		}
		case "45":
			GameEntry.Lua.UIManager.OpenWindow("UIDelAllAcctUnderReview");
			break;
		case "46":
			GameEntry.Lua.UIManager.OpenWindow("UIDelAllAcctReviewEnded");
			break;
		case "47":
			GameEntry.Lua.UIManager.OpenWindow("UIDelAllAcctReviewNotPassed");
			break;
		case "49":
			UIUtils.ShowMessage(GameEntry.Localization.GetString("traffic_control_ban_tips", (string)args[1]), 1, null);
			break;
		case "E900":
			UIUtils.ShowMessage(GetTipsMessage(text, "install_tips_capacity"), 1, "110006", "", delegate
			{
				ApplicationLaunch.Instance.Quit();
			}, delegate
			{
				ApplicationLaunch.Instance.Quit();
			});
			break;
		case "E901":
			UIUtils.ShowMessage(GetTipsMessage(text, "plugin_error_tips"), 1, "110006", "", delegate
			{
				ApplicationLaunch.Instance.Quit();
			}, delegate
			{
				ApplicationLaunch.Instance.Quit();
			});
			break;
		case "E130":
			UIUtils.ShowMessage(GetTipsMessage(text, "account_error_tips"), 1, "129091", "", delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, delegate
			{
				ApplicationLaunch.Instance.Quit();
			});
			break;
		case "E902":
			UIUtils.ShowMessage(GetTipsMessage(text, "error_tips_disk_full"), 1, "110006", "", delegate
			{
				ApplicationLaunch.Instance.Quit();
			}, delegate
			{
				ApplicationLaunch.Instance.Quit();
			});
			break;
		case "E213":
			UIUtils.ShowMessage(GetTipsMessage(text, "129063"), 1, "120952", "", delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			});
			break;
		case "E012":
		{
			string text6 = "";
			string text7 = "";
			string text8 = "";
			if (args.Length >= 2)
			{
				string text9 = (string)args[1];
				string[] array = text9.Split(new char[1] { ';' });
				if (array.Length >= 3)
				{
					text6 = array[0];
					text7 = array[1];
					text8 = array[2];
				}
				else if (array.Length >= 2)
				{
					text6 = array[0];
					text7 = array[1];
				}
				else
				{
					text6 = text9;
				}
			}
			_startupLoading.SetState(LoadingState.KRAuth, text6, text7, text8);
			break;
		}
		case "E013":
			UIUtils.ShowMessage(GetTipsMessage(text, "season_creat_role_tips"), 1, "110006", "", delegate
			{
				Log.Info("[AT]ClearUserData_E013");
				CleanUserDataAndReloadGame();
			}, delegate
			{
				Log.Info("[AT]ClearUserData_E013");
				CleanUserDataAndReloadGame();
			});
			break;
		case "E015":
			UIUtils.ShowMessage(GameEntry.Localization.GetString("season_open_tips01", text), delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, delegate
			{
				ApplicationLaunch.Instance.Quit();
			});
			break;
		case "E016":
			UIUtils.ShowMessage(GameEntry.Localization.GetString("season_close_tips01", text), delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, delegate
			{
				ApplicationLaunch.Instance.Quit();
			});
			break;
		case "E014":
			Log.Info("[AT]ClearUserData_E014");
			CleanUserDataAndReloadGame();
			break;
		}
		if (!string.IsNullOrEmpty(text2))
		{
			UIUtils.ShowMessage3Action(GameEntry.Localization.GetString(text2, text), "129091", "280014", delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, delegate
			{
				AIHelpProxy.Show("E005", GameEntry.Localization.GetString("2700006"));
			}, delegate
			{
				ApplicationLaunch.Instance.ReloadGame();
			}, isChangeImg: false);
		}
	}

	private void CleanUserDataAndReloadGame()
	{
		_startupLoading.ClearAllAccountSettings();
		_startupLoading.ClearAccessToken();
		_startupLoading.ClearRefreshToken();
		_startupLoading.ClearLoginKey();
		GameEntry.Setting.Save();
		ApplicationLaunch.Instance.ReloadGame();
	}

	private string GetTipsMessage(string errCode, string msgKey)
	{
		return GameEntry.Localization.GetString(msgKey) + "\n" + errCode;
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}
}
