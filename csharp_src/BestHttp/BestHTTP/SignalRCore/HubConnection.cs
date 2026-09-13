using System;
using System.Collections.Generic;
using System.Threading;
using BestHTTP.Extensions;
using BestHTTP.Futures;
using BestHTTP.SignalRCore.Authentication;
using BestHTTP.SignalRCore.Messages;
using BestHTTP.SignalRCore.Transports;

namespace BestHTTP.SignalRCore;

public sealed class HubConnection : IHeartbeat
{
	public static readonly object[] EmptyArgs = new object[0];

	private long lastInvocationId;

	private Dictionary<long, Action<Message>> invocations = new Dictionary<long, Action<Message>>();

	private Dictionary<string, Subscription> subscriptions = new Dictionary<string, Subscription>(StringComparer.OrdinalIgnoreCase);

	private DateTime lastMessageSent;

	public Uri Uri { get; private set; }

	public ConnectionStates State { get; private set; }

	public ITransport Transport { get; private set; }

	public IProtocol Protocol { get; private set; }

	public IAuthenticationProvider AuthenticationProvider { get; set; }

	public NegotiationResult NegotiationResult { get; private set; }

	public HubOptions Options { get; private set; }

	public int RedirectCount { get; private set; }

	public event Action<HubConnection, Uri, Uri> OnRedirected;

	public event Action<HubConnection> OnConnected;

	public event Action<HubConnection, string> OnError;

	public event Action<HubConnection> OnClosed;

	public event Func<HubConnection, Message, bool> OnMessage;

	public HubConnection(Uri hubUri, IProtocol protocol)
		: this(hubUri, protocol, new HubOptions())
	{
	}

	public HubConnection(Uri hubUri, IProtocol protocol, HubOptions options)
	{
		Uri = hubUri;
		State = ConnectionStates.Initial;
		Options = options;
		Protocol = protocol;
		Protocol.Connection = this;
		AuthenticationProvider = new DefaultAccessTokenAuthenticator(this);
	}

	public void StartConnect()
	{
		if (State != ConnectionStates.Initial && State != ConnectionStates.Redirected)
		{
			HTTPManager.Logger.Warning("HubConnection", "StartConnect - Expected Initial or Redirected state, got " + State);
			return;
		}
		HTTPManager.Logger.Verbose("HubConnection", "StartConnect");
		if (AuthenticationProvider != null && AuthenticationProvider.IsPreAuthRequired)
		{
			HTTPManager.Logger.Information("HubConnection", "StartConnect - Authenticating");
			SetState(ConnectionStates.Authenticating);
			AuthenticationProvider.OnAuthenticationSucceded += OnAuthenticationSucceded;
			AuthenticationProvider.OnAuthenticationFailed += OnAuthenticationFailed;
			AuthenticationProvider.StartAuthentication();
		}
		else
		{
			StartNegotiation();
		}
	}

	private void OnAuthenticationSucceded(IAuthenticationProvider provider)
	{
		HTTPManager.Logger.Verbose("HubConnection", "OnAuthenticationSucceded");
		AuthenticationProvider.OnAuthenticationSucceded -= OnAuthenticationSucceded;
		AuthenticationProvider.OnAuthenticationFailed -= OnAuthenticationFailed;
		StartNegotiation();
	}

	private void OnAuthenticationFailed(IAuthenticationProvider provider, string reason)
	{
		HTTPManager.Logger.Error("HubConnection", "OnAuthenticationFailed: " + reason);
		AuthenticationProvider.OnAuthenticationSucceded -= OnAuthenticationSucceded;
		AuthenticationProvider.OnAuthenticationFailed -= OnAuthenticationFailed;
		SetState(ConnectionStates.Closed, reason);
	}

	private void StartNegotiation()
	{
		HTTPManager.Logger.Verbose("HubConnection", "StartNegotiation");
		if (State == ConnectionStates.CloseInitiated)
		{
			SetState(ConnectionStates.Closed);
			return;
		}
		if (Options.SkipNegotiation)
		{
			HTTPManager.Logger.Verbose("HubConnection", "Skipping negotiation");
			ConnectImpl();
			return;
		}
		SetState(ConnectionStates.Negotiating);
		UriBuilder uriBuilder = new UriBuilder(Uri);
		uriBuilder.Path += "/negotiate";
		HTTPRequest hTTPRequest = new HTTPRequest(uriBuilder.Uri, HTTPMethods.Post, OnNegotiationRequestFinished);
		if (AuthenticationProvider != null)
		{
			AuthenticationProvider.PrepareRequest(hTTPRequest);
		}
		hTTPRequest.Send();
	}

