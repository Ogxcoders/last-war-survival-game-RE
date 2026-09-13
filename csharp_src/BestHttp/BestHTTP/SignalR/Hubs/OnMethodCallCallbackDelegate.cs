using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Hubs;

public delegate void OnMethodCallCallbackDelegate(Hub hub, MethodCallMessage methodCall);
