using System.Text;
using UnityEngine;

public class KRAuthState : LoadingStateBase
{
	private bool _inSwitchAccount;

	private float _switchAccountTime;

	private float _switchAccountTimeout = 2f;

	private string _baseUrl = "";

	private string _authToken = "";

	private string _authErrorCode = "";

	private readonly string[] _switchAccountFlowWindowNames = new string[8] { "UIChooseSwitchAccount", "UIAddAccount", "UIAccountVerify", "UIRoleCreate", "UIRoles", "UIRoleLogin", "UIChooseServerView", "UILoginConfirm" };

	public KRAuthState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_inSwitchAccount = false;
		_baseUrl = "";
		_authToken = "";
		_authErrorCode = "";
		if (args.Length != 0)
		{
			_baseUrl = args[0].ToString();
		}
		if (args.Length > 1)
		{
			_authToken = args[1].ToString();
		}
		if (args.Length > 2)
		{
			_authErrorCode = args[2].ToString();
		}
		DoKRRealNameAuthProcess();
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		if (!_inSwitchAccount)
		{
			return;
		}
		float time = Time.time;
		bool flag = false;
		string[] switchAccountFlowWindowNames = _switchAccountFlowWindowNames;
		foreach (string uiName in switchAccountFlowWindowNames)
		{
			if (GameEntry.Lua.UIManager.IsWindowOpen(uiName))
			{
				flag = true;
				_switchAccountTime = time;
				break;
			}
		}
		if (time - _switchAccountTime > _switchAccountTimeout)
		{
			_switchAccountTime = time;
			if (!flag)
			{
				DoKRRealNameAuthProcess();
			}
		}
	}

	private void DoKRRealNameAuthProcess()
	{
		_inSwitchAccount = false;
		_switchAccountTime = Time.time;
		if (string.IsNullOrEmpty(_authErrorCode))
		{
			GameEntry.Lua.ShowMessage(GameEntry.Localization.GetString("korea_verify2"), 1, "korea_verify3", "korea_verify4", delegate
			{
				OpenKRRealNameAuth();
			}, delegate
			{
				GameEntry.Lua.UIManager.OpenWindow("UIChooseSwitchAccount");
				_inSwitchAccount = true;
			}, delegate
			{
				DoKRRealNameAuthProcess();
			}, "korea_verify1", isChangeImg: false);
		}
		else
		{
			GameEntry.Lua.ShowMessage(GameEntry.Localization.GetString("korea_verify9"), 0, "korea_verify4", "", delegate
			{
				GameEntry.Lua.UIManager.OpenWindow("UIChooseSwitchAccount");
				_inSwitchAccount = true;
			}, null, delegate
			{
				DoKRRealNameAuthProcess();
			}, "korea_verify1", isChangeImg: false);
		}
	}

	private void OpenKRRealNameAuth()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_baseUrl);
		stringBuilder.Append("?uid=");
		stringBuilder.Append(GameEntry.Network.Uid);
		stringBuilder.Append("&token=");
		stringBuilder.Append(_authToken);
		Application.OpenURL(stringBuilder.ToString());
		OpenKRRealNameAuthFinish();
	}

	private void OpenKRRealNameAuthFinish()
	{
		GameEntry.Lua.ShowMessage(GameEntry.Localization.GetString("korea_verify5"), 3, "korea_verify6", "korea_verify7", delegate
		{
			ApplicationLaunch.Instance.ReloadGame();
		}, delegate
		{
			ApplicationLaunch.Instance.ReloadGame();
		}, delegate
		{
			OpenKRRealNameAuthFinish();
		}, "korea_verify1", isChangeImg: false);
	}
}
