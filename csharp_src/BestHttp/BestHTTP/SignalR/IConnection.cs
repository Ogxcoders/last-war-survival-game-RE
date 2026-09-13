using System;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using BestHTTP.SignalR.Transports;

namespace BestHTTP.SignalR;

public interface IConnection
{
	ProtocolVersions Protocol { get; }

	NegotiationData NegotiationResult { get; }

	IJsonEncoder JsonEncoder { get; set; }

	void OnMessage(IServerMessage msg);

	void TransportStarted();

	void TransportReconnected();

	void TransportAborted();

	void Error(string reason);

	Uri BuildUri(RequestTypes type);

	Uri BuildUri(RequestTypes type, TransportBase transport);

	HTTPRequest PrepareRequest(HTTPRequest req, RequestTypes type);

	string ParseResponse(string responseStr);
}
