using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Joker;

namespace MiniGame.Core.Server;

public class PlayerPingDebugSession : PlayerSession
{
	protected int _minDelay;

	protected int _maxDelay;

	protected object _timeLock = new object();

	protected object _sendLock = new object();

	protected object _recvLock = new object();

	protected LinkedList<(long time, IMessage message)> _pingSendQueue = new LinkedList<(long, IMessage)>();

	protected LinkedList<(long time, IMessage message)> _pingRecvQueue = new LinkedList<(long, IMessage)>();

	protected Stopwatch _timer = new Stopwatch();

	protected Random _random = new Random();

	protected System.Threading.Tasks.Task _pingTask;

	protected CancellationTokenSource _cts;

	public PlayerPingDebugSession(int ping, float pingJitter, IGameServer server, NetworkSystem owner, long channelID, IPEndPoint ipEndPoint)
		: base(server, owner, channelID, ipEndPoint)
	{
		_minDelay = Math.Max((int)((float)ping * 0.5f * (1f - pingJitter)), 0);
		_maxDelay = (int)((float)ping * 0.5f * (1f + pingJitter));
		_timer.Start();
		_cts = new CancellationTokenSource();
		CancellationToken token = _cts.Token;
		_pingTask = System.Threading.Tasks.Task.Run(async delegate
		{
			while (!token.IsCancellationRequested)
			{
				long num = 0L;
				lock (_timeLock)
				{
					if (_timer == null)
					{
						break;
					}
					num = _timer.ElapsedMilliseconds;
				}
				lock (_sendLock)
				{
					if (_pingSendQueue.Count > 0)
					{
						LinkedListNode<(long, IMessage)> first = _pingSendQueue.First;
						if (first.Value.Item1 <= num)
						{
							_pingSendQueue.RemoveFirst();
							IMessage msg = first.Value.Item2;
							Singleton<ThreadService>.Instance.PostMainThread(delegate
							{
								if (base.Owner.IsChannelValid(base.ChannelID))
								{
									base.SendMessage(msg);
								}
							});
						}
					}
				}
				lock (_recvLock)
				{
					if (_pingRecvQueue.Count > 0)
					{
						LinkedListNode<(long, IMessage)> first2 = _pingRecvQueue.First;
						if (first2.Value.Item1 <= num)
						{
							_pingRecvQueue.RemoveFirst();
							IMessage msg2 = first2.Value.Item2;
							Singleton<ThreadService>.Instance.PostMainThread(delegate
							{
								if (base.Owner.IsChannelValid(base.ChannelID))
								{
									base.HandleMessage(msg2);
								}
							});
						}
					}
				}
				await System.Threading.Tasks.Task.Delay(10);
			}
		}, token);
	}

	public override void SendMessage(IMessage message)
	{
		if (!base.Owner.IsChannelValid(base.ChannelID))
		{
			return;
		}
		long num = _random.Next(_minDelay, _maxDelay + 1);
		long num2 = 0L;
		lock (_timeLock)
		{
			num2 = num + _timer.ElapsedMilliseconds;
		}
		lock (_sendLock)
		{
			if (_pingSendQueue.Count > 0 && _pingSendQueue.Last.Value.time > num2)
			{
				num2 = _pingSendQueue.Last.Value.time + 10;
			}
			_pingSendQueue.AddLast((num2, message));
		}
	}

	public override void HandleMessage(IMessage message)
	{
		if (!base.Owner.IsChannelValid(base.ChannelID))
		{
			return;
		}
		long num = _random.Next(_minDelay, _maxDelay + 1);
		long num2 = 0L;
		lock (_timeLock)
		{
			num2 = num + _timer.ElapsedMilliseconds;
		}
		lock (_recvLock)
		{
			if (_pingRecvQueue.Count > 0 && _pingRecvQueue.Last.Value.time > num2)
			{
				num2 = _pingRecvQueue.Last.Value.time + 10;
			}
			_pingRecvQueue.AddLast((num2, message));
		}
	}

	public override void Dispose()
	{
		if (base.IsValid)
		{
			_cts?.Cancel();
			_pingTask = null;
			lock (_timeLock)
			{
				_timer?.Stop();
				_timer = null;
			}
			base.Dispose();
			if (!string.IsNullOrEmpty(base.PlayerSessionID))
			{
				_server.LogoutPlayer(this);
			}
			_server.DisconnectPlayer(this);
		}
	}
}
