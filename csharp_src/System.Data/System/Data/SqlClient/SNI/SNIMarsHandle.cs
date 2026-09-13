using System.Collections.Generic;
using System.Threading;

namespace System.Data.SqlClient.SNI;

internal class SNIMarsHandle : SNIHandle
{
	private const uint ACK_THRESHOLD = 2u;

	private readonly SNIMarsConnection _connection;

	private readonly uint _status = uint.MaxValue;

	private readonly Queue<SNIPacket> _receivedPacketQueue = new Queue<SNIPacket>();

	private readonly Queue<SNIMarsQueuedPacket> _sendPacketQueue = new Queue<SNIMarsQueuedPacket>();

	private readonly object _callbackObject;

	private readonly Guid _connectionId = Guid.NewGuid();

	private readonly ushort _sessionId;

	private readonly ManualResetEventSlim _packetEvent = new ManualResetEventSlim(initialState: false);

	private readonly ManualResetEventSlim _ackEvent = new ManualResetEventSlim(initialState: false);

	private readonly SNISMUXHeader _currentHeader = new SNISMUXHeader();

	private uint _sendHighwater = 4u;

	private int _asyncReceives;

	private uint _receiveHighwater = 4u;

	private uint _receiveHighwaterLastAck = 4u;

	private uint _sequenceNumber;

	private SNIError _connectionError;

	public override Guid ConnectionId => _connectionId;

	public override uint Status => _status;

	public override void Dispose()
	{
		try
		{
			SendControlPacket(SNISMUXFlags.SMUX_FIN);
		}
		catch (Exception sniException)
		{
			SNICommon.ReportSNIError(SNIProviders.SMUX_PROV, 35u, sniException);
			throw;
		}
	}

	public SNIMarsHandle(SNIMarsConnection connection, ushort sessionId, object callbackObject, bool async)
	{
		_sessionId = sessionId;
		_connection = connection;
		_callbackObject = callbackObject;
		SendControlPacket(SNISMUXFlags.SMUX_SYN);
		_status = 0u;
	}

	private void SendControlPacket(SNISMUXFlags flags)
	{
		byte[] headerBytes = null;
		lock (this)
		{
			GetSMUXHeaderBytes(0, (byte)flags, ref headerBytes);
		}
		SNIPacket sNIPacket = new SNIPacket();
		sNIPacket.SetData(headerBytes, 16);
		_connection.Send(sNIPacket);
	}

	private void GetSMUXHeaderBytes(int length, byte flags, ref byte[] headerBytes)
	{
		headerBytes = new byte[16];
		_currentHeader.SMID = 83;
		_currentHeader.flags = flags;
		_currentHeader.sessionId = _sessionId;
		_currentHeader.length = (uint)(16 + length);
		_currentHeader.sequenceNumber = ((flags == 4 || flags == 2) ? (_sequenceNumber - 1) : _sequenceNumber++);
		_currentHeader.highwater = _receiveHighwater;
		_receiveHighwaterLastAck = _currentHeader.highwater;
		BitConverter.GetBytes(_currentHeader.SMID).CopyTo(headerBytes, 0);
		BitConverter.GetBytes(_currentHeader.flags).CopyTo(headerBytes, 1);
		BitConverter.GetBytes(_currentHeader.sessionId).CopyTo(headerBytes, 2);
		BitConverter.GetBytes(_currentHeader.length).CopyTo(headerBytes, 4);
		BitConverter.GetBytes(_currentHeader.sequenceNumber).CopyTo(headerBytes, 8);
		BitConverter.GetBytes(_currentHeader.highwater).CopyTo(headerBytes, 12);
	}

	private SNIPacket GetSMUXEncapsulatedPacket(SNIPacket packet)
	{
		uint sequenceNumber = _sequenceNumber;
		byte[] headerBytes = null;
		GetSMUXHeaderBytes(packet.Length, 8, ref headerBytes);
		SNIPacket sNIPacket = new SNIPacket(16 + packet.Length);
		sNIPacket.Description = string.Format("({0}) SMUX packet {1}", (packet.Description == null) ? "" : packet.Description, sequenceNumber);
		sNIPacket.AppendData(headerBytes, 16);
		sNIPacket.AppendPacket(packet);
		return sNIPacket;
	}

	public override uint Send(SNIPacket packet)
	{
		while (true)
		{
			lock (this)
			{
				if (_sequenceNumber < _sendHighwater)
				{
					break;
				}
			}
			_ackEvent.Wait();
			lock (this)
			{
				_ackEvent.Reset();
			}
		}
		return _connection.Send(GetSMUXEncapsulatedPacket(packet));
	}

	private uint InternalSendAsync(SNIPacket packet, SNIAsyncCallback callback)
	{
		SNIPacket sNIPacket = null;
		lock (this)
		{
			if (_sequenceNumber >= _sendHighwater)
			{
				return 1048576u;
			}
			sNIPacket = GetSMUXEncapsulatedPacket(packet);
			if (callback != null)
			{
				sNIPacket.SetCompletionCallback(callback);
			}
			else
			{
				sNIPacket.SetCompletionCallback(HandleSendComplete);
			}
			return _connection.SendAsync(sNIPacket, callback);
		}
	}

