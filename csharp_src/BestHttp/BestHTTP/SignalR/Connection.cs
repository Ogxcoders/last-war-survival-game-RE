using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.Extensions;
using BestHTTP.JSON;
using BestHTTP.SignalR.Authentication;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using BestHTTP.SignalR.Transports;
using PlatformSupport.Collections.ObjectModel;
using PlatformSupport.Collections.Specialized;

namespace BestHTTP.SignalR;

public sealed class Connection : IHeartbeat, IConnection
{
	public static IJsonEncoder DefaultEncoder = new DefaultJsonEncoder();

	private ConnectionStates _state;

	private ObservableDictionary<string, string> additionalQueryParams;

	internal object SyncRoot = new object();

	private readonly string[] ClientProtocols = new string[3] { "1.3", "1.4", "1.5" };

	private ulong RequestCounter;

	private MultiMessage LastReceivedMessage;

	private string GroupsToken;

	private List<IServerMessage> BufferedMessages;

	private DateTime LastMessageReceivedAt;

	private DateTime ReconnectStartedAt;

	private DateTime ReconnectDelayStartedAt;

	private bool ReconnectStarted;

	private DateTime LastPingSentAt;

	private HTTPRequest PingRequest;

	private DateTime? TransportConnectionStartedAt;

	private StringBuilder queryBuilder = new StringBuilder();

	private string BuiltConnectionData;

	private string BuiltQueryParams;

	private SupportedProtocols NextProtocolToTry;

	public Uri Uri { get; private set; }

	public ConnectionStates State
	{
		get
		{
			return _state;
		}
		private set
		{
			ConnectionStates state = _state;
			_state = value;
			if (this.OnStateChanged != null)
			{
				this.OnStateChanged(this, state, _state);
			}
		}
	}

	public NegotiationData NegotiationResult { get; private set; }

	public Hub[] Hubs { get; private set; }

	public TransportBase Transport { get; private set; }

	public ProtocolVersions Protocol { get; private set; }

	public ObservableDictionary<string, string> AdditionalQueryParams
	{
		get
		{
			return additionalQueryParams;
		}
		set
		{
			if (additionalQueryParams != null)
			{
				additionalQueryParams.CollectionChanged -= AdditionalQueryParams_CollectionChanged;
			}
			additionalQueryParams = value;
			BuiltQueryParams = null;
			if (value != null)
			{
				value.CollectionChanged += AdditionalQueryParams_CollectionChanged;
			}
		}
	}

	public bool QueryParamsOnlyForHandshake { get; set; }

	public IJsonEncoder JsonEncoder { get; set; }

	public IAuthenticationProvider AuthenticationProvider { get; set; }

	public TimeSpan PingInterval { get; set; }

	public TimeSpan ReconnectDelay { get; set; }

	public OnPrepareRequestDelegate RequestPreparator { get; set; }

	public Hub this[int idx] => Hubs[idx];

	public Hub this[string hubName]
	{
		get
		{
			for (int i = 0; i < Hubs.Length; i++)
			{
				Hub hub = Hubs[i];
				if (hub.Name.Equals(hubName, StringComparison.OrdinalIgnoreCase))
				{
					return hub;
				}
			}
			return null;
		}
	}

	internal ulong ClientMessageCounter { get; set; }

