using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket;

public delegate void OnWebSocketIncompleteFrameDelegate(WebSocket webSocket, WebSocketFrameReader frame);
