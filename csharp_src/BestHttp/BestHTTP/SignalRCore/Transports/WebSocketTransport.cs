using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.JSON;
using BestHTTP.SignalRCore.Messages;
using BestHTTP.WebSocket;

namespace BestHTTP.SignalRCore.Transports;

internal sealed class WebSocketTransport : ITransport
{
	private TransportStates _state;

	private BestHTTP.WebSocket.WebSocket webSocket;

	private HubConnection connection;

	private List<Message> messages = new List<Message>();

	public TransportTypes TransportType => TransportTypes.WebSocket;

	public TransferModes TransferMode => TransferModes.Binary;

	public TransportStates State
	{
		get
		{
			return _state;
		}
		private set
		{
			if (_state != value)
			{
				TransportStates state = _state;
				_state = value;
				if (this.OnStateChanged != null)
				{
					this.OnStateChanged(state, _state);
				}
			}
		}
	}

	public string ErrorReason { get; private set; }

	public event Action<TransportStates, TransportStates> OnStateChanged;

	public WebSocketTransport(HubConnection con)
	{
		connection = con;
		State = TransportStates.Initial;
	}

	void ITransport.StartConnect()
	{
		HTTPManager.Logger.Verbose("WebSocketTransport", "StartConnect");
		if (webSocket == null)
		{
			Uri uri = BuildUri(connection.Uri);
			if (connection.AuthenticationProvider != null)
			{
				uri = connection.AuthenticationProvider.PrepareUri(uri) ?? uri;
			}
			HTTPManager.Logger.Verbose("WebSocketTransport", "StartConnect connecting to Uri: " + uri.ToString());
			webSocket = new BestHTTP.WebSocket.WebSocket(uri);
		}
		if (connection.AuthenticationProvider != null)
		{
			connection.AuthenticationProvider.PrepareRequest(webSocket.InternalRequest);
		}
		BestHTTP.WebSocket.WebSocket obj = webSocket;
		obj.OnOpen = (OnWebSocketOpenDelegate)Delegate.Combine(obj.OnOpen, new OnWebSocketOpenDelegate(OnOpen));
		BestHTTP.WebSocket.WebSocket obj2 = webSocket;
		obj2.OnMessage = (OnWebSocketMessageDelegate)Delegate.Combine(obj2.OnMessage, new OnWebSocketMessageDelegate(OnMessage));
		BestHTTP.WebSocket.WebSocket obj3 = webSocket;
		obj3.OnBinary = (OnWebSocketBinaryDelegate)Delegate.Combine(obj3.OnBinary, new OnWebSocketBinaryDelegate(OnBinary));
		BestHTTP.WebSocket.WebSocket obj4 = webSocket;
		obj4.OnErrorDesc = (OnWebSocketErrorDescriptionDelegate)Delegate.Combine(obj4.OnErrorDesc, new OnWebSocketErrorDescriptionDelegate(OnError));
		BestHTTP.WebSocket.WebSocket obj5 = webSocket;
		obj5.OnClosed = (OnWebSocketClosedDelegate)Delegate.Combine(obj5.OnClosed, new OnWebSocketClosedDelegate(OnClosed));
		webSocket.Open();
		State = TransportStates.Connecting;
	}

	void ITransport.Send(byte[] msg)
	{
		webSocket.Send(msg);
	}

	private void OnOpen(BestHTTP.WebSocket.WebSocket webSocket)
	{
		HTTPManager.Logger.Verbose("WebSocketTransport", "OnOpen");
		byte[] msg = JsonProtocol.WithSeparator($"{{'protocol':'{connection.Protocol.Encoder.Name}', 'version': 1}}");
		((ITransport)this).Send(msg);
	}

	private string ParseHandshakeResponse(string data)
	{
		if (!(Json.Decode(data) is Dictionary<string, object> dictionary))
		{
			return "Couldn't parse json data: " + data;
		}
		if (dictionary.TryGetValue("error", out var value))
		{
			return value.ToString();
		}
		return null;
	}

	private void HandleHandshakeResponse(string data)
	{
		ErrorReason = ParseHandshakeResponse(data);
		State = (string.IsNullOrEmpty(ErrorReason) ? TransportStates.Connected : TransportStates.Failed);
	}

	private void OnMessage(BestHTTP.WebSocket.WebSocket webSocket, string data)
	{
		if (State == TransportStates.Connecting)
		{
			HandleHandshakeResponse(data);
			return;
		}
		messages.Clear();
		try
		{
			connection.Protocol.ParseMessages(data, ref messages);
			connection.OnMessages(messages);
		}
		catch (Exception ex)
		{
			HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage(string)", ex);
		}
		finally
		{
			messages.Clear();
		}
	}

	private void OnBinary(BestHTTP.WebSocket.WebSocket webSocket, byte[] data)
	{
		if (State == TransportStates.Connecting)
		{
			HandleHandshakeResponse(Encoding.UTF8.GetString(data, 0, data.Length));
			return;
		}
		messages.Clear();
		try
		{
			connection.Protocol.ParseMessages(data, ref messages);
			connection.OnMessages(messages);
		}
		catch (Exception ex)
		{
			HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage(byte[])", ex);
		}
		finally
		{
			messages.Clear();
		}
	}

	private void OnError(BestHTTP.WebSocket.WebSocket webSocket, string reason)
	{
		HTTPManager.Logger.Verbose("WebSocketTransport", "OnError: " + reason);
		ErrorReason = reason;
		State = TransportStates.Failed;
	}

	private void OnClosed(BestHTTP.WebSocket.WebSocket webSocket, ushort code, string message)
	{
		HTTPManager.Logger.Verbose("WebSocketTransport", "OnClosed: " + code + " " + message);
		this.webSocket = null;
		State = TransportStates.Closed;
	}

	void ITransport.StartClose()
	{
		HTTPManager.Logger.Verbose("WebSocketTransport", "StartClose");
		if (webSocket != null)
		{
			State = TransportStates.Closing;
			webSocket.Close();
		}
		else
		{
			State = TransportStates.Closed;
		}
	}

	private Uri BuildUri(Uri baseUri)
	{
		if (connection.NegotiationResult == null)
		{
			return baseUri;
		}
		UriBuilder uriBuilder = new UriBuilder(baseUri);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(baseUri.Query);
		if (!string.IsNullOrEmpty(connection.NegotiationResult.ConnectionId))
		{
			stringBuilder.Append("&id=").Append(connection.NegotiationResult.ConnectionId);
		}
		uriBuilder.Query = stringBuilder.ToString();
		return uriBuilder.Uri;
	}
}
