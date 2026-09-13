using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace System.Data.SqlClient.SNI;

internal class TdsParserStateObjectManaged : TdsParserStateObject
{
	internal sealed class WritePacketCache : IDisposable
	{
		private bool _disposed;

		private Stack<SNIPacket> _packets;

		public WritePacketCache()
		{
			_disposed = false;
			_packets = new Stack<SNIPacket>();
		}

		public SNIPacket Take(SNIHandle sniHandle)
		{
			SNIPacket sNIPacket;
			if (_packets.Count > 0)
			{
				sNIPacket = _packets.Pop();
				sNIPacket.Reset();
			}
			else
			{
				sNIPacket = new SNIPacket();
			}
			return sNIPacket;
		}

		public void Add(SNIPacket packet)
		{
			if (!_disposed)
			{
				_packets.Push(packet);
			}
			else
			{
				packet.Dispose();
			}
		}

		public void Clear()
		{
			while (_packets.Count > 0)
			{
				_packets.Pop().Dispose();
			}
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				Clear();
			}
		}
	}

	private SNIMarsConnection _marsConnection;

	private SNIHandle _sessionHandle;

	private SNIPacket _sniPacket;

	internal SNIPacket _sniAsyncAttnPacket;

	private readonly Dictionary<SNIPacket, SNIPacket> _pendingWritePackets = new Dictionary<SNIPacket, SNIPacket>();

	private readonly WritePacketCache _writePacketCache = new WritePacketCache();

	internal SspiClientContextStatus sspiClientContextStatus = new SspiClientContextStatus();

	internal SNIHandle Handle => _sessionHandle;

	internal override uint Status
	{
		get
		{
			if (_sessionHandle == null)
			{
				return uint.MaxValue;
			}
			return _sessionHandle.Status;
		}
	}

	internal override object SessionHandle => _sessionHandle;

	protected override object EmptyReadPacket => null;

	public TdsParserStateObjectManaged(TdsParser parser)
		: base(parser)
	{
	}

	internal TdsParserStateObjectManaged(TdsParser parser, TdsParserStateObject physicalConnection, bool async)
		: base(parser, physicalConnection, async)
	{
	}

	protected override bool CheckPacket(object packet, TaskCompletionSource<object> source)
	{
		SNIPacket sNIPacket = packet as SNIPacket;
		if (!sNIPacket.IsInvalid)
		{
			if (!sNIPacket.IsInvalid)
			{
				return source != null;
			}
			return false;
		}
		return true;
	}

	protected override void CreateSessionHandle(TdsParserStateObject physicalConnection, bool async)
	{
		TdsParserStateObjectManaged tdsParserStateObjectManaged = physicalConnection as TdsParserStateObjectManaged;
		_sessionHandle = tdsParserStateObjectManaged.CreateMarsSession(this, async);
	}

	internal SNIMarsHandle CreateMarsSession(object callbackObject, bool async)
	{
		return _marsConnection.CreateMarsSession(callbackObject, async);
	}

	protected override uint SNIPacketGetData(object packet, byte[] _inBuff, ref uint dataSize)
	{
		return SNIProxy.Singleton.PacketGetData(packet as SNIPacket, _inBuff, ref dataSize);
	}

	internal override void CreatePhysicalSNIHandle(string serverName, bool ignoreSniOpenTimeout, long timerExpire, out byte[] instanceName, ref byte[] spnBuffer, bool flushCache, bool async, bool parallel, bool isIntegratedSecurity)
	{
		_sessionHandle = SNIProxy.Singleton.CreateConnectionHandle(this, serverName, ignoreSniOpenTimeout, timerExpire, out instanceName, ref spnBuffer, flushCache, async, parallel, isIntegratedSecurity);
		if (_sessionHandle == null)
		{
			_parser.ProcessSNIError(this);
		}
		else if (async)
		{
			SNIAsyncCallback receiveCallback = ReadAsyncCallback;
			SNIAsyncCallback sendCallback = WriteAsyncCallback;
			_sessionHandle.SetAsyncCallbacks(receiveCallback, sendCallback);
		}
	}

	internal void ReadAsyncCallback(SNIPacket packet, uint error)
	{
		ReadAsyncCallback(IntPtr.Zero, packet, error);
	}

	internal void WriteAsyncCallback(SNIPacket packet, uint sniError)
	{
		WriteAsyncCallback(IntPtr.Zero, packet, sniError);
	}

	protected override void RemovePacketFromPendingList(object packet)
	{
	}

	internal override void Dispose()
	{
		SNIPacket sniPacket = _sniPacket;
		SNIHandle sessionHandle = _sessionHandle;
		SNIPacket sniAsyncAttnPacket = _sniAsyncAttnPacket;
		_sniPacket = null;
		_sessionHandle = null;
		_sniAsyncAttnPacket = null;
		_marsConnection = null;
		DisposeCounters();
		if (sessionHandle != null || sniPacket != null)
		{
			sniPacket?.Dispose();
			sniAsyncAttnPacket?.Dispose();
			if (sessionHandle != null)
			{
				sessionHandle.Dispose();
				DecrementPendingCallbacks(release: true);
			}
		}
		DisposePacketCache();
	}

	internal override void DisposePacketCache()
	{
		lock (_writePacketLockObject)
		{
			_writePacketCache.Dispose();
		}
	}

	protected override void FreeGcHandle(int remaining, bool release)
	{
	}

	internal override bool IsFailedHandle()
	{
		return _sessionHandle.Status != 0;
	}

	internal override object ReadSyncOverAsync(int timeoutRemaining, out uint error)
	{
		SNIHandle handle = Handle;
		if (handle == null)
		{
			throw ADP.ClosedConnectionError();
		}
		SNIPacket packet = null;
		error = SNIProxy.Singleton.ReadSyncOverAsync(handle, out packet, timeoutRemaining);
		return packet;
	}

	internal override bool IsPacketEmpty(object packet)
	{
		return packet == null;
	}

	internal override void ReleasePacket(object syncReadPacket)
	{
		((SNIPacket)syncReadPacket).Dispose();
	}

	internal override uint CheckConnection()
	{
		SNIHandle handle = Handle;
		if (handle != null)
		{
			return SNIProxy.Singleton.CheckConnection(handle);
		}
		return 0u;
	}

	internal override object ReadAsync(out uint error, ref object handle)
	{
		error = SNIProxy.Singleton.ReadAsync((SNIHandle)handle, out var packet);
		return packet;
	}

	internal override object CreateAndSetAttentionPacket()
	{
		if (_sniAsyncAttnPacket == null)
		{
			SNIPacket sNIPacket = new SNIPacket();
			SetPacketData(sNIPacket, SQL.AttentionHeader, 8);
			_sniAsyncAttnPacket = sNIPacket;
		}
		return _sniAsyncAttnPacket;
	}

	internal override uint WritePacket(object packet, bool sync)
	{
		return SNIProxy.Singleton.WritePacket(Handle, (SNIPacket)packet, sync);
	}

	internal override object AddPacketToPendingList(object packet)
	{
		return packet;
	}

	internal override bool IsValidPacket(object packetPointer)
	{
		if ((SNIPacket)packetPointer != null)
		{
			return !((SNIPacket)packetPointer).IsInvalid;
		}
		return false;
	}

	internal override object GetResetWritePacket()
	{
		if (_sniPacket != null)
		{
			_sniPacket.Reset();
		}
		else
		{
			lock (_writePacketLockObject)
			{
				_sniPacket = _writePacketCache.Take(Handle);
			}
		}
		return _sniPacket;
	}

	internal override void ClearAllWritePackets()
	{
		if (_sniPacket != null)
		{
			_sniPacket.Dispose();
			_sniPacket = null;
		}
		lock (_writePacketLockObject)
		{
			_writePacketCache.Clear();
		}
	}

	internal override void SetPacketData(object packet, byte[] buffer, int bytesUsed)
	{
		SNIProxy.Singleton.PacketSetData((SNIPacket)packet, buffer, bytesUsed);
	}

	internal override uint SniGetConnectionId(ref Guid clientConnectionId)
	{
		return SNIProxy.Singleton.GetConnectionId(Handle, ref clientConnectionId);
	}

	internal override uint DisabeSsl()
	{
		return SNIProxy.Singleton.DisableSsl(Handle);
	}

	internal override uint EnableMars(ref uint info)
	{
		_marsConnection = new SNIMarsConnection(Handle);
		if (_marsConnection.StartReceive() == 997)
		{
			return 0u;
		}
		return 1u;
	}

	internal override uint EnableSsl(ref uint info)
	{
		return SNIProxy.Singleton.EnableSsl(Handle, info);
	}

	internal override uint SetConnectionBufferSize(ref uint unsignedPacketSize)
	{
		return SNIProxy.Singleton.SetConnectionBufferSize(Handle, unsignedPacketSize);
	}

	internal override uint GenerateSspiClientContext(byte[] receivedBuff, uint receivedLength, ref byte[] sendBuff, ref uint sendLength, byte[] _sniSpnBuffer)
	{
		SNIProxy.Singleton.GenSspiClientContext(sspiClientContextStatus, receivedBuff, ref sendBuff, _sniSpnBuffer);
		sendLength = ((sendBuff != null) ? ((uint)sendBuff.Length) : 0u);
		return 0u;
	}

	internal override uint WaitForSSLHandShakeToComplete()
	{
		return 0u;
	}
}
