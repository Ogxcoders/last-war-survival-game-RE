using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;

namespace BaseUtils;

public class MessageFactoryProxy
{
	private static MessageFactoryProxy _instance;

	public Action<BaseEvent> DispatchResponse1;

	public Action<string, SFSObject> DispatchResponse2;

	public static MessageFactoryProxy Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new MessageFactoryProxy();
			}
			return _instance;
		}
	}

	public void DispatchResponse(BaseEvent e)
	{
		DispatchResponse1(e);
	}

	public void DispatchResponse(string cmd, SFSObject so)
	{
		DispatchResponse2(cmd, so);
	}

	public void Dispose()
	{
	}
}
