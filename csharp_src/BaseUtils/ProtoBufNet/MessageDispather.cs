using System;
using System.Collections;
using System.Net.Sockets;
using GameFramework;
using GameKit.Base;
using Sfs2X.Bitswarm;
using Sfs2X.Controllers;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Exceptions;
using Sfs2X.Requests;

namespace ProtoBufNet;

public class MessageDispather
{
	public static readonly string CONTROLLER_ID = "c";

	public static readonly string ACTION_ID = "a";

	public static readonly string PARAM_ID = "p";

	public static readonly string USER_ID = "u";

	public static readonly string UDP_PACKET_ID = "i";

	private static readonly int MsgType_SYS = 0;

	private static readonly int MsgType_GAME = 1;

	private NetRawProxy proxy;

	public MessageDispather(NetRawProxy proxy)
	{
		this.proxy = proxy;
	}

	public void OnConnectionLost(string msg, SocketError error)
	{
		proxy.OnConnectionLost(msg, error);
	}

	public void Dispatch(INetConnection c, INetPacket p)
	{
		ISFSObject info = p.info;
		p.Dispose();
		IMessage message = new Message();
		if (info.IsNull(CONTROLLER_ID))
		{
			throw new SFSCodecError("Request rejected: No Controller ID in request!");
		}
		if (info.IsNull(ACTION_ID))
		{
			throw new SFSCodecError("Request rejected: No Action ID in request!");
		}
		message.Id = Convert.ToInt32(info.GetShort(ACTION_ID));
		message.Content = info.GetSFSObject(PARAM_ID);
		message.IsUDP = info.ContainsKey(UDP_PACKET_ID);
		int num = info.GetByte(CONTROLLER_ID);
		if (num == MsgType_SYS)
		{
			HandleSystemMessage(message, p.recvTime);
			return;
		}
		if (num == MsgType_GAME)
		{
			HandleExtensionMessage(message);
			return;
		}
		throw new SFSError("Cannot handle server response. Unknown controller, id: " + num);
	}

	private void HandleExtensionMessage(IMessage message)
	{
		ISFSObject content = message.Content;
		string utfString = content.GetUtfString(ExtensionController.KEY_CMD);
		SFSObject so = (SFSObject)content.GetSFSObject(ExtensionController.KEY_PARAMS);
		proxy.OnExtensionResponse(utfString, so);
	}

	private void HandleSystemMessage(IMessage message, long recvTime)
	{
		if (message.Id == 1)
		{
			FnLogin(message);
		}
		else if (message.Id == 29)
		{
			FnPingPong(message, recvTime);
		}
		else
		{
			Log.Error("not support sys {0} data {1}", message.Id, message.Content.ToJson());
		}
	}

	private void FnLogin(IMessage msg)
	{
		ISFSObject content = msg.Content;
		Hashtable hashtable = new Hashtable();
		if (content.IsNull(BaseRequest.KEY_ERROR_CODE))
		{
			hashtable["data"] = content.GetSFSObject(LoginRequest.KEY_PARAMS);
			SFSEvent e = new SFSEvent(SFSEvent.LOGIN, hashtable);
			proxy.OnLogin(e);
			return;
		}
		short num = content.GetShort(BaseRequest.KEY_ERROR_CODE);
		object[] utfStringArray = content.GetUtfStringArray(BaseRequest.KEY_ERROR_PARAMS);
		string errorMessage = ErrorCodes.GetErrorMessage(num, utfStringArray);
		ISFSObject iSFSObject = new SFSObject();
		iSFSObject.PutUtfString("errorMessage", errorMessage);
		iSFSObject.PutShort("errorCode", num);
		hashtable["errorMessage"] = iSFSObject;
		proxy.OnLoginError(new SFSEvent(SFSEvent.LOGIN_ERROR, hashtable));
	}

	private void FnPingPong(IMessage msg, long recvTime)
	{
		proxy.OnPingPong(msg, recvTime);
	}
}
