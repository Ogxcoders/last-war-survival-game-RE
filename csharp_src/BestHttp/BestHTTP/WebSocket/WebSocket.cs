using System;
using System.Text;
using BestHTTP.Decompression.Zlib;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Extensions;
using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket;

public sealed class WebSocket
{
	public OnWebSocketOpenDelegate OnOpen;

	public OnWebSocketMessageDelegate OnMessage;

	public OnWebSocketBinaryDelegate OnBinary;

	public OnWebSocketClosedDelegate OnClosed;

	public OnWebSocketErrorDelegate OnError;

	public OnWebSocketErrorDescriptionDelegate OnErrorDesc;

	public OnWebSocketIncompleteFrameDelegate OnIncompleteFrame;

	private bool requestSent;

	private WebSocketResponse webSocket;

	public WebSocketStates State { get; private set; }

	public bool IsOpen
	{
		get
		{
			if (webSocket != null)
			{
				return !webSocket.IsClosed;
			}
			return false;
		}
	}

	public int BufferedAmount => webSocket.BufferedAmount;

	public bool StartPingThread { get; set; }

	public int PingFrequency { get; set; }

	public TimeSpan CloseAfterNoMesssage { get; set; }

	public HTTPRequest InternalRequest { get; private set; }

	public IExtension[] Extensions { get; private set; }

	public int Latency => webSocket.Latency;

	public WebSocket(Uri uri)
		: this(uri, string.Empty, string.Empty)
	{
		Extensions = new IExtension[1]
		{
			new PerMessageCompression(CompressionLevel.Default, clientNoContextTakeover: false, serverNoContextTakeover: false, 15, 15, 256)
		};
	}

	public WebSocket(Uri uri, string origin, string protocol, params IExtension[] extensions)
	{
		string text = (HTTPProtocolFactory.IsSecureProtocol(uri) ? "wss" : "ws");
		int num = ((uri.Port != -1) ? uri.Port : (text.Equals("wss", StringComparison.OrdinalIgnoreCase) ? 443 : 80));
		uri = new Uri(text + "://" + uri.Host + ":" + num + uri.GetRequestPathAndQueryURL());
		PingFrequency = 1000;
		CloseAfterNoMesssage = TimeSpan.FromSeconds(10.0);
		InternalRequest = new HTTPRequest(uri, OnInternalRequestCallback);
		InternalRequest.OnUpgraded = OnInternalRequestUpgraded;
		if (!HTTPProtocolFactory.IsSecureProtocol(uri) && uri.Port != 80 && HTTPProtocolFactory.IsSecureProtocol(uri) && uri.Port != 443)
		{
			InternalRequest.SetHeader("Host", uri.Host + ":" + uri.Port);
		}
		else
		{
			InternalRequest.SetHeader("Host", uri.Host);
		}
		InternalRequest.SetHeader("Upgrade", "websocket");
		InternalRequest.SetHeader("Connection", "Upgrade");
		InternalRequest.SetHeader("Sec-WebSocket-Key", GetSecKey(new object[4]
		{
			this,
			InternalRequest,
			uri,
			new object()
		}));
		if (!string.IsNullOrEmpty(origin))
		{
			InternalRequest.SetHeader("Origin", origin);
		}
		InternalRequest.SetHeader("Sec-WebSocket-Version", "13");
		if (!string.IsNullOrEmpty(protocol))
		{
			InternalRequest.SetHeader("Sec-WebSocket-Protocol", protocol);
		}
		InternalRequest.SetHeader("Cache-Control", "no-cache");
		InternalRequest.SetHeader("Pragma", "no-cache");
		Extensions = extensions;
		InternalRequest.DisableCache = true;
		InternalRequest.DisableRetry = true;
		InternalRequest.TryToMinimizeTCPLatency = true;
		if (HTTPManager.Proxy is HTTPProxy hTTPProxy)
		{
			InternalRequest.Proxy = new HTTPProxy(hTTPProxy.Address, hTTPProxy.Credentials, isTransparent: false, sendWholeUri: false, hTTPProxy.NonTransparentForHTTPS);
		}
		HTTPManager.Setup();
	}

	private void OnInternalRequestCallback(HTTPRequest req, HTTPResponse resp)
	{
		string empty = string.Empty;
		switch (req.State)
		{
		default:
			return;
		case HTTPRequestStates.Finished:
			if (resp.IsSuccess || resp.StatusCode == 101)
			{
				HTTPManager.Logger.Information("WebSocket", $"Request finished. Status Code: {resp.StatusCode.ToString()} Message: {resp.Message}");
				return;
			}
			empty = $"Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
			break;
		case HTTPRequestStates.Error:
			empty = "Request Finished with Error! " + ((req.Exception != null) ? ("Exception: " + req.Exception.Message + req.Exception.StackTrace) : string.Empty);
			break;
		case HTTPRequestStates.Aborted:
			empty = "Request Aborted!";
			break;
		case HTTPRequestStates.ConnectionTimedOut:
			empty = "Connection Timed Out!";
			break;
		case HTTPRequestStates.TimedOut:
			empty = "Processing the request Timed Out!";
			break;
		}
		if (State != WebSocketStates.Connecting || !string.IsNullOrEmpty(empty))
		{
			if (OnError != null)
			{
				OnError(this, req.Exception);
			}
			if (OnErrorDesc != null)
			{
				OnErrorDesc(this, empty);
			}
			if (OnError == null && OnErrorDesc == null)
			{
				HTTPManager.Logger.Error("WebSocket", empty);
			}
		}
		else if (OnClosed != null)
		{
			OnClosed(this, 1000, "Closed while opening");
		}
		if (!req.IsKeepAlive && resp != null && resp is WebSocketResponse)
		{
			(resp as WebSocketResponse).CloseStream();
		}
	}

