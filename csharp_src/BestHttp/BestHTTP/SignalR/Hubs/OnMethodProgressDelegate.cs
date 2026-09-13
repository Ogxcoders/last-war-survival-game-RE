using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Hubs;

public delegate void OnMethodProgressDelegate(Hub hub, ClientMessage originialMessage, ProgressMessage progress);
