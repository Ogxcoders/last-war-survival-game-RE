using System.Collections.Generic;

namespace BestHTTP.SocketIO.Transports;

public interface ITransport
{
	TransportTypes Type { get; }

	TransportStates State { get; }

	SocketManager Manager { get; }

	bool IsRequestInProgress { get; }

	bool IsPollingInProgress { get; }

	void Open();

	void Poll();

	void Send(Packet packet);

	void Send(List<Packet> packets);

	void Close();
}
