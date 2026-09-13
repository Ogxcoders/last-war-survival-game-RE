using System.Collections.Generic;
using GameFramework;
using Sfs2X.Entities.Data;
using UnityEngine;

public class LoginState : LoadingStateBase
{
	private float _elapseTime;

	private float _timeout = 7f;

	private bool _onResponse;

	public LoginState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		PostEventLog.TrackMap("login_state", SDKManager.AddBILaunchTimeProperty(new Dictionary<string, object> { 
		{
			"errMsg",
			GameEntry.Network.getCurLine() ?? ""
		} }));
		_elapseTime = 0f;
		_onResponse = false;
		PostEventLog.Record("LOGIN_START");
		LoginMessage.Instance.onLoginResponse = OnLoginResponse;
		LoginMessage.Instance.Send(GameEntry.Network.Uid, "", GameEntry.Network.ZoneName, 0);
		GameEntry.Event.Fire(EventId.BeforeLoginMsgSend);
		Log.Info("LoginState::Send login " + GameEntry.Network.Uid + "," + GameEntry.Network.ZoneName + " line:" + GameEntry.Network.getCurLine());
		DynamicAtlasManager.Instance.DoSelfCheck();
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		if (_onResponse)
		{
			return;
		}
		_elapseTime += Time.deltaTime;
		if (_elapseTime > _timeout)
		{
			if (_startupLoading.LoginTryCount < 3)
			{
				GameEntry.GlobalData.gameLineBlackList.Add(GameEntry.Network.getCurLine());
				PostEventLog.TrackMap("login_retry", new Dictionary<string, object> { 
				{
					"errMsg",
					GameEntry.Network.getCurLine() ?? ""
				} });
				_startupLoading.StartConnect();
			}
			else
			{
				PostEventLog.TrackMap("login_timeout", new Dictionary<string, object> { 
				{
					"errMsg",
					GameEntry.Network.getCurLine() ?? ""
				} });
				GameEntry.GlobalData.gameLineBlackList.Clear();
				_startupLoading.SetState(LoadingState.LoadingError, "E113");
			}
		}
	}

	private void OnLoginResponse(ISFSObject loginMessage)
	{
		_onResponse = true;
		if (loginMessage.ContainsKey("errorMessage"))
		{
			string utfString = loginMessage.GetUtfString("errorMessage");
			string loginErrorCode = "E117";
			object errorParam = null;
			switch (utfString)
			{
			case "E001":
			case "E002":
				loginErrorCode = "E001";
				break;
			case "E005":
				loginErrorCode = "E109";
				break;
			case "E010":
				UIUtils.ShowMessage(GameEntry.Localization.GetString("delete_account_content_14"), delegate
				{
					ApplicationLaunch.Instance.ReloadGame();
				});
				loginErrorCode = "E010";
				break;
			case "E011":
				loginErrorCode = "E130";
				Log.Info("[AT]ClearAT_E011");
				_startupLoading.ClearAccessToken();
				break;
			case "E013":
				loginErrorCode = "E013";
				break;
			case "E014":
				loginErrorCode = "E014";
				break;
			case "E015":
				loginErrorCode = "E015";
				break;
			case "E016":
				loginErrorCode = "E016";
				break;
			default:
				if (utfString.Contains(";"))
				{
					OnErrorMessageSemicolonSplit(utfString, ref loginErrorCode, ref errorParam);
				}
				break;
			}
			_startupLoading.SetState(LoadingState.LoadingError, loginErrorCode, errorParam);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("err_code", utfString);
			PostEventLog.TrackMap("login_fail", dictionary);
		}
		else if (GameEntry.GlobalData.updateType == 2)
		{
			_startupLoading.SetState(LoadingState.AppUpdate);
		}
		else
		{
			GameEntry.GlobalData.gameLineBlackList.Clear();
			_startupLoading.SetState(LoadingState.PushInit, loginMessage);
		}
	}

	private void OnErrorMessageSemicolonSplit(string errorCode, ref string loginErrorCode, ref object errorParam)
	{
		string[] array = errorCode.Split(new char[1] { ';' });
		switch (array[0])
		{
		case "4":
			loginErrorCode = "4";
			if (array.Length >= 4)
			{
				errorParam = errorCode;
			}
			break;
		case "45":
			loginErrorCode = "45";
			break;
		case "46":
			loginErrorCode = "46";
			break;
		case "47":
			loginErrorCode = "47";
			break;
		case "48":
			GameEntry.Lua.Call("CSharpCallLuaInterface.OpenDelAccountSuccessView");
			loginErrorCode = "48";
			break;
		case "49":
			loginErrorCode = "49";
			errorParam = array[1];
			break;
		case "E012":
			loginErrorCode = "E012";
			errorParam = "";
			if (array.Length > 1)
			{
				errorParam = string.Join(";", array, 1, array.Length - 1);
			}
			break;
		}
	}
}
