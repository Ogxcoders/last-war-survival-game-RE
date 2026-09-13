using System.Collections.Generic;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient.SNI;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient;

internal sealed class TdsParser
{
	private class TdsOrderUnique
	{
		internal short ColumnOrdinal;

		internal byte Flags;

		internal TdsOrderUnique(short ordinal, byte flags)
		{
			ColumnOrdinal = ordinal;
			Flags = flags;
		}
	}

	private class TdsOutputStream : Stream
	{
		private TdsParser _parser;

		private TdsParserStateObject _stateObj;

		private byte[] _preambleToStrip;

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public TdsOutputStream(TdsParser parser, TdsParserStateObject stateObj, byte[] preambleToStrip)
		{
			_parser = parser;
			_stateObj = stateObj;
			_preambleToStrip = preambleToStrip;
		}

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		private void StripPreamble(byte[] buffer, ref int offset, ref int count)
		{
			if (_preambleToStrip != null && count >= _preambleToStrip.Length)
			{
				for (int i = 0; i < _preambleToStrip.Length; i++)
				{
					if (_preambleToStrip[i] != buffer[i])
					{
						_preambleToStrip = null;
						return;
					}
				}
				offset += _preambleToStrip.Length;
				count -= _preambleToStrip.Length;
			}
			_preambleToStrip = null;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			ValidateWriteParameters(buffer, offset, count);
			StripPreamble(buffer, ref offset, ref count);
			if (count > 0)
			{
				_parser.WriteInt(count, _stateObj);
				_stateObj.WriteByteArray(buffer, count, offset);
			}
		}

		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			ValidateWriteParameters(buffer, offset, count);
			StripPreamble(buffer, ref offset, ref count);
			Task task = null;
			if (count > 0)
			{
				_parser.WriteInt(count, _stateObj);
				task = _stateObj.WriteByteArray(buffer, count, offset, canAccumulate: false);
			}
			return task ?? Task.CompletedTask;
		}

