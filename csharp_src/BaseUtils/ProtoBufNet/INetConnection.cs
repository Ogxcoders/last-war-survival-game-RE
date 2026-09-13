using System;
using Sfs2X.Requests;

namespace ProtoBufNet;

public interface INetConnection : IDisposable
{
	bool IsConnected { get; }

	long lastRecvTick { get; set; }

	void Close(bool graceful, string reason);

	bool SendMessage(IRequest m);
}
