using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Hubs;

public delegate void OnMethodFailedDelegate(Hub hub, ClientMessage originalMessage, FailureMessage error);