	private uint Timestamp => (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Ticks;

	private string ConnectionData
	{
		get
		{
			if (!string.IsNullOrEmpty(BuiltConnectionData))
			{
				return BuiltConnectionData;
			}
			StringBuilder stringBuilder = new StringBuilder("[", Hubs.Length * 4);
			if (Hubs != null)
			{
				for (int i = 0; i < Hubs.Length; i++)
				{
					stringBuilder.Append("{\"Name\":\"");
					stringBuilder.Append(Hubs[i].Name);
					stringBuilder.Append("\"}");
					if (i < Hubs.Length - 1)
					{
						stringBuilder.Append(",");
					}
				}
			}
			stringBuilder.Append("]");
			return BuiltConnectionData = Uri.EscapeUriString(stringBuilder.ToString());
		}
	}

	private string QueryParams
	{
		get
		{
			if (AdditionalQueryParams == null || AdditionalQueryParams.Count == 0)
			{
				return string.Empty;
			}
			if (!string.IsNullOrEmpty(BuiltQueryParams))
			{
				return BuiltQueryParams;
			}
			StringBuilder stringBuilder = new StringBuilder(AdditionalQueryParams.Count * 4);
			foreach (KeyValuePair<string, string> additionalQueryParam in AdditionalQueryParams)
			{
				stringBuilder.Append("&");
				stringBuilder.Append(additionalQueryParam.Key);
				if (!string.IsNullOrEmpty(additionalQueryParam.Value))
				{
					stringBuilder.Append("=");
					stringBuilder.Append(Uri.EscapeDataString(additionalQueryParam.Value));
				}
			}
			return BuiltQueryParams = stringBuilder.ToString();
		}
	}

	public event OnConnectedDelegate OnConnected;

	public event OnClosedDelegate OnClosed;

	public event OnErrorDelegate OnError;

	public event OnConnectedDelegate OnReconnecting;

	public event OnConnectedDelegate OnReconnected;

	public event OnStateChanged OnStateChanged;

	public event OnNonHubMessageDelegate OnNonHubMessage;

	public Connection(Uri uri, params string[] hubNames)
		: this(uri)
	{
		if (hubNames != null && hubNames.Length != 0)
		{
			Hubs = new Hub[hubNames.Length];
			for (int i = 0; i < hubNames.Length; i++)
			{
				Hubs[i] = new Hub(hubNames[i], this);
			}
		}
	}

	public Connection(Uri uri, params Hub[] hubs)
		: this(uri)
	{
		Hubs = hubs;
		if (hubs != null)
		{
			for (int i = 0; i < hubs.Length; i++)
			{
				((IHub)hubs[i]).Connection = this;
			}
		}
	}

	public Connection(Uri uri)
	{
		State = ConnectionStates.Initial;
		Uri = uri;
		JsonEncoder = DefaultEncoder;
		PingInterval = TimeSpan.FromMinutes(5.0);
		Protocol = ProtocolVersions.Protocol_2_2;
		ReconnectDelay = TimeSpan.FromSeconds(5.0);
	}

	public void Open()
	{
		if (State == ConnectionStates.Initial || State == ConnectionStates.Closed)
		{
			if (AuthenticationProvider != null && AuthenticationProvider.IsPreAuthRequired)
			{
				State = ConnectionStates.Authenticating;
				AuthenticationProvider.OnAuthenticationSucceded += OnAuthenticationSucceded;
				AuthenticationProvider.OnAuthenticationFailed += OnAuthenticationFailed;
				AuthenticationProvider.StartAuthentication();
			}
			else
			{
				StartImpl();
			}
		}
	}

	private void OnAuthenticationSucceded(IAuthenticationProvider provider)
	{
		provider.OnAuthenticationSucceded -= OnAuthenticationSucceded;
		provider.OnAuthenticationFailed -= OnAuthenticationFailed;
		StartImpl();
	}

	private void OnAuthenticationFailed(IAuthenticationProvider provider, string reason)
	{
		provider.OnAuthenticationSucceded -= OnAuthenticationSucceded;
		provider.OnAuthenticationFailed -= OnAuthenticationFailed;
		((IConnection)this).Error(reason);
	}

	private void StartImpl()
	{
		State = ConnectionStates.Negotiating;
		NegotiationResult = new NegotiationData(this);
		NegotiationResult.OnReceived = OnNegotiationDataReceived;
		NegotiationResult.OnError = OnNegotiationError;
		NegotiationResult.Start();
	}

	private void OnNegotiationDataReceived(NegotiationData data)
	{
		int num = -1;
		for (int i = 0; i < ClientProtocols.Length; i++)
		{
			if (num != -1)
			{
				break;
			}
			if (data.ProtocolVersion == ClientProtocols[i])
			{
				num = i;
			}
		}
		if (num == -1)
		{
			num = 2;
			HTTPManager.Logger.Warning("SignalR Connection", "Unknown protocol version: " + data.ProtocolVersion);
		}
		Protocol = (ProtocolVersions)num;
		if (data.TryWebSockets)
		{
			Transport = new WebSocketTransport(this);
			NextProtocolToTry = SupportedProtocols.ServerSentEvents;
		}
		else
		{
			Transport = new ServerSentEventsTransport(this);
			NextProtocolToTry = SupportedProtocols.HTTP;
		}
		State = ConnectionStates.Connecting;
		TransportConnectionStartedAt = DateTime.UtcNow;
		Transport.Connect();
	}

	private void OnNegotiationError(NegotiationData data, string error)
	{
		((IConnection)this).Error(error);
	}

	public void Close()
	{
		if (State == ConnectionStates.Closed)
		{
			return;
		}
		State = ConnectionStates.Closed;
		ReconnectStarted = false;
		TransportConnectionStartedAt = null;
		if (Transport != null)
		{
			Transport.Abort();
			Transport = null;
		}
		NegotiationResult = null;
		HTTPManager.Heartbeats.Unsubscribe(this);
		LastReceivedMessage = null;
		if (Hubs != null)
		{
			for (int i = 0; i < Hubs.Length; i++)
			{
				((IHub)Hubs[i]).Close();
			}
		}
		if (BufferedMessages != null)
		{
			BufferedMessages.Clear();
			BufferedMessages = null;
		}
		if (this.OnClosed == null)
		{
			return;
		}
		try
		{
			this.OnClosed(this);
		}
		catch (Exception ex)
		{
			HTTPManager.Logger.Exception("SignalR Connection", "OnClosed", ex);
		}
	}

	public void Reconnect()
	{
		if (ReconnectStarted)
		{
			return;
		}
		ReconnectStarted = true;
		if (State != ConnectionStates.Reconnecting)
		{
			ReconnectStartedAt = DateTime.UtcNow;
		}
		State = ConnectionStates.Reconnecting;
		HTTPManager.Logger.Warning("SignalR Connection", "Reconnecting");
		Transport.Reconnect();
		if (PingRequest != null)
		{
			PingRequest.Abort();
		}
		if (this.OnReconnecting == null)
		{
			return;
		}
		try
		{
			this.OnReconnecting(this);
		}
		catch (Exception ex)
		{
			HTTPManager.Logger.Exception("SignalR Connection", "OnReconnecting", ex);
		}
	}

	public bool Send(object arg)
	{
		if (arg == null)
		{
			throw new ArgumentNullException("arg");
		}
		lock (SyncRoot)
		{
			if (State != ConnectionStates.Connected)
			{
				return false;
			}
			string text = JsonEncoder.Encode(arg);
			if (string.IsNullOrEmpty(text))
			{
				HTTPManager.Logger.Error("SignalR Connection", "Failed to JSon encode the given argument. Please try to use an advanced JSon encoder(check the documentation how you can do it).");
			}
			else
			{
				Transport.Send(text);
			}
		}
		return true;
	}

	public bool SendJson(string json)
	{
		if (json == null)
		{
			throw new ArgumentNullException("json");
		}
		lock (SyncRoot)
		{
			if (State != ConnectionStates.Connected)
			{
				return false;
			}
			Transport.Send(json);
		}
		return true;
	}

	void IConnection.OnMessage(IServerMessage msg)
	{
		if (State == ConnectionStates.Closed)
		{
			return;
		}
		if (State == ConnectionStates.Connecting)
		{
			if (BufferedMessages == null)
			{
				BufferedMessages = new List<IServerMessage>();
			}
			BufferedMessages.Add(msg);
			return;
		}
		LastMessageReceivedAt = DateTime.UtcNow;
		switch (msg.Type)
		{
		case MessageTypes.Multiple:
			LastReceivedMessage = msg as MultiMessage;
			if (LastReceivedMessage.IsInitialization)
			{
				HTTPManager.Logger.Information("SignalR Connection", "OnMessage - Init");
			}
			if (LastReceivedMessage.GroupsToken != null)
			{
				GroupsToken = LastReceivedMessage.GroupsToken;
			}
			if (LastReceivedMessage.ShouldReconnect)
			{
				HTTPManager.Logger.Information("SignalR Connection", "OnMessage - Should Reconnect");
				Reconnect();
			}
			if (LastReceivedMessage.Data != null)
			{
				for (int i = 0; i < LastReceivedMessage.Data.Count; i++)
				{
					((IConnection)this).OnMessage(LastReceivedMessage.Data[i]);
				}
			}
			break;
		case MessageTypes.MethodCall:
		{
			MethodCallMessage methodCallMessage = msg as MethodCallMessage;
			Hub hub = this[methodCallMessage.Hub];
			if (hub != null)
			{
				((IHub)hub).OnMethod(methodCallMessage);
			}
			else
			{
				HTTPManager.Logger.Warning("SignalR Connection", $"Hub \"{methodCallMessage.Hub}\" not found!");
			}
			break;
		}
		case MessageTypes.Result:
		case MessageTypes.Failure:
		case MessageTypes.Progress:
		{
			ulong invocationId = (msg as IHubMessage).InvocationId;
			Hub hub = FindHub(invocationId);
			if (hub != null)
			{
				((IHub)hub).OnMessage(msg);
			}
			else
			{
				HTTPManager.Logger.Warning("SignalR Connection", $"No Hub found for Progress message! Id: {invocationId.ToString()}");
			}
			break;
		}
		case MessageTypes.Data:
			if (this.OnNonHubMessage != null)
			{
				this.OnNonHubMessage(this, (msg as DataMessage).Data);
			}
			break;
		default:
			HTTPManager.Logger.Warning("SignalR Connection", "Unknown message type received: " + msg.Type);
			break;
		case MessageTypes.KeepAlive:
			break;
		}
	}

	void IConnection.TransportStarted()
	{
		if (State != ConnectionStates.Connecting)
		{
			return;
		}
		InitOnStart();
		if (this.OnConnected != null)
		{
			try
			{
				this.OnConnected(this);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("SignalR Connection", "OnOpened", ex);
			}
		}
		if (BufferedMessages != null)
		{
			for (int i = 0; i < BufferedMessages.Count; i++)
			{
				((IConnection)this).OnMessage(BufferedMessages[i]);
			}
			BufferedMessages.Clear();
			BufferedMessages = null;
		}
	}

	void IConnection.TransportReconnected()
	{
		if (State != ConnectionStates.Reconnecting)
		{
			return;
		}
		HTTPManager.Logger.Information("SignalR Connection", "Transport Reconnected");
		InitOnStart();
		if (this.OnReconnected == null)
		{
			return;
		}
		try
		{
			this.OnReconnected(this);
		}
		catch (Exception ex)
		{
			HTTPManager.Logger.Exception("SignalR Connection", "OnReconnected", ex);
		}
	}

	void IConnection.TransportAborted()
	{
		Close();
	}

	void IConnection.Error(string reason)
	{
		if (State == ConnectionStates.Closed)
		{
			return;
		}
		if (HTTPManager.IsQuitting)
		{
			Close();
			return;
		}
		HTTPManager.Logger.Error("SignalR Connection", reason);
		ReconnectStarted = false;
		if (this.OnError != null)
		{
			this.OnError(this, reason);
		}
		if (State == ConnectionStates.Connected || State == ConnectionStates.Reconnecting)
		{
			ReconnectDelayStartedAt = DateTime.UtcNow;
			if (State != ConnectionStates.Reconnecting)
			{
				ReconnectStartedAt = DateTime.UtcNow;
			}
		}
		else if (State != ConnectionStates.Connecting || !TryFallbackTransport())
		{
			Close();
		}
	}

	Uri IConnection.BuildUri(RequestTypes type)
	{
		return ((IConnection)this).BuildUri(type, (TransportBase)null);
	}

	Uri IConnection.BuildUri(RequestTypes type, TransportBase transport)
	{
		lock (SyncRoot)
		{
			queryBuilder.Length = 0;
			UriBuilder uriBuilder = new UriBuilder(Uri);
			if (!uriBuilder.Path.EndsWith("/"))
			{
				uriBuilder.Path += "/";
			}
			RequestCounter %= ulong.MaxValue;
			switch (type)
			{
			case RequestTypes.Negotiate:
				uriBuilder.Path += "negotiate";
				goto default;
			case RequestTypes.Connect:
				if (transport != null && transport.Type == TransportTypes.WebSocket)
				{
					uriBuilder.Scheme = (HTTPProtocolFactory.IsSecureProtocol(Uri) ? "wss" : "ws");
				}
				uriBuilder.Path += "connect";
				goto default;
			case RequestTypes.Start:
				uriBuilder.Path += "start";
				goto default;
			case RequestTypes.Poll:
				uriBuilder.Path += "poll";
				if (LastReceivedMessage != null)
				{
					queryBuilder.Append("messageId=");
					queryBuilder.Append(LastReceivedMessage.MessageId);
				}
				if (!string.IsNullOrEmpty(GroupsToken))
				{
					if (queryBuilder.Length > 0)
					{
						queryBuilder.Append("&");
					}
					queryBuilder.Append("groupsToken=");
					queryBuilder.Append(GroupsToken);
				}
				goto default;
			case RequestTypes.Send:
				uriBuilder.Path += "send";
				goto default;
			case RequestTypes.Reconnect:
				if (transport != null && transport.Type == TransportTypes.WebSocket)
				{
					uriBuilder.Scheme = (HTTPProtocolFactory.IsSecureProtocol(Uri) ? "wss" : "ws");
				}
				uriBuilder.Path += "reconnect";
				if (LastReceivedMessage != null)
				{
					queryBuilder.Append("messageId=");
					queryBuilder.Append(LastReceivedMessage.MessageId);
				}
				if (!string.IsNullOrEmpty(GroupsToken))
				{
					if (queryBuilder.Length > 0)
					{
						queryBuilder.Append("&");
					}
					queryBuilder.Append("groupsToken=");
					queryBuilder.Append(GroupsToken);
				}
				goto default;
			case RequestTypes.Abort:
				uriBuilder.Path += "abort";
				goto default;
			case RequestTypes.Ping:
				uriBuilder.Path += "ping";
				queryBuilder.Append("&tid=");
				queryBuilder.Append(RequestCounter++.ToString());
				queryBuilder.Append("&_=");
				queryBuilder.Append(Timestamp.ToString());
				break;
			default:
				if (queryBuilder.Length > 0)
				{
					queryBuilder.Append("&");
				}
				queryBuilder.Append("tid=");
				queryBuilder.Append(RequestCounter++.ToString());
				queryBuilder.Append("&_=");
				queryBuilder.Append(Timestamp.ToString());
				if (transport != null)
				{
					queryBuilder.Append("&transport=");
					queryBuilder.Append(transport.Name);
				}
				queryBuilder.Append("&clientProtocol=");
				queryBuilder.Append(ClientProtocols[(uint)Protocol]);
				if (NegotiationResult != null && !string.IsNullOrEmpty(NegotiationResult.ConnectionToken))
				{
					queryBuilder.Append("&connectionToken=");
					queryBuilder.Append(NegotiationResult.ConnectionToken);
				}
				if (Hubs != null && Hubs.Length != 0)
				{
					queryBuilder.Append("&connectionData=");
					queryBuilder.Append(ConnectionData);
				}
				break;
			}
			if (AdditionalQueryParams != null && AdditionalQueryParams.Count > 0)
			{
				queryBuilder.Append(QueryParams);
			}
			uriBuilder.Query = queryBuilder.ToString();
			queryBuilder.Length = 0;
			return uriBuilder.Uri;
		}
	}

	HTTPRequest IConnection.PrepareRequest(HTTPRequest req, RequestTypes type)
	{
		if (req != null && AuthenticationProvider != null)
		{
			AuthenticationProvider.PrepareRequest(req, type);
		}
		if (RequestPreparator != null)
		{
			RequestPreparator(this, req, type);
		}
		return req;
	}

	string IConnection.ParseResponse(string responseStr)
	{
		if (!(Json.Decode(responseStr) is Dictionary<string, object> dictionary))
		{
			((IConnection)this).Error("Failed to parse Start response: " + responseStr);
			return string.Empty;
		}
		if (!dictionary.TryGetValue("Response", out var value) || value == null)
		{
			((IConnection)this).Error("No 'Response' key found in response: " + responseStr);
			return string.Empty;
		}
		return value.ToString();
	}

	void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
	{
		switch (State)
		{
		case ConnectionStates.Connected:
			if (Transport.SupportsKeepAlive && NegotiationResult.KeepAliveTimeout.HasValue)
			{
				TimeSpan value = DateTime.UtcNow - LastMessageReceivedAt;
				TimeSpan? keepAliveTimeout = NegotiationResult.KeepAliveTimeout;
				if (value >= keepAliveTimeout)
				{
					Reconnect();
				}
			}
			if (PingRequest == null && DateTime.UtcNow - LastPingSentAt >= PingInterval)
			{
				Ping();
			}
			return;
		case ConnectionStates.Reconnecting:
			if (DateTime.UtcNow - ReconnectStartedAt >= NegotiationResult.DisconnectTimeout)
			{
				HTTPManager.Logger.Warning("SignalR Connection", "OnHeartbeatUpdate - Failed to reconnect in the given time!");
				Close();
			}
			else if (DateTime.UtcNow - ReconnectDelayStartedAt >= ReconnectDelay)
			{
				if ((int)HTTPManager.Logger.Level <= 2)
				{
					HTTPManager.Logger.Warning("SignalR Connection", ReconnectStarted + " " + ReconnectStartedAt.ToString() + " " + NegotiationResult.DisconnectTimeout);
				}
				Reconnect();
			}
			return;
		}
		if (TransportConnectionStartedAt.HasValue)
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime? transportConnectionStartedAt = TransportConnectionStartedAt;
			if (utcNow - transportConnectionStartedAt >= NegotiationResult.TransportConnectTimeout)
			{
				HTTPManager.Logger.Warning("SignalR Connection", "OnHeartbeatUpdate - Transport failed to connect in the given time!");
				((IConnection)this).Error("Transport failed to connect in the given time!");
			}
		}
	}