	private void ConnectImpl()
	{
		HTTPManager.Logger.Verbose("HubConnection", "ConnectImpl");
		if (Options.PreferedTransport == TransportTypes.WebSocket)
		{
			if (NegotiationResult != null && !IsTransportSupported("WebSockets"))
			{
				SetState(ConnectionStates.Closed, "The 'WebSockets' transport isn't supported by the server!");
				return;
			}
			Transport = new WebSocketTransport(this);
			Transport.OnStateChanged += Transport_OnStateChanged;
		}
		else
		{
			SetState(ConnectionStates.Closed, "Unsupportted transport: " + Options.PreferedTransport);
		}
		Transport.StartConnect();
	}

	private bool IsTransportSupported(string transportName)
	{
		if (NegotiationResult.SupportedTransports == null)
		{
			return true;
		}
		for (int i = 0; i < NegotiationResult.SupportedTransports.Count; i++)
		{
			if (NegotiationResult.SupportedTransports[i].Name == transportName)
			{
				return true;
			}
		}
		return false;
	}

	private void OnNegotiationRequestFinished(HTTPRequest req, HTTPResponse resp)
	{
		if (State == ConnectionStates.CloseInitiated)
		{
			SetState(ConnectionStates.Closed);
			return;
		}
		string error = null;
		switch (req.State)
		{
		case HTTPRequestStates.Finished:
			if (resp.IsSuccess)
			{
				HTTPManager.Logger.Information("HubConnection", "Negotiation Request Finished Successfully! Response: " + resp.DataAsText);
				NegotiationResult = NegotiationResult.Parse(resp.DataAsText, out error, this);
				if (!string.IsNullOrEmpty(error))
				{
					break;
				}
				if (NegotiationResult.Url != null)
				{
					SetState(ConnectionStates.Redirected);
					if (++RedirectCount >= Options.MaxRedirects)
					{
						error = $"MaxRedirects ({Options.MaxRedirects:N0}) reached!";
						break;
					}
					Uri uri = Uri;
					Uri = NegotiationResult.Url;
					if (this.OnRedirected != null)
					{
						try
						{
							this.OnRedirected(this, uri, Uri);
						}
						catch (Exception ex)
						{
							HTTPManager.Logger.Exception("HubConnection", "OnNegotiationRequestFinished - OnRedirected", ex);
						}
					}
					StartConnect();
				}
				else
				{
					ConnectImpl();
				}
			}
			else
			{
				error = $"Negotiation Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
			}
			break;
		case HTTPRequestStates.Error:
			error = "Negotiation Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
			break;
		case HTTPRequestStates.Aborted:
			error = "Negotiation Request Aborted!";
			break;
		case HTTPRequestStates.ConnectionTimedOut:
			error = "Negotiation Request - Connection Timed Out!";
			break;
		case HTTPRequestStates.TimedOut:
			error = "Negotiation Request - Processing the request Timed Out!";
			break;
		}
		if (error != null)
		{
			SetState(ConnectionStates.Closed, error);
		}
	}

	public void StartClose()
	{
		HTTPManager.Logger.Verbose("HubConnection", "StartClose");
		SetState(ConnectionStates.CloseInitiated);
		if (Transport != null)
		{
			Transport.StartClose();
		}
	}

	public IFuture<StreamItemContainer<TResult>> Stream<TResult>(string target, params object[] args)
	{
		Future<StreamItemContainer<TResult>> future = new Future<StreamItemContainer<TResult>>();
		long id = InvokeImp(target, args, delegate(Message message)
		{
			switch (message.type)
			{
			case MessageTypes.StreamItem:
			{
				StreamItemContainer<TResult> value2 = future.value;
				if (!value2.IsCanceled)
				{
					value2.AddItem((TResult)Protocol.ConvertTo(typeof(TResult), message.item));
					future.AssignItem(value2);
				}
				break;
			}
			case MessageTypes.Completion:
				if (string.IsNullOrEmpty(message.error))
				{
					StreamItemContainer<TResult> value = future.value;
					future.Assign(value);
				}
				else
				{
					future.Fail(new Exception(message.error));
				}
				break;
			}
		}, isStreamingInvocation: true);
		future.BeginProcess(new StreamItemContainer<TResult>(id));
		return future;
	}

