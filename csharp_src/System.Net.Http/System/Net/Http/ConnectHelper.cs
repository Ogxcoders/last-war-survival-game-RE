using System.Collections.Concurrent;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static class ConnectHelper
{
	internal sealed class CertificateCallbackMapper
	{
		public readonly Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> FromHttpClientHandler;

		public readonly RemoteCertificateValidationCallback ForSocketsHttpHandler;

		public CertificateCallbackMapper(Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> fromHttpClientHandler)
		{
			FromHttpClientHandler = fromHttpClientHandler;
			ForSocketsHttpHandler = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => FromHttpClientHandler(sender as HttpRequestMessage, certificate as X509Certificate2, chain, sslPolicyErrors);
		}
	}

	private sealed class ConnectEventArgs : SocketAsyncEventArgs
	{
		public AsyncTaskMethodBuilder Builder { get; private set; }

		public CancellationToken CancellationToken { get; private set; }

		public void Initialize(CancellationToken cancellationToken)
		{
			CancellationToken = cancellationToken;
			AsyncTaskMethodBuilder builder = default(AsyncTaskMethodBuilder);
			_ = builder.Task;
			Builder = builder;
		}

		public void Clear()
		{
			CancellationToken = default(CancellationToken);
		}

		protected override void OnCompleted(SocketAsyncEventArgs _)
		{
			switch (base.SocketError)
			{
			case SocketError.Success:
				Builder.SetResult();
				return;
			case SocketError.OperationAborted:
			case SocketError.ConnectionAborted:
				if (CancellationToken.IsCancellationRequested)
				{
					Builder.SetException(CancellationHelper.CreateOperationCanceledException(null, CancellationToken));
					return;
				}
				break;
			}
			Builder.SetException(new SocketException((int)base.SocketError));
		}
	}

	private static readonly ConcurrentQueue<ConnectEventArgs>.Segment s_connectEventArgs = new ConcurrentQueue<ConnectEventArgs>.Segment(ConcurrentQueue<ConnectEventArgs>.Segment.RoundUpToPowerOf2(Math.Max(2, Environment.ProcessorCount)));

	public static async ValueTask<(Socket, Stream)> ConnectAsync(string host, int port, CancellationToken cancellationToken)
	{
		if (!s_connectEventArgs.TryDequeue(out var saea))
		{
			saea = new ConnectEventArgs();
		}
		try
		{
			saea.Initialize(cancellationToken);
			saea.RemoteEndPoint = new DnsEndPoint(host, port);
			if (Socket.ConnectAsync(SocketType.Stream, ProtocolType.Tcp, saea))
			{
				using (cancellationToken.Register(delegate(object s)
				{
					Socket.CancelConnectAsync((SocketAsyncEventArgs)s);
				}, saea))
				{
					await saea.Builder.Task.ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			else if (saea.SocketError != SocketError.Success)
			{
				throw new SocketException((int)saea.SocketError);
			}
			Socket connectSocket = saea.ConnectSocket;
			connectSocket.NoDelay = true;
			return (connectSocket, new NetworkStream(connectSocket, ownsSocket: true));
		}
		catch (Exception ex)
		{
			throw CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken) ? CancellationHelper.CreateOperationCanceledException(ex, cancellationToken) : new HttpRequestException(ex.Message, ex);
		}
		finally
		{
			saea.Clear();
			if (!s_connectEventArgs.TryEnqueue(saea))
			{
				saea.Dispose();
			}
		}
	}

	public static ValueTask<SslStream> EstablishSslConnectionAsync(SslClientAuthenticationOptions sslOptions, HttpRequestMessage request, Stream stream, CancellationToken cancellationToken)
	{
		RemoteCertificateValidationCallback remoteCertificateValidationCallback = sslOptions.RemoteCertificateValidationCallback;
		if (remoteCertificateValidationCallback != null && remoteCertificateValidationCallback.Target is CertificateCallbackMapper certificateCallbackMapper)
		{
			sslOptions = sslOptions.ShallowClone();
			Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> localFromHttpClientHandler = certificateCallbackMapper.FromHttpClientHandler;
			HttpRequestMessage localRequest = request;
			sslOptions.RemoteCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => localFromHttpClientHandler(localRequest, certificate as X509Certificate2, chain, sslPolicyErrors);
		}
		return EstablishSslConnectionAsyncCore(stream, sslOptions, cancellationToken);
	}

	private static async ValueTask<SslStream> EstablishSslConnectionAsyncCore(Stream stream, SslClientAuthenticationOptions sslOptions, CancellationToken cancellationToken)
	{
		SslStream sslStream = new SslStream(stream);
		CancellationTokenRegistration ctr = cancellationToken.Register(delegate(object s)
		{
			((Stream)s).Dispose();
		}, stream);
		try
		{
			await sslStream.AuthenticateAsClientAsync(sslOptions, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception ex)
		{
			sslStream.Dispose();
			if (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
			{
				throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
			}
			throw new HttpRequestException("The SSL connection could not be established, see inner exception.", ex);
		}
		finally
		{
			ctr.Dispose();
		}
		if (cancellationToken.IsCancellationRequested)
		{
			sslStream.Dispose();
			throw CancellationHelper.CreateOperationCanceledException(null, cancellationToken);
		}
		return sslStream;
	}
}
