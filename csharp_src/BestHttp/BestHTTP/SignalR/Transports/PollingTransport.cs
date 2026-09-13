using System;
using BestHTTP.Extensions;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Transports;

public sealed class PollingTransport : PostSendTransportBase, IHeartbeat
{
	private DateTime LastPoll;

	private TimeSpan PollDelay;

	private TimeSpan PollTimeout;

	private HTTPRequest pollRequest;

	public override bool SupportsKeepAlive => false;

	public override TransportTypes Type => TransportTypes.LongPoll;

	public PollingTransport(Connection connection)
		: base("longPolling", connection)
	{
		LastPoll = DateTime.MinValue;
		PollTimeout = connection.NegotiationResult.ConnectionTimeout + TimeSpan.FromSeconds(10.0);
	}

	public override void Connect()
	{
		HTTPManager.Logger.Information("Transport - " + base.Name, "Sending Open Request");
		if (base.State != TransportStates.Reconnecting)
		{
			base.State = TransportStates.Connecting;
		}
		RequestTypes type = ((base.State != TransportStates.Reconnecting) ? RequestTypes.Connect : RequestTypes.Reconnect);
		HTTPRequest hTTPRequest = new HTTPRequest(base.Connection.BuildUri(type, this), HTTPMethods.Get, isKeepAlive: true, disableCache: true, OnConnectRequestFinished);
		base.Connection.PrepareRequest(hTTPRequest, type);
		hTTPRequest.Send();
	}

	public override void Stop()
	{
		HTTPManager.Heartbeats.Unsubscribe(this);
		if (pollRequest != null)
		{
			pollRequest.Abort();
			pollRequest = null;
		}
	}

	protected override void Started()
	{
		LastPoll = DateTime.UtcNow;
		HTTPManager.Heartbeats.Subscribe(this);
	}

	protected override void Aborted()
	{
		HTTPManager.Heartbeats.Unsubscribe(this);
	}

	private void OnConnectRequestFinished(HTTPRequest req, HTTPResponse resp)
	{
		string text = string.Empty;
		switch (req.State)
		{
		case HTTPRequestStates.Finished:
			if (resp.IsSuccess)
			{
				HTTPManager.Logger.Information("Transport - " + base.Name, "Connect - Request Finished Successfully! " + resp.DataAsText);
				OnConnected();
				IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
				if (serverMessage != null)
				{
					base.Connection.OnMessage(serverMessage);
					if (serverMessage is MultiMessage { PollDelay: not null } multiMessage)
					{
						PollDelay = multiMessage.PollDelay.Value;
					}
				}
			}
			else
			{
				text = $"Connect - Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
			}
			break;
		case HTTPRequestStates.Error:
			text = "Connect - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
			break;
		case HTTPRequestStates.Aborted:
			text = "Connect - Request Aborted!";
			break;
		case HTTPRequestStates.ConnectionTimedOut:
			text = "Connect - Connection Timed Out!";
			break;
		case HTTPRequestStates.TimedOut:
			text = "Connect - Processing the request Timed Out!";
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			base.Connection.Error(text);
		}
	}

	private void OnPollRequestFinished(HTTPRequest req, HTTPResponse resp)
	{
		if (req.State == HTTPRequestStates.Aborted)
		{
			HTTPManager.Logger.Warning("Transport - " + base.Name, "Poll - Request Aborted!");
			return;
		}
		pollRequest = null;
		string text = string.Empty;
		switch (req.State)
		{
		case HTTPRequestStates.Finished:
			if (resp.IsSuccess)
			{
				HTTPManager.Logger.Information("Transport - " + base.Name, "Poll - Request Finished Successfully! " + resp.DataAsText);
				IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
				if (serverMessage != null)
				{
					base.Connection.OnMessage(serverMessage);
					if (serverMessage is MultiMessage { PollDelay: not null } multiMessage)
					{
						PollDelay = multiMessage.PollDelay.Value;
					}
					LastPoll = DateTime.UtcNow;
				}
			}
			else
			{
				text = $"Poll - Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
			}
			break;
		case HTTPRequestStates.Error:
			text = "Poll - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
			break;
		case HTTPRequestStates.ConnectionTimedOut:
			text = "Poll - Connection Timed Out!";
			break;
		case HTTPRequestStates.TimedOut:
			text = "Poll - Processing the request Timed Out!";
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			base.Connection.Error(text);
		}
	}

	private void Poll()
	{
		pollRequest = new HTTPRequest(base.Connection.BuildUri(RequestTypes.Poll, this), HTTPMethods.Get, isKeepAlive: true, disableCache: true, OnPollRequestFinished);
		base.Connection.PrepareRequest(pollRequest, RequestTypes.Poll);
		pollRequest.Timeout = PollTimeout;
		pollRequest.Send();
	}

	void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
	{
		TransportStates state = base.State;
		if (state == TransportStates.Started && pollRequest == null && DateTime.UtcNow >= LastPoll + PollDelay + base.Connection.NegotiationResult.LongPollDelay)
		{
			Poll();
		}
	}
}
