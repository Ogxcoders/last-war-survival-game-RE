using System;

namespace Main.Scripts.Network;

internal struct msgSendInfo
{
	private int _futureId;

	private string _msgId;

	private long _sendTime;

	public msgSendInfo(int fuid, string msgId)
	{
		_futureId = fuid;
		_msgId = msgId;
		_sendTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
	}

	public long getSendTime()
	{
		return _sendTime;
	}

	public string getMsgId()
	{
		return _msgId;
	}
}
