namespace BestHTTP.SignalR.Transports;

public delegate void OnTransportStateChangedDelegate(TransportBase transport, TransportStates oldState, TransportStates newState);