	public void CancelStream<T>(StreamItemContainer<T> container)
	{
		Message message = new Message
		{
			type = MessageTypes.CancelInvocation
		};
		long id = container.id;
		message.invocationId = id.ToString();
		Message message2 = message;
		container.IsCanceled = true;
		SendMessage(message2);
	}

	public IFuture<TResult> Invoke<TResult>(string target, params object[] args)
	{
		Future<TResult> future = new Future<TResult>();
		InvokeImp(target, args, delegate(Message message)
		{
			if (string.IsNullOrEmpty(message.error))
			{
				future.Assign((TResult)Protocol.ConvertTo(typeof(TResult), message.result));
			}
			else
			{
				future.Fail(new Exception(message.error));
			}
		});
		return future;
	}

	public IFuture<bool> Send(string target, params object[] args)
	{
		Future<bool> future = new Future<bool>();
		InvokeImp(target, args, delegate(Message message)
		{
			if (string.IsNullOrEmpty(message.error))
			{
				future.Assign(value: true);
			}
			else
			{
				future.Fail(new Exception(message.error));
			}
		});
		return future;
	}

	private long InvokeImp(string target, object[] args, Action<Message> callback, bool isStreamingInvocation = false)
	{
		if (State != ConnectionStates.Connected)
		{
			return -1L;
		}
		long num = Interlocked.Increment(ref lastInvocationId);
		Message message = new Message
		{
			type = ((!isStreamingInvocation) ? MessageTypes.Invocation : MessageTypes.StreamInvocation),
			invocationId = num.ToString(),
			target = target,
			arguments = args,
			nonblocking = (callback == null)
		};
		SendMessage(message);
		if (callback != null)
		{
			invocations.Add(num, callback);
		}
		return num;
	}

	private void SendMessage(Message message)
	{
		byte[] msg = Protocol.EncodeMessage(message);
		Transport.Send(msg);
		lastMessageSent = DateTime.UtcNow;
	}

	public void On(string methodName, Action callback)
	{
		On(methodName, null, delegate
		{
			callback();
		});
	}

	public void On<T1>(string methodName, Action<T1> callback)
	{
		On(methodName, new Type[1] { typeof(T1) }, delegate(object[] args)
		{
			callback((T1)args[0]);
		});
	}

	public void On<T1, T2>(string methodName, Action<T1, T2> callback)
	{
		On(methodName, new Type[2]
		{
			typeof(T1),
			typeof(T2)
		}, delegate(object[] args)
		{
			callback((T1)args[0], (T2)args[1]);
		});
	}

	public void On<T1, T2, T3>(string methodName, Action<T1, T2, T3> callback)
	{
		On(methodName, new Type[3]
		{
			typeof(T1),
			typeof(T2),
			typeof(T3)
		}, delegate(object[] args)
		{
			callback((T1)args[0], (T2)args[1], (T3)args[2]);
		});
	}

