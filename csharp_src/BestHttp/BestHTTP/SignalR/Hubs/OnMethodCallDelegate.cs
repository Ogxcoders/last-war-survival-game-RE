namespace BestHTTP.SignalR.Hubs;

public delegate void OnMethodCallDelegate(Hub hub, string method, params object[] args);
