using System.Collections.Generic;

namespace System.Data.SqlClient.SNI;

internal class SNIMarsConnection
{
	private readonly Guid _connectionId = Guid.NewGuid();

	private readonly Dictionary<int, SNIMarsHandle> _sessions = new Dictionary<int, SNIMarsHandle>();

	private readonly byte[] _headerBytes = new byte[16];

	private SNIHandle _lowerHandle;

	private ushort _nextSessionId;

	private int _currentHeaderByteCount;

	private int _dataBytesLeft;

	private SNISMUXHeader _currentHeader;

	private SNIPacket _currentPacket;

	public Guid ConnectionId => _connectionId;

	public SNIMarsConnection(SNIHandle lowerHandle)
	{
		_lowerHandle = lowerHandle;
		_lowerHandle.SetAsyncCallbacks(HandleReceiveComplete, HandleSendComplete);
	}

	public SNIMarsHandle CreateMarsSession(object callbackObject, bool async)
	{
		lock (this)
		{
			ushort num = _nextSessionId++;
			SNIMarsHandle sNIMarsHandle = new SNIMarsHandle(this, num, callbackObject, async);
			_sessions.Add(num, sNIMarsHandle);
			return sNIMarsHandle;
		}
	}

	public uint StartReceive()
	{
		SNIPacket packet = null;
		if (ReceiveAsync(ref packet) == 997)
		{
			return 997u;
		}
		return SNICommon.ReportSNIError(SNIProviders.SMUX_PROV, 0u, 19u, string.Empty);
	}

	public uint Send(SNIPacket packet)
	{
		lock (this)
		{
			return _lowerHandle.Send(packet);
		}
	}

	public uint SendAsync(SNIPacket packet, SNIAsyncCallback callback)
	{
		lock (this)
		{
			return _lowerHandle.SendAsync(packet, disposePacketAfterSendAsync: false, callback);
		}
	}

	public uint ReceiveAsync(ref SNIPacket packet)
	{
		lock (this)
		{
			return _lowerHandle.ReceiveAsync(ref packet);
		}
	}

	public uint CheckConnection()
	{
		lock (this)
		{
			return _lowerHandle.CheckConnection();
		}
	}

	public void HandleReceiveError(SNIPacket packet)
	{
		foreach (SNIMarsHandle value in _sessions.Values)
		{
			value.HandleReceiveError(packet);
		}
	}

	public void HandleSendComplete(SNIPacket packet, uint sniErrorCode)
	{
		packet.InvokeCompletionCallback(sniErrorCode);
	}

	public void HandleReceiveComplete(SNIPacket packet, uint sniErrorCode)
	{
		SNISMUXHeader sNISMUXHeader = null;
		SNIPacket packet2 = null;
		SNIMarsHandle sNIMarsHandle = null;
		if (sniErrorCode != 0)
		{
			lock (this)
			{
				HandleReceiveError(packet);
				return;
			}
		}
		while (true)
		{
			lock (this)
			{
				if (_currentHeaderByteCount != 16)
				{
					sNISMUXHeader = null;
					packet2 = null;
					sNIMarsHandle = null;
					while (_currentHeaderByteCount != 16)
					{
						int num = packet.TakeData(_headerBytes, _currentHeaderByteCount, 16 - _currentHeaderByteCount);
						_currentHeaderByteCount += num;
						if (num == 0)
						{
							sniErrorCode = ReceiveAsync(ref packet);
							if (sniErrorCode != 997)
							{
								HandleReceiveError(packet);
							}
							return;
						}
					}
					_currentHeader = new SNISMUXHeader
					{
						SMID = _headerBytes[0],
						flags = _headerBytes[1],
						sessionId = BitConverter.ToUInt16(_headerBytes, 2),
						length = BitConverter.ToUInt32(_headerBytes, 4) - 16,
						sequenceNumber = BitConverter.ToUInt32(_headerBytes, 8),
						highwater = BitConverter.ToUInt32(_headerBytes, 12)
					};
					_dataBytesLeft = (int)_currentHeader.length;
					_currentPacket = new SNIPacket((int)_currentHeader.length);
				}
				sNISMUXHeader = _currentHeader;
				packet2 = _currentPacket;
				if (_currentHeader.flags == 8 && _dataBytesLeft > 0)
				{
					int num2 = packet.TakeData(_currentPacket, _dataBytesLeft);
					_dataBytesLeft -= num2;
					if (_dataBytesLeft > 0)
					{
						sniErrorCode = ReceiveAsync(ref packet);
						if (sniErrorCode != 997)
						{
							HandleReceiveError(packet);
						}
						break;
					}
				}
				_currentHeaderByteCount = 0;
				if (!_sessions.ContainsKey(_currentHeader.sessionId))
				{
					SNILoadHandle.SingletonInstance.LastError = new SNIError(SNIProviders.SMUX_PROV, 0u, 5u, string.Empty);
					HandleReceiveError(packet);
					_lowerHandle.Dispose();
					_lowerHandle = null;
					break;
				}
				if (_currentHeader.flags == 4)
				{
					_sessions.Remove(_currentHeader.sessionId);
				}
				else
				{
					sNIMarsHandle = _sessions[_currentHeader.sessionId];
				}
			}
			if (sNISMUXHeader.flags == 8)
			{
				sNIMarsHandle.HandleReceiveComplete(packet2, sNISMUXHeader);
			}
			if (_currentHeader.flags == 2)
			{
				try
				{
					sNIMarsHandle.HandleAck(sNISMUXHeader.highwater);
				}
				catch (Exception sniException)
				{
					SNICommon.ReportSNIError(SNIProviders.SMUX_PROV, 35u, sniException);
				}
			}
			lock (this)
			{
				if (packet.DataLeft == 0)
				{
					sniErrorCode = ReceiveAsync(ref packet);
					if (sniErrorCode != 997)
					{
						HandleReceiveError(packet);
					}
					break;
				}
			}
		}
	}

	public uint EnableSsl(uint options)
	{
		return _lowerHandle.EnableSsl(options);
	}

	public void DisableSsl()
	{
		_lowerHandle.DisableSsl();
	}
}
