using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using BaseUtils;
using GameFramework;
using Sfs2X.Bitswarm;
using Sfs2X.Entities.Data;
using Sfs2X.Exceptions;
using Sfs2X.Requests;
using ZstdNet;

namespace ProtoBufNet;

public class NetConnection : INetConnection, IDisposable
{
	private NetIOService m_service;

	private LinkedList<NetPacket> m_packets_send;

	private Socket m_socket;

	private MessageDispather m_dispatch;

	private string m_name;

	private int m_send_offset;

	private bool m_disposed;

	private int m_sendId;

	private NetProfiler m_profiler;

	public ushort m_sid;

	private const int DEFAULT_RECEIVE_SIZE = 524288;

	private const int HUGE_RECEIVE_SIZE = 2097152;

	private const int DEFAULT_SEND_SIZE = 65536;

	private static bool USE_HUGE_RECEIVE_SIZE = true;

	private Decompressor _ztsdDecompressor;

	internal Socket socket => m_socket;

	public bool IsConnected
	{
		get
		{
			if (m_socket != null)
			{
				return m_socket.Connected;
			}
			return false;
		}
	}

	public long lastRecvTick { get; set; }

	public NetConnection(NetIOService service, MessageDispather dispatch, string name, NetProfiler profiler, ushort sid)
	{
		m_service = service;
		m_dispatch = dispatch;
		m_name = name;
		m_packets_send = new LinkedList<NetPacket>();
		m_profiler = profiler;
		m_sid = sid;
	}

	~NetConnection()
	{
		Dispose(disposing: true);
	}

	internal void Init()
	{
	}

	public void BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
	{
		if (m_socket != null)
		{
			NetLogError("Connect when socket != null");
			m_socket.Close();
		}
		if (_ztsdDecompressor != null)
		{
			_ztsdDecompressor.Dispose();
		}
		if (NetPacketConst.useNewPacket && NetPacketConst.useZStd)
		{
			_ztsdDecompressor = new Decompressor();
		}
		m_socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		m_socket.NoDelay = true;
		if (USE_HUGE_RECEIVE_SIZE)
		{
			m_socket.ReceiveBufferSize = 2097152;
		}
		else
		{
			m_socket.ReceiveBufferSize = 524288;
		}
		m_socket.SendBufferSize = 65536;
		m_socket.ReceiveTimeout = 30000;
		m_socket.SendTimeout = 30000;
		m_socket.BeginConnect(address, port, requestCallback, state);
	}

