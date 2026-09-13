using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class GetServerListMessage : BaseMessage
{
	public class Request
	{
		public int serverId;
	}

	private static GetServerListMessage _instance;

	public static GetServerListMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<GetServerListMessage>());

	public override string GetMsgId()
	{
		return "get.server.list";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		ISFSObject iSFSObject = new SFSObject();
		iSFSObject.PutInt("serverId", request.serverId);
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message.ContainsKey("serverInfo"))
		{
			try
			{
				ISFSObject sFSObject = message.GetSFSObject("serverInfo");
				string ip = sFSObject.GetUtfString("ip");
				string utfString = sFSObject.GetUtfString("ws_ip");
				string s = sFSObject.GetUtfString("port");
				int connectionType = 0;
				if (ClientSwitch.IsOn(26) && !string.IsNullOrEmpty(utfString))
				{
					ip = utfString;
					connectionType = 1;
					s = "80";
				}
				string utfString2 = sFSObject.GetUtfString("zone");
				GameEntry.NetworkCross.OnGetServerListFromSFS(utfString2, ip, int.Parse(s), connectionType);
				return;
			}
			catch (Exception arg)
			{
				GameEntry.NetworkCross.OnGetServerListFromSFSFailed($"sfs get.server.list Exception: {arg}");
				return;
			}
		}
		string utfString3 = message.GetUtfString("errorCode");
		GameEntry.NetworkCross.OnGetServerListFromSFSFailed("sfs get.server.list serverInfo is null. errorCode:" + utfString3);
	}
}