		internal static void ValidateWriteParameters(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw ADP.ArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw ADP.ArgumentOutOfRange("offset");
			}
			if (count < 0)
			{
				throw ADP.ArgumentOutOfRange("count");
			}
			try
			{
				if (checked(offset + count) > buffer.Length)
				{
					throw ExceptionBuilder.InvalidOffsetLength();
				}
			}
			catch (OverflowException)
			{
				throw ExceptionBuilder.InvalidOffsetLength();
			}
		}
	}

	private class ConstrainedTextWriter : TextWriter
	{
		private TextWriter _next;

		private int _size;

		private int _written;

		public bool IsComplete
		{
			get
			{
				if (_size > 0)
				{
					return _written >= _size;
				}
				return false;
			}
		}

		public override Encoding Encoding => _next.Encoding;

		public ConstrainedTextWriter(TextWriter next, int size)
		{
			_next = next;
			_size = size;
			_written = 0;
			if (_size < 1)
			{
				_size = int.MaxValue;
			}
		}

		public override void Flush()
		{
			_next.Flush();
		}

		public override Task FlushAsync()
		{
			return _next.FlushAsync();
		}

		public override void Write(char value)
		{
			if (_written < _size)
			{
				_next.Write(value);
				_written++;
			}
		}

		public override void Write(char[] buffer, int index, int count)
		{
			ValidateWriteParameters(buffer, index, count);
			count = Math.Min(_size - _written, count);
			if (count > 0)
			{
				_next.Write(buffer, index, count);
			}
			_written += count;
		}

		public override Task WriteAsync(char value)
		{
			if (_written < _size)
			{
				_written++;
				return _next.WriteAsync(value);
			}
			return Task.CompletedTask;
		}

		public override Task WriteAsync(char[] buffer, int index, int count)
		{
			ValidateWriteParameters(buffer, index, count);
			count = Math.Min(_size - _written, count);
			if (count > 0)
			{
				_written += count;
				return _next.WriteAsync(buffer, index, count);
			}
			return Task.CompletedTask;
		}

		public override Task WriteAsync(string value)
		{
			return WriteAsync(value.ToCharArray());
		}

		internal static void ValidateWriteParameters(char[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw ADP.ArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw ADP.ArgumentOutOfRange("offset");
			}
			if (count < 0)
			{
				throw ADP.ArgumentOutOfRange("count");
			}
			try
			{
				if (checked(offset + count) > buffer.Length)
				{
					throw ExceptionBuilder.InvalidOffsetLength();
				}
			}
			catch (OverflowException)
			{
				throw ExceptionBuilder.InvalidOffsetLength();
			}
		}
	}

	internal TdsParserStateObject _physicalStateObj;

	internal TdsParserStateObject _pMarsPhysicalConObj;

	private const int constBinBufferSize = 4096;

	private const int constTextBufferSize = 4096;

	internal TdsParserState _state;

	private string _server = "";

	internal volatile bool _fResetConnection;

	internal volatile bool _fPreserveTransaction;

	private SqlCollation _defaultCollation;

	private int _defaultCodePage;

	private int _defaultLCID;

	internal Encoding _defaultEncoding;

	private static EncryptionOptions s_sniSupportedEncryptionOption = TdsParserStateObjectFactory.Singleton.EncryptionOptions;

	private EncryptionOptions _encryptionOption = s_sniSupportedEncryptionOption;

	private SqlInternalTransaction _currentTransaction;

	private SqlInternalTransaction _pendingTransaction;

	private long _retainedTransactionId;

	private int _nonTransactedOpenResultCount;

	private SqlInternalConnectionTds _connHandler;

	private bool _fMARS;

	internal bool _loginWithFailover;

	internal AutoResetEvent _resetConnectionEvent;

	internal TdsParserSessionPool _sessionPool;

	private bool _isYukon;

	private bool _isKatmai;

	private bool _isDenali;

	private byte[] _sniSpnBuffer;

	private SqlStatistics _statistics;

	private bool _statisticsIsInTransaction;

	private static byte[] s_nicAddress;

	private static volatile uint s_maxSSPILength = 0u;

	private static readonly byte[] s_longDataHeader = new byte[25]
	{
		16, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255
	};

	private static object s_tdsParserLock = new object();

	private static readonly byte[] s_xmlMetadataSubstituteSequence = new byte[8] { 231, 255, 255, 0, 0, 0, 0, 0 };

	private const int GUID_SIZE = 16;

	internal bool _asyncWrite;

	private static readonly IEnumerable<SqlDataRecord> s_tvpEmptyValue = new SqlDataRecord[0];

	private const ulong _indeterminateSize = ulong.MaxValue;

	internal SqlInternalConnectionTds Connection => _connHandler;

	internal SqlInternalTransaction CurrentTransaction
	{
		get
		{
			return _currentTransaction;
		}
		set
		{
			if ((_currentTransaction == null && value != null) || (_currentTransaction != null && value == null))
			{
				_currentTransaction = value;
			}
		}
	}

	internal int DefaultLCID => _defaultLCID;

	internal EncryptionOptions EncryptionOptions
	{
		get
		{
			return _encryptionOption;
		}
		set
		{
			_encryptionOption = value;
		}
	}

	internal bool IsKatmaiOrNewer => _isKatmai;

	internal bool MARSOn => _fMARS;

	internal SqlInternalTransaction PendingTransaction
	{
		get
		{
			return _pendingTransaction;
		}
		set
		{
			_pendingTransaction = value;
		}
	}

	internal string Server => _server;

	internal TdsParserState State
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
		}
	}

	internal SqlStatistics Statistics
	{
		get
		{
			return _statistics;
		}
		set
		{
			_statistics = value;
		}
	}

	internal void PostReadAsyncForMars()
	{
	}

	private void LoadSSPILibrary()
	{
	}

	private void WaitForSSLHandShakeToComplete(ref uint error)
	{
	}

	private SNIErrorDetails GetSniErrorDetails()
	{
		SNIError lastError = SNIProxy.Singleton.GetLastError();
		SNIErrorDetails result = default(SNIErrorDetails);
		result.sniErrorNumber = lastError.sniError;
		result.errorMessage = lastError.errorMessage;
		result.nativeError = lastError.nativeError;
		result.provider = (int)lastError.provider;
		result.lineNumber = lastError.lineNumber;
		result.function = lastError.function;
		result.exception = lastError.exception;
		return result;
	}

	internal TdsParser(bool MARS, bool fAsynchronous)
	{
		_fMARS = MARS;
		_physicalStateObj = TdsParserStateObjectFactory.Singleton.CreateTdsParserStateObject(this);
	}

	internal int IncrementNonTransactedOpenResultCount()
	{
		return Interlocked.Increment(ref _nonTransactedOpenResultCount);
	}

	internal void DecrementNonTransactedOpenResultCount()
	{
		Interlocked.Decrement(ref _nonTransactedOpenResultCount);
	}

	internal void ProcessPendingAck(TdsParserStateObject stateObj)
	{
		if (stateObj._attentionSent)
		{
			ProcessAttention(stateObj);
		}
	}

	internal void Connect(ServerInfo serverInfo, SqlInternalConnectionTds connHandler, bool ignoreSniOpenTimeout, long timerExpire, bool encrypt, bool trustServerCert, bool integratedSecurity, bool withFailover)
	{
		if (_state != TdsParserState.Closed)
		{
			return;
		}
		_connHandler = connHandler;
		_loginWithFailover = withFailover;
		if (TdsParserStateObjectFactory.Singleton.SNIStatus != 0)
		{
			_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
			_physicalStateObj.Dispose();
			ThrowExceptionAndWarning(_physicalStateObj);
		}
		_sniSpnBuffer = null;
		if (integratedSecurity)
		{
			LoadSSPILibrary();
		}
		byte[] instanceName = null;
		_connHandler.TimeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.PreLoginBegin);
		_connHandler.TimeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.InitializeConnection);
		bool multiSubnetFailover = _connHandler.ConnectionOptions.MultiSubnetFailover;
		_physicalStateObj.CreatePhysicalSNIHandle(serverInfo.ExtendedServerName, ignoreSniOpenTimeout, timerExpire, out instanceName, ref _sniSpnBuffer, flushCache: false, async: true, multiSubnetFailover, integratedSecurity);
		if (_physicalStateObj.Status != 0)
		{
			_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
			_physicalStateObj.Dispose();
			ThrowExceptionAndWarning(_physicalStateObj);
		}
		_server = serverInfo.ResolvedServerName;
		if (connHandler.PoolGroupProviderInfo != null)
		{
			connHandler.PoolGroupProviderInfo.AliasCheck((serverInfo.PreRoutingServerName == null) ? serverInfo.ResolvedServerName : serverInfo.PreRoutingServerName);
		}
		_state = TdsParserState.OpenNotLoggedIn;
		_physicalStateObj.SniContext = SniContext.Snix_PreLoginBeforeSuccessfulWrite;
		_physicalStateObj.TimeoutTime = timerExpire;
		bool marsCapable = false;
		_connHandler.TimeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.InitializeConnection);
		_connHandler.TimeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.SendPreLoginHandshake);
		_physicalStateObj.SniGetConnectionId(ref _connHandler._clientConnectionId);
		SendPreLoginHandshake(instanceName, encrypt);
		_connHandler.TimeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.SendPreLoginHandshake);
		_connHandler.TimeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.ConsumePreLoginHandshake);
		_physicalStateObj.SniContext = SniContext.Snix_PreLogin;
		if (ConsumePreLoginHandshake(encrypt, trustServerCert, integratedSecurity, out marsCapable, out _connHandler._fedAuthRequired) == PreLoginHandshakeStatus.InstanceFailure)
		{
			_physicalStateObj.Dispose();
			_physicalStateObj.SniContext = SniContext.Snix_Connect;
			_physicalStateObj.CreatePhysicalSNIHandle(serverInfo.ExtendedServerName, ignoreSniOpenTimeout, timerExpire, out instanceName, ref _sniSpnBuffer, flushCache: true, async: true, multiSubnetFailover, integratedSecurity);
			if (_physicalStateObj.Status != 0)
			{
				_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
				ThrowExceptionAndWarning(_physicalStateObj);
			}
			_physicalStateObj.SniGetConnectionId(ref _connHandler._clientConnectionId);
			SendPreLoginHandshake(instanceName, encrypt);
			if (ConsumePreLoginHandshake(encrypt, trustServerCert, integratedSecurity, out marsCapable, out _connHandler._fedAuthRequired) == PreLoginHandshakeStatus.InstanceFailure)
			{
				throw SQL.InstanceFailure();
			}
		}
		if (_fMARS && marsCapable)
		{
			_sessionPool = new TdsParserSessionPool(this);
		}
		else
		{
			_fMARS = false;
		}
	}

	internal void RemoveEncryption()
	{
		if (_physicalStateObj.DisabeSsl() != 0)
		{
			_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
			ThrowExceptionAndWarning(_physicalStateObj);
		}
		_physicalStateObj.ClearAllWritePackets();
	}

	internal void EnableMars()
	{
		if (_fMARS)
		{
			_pMarsPhysicalConObj = _physicalStateObj;
			if (TdsParserStateObjectFactory.UseManagedSNI)
			{
				_pMarsPhysicalConObj.IncrementPendingCallbacks();
			}
			uint info = 0u;
			if (_pMarsPhysicalConObj.EnableMars(ref info) != 0)
			{
				_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
				ThrowExceptionAndWarning(_physicalStateObj);
			}
			PostReadAsyncForMars();
			_physicalStateObj = CreateSession();
		}
	}

	internal TdsParserStateObject CreateSession()
	{
		return TdsParserStateObjectFactory.Singleton.CreateSessionObject(this, _pMarsPhysicalConObj, v: true);
	}

	internal TdsParserStateObject GetSession(object owner)
	{
		TdsParserStateObject tdsParserStateObject = null;
		if (MARSOn)
		{
			return _sessionPool.GetSession(owner);
		}
		return _physicalStateObj;
	}

	internal void PutSession(TdsParserStateObject session)
	{
		if (MARSOn)
		{
			_sessionPool.PutSession(session);
		}
		else if (_state == TdsParserState.Closed || _state == TdsParserState.Broken)
		{
			_physicalStateObj.SniContext = SniContext.Snix_Close;
			_physicalStateObj.Dispose();
		}
		else
		{
			_physicalStateObj.Owner = null;
		}
	}

	private void SendPreLoginHandshake(byte[] instanceName, bool encrypt)
	{
		_physicalStateObj._outputMessageType = 18;
		int num = 36;
		byte[] array = new byte[1059];
		int num2 = 0;
		for (int i = 0; i < 7; i++)
		{
			int num3 = 0;
			_physicalStateObj.WriteByte((byte)i);
			_physicalStateObj.WriteByte((byte)((num & 0xFF00) >> 8));
			_physicalStateObj.WriteByte((byte)(num & 0xFF));
			switch (i)
			{
			case 0:
			{
				Version assemblyVersion = ADP.GetAssemblyVersion();
				array[num2++] = (byte)(assemblyVersion.Major & 0xFF);
				array[num2++] = (byte)(assemblyVersion.Minor & 0xFF);
				array[num2++] = (byte)((assemblyVersion.Build & 0xFF00) >> 8);
				array[num2++] = (byte)(assemblyVersion.Build & 0xFF);
				array[num2++] = (byte)(assemblyVersion.Revision & 0xFF);
				array[num2++] = (byte)((assemblyVersion.Revision & 0xFF00) >> 8);
				num += 6;
				num3 = 6;
				break;
			}
			case 1:
				if (_encryptionOption == EncryptionOptions.NOT_SUP)
				{
					array[num2] = 2;
				}
				else if (encrypt)
				{
					array[num2] = 1;
					_encryptionOption = EncryptionOptions.ON;
				}
				else
				{
					array[num2] = 0;
					_encryptionOption = EncryptionOptions.OFF;
				}
				num2++;
				num++;
				num3 = 1;
				break;
			case 2:
			{
				int j;
				for (j = 0; instanceName[j] != 0; j++)
				{
					array[num2] = instanceName[j];
					num2++;
				}
				array[num2] = 0;
				num2++;
				j++;
				num += j;
				num3 = j;
				break;
			}
			case 3:
			{
				int currentThreadIdForTdsLoginOnly = TdsParserStaticMethods.GetCurrentThreadIdForTdsLoginOnly();
				array[num2++] = (byte)((0xFF000000u & currentThreadIdForTdsLoginOnly) >> 24);
				array[num2++] = (byte)((0xFF0000 & currentThreadIdForTdsLoginOnly) >> 16);
				array[num2++] = (byte)((0xFF00 & currentThreadIdForTdsLoginOnly) >> 8);
				array[num2++] = (byte)(0xFF & currentThreadIdForTdsLoginOnly);
				num += 4;
				num3 = 4;
				break;
			}
			case 4:
				array[num2++] = (byte)(_fMARS ? 1u : 0u);
				num++;
				num3++;
				break;
			case 5:
			{
				Buffer.BlockCopy(_connHandler._clientConnectionId.ToByteArray(), 0, array, num2, 16);
				num2 += 16;
				num += 16;
				num3 = 16;
				ActivityCorrelator.ActivityId activityId = ActivityCorrelator.Next();
				Buffer.BlockCopy(activityId.Id.ToByteArray(), 0, array, num2, 16);
				num2 += 16;
				array[num2++] = (byte)(0xFF & activityId.Sequence);
				array[num2++] = (byte)((0xFF00 & activityId.Sequence) >> 8);
				array[num2++] = (byte)((0xFF0000 & activityId.Sequence) >> 16);
				array[num2++] = (byte)((0xFF000000u & activityId.Sequence) >> 24);
				int num4 = 20;
				num += num4;
				num3 += num4;
				break;
			}
			case 6:
				array[num2++] = 1;
				num++;
				num3++;
				break;
			}
			_physicalStateObj.WriteByte((byte)((num3 & 0xFF00) >> 8));
			_physicalStateObj.WriteByte((byte)(num3 & 0xFF));
		}
		_physicalStateObj.WriteByte(byte.MaxValue);
		_physicalStateObj.WriteByteArray(array, num2, 0);
		_physicalStateObj.WritePacket(1);
	}

	private PreLoginHandshakeStatus ConsumePreLoginHandshake(bool encrypt, bool trustServerCert, bool integratedSecurity, out bool marsCapable, out bool fedAuthRequired)
	{
		marsCapable = _fMARS;
		fedAuthRequired = false;
		bool flag = false;
		if (!_physicalStateObj.TryReadNetworkPacket())
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		if (_physicalStateObj._inBytesRead == 0)
		{
			_physicalStateObj.AddError(new SqlError(0, 0, 20, _server, SQLMessage.PreloginError(), "", 0));
			_physicalStateObj.Dispose();
			ThrowExceptionAndWarning(_physicalStateObj);
		}
		if (!_physicalStateObj.TryProcessHeader())
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		if (_physicalStateObj._inBytesPacket > 32768 || _physicalStateObj._inBytesPacket <= 0)
		{
			throw SQL.ParsingError();
		}
		byte[] array = new byte[_physicalStateObj._inBytesPacket];
		if (!_physicalStateObj.TryReadByteArray(array, 0, array.Length))
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		if (array[0] == 170)
		{
			throw SQL.InvalidSQLServerVersionUnknown();
		}
		int num = 0;
		int num2 = 0;
		int num3 = array[num++];
		while (true)
		{
			switch (num3)
			{
			case 0:
			{
				num2 = (array[num++] << 8) | array[num++];
				_ = array[num++];
				_ = array[num++];
				byte num4 = array[num2];
				_ = array[num2 + 1];
				_ = array[num2 + 2];
				_ = array[num2 + 3];
				flag = num4 >= 9;
				if (!flag)
				{
					marsCapable = false;
				}
				goto IL_03d0;
			}
			case 1:
			{
				num2 = (array[num++] << 8) | array[num++];
				_ = array[num++];
				_ = array[num++];
				EncryptionOptions encryptionOptions = (EncryptionOptions)array[num2];
				switch (_encryptionOption)
				{
				case EncryptionOptions.ON:
					if (encryptionOptions == EncryptionOptions.NOT_SUP)
					{
						_physicalStateObj.AddError(new SqlError(20, 0, 20, _server, SQLMessage.EncryptionNotSupportedByServer(), "", 0));
						_physicalStateObj.Dispose();
						ThrowExceptionAndWarning(_physicalStateObj);
					}
					break;
				case EncryptionOptions.OFF:
					switch (encryptionOptions)
					{
					case EncryptionOptions.OFF:
						_encryptionOption = EncryptionOptions.LOGIN;
						break;
					case EncryptionOptions.REQ:
						_encryptionOption = EncryptionOptions.ON;
						break;
					}
					break;
				case EncryptionOptions.NOT_SUP:
					if (encryptionOptions == EncryptionOptions.REQ)
					{
						_physicalStateObj.AddError(new SqlError(20, 0, 20, _server, SQLMessage.EncryptionNotSupportedByClient(), "", 0));
						_physicalStateObj.Dispose();
						ThrowExceptionAndWarning(_physicalStateObj);
					}
					break;
				}
				if (_encryptionOption == EncryptionOptions.ON || _encryptionOption == EncryptionOptions.LOGIN)
				{
					uint num5 = 0u;
					uint info = (uint)((((encrypt && !trustServerCert) || (_connHandler._accessTokenInBytes != null && !trustServerCert)) ? 1 : 0) | (flag ? 2 : 0));
					if (encrypt && !integratedSecurity)
					{
						info |= 0x10;
					}
					num5 = _physicalStateObj.EnableSsl(ref info);
					if (num5 != 0)
					{
						_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
						ThrowExceptionAndWarning(_physicalStateObj);
					}
					WaitForSSLHandShakeToComplete(ref num5);
					_physicalStateObj.ClearAllWritePackets();
				}
				goto IL_03d0;
			}
			case 2:
			{
				num2 = (array[num++] << 8) | array[num++];
				_ = array[num++];
				_ = array[num++];
				byte b = 1;
				if (array[num2] == b)
				{
					return PreLoginHandshakeStatus.InstanceFailure;
				}
				goto IL_03d0;
			}
			case 3:
				num += 4;
				goto IL_03d0;
			case 4:
				num2 = (array[num++] << 8) | array[num++];
				_ = array[num++];
				_ = array[num++];
				marsCapable = ((array[num2] != 0) ? true : false);
				goto IL_03d0;
			case 5:
				num += 4;
				goto IL_03d0;
			case 6:
				num2 = (array[num++] << 8) | array[num++];
				_ = array[num++];
				_ = array[num++];
				if (array[num2] != 0 && array[num2] != 1)
				{
					throw SQL.ParsingErrorValue(ParsingErrorState.FedAuthRequiredPreLoginResponseInvalidValue, array[num2]);
				}
				if (_connHandler.ConnectionOptions != null || _connHandler._accessTokenInBytes != null)
				{
					fedAuthRequired = array[num2] == 1;
				}
				goto IL_03d0;
			default:
				num += 4;
				goto IL_03d0;
			case 255:
				break;
				IL_03d0:
				if (num < array.Length)
				{
					goto IL_03d6;
				}
				break;
			}
			break;
			IL_03d6:
			num3 = array[num++];
		}
		return PreLoginHandshakeStatus.Successful;
	}

	internal void Deactivate(bool connectionIsDoomed)
	{
		if (MARSOn)
		{
			_sessionPool.Deactivate();
		}
		if (!connectionIsDoomed && _physicalStateObj != null)
		{
			if (_physicalStateObj._pendingData)
			{
				DrainData(_physicalStateObj);
			}
			if (_physicalStateObj.HasOpenResult)
			{
				_physicalStateObj.DecrementOpenResultCount();
			}
		}
		SqlInternalTransaction currentTransaction = CurrentTransaction;
		if (currentTransaction != null && currentTransaction.HasParentTransaction)
		{
			currentTransaction.CloseFromConnection();
		}
		Statistics = null;
	}

	internal void Disconnect()
	{
		if (_sessionPool != null)
		{
			_sessionPool.Dispose();
		}
		if (_state == TdsParserState.Closed)
		{
			return;
		}
		_state = TdsParserState.Closed;
		try
		{
			if (!_physicalStateObj.HasOwner)
			{
				_physicalStateObj.SniContext = SniContext.Snix_Close;
				_physicalStateObj.Dispose();
			}
			else
			{
				_physicalStateObj.DecrementPendingCallbacks(release: false);
			}
			if (_pMarsPhysicalConObj != null)
			{
				_pMarsPhysicalConObj.Dispose();
			}
		}
		finally
		{
			_pMarsPhysicalConObj = null;
		}
	}

	private void FireInfoMessageEvent(SqlConnection connection, TdsParserStateObject stateObj, SqlError error)
	{
		string serverVersion = null;
		if (_state == TdsParserState.OpenLoggedIn)
		{
			serverVersion = _connHandler.ServerVersion;
		}
		SqlException exception = SqlException.CreateException(new SqlErrorCollection { error }, serverVersion, _connHandler);
		connection.OnInfoMessage(new SqlInfoMessageEventArgs(exception), out var notified);
		if (notified)
		{
			stateObj._syncOverAsync = true;
		}
	}

	internal void DisconnectTransaction(SqlInternalTransaction internalTransaction)
	{
		if (_currentTransaction != null && _currentTransaction == internalTransaction)
		{
			_currentTransaction = null;
		}
	}

	internal void RollbackOrphanedAPITransactions()
	{
		SqlInternalTransaction currentTransaction = CurrentTransaction;
		if (currentTransaction != null && currentTransaction.HasParentTransaction && currentTransaction.IsOrphaned)
		{
			currentTransaction.CloseFromConnection();
		}
	}

	internal void ThrowExceptionAndWarning(TdsParserStateObject stateObj, bool callerHasConnectionLock = false, bool asyncClose = false)
	{
		SqlException ex = null;
		bool broken;
		SqlErrorCollection fullErrorAndWarningCollection = stateObj.GetFullErrorAndWarningCollection(out broken);
		broken &= _state != TdsParserState.Closed;
		if (broken)
		{
			if (_state == TdsParserState.OpenNotLoggedIn && (_connHandler.ConnectionOptions.MultiSubnetFailover || _loginWithFailover) && fullErrorAndWarningCollection.Count == 1 && (fullErrorAndWarningCollection[0].Number == -2 || (long)fullErrorAndWarningCollection[0].Number == 258))
			{
				broken = false;
				Disconnect();
			}
			else
			{
				_state = TdsParserState.Broken;
			}
		}
		if (fullErrorAndWarningCollection != null && fullErrorAndWarningCollection.Count > 0)
		{
			string serverVersion = null;
			if (_state == TdsParserState.OpenLoggedIn)
			{
				serverVersion = _connHandler.ServerVersion;
			}
			ex = ((fullErrorAndWarningCollection.Count != 1 || fullErrorAndWarningCollection[0].Exception == null) ? SqlException.CreateException(fullErrorAndWarningCollection, serverVersion, _connHandler) : SqlException.CreateException(fullErrorAndWarningCollection, serverVersion, _connHandler, fullErrorAndWarningCollection[0].Exception));
		}
		if (ex == null)
		{
			return;
		}
		if (broken)
		{
			stateObj._networkPacketTaskSource?.TrySetException(ADP.ExceptionWithStackTrace(ex));
		}
		if (asyncClose)
		{
			SqlInternalConnectionTds connHandler = _connHandler;
			Action<Action> wrapCloseInAction = delegate(Action closeAction)
			{
				Task.Factory.StartNew(delegate
				{
					connHandler._parserLock.Wait(canReleaseFromAnyThread: false);
					connHandler.ThreadHasParserLockForClose = true;
					try
					{
						closeAction();
					}
					finally
					{
						connHandler.ThreadHasParserLockForClose = false;
						connHandler._parserLock.Release();
					}
				});
			};
			_connHandler.OnError(ex, broken, wrapCloseInAction);
			return;
		}
		bool threadHasParserLockForClose = _connHandler.ThreadHasParserLockForClose;
		if (callerHasConnectionLock)
		{
			_connHandler.ThreadHasParserLockForClose = true;
		}
		try
		{
			_connHandler.OnError(ex, broken);
		}
		finally
		{
			if (callerHasConnectionLock)
			{
				_connHandler.ThreadHasParserLockForClose = threadHasParserLockForClose;
			}
		}
	}

	internal SqlError ProcessSNIError(TdsParserStateObject stateObj)
	{
		SNIErrorDetails sniErrorDetails = GetSniErrorDetails();
		if (sniErrorDetails.sniErrorNumber != 0)
		{
			switch (sniErrorDetails.sniErrorNumber)
			{
			case 47u:
				throw SQL.MultiSubnetFailoverWithMoreThan64IPs();
			case 48u:
				throw SQL.MultiSubnetFailoverWithInstanceSpecified();
			case 49u:
				throw SQL.MultiSubnetFailoverWithNonTcpProtocol();
			}
		}
		string text = sniErrorDetails.errorMessage;
		_ = TdsParserStateObjectFactory.UseManagedSNI;
		string sniContextEnumName = TdsEnums.GetSniContextEnumName(stateObj.SniContext);
		string resourceString = global::SR.GetResourceString(sniContextEnumName, sniContextEnumName);
		string text2 = string.Format(null, "SNI_PN{0}", sniErrorDetails.provider);
		string resourceString2 = global::SR.GetResourceString(text2, text2);
		if (sniErrorDetails.sniErrorNumber == 0)
		{
			int num = text.IndexOf(':');
			if (0 <= num)
			{
				int length = text.Length;
				length -= Environment.NewLine.Length;
				num += 2;
				length -= num;
				if (length > 0)
				{
					text = text.Substring(num, length);
				}
			}
		}
		else if (TdsParserStateObjectFactory.UseManagedSNI)
		{
			string sNIErrorMessage = SQL.GetSNIErrorMessage((int)sniErrorDetails.sniErrorNumber);
			text = ((text != string.Empty) ? (sNIErrorMessage + ": " + text) : sNIErrorMessage);
		}
		else
		{
			text = SQL.GetSNIErrorMessage((int)sniErrorDetails.sniErrorNumber);
			if (sniErrorDetails.sniErrorNumber == 50)
			{
				text += LocalDBAPI.GetLocalDBMessage((int)sniErrorDetails.nativeError);
			}
		}
		text = string.Format(null, "{0} (provider: {1}, error: {2} - {3})", resourceString, resourceString2, (int)sniErrorDetails.sniErrorNumber, text);
		return new SqlError((int)sniErrorDetails.nativeError, 0, 20, _server, text, sniErrorDetails.function, (int)sniErrorDetails.lineNumber, sniErrorDetails.nativeError, sniErrorDetails.exception);
	}

	internal void CheckResetConnection(TdsParserStateObject stateObj)
	{
		if (!_fResetConnection || stateObj._fResetConnectionSent)
		{
			return;
		}
		try
		{
			if (_fMARS && !stateObj._fResetEventOwned)
			{
				stateObj._fResetEventOwned = _resetConnectionEvent.WaitOne(stateObj.GetTimeoutRemaining());
				if (stateObj._fResetEventOwned && stateObj.TimeoutHasExpired)
				{
					stateObj._fResetEventOwned = !_resetConnectionEvent.Set();
					stateObj.TimeoutTime = 0L;
				}
				if (!stateObj._fResetEventOwned)
				{
					stateObj.ResetBuffer();
					stateObj.AddError(new SqlError(-2, 0, 11, _server, _connHandler.TimeoutErrorInternal.GetErrorMessage(), "", 0, 258u));
					ThrowExceptionAndWarning(stateObj, callerHasConnectionLock: true);
				}
			}
			if (_fResetConnection)
			{
				if (_fPreserveTransaction)
				{
					stateObj._outBuff[1] = (byte)(stateObj._outBuff[1] | 0x10);
				}
				else
				{
					stateObj._outBuff[1] = (byte)(stateObj._outBuff[1] | 8);
				}
				if (!_fMARS)
				{
					_fResetConnection = false;
					_fPreserveTransaction = false;
				}
				else
				{
					stateObj._fResetConnectionSent = true;
				}
			}
			else if (_fMARS && stateObj._fResetEventOwned)
			{
				stateObj._fResetEventOwned = !_resetConnectionEvent.Set();
			}
		}
		catch (Exception)
		{
			if (_fMARS && stateObj._fResetEventOwned)
			{
				stateObj._fResetConnectionSent = false;
				stateObj._fResetEventOwned = !_resetConnectionEvent.Set();
			}
			throw;
		}
	}

	internal void WriteShort(int v, TdsParserStateObject stateObj)
	{
		if (stateObj._outBytesUsed + 2 > stateObj._outBuff.Length)
		{
			stateObj.WriteByte((byte)(v & 0xFF));
			stateObj.WriteByte((byte)((v >> 8) & 0xFF));
		}
		else
		{
			stateObj._outBuff[stateObj._outBytesUsed] = (byte)(v & 0xFF);
			stateObj._outBuff[stateObj._outBytesUsed + 1] = (byte)((v >> 8) & 0xFF);
			stateObj._outBytesUsed += 2;
		}
	}

	internal void WriteUnsignedShort(ushort us, TdsParserStateObject stateObj)
	{
		WriteShort((short)us, stateObj);
	}

	internal void WriteUnsignedInt(uint i, TdsParserStateObject stateObj)
	{
		WriteInt((int)i, stateObj);
	}

	internal void WriteInt(int v, TdsParserStateObject stateObj)
	{
		if (stateObj._outBytesUsed + 4 > stateObj._outBuff.Length)
		{
			for (int i = 0; i < 32; i += 8)
			{
				stateObj.WriteByte((byte)((v >> i) & 0xFF));
			}
		}
		else
		{
			stateObj._outBuff[stateObj._outBytesUsed] = (byte)(v & 0xFF);
			stateObj._outBuff[stateObj._outBytesUsed + 1] = (byte)((v >> 8) & 0xFF);
			stateObj._outBuff[stateObj._outBytesUsed + 2] = (byte)((v >> 16) & 0xFF);
			stateObj._outBuff[stateObj._outBytesUsed + 3] = (byte)((v >> 24) & 0xFF);
			stateObj._outBytesUsed += 4;
		}
	}

	internal void WriteFloat(float v, TdsParserStateObject stateObj)
	{
		byte[] bytes = BitConverter.GetBytes(v);
		stateObj.WriteByteArray(bytes, bytes.Length, 0);
	}

	internal void WriteLong(long v, TdsParserStateObject stateObj)
	{
		if (stateObj._outBytesUsed + 8 > stateObj._outBuff.Length)
		{
			for (int i = 0; i < 64; i += 8)
			{
				stateObj.WriteByte((byte)((v >> i) & 0xFF));
			}
			return;
		}
		stateObj._outBuff[stateObj._outBytesUsed] = (byte)(v & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 1] = (byte)((v >> 8) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 2] = (byte)((v >> 16) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 3] = (byte)((v >> 24) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 4] = (byte)((v >> 32) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 5] = (byte)((v >> 40) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 6] = (byte)((v >> 48) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 7] = (byte)((v >> 56) & 0xFF);
		stateObj._outBytesUsed += 8;
	}

	internal void WritePartialLong(long v, int length, TdsParserStateObject stateObj)
	{
		if (stateObj._outBytesUsed + length > stateObj._outBuff.Length)
		{
			for (int i = 0; i < length * 8; i += 8)
			{
				stateObj.WriteByte((byte)((v >> i) & 0xFF));
			}
			return;
		}
		for (int j = 0; j < length; j++)
		{
			stateObj._outBuff[stateObj._outBytesUsed + j] = (byte)((v >> j * 8) & 0xFF);
		}
		stateObj._outBytesUsed += length;
	}

	internal void WriteUnsignedLong(ulong uv, TdsParserStateObject stateObj)
	{
		WriteLong((long)uv, stateObj);
	}

	internal void WriteDouble(double v, TdsParserStateObject stateObj)
	{
		byte[] bytes = BitConverter.GetBytes(v);
		stateObj.WriteByteArray(bytes, bytes.Length, 0);
	}

	internal void PrepareResetConnection(bool preserveTransaction)
	{
		_fResetConnection = true;
		_fPreserveTransaction = preserveTransaction;
	}

	internal bool Run(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj)
	{
		bool syncOverAsync = stateObj._syncOverAsync;
		try
		{
			stateObj._syncOverAsync = true;
			TryRun(runBehavior, cmdHandler, dataStream, bulkCopyHandler, stateObj, out var dataReady);
			return dataReady;
		}
		finally
		{
			stateObj._syncOverAsync = syncOverAsync;
		}
	}

	internal static bool IsValidTdsToken(byte token)
	{
		if (token != 170 && token != 171 && token != 173 && token != 227 && token != 172 && token != 121 && token != 160 && token != 161 && token != 129 && token != 136 && token != 164 && token != 165 && token != 169 && token != 211 && token != 209 && token != 210 && token != 253 && token != 254 && token != byte.MaxValue && token != 57 && token != 237 && token != 124 && token != 120 && token != 237 && token != 174)
		{
			return token == 228;
		}
		return true;
	}

	internal bool TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, out bool dataReady)
	{
		if (TdsParserState.Broken == State || State == TdsParserState.Closed)
		{
			dataReady = true;
			return true;
		}
		dataReady = false;
		do
		{
			if (stateObj._internalTimeout)
			{
				runBehavior = RunBehavior.Attention;
			}
			if (TdsParserState.Broken == State || State == TdsParserState.Closed)
			{
				break;
			}
			if (!stateObj._accumulateInfoEvents && stateObj._pendingInfoEvents != null)
			{
				if (RunBehavior.Clean != (RunBehavior.Clean & runBehavior))
				{
					SqlConnection sqlConnection = null;
					if (_connHandler != null)
					{
						sqlConnection = _connHandler.Connection;
					}
					if (sqlConnection != null && sqlConnection.FireInfoMessageEventOnUserErrors)
					{
						foreach (SqlError pendingInfoEvent in stateObj._pendingInfoEvents)
						{
							FireInfoMessageEvent(sqlConnection, stateObj, pendingInfoEvent);
						}
					}
					else
					{
						foreach (SqlError pendingInfoEvent2 in stateObj._pendingInfoEvents)
						{
							stateObj.AddWarning(pendingInfoEvent2);
						}
					}
				}
				stateObj._pendingInfoEvents = null;
			}
			if (!stateObj.TryReadByte(out var value))
			{
				return false;
			}
			if (!IsValidTdsToken(value))
			{
				_state = TdsParserState.Broken;
				_connHandler.BreakConnection();
				throw SQL.ParsingError();
			}
			if (!TryGetTokenLength(value, stateObj, out var tokenLength))
			{
				return false;
			}
			switch (value)
			{
			case 170:
				stateObj._errorTokenReceived = true;
				goto case 171;
			case 171:
			{
				if (!TryProcessError(value, stateObj, out var error))
				{
					return false;
				}
				if (value == 171 && stateObj._accumulateInfoEvents)
				{
					if (stateObj._pendingInfoEvents == null)
					{
						stateObj._pendingInfoEvents = new List<SqlError>();
					}
					stateObj._pendingInfoEvents.Add(error);
					stateObj._syncOverAsync = true;
				}
				else if (RunBehavior.Clean != (RunBehavior.Clean & runBehavior))
				{
					SqlConnection sqlConnection2 = null;
					if (_connHandler != null)
					{
						sqlConnection2 = _connHandler.Connection;
					}
					if (sqlConnection2 != null && sqlConnection2.FireInfoMessageEventOnUserErrors && error.Class <= 16)
					{
						FireInfoMessageEvent(sqlConnection2, stateObj, error);
					}
					else if (error.Class < 11)
					{
						stateObj.AddWarning(error);
					}
					else if (error.Class < 20)
					{
						stateObj.AddError(error);
						if (dataStream != null && !dataStream.IsInitialized)
						{
							runBehavior = RunBehavior.UntilDone;
						}
					}
					else
					{
						stateObj.AddError(error);
						runBehavior = RunBehavior.UntilDone;
					}
				}
				else if (error.Class >= 20)
				{
					stateObj.AddError(error);
				}
				break;
			}
			case 165:
				if (dataStream != null)
				{
					if (!TryProcessColInfo(dataStream.MetaData, dataStream, stateObj, out var metaData2))
					{
						return false;
					}
					if (!dataStream.TrySetMetaData(metaData2, moreInfo: false))
					{
						return false;
					}
					dataStream.BrowseModeInfoConsumed = true;
				}
				else if (!stateObj.TrySkipBytes(tokenLength))
				{
					return false;
				}
				break;
			case 253:
			case 254:
			case byte.MaxValue:
				if (!TryProcessDone(cmdHandler, dataStream, ref runBehavior, stateObj))
				{
					return false;
				}
				if (value == 254)
				{
					cmdHandler?.OnDoneProc();
				}
				break;
			case 169:
				if (!stateObj.TrySkipBytes(tokenLength))
				{
					return false;
				}
				break;
			case 136:
			{
				stateObj.CloneCleanupAltMetaDataSetArray();
				if (stateObj._cleanupAltMetaDataSetArray == null)
				{
					stateObj._cleanupAltMetaDataSetArray = new _SqlMetaDataSetCollection();
				}
				if (!TryProcessAltMetaData(tokenLength, stateObj, out var metaData))
				{
					return false;
				}
				stateObj._cleanupAltMetaDataSetArray.SetAltMetaData(metaData);
				if (dataStream != null)
				{
					if (!stateObj.TryPeekByte(out var value3))
					{
						return false;
					}
					if (!dataStream.TrySetAltMetaDataSet(metaData, 136 != value3))
					{
						return false;
					}
				}
				break;
			}
			case 211:
				if (!stateObj.TryStartNewRow(isNullCompressed: false))
				{
					return false;
				}
				if (RunBehavior.ReturnImmediately != (RunBehavior.ReturnImmediately & runBehavior))
				{
					if (!stateObj.TryReadUInt16(out var value2))
					{
						return false;
					}
					if (!TrySkipRow(stateObj._cleanupAltMetaDataSetArray.GetAltMetaData(value2), stateObj))
					{
						return false;
					}
				}
				else
				{
					dataReady = true;
				}
				break;
			case 227:
			{
				stateObj._syncOverAsync = true;
				if (!TryProcessEnvChange(tokenLength, stateObj, out var sqlEnvChange))
				{
					return false;
				}
				for (int i = 0; i < sqlEnvChange.Length; i++)
				{
					if (sqlEnvChange[i] == null || Connection.IgnoreEnvChange)
					{
						continue;
					}
					switch (sqlEnvChange[i].type)
					{
					case 8:
					case 11:
						_currentTransaction = _pendingTransaction;
						_pendingTransaction = null;
						if (_currentTransaction != null)
						{
							_currentTransaction.TransactionId = sqlEnvChange[i].newLongValue;
						}
						else
						{
							TransactionType type = TransactionType.LocalFromTSQL;
							_currentTransaction = new SqlInternalTransaction(_connHandler, type, null, sqlEnvChange[i].newLongValue);
						}
						if (_statistics != null && !_statisticsIsInTransaction)
						{
							_statistics.SafeIncrement(ref _statistics._transactions);
						}
						_statisticsIsInTransaction = true;
						_retainedTransactionId = 0L;
						break;
					case 9:
					case 12:
					case 17:
						_retainedTransactionId = 0L;
						goto case 10;
					case 10:
						if (_currentTransaction != null)
						{
							if (9 == sqlEnvChange[i].type)
							{
								_currentTransaction.Completed(TransactionState.Committed);
							}
							else if (10 == sqlEnvChange[i].type)
							{
								if (_currentTransaction.IsDistributed && _currentTransaction.IsActive)
								{
									_retainedTransactionId = sqlEnvChange[i].oldLongValue;
								}
								_currentTransaction.Completed(TransactionState.Aborted);
							}
							else
							{
								_currentTransaction.Completed(TransactionState.Unknown);
							}
							_currentTransaction = null;
						}
						_statisticsIsInTransaction = false;
						break;
					default:
						_connHandler.OnEnvChange(sqlEnvChange[i]);
						break;
					}
				}
				break;
			}
			case 173:
			{
				if (!TryProcessLoginAck(stateObj, out var sqlLoginAck))
				{
					return false;
				}
				_connHandler.OnLoginAck(sqlLoginAck);
				break;
			}
			case 174:
				if (!TryProcessFeatureExtAck(stateObj))
				{
					return false;
				}
				break;
			case 228:
				if (!TryProcessSessionState(stateObj, tokenLength, _connHandler._currentSessionData))
				{
					return false;
				}
				break;
			case 129:
				if (tokenLength != 65535)
				{
					if (!TryProcessMetaData(tokenLength, stateObj, out var metaData3))
					{
						return false;
					}
					stateObj._cleanupMetaData = metaData3;
				}
				else if (cmdHandler != null)
				{
					stateObj._cleanupMetaData = cmdHandler.MetaData;
				}
				if (dataStream != null)
				{
					if (!stateObj.TryPeekByte(out var value5))
					{
						return false;
					}
					if (!dataStream.TrySetMetaData(stateObj._cleanupMetaData, 164 == value5 || 165 == value5))
					{
						return false;
					}
				}
				else
				{
					bulkCopyHandler?.SetMetaData(stateObj._cleanupMetaData);
				}
				break;
			case 210:
				if (!stateObj.TryStartNewRow(isNullCompressed: true, stateObj._cleanupMetaData.Length))
				{
					return false;
				}
				goto IL_07d9;
			case 209:
				if (!stateObj.TryStartNewRow(isNullCompressed: false))
				{
					return false;
				}
				goto IL_07d9;
			case 121:
			{
				if (!stateObj.TryReadInt32(out var value4))
				{
					return false;
				}
				cmdHandler?.OnReturnStatus(value4);
				break;
			}
			case 172:
			{
				if (!TryProcessReturnValue(tokenLength, stateObj, out var returnValue))
				{
					return false;
				}
				cmdHandler?.OnReturnValue(returnValue, stateObj);
				break;
			}
			case 237:
				stateObj._syncOverAsync = true;
				ProcessSSPI(tokenLength);
				break;
			case 164:
				{
					if (dataStream != null)
					{
						if (!TryProcessTableName(tokenLength, stateObj, out var multiPartTableNames))
						{
							return false;
						}
						dataStream.TableNames = multiPartTableNames;
					}
					else if (!stateObj.TrySkipBytes(tokenLength))
					{
						return false;
					}
					break;
				}
				IL_07d9:
				if (bulkCopyHandler != null)
				{
					if (!TryProcessRow(stateObj._cleanupMetaData, bulkCopyHandler.CreateRowBuffer(), bulkCopyHandler.CreateIndexMap(), stateObj))
					{
						return false;
					}
				}
				else if (RunBehavior.ReturnImmediately != (RunBehavior.ReturnImmediately & runBehavior))
				{
					if (!TrySkipRow(stateObj._cleanupMetaData, stateObj))
					{
						return false;
					}
				}
				else
				{
					dataReady = true;
				}
				if (_statistics != null)
				{
					_statistics.WaitForDoneAfterRow = true;
				}
				break;
			}
		}
		while ((stateObj._pendingData && RunBehavior.ReturnImmediately != (RunBehavior.ReturnImmediately & runBehavior)) || (!stateObj._pendingData && stateObj._attentionSent && !stateObj._attentionReceived));
		if (!stateObj._pendingData && CurrentTransaction != null)
		{
			CurrentTransaction.Activate();
		}
		if (stateObj._attentionReceived)
		{
			SpinWait.SpinUntil(() => !stateObj._attentionSending);
			if (stateObj._attentionSent)
			{
				stateObj._attentionSent = false;
				stateObj._attentionReceived = false;
				if (RunBehavior.Clean != (RunBehavior.Clean & runBehavior) && !stateObj._internalTimeout)
				{
					stateObj.AddError(new SqlError(0, 0, 11, _server, SQLMessage.OperationCancelled(), "", 0));
				}
			}
		}
		if (stateObj.HasErrorOrWarning)
		{
			ThrowExceptionAndWarning(stateObj);
		}
		return true;
	}

	private bool TryProcessEnvChange(int tokenLength, TdsParserStateObject stateObj, out SqlEnvChange[] sqlEnvChange)
	{
		int i = 0;
		int num = 0;
		SqlEnvChange[] array = new SqlEnvChange[3];
		sqlEnvChange = null;
		SqlEnvChange sqlEnvChange2;
		for (; tokenLength > i; i += sqlEnvChange2.length)
		{
			if (num >= array.Length)
			{
				SqlEnvChange[] array2 = new SqlEnvChange[array.Length + 3];
				for (int j = 0; j < array.Length; j++)
				{
					array2[j] = array[j];
				}
				array = array2;
			}
			sqlEnvChange2 = new SqlEnvChange();
			if (!stateObj.TryReadByte(out sqlEnvChange2.type))
			{
				return false;
			}
			array[num] = sqlEnvChange2;
			num++;
			byte value7;
			switch (sqlEnvChange2.type)
			{
			case 1:
			case 2:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				break;
			case 3:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				if (sqlEnvChange2.newValue == "iso_1")
				{
					_defaultCodePage = 1252;
					_defaultEncoding = Encoding.GetEncoding(_defaultCodePage);
				}
				else
				{
					string s = sqlEnvChange2.newValue.Substring(2);
					_defaultCodePage = int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
					_defaultEncoding = Encoding.GetEncoding(_defaultCodePage);
				}
				break;
			case 4:
			{
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					throw SQL.SynchronousCallMayNotPend();
				}
				int num2 = int.Parse(sqlEnvChange2.newValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
				if (_physicalStateObj.SetPacketSize(num2))
				{
					_physicalStateObj.ClearAllWritePackets();
					uint unsignedPacketSize = (uint)num2;
					_physicalStateObj.SetConnectionBufferSize(ref unsignedPacketSize);
				}
				break;
			}
			case 5:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				_defaultLCID = int.Parse(sqlEnvChange2.newValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
				break;
			case 6:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				break;
			case 7:
				if (!stateObj.TryReadByte(out value7))
				{
					return false;
				}
				sqlEnvChange2.newLength = value7;
				if (sqlEnvChange2.newLength == 5)
				{
					if (!TryProcessCollation(stateObj, out sqlEnvChange2.newCollation))
					{
						return false;
					}
					_defaultCollation = sqlEnvChange2.newCollation;
					int codePage = GetCodePage(sqlEnvChange2.newCollation, stateObj);
					if (codePage != _defaultCodePage)
					{
						_defaultCodePage = codePage;
						_defaultEncoding = Encoding.GetEncoding(_defaultCodePage);
					}
					_defaultLCID = sqlEnvChange2.newCollation.LCID;
				}
				if (!stateObj.TryReadByte(out value7))
				{
					return false;
				}
				sqlEnvChange2.oldLength = value7;
				if (sqlEnvChange2.oldLength == 5 && !TryProcessCollation(stateObj, out sqlEnvChange2.oldCollation))
				{
					return false;
				}
				sqlEnvChange2.length = 3 + sqlEnvChange2.newLength + sqlEnvChange2.oldLength;
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 17:
				if (!stateObj.TryReadByte(out value7))
				{
					return false;
				}
				sqlEnvChange2.newLength = value7;
				if (sqlEnvChange2.newLength > 0)
				{
					if (!stateObj.TryReadInt64(out sqlEnvChange2.newLongValue))
					{
						return false;
					}
				}
				else
				{
					sqlEnvChange2.newLongValue = 0L;
				}
				if (!stateObj.TryReadByte(out value7))
				{
					return false;
				}
				sqlEnvChange2.oldLength = value7;
				if (sqlEnvChange2.oldLength > 0)
				{
					if (!stateObj.TryReadInt64(out sqlEnvChange2.oldLongValue))
					{
						return false;
					}
				}
				else
				{
					sqlEnvChange2.oldLongValue = 0L;
				}
				sqlEnvChange2.length = 3 + sqlEnvChange2.newLength + sqlEnvChange2.oldLength;
				break;
			case 13:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				break;
			case 15:
				if (!stateObj.TryReadInt32(out sqlEnvChange2.newLength))
				{
					return false;
				}
				sqlEnvChange2.newBinValue = new byte[sqlEnvChange2.newLength];
				if (!stateObj.TryReadByteArray(sqlEnvChange2.newBinValue, 0, sqlEnvChange2.newLength))
				{
					return false;
				}
				if (!stateObj.TryReadByte(out value7))
				{
					return false;
				}
				sqlEnvChange2.oldLength = value7;
				sqlEnvChange2.length = 5 + sqlEnvChange2.newLength;
				break;
			case 16:
			case 18:
				if (!TryReadTwoBinaryFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				break;
			case 19:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				break;
			case 20:
			{
				if (!stateObj.TryReadUInt16(out var value))
				{
					return false;
				}
				sqlEnvChange2.newLength = value;
				if (!stateObj.TryReadByte(out var value2))
				{
					return false;
				}
				if (!stateObj.TryReadUInt16(out var value3))
				{
					return false;
				}
				if (!stateObj.TryReadUInt16(out var value4))
				{
					return false;
				}
				if (!stateObj.TryReadString(value4, out var value5))
				{
					return false;
				}
				sqlEnvChange2.newRoutingInfo = new RoutingInfo(value2, value3, value5);
				if (!stateObj.TryReadUInt16(out var value6))
				{
					return false;
				}
				if (!stateObj.TrySkipBytes(value6))
				{
					return false;
				}
				sqlEnvChange2.length = sqlEnvChange2.newLength + value6 + 5;
				break;
			}
			}
		}
		sqlEnvChange = array;
		return true;
	}

	private bool TryReadTwoBinaryFields(SqlEnvChange env, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		env.newLength = value;
		env.newBinValue = new byte[env.newLength];
		if (!stateObj.TryReadByteArray(env.newBinValue, 0, env.newLength))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out value))
		{
			return false;
		}
		env.oldLength = value;
		env.oldBinValue = new byte[env.oldLength];
		if (!stateObj.TryReadByteArray(env.oldBinValue, 0, env.oldLength))
		{
			return false;
		}
		env.length = 3 + env.newLength + env.oldLength;
		return true;
	}

	private bool TryReadTwoStringFields(SqlEnvChange env, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		if (!stateObj.TryReadString(value, out var value2))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value3))
		{
			return false;
		}
		if (!stateObj.TryReadString(value3, out var value4))
		{
			return false;
		}
		env.newLength = value;
		env.newValue = value2;
		env.oldLength = value3;
		env.oldValue = value4;
		env.length = 3 + env.newLength * 2 + env.oldLength * 2;
		return true;
	}

	private bool TryProcessDone(SqlCommand cmd, SqlDataReader reader, ref RunBehavior run, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadUInt16(out var value))
		{
			return false;
		}
		if (!stateObj.TryReadUInt16(out var value2))
		{
			return false;
		}
		if (!stateObj.TryReadInt64(out var value3))
		{
			return false;
		}
		int num = (int)value3;
		if (32 == (value & 0x20))
		{
			stateObj._attentionReceived = true;
		}
		if (cmd != null && 16 == (value & 0x10))
		{
			if (value2 != 193)
			{
				cmd.InternalRecordsAffected = num;
			}
			if (stateObj._receivedColMetaData || value2 != 193)
			{
				cmd.OnStatementCompleted(num);
			}
		}
		stateObj._receivedColMetaData = false;
		if (2 == (2 & value) && stateObj.ErrorCount == 0 && !stateObj._errorTokenReceived && RunBehavior.Clean != (RunBehavior.Clean & run))
		{
			stateObj.AddError(new SqlError(0, 0, 11, _server, SQLMessage.SevereError(), "", 0));
			if (reader != null && !reader.IsInitialized)
			{
				run = RunBehavior.UntilDone;
			}
		}
		if (256 == (0x100 & value) && RunBehavior.Clean != (RunBehavior.Clean & run))
		{
			stateObj.AddError(new SqlError(0, 0, 20, _server, SQLMessage.SevereError(), "", 0));
			if (reader != null && !reader.IsInitialized)
			{
				run = RunBehavior.UntilDone;
			}
		}
		ProcessSqlStatistics(value2, value, num);
		if (1 != (value & 1))
		{
			stateObj._errorTokenReceived = false;
			if (stateObj._inBytesUsed >= stateObj._inBytesRead)
			{
				stateObj._pendingData = false;
			}
		}
		if (!stateObj._pendingData && stateObj._hasOpenResult)
		{
			stateObj.DecrementOpenResultCount();
		}
		return true;
	}

	private void ProcessSqlStatistics(ushort curCmd, ushort status, int count)
	{
		if (_statistics != null)
		{
			if (_statistics.WaitForDoneAfterRow)
			{
				_statistics.SafeIncrement(ref _statistics._sumResultSets);
				_statistics.WaitForDoneAfterRow = false;
			}
			if (16 != (status & 0x10))
			{
				count = 0;
			}
			switch (curCmd)
			{
			case 195:
			case 196:
			case 197:
			case 279:
				_statistics.SafeIncrement(ref _statistics._iduCount);
				_statistics.SafeAdd(ref _statistics._iduRows, count);
				if (!_statisticsIsInTransaction)
				{
					_statistics.SafeIncrement(ref _statistics._transactions);
				}
				break;
			case 193:
				_statistics.SafeIncrement(ref _statistics._selectCount);
				_statistics.SafeAdd(ref _statistics._selectRows, count);
				break;
			case 212:
				if (!_statisticsIsInTransaction)
				{
					_statistics.SafeIncrement(ref _statistics._transactions);
				}
				_statisticsIsInTransaction = true;
				break;
			case 32:
				_statistics.SafeIncrement(ref _statistics._cursorOpens);
				break;
			case 210:
				_statisticsIsInTransaction = false;
				break;
			case 213:
				_statisticsIsInTransaction = false;
				break;
			}
		}
		else
		{
			switch (curCmd)
			{
			case 212:
				_statisticsIsInTransaction = true;
				break;
			case 210:
			case 213:
				_statisticsIsInTransaction = false;
				break;
			case 211:
				break;
			}
		}
	}

	private bool TryProcessFeatureExtAck(TdsParserStateObject stateObj)
	{
		byte value;
		do
		{
			if (!stateObj.TryReadByte(out value))
			{
				return false;
			}
			if (value != byte.MaxValue)
			{
				if (!stateObj.TryReadUInt32(out var value2))
				{
					return false;
				}
				byte[] array = new byte[value2];
				if (value2 != 0 && !stateObj.TryReadByteArray(array, 0, checked((int)value2)))
				{
					return false;
				}
				_connHandler.OnFeatureExtAck(value, array);
			}
		}
		while (value != byte.MaxValue);
		return true;
	}

	private bool TryProcessSessionState(TdsParserStateObject stateObj, int length, SessionData sdata)
	{
		if (length < 5)
		{
			throw SQL.ParsingError();
		}
		if (!stateObj.TryReadUInt32(out var value))
		{
			return false;
		}
		if (value == uint.MaxValue)
		{
			_connHandler.DoNotPoolThisConnection();
		}
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		if (value2 > 1)
		{
			throw SQL.ParsingError();
		}
		bool flag = value2 != 0;
		length -= 5;
		while (length > 0)
		{
			if (!stateObj.TryReadByte(out var value3))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out var value4))
			{
				return false;
			}
			int value5;
			if (value4 < byte.MaxValue)
			{
				value5 = value4;
			}
			else if (!stateObj.TryReadInt32(out value5))
			{
				return false;
			}
			byte[] array = null;
			checked
			{
				lock (sdata._delta)
				{
					if (sdata._delta[value3] == null)
					{
						array = new byte[value5];
						sdata._delta[value3] = new SessionStateRecord
						{
							_version = value,
							_dataLength = value5,
							_data = array,
							_recoverable = flag
						};
						sdata._deltaDirty = true;
						if (!flag)
						{
							sdata._unrecoverableStatesCount = (byte)(unchecked((uint)sdata._unrecoverableStatesCount) + 1u);
						}
					}
					else if (sdata._delta[value3]._version <= value)
					{
						SessionStateRecord sessionStateRecord = sdata._delta[value3];
						sessionStateRecord._version = value;
						sessionStateRecord._dataLength = value5;
						if (sessionStateRecord._recoverable != flag)
						{
							if (flag)
							{
								unchecked
								{
									sdata._unrecoverableStatesCount--;
								}
							}
							else
							{
								sdata._unrecoverableStatesCount = (byte)(unchecked((uint)sdata._unrecoverableStatesCount) + 1u);
							}
							sessionStateRecord._recoverable = flag;
						}
						array = sessionStateRecord._data;
						if (array.Length < value5)
						{
							array = (sessionStateRecord._data = new byte[value5]);
						}
					}
				}
				if (array != null)
				{
					if (!stateObj.TryReadByteArray(array, 0, value5))
					{
						return false;
					}
				}
				else if (!stateObj.TrySkipBytes(value5))
				{
					return false;
				}
			}
			length = ((value4 >= byte.MaxValue) ? (length - (6 + value5)) : (length - (2 + value5)));
		}
		return true;
	}

	private bool TryProcessLoginAck(TdsParserStateObject stateObj, out SqlLoginAck sqlLoginAck)
	{
		SqlLoginAck sqlLoginAck2 = new SqlLoginAck();
		sqlLoginAck = null;
		if (!stateObj.TrySkipBytes(1))
		{
			return false;
		}
		byte[] array = new byte[4];
		if (!stateObj.TryReadByteArray(array, 0, array.Length))
		{
			return false;
		}
		sqlLoginAck2.tdsVersion = (uint)((((((array[0] << 8) | array[1]) << 8) | array[2]) << 8) | array[3]);
		uint num = sqlLoginAck2.tdsVersion & 0xFF00FFFFu;
		uint num2 = (sqlLoginAck2.tdsVersion >> 16) & 0xFF;
		switch (num)
		{
		case 1912602626u:
			if (num2 != 9)
			{
				throw SQL.InvalidTDSVersion();
			}
			_isYukon = true;
			break;
		case 1929379843u:
			if (num2 != 11)
			{
				throw SQL.InvalidTDSVersion();
			}
			_isKatmai = true;
			break;
		case 1946157060u:
			if (num2 != 0)
			{
				throw SQL.InvalidTDSVersion();
			}
			_isDenali = true;
			break;
		default:
			throw SQL.InvalidTDSVersion();
		}
		_isKatmai |= _isDenali;
		_isYukon |= _isKatmai;
		stateObj._outBytesUsed = stateObj._outputHeaderLen;
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		if (!stateObj.TrySkipBytes(value * 2))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out sqlLoginAck2.majorVersion))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out sqlLoginAck2.minorVersion))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value3))
		{
			return false;
		}
		sqlLoginAck2.buildNum = (short)((value2 << 8) + value3);
		_state = TdsParserState.OpenLoggedIn;
		if (_fMARS)
		{
			_resetConnectionEvent = new AutoResetEvent(initialState: true);
		}
		if (_connHandler.ConnectionOptions.UserInstance && string.IsNullOrEmpty(_connHandler.InstanceName))
		{
			stateObj.AddError(new SqlError(0, 0, 20, Server, SQLMessage.UserInstanceFailure(), "", 0));
			ThrowExceptionAndWarning(stateObj);
		}
		sqlLoginAck = sqlLoginAck2;
		return true;
	}

	internal bool TryProcessError(byte token, TdsParserStateObject stateObj, out SqlError error)
	{
		error = null;
		if (!stateObj.TryReadInt32(out var value))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value3))
		{
			return false;
		}
		if (!stateObj.TryReadUInt16(out var value4))
		{
			return false;
		}
		if (!stateObj.TryReadString(value4, out var value5))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value6))
		{
			return false;
		}
		string server;
		if (value6 == 0)
		{
			server = _server;
		}
		else if (!stateObj.TryReadString(value6, out server))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out value6))
		{
			return false;
		}
		if (!stateObj.TryReadString(value6, out var value7))
		{
			return false;
		}
		int value8;
		if (_isYukon)
		{
			if (!stateObj.TryReadInt32(out value8))
			{
				return false;
			}
		}
		else
		{
			if (!stateObj.TryReadUInt16(out var value9))
			{
				return false;
			}
			value8 = value9;
			if (_state == TdsParserState.OpenNotLoggedIn)
			{
				if (!stateObj.TryPeekByte(out var value10))
				{
					return false;
				}
				if (value10 == 0)
				{
					if (!stateObj.TryReadUInt16(out var value11))
					{
						return false;
					}
					value8 = (value8 << 16) + value11;
				}
			}
		}
		error = new SqlError(value, value2, value3, _server, value5, value7, value8);
		return true;
	}

	internal bool TryProcessReturnValue(int length, TdsParserStateObject stateObj, out SqlReturnValue returnValue)
	{
		returnValue = null;
		SqlReturnValue sqlReturnValue = new SqlReturnValue();
		sqlReturnValue.length = length;
		if (!stateObj.TryReadUInt16(out var _))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		if (value2 > 0 && !stateObj.TryReadString(value2, out sqlReturnValue.parameter))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var _))
		{
			return false;
		}
		if (!stateObj.TryReadUInt32(out var value4))
		{
			return false;
		}
		if (!stateObj.TryReadUInt16(out var _))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value6))
		{
			return false;
		}
		int tokenLength;
		if (value6 == 241)
		{
			tokenLength = 65535;
		}
		else if (IsVarTimeTds(value6))
		{
			tokenLength = 0;
		}
		else if (value6 == 40)
		{
			tokenLength = 3;
		}
		else if (!TryGetTokenLength(value6, stateObj, out tokenLength))
		{
			return false;
		}
		sqlReturnValue.metaType = MetaType.GetSqlDataType(value6, value4, tokenLength);
		sqlReturnValue.type = sqlReturnValue.metaType.SqlDbType;
		sqlReturnValue.tdsType = sqlReturnValue.metaType.NullableType;
		sqlReturnValue.isNullable = true;
		if (tokenLength == 65535)
		{
			sqlReturnValue.metaType = MetaType.GetMaxMetaTypeFromMetaType(sqlReturnValue.metaType);
		}
		if (sqlReturnValue.type == SqlDbType.Decimal)
		{
			if (!stateObj.TryReadByte(out sqlReturnValue.precision))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out sqlReturnValue.scale))
			{
				return false;
			}
		}
		if (sqlReturnValue.metaType.IsVarTime && !stateObj.TryReadByte(out sqlReturnValue.scale))
		{
			return false;
		}
		if (value6 == 240 && !TryProcessUDTMetaData(sqlReturnValue, stateObj))
		{
			return false;
		}
		if (sqlReturnValue.type == SqlDbType.Xml)
		{
			if (!stateObj.TryReadByte(out var value7))
			{
				return false;
			}
			if ((value7 & 1) != 0)
			{
				if (!stateObj.TryReadByte(out value2))
				{
					return false;
				}
				if (value2 != 0 && !stateObj.TryReadString(value2, out sqlReturnValue.xmlSchemaCollectionDatabase))
				{
					return false;
				}
				if (!stateObj.TryReadByte(out value2))
				{
					return false;
				}
				if (value2 != 0 && !stateObj.TryReadString(value2, out sqlReturnValue.xmlSchemaCollectionOwningSchema))
				{
					return false;
				}
				if (!stateObj.TryReadInt16(out var value8))
				{
					return false;
				}
				if (value8 != 0 && !stateObj.TryReadString(value8, out sqlReturnValue.xmlSchemaCollectionName))
				{
					return false;
				}
			}
		}
		else if (sqlReturnValue.metaType.IsCharType)
		{
			if (!TryProcessCollation(stateObj, out sqlReturnValue.collation))
			{
				return false;
			}
			int codePage = GetCodePage(sqlReturnValue.collation, stateObj);
			if (codePage == _defaultCodePage)
			{
				sqlReturnValue.codePage = _defaultCodePage;
				sqlReturnValue.encoding = _defaultEncoding;
			}
			else
			{
				sqlReturnValue.codePage = codePage;
				sqlReturnValue.encoding = Encoding.GetEncoding(sqlReturnValue.codePage);
			}
		}
		bool isNull = false;
		if (!TryProcessColumnHeaderNoNBC(sqlReturnValue, stateObj, out isNull, out var length2))
		{
			return false;
		}
		int length3 = (int)((length2 > int.MaxValue) ? int.MaxValue : length2);
		if (sqlReturnValue.metaType.IsPlp)
		{
			length3 = int.MaxValue;
		}
		if (isNull)
		{
			GetNullSqlValue(sqlReturnValue.value, sqlReturnValue);
		}
		else if (!TryReadSqlValue(sqlReturnValue.value, sqlReturnValue, length3, stateObj))
		{
			return false;
		}
		returnValue = sqlReturnValue;
		return true;
	}

	internal bool TryProcessCollation(TdsParserStateObject stateObj, out SqlCollation collation)
	{
		SqlCollation sqlCollation = new SqlCollation();
		if (!stateObj.TryReadUInt32(out sqlCollation.info))
		{
			collation = null;
			return false;
		}
		if (!stateObj.TryReadByte(out sqlCollation.sortId))
		{
			collation = null;
			return false;
		}
		collation = sqlCollation;
		return true;
	}

	private void WriteCollation(SqlCollation collation, TdsParserStateObject stateObj)
	{
		if (collation == null)
		{
			_physicalStateObj.WriteByte(0);
			return;
		}
		_physicalStateObj.WriteByte(5);
		WriteUnsignedInt(collation.info, _physicalStateObj);
		_physicalStateObj.WriteByte(collation.sortId);
	}

	internal int GetCodePage(SqlCollation collation, TdsParserStateObject stateObj)
	{
		int num = 0;
		if (collation.sortId != 0)
		{
			num = TdsEnums.CODE_PAGE_FROM_SORT_ID[collation.sortId];
		}
		else
		{
			int lCID = collation.LCID;
			bool flag = false;
			try
			{
				num = CultureInfo.GetCultureInfo(lCID).TextInfo.ANSICodePage;
				flag = true;
			}
			catch (ArgumentException)
			{
			}
			if (!flag || num == 0)
			{
				switch (lCID)
				{
				case 66564:
				case 66577:
				case 66578:
				case 67588:
				case 68612:
				case 69636:
				case 70660:
					lCID &= 0x3FFF;
					try
					{
						num = new CultureInfo(lCID).TextInfo.ANSICodePage;
						flag = true;
					}
					catch (ArgumentException)
					{
					}
					break;
				case 2087:
					try
					{
						num = new CultureInfo(1063).TextInfo.ANSICodePage;
						flag = true;
					}
					catch (ArgumentException)
					{
					}
					break;
				}
				if (!flag)
				{
					ThrowUnsupportedCollationEncountered(stateObj);
				}
			}
		}
		return num;
	}

	internal void DrainData(TdsParserStateObject stateObj)
	{
		try
		{
			SqlDataReader.SharedState readerState = stateObj._readerState;
			if (readerState != null && readerState._dataReady)
			{
				_SqlMetaDataSet cleanupMetaData = stateObj._cleanupMetaData;
				if (stateObj._partialHeaderBytesRead > 0 && !stateObj.TryProcessHeader())
				{
					throw SQL.SynchronousCallMayNotPend();
				}
				if (readerState._nextColumnHeaderToRead == 0)
				{
					if (!stateObj.Parser.TrySkipRow(stateObj._cleanupMetaData, stateObj))
					{
						throw SQL.SynchronousCallMayNotPend();
					}
				}
				else
				{
					if (readerState._nextColumnDataToRead < readerState._nextColumnHeaderToRead)
					{
						if (readerState._nextColumnHeaderToRead > 0 && cleanupMetaData[readerState._nextColumnHeaderToRead - 1].metaType.IsPlp)
						{
							if (stateObj._longlen != 0L && !TrySkipPlpValue(ulong.MaxValue, stateObj, out var _))
							{
								throw SQL.SynchronousCallMayNotPend();
							}
						}
						else if (0 < readerState._columnDataBytesRemaining && !stateObj.TrySkipLongBytes(readerState._columnDataBytesRemaining))
						{
							throw SQL.SynchronousCallMayNotPend();
						}
					}
					if (!stateObj.Parser.TrySkipRow(cleanupMetaData, readerState._nextColumnHeaderToRead, stateObj))
					{
						throw SQL.SynchronousCallMayNotPend();
					}
				}
			}
			Run(RunBehavior.Clean, null, null, null, stateObj);
		}
		catch
		{
			_connHandler.DoomThisConnection();
			throw;
		}
	}

	internal void ThrowUnsupportedCollationEncountered(TdsParserStateObject stateObj)
	{
		stateObj.AddError(new SqlError(0, 0, 11, _server, SQLMessage.CultureIdError(), "", 0));
		if (stateObj != null)
		{
			DrainData(stateObj);
			stateObj._pendingData = false;
		}
		ThrowExceptionAndWarning(stateObj);
	}

	internal bool TryProcessAltMetaData(int cColumns, TdsParserStateObject stateObj, out _SqlMetaDataSet metaData)
	{
		metaData = null;
		_SqlMetaDataSet sqlMetaDataSet = new _SqlMetaDataSet(cColumns);
		int[] array = new int[cColumns];
		if (!stateObj.TryReadUInt16(out sqlMetaDataSet.id))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		while (value > 0)
		{
			if (!stateObj.TrySkipBytes(2))
			{
				return false;
			}
			value--;
		}
		for (int i = 0; i < cColumns; i++)
		{
			_SqlMetaData col = sqlMetaDataSet[i];
			if (!stateObj.TryReadByte(out var _))
			{
				return false;
			}
			if (!stateObj.TryReadUInt16(out var _))
			{
				return false;
			}
			if (!TryCommonProcessMetaData(stateObj, col))
			{
				return false;
			}
			array[i] = i;
		}
		sqlMetaDataSet.indexMap = array;
		sqlMetaDataSet.visibleColumns = cColumns;
		metaData = sqlMetaDataSet;
		return true;
	}

	internal bool TryProcessMetaData(int cColumns, TdsParserStateObject stateObj, out _SqlMetaDataSet metaData)
	{
		_SqlMetaDataSet sqlMetaDataSet = new _SqlMetaDataSet(cColumns);
		for (int i = 0; i < cColumns; i++)
		{
			if (!TryCommonProcessMetaData(stateObj, sqlMetaDataSet[i]))
			{
				metaData = null;
				return false;
			}
		}
		metaData = sqlMetaDataSet;
		return true;
	}

	private bool IsVarTimeTds(byte tdsType)
	{
		if (tdsType != 41 && tdsType != 42)
		{
			return tdsType == 43;
		}
		return true;
	}

	private bool TryCommonProcessMetaData(TdsParserStateObject stateObj, _SqlMetaData col)
	{
		if (!stateObj.TryReadUInt32(out var value))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		col.updatability = (byte)((value2 & 0xB) >> 2);
		col.isNullable = 1 == (value2 & 1);
		col.isIdentity = 16 == (value2 & 0x10);
		if (!stateObj.TryReadByte(out value2))
		{
			return false;
		}
		col.isColumnSet = 4 == (value2 & 4);
		if (!stateObj.TryReadByte(out var value3))
		{
			return false;
		}
		if (value3 == 241)
		{
			col.length = 65535;
		}
		else if (IsVarTimeTds(value3))
		{
			col.length = 0;
		}
		else if (value3 == 40)
		{
			col.length = 3;
		}
		else if (!TryGetTokenLength(value3, stateObj, out col.length))
		{
			return false;
		}
		col.metaType = MetaType.GetSqlDataType(value3, value, col.length);
		col.type = col.metaType.SqlDbType;
		col.tdsType = (col.isNullable ? col.metaType.NullableType : col.metaType.TDSType);
		if (240 == value3 && !TryProcessUDTMetaData(col, stateObj))
		{
			return false;
		}
		byte value5;
		if (col.length == 65535)
		{
			col.metaType = MetaType.GetMaxMetaTypeFromMetaType(col.metaType);
			col.length = int.MaxValue;
			if (value3 == 241)
			{
				if (!stateObj.TryReadByte(out var value4))
				{
					return false;
				}
				if ((value4 & 1) != 0)
				{
					if (!stateObj.TryReadByte(out value5))
					{
						return false;
					}
					if (value5 != 0 && !stateObj.TryReadString(value5, out col.xmlSchemaCollectionDatabase))
					{
						return false;
					}
					if (!stateObj.TryReadByte(out value5))
					{
						return false;
					}
					if (value5 != 0 && !stateObj.TryReadString(value5, out col.xmlSchemaCollectionOwningSchema))
					{
						return false;
					}
					if (!stateObj.TryReadInt16(out var value6))
					{
						return false;
					}
					if (value5 != 0 && !stateObj.TryReadString(value6, out col.xmlSchemaCollectionName))
					{
						return false;
					}
				}
			}
		}
		if (col.type == SqlDbType.Decimal)
		{
			if (!stateObj.TryReadByte(out col.precision))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out col.scale))
			{
				return false;
			}
		}
		if (col.metaType.IsVarTime)
		{
			if (!stateObj.TryReadByte(out col.scale))
			{
				return false;
			}
			switch (col.metaType.SqlDbType)
			{
			case SqlDbType.Time:
				col.length = MetaType.GetTimeSizeFromScale(col.scale);
				break;
			case SqlDbType.DateTime2:
				col.length = 3 + MetaType.GetTimeSizeFromScale(col.scale);
				break;
			case SqlDbType.DateTimeOffset:
				col.length = 5 + MetaType.GetTimeSizeFromScale(col.scale);
				break;
			}
		}
		if (col.metaType.IsCharType && value3 != 241)
		{
			if (!TryProcessCollation(stateObj, out col.collation))
			{
				return false;
			}
			int codePage = GetCodePage(col.collation, stateObj);
			if (codePage == _defaultCodePage)
			{
				col.codePage = _defaultCodePage;
				col.encoding = _defaultEncoding;
			}
			else
			{
				col.codePage = codePage;
				col.encoding = Encoding.GetEncoding(col.codePage);
			}
		}
		if (col.metaType.IsLong && !col.metaType.IsPlp)
		{
			int length = 65535;
			if (!TryProcessOneTable(stateObj, ref length, out col.multiPartTableName))
			{
				return false;
			}
		}
		if (!stateObj.TryReadByte(out value5))
		{
			return false;
		}
		if (!stateObj.TryReadString(value5, out col.column))
		{
			return false;
		}
		stateObj._receivedColMetaData = true;
		return true;
	}

	private void WriteUDTMetaData(object value, string database, string schema, string type, TdsParserStateObject stateObj)
	{
		if (string.IsNullOrEmpty(database))
		{
			stateObj.WriteByte(0);
		}
		else
		{
			stateObj.WriteByte((byte)database.Length);
			WriteString(database, stateObj);
		}
		if (string.IsNullOrEmpty(schema))
		{
			stateObj.WriteByte(0);
		}
		else
		{
			stateObj.WriteByte((byte)schema.Length);
			WriteString(schema, stateObj);
		}
		if (string.IsNullOrEmpty(type))
		{
			stateObj.WriteByte(0);
			return;
		}
		stateObj.WriteByte((byte)type.Length);
		WriteString(type, stateObj);
	}

	internal bool TryProcessTableName(int length, TdsParserStateObject stateObj, out MultiPartTableName[] multiPartTableNames)
	{
		int num = 0;
		MultiPartTableName[] array = new MultiPartTableName[1];
		while (length > 0)
		{
			if (!TryProcessOneTable(stateObj, ref length, out var multiPartTableName))
			{
				multiPartTableNames = null;
				return false;
			}
			if (num == 0)
			{
				array[num] = multiPartTableName;
			}
			else
			{
				MultiPartTableName[] array2 = new MultiPartTableName[array.Length + 1];
				Array.Copy(array, 0, array2, 0, array.Length);
				array2[array.Length] = multiPartTableName;
				array = array2;
			}
			num++;
		}
		multiPartTableNames = array;
		return true;
	}

	private bool TryProcessOneTable(TdsParserStateObject stateObj, ref int length, out MultiPartTableName multiPartTableName)
	{
		multiPartTableName = default(MultiPartTableName);
		MultiPartTableName multiPartTableName2 = default(MultiPartTableName);
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		length--;
		ushort value2;
		string value3;
		if (value == 4)
		{
			if (!stateObj.TryReadUInt16(out value2))
			{
				return false;
			}
			length -= 2;
			if (!stateObj.TryReadString(value2, out value3))
			{
				return false;
			}
			multiPartTableName2.ServerName = value3;
			value--;
			length -= value2 * 2;
		}
		if (value == 3)
		{
			if (!stateObj.TryReadUInt16(out value2))
			{
				return false;
			}
			length -= 2;
			if (!stateObj.TryReadString(value2, out value3))
			{
				return false;
			}
			multiPartTableName2.CatalogName = value3;
			length -= value2 * 2;
			value--;
		}
		if (value == 2)
		{
			if (!stateObj.TryReadUInt16(out value2))
			{
				return false;
			}
			length -= 2;
			if (!stateObj.TryReadString(value2, out value3))
			{
				return false;
			}
			multiPartTableName2.SchemaName = value3;
			length -= value2 * 2;
			value--;
		}
		if (value == 1)
		{
			if (!stateObj.TryReadUInt16(out value2))
			{
				return false;
			}
			length -= 2;
			if (!stateObj.TryReadString(value2, out value3))
			{
				return false;
			}
			multiPartTableName2.TableName = value3;
			length -= value2 * 2;
			value--;
		}
		multiPartTableName = multiPartTableName2;
		return true;
	}

	private bool TryProcessColInfo(_SqlMetaDataSet columns, SqlDataReader reader, TdsParserStateObject stateObj, out _SqlMetaDataSet metaData)
	{
		metaData = null;
		for (int i = 0; i < columns.Length; i++)
		{
			_SqlMetaData sqlMetaData = columns[i];
			if (!stateObj.TryReadByte(out var _))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out sqlMetaData.tableNum))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out var value2))
			{
				return false;
			}
			sqlMetaData.isDifferentName = 32 == (value2 & 0x20);
			sqlMetaData.isExpression = 4 == (value2 & 4);
			sqlMetaData.isKey = 8 == (value2 & 8);
			sqlMetaData.isHidden = 16 == (value2 & 0x10);
			if (sqlMetaData.isDifferentName)
			{
				if (!stateObj.TryReadByte(out var value3))
				{
					return false;
				}
				if (!stateObj.TryReadString(value3, out sqlMetaData.baseColumn))
				{
					return false;
				}
			}
			if (reader.TableNames != null && sqlMetaData.tableNum > 0)
			{
				sqlMetaData.multiPartTableName = reader.TableNames[sqlMetaData.tableNum - 1];
			}
			if (sqlMetaData.isExpression)
			{
				sqlMetaData.updatability = 0;
			}
		}
		metaData = columns;
		return true;
	}

	internal bool TryProcessColumnHeader(SqlMetaDataPriv col, TdsParserStateObject stateObj, int columnOrdinal, out bool isNull, out ulong length)
	{
		if (stateObj.IsNullCompressionBitSet(columnOrdinal))
		{
			isNull = true;
			length = 0uL;
			return true;
		}
		return TryProcessColumnHeaderNoNBC(col, stateObj, out isNull, out length);
	}

	private bool TryProcessColumnHeaderNoNBC(SqlMetaDataPriv col, TdsParserStateObject stateObj, out bool isNull, out ulong length)
	{
		if (col.metaType.IsLong && !col.metaType.IsPlp)
		{
			if (!stateObj.TryReadByte(out var value))
			{
				isNull = false;
				length = 0uL;
				return false;
			}
			if (value != 0)
			{
				if (!stateObj.TrySkipBytes(value))
				{
					isNull = false;
					length = 0uL;
					return false;
				}
				if (!stateObj.TrySkipBytes(8))
				{
					isNull = false;
					length = 0uL;
					return false;
				}
				isNull = false;
				return TryGetDataLength(col, stateObj, out length);
			}
			isNull = true;
			length = 0uL;
			return true;
		}
		if (!TryGetDataLength(col, stateObj, out var length2))
		{
			isNull = false;
			length = 0uL;
			return false;
		}
		isNull = IsNull(col.metaType, length2);
		length = (isNull ? 0 : length2);
		return true;
	}

	internal bool TryGetAltRowId(TdsParserStateObject stateObj, out int id)
	{
		if (!stateObj.TryReadByte(out var _))
		{
			id = 0;
			return false;
		}
		if (!stateObj.TryStartNewRow(isNullCompressed: false))
		{
			id = 0;
			return false;
		}
		if (!stateObj.TryReadUInt16(out var value2))
		{
			id = 0;
			return false;
		}
		id = value2;
		return true;
	}

	private bool TryProcessRow(_SqlMetaDataSet columns, object[] buffer, int[] map, TdsParserStateObject stateObj)
	{
		SqlBuffer sqlBuffer = new SqlBuffer();
		for (int i = 0; i < columns.Length; i++)
		{
			_SqlMetaData sqlMetaData = columns[i];
			if (!TryProcessColumnHeader(sqlMetaData, stateObj, i, out var isNull, out var length))
			{
				return false;
			}
			if (isNull)
			{
				GetNullSqlValue(sqlBuffer, sqlMetaData);
				buffer[map[i]] = sqlBuffer.SqlValue;
			}
			else
			{
				if (!TryReadSqlValue(sqlBuffer, sqlMetaData, (int)(sqlMetaData.metaType.IsPlp ? int.MaxValue : length), stateObj))
				{
					return false;
				}
				buffer[map[i]] = sqlBuffer.SqlValue;
				if (stateObj._longlen != 0L)
				{
					throw new SqlTruncateException(global::SR.GetString("Data returned is larger than 2Gb in size. Use SequentialAccess command behavior in order to get all of the data."));
				}
			}
			sqlBuffer.Clear();
		}
		return true;
	}

	internal object GetNullSqlValue(SqlBuffer nullVal, SqlMetaDataPriv md)
	{
		switch (md.type)
		{
		case SqlDbType.Real:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Single);
			break;
		case SqlDbType.Float:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Double);
			break;
		case SqlDbType.Binary:
		case SqlDbType.Image:
		case SqlDbType.VarBinary:
		case SqlDbType.Udt:
			nullVal.SqlBinary = SqlBinary.Null;
			break;
		case SqlDbType.UniqueIdentifier:
			nullVal.SqlGuid = SqlGuid.Null;
			break;
		case SqlDbType.Bit:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Boolean);
			break;
		case SqlDbType.TinyInt:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Byte);
			break;
		case SqlDbType.SmallInt:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Int16);
			break;
		case SqlDbType.Int:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Int32);
			break;
		case SqlDbType.BigInt:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Int64);
			break;
		case SqlDbType.Char:
		case SqlDbType.NChar:
		case SqlDbType.NText:
		case SqlDbType.NVarChar:
		case SqlDbType.Text:
		case SqlDbType.VarChar:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.String);
			break;
		case SqlDbType.Decimal:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Decimal);
			break;
		case SqlDbType.DateTime:
		case SqlDbType.SmallDateTime:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.DateTime);
			break;
		case SqlDbType.Money:
		case SqlDbType.SmallMoney:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Money);
			break;
		case SqlDbType.Variant:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Empty);
			break;
		case SqlDbType.Xml:
			nullVal.SqlCachedBuffer = SqlCachedBuffer.Null;
			break;
		case SqlDbType.Date:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Date);
			break;
		case SqlDbType.Time:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Time);
			break;
		case SqlDbType.DateTime2:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.DateTime2);
			break;
		case SqlDbType.DateTimeOffset:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.DateTimeOffset);
			break;
		}
		return nullVal;
	}

	internal bool TrySkipRow(_SqlMetaDataSet columns, TdsParserStateObject stateObj)
	{
		return TrySkipRow(columns, 0, stateObj);
	}

	internal bool TrySkipRow(_SqlMetaDataSet columns, int startCol, TdsParserStateObject stateObj)
	{
		for (int i = startCol; i < columns.Length; i++)
		{
			_SqlMetaData md = columns[i];
			if (!TrySkipValue(md, i, stateObj))
			{
				return false;
			}
		}
		return true;
	}

	internal bool TrySkipValue(SqlMetaDataPriv md, int columnOrdinal, TdsParserStateObject stateObj)
	{
		if (stateObj.IsNullCompressionBitSet(columnOrdinal))
		{
			return true;
		}
		if (md.metaType.IsPlp)
		{
			if (!TrySkipPlpValue(ulong.MaxValue, stateObj, out var _))
			{
				return false;
			}
		}
		else if (md.metaType.IsLong)
		{
			if (!stateObj.TryReadByte(out var value))
			{
				return false;
			}
			if (value != 0)
			{
				if (!stateObj.TrySkipBytes(value + 8))
				{
					return false;
				}
				if (!TryGetTokenLength(md.tdsType, stateObj, out var tokenLength))
				{
					return false;
				}
				if (!stateObj.TrySkipBytes(tokenLength))
				{
					return false;
				}
			}
		}
		else
		{
			if (!TryGetTokenLength(md.tdsType, stateObj, out var tokenLength2))
			{
				return false;
			}
			if (!IsNull(md.metaType, (ulong)tokenLength2) && !stateObj.TrySkipBytes(tokenLength2))
			{
				return false;
			}
		}
		return true;
	}

	private bool IsNull(MetaType mt, ulong length)
	{
		if (mt.IsPlp)
		{
			return ulong.MaxValue == length;
		}
		if (65535 == length && !mt.IsLong)
		{
			return true;
		}
		if (length == 0L && !mt.IsCharType)
		{
			return !mt.IsBinType;
		}
		return false;
	}

	private bool TryReadSqlStringValue(SqlBuffer value, byte type, int length, Encoding encoding, bool isPlp, TdsParserStateObject stateObj)
	{
		switch (type)
		{
		case 35:
		case 39:
		case 47:
		case 167:
		case 175:
		{
			if (encoding == null)
			{
				encoding = _defaultEncoding;
			}
			if (!stateObj.TryReadStringWithEncoding(length, encoding, isPlp, out var value3))
			{
				return false;
			}
			value.SetToString(value3);
			break;
		}
		case 99:
		case 231:
		case 239:
		{
			string value2 = null;
			if (isPlp)
			{
				char[] buff = null;
				if (!TryReadPlpUnicodeChars(ref buff, 0, length >> 1, stateObj, out length))
				{
					return false;
				}
				value2 = ((length <= 0) ? ADP.StrEmpty : new string(buff, 0, length));
			}
			else if (!stateObj.TryReadString(length >> 1, out value2))
			{
				return false;
			}
			value.SetToString(value2);
			break;
		}
		}
		return true;
	}

	internal bool TryReadSqlValue(SqlBuffer value, SqlMetaDataPriv md, int length, TdsParserStateObject stateObj)
	{
		bool isPlp = md.metaType.IsPlp;
		byte tdsType = md.tdsType;
		if (isPlp)
		{
			length = int.MaxValue;
		}
		switch (tdsType)
		{
		case 106:
		case 108:
			if (!TryReadSqlDecimal(value, length, md.precision, md.scale, stateObj))
			{
				return false;
			}
			break;
		case 34:
		case 37:
		case 45:
		case 165:
		case 173:
		case 240:
		{
			byte[] buff = null;
			if (isPlp)
			{
				if (!stateObj.TryReadPlpBytes(ref buff, 0, length, out var _))
				{
					return false;
				}
			}
			else
			{
				buff = new byte[length];
				if (!stateObj.TryReadByteArray(buff, 0, length))
				{
					return false;
				}
			}
			value.SqlBinary = SqlTypeWorkarounds.SqlBinaryCtor(buff, ignored: true);
			break;
		}
		case 35:
		case 39:
		case 47:
		case 99:
		case 167:
		case 175:
		case 231:
		case 239:
			if (!TryReadSqlStringValue(value, tdsType, length, md.encoding, isPlp, stateObj))
			{
				return false;
			}
			break;
		case 241:
		{
			if (!SqlCachedBuffer.TryCreate(md, this, stateObj, out var buffer))
			{
				return false;
			}
			value.SqlCachedBuffer = buffer;
			break;
		}
		case 40:
		case 41:
		case 42:
		case 43:
			if (!TryReadSqlDateTime(value, tdsType, length, md.scale, stateObj))
			{
				return false;
			}
			break;
		default:
			if (!TryReadSqlValueInternal(value, tdsType, length, stateObj))
			{
				return false;
			}
			break;
		}
		return true;
	}

	private bool TryReadSqlDateTime(SqlBuffer value, byte tdsType, int length, byte scale, TdsParserStateObject stateObj)
	{
		byte[] array = new byte[length];
		if (!stateObj.TryReadByteArray(array, 0, length))
		{
			return false;
		}
		switch (tdsType)
		{
		case 40:
			value.SetToDate(array);
			break;
		case 41:
			value.SetToTime(array, length, scale);
			break;
		case 42:
			value.SetToDateTime2(array, length, scale);
			break;
		case 43:
			value.SetToDateTimeOffset(array, length, scale);
			break;
		}
		return true;
	}

	internal bool TryReadSqlValueInternal(SqlBuffer value, byte tdsType, int length, TdsParserStateObject stateObj)
	{
		byte value2;
		int value5;
		switch (tdsType)
		{
		case 50:
		case 104:
			if (!stateObj.TryReadByte(out value2))
			{
				return false;
			}
			value.Boolean = value2 != 0;
			break;
		case 38:
			if (length == 1)
			{
				goto case 48;
			}
			if (length == 2)
			{
				goto case 52;
			}
			if (length == 4)
			{
				goto case 56;
			}
			goto case 127;
		case 48:
			if (!stateObj.TryReadByte(out value2))
			{
				return false;
			}
			value.Byte = value2;
			break;
		case 52:
		{
			if (!stateObj.TryReadInt16(out var value9))
			{
				return false;
			}
			value.Int16 = value9;
			break;
		}
		case 56:
			if (!stateObj.TryReadInt32(out value5))
			{
				return false;
			}
			value.Int32 = value5;
			break;
		case 127:
		{
			if (!stateObj.TryReadInt64(out var value12))
			{
				return false;
			}
			value.Int64 = value12;
			break;
		}
		case 109:
			if (length == 4)
			{
				goto case 59;
			}
			goto case 62;
		case 59:
		{
			if (!stateObj.TryReadSingle(out var value13))
			{
				return false;
			}
			value.Single = value13;
			break;
		}
		case 62:
		{
			if (!stateObj.TryReadDouble(out var value6))
			{
				return false;
			}
			value.Double = value6;
			break;
		}
		case 110:
			if (length != 4)
			{
				goto case 60;
			}
			goto case 122;
		case 60:
		{
			if (!stateObj.TryReadInt32(out var value3))
			{
				return false;
			}
			if (!stateObj.TryReadUInt32(out var value4))
			{
				return false;
			}
			long toMoney = ((long)value3 << 32) + value4;
			value.SetToMoney(toMoney);
			break;
		}
		case 122:
			if (!stateObj.TryReadInt32(out value5))
			{
				return false;
			}
			value.SetToMoney(value5);
			break;
		case 111:
			if (length == 4)
			{
				goto case 58;
			}
			goto case 61;
		case 58:
		{
			if (!stateObj.TryReadUInt16(out var value7))
			{
				return false;
			}
			if (!stateObj.TryReadUInt16(out var value8))
			{
				return false;
			}
			value.SetToDateTime(value7, value8 * SqlDateTime.SQLTicksPerMinute);
			break;
		}
		case 61:
		{
			if (!stateObj.TryReadInt32(out var value10))
			{
				return false;
			}
			if (!stateObj.TryReadUInt32(out var value11))
			{
				return false;
			}
			value.SetToDateTime(value10, (int)value11);
			break;
		}
		case 36:
		{
			byte[] array2 = new byte[length];
			if (!stateObj.TryReadByteArray(array2, 0, length))
			{
				return false;
			}
			value.SqlGuid = SqlTypeWorkarounds.SqlGuidCtor(array2, ignored: true);
			break;
		}
		case 34:
		case 37:
		case 45:
		case 165:
		case 173:
		{
			byte[] array = new byte[length];
			if (!stateObj.TryReadByteArray(array, 0, length))
			{
				return false;
			}
			value.SqlBinary = SqlTypeWorkarounds.SqlBinaryCtor(array, ignored: true);
			break;
		}
		case 98:
			if (!TryReadSqlVariant(value, length, stateObj))
			{
				return false;
			}
			break;
		}
		return true;
	}

	internal bool TryReadSqlVariant(SqlBuffer value, int lenTotal, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		ushort value3 = 0;
		if (!stateObj.TryReadByte(out var value4))
		{
			return false;
		}
		byte propBytes = MetaType.GetSqlDataType(value2, 0u, 0).PropBytes;
		int num = 2 + value4;
		int length = lenTotal - num;
		switch (value2)
		{
		case 36:
		case 48:
		case 50:
		case 52:
		case 56:
		case 58:
		case 59:
		case 60:
		case 61:
		case 62:
		case 122:
		case 127:
			if (!TryReadSqlValueInternal(value, value2, length, stateObj))
			{
				return false;
			}
			break;
		case 106:
		case 108:
		{
			if (!stateObj.TryReadByte(out var value6))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out var value7))
			{
				return false;
			}
			if (value4 > propBytes && !stateObj.TrySkipBytes(value4 - propBytes))
			{
				return false;
			}
			if (!TryReadSqlDecimal(value, 17, value6, value7, stateObj))
			{
				return false;
			}
			break;
		}
		case 165:
		case 173:
			if (!stateObj.TryReadUInt16(out value3))
			{
				return false;
			}
			if (value4 > propBytes && !stateObj.TrySkipBytes(value4 - propBytes))
			{
				return false;
			}
			goto case 36;
		case 167:
		case 175:
		case 231:
		case 239:
		{
			if (!TryProcessCollation(stateObj, out var collation))
			{
				return false;
			}
			if (!stateObj.TryReadUInt16(out value3))
			{
				return false;
			}
			if (value4 > propBytes && !stateObj.TrySkipBytes(value4 - propBytes))
			{
				return false;
			}
			Encoding encoding = Encoding.GetEncoding(GetCodePage(collation, stateObj));
			if (!TryReadSqlStringValue(value, value2, length, encoding, isPlp: false, stateObj))
			{
				return false;
			}
			break;
		}
		case 40:
			if (!TryReadSqlDateTime(value, value2, length, 0, stateObj))
			{
				return false;
			}
			break;
		case 41:
		case 42:
		case 43:
		{
			if (!stateObj.TryReadByte(out var value5))
			{
				return false;
			}
			if (value4 > propBytes && !stateObj.TrySkipBytes(value4 - propBytes))
			{
				return false;
			}
			if (!TryReadSqlDateTime(value, value2, length, value5, stateObj))
			{
				return false;
			}
			break;
		}
		}
		return true;
	}

	internal Task WriteSqlVariantValue(object value, int length, int offset, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		if (ADP.IsNull(value))
		{
			WriteInt(0, stateObj);
			WriteInt(0, stateObj);
			return null;
		}
		MetaType metaTypeFromValue = MetaType.GetMetaTypeFromValue(value);
		if (108 == metaTypeFromValue.TDSType && 8 == length)
		{
			metaTypeFromValue = MetaType.GetMetaTypeFromValue(new SqlMoney((decimal)value));
		}
		if (metaTypeFromValue.IsAnsiType)
		{
			length = GetEncodingCharLength((string)value, length, 0, _defaultEncoding);
		}
		WriteInt(2 + metaTypeFromValue.PropBytes + length, stateObj);
		WriteInt(2 + metaTypeFromValue.PropBytes + length, stateObj);
		stateObj.WriteByte(metaTypeFromValue.TDSType);
		stateObj.WriteByte(metaTypeFromValue.PropBytes);
		switch (metaTypeFromValue.TDSType)
		{
		case 59:
			WriteFloat((float)value, stateObj);
			break;
		case 62:
			WriteDouble((double)value, stateObj);
			break;
		case 127:
			WriteLong((long)value, stateObj);
			break;
		case 56:
			WriteInt((int)value, stateObj);
			break;
		case 52:
			WriteShort((short)value, stateObj);
			break;
		case 48:
			stateObj.WriteByte((byte)value);
			break;
		case 50:
			if ((bool)value)
			{
				stateObj.WriteByte(1);
			}
			else
			{
				stateObj.WriteByte(0);
			}
			break;
		case 165:
		{
			byte[] b2 = (byte[])value;
			WriteShort(length, stateObj);
			return stateObj.WriteByteArray(b2, length, offset, canAccumulate);
		}
		case 167:
		{
			string s2 = (string)value;
			WriteUnsignedInt(_defaultCollation.info, stateObj);
			stateObj.WriteByte(_defaultCollation.sortId);
			WriteShort(length, stateObj);
			return WriteEncodingChar(s2, _defaultEncoding, stateObj, canAccumulate);
		}
		case 36:
		{
			byte[] b = ((Guid)value).ToByteArray();
			stateObj.WriteByteArray(b, length, 0);
			break;
		}
		case 231:
		{
			string s = (string)value;
			WriteUnsignedInt(_defaultCollation.info, stateObj);
			stateObj.WriteByte(_defaultCollation.sortId);
			WriteShort(length, stateObj);
			length >>= 1;
			return WriteString(s, length, offset, stateObj, canAccumulate);
		}
		case 61:
		{
			TdsDateTime tdsDateTime = MetaType.FromDateTime((DateTime)value, 8);
			WriteInt(tdsDateTime.days, stateObj);
			WriteInt(tdsDateTime.time, stateObj);
			break;
		}
		case 60:
			WriteCurrency((decimal)value, 8, stateObj);
			break;
		case 108:
			stateObj.WriteByte(metaTypeFromValue.Precision);
			stateObj.WriteByte((byte)((decimal.GetBits((decimal)value)[3] & 0xFF0000) >> 16));
			WriteDecimal((decimal)value, stateObj);
			break;
		case 41:
			stateObj.WriteByte(metaTypeFromValue.Scale);
			WriteTime((TimeSpan)value, metaTypeFromValue.Scale, length, stateObj);
			break;
		case 43:
			stateObj.WriteByte(metaTypeFromValue.Scale);
			WriteDateTimeOffset((DateTimeOffset)value, metaTypeFromValue.Scale, length, stateObj);
			break;
		}
		return null;
	}

	internal Task WriteSqlVariantDataRowValue(object value, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		if (value == null || DBNull.Value == value)
		{
			WriteInt(0, stateObj);
			return null;
		}
		MetaType metaTypeFromValue = MetaType.GetMetaTypeFromValue(value);
		int numChars = 0;
		if (metaTypeFromValue.IsAnsiType)
		{
			numChars = GetEncodingCharLength((string)value, numChars, 0, _defaultEncoding);
		}
		switch (metaTypeFromValue.TDSType)
		{
		case 59:
			WriteSqlVariantHeader(6, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteFloat((float)value, stateObj);
			break;
		case 62:
			WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteDouble((double)value, stateObj);
			break;
		case 127:
			WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteLong((long)value, stateObj);
			break;
		case 56:
			WriteSqlVariantHeader(6, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteInt((int)value, stateObj);
			break;
		case 52:
			WriteSqlVariantHeader(4, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteShort((short)value, stateObj);
			break;
		case 48:
			WriteSqlVariantHeader(3, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			stateObj.WriteByte((byte)value);
			break;
		case 50:
			WriteSqlVariantHeader(3, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			if ((bool)value)
			{
				stateObj.WriteByte(1);
			}
			else
			{
				stateObj.WriteByte(0);
			}
			break;
		case 165:
		{
			byte[] array2 = (byte[])value;
			numChars = array2.Length;
			WriteSqlVariantHeader(4 + numChars, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteShort(numChars, stateObj);
			return stateObj.WriteByteArray(array2, numChars, 0, canAccumulate);
		}
		case 167:
		{
			string text2 = (string)value;
			numChars = text2.Length;
			WriteSqlVariantHeader(9 + numChars, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteUnsignedInt(_defaultCollation.info, stateObj);
			stateObj.WriteByte(_defaultCollation.sortId);
			WriteShort(numChars, stateObj);
			return WriteEncodingChar(text2, _defaultEncoding, stateObj, canAccumulate);
		}
		case 36:
		{
			byte[] array = ((Guid)value).ToByteArray();
			numChars = array.Length;
			WriteSqlVariantHeader(18, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			stateObj.WriteByteArray(array, numChars, 0);
			break;
		}
		case 231:
		{
			string text = (string)value;
			numChars = text.Length * 2;
			WriteSqlVariantHeader(9 + numChars, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteUnsignedInt(_defaultCollation.info, stateObj);
			stateObj.WriteByte(_defaultCollation.sortId);
			WriteShort(numChars, stateObj);
			numChars >>= 1;
			return WriteString(text, numChars, 0, stateObj, canAccumulate);
		}
		case 61:
		{
			TdsDateTime tdsDateTime = MetaType.FromDateTime((DateTime)value, 8);
			WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteInt(tdsDateTime.days, stateObj);
			WriteInt(tdsDateTime.time, stateObj);
			break;
		}
		case 60:
			WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteCurrency((decimal)value, 8, stateObj);
			break;
		case 108:
			WriteSqlVariantHeader(21, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			stateObj.WriteByte(metaTypeFromValue.Precision);
			stateObj.WriteByte((byte)((decimal.GetBits((decimal)value)[3] & 0xFF0000) >> 16));
			WriteDecimal((decimal)value, stateObj);
			break;
		case 41:
			WriteSqlVariantHeader(8, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			stateObj.WriteByte(metaTypeFromValue.Scale);
			WriteTime((TimeSpan)value, metaTypeFromValue.Scale, 5, stateObj);
			break;
		case 43:
			WriteSqlVariantHeader(13, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			stateObj.WriteByte(metaTypeFromValue.Scale);
			WriteDateTimeOffset((DateTimeOffset)value, metaTypeFromValue.Scale, 10, stateObj);
			break;
		}
		return null;
	}

	internal void WriteSqlVariantHeader(int length, byte tdstype, byte propbytes, TdsParserStateObject stateObj)
	{
		WriteInt(length, stateObj);
		stateObj.WriteByte(tdstype);
		stateObj.WriteByte(propbytes);
	}

	internal void WriteSqlVariantDateTime2(DateTime value, TdsParserStateObject stateObj)
	{
		SmiMetaData defaultDateTime = SmiMetaData.DefaultDateTime2;
		WriteSqlVariantHeader((int)(defaultDateTime.MaxLength + 3), 42, 1, stateObj);
		stateObj.WriteByte(defaultDateTime.Scale);
		WriteDateTime2(value, defaultDateTime.Scale, (int)defaultDateTime.MaxLength, stateObj);
	}

	internal void WriteSqlVariantDate(DateTime value, TdsParserStateObject stateObj)
	{
		SmiMetaData defaultDate = SmiMetaData.DefaultDate;
		WriteSqlVariantHeader((int)(defaultDate.MaxLength + 2), 40, 0, stateObj);
		WriteDate(value, stateObj);
	}

	private void WriteSqlMoney(SqlMoney value, int length, TdsParserStateObject stateObj)
	{
		int[] bits = decimal.GetBits(value.Value);
		bool num = (bits[3] & int.MinValue) != 0;
		long num2 = (long)(((ulong)(uint)bits[1] << 32) | (uint)bits[0]);
		if (num)
		{
			num2 = -num2;
		}
		if (length == 4)
		{
			decimal value2 = value.Value;
			if (value2 < TdsEnums.SQL_SMALL_MONEY_MIN || value2 > TdsEnums.SQL_SMALL_MONEY_MAX)
			{
				throw SQL.MoneyOverflow(value2.ToString(CultureInfo.InvariantCulture));
			}
			WriteInt((int)num2, stateObj);
		}
		else
		{
			WriteInt((int)(num2 >> 32), stateObj);
			WriteInt((int)num2, stateObj);
		}
	}

	private void WriteCurrency(decimal value, int length, TdsParserStateObject stateObj)
	{
		int[] bits = decimal.GetBits(new SqlMoney(value).Value);
		bool num = (bits[3] & int.MinValue) != 0;
		long num2 = (long)(((ulong)(uint)bits[1] << 32) | (uint)bits[0]);
		if (num)
		{
			num2 = -num2;
		}
		if (length == 4)
		{
			if (value < TdsEnums.SQL_SMALL_MONEY_MIN || value > TdsEnums.SQL_SMALL_MONEY_MAX)
			{
				throw SQL.MoneyOverflow(value.ToString(CultureInfo.InvariantCulture));
			}
			WriteInt((int)num2, stateObj);
		}
		else
		{
			WriteInt((int)(num2 >> 32), stateObj);
			WriteInt((int)num2, stateObj);
		}
	}

	private void WriteDate(DateTime value, TdsParserStateObject stateObj)
	{
		long v = value.Subtract(DateTime.MinValue).Days;
		WritePartialLong(v, 3, stateObj);
	}

	private void WriteTime(TimeSpan value, byte scale, int length, TdsParserStateObject stateObj)
	{
		if (0 > value.Ticks || value.Ticks >= 864000000000L)
		{
			throw SQL.TimeOverflow(value.ToString());
		}
		long v = value.Ticks / TdsEnums.TICKS_FROM_SCALE[scale];
		WritePartialLong(v, length, stateObj);
	}

	private void WriteDateTime2(DateTime value, byte scale, int length, TdsParserStateObject stateObj)
	{
		long v = value.TimeOfDay.Ticks / TdsEnums.TICKS_FROM_SCALE[scale];
		WritePartialLong(v, length - 3, stateObj);
		WriteDate(value, stateObj);
	}

	private void WriteDateTimeOffset(DateTimeOffset value, byte scale, int length, TdsParserStateObject stateObj)
	{
		WriteDateTime2(value.UtcDateTime, scale, length - 2, stateObj);
		short num = (short)value.Offset.TotalMinutes;
		stateObj.WriteByte((byte)(num & 0xFF));
		stateObj.WriteByte((byte)((num >> 8) & 0xFF));
	}

	private bool TryReadSqlDecimal(SqlBuffer value, int length, byte precision, byte scale, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		bool positive = 1 == value2;
		length = checked(length - 1);
		if (!TryReadDecimalBits(length, stateObj, out var bits))
		{
			return false;
		}
		value.SetToDecimal(precision, scale, positive, bits);
		return true;
	}

	private bool TryReadDecimalBits(int length, TdsParserStateObject stateObj, out int[] bits)
	{
		bits = stateObj._decimalBits;
		if (bits == null)
		{
			bits = new int[4];
			stateObj._decimalBits = bits;
		}
		else
		{
			for (int i = 0; i < bits.Length; i++)
			{
				bits[i] = 0;
			}
		}
		int num = length >> 2;
		for (int i = 0; i < num; i++)
		{
			if (!stateObj.TryReadInt32(out bits[i]))
			{
				return false;
			}
		}
		return true;
	}

	internal static SqlDecimal AdjustSqlDecimalScale(SqlDecimal d, int newScale)
	{
		if (d.Scale != newScale)
		{
			return SqlDecimal.AdjustScale(d, newScale - d.Scale, fRound: false);
		}
		return d;
	}

	internal static decimal AdjustDecimalScale(decimal value, int newScale)
	{
		int num = (decimal.GetBits(value)[3] & 0xFF0000) >> 16;
		if (newScale != num)
		{
			SqlDecimal n = new SqlDecimal(value);
			return SqlDecimal.AdjustScale(n, newScale - num, fRound: false).Value;
		}
		return value;
	}

	internal void WriteSqlDecimal(SqlDecimal d, TdsParserStateObject stateObj)
	{
		if (d.IsPositive)
		{
			stateObj.WriteByte(1);
		}
		else
		{
			stateObj.WriteByte(0);
		}
		SqlTypeWorkarounds.SqlDecimalExtractData(d, out var data, out var data2, out var data3, out var data4);
		WriteUnsignedInt(data, stateObj);
		WriteUnsignedInt(data2, stateObj);
		WriteUnsignedInt(data3, stateObj);
		WriteUnsignedInt(data4, stateObj);
	}

	private void WriteDecimal(decimal value, TdsParserStateObject stateObj)
	{
		stateObj._decimalBits = decimal.GetBits(value);
		if (2147483648u == (stateObj._decimalBits[3] & 0x80000000u))
		{
			stateObj.WriteByte(0);
		}
		else
		{
			stateObj.WriteByte(1);
		}
		WriteInt(stateObj._decimalBits[0], stateObj);
		WriteInt(stateObj._decimalBits[1], stateObj);
		WriteInt(stateObj._decimalBits[2], stateObj);
		WriteInt(0, stateObj);
	}

	private void WriteIdentifier(string s, TdsParserStateObject stateObj)
	{
		if (s != null)
		{
			stateObj.WriteByte(checked((byte)s.Length));
			WriteString(s, stateObj);
		}
		else
		{
			stateObj.WriteByte(0);
		}
	}

	private void WriteIdentifierWithShortLength(string s, TdsParserStateObject stateObj)
	{
		if (s != null)
		{
			WriteShort(checked((short)s.Length), stateObj);
			WriteString(s, stateObj);
		}
		else
		{
			WriteShort(0, stateObj);
		}
	}

	private Task WriteString(string s, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		return WriteString(s, s.Length, 0, stateObj, canAccumulate);
	}

	internal Task WriteCharArray(char[] carr, int length, int offset, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		int num = 2 * length;
		if (num < stateObj._outBuff.Length - stateObj._outBytesUsed)
		{
			CopyCharsToBytes(carr, offset, stateObj._outBuff, stateObj._outBytesUsed, length);
			stateObj._outBytesUsed += num;
			return null;
		}
		if (stateObj._bTmp == null || stateObj._bTmp.Length < num)
		{
			stateObj._bTmp = new byte[num];
		}
		CopyCharsToBytes(carr, offset, stateObj._bTmp, 0, length);
		return stateObj.WriteByteArray(stateObj._bTmp, num, 0, canAccumulate);
	}

	internal Task WriteString(string s, int length, int offset, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		int num = 2 * length;
		if (num < stateObj._outBuff.Length - stateObj._outBytesUsed)
		{
			CopyStringToBytes(s, offset, stateObj._outBuff, stateObj._outBytesUsed, length);
			stateObj._outBytesUsed += num;
			return null;
		}
		if (stateObj._bTmp == null || stateObj._bTmp.Length < num)
		{
			stateObj._bTmp = new byte[num];
		}
		CopyStringToBytes(s, offset, stateObj._bTmp, 0, length);
		return stateObj.WriteByteArray(stateObj._bTmp, num, 0, canAccumulate);
	}

	private static void CopyCharsToBytes(char[] source, int sourceOffset, byte[] dest, int destOffset, int charLength)
	{
		Buffer.BlockCopy(source, sourceOffset, dest, destOffset, charLength * 2);
	}

	private static void CopyStringToBytes(string source, int sourceOffset, byte[] dest, int destOffset, int charLength)
	{
		Encoding.Unicode.GetBytes(source, sourceOffset, charLength, dest, destOffset);
	}

	private Task WriteEncodingChar(string s, Encoding encoding, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		return WriteEncodingChar(s, s.Length, 0, encoding, stateObj, canAccumulate);
	}

	private Task WriteEncodingChar(string s, int numChars, int offset, Encoding encoding, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		if (encoding == null)
		{
			encoding = _defaultEncoding;
		}
		char[] array = s.ToCharArray(offset, numChars);
		int num = stateObj._outBuff.Length - stateObj._outBytesUsed;
		if (numChars <= num && encoding.GetMaxByteCount(array.Length) <= num)
		{
			int bytes = encoding.GetBytes(array, 0, array.Length, stateObj._outBuff, stateObj._outBytesUsed);
			stateObj._outBytesUsed += bytes;
			return null;
		}
		byte[] bytes2 = encoding.GetBytes(array, 0, numChars);
		return stateObj.WriteByteArray(bytes2, bytes2.Length, 0, canAccumulate);
	}

	internal int GetEncodingCharLength(string value, int numChars, int charOffset, Encoding encoding)
	{
		if (value == null || value == ADP.StrEmpty)
		{
			return 0;
		}
		if (encoding == null)
		{
			if (_defaultEncoding == null)
			{
				ThrowUnsupportedCollationEncountered(null);
			}
			encoding = _defaultEncoding;
		}
		char[] chars = value.ToCharArray(charOffset, numChars);
		return encoding.GetByteCount(chars, 0, numChars);
	}

	internal bool TryGetDataLength(SqlMetaDataPriv colmeta, TdsParserStateObject stateObj, out ulong length)
	{
		if (colmeta.metaType.IsPlp)
		{
			return stateObj.TryReadPlpLength(returnPlpNullIfNull: true, out length);
		}
		if (!TryGetTokenLength(colmeta.tdsType, stateObj, out var tokenLength))
		{
			length = 0uL;
			return false;
		}
		length = (ulong)tokenLength;
		return true;
	}

	internal bool TryGetTokenLength(byte token, TdsParserStateObject stateObj, out int tokenLength)
	{
		switch (token)
		{
		case 174:
			tokenLength = -1;
			return true;
		case 228:
			return stateObj.TryReadInt32(out tokenLength);
		case 240:
			tokenLength = -1;
			return true;
		case 172:
			tokenLength = -1;
			return true;
		case 241:
		{
			if (!stateObj.TryReadUInt16(out var value3))
			{
				tokenLength = 0;
				return false;
			}
			tokenLength = value3;
			return true;
		}
		default:
			switch (token & 0x30)
			{
			case 48:
				tokenLength = (1 << ((token & 0xC) >> 2)) & 0xFF;
				return true;
			case 16:
				tokenLength = 0;
				return true;
			case 0:
			case 32:
			{
				if ((token & 0x80) != 0)
				{
					if (!stateObj.TryReadUInt16(out var value))
					{
						tokenLength = 0;
						return false;
					}
					tokenLength = value;
					return true;
				}
				if ((token & 0xC) == 0)
				{
					if (!stateObj.TryReadInt32(out tokenLength))
					{
						return false;
					}
					return true;
				}
				if (!stateObj.TryReadByte(out var value2))
				{
					tokenLength = 0;
					return false;
				}
				tokenLength = value2;
				return true;
			}
			default:
				tokenLength = 0;
				return true;
			}
		}
	}

	private void ProcessAttention(TdsParserStateObject stateObj)
	{
		if (_state == TdsParserState.Closed || _state == TdsParserState.Broken)
		{
			return;
		}
		stateObj.StoreErrorAndWarningForAttention();
		try
		{
			Run(RunBehavior.Attention, null, null, null, stateObj);
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			_state = TdsParserState.Broken;
			_connHandler.BreakConnection();
			throw;
		}
		stateObj.RestoreErrorAndWarningAfterAttention();
	}

	private static int StateValueLength(int dataLen)
	{
		if (dataLen >= 255)
		{
			return dataLen + 5;
		}
		return dataLen + 1;
	}

	internal int WriteSessionRecoveryFeatureRequest(SessionData reconnectData, bool write)
	{
		int num = 1;
		if (write)
		{
			_physicalStateObj.WriteByte(1);
		}
		if (reconnectData == null)
		{
			if (write)
			{
				WriteInt(0, _physicalStateObj);
			}
			return num + 4;
		}
		int num2 = 0;
		num2 += 1 + 2 * TdsParserStaticMethods.NullAwareStringLength(reconnectData._initialDatabase);
		num2 += 1 + 2 * TdsParserStaticMethods.NullAwareStringLength(reconnectData._initialLanguage);
		num2 += ((reconnectData._initialCollation == null) ? 1 : 6);
		for (int i = 0; i < 256; i++)
		{
			if (reconnectData._initialState[i] != null)
			{
				num2 += 1 + StateValueLength(reconnectData._initialState[i].Length);
			}
		}
		int num3 = 0;
		num3 += 1 + 2 * ((!(reconnectData._initialDatabase == reconnectData._database)) ? TdsParserStaticMethods.NullAwareStringLength(reconnectData._database) : 0);
		num3 += 1 + 2 * ((!(reconnectData._initialLanguage == reconnectData._language)) ? TdsParserStaticMethods.NullAwareStringLength(reconnectData._language) : 0);
		num3 += ((reconnectData._collation == null || SqlCollation.AreSame(reconnectData._collation, reconnectData._initialCollation)) ? 1 : 6);
		bool[] array = new bool[256];
		for (int j = 0; j < 256; j++)
		{
			if (reconnectData._delta[j] == null)
			{
				continue;
			}
			array[j] = true;
			if (reconnectData._initialState[j] != null && reconnectData._initialState[j].Length == reconnectData._delta[j]._dataLength)
			{
				array[j] = false;
				for (int k = 0; k < reconnectData._delta[j]._dataLength; k++)
				{
					if (reconnectData._initialState[j][k] != reconnectData._delta[j]._data[k])
					{
						array[j] = true;
						break;
					}
				}
			}
			if (array[j])
			{
				num3 += 1 + StateValueLength(reconnectData._delta[j]._dataLength);
			}
		}
		if (write)
		{
			WriteInt(8 + num2 + num3, _physicalStateObj);
			WriteInt(num2, _physicalStateObj);
			WriteIdentifier(reconnectData._initialDatabase, _physicalStateObj);
			WriteCollation(reconnectData._initialCollation, _physicalStateObj);
			WriteIdentifier(reconnectData._initialLanguage, _physicalStateObj);
			for (int l = 0; l < 256; l++)
			{
				if (reconnectData._initialState[l] != null)
				{
					_physicalStateObj.WriteByte((byte)l);
					if (reconnectData._initialState[l].Length < 255)
					{
						_physicalStateObj.WriteByte((byte)reconnectData._initialState[l].Length);
					}
					else
					{
						_physicalStateObj.WriteByte(byte.MaxValue);
						WriteInt(reconnectData._initialState[l].Length, _physicalStateObj);
					}
					_physicalStateObj.WriteByteArray(reconnectData._initialState[l], reconnectData._initialState[l].Length, 0);
				}
			}
			WriteInt(num3, _physicalStateObj);
			WriteIdentifier((reconnectData._database != reconnectData._initialDatabase) ? reconnectData._database : null, _physicalStateObj);
			WriteCollation(SqlCollation.AreSame(reconnectData._initialCollation, reconnectData._collation) ? null : reconnectData._collation, _physicalStateObj);
			WriteIdentifier((reconnectData._language != reconnectData._initialLanguage) ? reconnectData._language : null, _physicalStateObj);
			for (int m = 0; m < 256; m++)
			{
				if (array[m])
				{
					_physicalStateObj.WriteByte((byte)m);
					if (reconnectData._delta[m]._dataLength < 255)
					{
						_physicalStateObj.WriteByte((byte)reconnectData._delta[m]._dataLength);
					}
					else
					{
						_physicalStateObj.WriteByte(byte.MaxValue);
						WriteInt(reconnectData._delta[m]._dataLength, _physicalStateObj);
					}
					_physicalStateObj.WriteByteArray(reconnectData._delta[m]._data, reconnectData._delta[m]._dataLength, 0);
				}
			}
		}
		return num + (num2 + num3 + 12);
	}

	internal int WriteFedAuthFeatureRequest(FederatedAuthenticationFeatureExtensionData fedAuthFeatureData, bool write)
	{
		int num = 0;
		if (fedAuthFeatureData.libraryType == TdsEnums.FedAuthLibrary.SecurityToken)
		{
			num = 5 + fedAuthFeatureData.accessToken.Length;
		}
		int result = num + 5;
		if (write)
		{
			_physicalStateObj.WriteByte(2);
			byte b = 0;
			if (fedAuthFeatureData.libraryType == TdsEnums.FedAuthLibrary.SecurityToken)
			{
				b |= 2;
			}
			b |= (byte)(fedAuthFeatureData.fedAuthRequiredPreLoginResponse ? 1 : 0);
			WriteInt(num, _physicalStateObj);
			_physicalStateObj.WriteByte(b);
			if (fedAuthFeatureData.libraryType == TdsEnums.FedAuthLibrary.SecurityToken)
			{
				WriteInt(fedAuthFeatureData.accessToken.Length, _physicalStateObj);
				_physicalStateObj.WriteByteArray(fedAuthFeatureData.accessToken, fedAuthFeatureData.accessToken.Length, 0);
			}
		}
		return result;
	}

	internal int WriteGlobalTransactionsFeatureRequest(bool write)
	{
		if (write)
		{
			_physicalStateObj.WriteByte(5);
			WriteInt(0, _physicalStateObj);
		}
		return 5;
	}

	internal void TdsLogin(SqlLogin rec, TdsEnums.FeatureExtension requestedFeatures, SessionData recoverySessionData, FederatedAuthenticationFeatureExtensionData? fedAuthFeatureExtensionData)
	{
		_physicalStateObj.SetTimeoutSeconds(rec.timeout);
		_connHandler.TimeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.LoginBegin);
		_connHandler.TimeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.ProcessConnectionAuth);
		byte[] array = null;
		byte[] array2 = null;
		bool flag = requestedFeatures != TdsEnums.FeatureExtension.None;
		string text;
		int num;
		if (rec.credential != null)
		{
			text = rec.credential.UserId;
			num = rec.credential.Password.Length * 2;
		}
		else
		{
			text = rec.userName;
			array = TdsParserStaticMethods.ObfuscatePassword(rec.password);
			num = array.Length;
		}
		int num2;
		if (rec.newSecurePassword != null)
		{
			num2 = rec.newSecurePassword.Length * 2;
		}
		else
		{
			array2 = TdsParserStaticMethods.ObfuscatePassword(rec.newPassword);
			num2 = array2.Length;
		}
		_physicalStateObj._outputMessageType = 16;
		int num3 = 94;
		string text2 = "Core .Net SqlClient Data Provider";
		byte[] sendBuff;
		uint sendLength;
		int v;
		checked
		{
			num3 += (rec.hostName.Length + rec.applicationName.Length + rec.serverName.Length + text2.Length + rec.language.Length + rec.database.Length + rec.attachDBFilename.Length) * 2;
			if (flag)
			{
				num3 += 4;
			}
			sendBuff = null;
			sendLength = 0u;
			if (!rec.useSSPI && !_connHandler._federatedAuthenticationRequested)
			{
				num3 += text.Length * 2 + num + num2;
			}
			else if (rec.useSSPI)
			{
				sendBuff = new byte[s_maxSSPILength];
				sendLength = s_maxSSPILength;
				_physicalStateObj.SniContext = SniContext.Snix_LoginSspi;
				SSPIData(null, 0u, ref sendBuff, ref sendLength);
				if (sendLength > int.MaxValue)
				{
					throw SQL.InvalidSSPIPacketSize();
				}
				_physicalStateObj.SniContext = SniContext.Snix_Login;
				num3 += (int)sendLength;
			}
			v = num3;
		}
		if (flag)
		{
			if ((requestedFeatures & TdsEnums.FeatureExtension.SessionRecovery) != TdsEnums.FeatureExtension.None)
			{
				num3 += WriteSessionRecoveryFeatureRequest(recoverySessionData, write: false);
			}
			if ((requestedFeatures & TdsEnums.FeatureExtension.GlobalTransactions) != TdsEnums.FeatureExtension.None)
			{
				num3 += WriteGlobalTransactionsFeatureRequest(write: false);
			}
			if ((requestedFeatures & TdsEnums.FeatureExtension.FedAuth) != TdsEnums.FeatureExtension.None)
			{
				num3 += WriteFedAuthFeatureRequest(fedAuthFeatureExtensionData.Value, write: false);
			}
			num3++;
		}
		try
		{
			WriteInt(num3, _physicalStateObj);
			if (recoverySessionData == null)
			{
				WriteInt(1946157060, _physicalStateObj);
			}
			else
			{
				WriteUnsignedInt(recoverySessionData._tdsVersion, _physicalStateObj);
			}
			WriteInt(rec.packetSize, _physicalStateObj);
			WriteInt(100663296, _physicalStateObj);
			WriteInt(TdsParserStaticMethods.GetCurrentProcessIdForTdsLoginOnly(), _physicalStateObj);
			WriteInt(0, _physicalStateObj);
			int num4 = 0;
			num4 |= 0x20;
			num4 |= 0x40;
			num4 |= 0x80;
			num4 |= 0x100;
			num4 |= 0x200;
			if (rec.useReplication)
			{
				num4 |= 0x3000;
			}
			if (rec.useSSPI)
			{
				num4 |= 0x8000;
			}
			if (rec.readOnlyIntent)
			{
				num4 |= 0x200000;
			}
			if (!string.IsNullOrEmpty(rec.newPassword) || (rec.newSecurePassword != null && rec.newSecurePassword.Length != 0))
			{
				num4 |= 0x1000000;
			}
			if (rec.userInstance)
			{
				num4 |= 0x4000000;
			}
			if (flag)
			{
				num4 |= 0x10000000;
			}
			WriteInt(num4, _physicalStateObj);
			WriteInt(0, _physicalStateObj);
			WriteInt(0, _physicalStateObj);
			int num5 = 94;
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.hostName.Length, _physicalStateObj);
			num5 += rec.hostName.Length * 2;
			if (!rec.useSSPI)
			{
				WriteShort(num5, _physicalStateObj);
				WriteShort(text.Length, _physicalStateObj);
				num5 += text.Length * 2;
				WriteShort(num5, _physicalStateObj);
				WriteShort(num / 2, _physicalStateObj);
				num5 += num;
			}
			else
			{
				WriteShort(0, _physicalStateObj);
				WriteShort(0, _physicalStateObj);
				WriteShort(0, _physicalStateObj);
				WriteShort(0, _physicalStateObj);
			}
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.applicationName.Length, _physicalStateObj);
			num5 += rec.applicationName.Length * 2;
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.serverName.Length, _physicalStateObj);
			num5 += rec.serverName.Length * 2;
			WriteShort(num5, _physicalStateObj);
			if (flag)
			{
				WriteShort(4, _physicalStateObj);
				num5 += 4;
			}
			else
			{
				WriteShort(0, _physicalStateObj);
			}
			WriteShort(num5, _physicalStateObj);
			WriteShort(text2.Length, _physicalStateObj);
			num5 += text2.Length * 2;
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.language.Length, _physicalStateObj);
			num5 += rec.language.Length * 2;
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.database.Length, _physicalStateObj);
			num5 += rec.database.Length * 2;
			if (s_nicAddress == null)
			{
				s_nicAddress = TdsParserStaticMethods.GetNetworkPhysicalAddressForTdsLoginOnly();
			}
			_physicalStateObj.WriteByteArray(s_nicAddress, s_nicAddress.Length, 0);
			WriteShort(num5, _physicalStateObj);
			if (rec.useSSPI)
			{
				WriteShort((int)sendLength, _physicalStateObj);
				num5 += (int)sendLength;
			}
			else
			{
				WriteShort(0, _physicalStateObj);
			}
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.attachDBFilename.Length, _physicalStateObj);
			num5 += rec.attachDBFilename.Length * 2;
			WriteShort(num5, _physicalStateObj);
			WriteShort(num2 / 2, _physicalStateObj);
			WriteInt(0, _physicalStateObj);
			WriteString(rec.hostName, _physicalStateObj);
			if (!rec.useSSPI)
			{
				WriteString(text, _physicalStateObj);
				if (rec.credential != null)
				{
					_physicalStateObj.WriteSecureString(rec.credential.Password);
				}
				else
				{
					_physicalStateObj.WriteByteArray(array, num, 0);
				}
			}
			WriteString(rec.applicationName, _physicalStateObj);
			WriteString(rec.serverName, _physicalStateObj);
			if (flag)
			{
				WriteInt(v, _physicalStateObj);
			}
			WriteString(text2, _physicalStateObj);
			WriteString(rec.language, _physicalStateObj);
			WriteString(rec.database, _physicalStateObj);
			if (rec.useSSPI)
			{
				_physicalStateObj.WriteByteArray(sendBuff, (int)sendLength, 0);
			}
			WriteString(rec.attachDBFilename, _physicalStateObj);
			if (!rec.useSSPI)
			{
				if (rec.newSecurePassword != null)
				{
					_physicalStateObj.WriteSecureString(rec.newSecurePassword);
				}
				else
				{
					_physicalStateObj.WriteByteArray(array2, num2, 0);
				}
			}
			if (flag)
			{
				if ((requestedFeatures & TdsEnums.FeatureExtension.SessionRecovery) != TdsEnums.FeatureExtension.None)
				{
					num3 += WriteSessionRecoveryFeatureRequest(recoverySessionData, write: true);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.GlobalTransactions) != TdsEnums.FeatureExtension.None)
				{
					WriteGlobalTransactionsFeatureRequest(write: true);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.FedAuth) != TdsEnums.FeatureExtension.None)
				{
					WriteFedAuthFeatureRequest(fedAuthFeatureExtensionData.Value, write: true);
				}
				_physicalStateObj.WriteByte(byte.MaxValue);
			}
		}
		catch (Exception e)
		{
			if (ADP.IsCatchableExceptionType(e))
			{
				_physicalStateObj._outputPacketNumber = 1;
				_physicalStateObj.ResetBuffer();
			}
			throw;
		}
		_physicalStateObj.WritePacket(1);
		_physicalStateObj.ResetSecurePasswordsInformation();
		_physicalStateObj._pendingData = true;
		_physicalStateObj._messageStatus = 0;
	}

	private void SSPIData(byte[] receivedBuff, uint receivedLength, ref byte[] sendBuff, ref uint sendLength)
	{
		SNISSPIData(receivedBuff, receivedLength, ref sendBuff, ref sendLength);
	}

	private void SNISSPIData(byte[] receivedBuff, uint receivedLength, ref byte[] sendBuff, ref uint sendLength)
	{
		if (TdsParserStateObjectFactory.UseManagedSNI)
		{
			try
			{
				_physicalStateObj.GenerateSspiClientContext(receivedBuff, receivedLength, ref sendBuff, ref sendLength, _sniSpnBuffer);
				return;
			}
			catch (Exception ex)
			{
				SSPIError(ex.Message + Environment.NewLine + ex.StackTrace, "GenClientContext");
				return;
			}
		}
		if (receivedBuff == null)
		{
			receivedLength = 0u;
		}
		if (_physicalStateObj.GenerateSspiClientContext(receivedBuff, receivedLength, ref sendBuff, ref sendLength, _sniSpnBuffer) != 0)
		{
			SSPIError(SQLMessage.SSPIGenerateError(), "GenClientContext");
		}
	}

	private void ProcessSSPI(int receivedLength)
	{
		SniContext sniContext = _physicalStateObj.SniContext;
		_physicalStateObj.SniContext = SniContext.Snix_ProcessSspi;
		byte[] array = new byte[receivedLength];
		if (!_physicalStateObj.TryReadByteArray(array, 0, receivedLength))
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		byte[] sendBuff = new byte[s_maxSSPILength];
		uint sendLength = s_maxSSPILength;
		SSPIData(array, (uint)receivedLength, ref sendBuff, ref sendLength);
		_physicalStateObj.WriteByteArray(sendBuff, (int)sendLength, 0);
		_physicalStateObj._outputMessageType = 17;
		_physicalStateObj.WritePacket(1);
		_physicalStateObj.SniContext = sniContext;
	}

	private void SSPIError(string error, string procedure)
	{
		_physicalStateObj.AddError(new SqlError(0, 0, 11, _server, error, procedure, 0));
		ThrowExceptionAndWarning(_physicalStateObj);
	}

	internal byte[] GetDTCAddress(int timeout, TdsParserStateObject stateObj)
	{
		byte[] array = null;
		using (SqlDataReader sqlDataReader = TdsExecuteTransactionManagerRequest(null, TdsEnums.TransactionManagerRequestType.GetDTCAddress, null, TdsEnums.TransactionManagerIsolationLevel.Unspecified, timeout, null, stateObj, isDelegateControlRequest: true))
		{
			if (sqlDataReader != null && sqlDataReader.Read())
			{
				long bytes = sqlDataReader.GetBytes(0, 0L, null, 0, 0);
				if (bytes <= int.MaxValue)
				{
					int num = (int)bytes;
					array = new byte[num];
					sqlDataReader.GetBytes(0, 0L, array, 0, num);
				}
			}
		}
		return array;
	}

	internal void PropagateDistributedTransaction(byte[] buffer, int timeout, TdsParserStateObject stateObj)
	{
		TdsExecuteTransactionManagerRequest(buffer, TdsEnums.TransactionManagerRequestType.Propagate, null, TdsEnums.TransactionManagerIsolationLevel.Unspecified, timeout, null, stateObj, isDelegateControlRequest: true);
	}

	internal SqlDataReader TdsExecuteTransactionManagerRequest(byte[] buffer, TdsEnums.TransactionManagerRequestType request, string transactionName, TdsEnums.TransactionManagerIsolationLevel isoLevel, int timeout, SqlInternalTransaction transaction, TdsParserStateObject stateObj, bool isDelegateControlRequest)
	{
		if (TdsParserState.Broken == State || State == TdsParserState.Closed)
		{
			return null;
		}
		bool threadHasParserLockForClose = _connHandler.ThreadHasParserLockForClose;
		if (!threadHasParserLockForClose)
		{
			_connHandler._parserLock.Wait(canReleaseFromAnyThread: false);
			_connHandler.ThreadHasParserLockForClose = true;
		}
		bool asyncWrite = _asyncWrite;
		try
		{
			_asyncWrite = false;
			if (!isDelegateControlRequest)
			{
				_connHandler.CheckEnlistedTransactionBinding();
			}
			stateObj._outputMessageType = 14;
			stateObj.SetTimeoutSeconds(timeout);
			stateObj.SniContext = SniContext.Snix_Execute;
			WriteInt(22, stateObj);
			WriteInt(18, stateObj);
			WriteMarsHeaderData(stateObj, _currentTransaction);
			WriteShort((short)request, stateObj);
			bool flag = false;
			switch (request)
			{
			case TdsEnums.TransactionManagerRequestType.GetDTCAddress:
				WriteShort(0, stateObj);
				flag = true;
				break;
			case TdsEnums.TransactionManagerRequestType.Propagate:
				if (buffer != null)
				{
					WriteShort(buffer.Length, stateObj);
					stateObj.WriteByteArray(buffer, buffer.Length, 0);
				}
				else
				{
					WriteShort(0, stateObj);
				}
				break;
			case TdsEnums.TransactionManagerRequestType.Begin:
				if (_currentTransaction != transaction)
				{
					PendingTransaction = transaction;
				}
				stateObj.WriteByte((byte)isoLevel);
				stateObj.WriteByte((byte)(transactionName.Length * 2));
				WriteString(transactionName, stateObj);
				break;
			case TdsEnums.TransactionManagerRequestType.Commit:
				stateObj.WriteByte(0);
				stateObj.WriteByte(0);
				break;
			case TdsEnums.TransactionManagerRequestType.Rollback:
				stateObj.WriteByte((byte)(transactionName.Length * 2));
				WriteString(transactionName, stateObj);
				stateObj.WriteByte(0);
				break;
			case TdsEnums.TransactionManagerRequestType.Save:
				stateObj.WriteByte((byte)(transactionName.Length * 2));
				WriteString(transactionName, stateObj);
				break;
			}
			stateObj.WritePacket(1);
			stateObj._pendingData = true;
			stateObj._messageStatus = 0;
			SqlDataReader sqlDataReader = null;
			stateObj.SniContext = SniContext.Snix_Read;
			if (flag)
			{
				sqlDataReader = new SqlDataReader(null, CommandBehavior.Default);
				sqlDataReader.Bind(stateObj);
				_ = sqlDataReader.MetaData;
			}
			else
			{
				Run(RunBehavior.UntilDone, null, null, null, stateObj);
			}
			if ((request == TdsEnums.TransactionManagerRequestType.Begin || request == TdsEnums.TransactionManagerRequestType.Propagate) && (transaction == null || transaction.TransactionId != _retainedTransactionId))
			{
				_retainedTransactionId = 0L;
			}
			return sqlDataReader;
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			FailureCleanup(stateObj, e);
			throw;
		}
		finally
		{
			_pendingTransaction = null;
			_asyncWrite = asyncWrite;
			if (!threadHasParserLockForClose)
			{
				_connHandler.ThreadHasParserLockForClose = false;
				_connHandler._parserLock.Release();
			}
		}
	}

	internal void FailureCleanup(TdsParserStateObject stateObj, Exception e)
	{
		byte outputPacketNumber = stateObj._outputPacketNumber;
		if (stateObj.HasOpenResult)
		{
			stateObj.DecrementOpenResultCount();
		}
		stateObj.ResetBuffer();
		stateObj._outputPacketNumber = 1;
		if (outputPacketNumber != 1 && _state == TdsParserState.OpenLoggedIn)
		{
			bool threadHasParserLockForClose = _connHandler.ThreadHasParserLockForClose;
			try
			{
				_connHandler.ThreadHasParserLockForClose = true;
				stateObj.SendAttention();
				ProcessAttention(stateObj);
			}
			finally
			{
				_connHandler.ThreadHasParserLockForClose = threadHasParserLockForClose;
			}
		}
	}

	internal Task TdsExecuteSQLBatch(string text, int timeout, SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj, bool sync, bool callerHasConnectionLock = false)
	{
		if (TdsParserState.Broken == State || State == TdsParserState.Closed)
		{
			return null;
		}
		if (stateObj.BcpLock)
		{
			throw SQL.ConnectionLockedForBcpEvent();
		}
		bool num = !callerHasConnectionLock && !_connHandler.ThreadHasParserLockForClose;
		bool flag = false;
		if (num)
		{
			_connHandler._parserLock.Wait(!sync);
			flag = true;
		}
		_asyncWrite = !sync;
		try
		{
			if (_state == TdsParserState.Closed || _state == TdsParserState.Broken)
			{
				throw ADP.ClosedConnectionError();
			}
			_connHandler.CheckEnlistedTransactionBinding();
			stateObj.SetTimeoutSeconds(timeout);
			stateObj.SniContext = SniContext.Snix_Execute;
			WriteRPCBatchHeaders(stateObj, notificationRequest);
			stateObj._outputMessageType = 1;
			WriteString(text, text.Length, 0, stateObj);
			Task task = stateObj.ExecuteFlush();
			if (task == null)
			{
				stateObj.SniContext = SniContext.Snix_Read;
				return null;
			}
			bool taskReleaseConnectionLock = flag;
			flag = false;
			return task.ContinueWith(delegate(Task t)
			{
				try
				{
					if (t.IsFaulted)
					{
						FailureCleanup(stateObj, t.Exception.InnerException);
						throw t.Exception.InnerException;
					}
					stateObj.SniContext = SniContext.Snix_Read;
				}
				finally
				{
					if (taskReleaseConnectionLock)
					{
						_connHandler._parserLock.Release();
					}
				}
			}, TaskScheduler.Default);
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			FailureCleanup(stateObj, e);
			throw;
		}
		finally
		{
			if (flag)
			{
				_connHandler._parserLock.Release();
			}
		}
	}

	internal Task TdsExecuteRPC(_SqlRPC[] rpcArray, int timeout, bool inSchema, SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj, bool isCommandProc, bool sync = true, TaskCompletionSource<object> completion = null, int startRpc = 0, int startParam = 0)
	{
		bool flag = completion == null;
		bool flag2 = false;
		try
		{
			_SqlRPC sqlRPC = null;
			if (flag)
			{
				_connHandler._parserLock.Wait(!sync);
				flag2 = true;
			}
			try
			{
				if (TdsParserState.Broken == State || State == TdsParserState.Closed)
				{
					throw ADP.ClosedConnectionError();
				}
				if (flag)
				{
					_asyncWrite = !sync;
					_connHandler.CheckEnlistedTransactionBinding();
					stateObj.SetTimeoutSeconds(timeout);
					stateObj.SniContext = SniContext.Snix_Execute;
					if (_isYukon)
					{
						WriteRPCBatchHeaders(stateObj, notificationRequest);
					}
					stateObj._outputMessageType = 3;
				}
				for (int ii = startRpc; ii < rpcArray.Length; ii++)
				{
					sqlRPC = rpcArray[ii];
					if (startParam == 0 || ii > startRpc)
					{
						if (sqlRPC.ProcID != 0)
						{
							WriteShort(65535, stateObj);
							WriteShort((short)sqlRPC.ProcID, stateObj);
						}
						else
						{
							int length = sqlRPC.rpcName.Length;
							WriteShort(length, stateObj);
							WriteString(sqlRPC.rpcName, length, 0, stateObj);
						}
						WriteShort((short)sqlRPC.options, stateObj);
					}
					SqlParameter[] parameters = sqlRPC.parameters;
					int i;
					for (i = ((ii == startRpc) ? startParam : 0); i < parameters.Length; i++)
					{
						SqlParameter sqlParameter = parameters[i];
						if (sqlParameter == null)
						{
							break;
						}
						sqlParameter.Validate(i, isCommandProc);
						MetaType internalMetaType = sqlParameter.InternalMetaType;
						if (internalMetaType.IsNewKatmaiType)
						{
							WriteSmiParameter(sqlParameter, i, (sqlRPC.paramoptions[i] & 2) != 0, stateObj);
							continue;
						}
						if ((!_isYukon && !internalMetaType.Is80Supported) || (!_isKatmai && !internalMetaType.Is90Supported))
						{
							throw ADP.VersionDoesNotSupportDataType(internalMetaType.TypeName);
						}
						object obj = null;
						bool flag3 = true;
						bool flag4 = false;
						bool flag5 = false;
						if (sqlParameter.Direction == ParameterDirection.Output)
						{
							flag4 = sqlParameter.ParameterIsSqlType;
							sqlParameter.Value = null;
							sqlParameter.ParameterIsSqlType = flag4;
						}
						else
						{
							obj = sqlParameter.GetCoercedValue();
							flag3 = sqlParameter.IsNull;
							if (!flag3)
							{
								flag4 = sqlParameter.CoercedValueIsSqlType;
								flag5 = sqlParameter.CoercedValueIsDataFeed;
							}
						}
						WriteParameterName(sqlParameter.ParameterNameFixed, stateObj);
						stateObj.WriteByte(sqlRPC.paramoptions[i]);
						int num = (internalMetaType.IsSizeInCharacters ? (sqlParameter.GetParameterSize() * 2) : sqlParameter.GetParameterSize());
						int num2 = ((internalMetaType.TDSType != 240) ? sqlParameter.GetActualSize() : 0);
						byte b = 0;
						byte b2 = 0;
						if (internalMetaType.SqlDbType == SqlDbType.Decimal)
						{
							b = sqlParameter.GetActualPrecision();
							b2 = sqlParameter.GetActualScale();
							if (b > 38)
							{
								throw SQL.PrecisionValueOutOfRange(b);
							}
							if (!flag3)
							{
								if (flag4)
								{
									obj = AdjustSqlDecimalScale((SqlDecimal)obj, b2);
									if (b != 0 && b < ((SqlDecimal)obj).Precision)
									{
										throw ADP.ParameterValueOutOfRange((SqlDecimal)obj);
									}
								}
								else
								{
									obj = AdjustDecimalScale((decimal)obj, b2);
									SqlDecimal sqlDecimal = new SqlDecimal((decimal)obj);
									if (b != 0 && b < sqlDecimal.Precision)
									{
										throw ADP.ParameterValueOutOfRange((decimal)obj);
									}
								}
							}
						}
						stateObj.WriteByte(internalMetaType.NullableType);
						if (internalMetaType.TDSType == 98)
						{
							WriteSqlVariantValue(flag4 ? MetaType.GetComValueFromSqlVariant(obj) : obj, sqlParameter.GetActualSize(), sqlParameter.Offset, stateObj);
							continue;
						}
						int num3 = 0;
						int maxSize = 0;
						if (internalMetaType.IsAnsiType)
						{
							if (!flag3 && !flag5)
							{
								string value = ((!flag4) ? ((string)obj) : ((!(obj is SqlString sqlString)) ? new string(((SqlChars)obj).Value) : sqlString.Value));
								num3 = GetEncodingCharLength(value, num2, sqlParameter.Offset, _defaultEncoding);
							}
							if (internalMetaType.IsPlp)
							{
								WriteShort(65535, stateObj);
							}
							else
							{
								maxSize = ((num > num3) ? num : num3);
								if (maxSize == 0)
								{
									maxSize = ((!internalMetaType.IsNCharType) ? 1 : 2);
								}
								WriteParameterVarLen(internalMetaType, maxSize, isNull: false, stateObj);
							}
						}
						else if (internalMetaType.SqlDbType == SqlDbType.Timestamp)
						{
							WriteParameterVarLen(internalMetaType, 8, isNull: false, stateObj);
						}
						else
						{
							if (internalMetaType.SqlDbType == SqlDbType.Udt)
							{
								byte[] array = null;
								Format format = Format.Native;
								if (!flag3)
								{
									array = _connHandler.Connection.GetBytes(obj, out format, out maxSize);
									num = array.Length;
									if (num < 0 || (num >= 65535 && maxSize != -1))
									{
										throw new IndexOutOfRangeException();
									}
								}
								BitConverter.GetBytes((long)num);
								if (string.IsNullOrEmpty(sqlParameter.UdtTypeName))
								{
									throw SQL.MustSetUdtTypeNameForUdtParams();
								}
								string[] array2 = SqlParameter.ParseTypeName(sqlParameter.UdtTypeName, isUdtTypeName: true);
								if (!string.IsNullOrEmpty(array2[0]) && 255 < array2[0].Length)
								{
									throw ADP.ArgumentOutOfRange("names");
								}
								if (!string.IsNullOrEmpty(array2[1]) && 255 < array2[^2].Length)
								{
									throw ADP.ArgumentOutOfRange("names");
								}
								if (255 < array2[2].Length)
								{
									throw ADP.ArgumentOutOfRange("names");
								}
								WriteUDTMetaData(obj, array2[0], array2[1], array2[2], stateObj);
								if (!flag3)
								{
									WriteUnsignedLong((ulong)array.Length, stateObj);
									if (array.Length != 0)
									{
										WriteInt(array.Length, stateObj);
										stateObj.WriteByteArray(array, array.Length, 0);
									}
									WriteInt(0, stateObj);
								}
								else
								{
									WriteUnsignedLong(ulong.MaxValue, stateObj);
								}
								continue;
							}
							if (internalMetaType.IsPlp)
							{
								if (internalMetaType.SqlDbType != SqlDbType.Xml)
								{
									WriteShort(65535, stateObj);
								}
							}
							else if (!internalMetaType.IsVarTime && internalMetaType.SqlDbType != SqlDbType.Date)
							{
								maxSize = ((num > num2) ? num : num2);
								if (maxSize == 0 && _isYukon)
								{
									maxSize = ((!internalMetaType.IsNCharType) ? 1 : 2);
								}
								WriteParameterVarLen(internalMetaType, maxSize, isNull: false, stateObj);
							}
						}
						if (internalMetaType.SqlDbType == SqlDbType.Decimal)
						{
							if (b == 0)
							{
								stateObj.WriteByte(29);
							}
							else
							{
								stateObj.WriteByte(b);
							}
							stateObj.WriteByte(b2);
						}
						else if (internalMetaType.IsVarTime)
						{
							stateObj.WriteByte(sqlParameter.GetActualScale());
						}
						if (_isYukon && internalMetaType.SqlDbType == SqlDbType.Xml)
						{
							if ((sqlParameter.XmlSchemaCollectionDatabase != null && sqlParameter.XmlSchemaCollectionDatabase != ADP.StrEmpty) || (sqlParameter.XmlSchemaCollectionOwningSchema != null && sqlParameter.XmlSchemaCollectionOwningSchema != ADP.StrEmpty) || (sqlParameter.XmlSchemaCollectionName != null && sqlParameter.XmlSchemaCollectionName != ADP.StrEmpty))
							{
								stateObj.WriteByte(1);
								if (sqlParameter.XmlSchemaCollectionDatabase != null && sqlParameter.XmlSchemaCollectionDatabase != ADP.StrEmpty)
								{
									int length = sqlParameter.XmlSchemaCollectionDatabase.Length;
									stateObj.WriteByte((byte)length);
									WriteString(sqlParameter.XmlSchemaCollectionDatabase, length, 0, stateObj);
								}
								else
								{
									stateObj.WriteByte(0);
								}
								if (sqlParameter.XmlSchemaCollectionOwningSchema != null && sqlParameter.XmlSchemaCollectionOwningSchema != ADP.StrEmpty)
								{
									int length = sqlParameter.XmlSchemaCollectionOwningSchema.Length;
									stateObj.WriteByte((byte)length);
									WriteString(sqlParameter.XmlSchemaCollectionOwningSchema, length, 0, stateObj);
								}
								else
								{
									stateObj.WriteByte(0);
								}
								if (sqlParameter.XmlSchemaCollectionName != null && sqlParameter.XmlSchemaCollectionName != ADP.StrEmpty)
								{
									int length = sqlParameter.XmlSchemaCollectionName.Length;
									WriteShort((short)length, stateObj);
									WriteString(sqlParameter.XmlSchemaCollectionName, length, 0, stateObj);
								}
								else
								{
									WriteShort(0, stateObj);
								}
							}
							else
							{
								stateObj.WriteByte(0);
							}
						}
						else if (internalMetaType.IsCharType)
						{
							SqlCollation sqlCollation = ((sqlParameter.Collation != null) ? sqlParameter.Collation : _defaultCollation);
							WriteUnsignedInt(sqlCollation.info, stateObj);
							stateObj.WriteByte(sqlCollation.sortId);
						}
						if (num3 == 0)
						{
							WriteParameterVarLen(internalMetaType, num2, flag3, stateObj, flag5);
						}
						else
						{
							WriteParameterVarLen(internalMetaType, num3, flag3, stateObj, flag5);
						}
						Task task = null;
						if (!flag3)
						{
							task = ((!flag4) ? WriteValue(obj, internalMetaType, sqlParameter.GetActualScale(), num2, num3, sqlParameter.Offset, stateObj, sqlParameter.Size, flag5) : WriteSqlValue(obj, internalMetaType, num2, num3, sqlParameter.Offset, stateObj));
						}
						if (sync)
						{
							continue;
						}
						if (task == null)
						{
							task = stateObj.WaitForAccumulatedWrites();
						}
						if (task == null)
						{
							continue;
						}
						Task task2 = null;
						if (completion == null)
						{
							completion = new TaskCompletionSource<object>();
							task2 = completion.Task;
						}
						AsyncHelper.ContinueTask(task, completion, delegate
						{
							TdsExecuteRPC(rpcArray, timeout, inSchema, notificationRequest, stateObj, isCommandProc, sync, completion, ii, i + 1);
						}, _connHandler, delegate(Exception exc)
						{
							TdsExecuteRPC_OnFailure(exc, stateObj);
						});
						if (flag2)
						{
							task2.ContinueWith(delegate
							{
								_connHandler._parserLock.Release();
							}, TaskScheduler.Default);
							flag2 = false;
						}
						return task2;
					}
					if (ii < rpcArray.Length - 1)
					{
						if (_isYukon)
						{
							stateObj.WriteByte(byte.MaxValue);
						}
						else
						{
							stateObj.WriteByte(128);
						}
					}
				}
				Task task3 = stateObj.ExecuteFlush();
				if (task3 != null)
				{
					Task result = null;
					if (completion == null)
					{
						completion = new TaskCompletionSource<object>();
						result = completion.Task;
					}
					bool taskReleaseConnectionLock = flag2;
					task3.ContinueWith(delegate(Task tsk)
					{
						ExecuteFlushTaskCallback(tsk, stateObj, completion, taskReleaseConnectionLock);
					}, TaskScheduler.Default);
					flag2 = false;
					return result;
				}
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableExceptionType(e))
				{
					throw;
				}
				FailureCleanup(stateObj, e);
				throw;
			}
			FinalizeExecuteRPC(stateObj);
			if (completion != null)
			{
				completion.SetResult(null);
			}
			return null;
		}
		catch (Exception exception)
		{
			FinalizeExecuteRPC(stateObj);
			if (completion != null)
			{
				completion.SetException(exception);
				return null;
			}
			throw;
		}
		finally
		{
			if (flag2)
			{
				_connHandler._parserLock.Release();
			}
		}
	}

	private void FinalizeExecuteRPC(TdsParserStateObject stateObj)
	{
		stateObj.SniContext = SniContext.Snix_Read;
		_asyncWrite = false;
	}

	private void TdsExecuteRPC_OnFailure(Exception exc, TdsParserStateObject stateObj)
	{
		FailureCleanup(stateObj, exc);
	}

	private void ExecuteFlushTaskCallback(Task tsk, TdsParserStateObject stateObj, TaskCompletionSource<object> completion, bool releaseConnectionLock)
	{
		try
		{
			FinalizeExecuteRPC(stateObj);
			if (tsk.Exception != null)
			{
				Exception exception = tsk.Exception.InnerException;
				try
				{
					FailureCleanup(stateObj, tsk.Exception);
				}
				catch (Exception ex)
				{
					exception = ex;
				}
				completion.SetException(exception);
			}
			else
			{
				completion.SetResult(null);
			}
		}
		finally
		{
			if (releaseConnectionLock)
			{
				_connHandler._parserLock.Release();
			}
		}
	}

	private void WriteParameterName(string parameterName, TdsParserStateObject stateObj)
	{
		if (!string.IsNullOrEmpty(parameterName))
		{
			int num = parameterName.Length & 0xFF;
			stateObj.WriteByte((byte)num);
			WriteString(parameterName, num, 0, stateObj);
		}
		else
		{
			stateObj.WriteByte(0);
		}
	}

	private void WriteSmiParameter(SqlParameter param, int paramIndex, bool sendDefault, TdsParserStateObject stateObj)
	{
		ParameterPeekAheadValue peekAhead;
		SmiParameterMetaData smiParameterMetaData = param.MetaDataForSmi(out peekAhead);
		if (!_isKatmai)
		{
			throw ADP.VersionDoesNotSupportDataType(MetaType.GetMetaTypeFromSqlDbType(smiParameterMetaData.SqlDbType, smiParameterMetaData.IsMultiValued).TypeName);
		}
		object value;
		ExtendedClrTypeCode typeCode;
		if (sendDefault)
		{
			if (SqlDbType.Structured == smiParameterMetaData.SqlDbType && smiParameterMetaData.IsMultiValued)
			{
				value = s_tvpEmptyValue;
				typeCode = ExtendedClrTypeCode.IEnumerableOfSqlDataRecord;
			}
			else
			{
				value = null;
				typeCode = ExtendedClrTypeCode.DBNull;
			}
		}
		else if (param.Direction == ParameterDirection.Output)
		{
			bool parameterIsSqlType = param.ParameterIsSqlType;
			param.Value = null;
			value = null;
			typeCode = ExtendedClrTypeCode.DBNull;
			param.ParameterIsSqlType = parameterIsSqlType;
		}
		else
		{
			value = param.GetCoercedValue();
			typeCode = MetaDataUtilsSmi.DetermineExtendedTypeCodeForUseWithSqlDbType(smiParameterMetaData.SqlDbType, smiParameterMetaData.IsMultiValued, value, null);
		}
		WriteSmiParameterMetaData(smiParameterMetaData, sendDefault, stateObj);
		TdsParameterSetter setters = new TdsParameterSetter(stateObj, smiParameterMetaData);
		ValueUtilsSmi.SetCompatibleValueV200(new SmiEventSink_Default(), setters, 0, smiParameterMetaData, value, typeCode, param.Offset, (0 < param.Size) ? param.Size : (-1), peekAhead);
	}

	private void WriteSmiParameterMetaData(SmiParameterMetaData metaData, bool sendDefault, TdsParserStateObject stateObj)
	{
		byte b = 0;
		if (ParameterDirection.Output == metaData.Direction || ParameterDirection.InputOutput == metaData.Direction)
		{
			b |= 1;
		}
		if (sendDefault)
		{
			b |= 2;
		}
		WriteParameterName(metaData.Name, stateObj);
		stateObj.WriteByte(b);
		WriteSmiTypeInfo(metaData, stateObj);
	}

	private void WriteSmiTypeInfo(SmiExtendedMetaData metaData, TdsParserStateObject stateObj)
	{
		checked
		{
			switch (metaData.SqlDbType)
			{
			case SqlDbType.BigInt:
				stateObj.WriteByte(38);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Binary:
				stateObj.WriteByte(173);
				WriteUnsignedShort((ushort)metaData.MaxLength, stateObj);
				break;
			case SqlDbType.Bit:
				stateObj.WriteByte(104);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Char:
				stateObj.WriteByte(175);
				WriteUnsignedShort((ushort)metaData.MaxLength, stateObj);
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.DateTime:
				stateObj.WriteByte(111);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Decimal:
				stateObj.WriteByte(108);
				stateObj.WriteByte((byte)MetaType.MetaDecimal.FixedLength);
				stateObj.WriteByte(unchecked((byte)((metaData.Precision == 0) ? 1 : metaData.Precision)));
				stateObj.WriteByte(metaData.Scale);
				break;
			case SqlDbType.Float:
				stateObj.WriteByte(109);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Image:
				stateObj.WriteByte(165);
				WriteUnsignedShort(ushort.MaxValue, stateObj);
				break;
			case SqlDbType.Int:
				stateObj.WriteByte(38);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Money:
				stateObj.WriteByte(110);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.NChar:
				stateObj.WriteByte(239);
				WriteUnsignedShort((ushort)(metaData.MaxLength * 2), stateObj);
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.NText:
				stateObj.WriteByte(231);
				WriteUnsignedShort(ushort.MaxValue, stateObj);
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.NVarChar:
				stateObj.WriteByte(231);
				if (-1 == metaData.MaxLength)
				{
					WriteUnsignedShort(ushort.MaxValue, stateObj);
				}
				else
				{
					WriteUnsignedShort((ushort)(metaData.MaxLength * 2), stateObj);
				}
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.Real:
				stateObj.WriteByte(109);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.UniqueIdentifier:
				stateObj.WriteByte(36);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.SmallDateTime:
				stateObj.WriteByte(111);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.SmallInt:
				stateObj.WriteByte(38);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.SmallMoney:
				stateObj.WriteByte(110);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Text:
				stateObj.WriteByte(167);
				WriteUnsignedShort(ushort.MaxValue, stateObj);
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.Timestamp:
				stateObj.WriteByte(173);
				WriteShort((int)metaData.MaxLength, stateObj);
				break;
			case SqlDbType.TinyInt:
				stateObj.WriteByte(38);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.VarBinary:
				stateObj.WriteByte(165);
				WriteUnsignedShort(unchecked((ushort)metaData.MaxLength), stateObj);
				break;
			case SqlDbType.VarChar:
				stateObj.WriteByte(167);
				WriteUnsignedShort(unchecked((ushort)metaData.MaxLength), stateObj);
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.Variant:
				stateObj.WriteByte(98);
				WriteInt((int)metaData.MaxLength, stateObj);
				break;
			case SqlDbType.Xml:
				stateObj.WriteByte(241);
				if (string.IsNullOrEmpty(metaData.TypeSpecificNamePart1) && string.IsNullOrEmpty(metaData.TypeSpecificNamePart2) && string.IsNullOrEmpty(metaData.TypeSpecificNamePart3))
				{
					stateObj.WriteByte(0);
					break;
				}
				stateObj.WriteByte(1);
				WriteIdentifier(metaData.TypeSpecificNamePart1, stateObj);
				WriteIdentifier(metaData.TypeSpecificNamePart2, stateObj);
				WriteIdentifierWithShortLength(metaData.TypeSpecificNamePart3, stateObj);
				break;
			case SqlDbType.Udt:
				stateObj.WriteByte(240);
				WriteIdentifier(metaData.TypeSpecificNamePart1, stateObj);
				WriteIdentifier(metaData.TypeSpecificNamePart2, stateObj);
				WriteIdentifier(metaData.TypeSpecificNamePart3, stateObj);
				break;
			case SqlDbType.Structured:
				if (metaData.IsMultiValued)
				{
					WriteTvpTypeInfo(metaData, stateObj);
				}
				break;
			case SqlDbType.Date:
				stateObj.WriteByte(40);
				break;
			case SqlDbType.Time:
				stateObj.WriteByte(41);
				stateObj.WriteByte(metaData.Scale);
				break;
			case SqlDbType.DateTime2:
				stateObj.WriteByte(42);
				stateObj.WriteByte(metaData.Scale);
				break;
			case SqlDbType.DateTimeOffset:
				stateObj.WriteByte(43);
				stateObj.WriteByte(metaData.Scale);
				break;
			case (SqlDbType)24:
			case (SqlDbType)26:
			case (SqlDbType)27:
			case (SqlDbType)28:
				break;
			}
		}
	}

	private void WriteTvpTypeInfo(SmiExtendedMetaData metaData, TdsParserStateObject stateObj)
	{
		stateObj.WriteByte(243);
		WriteIdentifier(metaData.TypeSpecificNamePart1, stateObj);
		WriteIdentifier(metaData.TypeSpecificNamePart2, stateObj);
		WriteIdentifier(metaData.TypeSpecificNamePart3, stateObj);
		if (metaData.FieldMetaData.Count == 0)
		{
			WriteUnsignedShort(ushort.MaxValue, stateObj);
		}
		else
		{
			WriteUnsignedShort(checked((ushort)metaData.FieldMetaData.Count), stateObj);
			SmiDefaultFieldsProperty smiDefaultFieldsProperty = (SmiDefaultFieldsProperty)metaData.ExtendedProperties[SmiPropertySelector.DefaultFields];
			for (int i = 0; i < metaData.FieldMetaData.Count; i++)
			{
				WriteTvpColumnMetaData(metaData.FieldMetaData[i], smiDefaultFieldsProperty[i], stateObj);
			}
			WriteTvpOrderUnique(metaData, stateObj);
		}
		stateObj.WriteByte(0);
	}

	private void WriteTvpColumnMetaData(SmiExtendedMetaData md, bool isDefault, TdsParserStateObject stateObj)
	{
		if (SqlDbType.Timestamp == md.SqlDbType)
		{
			WriteUnsignedInt(80u, stateObj);
		}
		else
		{
			WriteUnsignedInt(0u, stateObj);
		}
		ushort num = 1;
		if (isDefault)
		{
			num |= 0x200;
		}
		WriteUnsignedShort(num, stateObj);
		WriteSmiTypeInfo(md, stateObj);
		WriteIdentifier(null, stateObj);
	}

	private void WriteTvpOrderUnique(SmiExtendedMetaData metaData, TdsParserStateObject stateObj)
	{
		SmiOrderProperty smiOrderProperty = (SmiOrderProperty)metaData.ExtendedProperties[SmiPropertySelector.SortOrder];
		SmiUniqueKeyProperty smiUniqueKeyProperty = (SmiUniqueKeyProperty)metaData.ExtendedProperties[SmiPropertySelector.UniqueKey];
		List<TdsOrderUnique> list = new List<TdsOrderUnique>(metaData.FieldMetaData.Count);
		for (int i = 0; i < metaData.FieldMetaData.Count; i++)
		{
			byte b = 0;
			SmiOrderProperty.SmiColumnOrder smiColumnOrder = smiOrderProperty[i];
			if (smiColumnOrder.Order == SortOrder.Ascending)
			{
				b = 1;
			}
			else if (SortOrder.Descending == smiColumnOrder.Order)
			{
				b = 2;
			}
			if (smiUniqueKeyProperty[i])
			{
				b |= 4;
			}
			if (b != 0)
			{
				list.Add(new TdsOrderUnique(checked((short)(i + 1)), b));
			}
		}
		if (0 >= list.Count)
		{
			return;
		}
		stateObj.WriteByte(16);
		WriteShort(list.Count, stateObj);
		foreach (TdsOrderUnique item in list)
		{
			WriteShort(item.ColumnOrdinal, stateObj);
			stateObj.WriteByte(item.Flags);
		}
	}

	internal Task WriteBulkCopyDone(TdsParserStateObject stateObj)
	{
		if (State != TdsParserState.OpenNotLoggedIn && State != TdsParserState.OpenLoggedIn)
		{
			throw ADP.ClosedConnectionError();
		}
		stateObj.WriteByte(253);
		WriteShort(0, stateObj);
		WriteShort(0, stateObj);
		WriteInt(0, stateObj);
		stateObj._pendingData = true;
		stateObj._messageStatus = 0;
		return stateObj.WritePacket(1);
	}

	internal void WriteBulkCopyMetaData(_SqlMetaDataSet metadataCollection, int count, TdsParserStateObject stateObj)
	{
		if (State != TdsParserState.OpenNotLoggedIn && State != TdsParserState.OpenLoggedIn)
		{
			throw ADP.ClosedConnectionError();
		}
		stateObj.WriteByte(129);
		WriteShort(count, stateObj);
		for (int i = 0; i < metadataCollection.Length; i++)
		{
			if (metadataCollection[i] == null)
			{
				continue;
			}
			_SqlMetaData sqlMetaData = metadataCollection[i];
			WriteInt(0, stateObj);
			ushort num = (ushort)(sqlMetaData.updatability << 2);
			num = (ushort)(num | (sqlMetaData.isNullable ? 1 : 0));
			num = (ushort)(num | (sqlMetaData.isIdentity ? 16 : 0));
			WriteShort(num, stateObj);
			switch (sqlMetaData.type)
			{
			case SqlDbType.Decimal:
				stateObj.WriteByte(sqlMetaData.tdsType);
				WriteTokenLength(sqlMetaData.tdsType, sqlMetaData.length, stateObj);
				stateObj.WriteByte(sqlMetaData.precision);
				stateObj.WriteByte(sqlMetaData.scale);
				break;
			case SqlDbType.Xml:
				stateObj.WriteByteArray(s_xmlMetadataSubstituteSequence, s_xmlMetadataSubstituteSequence.Length, 0);
				break;
			case SqlDbType.Udt:
				stateObj.WriteByte(165);
				WriteTokenLength(165, sqlMetaData.length, stateObj);
				break;
			case SqlDbType.Date:
				stateObj.WriteByte(sqlMetaData.tdsType);
				break;
			case SqlDbType.Time:
			case SqlDbType.DateTime2:
			case SqlDbType.DateTimeOffset:
				stateObj.WriteByte(sqlMetaData.tdsType);
				stateObj.WriteByte(sqlMetaData.scale);
				break;
			default:
				stateObj.WriteByte(sqlMetaData.tdsType);
				WriteTokenLength(sqlMetaData.tdsType, sqlMetaData.length, stateObj);
				if (sqlMetaData.metaType.IsCharType)
				{
					WriteUnsignedInt(sqlMetaData.collation.info, stateObj);
					stateObj.WriteByte(sqlMetaData.collation.sortId);
				}
				break;
			}
			if (sqlMetaData.metaType.IsLong && !sqlMetaData.metaType.IsPlp)
			{
				WriteShort(sqlMetaData.tableName.Length, stateObj);
				WriteString(sqlMetaData.tableName, stateObj);
			}
			stateObj.WriteByte((byte)sqlMetaData.column.Length);
			WriteString(sqlMetaData.column, stateObj);
		}
	}

	internal Task WriteBulkCopyValue(object value, SqlMetaDataPriv metadata, TdsParserStateObject stateObj, bool isSqlType, bool isDataFeed, bool isNull)
	{
		Encoding defaultEncoding = _defaultEncoding;
		SqlCollation defaultCollation = _defaultCollation;
		int defaultCodePage = _defaultCodePage;
		int defaultLCID = _defaultLCID;
		Task result = null;
		Task task = null;
		if (State != TdsParserState.OpenNotLoggedIn && State != TdsParserState.OpenLoggedIn)
		{
			throw ADP.ClosedConnectionError();
		}
		try
		{
			if (metadata.encoding != null)
			{
				_defaultEncoding = metadata.encoding;
			}
			if (metadata.collation != null)
			{
				_defaultCollation = metadata.collation;
				_defaultLCID = _defaultCollation.LCID;
			}
			_defaultCodePage = metadata.codePage;
			MetaType metaType = metadata.metaType;
			int num = 0;
			int num2 = 0;
			if (isNull)
			{
				if (metaType.IsPlp && (metaType.NullableType != 240 || metaType.IsLong))
				{
					WriteLong(-1L, stateObj);
				}
				else if (!metaType.IsFixed && !metaType.IsLong && !metaType.IsVarTime)
				{
					WriteShort(65535, stateObj);
				}
				else
				{
					stateObj.WriteByte(0);
				}
				return result;
			}
			if (!isDataFeed)
			{
				switch (metaType.NullableType)
				{
				case 34:
				case 165:
				case 173:
				case 240:
					num = (isSqlType ? ((SqlBinary)value).Length : ((byte[])value).Length);
					break;
				case 36:
					num = 16;
					break;
				case 35:
				case 167:
				case 175:
				{
					if (_defaultEncoding == null)
					{
						ThrowUnsupportedCollationEncountered(null);
					}
					string text = null;
					text = ((!isSqlType) ? ((string)value) : ((SqlString)value).Value);
					num = text.Length;
					num2 = _defaultEncoding.GetByteCount(text);
					break;
				}
				case 99:
				case 231:
				case 239:
					num = (isSqlType ? ((SqlString)value).Value.Length : ((string)value).Length) * 2;
					break;
				case 241:
					if (value is XmlReader)
					{
						value = MetaType.GetStringFromXml((XmlReader)value);
					}
					num = (isSqlType ? ((SqlString)value).Value.Length : ((string)value).Length) * 2;
					break;
				default:
					num = metadata.length;
					break;
				}
			}
			if (metaType.IsLong)
			{
				switch (metaType.SqlDbType)
				{
				case SqlDbType.Image:
				case SqlDbType.NText:
				case SqlDbType.Text:
					stateObj.WriteByteArray(s_longDataHeader, s_longDataHeader.Length, 0);
					WriteTokenLength(metadata.tdsType, (num2 == 0) ? num : num2, stateObj);
					break;
				case SqlDbType.NVarChar:
				case SqlDbType.VarBinary:
				case SqlDbType.VarChar:
				case SqlDbType.Xml:
				case SqlDbType.Udt:
					WriteUnsignedLong(18446744073709551614uL, stateObj);
					break;
				}
			}
			else
			{
				WriteTokenLength(metadata.tdsType, (num2 == 0) ? num : num2, stateObj);
			}
			if (isSqlType)
			{
				task = WriteSqlValue(value, metaType, num, num2, 0, stateObj);
			}
			else if (metaType.SqlDbType != SqlDbType.Udt || metaType.IsLong)
			{
				task = WriteValue(value, metaType, metadata.scale, num, num2, 0, stateObj, metadata.length, isDataFeed);
				if (task == null && _asyncWrite)
				{
					task = stateObj.WaitForAccumulatedWrites();
				}
			}
			else
			{
				WriteShort(num, stateObj);
				task = stateObj.WriteByteArray((byte[])value, num, 0);
			}
			if (task != null)
			{
				result = WriteBulkCopyValueSetupContinuation(task, defaultEncoding, defaultCollation, defaultCodePage, defaultLCID);
			}
		}
		finally
		{
			if (task == null)
			{
				_defaultEncoding = defaultEncoding;
				_defaultCollation = defaultCollation;
				_defaultCodePage = defaultCodePage;
				_defaultLCID = defaultLCID;
			}
		}
		return result;
	}

	private Task WriteBulkCopyValueSetupContinuation(Task internalWriteTask, Encoding saveEncoding, SqlCollation saveCollation, int saveCodePage, int saveLCID)
	{
		return internalWriteTask.ContinueWith(delegate(Task t)
		{
			_defaultEncoding = saveEncoding;
			_defaultCollation = saveCollation;
			_defaultCodePage = saveCodePage;
			_defaultLCID = saveLCID;
			return t;
		}, TaskScheduler.Default).Unwrap();
	}

	private void WriteMarsHeaderData(TdsParserStateObject stateObj, SqlInternalTransaction transaction)
	{
		WriteShort(2, stateObj);
		if (transaction != null && transaction.TransactionId != 0L)
		{
			WriteLong(transaction.TransactionId, stateObj);
			WriteInt(stateObj.IncrementAndObtainOpenResultCount(transaction), stateObj);
		}
		else
		{
			WriteLong(0L, stateObj);
			WriteInt(stateObj.IncrementAndObtainOpenResultCount(null), stateObj);
		}
	}

	private int GetNotificationHeaderSize(SqlNotificationRequest notificationRequest)
	{
		if (notificationRequest != null)
		{
			string userData = notificationRequest.UserData;
			string options = notificationRequest.Options;
			int timeout = notificationRequest.Timeout;
			if (userData == null)
			{
				throw ADP.ArgumentNull("callbackId");
			}
			if (65535 < userData.Length)
			{
				throw ADP.ArgumentOutOfRange("callbackId");
			}
			if (options == null)
			{
				throw ADP.ArgumentNull("service");
			}
			if (65535 < options.Length)
			{
				throw ADP.ArgumentOutOfRange("service");
			}
			if (-1 > timeout)
			{
				throw ADP.ArgumentOutOfRange("timeout");
			}
			int num = 8 + userData.Length * 2 + 2 + options.Length * 2;
			if (timeout > 0)
			{
				num += 4;
			}
			return num;
		}
		return 0;
	}

	private void WriteQueryNotificationHeaderData(SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj)
	{
		string userData = notificationRequest.UserData;
		string options = notificationRequest.Options;
		int timeout = notificationRequest.Timeout;
		WriteShort(1, stateObj);
		WriteShort(userData.Length * 2, stateObj);
		WriteString(userData, stateObj);
		WriteShort(options.Length * 2, stateObj);
		WriteString(options, stateObj);
		if (timeout > 0)
		{
			WriteInt(timeout, stateObj);
		}
	}

	private void WriteRPCBatchHeaders(TdsParserStateObject stateObj, SqlNotificationRequest notificationRequest)
	{
		int notificationHeaderSize = GetNotificationHeaderSize(notificationRequest);
		int v = 22 + notificationHeaderSize;
		WriteInt(v, stateObj);
		WriteInt(18, stateObj);
		WriteMarsHeaderData(stateObj, CurrentTransaction);
		if (notificationHeaderSize != 0)
		{
			WriteInt(notificationHeaderSize, stateObj);
			WriteQueryNotificationHeaderData(notificationRequest, stateObj);
		}
	}

	private void WriteTokenLength(byte token, int length, TdsParserStateObject stateObj)
	{
		int num = 0;
		if (240 == token)
		{
			num = 8;
		}
		else if (token == 241)
		{
			num = 8;
		}
		if (num == 0)
		{
			switch (token & 0x30)
			{
			case 48:
				num = 0;
				break;
			case 16:
				num = 0;
				break;
			case 0:
			case 32:
				num = (((token & 0x80) == 0) ? (((token & 0xC) != 0) ? 1 : 4) : 2);
				break;
			}
			switch (num)
			{
			case 1:
				stateObj.WriteByte((byte)length);
				break;
			case 2:
				WriteShort(length, stateObj);
				break;
			case 4:
				WriteInt(length, stateObj);
				break;
			case 8:
				WriteShort(65535, stateObj);
				break;
			}
		}
	}

	private bool IsBOMNeeded(MetaType type, object value)
	{
		if (type.NullableType == 241)
		{
			Type type2 = value.GetType();
			if (type2 == typeof(SqlString))
			{
				if (!((SqlString)value).IsNull && ((SqlString)value).Value.Length > 0 && (((SqlString)value).Value[0] & 0xFF) != 255)
				{
					return true;
				}
			}
			else if (type2 == typeof(string) && ((string)value).Length > 0)
			{
				if (value != null && (((string)value)[0] & 0xFF) != 255)
				{
					return true;
				}
			}
			else if (type2 == typeof(SqlXml))
			{
				if (!((SqlXml)value).IsNull)
				{
					return true;
				}
			}
			else if (type2 == typeof(XmlDataFeed))
			{
				return true;
			}
		}
		return false;
	}

	private Task GetTerminationTask(Task unterminatedWriteTask, object value, MetaType type, int actualLength, TdsParserStateObject stateObj, bool isDataFeed)
	{
		if (type.IsPlp && (actualLength > 0 || isDataFeed))
		{
			if (unterminatedWriteTask == null)
			{
				WriteInt(0, stateObj);
				return null;
			}
			return AsyncHelper.CreateContinuationTask(unterminatedWriteTask, WriteInt, 0, stateObj, _connHandler);
		}
		return unterminatedWriteTask;
	}

	private Task WriteSqlValue(object value, MetaType type, int actualLength, int codePageByteSize, int offset, TdsParserStateObject stateObj)
	{
		return GetTerminationTask(WriteUnterminatedSqlValue(value, type, actualLength, codePageByteSize, offset, stateObj), value, type, actualLength, stateObj, isDataFeed: false);
	}

	private Task WriteUnterminatedSqlValue(object value, MetaType type, int actualLength, int codePageByteSize, int offset, TdsParserStateObject stateObj)
	{
		switch (type.NullableType)
		{
		case 109:
			if (type.FixedLength == 4)
			{
				WriteFloat(((SqlSingle)value).Value, stateObj);
			}
			else
			{
				WriteDouble(((SqlDouble)value).Value, stateObj);
			}
			break;
		case 34:
		case 165:
		case 173:
			if (type.IsPlp)
			{
				WriteInt(actualLength, stateObj);
			}
			if (value is SqlBinary)
			{
				return stateObj.WriteByteArray(((SqlBinary)value).Value, actualLength, offset, canAccumulate: false);
			}
			return stateObj.WriteByteArray(((SqlBytes)value).Value, actualLength, offset, canAccumulate: false);
		case 36:
		{
			byte[] b = ((SqlGuid)value).ToByteArray();
			stateObj.WriteByteArray(b, actualLength, 0);
			break;
		}
		case 104:
			if (((SqlBoolean)value).Value)
			{
				stateObj.WriteByte(1);
			}
			else
			{
				stateObj.WriteByte(0);
			}
			break;
		case 38:
			if (type.FixedLength == 1)
			{
				stateObj.WriteByte(((SqlByte)value).Value);
			}
			else if (type.FixedLength == 2)
			{
				WriteShort(((SqlInt16)value).Value, stateObj);
			}
			else if (type.FixedLength == 4)
			{
				WriteInt(((SqlInt32)value).Value, stateObj);
			}
			else
			{
				WriteLong(((SqlInt64)value).Value, stateObj);
			}
			break;
		case 35:
		case 167:
		case 175:
			if (type.IsPlp)
			{
				WriteInt(codePageByteSize, stateObj);
			}
			if (value is SqlChars)
			{
				string s = new string(((SqlChars)value).Value);
				return WriteEncodingChar(s, actualLength, offset, _defaultEncoding, stateObj, canAccumulate: false);
			}
			return WriteEncodingChar(((SqlString)value).Value, actualLength, offset, _defaultEncoding, stateObj, canAccumulate: false);
		case 99:
		case 231:
		case 239:
		case 241:
			if (type.IsPlp)
			{
				if (IsBOMNeeded(type, value))
				{
					WriteInt(actualLength + 2, stateObj);
					WriteShort(65279, stateObj);
				}
				else
				{
					WriteInt(actualLength, stateObj);
				}
			}
			if (actualLength != 0)
			{
				actualLength >>= 1;
			}
			if (value is SqlChars)
			{
				return WriteCharArray(((SqlChars)value).Value, actualLength, offset, stateObj, canAccumulate: false);
			}
			return WriteString(((SqlString)value).Value, actualLength, offset, stateObj, canAccumulate: false);
		case 108:
			WriteSqlDecimal((SqlDecimal)value, stateObj);
			break;
		case 111:
		{
			SqlDateTime sqlDateTime = (SqlDateTime)value;
			if (type.FixedLength == 4)
			{
				if (0 > sqlDateTime.DayTicks || sqlDateTime.DayTicks > 65535)
				{
					throw SQL.SmallDateTimeOverflow(sqlDateTime.ToString());
				}
				WriteShort(sqlDateTime.DayTicks, stateObj);
				WriteShort(sqlDateTime.TimeTicks / SqlDateTime.SQLTicksPerMinute, stateObj);
			}
			else
			{
				WriteInt(sqlDateTime.DayTicks, stateObj);
				WriteInt(sqlDateTime.TimeTicks, stateObj);
			}
			break;
		}
		case 110:
			WriteSqlMoney((SqlMoney)value, type.FixedLength, stateObj);
			break;
		case 240:
			throw SQL.UDTUnexpectedResult(value.GetType().AssemblyQualifiedName);
		}
		return null;
	}

	private async Task WriteXmlFeed(XmlDataFeed feed, TdsParserStateObject stateObj, bool needBom, Encoding encoding, int size)
	{
		byte[] preambleToStrip = null;
		if (!needBom)
		{
			preambleToStrip = encoding.GetPreamble();
		}
		ConstrainedTextWriter writer = new ConstrainedTextWriter(new StreamWriter(new TdsOutputStream(this, stateObj, preambleToStrip), encoding), size);
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.CloseOutput = false;
		xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
		if (_asyncWrite)
		{
			xmlWriterSettings.Async = true;
		}
		XmlWriter ww = XmlWriter.Create(writer, xmlWriterSettings);
		if (feed._source.ReadState == ReadState.Initial)
		{
			feed._source.Read();
		}
		while (!feed._source.EOF && !writer.IsComplete)
		{
			if (feed._source.NodeType == XmlNodeType.XmlDeclaration)
			{
				feed._source.Read();
			}
			else if (_asyncWrite)
			{
				await ww.WriteNodeAsync(feed._source, defattr: true).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				ww.WriteNode(feed._source, defattr: true);
			}
		}
		if (_asyncWrite)
		{
			await ww.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			ww.Flush();
		}
	}

	private async Task WriteTextFeed(TextDataFeed feed, Encoding encoding, bool needBom, TdsParserStateObject stateObj, int size)
	{
		char[] inBuff = new char[4096];
		encoding = encoding ?? new UnicodeEncoding(bigEndian: false, byteOrderMark: false);
		ConstrainedTextWriter writer = new ConstrainedTextWriter(new StreamWriter(new TdsOutputStream(this, stateObj, null), encoding), size);
		if (needBom)
		{
			if (_asyncWrite)
			{
				await writer.WriteAsync('\ufeff').ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				writer.Write('\ufeff');
			}
		}
		int nWritten = 0;
		do
		{
			int nRead = ((!_asyncWrite) ? feed._source.ReadBlock(inBuff, 0, 4096) : (await feed._source.ReadBlockAsync(inBuff, 0, 4096).ConfigureAwait(continueOnCapturedContext: false)));
			if (nRead == 0)
			{
				break;
			}
			if (_asyncWrite)
			{
				await writer.WriteAsync(inBuff, 0, nRead).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				writer.Write(inBuff, 0, nRead);
			}
			nWritten += nRead;
		}
		while (!writer.IsComplete);
		if (_asyncWrite)
		{
			await writer.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			writer.Flush();
		}
	}

	private async Task WriteStreamFeed(StreamDataFeed feed, TdsParserStateObject stateObj, int len)
	{
		TdsOutputStream output = new TdsOutputStream(this, stateObj, null);
		byte[] buff = new byte[4096];
		int nWritten = 0;
		do
		{
			int num = 4096;
			if (len > 0 && nWritten + num > len)
			{
				num = len - nWritten;
			}
			int nRead = ((!_asyncWrite) ? feed._source.Read(buff, 0, num) : (await feed._source.ReadAsync(buff, 0, num).ConfigureAwait(continueOnCapturedContext: false)));
			if (nRead == 0)
			{
				break;
			}
			if (_asyncWrite)
			{
				await output.WriteAsync(buff, 0, nRead).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				output.Write(buff, 0, nRead);
			}
			nWritten += nRead;
		}
		while (len <= 0 || nWritten < len);
	}

	private Task NullIfCompletedWriteTask(Task task)
	{
		if (task == null)
		{
			return null;
		}
		return task.Status switch
		{
			TaskStatus.RanToCompletion => null, 
			TaskStatus.Faulted => throw task.Exception.InnerException, 
			TaskStatus.Canceled => throw SQL.OperationCancelled(), 
			_ => task, 
		};
	}

	private Task WriteValue(object value, MetaType type, byte scale, int actualLength, int encodingByteSize, int offset, TdsParserStateObject stateObj, int paramSize, bool isDataFeed)
	{
		return GetTerminationTask(WriteUnterminatedValue(value, type, scale, actualLength, encodingByteSize, offset, stateObj, paramSize, isDataFeed), value, type, actualLength, stateObj, isDataFeed);
	}

	private Task WriteUnterminatedValue(object value, MetaType type, byte scale, int actualLength, int encodingByteSize, int offset, TdsParserStateObject stateObj, int paramSize, bool isDataFeed)
	{
		switch (type.NullableType)
		{
		case 109:
			if (type.FixedLength == 4)
			{
				WriteFloat((float)value, stateObj);
			}
			else
			{
				WriteDouble((double)value, stateObj);
			}
			break;
		case 34:
		case 165:
		case 173:
		case 240:
			if (isDataFeed)
			{
				return NullIfCompletedWriteTask(WriteStreamFeed((StreamDataFeed)value, stateObj, paramSize));
			}
			if (type.IsPlp)
			{
				WriteInt(actualLength, stateObj);
			}
			return stateObj.WriteByteArray((byte[])value, actualLength, offset, canAccumulate: false);
		case 36:
		{
			byte[] b = ((Guid)value).ToByteArray();
			stateObj.WriteByteArray(b, actualLength, 0);
			break;
		}
		case 104:
			if ((bool)value)
			{
				stateObj.WriteByte(1);
			}
			else
			{
				stateObj.WriteByte(0);
			}
			break;
		case 38:
			if (type.FixedLength == 1)
			{
				stateObj.WriteByte((byte)value);
			}
			else if (type.FixedLength == 2)
			{
				WriteShort((short)value, stateObj);
			}
			else if (type.FixedLength == 4)
			{
				WriteInt((int)value, stateObj);
			}
			else
			{
				WriteLong((long)value, stateObj);
			}
			break;
		case 35:
		case 167:
		case 175:
			if (isDataFeed)
			{
				if (!(value is TextDataFeed feed2))
				{
					return NullIfCompletedWriteTask(WriteXmlFeed((XmlDataFeed)value, stateObj, needBom: true, _defaultEncoding, paramSize));
				}
				return NullIfCompletedWriteTask(WriteTextFeed(feed2, _defaultEncoding, needBom: false, stateObj, paramSize));
			}
			if (type.IsPlp)
			{
				WriteInt(encodingByteSize, stateObj);
			}
			if (value is byte[])
			{
				return stateObj.WriteByteArray((byte[])value, actualLength, 0, canAccumulate: false);
			}
			return WriteEncodingChar((string)value, actualLength, offset, _defaultEncoding, stateObj, canAccumulate: false);
		case 99:
		case 231:
		case 239:
		case 241:
			if (isDataFeed)
			{
				if (!(value is TextDataFeed feed))
				{
					return NullIfCompletedWriteTask(WriteXmlFeed((XmlDataFeed)value, stateObj, IsBOMNeeded(type, value), Encoding.Unicode, paramSize));
				}
				return NullIfCompletedWriteTask(WriteTextFeed(feed, null, IsBOMNeeded(type, value), stateObj, paramSize));
			}
			if (type.IsPlp)
			{
				if (IsBOMNeeded(type, value))
				{
					WriteInt(actualLength + 2, stateObj);
					WriteShort(65279, stateObj);
				}
				else
				{
					WriteInt(actualLength, stateObj);
				}
			}
			if (value is byte[])
			{
				return stateObj.WriteByteArray((byte[])value, actualLength, 0, canAccumulate: false);
			}
			actualLength >>= 1;
			return WriteString((string)value, actualLength, offset, stateObj, canAccumulate: false);
		case 108:
			WriteDecimal((decimal)value, stateObj);
			break;
		case 111:
		{
			TdsDateTime tdsDateTime = MetaType.FromDateTime((DateTime)value, (byte)type.FixedLength);
			if (type.FixedLength == 4)
			{
				if (0 > tdsDateTime.days || tdsDateTime.days > 65535)
				{
					throw SQL.SmallDateTimeOverflow(MetaType.ToDateTime(tdsDateTime.days, tdsDateTime.time, 4).ToString(CultureInfo.InvariantCulture));
				}
				WriteShort(tdsDateTime.days, stateObj);
				WriteShort(tdsDateTime.time, stateObj);
			}
			else
			{
				WriteInt(tdsDateTime.days, stateObj);
				WriteInt(tdsDateTime.time, stateObj);
			}
			break;
		}
		case 110:
			WriteCurrency((decimal)value, type.FixedLength, stateObj);
			break;
		case 40:
			WriteDate((DateTime)value, stateObj);
			break;
		case 41:
			if (scale > 7)
			{
				throw SQL.TimeScaleValueOutOfRange(scale);
			}
			WriteTime((TimeSpan)value, scale, actualLength, stateObj);
			break;
		case 42:
			if (scale > 7)
			{
				throw SQL.TimeScaleValueOutOfRange(scale);
			}
			WriteDateTime2((DateTime)value, scale, actualLength, stateObj);
			break;
		case 43:
			WriteDateTimeOffset((DateTimeOffset)value, scale, actualLength, stateObj);
			break;
		}
		return null;
	}

	internal void WriteParameterVarLen(MetaType type, int size, bool isNull, TdsParserStateObject stateObj, bool unknownLength = false)
	{
		if (type.IsLong)
		{
			if (isNull)
			{
				if (type.IsPlp)
				{
					WriteLong(-1L, stateObj);
				}
				else
				{
					WriteInt(-1, stateObj);
				}
			}
			else if (type.NullableType == 241 || unknownLength)
			{
				WriteUnsignedLong(18446744073709551614uL, stateObj);
			}
			else if (type.IsPlp)
			{
				WriteLong(size, stateObj);
			}
			else
			{
				WriteInt(size, stateObj);
			}
		}
		else if (type.IsVarTime)
		{
			if (isNull)
			{
				stateObj.WriteByte(0);
			}
			else
			{
				stateObj.WriteByte((byte)size);
			}
		}
		else if (!type.IsFixed)
		{
			if (isNull)
			{
				WriteShort(65535, stateObj);
			}
			else
			{
				WriteShort(size, stateObj);
			}
		}
		else if (isNull)
		{
			stateObj.WriteByte(0);
		}
		else
		{
			stateObj.WriteByte((byte)(type.FixedLength & 0xFF));
		}
	}

	private bool TryReadPlpUnicodeCharsChunk(char[] buff, int offst, int len, TdsParserStateObject stateObj, out int charsRead)
	{
		if (stateObj._longlenleft == 0L)
		{
			charsRead = 0;
			return true;
		}
		charsRead = len;
		if (stateObj._longlenleft >> 1 < (ulong)len)
		{
			charsRead = (int)(stateObj._longlenleft >> 1);
		}
		for (int i = 0; i < charsRead; i++)
		{
			if (!stateObj.TryReadChar(out buff[offst + i]))
			{
				return false;
			}
		}
		stateObj._longlenleft -= (ulong)((long)charsRead << 1);
		return true;
	}

	internal int ReadPlpUnicodeChars(ref char[] buff, int offst, int len, TdsParserStateObject stateObj)
	{
		if (!TryReadPlpUnicodeChars(ref buff, offst, len, stateObj, out var totalCharsRead))
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		return totalCharsRead;
	}

	internal bool TryReadPlpUnicodeChars(ref char[] buff, int offst, int len, TdsParserStateObject stateObj, out int totalCharsRead)
	{
		int num = 0;
		int num2 = 0;
		if (stateObj._longlen == 0L)
		{
			totalCharsRead = 0;
			return true;
		}
		num2 = len;
		if (buff == null && stateObj._longlen != 18446744073709551614uL)
		{
			buff = new char[Math.Min((int)stateObj._longlen, len)];
		}
		if (stateObj._longlenleft == 0L)
		{
			if (!stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out var _))
			{
				totalCharsRead = 0;
				return false;
			}
			if (stateObj._longlenleft == 0L)
			{
				totalCharsRead = 0;
				return true;
			}
		}
		totalCharsRead = 0;
		while (num2 > 0)
		{
			num = (int)Math.Min(stateObj._longlenleft + 1 >> 1, (ulong)num2);
			if (buff == null || buff.Length < offst + num)
			{
				char[] array = new char[offst + num];
				if (buff != null)
				{
					Buffer.BlockCopy(buff, 0, array, 0, offst * 2);
				}
				buff = array;
			}
			if (num > 0)
			{
				if (!TryReadPlpUnicodeCharsChunk(buff, offst, num, stateObj, out num))
				{
					return false;
				}
				num2 -= num;
				offst += num;
				totalCharsRead += num;
			}
			if (stateObj._longlenleft == 1 && num2 > 0)
			{
				if (!stateObj.TryReadByte(out var value))
				{
					return false;
				}
				stateObj._longlenleft--;
				if (!stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out var _))
				{
					return false;
				}
				if (!stateObj.TryReadByte(out var value2))
				{
					return false;
				}
				stateObj._longlenleft--;
				buff[offst] = (char)(((value2 & 0xFF) << 8) + (value & 0xFF));
				offst = checked(offst + 1);
				num++;
				num2--;
				totalCharsRead++;
			}
			if (stateObj._longlenleft == 0L && !stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out var _))
			{
				return false;
			}
			if (stateObj._longlenleft == 0L)
			{
				break;
			}
		}
		return true;
	}

	internal int ReadPlpAnsiChars(ref char[] buff, int offst, int len, SqlMetaDataPriv metadata, TdsParserStateObject stateObj)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (stateObj._longlen == 0L)
		{
			return 0;
		}
		num2 = len;
		if (stateObj._longlenleft == 0L)
		{
			stateObj.ReadPlpLength(returnPlpNullIfNull: false);
			if (stateObj._longlenleft == 0L)
			{
				stateObj._plpdecoder = null;
				return 0;
			}
		}
		if (stateObj._plpdecoder == null)
		{
			Encoding encoding = metadata.encoding;
			if (encoding == null)
			{
				if (_defaultEncoding == null)
				{
					ThrowUnsupportedCollationEncountered(stateObj);
				}
				encoding = _defaultEncoding;
			}
			stateObj._plpdecoder = encoding.GetDecoder();
		}
		while (num2 > 0)
		{
			num3 = (int)Math.Min(stateObj._longlenleft, (ulong)num2);
			if (stateObj._bTmp == null || stateObj._bTmp.Length < num3)
			{
				stateObj._bTmp = new byte[num3];
			}
			num3 = stateObj.ReadPlpBytesChunk(stateObj._bTmp, 0, num3);
			num = stateObj._plpdecoder.GetChars(stateObj._bTmp, 0, num3, buff, offst);
			num2 -= num;
			offst += num;
			num4 += num;
			if (stateObj._longlenleft == 0L)
			{
				stateObj.ReadPlpLength(returnPlpNullIfNull: false);
			}
			if (stateObj._longlenleft == 0L)
			{
				stateObj._plpdecoder = null;
				break;
			}
		}
		return num4;
	}

	internal ulong SkipPlpValue(ulong cb, TdsParserStateObject stateObj)
	{
		if (!TrySkipPlpValue(cb, stateObj, out var totalBytesSkipped))
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		return totalBytesSkipped;
	}

	internal bool TrySkipPlpValue(ulong cb, TdsParserStateObject stateObj, out ulong totalBytesSkipped)
	{
		totalBytesSkipped = 0uL;
		if (stateObj._longlenleft == 0L && !stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out var _))
		{
			return false;
		}
		while (totalBytesSkipped < cb && stateObj._longlenleft != 0)
		{
			int num = (int)((stateObj._longlenleft <= int.MaxValue) ? stateObj._longlenleft : int.MaxValue);
			num = ((cb - totalBytesSkipped < (ulong)num) ? ((int)(cb - totalBytesSkipped)) : num);
			if (!stateObj.TrySkipBytes(num))
			{
				return false;
			}
			stateObj._longlenleft -= (ulong)num;
			totalBytesSkipped += (ulong)num;
			if (stateObj._longlenleft == 0L && !stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out var _))
			{
				return false;
			}
		}
		return true;
	}

	internal ulong PlpBytesLeft(TdsParserStateObject stateObj)
	{
		if (stateObj._longlen != 0L && stateObj._longlenleft == 0L)
		{
			stateObj.ReadPlpLength(returnPlpNullIfNull: false);
		}
		return stateObj._longlenleft;
	}

	internal bool TryPlpBytesLeft(TdsParserStateObject stateObj, out ulong left)
	{
		if (stateObj._longlen != 0L && stateObj._longlenleft == 0L && !stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out left))
		{
			return false;
		}
		left = stateObj._longlenleft;
		return true;
	}

	internal ulong PlpBytesTotalLength(TdsParserStateObject stateObj)
	{
		if (stateObj._longlen == 18446744073709551614uL)
		{
			return ulong.MaxValue;
		}
		if (stateObj._longlen == ulong.MaxValue)
		{
			return 0uL;
		}
		return stateObj._longlen;
	}

	private bool TryProcessUDTMetaData(SqlMetaDataPriv metaData, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadUInt16(out var value))
		{
			return false;
		}
		metaData.length = value;
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		if (value2 != 0 && !stateObj.TryReadString(value2, out metaData.udtDatabaseName))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out value2))
		{
			return false;
		}
		if (value2 != 0 && !stateObj.TryReadString(value2, out metaData.udtSchemaName))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out value2))
		{
			return false;
		}
		if (value2 != 0 && !stateObj.TryReadString(value2, out metaData.udtTypeName))
		{
			return false;
		}
		if (!stateObj.TryReadUInt16(out value))
		{
			return false;
		}
		if (value != 0 && !stateObj.TryReadString(value, out metaData.udtAssemblyQualifiedName))
		{
			return false;
		}
		return true;
	}
}
