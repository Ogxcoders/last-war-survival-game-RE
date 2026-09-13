using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.Data.SqlClient.SNI;

internal class SNINpHandle : SNIHandle
{
	internal const string DefaultPipePath = "sql\\query";

	private const int MAX_PIPE_INSTANCES = 255;

	private readonly string _targetServer;

	private readonly object _callbackObject;

	private Stream _stream;

	private NamedPipeClientStream _pipeStream;

	private SslOverTdsStream _sslOverTdsStream;

	private SslStream _sslStream;

	private SNIAsyncCallback _receiveCallback;

	private SNIAsyncCallback _sendCallback;

	private bool _validateCert = true;

	private readonly uint _status = uint.MaxValue;

	private int _bufferSize = 4096;

	private readonly Guid _connectionId = Guid.NewGuid();

	public override Guid ConnectionId => _connectionId;

	public override uint Status => _status;

	public SNINpHandle(string serverName, string pipeName, long timerExpire, object callbackObject)
	{
		_targetServer = serverName;
		_callbackObject = callbackObject;
		try
		{
			_pipeStream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.WriteThrough | PipeOptions.Asynchronous);
			if (long.MaxValue == timerExpire)
			{
				_pipeStream.Connect(-1);
			}
			else
			{
				TimeSpan timeSpan = DateTime.FromFileTime(timerExpire) - DateTime.Now;
				timeSpan = ((timeSpan.Ticks < 0) ? TimeSpan.FromTicks(0L) : timeSpan);
				_pipeStream.Connect((int)timeSpan.TotalMilliseconds);
			}
		}
		catch (TimeoutException sniException)
		{
			SNICommon.ReportSNIError(SNIProviders.NP_PROV, 40u, sniException);
			_status = 1u;
			return;
		}
		catch (IOException sniException2)
		{
			SNICommon.ReportSNIError(SNIProviders.NP_PROV, 40u, sniException2);
			_status = 1u;
			return;
		}
		if (!_pipeStream.IsConnected || !_pipeStream.CanWrite || !_pipeStream.CanRead)
		{
			SNICommon.ReportSNIError(SNIProviders.NP_PROV, 0u, 40u, string.Empty);
			_status = 1u;
			return;
		}
		_sslOverTdsStream = new SslOverTdsStream(_pipeStream);
		_sslStream = new SslStream((Stream)_sslOverTdsStream, leaveInnerStreamOpen: true, (RemoteCertificateValidationCallback)ValidateServerCertificate, (LocalCertificateSelectionCallback)null);
		_stream = _pipeStream;
		_status = 0u;
	}

	public override uint CheckConnection()
	{
		if (!_stream.CanWrite || !_stream.CanRead)
		{
			return 1u;
		}
		return 0u;
	}

	public override void Dispose()
	{
		lock (this)
		{
			if (_sslOverTdsStream != null)
			{
				_sslOverTdsStream.Dispose();
				_sslOverTdsStream = null;
			}
			if (_sslStream != null)
			{
				_sslStream.Dispose();
				_sslStream = null;
			}
			if (_pipeStream != null)
			{
				_pipeStream.Dispose();
				_pipeStream = null;
			}
			_stream = null;
		}
	}

	public override uint Receive(out SNIPacket packet, int timeout)
	{
		lock (this)
		{
			packet = null;
			try
			{
				packet = new SNIPacket(_bufferSize);
				packet.ReadFromStream(_stream);
				if (packet.Length == 0)
				{
					Win32Exception ex = new Win32Exception();
					return ReportErrorAndReleasePacket(packet, (uint)ex.NativeErrorCode, 0u, ex.Message);
				}
			}
			catch (ObjectDisposedException sniException)
			{
				return ReportErrorAndReleasePacket(packet, sniException);
			}
			catch (IOException sniException2)
			{
				return ReportErrorAndReleasePacket(packet, sniException2);
			}
			return 0u;
		}
	}

	public override uint ReceiveAsync(ref SNIPacket packet)
	{
		packet = new SNIPacket(_bufferSize);
		try
		{
			packet.ReadFromStreamAsync(_stream, _receiveCallback);
			return 997u;
		}
		catch (ObjectDisposedException sniException)
		{
			return ReportErrorAndReleasePacket(packet, sniException);
		}
		catch (IOException sniException2)
		{
			return ReportErrorAndReleasePacket(packet, sniException2);
		}
	}

	public override uint Send(SNIPacket packet)
	{
		lock (this)
		{
			try
			{
				packet.WriteToStream(_stream);
				return 0u;
			}
			catch (ObjectDisposedException sniException)
			{
				return ReportErrorAndReleasePacket(packet, sniException);
			}
			catch (IOException sniException2)
			{
				return ReportErrorAndReleasePacket(packet, sniException2);
			}
		}
	}

	public override uint SendAsync(SNIPacket packet, bool disposePacketAfterSendAsync, SNIAsyncCallback callback = null)
	{
		SNIAsyncCallback callback2 = callback ?? _sendCallback;
		packet.WriteToStreamAsync(_stream, callback2, SNIProviders.NP_PROV, disposePacketAfterSendAsync);
		return 997u;
	}

	public override void SetAsyncCallbacks(SNIAsyncCallback receiveCallback, SNIAsyncCallback sendCallback)
	{
		_receiveCallback = receiveCallback;
		_sendCallback = sendCallback;
	}

	public override uint EnableSsl(uint options)
	{
		_validateCert = (options & 1) != 0;
		try
		{
			_sslStream.AuthenticateAsClientAsync(_targetServer).GetAwaiter().GetResult();
			_sslOverTdsStream.FinishHandshake();
		}
		catch (AuthenticationException sniException)
		{
			return SNICommon.ReportSNIError(SNIProviders.NP_PROV, 35u, sniException);
		}
		catch (InvalidOperationException sniException2)
		{
			return SNICommon.ReportSNIError(SNIProviders.NP_PROV, 35u, sniException2);
		}
		_stream = _sslStream;
		return 0u;
	}

	public override void DisableSsl()
	{
		_sslStream.Dispose();
		_sslStream = null;
		_sslOverTdsStream.Dispose();
		_sslOverTdsStream = null;
		_stream = _pipeStream;
	}

	private bool ValidateServerCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
	{
		if (!_validateCert)
		{
			return true;
		}
		return SNICommon.ValidateSslServerCertificate(_targetServer, sender, cert, chain, policyErrors);
	}

	public override void SetBufferSize(int bufferSize)
	{
		_bufferSize = bufferSize;
	}

	private uint ReportErrorAndReleasePacket(SNIPacket packet, Exception sniException)
	{
		packet?.Release();
		return SNICommon.ReportSNIError(SNIProviders.NP_PROV, 35u, sniException);
	}

	private uint ReportErrorAndReleasePacket(SNIPacket packet, uint nativeError, uint sniError, string errorMessage)
	{
		packet?.Release();
		return SNICommon.ReportSNIError(SNIProviders.NP_PROV, nativeError, sniError, errorMessage);
	}
}