	private uint SendPendingPackets()
	{
		SNIMarsQueuedPacket sNIMarsQueuedPacket = null;
		while (true)
		{
			lock (this)
			{
				if (_sequenceNumber >= _sendHighwater)
				{
					break;
				}
				if (_sendPacketQueue.Count != 0)
				{
					sNIMarsQueuedPacket = _sendPacketQueue.Peek();
					uint num = InternalSendAsync(sNIMarsQueuedPacket.Packet, sNIMarsQueuedPacket.Callback);
					if (num != 0 && num != 997)
					{
						return num;
					}
					_sendPacketQueue.Dequeue();
					continue;
				}
				_ackEvent.Set();
			}
			break;
		}
		return 0u;
	}

	public override uint SendAsync(SNIPacket packet, bool disposePacketAfterSendAsync, SNIAsyncCallback callback = null)
	{
		lock (this)
		{
			_sendPacketQueue.Enqueue(new SNIMarsQueuedPacket(packet, (callback != null) ? callback : new SNIAsyncCallback(HandleSendComplete)));
		}
		SendPendingPackets();
		return 997u;
	}

	public override uint ReceiveAsync(ref SNIPacket packet)
	{
		lock (_receivedPacketQueue)
		{
			int count = _receivedPacketQueue.Count;
			if (_connectionError != null)
			{
				return SNICommon.ReportSNIError(_connectionError);
			}
			if (count == 0)
			{
				_asyncReceives++;
				return 997u;
			}
			packet = _receivedPacketQueue.Dequeue();
			if (count == 1)
			{
				_packetEvent.Reset();
			}
		}
		lock (this)
		{
			_receiveHighwater++;
		}
		SendAckIfNecessary();
		return 0u;
	}

	public void HandleReceiveError(SNIPacket packet)
	{
		lock (_receivedPacketQueue)
		{
			_connectionError = SNILoadHandle.SingletonInstance.LastError;
			_packetEvent.Set();
		}
		((TdsParserStateObject)_callbackObject).ReadAsyncCallback(packet, 1u);
	}

	public void HandleSendComplete(SNIPacket packet, uint sniErrorCode)
	{
		lock (this)
		{
			((TdsParserStateObject)_callbackObject).WriteAsyncCallback(packet, sniErrorCode);
		}
	}

	public void HandleAck(uint highwater)
	{
		lock (this)
		{
			if (_sendHighwater != highwater)
			{
				_sendHighwater = highwater;
				SendPendingPackets();
			}
		}
	}

	public void HandleReceiveComplete(SNIPacket packet, SNISMUXHeader header)
	{
		lock (this)
		{
			if (_sendHighwater != header.highwater)
			{
				HandleAck(header.highwater);
			}
			lock (_receivedPacketQueue)
			{
				if (_asyncReceives == 0)
				{
					_receivedPacketQueue.Enqueue(packet);
					_packetEvent.Set();
					return;
				}
				_asyncReceives--;
				((TdsParserStateObject)_callbackObject).ReadAsyncCallback(packet, 0u);
			}
		}
		lock (this)
		{
			_receiveHighwater++;
		}
		SendAckIfNecessary();
	}

	private void SendAckIfNecessary()
	{
		uint receiveHighwater;
		uint receiveHighwaterLastAck;
		lock (this)
		{
			receiveHighwater = _receiveHighwater;
			receiveHighwaterLastAck = _receiveHighwaterLastAck;
		}
		if (receiveHighwater - receiveHighwaterLastAck > 2)
		{
			SendControlPacket(SNISMUXFlags.SMUX_ACK);
		}
	}

	public override uint Receive(out SNIPacket packet, int timeoutInMilliseconds)
	{
		packet = null;
		uint num = 997u;
		do
		{
			lock (_receivedPacketQueue)
			{
				if (_connectionError != null)
				{
					return SNICommon.ReportSNIError(_connectionError);
				}
				int count = _receivedPacketQueue.Count;
				if (count > 0)
				{
					packet = _receivedPacketQueue.Dequeue();
					if (count == 1)
					{
						_packetEvent.Reset();
					}
					num = 0u;
				}
			}
			if (num == 0)
			{
				lock (this)
				{
					_receiveHighwater++;
				}
				SendAckIfNecessary();
				return num;
			}
		}
		while (_packetEvent.Wait(timeoutInMilliseconds));
		SNILoadHandle.SingletonInstance.LastError = new SNIError(SNIProviders.SMUX_PROV, 0u, 11u, string.Empty);
		return 258u;
	}

	public override uint CheckConnection()
	{
		return _connection.CheckConnection();
	}

	public override void SetAsyncCallbacks(SNIAsyncCallback receiveCallback, SNIAsyncCallback sendCallback)
	{
	}

	public override void SetBufferSize(int bufferSize)
	{
	}

	public override uint EnableSsl(uint options)
	{
		return _connection.EnableSsl(options);
	}

	public override void DisableSsl()
	{
		_connection.DisableSsl();
	}
}
