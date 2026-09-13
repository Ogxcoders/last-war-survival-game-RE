using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using BestHTTP.Authentication;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using BestHTTP.Extensions;
using BestHTTP.Logger;
using BestHTTP.PlatformSupport.TcpClient.General;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Tls;

namespace BestHTTP;

internal sealed class HTTPConnection : ConnectionBase
{
	private TcpClient Client;

	private Stream Stream;

	private KeepAliveHeader KeepAlive;

	public override bool IsRemovable
	{
		get
		{
			if (base.IsRemovable)
			{
				return true;
			}
			if (base.IsFree && KeepAlive != null && DateTime.UtcNow - LastProcessTime >= KeepAlive.TimeOut)
			{
				return true;
			}
			return false;
		}
	}

	internal HTTPConnection(string serverAddress)
		: base(serverAddress)
	{
	}

	protected override void ThreadFunc(object param)
	{
		bool flag = false;
		bool flag2 = false;
		RetryCauses retryCauses = RetryCauses.None;
		try
		{
			if (TryLoadAllFromCache())
			{
				return;
			}
			if (Client != null && !Client.IsConnected())
			{
				Close();
			}
			do
			{
				if (retryCauses == RetryCauses.Reconnect)
				{
					Close();
					Thread.Sleep(100);
				}
				base.LastProcessedUri = base.CurrentRequest.CurrentUri;
				retryCauses = RetryCauses.None;
				Connect();
				if (base.State == HTTPConnectionStates.AbortRequested)
				{
					throw new Exception("AbortRequested");
				}
				if (!base.CurrentRequest.DisableCache)
				{
					HTTPCacheService.SetHeaders(base.CurrentRequest);
				}
				bool flag3 = false;
				try
				{
					Client.NoDelay = base.CurrentRequest.TryToMinimizeTCPLatency;
					base.CurrentRequest.SendOutTo(Stream);
					flag3 = true;
				}
				catch (Exception ex)
				{
					Close();
					if (base.State == HTTPConnectionStates.TimedOut || base.State == HTTPConnectionStates.AbortRequested)
					{
						throw new Exception("AbortRequested");
					}
					if (flag || base.CurrentRequest.DisableRetry)
					{
						throw ex;
					}
					flag = true;
					retryCauses = RetryCauses.Reconnect;
				}
				if (!flag3)
				{
					continue;
				}
				bool num = Receive();
				if (base.State == HTTPConnectionStates.TimedOut || base.State == HTTPConnectionStates.AbortRequested)
				{
					throw new Exception("AbortRequested");
				}
				if (!num && !flag && !base.CurrentRequest.DisableRetry)
				{
					flag = true;
					retryCauses = RetryCauses.Reconnect;
				}
				if (base.CurrentRequest.Response == null)
				{
					continue;
				}
				if (base.CurrentRequest.IsCookiesEnabled)
				{
					CookieJar.Set(base.CurrentRequest.Response);
					CookieJar.Persist();
				}
				switch (base.CurrentRequest.Response.StatusCode)
				{
				case 401:
				{
					string text2 = DigestStore.FindBest(base.CurrentRequest.Response.GetHeaderValues("www-authenticate"));
					if (!string.IsNullOrEmpty(text2))
					{
						Digest orCreate2 = DigestStore.GetOrCreate(base.CurrentRequest.CurrentUri);
						orCreate2.ParseChallange(text2);
						if (base.CurrentRequest.Credentials != null && orCreate2.IsUriProtected(base.CurrentRequest.CurrentUri) && (!base.CurrentRequest.HasHeader("Authorization") || orCreate2.Stale))
						{
							retryCauses = RetryCauses.Authenticate;
						}
					}
					break;
				}
				case 407:
				{
					if (!base.CurrentRequest.HasProxy)
					{
						break;
					}
					string text = DigestStore.FindBest(base.CurrentRequest.Response.GetHeaderValues("proxy-authenticate"));
					if (!string.IsNullOrEmpty(text))
					{
						Digest orCreate = DigestStore.GetOrCreate(base.CurrentRequest.Proxy.Address);
						orCreate.ParseChallange(text);
						if (base.CurrentRequest.Proxy.Credentials != null && orCreate.IsUriProtected(base.CurrentRequest.Proxy.Address) && (!base.CurrentRequest.HasHeader("Proxy-Authorization") || orCreate.Stale))
						{
							retryCauses = RetryCauses.ProxyAuthenticate;
						}
					}
					break;
				}
				case 301:
				case 302:
				case 307:
				case 308:
					if (base.CurrentRequest.RedirectCount < base.CurrentRequest.MaxRedirects)
					{
						base.CurrentRequest.RedirectCount++;
						string firstHeaderValue = base.CurrentRequest.Response.GetFirstHeaderValue("location");
						if (string.IsNullOrEmpty(firstHeaderValue))
						{
							throw new MissingFieldException($"Got redirect status({base.CurrentRequest.Response.StatusCode.ToString()}) without 'location' header!");
						}
						Uri redirectUri = GetRedirectUri(firstHeaderValue);
						if (HTTPManager.Logger.Level == Loglevels.All)
						{
							HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - Redirected to Location: '{1}' redirectUri: '{1}'", base.CurrentRequest.CurrentUri.ToString(), firstHeaderValue, redirectUri));
						}
						if (!base.CurrentRequest.CallOnBeforeRedirection(redirectUri))
						{
							HTTPManager.Logger.Information("HTTPConnection", "OnBeforeRedirection returned False");
							break;
						}
						base.CurrentRequest.RemoveHeader("Host");
						base.CurrentRequest.SetHeader("Referer", base.CurrentRequest.CurrentUri.ToString());
						base.CurrentRequest.RedirectUri = redirectUri;
						base.CurrentRequest.Response = null;
						bool flag4 = (base.CurrentRequest.IsRedirected = true);
						flag2 = flag4;
					}
					break;
				}
				TryStoreInCache();
				if (base.CurrentRequest.Response != null && base.CurrentRequest.Response.IsClosedManually)
				{
					continue;
				}
				bool num2 = base.CurrentRequest.Response == null || base.CurrentRequest.Response.HasHeaderWithValue("connection", "close");
				bool flag6 = !base.CurrentRequest.IsKeepAlive;
				if (num2 || flag6)
				{
					Close();
				}
				else
				{
					if (base.CurrentRequest.Response == null)
					{
						continue;
					}
					List<string> headerValues = base.CurrentRequest.Response.GetHeaderValues("keep-alive");
					if (headerValues != null && headerValues.Count > 0)
					{
						if (KeepAlive == null)
						{
							KeepAlive = new KeepAliveHeader();
						}
						KeepAlive.Parse(headerValues);
					}
				}
			}
			while (retryCauses != RetryCauses.None);
		}
		catch (TimeoutException exception)
		{
			base.CurrentRequest.Response = null;
			base.CurrentRequest.Exception = exception;
			base.CurrentRequest.State = HTTPRequestStates.ConnectionTimedOut;
			Close();
		}
		catch (Exception exception2)
		{
			if (base.CurrentRequest != null)
			{
				if (base.CurrentRequest.UseStreaming)
				{
					HTTPCacheService.DeleteEntity(base.CurrentRequest.CurrentUri);
				}
				base.CurrentRequest.Response = null;
				switch (base.State)
				{
				case HTTPConnectionStates.AbortRequested:
				case HTTPConnectionStates.Closed:
					base.CurrentRequest.State = HTTPRequestStates.Aborted;
					break;
				case HTTPConnectionStates.TimedOut:
					base.CurrentRequest.State = HTTPRequestStates.TimedOut;
					break;
				default:
					base.CurrentRequest.Exception = exception2;
					base.CurrentRequest.State = HTTPRequestStates.Error;
					break;
				}
			}
			Close();
		}
		finally
		{
			if (base.CurrentRequest != null)
			{
				lock (HTTPManager.Locker)
				{
					if (base.CurrentRequest != null && base.CurrentRequest.Response != null && base.CurrentRequest.Response.IsUpgraded)
					{
						base.State = HTTPConnectionStates.Upgraded;
					}
					else
					{
						base.State = (flag2 ? HTTPConnectionStates.Redirected : ((Client == null) ? HTTPConnectionStates.Closed : HTTPConnectionStates.WaitForRecycle));
					}
					if (base.CurrentRequest != null && base.CurrentRequest.State == HTTPRequestStates.Processing && (base.State == HTTPConnectionStates.Closed || base.State == HTTPConnectionStates.WaitForRecycle))
					{
						if (base.CurrentRequest.Response != null)
						{
							base.CurrentRequest.State = HTTPRequestStates.Finished;
						}
						else
						{
							base.CurrentRequest.Exception = new Exception($"Remote server closed the connection before sending response header! Previous request state: {base.CurrentRequest.State.ToString()}. Connection state: {base.State.ToString()}");
							base.CurrentRequest.State = HTTPRequestStates.Error;
						}
					}
					if (base.CurrentRequest != null && base.CurrentRequest.State == HTTPRequestStates.ConnectionTimedOut)
					{
						base.State = HTTPConnectionStates.Closed;
					}
					LastProcessTime = DateTime.UtcNow;
					if (OnConnectionRecycled != null)
					{
						RecycleNow();
					}
				}
			}
		}
	}

