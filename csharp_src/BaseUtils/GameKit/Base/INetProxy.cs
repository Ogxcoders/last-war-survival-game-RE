using System;
using System.Collections.Generic;
using Sfs2X.Requests;

namespace GameKit.Base;

public interface INetProxy
{
	string proxyName { get; }

	bool IsConnected { get; }

	bool IsConnecting { get; }

	bool IsPingPongTimeOut { get; }

	ProxyStatus Status { get; }

	string proxyHost { get; }

	int proxyPort { get; }

	int proxyConnectionType { get; }

	long resolveDnsTime { get; }

	long connectTime { get; }

	bool syncServerTime { get; }

	void Connect();

	void Disconnect();

	void Send(IRequest request);

	void SyncPingPong(int time = -1);

	void UpdateSmartFoxClient();

	int GetPing();

	int GetLastPingPongTime();

	void UpdateSrcServerId(ushort sid);

	void FlushProfiler(Action<Dictionary<string, string>> cb);
}
