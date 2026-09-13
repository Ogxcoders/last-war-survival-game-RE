using GameFramework;
using Sfs2X.Entities.Data;
using UnityEngine;

public class CrossServerLogin : FsmBaseState
{
	private float _elapseTime;

	private float _timeout = 7f;

	public override int id => 4;

	public CrossServerLogin(CrossServerFsmManager mgr)
		: base(mgr)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_elapseTime = 0f;
		ApplicationLaunch.Instance.Loading.IsPushInitReceived = false;
		LoginMessage.Instance.onLoginResponse = OnLoginResponse;
		LoginMessage.Instance.Send(GameEntry.Network.Uid, "", GameEntry.Network.ZoneName, 0);
		Log.Info("LoginState::Send login " + GameEntry.Network.Uid + "," + GameEntry.Network.ZoneName + " line:" + GameEntry.Network.getCurLine());
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		_elapseTime += Time.deltaTime;
		if (_elapseTime > _timeout)
		{
			GotoLoadingError(10);
		}
	}

	private void OnLoginResponse(ISFSObject loginMessage)
	{
		bool flag = false;
		if (!loginMessage.ContainsKey("errorMessage") && GameEntry.GlobalData.updateType != 2)
		{
			flag = true;
			_mgr.SetState(5);
		}
		if (!flag)
		{
			GotoLoadingError(9);
		}
		LoginMessage.Instance.onLoginResponse = null;
	}
}
