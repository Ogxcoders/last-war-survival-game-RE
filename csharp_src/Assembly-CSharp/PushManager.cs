using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class PushManager
{
	private bool connected;

	private string platform;

	private string userId;

	private string channelId;

	private string parseId;

	private bool openGiftPage;

	private bool initFlag = true;

	private string firebaseAppId = "";

	private int tryCount;

	private readonly int[] pollIntervals = new int[9] { 1, 3, 5, 10, 30, 60, 300, 600, 3600 };

	private string lastFirebaseAppId = "";

	private string lastParseId = "";

	private float recordTime;

	private ITimer timer;

	private static PushManager _instance;

	public static PushManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PushManager();
			}
			return _instance;
		}
	}

	public static void Purge()
	{
		_instance = null;
	}

	private PushManager()
	{
		tryCount = 0;
		recordTime = Time.realtimeSinceStartup;
	}

	public void reqFcmTokenMessage()
	{
		Log.Info($"reqFcmTokenMessage {connected} {lastFirebaseAppId}=>{firebaseAppId} {lastParseId}=>{parseId}");
		if (!connected)
		{
			return;
		}
		string text = "";
		if (!string.IsNullOrEmpty(parseId) && !string.IsNullOrEmpty(platform))
		{
			text = parseId + "|" + platform;
		}
		string text2 = "";
		if (!string.IsNullOrEmpty(firebaseAppId))
		{
			text2 = firebaseAppId;
		}
		if ((!string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(text2)) && (!(lastFirebaseAppId == text2) || !(lastParseId == text)))
		{
			FcmTokenMessage.Request request = new FcmTokenMessage.Request();
			if (!string.IsNullOrEmpty(text))
			{
				request.token = text;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				request.fireabaseAppId = text2;
			}
			FcmTokenMessage.Instance.Send(request);
			lastFirebaseAppId = text2;
			lastParseId = text;
		}
	}

	public void registerdAccount(string strPlatform, string strUserId, string strChannelId)
	{
		if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(platform))
		{
			platform = strPlatform;
			userId = strUserId;
			channelId = strChannelId;
			if (connected)
			{
				FcmTokenMessage.Instance.Send(new FcmTokenMessage.Request
				{
					token = userId + "|" + platform,
					fireabaseAppId = null
				});
			}
		}
	}

	public void registerdParseAccount(string strParseId, string strPlatform)
	{
		if (!string.IsNullOrEmpty(strParseId))
		{
			parseId = strParseId;
			platform = strPlatform;
			reqFcmTokenMessage();
		}
	}

	public void SetFireBaseId(string firebaseId)
	{
		if (!string.IsNullOrEmpty(firebaseId))
		{
			if (string.IsNullOrEmpty(firebaseAppId) && !string.IsNullOrEmpty(firebaseId))
			{
				PostEventLog.TrackMap("FIREBASE_APPID_READY", new Dictionary<string, object>
				{
					{
						"appid",
						firebaseId ?? ""
					},
					{
						"count",
						$"{tryCount}"
					},
					{
						"time",
						$"{Time.realtimeSinceStartup - recordTime}"
					}
				});
			}
			firebaseAppId = firebaseId;
			reqFcmTokenMessage();
		}
	}

	public string GetFireBaseId()
	{
		return firebaseAppId;
	}

	public void onLoginComplete()
	{
		connected = true;
		lastFirebaseAppId = "";
		lastParseId = "";
		reqFcmTokenMessage();
		PollForFirebase();
	}

	private void PollForFirebase()
	{
		if (!string.IsNullOrEmpty(firebaseAppId) && !string.IsNullOrEmpty(parseId))
		{
			PostEventLog.TrackMap("FIREBASE_APPID_ALL_READY", new Dictionary<string, object>
			{
				{
					"appid",
					firebaseAppId ?? ""
				},
				{
					"aid",
					parseId ?? ""
				},
				{
					"count",
					$"{tryCount}"
				},
				{
					"time",
					$"{Time.realtimeSinceStartup - recordTime}"
				}
			});
			CancelTimer();
		}
		else
		{
			GameEntry.Sdk.requestFCMToken();
			CreateTimer(pollIntervals[Mathf.Min(tryCount, pollIntervals.Length - 1)]);
			tryCount++;
		}
	}

	private void CreateTimer(float delaySec)
	{
		CancelTimer();
		timer = GameEntry.Timer.RegisterTimer(delaySec, PollForFirebase);
	}

	private void CancelTimer()
	{
		if (timer != null)
		{
			GameEntry.Timer.CancelTimer(timer);
			timer = null;
		}
	}

	public void pushFirMessaging()
	{
		GameEntry.Setting.GetPublicString("GOTO_TYPE", "");
	}

	public void CancelNotice(int type)
	{
	}

	public void recordPushData()
	{
		string publicString = GameEntry.Setting.GetPublicString("CATCH_PUSH_CLICK_DATA", "");
		if (!string.IsNullOrEmpty(publicString))
		{
			PushRecordMessage.Instance.Send(new PushRecordMessage.Request
			{
				record = "",
				click = publicString
			});
		}
	}

	public void clearPushCache()
	{
		GameEntry.Setting.SetPublicString("CATCH_PUSH_RECORD", "");
		GameEntry.Setting.SetPublicString("CATCH_PUSH_CLICK_DATA", "");
	}

	public long GetCurrentTimeUnix()
	{
		return (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds;
	}

	public void pushDataToHttpServer()
	{
	}
}
