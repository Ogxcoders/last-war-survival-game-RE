using System;

namespace BestHTTP.SignalRCore;

public interface ITransport
{
	TransferModes TransferMode { get; }

	TransportTypes TransportType { get; }

	TransportStates State { get; }

	string ErrorReason { get; }

	event Action<TransportStates, TransportStates> OnStateChanged;

	void StartConnect();

	void StartClose();

	void Send(byte[] msg);
}