	public void EndConnect(IAsyncResult ar)
	{
		if (m_socket != null)
		{
			m_socket.EndConnect(ar);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		NetLogInfo("connect Dispose");
		if (m_disposed)
		{
			return;
		}
		m_disposed = true;
		if (disposing)
		{
			_ztsdDecompressor?.Dispose();
			_ztsdDecompressor = null;
			if (m_socket != null)
			{
				m_socket.Close();
				m_socket.Dispose();
				m_socket = null;
			}
			m_dispatch = null;
			lock (m_packets_send)
			{
				m_packets_send = null;
			}
			m_service = null;
		}
	}

	public bool IsDisposed()
	{
		return m_disposed;
	}

	public bool SendMessage(IRequest request)
	{
		if (!IsConnected)
		{
			NetLogError("You are not connected. Request cannot be send");
			return false;
		}
		try
		{
			request.Execute(null);
			sendPackage(request.Message);
		}
		catch (SFSValidationError sFSValidationError)
		{
			string text = sFSValidationError.Message;
			foreach (string error in sFSValidationError.Errors)
			{
				text = text + "\t" + error + "\n";
			}
			NetLogError(text);
		}
		catch (SFSCodecError arg)
		{
			NetLogError($"send error {arg}");
		}
		return true;
	}

	private void sendPackage(IMessage message)
	{
		NetPacket netPacket = new NetPacket(m_profiler, m_sid, read: false);
		ISFSObject iSFSObject = new SFSObject();
		iSFSObject.PutByte(MessageDispather.CONTROLLER_ID, Convert.ToByte(message.TargetController));
		iSFSObject.PutShort(MessageDispather.ACTION_ID, Convert.ToInt16(message.Id));
		iSFSObject.PutSFSObject(MessageDispather.PARAM_ID, message.Content);
		message.Content = iSFSObject;
		if (message.Id == 1)
		{
			netPacket.logType = "login";
		}
		if (DebugUtils.logClinetSendMsgDetail && message.Content.GetByte("c") != 0)
		{
			Log.Info($"<color=#FFA510>[Msg Send]{message.Content.ToJson()}</color>");
		}
		netPacket.onBeforeSend(message);
		SendPacket(netPacket);
	}

	public void Close(bool graceful, string reason)
	{
		NetLogInfo($"shutdown m_socket reason:{reason}, socket connected:{m_socket?.Connected}");
		if (m_socket == null || !m_socket.Connected)
		{
			return;
		}
		try
		{
			NetLogInfo("shutdown m_socket");
			m_socket.Shutdown(SocketShutdown.Both);
		}
		catch (SocketException ex)
		{
			NetLogError($"shutdown m_socket Exception:{ex.SocketErrorCode} {ex}");
		}
	}

	private void SendPacket(NetPacket p)
	{
		try
		{
			lock (m_packets_send)
			{
				m_packets_send.AddLast(p);
				if (m_packets_send.Count == 1)
				{
					PostSend();
				}
			}
		}
		catch (SocketException ex)
		{
			Error("Connection error: " + ex.Message + " " + ex.StackTrace, ex.SocketErrorCode);
		}
	}

	private void PostSend()
	{
		if (m_packets_send.Count != 0)
		{
			NetPacket value = m_packets_send.First.Value;
			m_send_offset = 0;
			value.onPackageSendBegin();
			m_socket.BeginSend(value.Buffer.Bytes, m_send_offset, value.Buffer.Length, SocketFlags.None, HandlePackageSend, value);
			if (!string.IsNullOrEmpty(value.logType))
			{
				PostLog(LogLevel.Info, "NetConnection:" + value.logType + " BeginSend");
			}
		}
	}

	private static INetPacket CreateOldPacket(NetProfiler profiler, ushort sid)
	{
		return new NetPacket(profiler, sid);
	}

	private static INetPacket CreateNewPacket(NetProfiler profiler, Decompressor decompressor, ushort sid)
	{
		return new NetPacketNew(decompressor, profiler, sid);
	}

	internal void PostRecv(ushort sid)
	{
		INetPacket netPacket = ((!NetPacketConst.useNewPacket) ? CreateOldPacket(m_profiler, sid) : CreateNewPacket(m_profiler, _ztsdDecompressor, sid));
		netPacket.headerBuffInfo(out var info);
		m_socket.BeginReceive(info.buffer, info.offset, info.length, SocketFlags.None, HandleHeaderRecv, netPacket);
	}

	private void HandleHeaderRecv(IAsyncResult ar)
	{
		INetPacket netPacket = ar.AsyncState as INetPacket;
		try
		{
			if (m_socket == null || IsDisposed())
			{
				netPacket?.Dispose();
				return;
			}
			if (m_socket.EndReceive(ar) < 1)
			{
				netPacket.headerBuffInfo(out var info);
				m_socket.BeginReceive(info.buffer, info.offset, info.length, SocketFlags.None, HandleHeaderRecv, netPacket);
				return;
			}
			netPacket.parseHeader();
			netPacket.bodyLengthBuffInfo(out var info2);
			netPacket.onPackageReceiveBegin();
			m_socket.BeginReceive(info2.buffer, info2.offset, info2.length, SocketFlags.None, HandleBodyLengthRecv, netPacket);
		}
		catch (SocketException ex)
		{
			netPacket?.Dispose();
			Error("Connection error: " + ex.Message + " " + ex.StackTrace, ex.SocketErrorCode);
		}
	}

	private void HandleBodyLengthRecv(IAsyncResult ar)
	{
		INetPacket netPacket = ar.AsyncState as INetPacket;
		try
		{
			int num = m_socket.EndReceive(ar);
			netPacket.bodyLengthHasRead += num;
			netPacket.bodyLengthBuffInfo(out var info);
			if (netPacket.bodyLengthHasRead < info.length)
			{
				m_socket.BeginReceive(info.buffer, info.offset + netPacket.bodyLengthHasRead, info.length - netPacket.bodyLengthHasRead, SocketFlags.None, HandleBodyLengthRecv, netPacket);
				return;
			}
			netPacket.parseBodyLength();
			m_socket.BeginReceive(netPacket.bodyBuffBytes(), 0, netPacket.bodyBuffLength(), SocketFlags.None, HandleBodyRecv, netPacket);
		}
		catch (SocketException ex)
		{
			netPacket?.Dispose();
			Error("Connection error: " + ex.Message + " " + ex.StackTrace, ex.SocketErrorCode);
		}
	}

	private void HandleBodyRecv(IAsyncResult ar)
	{
		INetPacket netPacket = ar.AsyncState as INetPacket;
		try
		{
			if (m_socket == null || IsDisposed())
			{
				netPacket?.Dispose();
				return;
			}
			int num = m_socket.EndReceive(ar);
			netPacket.bodyHasRead += num;
			if (netPacket.bodyHasRead < netPacket.bodyBuffLength())
			{
				m_socket.BeginReceive(netPacket.bodyBuffBytes(), netPacket.bodyHasRead, netPacket.bodyBuffLength() - netPacket.bodyHasRead, SocketFlags.None, HandleBodyRecv, netPacket);
				return;
			}
			netPacket.onPackageReceiveFinish();
			FinishRecvPacket(netPacket);
		}
		catch (SocketException ex)
		{
			netPacket?.Dispose();
			Error("Connection error: " + ex.Message + " " + ex.StackTrace, ex.SocketErrorCode);
		}
	}

	private void FinishRecvPacket(INetPacket p)
	{
		p.recvTime = RealTimer.elapsedMilliseconds;
		lastRecvTick = DateTime.Now.Ticks / 10000;
		if (m_service == null)
		{
			return;
		}
		m_service.Post(delegate
		{
			if (m_dispatch != null)
			{
				m_profiler.OnReceiveDispatchBegin(p);
				m_dispatch.Dispatch(this, p);
				m_profiler.OnReceiveDispatchFinish(p);
			}
		});
		PostRecv(m_sid);
	}

	private void HandlePackageSend(IAsyncResult ar)
	{
		try
		{
			if (IsDisposed() || m_socket == null)
			{
				return;
			}
			int num = m_socket.EndSend(ar);
			NetPacket netPacket = ar.AsyncState as NetPacket;
			m_send_offset += num;
			if (m_send_offset < netPacket.Buffer.Length)
			{
				m_socket.BeginSend(netPacket.Buffer.Bytes, m_send_offset, netPacket.Buffer.Length - m_send_offset, SocketFlags.None, HandlePackageSend, netPacket);
				if (!string.IsNullOrEmpty(netPacket.logType))
				{
					PostLog(LogLevel.Info, $"NetConnection:{netPacket.logType} continue send:{m_send_offset}->{netPacket.Buffer.Length}");
				}
			}
			else
			{
				m_send_offset = 0;
				FinishSendPacket(netPacket);
			}
		}
		catch (SocketException ex)
		{
			Error("Connection error: " + ex.Message + " " + ex.StackTrace, ex.SocketErrorCode);
		}
	}

	private void FinishSendPacket(NetPacket p)
	{
		lock (m_packets_send)
		{
			p.onPackageSendFinish();
			m_packets_send.RemoveFirst();
			PostSend();
			if (!string.IsNullOrEmpty(p.logType))
			{
				PostLog(LogLevel.Info, "NetConnection:" + p.logType + " send finish");
			}
		}
	}

	internal void PostLog(LogLevel level, string msg)
	{
		m_service?.Post(delegate
		{
			if (level == LogLevel.Error)
			{
				NetLogError(msg);
			}
			else
			{
				NetLogInfo(msg);
			}
		});
	}

	internal void Error(string err, SocketError errCode)
	{
		Close(graceful: false, "socket error");
		m_service?.Post(delegate
		{
			if (!m_disposed)
			{
				lock (m_packets_send)
				{
					m_packets_send.Clear();
				}
				m_dispatch.OnConnectionLost(err, errCode);
				Dispose();
			}
		});
	}

	public void UpdateSrcServerId(ushort sid)
	{
		m_sid = sid;
	}

	public void NetLogInfo(string msg)
	{
		Log.Info("[Net] [conn] [" + m_name + "]: " + msg);
	}

	public void NetLogError(string msg)
	{
		Log.Error("[Net] [conn] [" + m_name + "]: " + msg);
	}
}
