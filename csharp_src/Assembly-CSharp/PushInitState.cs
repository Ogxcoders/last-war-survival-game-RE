using System.Collections.Generic;
using GameFramework;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityGameFramework.SDK;

public class PushInitState : LoadingStateBase
{
	private const float PushInitTimeOut = 13f;

	private float _elapseTime;

	public PushInitState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_elapseTime = 0f;
		PostEventLog.TrackMap("pushinit_state", SDKManager.AddBILaunchTimeProperty());
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		_elapseTime += Time.deltaTime;
		if (!_startupLoading.IsPushInitReceived)
		{
			if (_elapseTime > 13f)
			{
				PostEventLog.Record("LONG_TIME_NOT_PUSH_INIT");
				if (_startupLoading.LoginTryCount < 3)
				{
					_startupLoading.StartConnect();
					return;
				}
				_startupLoading.SetState(LoadingState.LoadingError, "E107");
			}
		}
		else if (_startupLoading.IsNeedIdentification)
		{
			_startupLoading.SetState(LoadingState.CNIdentify);
		}
		else if (OnLoginComplete())
		{
			_startupLoading.SetState(LoadingState.LoadScene);
		}
	}

	private bool OnLoginComplete()
	{
		if ((!(GameEntry.Data?.Player?.CheckSwitch("refund_client2", defaultVal: false))) ?? true)
		{
			if (GameEntry.Lua.CallWithReturn<long>("CSharpCallLuaInterface.GetCreditValue") < 0)
			{
				_startupLoading.SetState(LoadingState.CreditLimit);
				return false;
			}
		}
		else if (!GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.RefundPunishCanLogIn"))
		{
			_startupLoading.SetState(LoadingState.CreditLimit);
			return false;
		}
		int mainLv = GameEntry.Data.Building.GetMainLv();
		bool flag = GameEntry.Setting.CheckFirstLaunchSkipUpdate();
		Log.Info($"PushInitState.OnLoginComplete FirstLaunch skip : {flag}");
		if (flag)
		{
			int num = GameEntry.Lua.CallWithReturn<int, string, string>("CSharpCallLuaInterface.GetConfigNum", "first_launch_skip_update", "k2");
			Log.Info($"PushInitState.OnLoginComplete FirstLaunch mainLv : {mainLv} ; ignoreMainLv : {num}");
			string text;
			if (num <= mainLv)
			{
				GameEntry.Setting.DisableFirstLaunchSkipUpdate();
				text = $"PushInitState.OnLoginComplete FirstLaunchUpdate force ! mainLv : {mainLv}; targetMainLv : {num};";
				Log.Info(text);
				PostEventLog.TrackMap("FirstLaunchSkipUpdate", new Dictionary<string, object>
				{
					{ "status", false },
					{ "reason", text },
					{
						"lw_res_version",
						GameEntry.Resource.GetResVersion()
					}
				});
				if (!GameEntry.Setting.IsReview)
				{
					ApplicationLaunch.Instance.ReStartGame();
					return false;
				}
			}
			bool flag2 = false;
			SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
			if (curSkinMeta == null || !curSkinMeta.IsNotSeason())
			{
				flag2 = true;
			}
			Log.Info($"PushInitState.OnLoginComplete FirstLaunch isInSeason : {flag2}");
			if (flag2)
			{
				GameEntry.Setting.DisableFirstLaunchSkipUpdate();
				text = "PushInitState.OnLoginComplete FirstLaunchUpdate force ! isInSeason";
				Log.Info(text);
				PostEventLog.TrackMap("FirstLaunchSkipUpdate", new Dictionary<string, object>
				{
					{ "status", false },
					{ "reason", text },
					{
						"lw_res_version",
						GameEntry.Resource.GetResVersion()
					}
				});
				if (!GameEntry.Setting.IsReview)
				{
					ApplicationLaunch.Instance.ReStartGame();
					return false;
				}
			}
			GameEntry.Setting.FirstLaunchSkipUpdateRunning();
			text = "PushInitState.OnLoginComplete FirstLaunchSkipUpdateRunning !";
			Log.Info(text);
			PostEventLog.TrackMap("FirstLaunchSkipUpdate", new Dictionary<string, object>
			{
				{ "status", true },
				{ "reason", text },
				{
					"lw_res_version",
					GameEntry.Resource.GetResVersion()
				}
			});
		}
		PostEventLog.Record("LOGIN_COMPLETE");
		GameEntry.Network.SyncPingPong();
		HelpManager.Instance.setUserData();
		GameEntry.Lua.DataCenterInit();
		GameEntry.Event.Fire(EventId.CloseDisconnectView);
		if (SceneManager.World != null)
		{
			SceneManager.World.CreateScene();
		}
		if (mainLv < 4)
		{
			int val = 172800;
			ISFSObject iSFSObject = SFSObject.NewInstance();
			iSFSObject.PutInt("id", 20151020);
			iSFSObject.PutInt("time", val);
			iSFSObject.PutUtfString("title", GameEntry.Localization.GetString("350011"));
			iSFSObject.PutUtfString("content", "");
			iSFSObject.PutInt("type", 99);
			GameEntry.Event.Fire(EventId.PUSH_NOTICE, iSFSObject);
		}
		else
		{
			int val2 = 172800;
			ISFSObject iSFSObject2 = SFSObject.NewInstance();
			iSFSObject2.PutInt("id", 20151020);
			iSFSObject2.PutInt("time", val2);
			iSFSObject2.PutUtfString("title", GameEntry.Localization.GetString("120113"));
			iSFSObject2.PutUtfString("content", "");
			iSFSObject2.PutInt("type", 99);
			GameEntry.Event.Fire(EventId.PUSH_NOTICE, iSFSObject2);
		}
		for (int i = 3; i <= 6; i++)
		{
			int val3 = 86400 * i;
			ISFSObject iSFSObject3 = SFSObject.NewInstance();
			iSFSObject3.PutInt("id", 20151020 + i);
			iSFSObject3.PutInt("time", val3);
			iSFSObject3.PutUtfString("title", GameEntry.Localization.GetString("350012"));
			iSFSObject3.PutUtfString("content", "");
			iSFSObject3.PutInt("type", 99);
			GameEntry.Event.Fire(EventId.PUSH_NOTICE, iSFSObject3);
		}
		GameEntry.Setting.GetString("Setting.GAME_UID", "");
		return true;
	}
}
