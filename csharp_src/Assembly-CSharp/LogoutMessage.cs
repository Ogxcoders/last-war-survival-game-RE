using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;

public class LogoutMessage : BaseMessage
{
	public class Request
	{
		public string zoneName;
	}

	private static LogoutMessage _instance;

	public string g_durloading_commandlist;

	private ISFSObject m_dic;

	public static LogoutMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<LogoutMessage>());

	public override string GetMsgId()
	{
		return "logout";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		g_durloading_commandlist += "logout;";
		return new LogoutRequest();
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		g_durloading_commandlist += "logoutRec;";
		if (message.ContainsKey("zoneName"))
		{
			Debug.Log("logout success");
			m_dic = message;
		}
	}

	public void Handle()
	{
		g_durloading_commandlist += "logoutRec;";
	}
}
