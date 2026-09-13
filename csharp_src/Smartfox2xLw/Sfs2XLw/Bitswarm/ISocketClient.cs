using Sfs2XLw.Bitswarm.BBox;
using Sfs2XLw.Controllers;
using Sfs2XLw.Core;
using Sfs2XLw.Core.Sockets;
using Sfs2XLw.Logging;
using Sfs2XLw.Util;

namespace Sfs2XLw.Bitswarm;

public interface ISocketClient
{
	string ConnectionMode { get; }

	bool UseBlueBox { get; }

	bool Debug { get; }

	SmartFox Sfs { get; }

	bool Connected { get; }

	IoHandler IoHandler { get; set; }

	int CompressionThreshold { get; set; }

	int MaxMessageSize { get; set; }

	SystemController SysController { get; }

	ExtensionController ExtController { get; }

	ISocketLayer Socket { get; }

	BBClient HttpClient { get; }

	bool IsReconnecting { get; set; }

	int ReconnectionSeconds { get; set; }

	EventDispatcher Dispatcher { get; set; }

	Logger Log { get; }

	string ConnectionHost { get; }

	int ConnectionPort { get; }

	IUDPManager UdpManager { get; set; }

	bool IsBinProtocol { get; }

	CryptoKey CryptoKey { get; set; }

	void ForceBlueBox(bool val);

	void EnableBlueBoxDebug(bool val);

	void Init();

	void Destroy();

	IController GetController(int id);

	void Connect();

	void Connect(string host, int port);

	void Send(IMessage message);

	void Disconnect();

	void Disconnect(string reason);

	void StopReconnection();

	void KillConnection();

	long NextUdpPacketId();

	void AddEventListener(string eventType, EventListenerDelegate listener);
}