	private void Connect()
	{
		Uri uri = (base.CurrentRequest.HasProxy ? base.CurrentRequest.Proxy.Address : base.CurrentRequest.CurrentUri);
		if (Client == null)
		{
			Client = new TcpClient();
		}
		if (!Client.Connected)
		{
			Client.ConnectTimeout = base.CurrentRequest.ConnectTimeout;
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("HTTPConnection", $"'{base.CurrentRequest.CurrentUri.ToString()}' - Connecting to {uri.Host}:{uri.Port.ToString()}");
			}
			Client.SendBufferSize = HTTPManager.SendBufferSize;
			Client.ReceiveBufferSize = HTTPManager.ReceiveBufferSize;
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("HTTPConnection", $"'{base.CurrentRequest.CurrentUri.ToString()}' - Buffer sizes - Send: {Client.SendBufferSize.ToString()} Receive: {Client.ReceiveBufferSize.ToString()} Blocking: {Client.Client.Blocking.ToString()}");
			}
			Client.Connect(uri.Host, uri.Port);
			if ((int)HTTPManager.Logger.Level <= 1)
			{
				HTTPManager.Logger.Information("HTTPConnection", "Connected to " + uri.Host + ":" + uri.Port);
			}
		}
		else if ((int)HTTPManager.Logger.Level <= 1)
		{
			HTTPManager.Logger.Information("HTTPConnection", "Already connected to " + uri.Host + ":" + uri.Port);
		}
		base.StartTime = DateTime.UtcNow;
		if (Stream != null)
		{
			return;
		}
		bool num = HTTPProtocolFactory.IsSecureProtocol(base.CurrentRequest.CurrentUri);
		Stream = Client.GetStream();
		if (base.CurrentRequest.Proxy != null)
		{
			base.CurrentRequest.Proxy.Connect(Stream, base.CurrentRequest);
		}
		if (!num)
		{
			return;
		}
		if (base.CurrentRequest.UseAlternateSSL)
		{
			TlsClientProtocol tlsClientProtocol = new TlsClientProtocol(Client.GetStream(), new SecureRandom());
			List<string> list = base.CurrentRequest.CustomTLSServerNameList;
			if ((list == null || list.Count == 0) && !base.CurrentRequest.CurrentUri.IsHostIsAnIPAddress())
			{
				list = new List<string>(1);
				list.Add(base.CurrentRequest.CurrentUri.Host);
			}
			Uri currentUri = base.CurrentRequest.CurrentUri;
			ICertificateVerifyer verifyer;
			if (base.CurrentRequest.CustomCertificateVerifyer != null)
			{
				verifyer = base.CurrentRequest.CustomCertificateVerifyer;
			}
			else
			{
				ICertificateVerifyer certificateVerifyer = new AlwaysValidVerifyer();
				verifyer = certificateVerifyer;
			}
			tlsClientProtocol.Connect(new LegacyTlsClient(currentUri, verifyer, base.CurrentRequest.CustomClientCredentialsProvider, list));
			Stream = tlsClientProtocol.Stream;
		}
		else
		{
			SslStream sslStream = new SslStream(Client.GetStream(), leaveInnerStreamOpen: false, (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) => base.CurrentRequest.CallCustomCertificationValidator(cert, chain));
			if (!sslStream.IsAuthenticated)
			{
				sslStream.AuthenticateAsClient(base.CurrentRequest.CurrentUri.Host);
			}
			Stream = sslStream;
		}
	}

	private bool Receive()
	{
		SupportedProtocols protocol = ((base.CurrentRequest.ProtocolHandler == SupportedProtocols.Unknown) ? HTTPProtocolFactory.GetProtocolFromUri(base.CurrentRequest.CurrentUri) : base.CurrentRequest.ProtocolHandler);
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Verbose("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - Receive - protocol: {protocol.ToString()}");
		}
		base.CurrentRequest.Response = HTTPProtocolFactory.Get(protocol, base.CurrentRequest, Stream, base.CurrentRequest.UseStreaming, isFromCache: false);
		if (!base.CurrentRequest.Response.Receive())
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - Receive - Failed! Response will be null, returning with false.");
			}
			base.CurrentRequest.Response = null;
			return false;
		}
		if (base.CurrentRequest.Response.StatusCode == 304 && !base.CurrentRequest.DisableCache)
		{
			if (base.CurrentRequest.IsRedirected)
			{
				if (!LoadFromCache(base.CurrentRequest.RedirectUri))
				{
					LoadFromCache(base.CurrentRequest.Uri);
				}
			}
			else
			{
				LoadFromCache(base.CurrentRequest.Uri);
			}
		}
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Verbose("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - Receive - Finished Successfully!");
		}
		return true;
	}

	private bool LoadFromCache(Uri uri)
	{
		if (HTTPManager.Logger.Level == Loglevels.All)
		{
			HTTPManager.Logger.Verbose("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - LoadFromCache for Uri: {uri.ToString()}");
		}
		HTTPCacheFileInfo entity = HTTPCacheService.GetEntity(uri);
		if (entity == null)
		{
			HTTPManager.Logger.Warning("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - LoadFromCache for Uri: {uri.ToString()} - Cached entity not found!");
			return false;
		}
		base.CurrentRequest.Response.CacheFileInfo = entity;
		int length;
		using (Stream stream = entity.GetBodyStream(out length))
		{
			if (stream == null)
			{
				return false;
			}
			if (!base.CurrentRequest.Response.HasHeader("content-length"))
			{
				base.CurrentRequest.Response.Headers.Add("content-length", new List<string>(1) { length.ToString() });
			}
			base.CurrentRequest.Response.IsFromCache = true;
			if (!base.CurrentRequest.CacheOnly)
			{
				base.CurrentRequest.Response.ReadRaw(stream, length);
			}
		}
		return true;
	}

	private bool TryLoadAllFromCache()
	{
		if (base.CurrentRequest.DisableCache || !HTTPCacheService.IsSupported)
		{
			return false;
		}
		try
		{
			if (HTTPCacheService.IsCachedEntityExpiresInTheFuture(base.CurrentRequest))
			{
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					HTTPManager.Logger.Verbose("HTTPConnection", $"{base.CurrentRequest.CurrentUri.ToString()} - TryLoadAllFromCache - whole response loading from cache");
				}
				base.CurrentRequest.Response = HTTPCacheService.GetFullResponse(base.CurrentRequest);
				if (base.CurrentRequest.Response != null)
				{
					return true;
				}
			}
		}
		catch
		{
			HTTPCacheService.DeleteEntity(base.CurrentRequest.CurrentUri);
		}
		return false;
	}

	private void TryStoreInCache()
	{
		if (!base.CurrentRequest.UseStreaming && !base.CurrentRequest.DisableCache && base.CurrentRequest.Response != null && HTTPCacheService.IsSupported && HTTPCacheService.IsCacheble(base.CurrentRequest.CurrentUri, base.CurrentRequest.MethodType, base.CurrentRequest.Response))
		{
			if (base.CurrentRequest.IsRedirected)
			{
				HTTPCacheService.Store(base.CurrentRequest.Uri, base.CurrentRequest.MethodType, base.CurrentRequest.Response);
			}
			else
			{
				HTTPCacheService.Store(base.CurrentRequest.CurrentUri, base.CurrentRequest.MethodType, base.CurrentRequest.Response);
			}
			HTTPCacheService.SaveLibrary();
		}
	}

	private Uri GetRedirectUri(string location)
	{
		Uri uri = null;
		try
		{
			uri = new Uri(location);
			if (uri.IsFile || uri.AbsolutePath == location)
			{
				uri = null;
			}
		}
		catch (UriFormatException)
		{
			uri = null;
		}
		if (uri == null)
		{
			Uri uri2 = base.CurrentRequest.Uri;
			uri = new UriBuilder(uri2.Scheme, uri2.Host, uri2.Port, location).Uri;
		}
		return uri;
	}

	internal override void Abort(HTTPConnectionStates newState)
	{
		base.State = newState;
		HTTPConnectionStates state = base.State;
		if (state == HTTPConnectionStates.TimedOut)
		{
			base.TimedOutStart = DateTime.UtcNow;
		}
		if (Stream != null)
		{
			try
			{
				Stream.Dispose();
			}
			catch
			{
			}
		}
	}

	private void Close()
	{
		KeepAlive = null;
		base.LastProcessedUri = null;
		if (Client == null)
		{
			return;
		}
		try
		{
			Client.Close();
		}
		catch
		{
		}
		finally
		{
			Stream = null;
			Client = null;
		}
	}

	protected override void Dispose(bool disposing)
	{
		Close();
		base.Dispose(disposing);
	}
}