	private void OnInternalRequestUpgraded(HTTPRequest req, HTTPResponse resp)
	{
		webSocket = resp as WebSocketResponse;
		if (webSocket == null)
		{
			if (OnError != null)
			{
				OnError(this, req.Exception);
			}
			if (OnErrorDesc != null)
			{
				string reason = string.Empty;
				if (req.Exception != null)
				{
					reason = req.Exception.Message + " " + req.Exception.StackTrace;
				}
				OnErrorDesc(this, reason);
			}
			State = WebSocketStates.Closed;
			return;
		}
		if (State == WebSocketStates.Closed)
		{
			webSocket.CloseStream();
			return;
		}
		webSocket.WebSocket = this;
		if (Extensions != null)
		{
			for (int i = 0; i < Extensions.Length; i++)
			{
				IExtension extension = Extensions[i];
				try
				{
					if (extension != null && !extension.ParseNegotiation(webSocket))
					{
						Extensions[i] = null;
					}
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("WebSocket", "ParseNegotiation", ex);
					Extensions[i] = null;
				}
			}
		}
		State = WebSocketStates.Open;
		if (OnOpen != null)
		{
			try
			{
				OnOpen(this);
			}
			catch (Exception ex2)
			{
				HTTPManager.Logger.Exception("WebSocket", "OnOpen", ex2);
			}
		}
		webSocket.OnText = delegate(WebSocketResponse ws, string msg)
		{
			if (OnMessage != null)
			{
				OnMessage(this, msg);
			}
		};
		webSocket.OnBinary = delegate(WebSocketResponse ws, byte[] bin)
		{
			if (OnBinary != null)
			{
				OnBinary(this, bin);
			}
		};
		webSocket.OnClosed = delegate(WebSocketResponse ws, ushort code, string msg)
		{
			State = WebSocketStates.Closed;
			if (OnClosed != null)
			{
				OnClosed(this, code, msg);
			}
		};
		if (OnIncompleteFrame != null)
		{
			webSocket.OnIncompleteFrame = delegate(WebSocketResponse ws, WebSocketFrameReader frame)
			{
				if (OnIncompleteFrame != null)
				{
					OnIncompleteFrame(this, frame);
				}
			};
		}
		if (StartPingThread)
		{
			webSocket.StartPinging(Math.Max(PingFrequency, 100));
		}
		webSocket.StartReceive();
	}

	public void Open()
	{
		if (requestSent)
		{
			throw new InvalidOperationException("Open already called! You can't reuse this WebSocket instance!");
		}
		if (Extensions != null)
		{
			try
			{
				for (int i = 0; i < Extensions.Length; i++)
				{
					Extensions[i]?.AddNegotiation(InternalRequest);
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("WebSocket", "Open", ex);
			}
		}
		InternalRequest.Send();
		requestSent = true;
		State = WebSocketStates.Connecting;
	}

	public void Send(string message)
	{
		if (IsOpen)
		{
			webSocket.Send(message);
		}
	}

	public void Send(byte[] buffer)
	{
		if (IsOpen)
		{
			webSocket.Send(buffer);
		}
	}

	public void Send(byte[] buffer, ulong offset, ulong count)
	{
		if (IsOpen)
		{
			webSocket.Send(buffer, offset, count);
		}
	}

	public void Send(WebSocketFrame frame)
	{
		if (IsOpen)
		{
			webSocket.Send(frame);
		}
	}

	public void Close()
	{
		if ((int)State >= 2)
		{
			return;
		}
		if (State == WebSocketStates.Connecting)
		{
			State = WebSocketStates.Closed;
			if (OnClosed != null)
			{
				OnClosed(this, 1005, string.Empty);
			}
		}
		else
		{
			State = WebSocketStates.Closing;
			webSocket.Close();
		}
	}

	public void Close(ushort code, string message)
	{
		if (IsOpen)
		{
			webSocket.Close(code, message);
		}
	}

	public static byte[] EncodeCloseData(ushort code, string message)
	{
		int byteCount = Encoding.UTF8.GetByteCount(message);
		using BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream(2 + byteCount);
		byte[] bytes = BitConverter.GetBytes(code);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse((Array)bytes, 0, bytes.Length);
		}
		bufferPoolMemoryStream.Write(bytes, 0, bytes.Length);
		bytes = Encoding.UTF8.GetBytes(message);
		bufferPoolMemoryStream.Write(bytes, 0, bytes.Length);
		return bufferPoolMemoryStream.ToArray();
	}

	private string GetSecKey(object[] from)
	{
		byte[] array = new byte[16];
		int num = 0;
		for (int i = 0; i < from.Length; i++)
		{
			byte[] bytes = BitConverter.GetBytes(from[i].GetHashCode());
			for (int j = 0; j < bytes.Length; j++)
			{
				if (num >= array.Length)
				{
					break;
				}
				array[num++] = bytes[j];
			}
		}
		return Convert.ToBase64String(array);
	}
}