	private void InitOnStart()
	{
		State = ConnectionStates.Connected;
		ReconnectStarted = false;
		TransportConnectionStartedAt = null;
		LastPingSentAt = DateTime.UtcNow;
		LastMessageReceivedAt = DateTime.UtcNow;
		HTTPManager.Heartbeats.Subscribe(this);
	}

	private Hub FindHub(ulong msgId)
	{
		if (Hubs != null)
		{
			for (int i = 0; i < Hubs.Length; i++)
			{
				if (((IHub)Hubs[i]).HasSentMessageId(msgId))
				{
					return Hubs[i];
				}
			}
		}
		return null;
	}

	private bool TryFallbackTransport()
	{
		if (State == ConnectionStates.Connecting)
		{
			if (BufferedMessages != null)
			{
				BufferedMessages.Clear();
			}
			Transport.Stop();
			Transport = null;
			switch (NextProtocolToTry)
			{
			case SupportedProtocols.WebSocket:
				Transport = new WebSocketTransport(this);
				break;
			case SupportedProtocols.ServerSentEvents:
				Transport = new ServerSentEventsTransport(this);
				NextProtocolToTry = SupportedProtocols.HTTP;
				break;
			case SupportedProtocols.HTTP:
				Transport = new PollingTransport(this);
				NextProtocolToTry = SupportedProtocols.Unknown;
				break;
			case SupportedProtocols.Unknown:
				return false;
			}
			TransportConnectionStartedAt = DateTime.UtcNow;
			Transport.Connect();
			if (PingRequest != null)
			{
				PingRequest.Abort();
			}
			return true;
		}
		return false;
	}

