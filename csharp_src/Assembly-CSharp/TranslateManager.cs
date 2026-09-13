using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SFSLitJson;
using UnityEngine;

public class TranslateManager
{
	private static TranslateManager _instance;

	public string version;

	private string mTransMailID;

	private List<string> mTransMailQueue = new List<string>();

	private string mDialogMailID = string.Empty;

	private string mDialogID = string.Empty;

	private ConcurrentQueue<MailDialogStruct> mMailDialogs = new ConcurrentQueue<MailDialogStruct>();

	private static object _dialoglock = new object();

	private string mChatRoomId = string.Empty;

	private int mChatSeqId;

	private readonly List<ChatTranslateStruct> mChatDataList = new List<ChatTranslateStruct>();

	public static TranslateManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new TranslateManager
				{
					version = GameEntry.Sdk.Version
				};
			}
			return _instance;
		}
	}

	public static void Purge()
	{
		_instance = null;
	}

	public string GetLangString(string str)
	{
		switch (str)
		{
		case "zh-CN":
		case "zh_CN":
		case "zh-Hans":
		case "zh-CHS":
		case "cn":
			return "zh-Hans";
		case "zh-TW":
		case "zh_TW":
		case "zh-Hant":
		case "zh-CHT":
		case "tw":
			return "zh-Hant";
		default:
			return str;
		}
	}

	public string TranslateLang(string originLang, string content, string targetLand, string scene = "private")
	{
		string text = (GameEntry.Timer.GetServerTime() / 1000).ToString();
		string langString = GetLangString(targetLand);
		string langString2 = GetLangString(originLang);
		string text2 = GameEntry.Data.Player.GetSelfServerId().ToString();
		string text3 = "";
		text3 += GameEntry.Data.Player.Uid;
		text3 += ",";
		text3 += text2;
		text3 += ",";
		text3 += version;
		string uid = GameEntry.Data.Player.Uid;
		string text4 = "lastshelter";
		string md5Hash = AESHelper.GetMd5Hash(string.Concat(string.Concat(string.Concat(string.Concat("" + langString2, langString), text4), text), uid));
		_ = "sc=" + content + "&sf=" + langString2 + "&tf=" + langString + "&ch=" + text4 + "&t=" + text + "&ui=" + text3 + "&scene=" + scene + "&sig=" + md5Hash + "&uid=" + uid;
		return "";
	}

	public void GoogleTransLateNormal(string originalLang, string content, string targeLand)
	{
		string json = TranslateLang(originalLang, content, targeLand);
		try
		{
			JsonData jsonData = JsonMapper.ToObject(json);
			if (jsonData == null)
			{
				return;
			}
			if ((int)jsonData["code"] != 0)
			{
				string msgKey = "invalid account.";
				if (((IDictionary)jsonData).Contains((object)"message"))
				{
					msgKey = (string)jsonData["message"];
				}
				UIUtils.ShowTips(msgKey, 3f);
				return;
			}
			string userData = "";
			if (((IDictionary)jsonData).Contains((object)"translateMsg"))
			{
				userData = (string)jsonData["translateMsg"];
			}
			if (((IDictionary)jsonData).Contains((object)"originalLang"))
			{
				_ = (string)jsonData["originalLang"];
			}
			GameEntry.Event.Fire(EventId.Translate_Normal, userData);
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
		}
	}

	public void GoogleTranslateByMail()
	{
	}

	protected void OnGoogleTranslateSuccess(JsonData data)
	{
	}

	protected void OnGoogleTranslateFail()
	{
		mTransMailID = "";
		GameEntry.Event.Fire(EventId.Translate_Mail);
	}

	public void GoogleTranslateByDialog()
	{
	}

	protected void OnGoogleTranslateDialogSuccess(JsonData jsonData)
	{
	}

	protected void OnGoogleTranslateDialogFail()
	{
		GameEntry.Event.Fire(EventId.Translate_Dialog);
		mDialogID = string.Empty;
		mDialogMailID = string.Empty;
	}

	public void GoogleTranslateByChat()
	{
		if (!string.IsNullOrEmpty(mChatRoomId))
		{
			_ = mChatSeqId;
		}
		OnGoogleTranslateByChatFailure();
	}

	private void OnGoogleTranslateByChatSuccess(TranslateResult result)
	{
		if (string.IsNullOrEmpty(mChatRoomId) || mChatSeqId == 0)
		{
			OnGoogleTranslateByChatFailure();
			return;
		}
		if (string.IsNullOrEmpty(result.translateMsg))
		{
			OnGoogleTranslateByChatFailure();
			return;
		}
		mChatSeqId = 0;
		mChatRoomId = "";
	}

	private void OnGoogleTranslateByChatFailure()
	{
		mChatSeqId = 0;
		mChatRoomId = "";
	}

	public void OnEnterFrame()
	{
		OnMailTranslate();
		OnDialogTranslate();
		OnChatTranslate();
	}

	protected void OnMailTranslate()
	{
		if (string.IsNullOrEmpty(mTransMailID) && mTransMailQueue.Count > 0)
		{
			mTransMailID = mTransMailQueue[0];
			mTransMailQueue.RemoveAt(0);
			TranslateUtils.OnThreadGoogleTranslateByMail();
		}
	}

	public void AddMailMailDialog(MailDialogStruct mailDialog)
	{
		mMailDialogs.Enqueue(mailDialog);
	}

	protected void OnDialogTranslate()
	{
		if (mMailDialogs.IsEmpty)
		{
			return;
		}
		MailDialogStruct result = default(MailDialogStruct);
		if (mMailDialogs.TryDequeue(out result))
		{
			if (result.HadTranslate)
			{
				GameEntry.Event.Fire(EventId.Translate_Dialog, result);
				return;
			}
			mDialogMailID = result.MailUid;
			mDialogID = result.DialogUid;
			TranslateUtils.OnThreadGoogleTranslateByDialog();
		}
	}

	public void AddChat(ChatTranslateStruct chat)
	{
		mChatDataList.Add(chat);
	}

	private void OnChatTranslate()
	{
		if (mChatSeqId == 0 && mChatDataList.Count > 0)
		{
			mChatSeqId = mChatDataList[0].seqId;
			mChatRoomId = mChatDataList[0].roomId;
			mChatDataList.RemoveAt(0);
			TranslateUtils.OnThreadGoogleTranslateByChat();
		}
	}
}
