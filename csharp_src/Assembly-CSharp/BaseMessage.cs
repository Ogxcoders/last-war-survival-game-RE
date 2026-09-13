using System;
using System.Reflection;
using GameFramework;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;

public abstract class BaseMessage
{
	public BaseMessage()
	{
		if (Application.isPlaying && MessageFactory.ContainsMessage(GetType()))
		{
			Log.Error($"BaseMessage Duplicate Instance, Use MessageFactory To Get Instance: {GetType()}");
		}
	}

	protected BaseMessage GetInstance()
	{
		return MessageFactory.GetMessage(GetType());
	}

	public abstract string GetMsgId();

	public virtual void Handle(ISFSObject message)
	{
		if (message.ContainsKey("errorCode"))
		{
			if (showErrorCode())
			{
				ShowErrorMessage(message);
			}
			GameEntry.Event.Fire(EventId.ServerError, GetMsgId());
		}
		CSHandleResponse(message);
	}

	private static void ShowErrorMessage(ISFSObject message)
	{
		string utfString = message.GetUtfString("errorCode");
		string[] array = null;
		if (message.ContainsKey("errorPara2"))
		{
			array = message.GetUtfStringArray("errorPara2");
		}
		if (utfString == "E190003")
		{
			UIUtils.ShowTips("120246", 3f);
		}
		else if (!(utfString == "E100173"))
		{
			if (array == null)
			{
				UIUtils.ShowTips(utfString, 3f);
				return;
			}
			object[] args = array;
			UIUtils.ShowTips(utfString, 3f, args);
		}
	}

	public virtual void Send(params object[] args)
	{
		try
		{
			IRequest request = CSSetData(args);
			if (request == null)
			{
				return;
			}
			if (GMSwitch.IsGM && GMSwitch.GetBool("DebugLogProtocolMsg"))
			{
				ISFSObject content = request.Message.Content;
				string text = content?.ToJson() ?? "{}";
				string text2 = "{}";
				if (request is ExtensionRequest obj && typeof(ExtensionRequest).GetField("parameters", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj) is ISFSObject iSFSObject && iSFSObject != content)
				{
					text2 = iSFSObject.ToJson();
				}
				Log.Warning("[Msg][Send]<color=green>send msg <" + GetMsgId() + "> |</color> content = " + text + ", extension = " + text2);
			}
			GameEntry.Network.Send(request);
		}
		catch (Exception arg)
		{
			Log.Error("send msg {0} error, {1}", GetMsgId(), arg);
		}
	}

	protected virtual void CSHandleResponse(ISFSObject message)
	{
	}

	protected virtual IRequest CSSetData(params object[] args)
	{
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected virtual bool showErrorCode()
	{
		return false;
	}
}