	private void AdditionalQueryParams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		BuiltQueryParams = null;
	}

	private void Ping()
	{
		HTTPManager.Logger.Information("SignalR Connection", "Sending Ping request.");
		PingRequest = new HTTPRequest(((IConnection)this).BuildUri(RequestTypes.Ping), OnPingRequestFinished);
		PingRequest.ConnectTimeout = PingInterval;
		((IConnection)this).PrepareRequest(PingRequest, RequestTypes.Ping);
		PingRequest.Send();
		LastPingSentAt = DateTime.UtcNow;
	}

	private void OnPingRequestFinished(HTTPRequest req, HTTPResponse resp)
	{
		PingRequest = null;
		string text = string.Empty;
		switch (req.State)
		{
		case HTTPRequestStates.Finished:
			if (resp.IsSuccess)
			{
				string text2 = ((IConnection)this).ParseResponse(resp.DataAsText);
				if (text2 != "pong")
				{
					text = "Wrong answer for ping request: " + text2;
				}
				else
				{
					HTTPManager.Logger.Information("SignalR Connection", "Pong received.");
				}
			}
			else
			{
				text = $"Ping - Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
			}
			break;
		case HTTPRequestStates.Error:
			text = "Ping - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
			break;
		case HTTPRequestStates.ConnectionTimedOut:
			text = "Ping - Connection Timed Out!";
			break;
		case HTTPRequestStates.TimedOut:
			text = "Ping - Processing the request Timed Out!";
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			((IConnection)this).Error(text);
		}
	}
}
