using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Hubs;

public delegate void OnMethodResultDelegate(Hub hub, ClientMessage originalMessage, ResultMessage result);