	public void On<T1, T2, T3, T4>(string methodName, Action<T1, T2, T3, T4> callback)
	{
		On(methodName, new Type[4]
		{
			typeof(T1),
			typeof(T2),
			typeof(T3),
			typeof(T4)
		}, delegate(object[] args)
		{
			callback((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
		});
	}

	public void On(string methodName, Type[] paramTypes, Action<object[]> callback)
	{
		Subscription value = null;
		if (!subscriptions.TryGetValue(methodName, out value))
		{
			subscriptions.Add(methodName, value = new Subscription());
		}
		value.Add(paramTypes, callback);
	}

	internal void OnMessages(List<Message> messages)
	{
		for (int i = 0; i < messages.Count; i++)
		{
			Message message = messages[i];
			try
			{
				if (this.OnMessage != null && !this.OnMessage(this, message))
				{
					break;
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HubConnection", "Exception in OnMessage user code!", ex);
			}
			switch (message.type)
			{
			case MessageTypes.Invocation:
			{
				Subscription value2 = null;
				if (!subscriptions.TryGetValue(message.target, out value2))
				{
					break;
				}
				for (int j = 0; j < value2.callbacks.Count; j++)
				{
					CallbackDescriptor callbackDescriptor = value2.callbacks[j];
					object[] obj = null;
					try
					{
						obj = Protocol.GetRealArguments(callbackDescriptor.ParamTypes, message.arguments);
					}
					catch (Exception ex3)
					{
						HTTPManager.Logger.Exception("HubConnection", "OnMessages - Invocation - GetRealArguments", ex3);
					}
					try
					{
						callbackDescriptor.Callback(obj);
					}
					catch (Exception ex4)
					{
						HTTPManager.Logger.Exception("HubConnection", "OnMessages - Invocation - Invoke", ex4);
					}
				}
				break;
			}
			case MessageTypes.StreamItem:
			{
				if (long.TryParse(message.invocationId, out var result2) && invocations.TryGetValue(result2, out var value3) && value3 != null)
				{
					try
					{
						value3(message);
					}
					catch (Exception ex5)
					{
						HTTPManager.Logger.Exception("HubConnection", "OnMessages - StreamItem - callback", ex5);
					}
				}
				break;
			}
			case MessageTypes.Completion:
			{
				if (!long.TryParse(message.invocationId, out var result))
				{
					break;
				}
				if (invocations.TryGetValue(result, out var value) && value != null)
				{
					try
					{
						value(message);
					}
					catch (Exception ex2)
					{
						HTTPManager.Logger.Exception("HubConnection", "OnMessages - Completion - callback", ex2);
					}
				}
				invocations.Remove(result);
				break;
			}
			case MessageTypes.Close:
				SetState(ConnectionStates.Closed, message.error);
				break;
			}
		}
	}

	private void Transport_OnStateChanged(TransportStates oldState, TransportStates newState)
	{
		HTTPManager.Logger.Verbose("HubConnection", $"Transport_OnStateChanged - oldState: {oldState.ToString()} newState: {newState.ToString()}");
		switch (newState)
		{
		case TransportStates.Connected:
			SetState(ConnectionStates.Connected);
			break;
		case TransportStates.Failed:
			SetState(ConnectionStates.Closed, Transport.ErrorReason);
			break;
		case TransportStates.Closed:
			SetState(ConnectionStates.Closed);
			break;
		case TransportStates.Closing:
			break;
		}
	}

	private void SetState(ConnectionStates state, string errorReason = null)
	{
		if (string.IsNullOrEmpty(errorReason))
		{
			HTTPManager.Logger.Information("HubConnection", "SetState - from State: '" + State.ToString() + "' to State: '" + state.ToString() + "'");
		}
		else
		{
			HTTPManager.Logger.Information("HubConnection", "SetState - from State: '" + State.ToString() + "' to State: '" + state.ToString() + "' errorReason: '" + errorReason + "'");
		}
		if (State == state)
		{
			return;
		}
		State = state;
		switch (state)
		{
		case ConnectionStates.Connected:
			try
			{
				if (this.OnConnected != null)
				{
					this.OnConnected(this);
				}
			}
			catch (Exception ex3)
			{
				HTTPManager.Logger.Exception("HubConnection", "Exception in OnConnected user code!", ex3);
			}
			HTTPManager.Heartbeats.Subscribe(this);
			lastMessageSent = DateTime.UtcNow;
			break;
		case ConnectionStates.Closed:
			if (string.IsNullOrEmpty(errorReason))
			{
				if (this.OnClosed != null)
				{
					try
					{
						this.OnClosed(this);
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("HubConnection", "Exception in OnClosed user code!", ex);
					}
				}
			}
			else if (this.OnError != null)
			{
				try
				{
					this.OnError(this, errorReason);
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("HubConnection", "Exception in OnError user code!", ex2);
				}
			}
			HTTPManager.Heartbeats.Unsubscribe(this);
			break;
		case ConnectionStates.Initial:
		case ConnectionStates.Authenticating:
		case ConnectionStates.Negotiating:
		case ConnectionStates.Redirected:
		case ConnectionStates.CloseInitiated:
			break;
		}
	}

	void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
	{
		ConnectionStates state = State;
		if (state == ConnectionStates.Connected && Options.PingInterval != TimeSpan.Zero && DateTime.UtcNow - lastMessageSent >= Options.PingInterval)
		{
			SendMessage(new Message
			{
				type = MessageTypes.Ping
			});
		}
	}
}
