using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using BestHTTP.Authentication;
using BestHTTP.Cookies;
using BestHTTP.Extensions;
using BestHTTP.Forms;
using Org.BouncyCastle.Crypto.Tls;

namespace BestHTTP;

public sealed class HTTPRequest : IEnumerator, IEnumerator<HTTPRequest>, IDisposable
{
	public static readonly byte[] EOL = new byte[2] { 13, 10 };

	public static readonly string[] MethodNames = new string[8]
	{
		HTTPMethods.Get.ToString().ToUpper(),
		HTTPMethods.Head.ToString().ToUpper(),
		HTTPMethods.Post.ToString().ToUpper(),
		HTTPMethods.Put.ToString().ToUpper(),
		HTTPMethods.Delete.ToString().ToUpper(),
		HTTPMethods.Patch.ToString().ToUpper(),
		HTTPMethods.Merge.ToString().ToUpper(),
		HTTPMethods.Options.ToString().ToUpper()
	};

	public static int UploadChunkSize = 2048;

	public OnUploadProgressDelegate OnUploadProgress;

	public OnDownloadProgressDelegate OnProgress;

	public OnRequestFinishedDelegate OnUpgraded;

	private List<Cookie> customCookies;

	private OnBeforeRedirectionDelegate onBeforeRedirection;

	private OnBeforeHeaderSendDelegate _onBeforeHeaderSend;

	private bool isKeepAlive;

	private bool disableCache;

	private bool cacheOnly;

	private int streamFragmentSize;

	private bool useStreaming;

	private HTTPFormBase FieldCollector;

	private HTTPFormBase FormImpl;

	public Uri Uri { get; set; }

	public HTTPMethods MethodType { get; set; }

	public byte[] RawData { get; set; }

	public Stream UploadStream { get; set; }

	public bool DisposeUploadStream { get; set; }

	public bool UseUploadStreamLength { get; set; }

	public bool IsKeepAlive
	{
		get
		{
			return isKeepAlive;
		}
		set
		{
			if (State == HTTPRequestStates.Processing)
			{
				throw new NotSupportedException("Changing the IsKeepAlive property while processing the request is not supported.");
			}
			isKeepAlive = value;
		}
	}

	public bool DisableCache
	{
		get
		{
			return disableCache;
		}
		set
		{
			if (State == HTTPRequestStates.Processing)
			{
				throw new NotSupportedException("Changing the DisableCache property while processing the request is not supported.");
			}
			disableCache = value;
		}
	}

	public bool CacheOnly
	{
		get
		{
			return cacheOnly;
		}
		set
		{
			if (State == HTTPRequestStates.Processing)
			{
				throw new NotSupportedException("Changing the CacheOnly property while processing the request is not supported.");
			}
			cacheOnly = value;
		}
	}

	public bool UseStreaming
	{
		get
		{
			return useStreaming;
		}
		set
		{
			if (State == HTTPRequestStates.Processing)
			{
				throw new NotSupportedException("Changing the UseStreaming property while processing the request is not supported.");
			}
			useStreaming = value;
		}
	}

	public int StreamFragmentSize
	{
		get
		{
			return streamFragmentSize;
		}
		set
		{
			if (State == HTTPRequestStates.Processing)
			{
				throw new NotSupportedException("Changing the StreamFragmentSize property while processing the request is not supported.");
			}
			if (value < 1)
			{
				throw new ArgumentException("StreamFragmentSize must be at least 1.");
			}
			streamFragmentSize = value;
		}
	}

	public int MaxFragmentQueueLength { get; set; }

	public OnRequestFinishedDelegate Callback { get; set; }

	public bool DisableRetry { get; set; }

	public bool IsRedirected { get; internal set; }

	public Uri RedirectUri { get; internal set; }

	public Uri CurrentUri
	{
		get
		{
			if (!IsRedirected)
			{
				return Uri;
			}
			return RedirectUri;
		}
	}

	public HTTPResponse Response { get; internal set; }

	public HTTPResponse ProxyResponse { get; internal set; }

	public Exception Exception { get; internal set; }

	public object Tag { get; set; }

	public Credentials Credentials { get; set; }

	public bool HasProxy => Proxy != null;

	public Proxy Proxy { get; set; }

	public int MaxRedirects { get; set; }

	public bool UseAlternateSSL { get; set; }

	public bool IsCookiesEnabled { get; set; }

	public List<Cookie> Cookies
	{
		get
		{
			if (customCookies == null)
			{
				customCookies = new List<Cookie>();
			}
			return customCookies;
		}
		set
		{
			customCookies = value;
		}
	}

	public HTTPFormUsage FormUsage { get; set; }

	public HTTPRequestStates State { get; internal set; }

	public int RedirectCount { get; internal set; }

	public TimeSpan ConnectTimeout { get; set; }

	public TimeSpan Timeout { get; set; }

	public bool EnableTimoutForStreaming { get; set; }

	public bool EnableSafeReadOnUnknownContentLength { get; set; }

	public int Priority { get; set; }

	public ICertificateVerifyer CustomCertificateVerifyer { get; set; }

	public IClientCredentialsProvider CustomClientCredentialsProvider { get; set; }

	public List<string> CustomTLSServerNameList { get; set; }

	public SupportedProtocols ProtocolHandler { get; set; }

	public bool TryToMinimizeTCPLatency { get; set; }

	internal long Downloaded { get; set; }

	internal long DownloadLength { get; set; }

	internal bool DownloadProgressChanged { get; set; }

	internal long UploadStreamLength
	{
		get
		{
			if (UploadStream == null || !UseUploadStreamLength)
			{
				return -1L;
			}
			try
			{
				return UploadStream.Length;
			}
			catch
			{
				return -1L;
			}
		}
	}

	internal long Uploaded { get; set; }

	internal long UploadLength { get; set; }

	internal bool UploadProgressChanged { get; set; }

	private Dictionary<string, List<string>> Headers { get; set; }

	public object Current => null;

	HTTPRequest IEnumerator<HTTPRequest>.Current => this;

	public event Func<HTTPRequest, X509Certificate, X509Chain, bool> CustomCertificationValidator;

	public event OnBeforeRedirectionDelegate OnBeforeRedirection
	{
		add
		{
			onBeforeRedirection = (OnBeforeRedirectionDelegate)Delegate.Combine(onBeforeRedirection, value);
		}
		remove
		{
			onBeforeRedirection = (OnBeforeRedirectionDelegate)Delegate.Remove(onBeforeRedirection, value);
		}
	}

	public event OnBeforeHeaderSendDelegate OnBeforeHeaderSend
	{
		add
		{
			_onBeforeHeaderSend = (OnBeforeHeaderSendDelegate)Delegate.Combine(_onBeforeHeaderSend, value);
		}
		remove
		{
			_onBeforeHeaderSend = (OnBeforeHeaderSendDelegate)Delegate.Remove(_onBeforeHeaderSend, value);
		}
	}

	public HTTPRequest(Uri uri)
		: this(uri, HTTPMethods.Get, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled, null)
	{
	}

	public HTTPRequest(Uri uri, OnRequestFinishedDelegate callback)
		: this(uri, HTTPMethods.Get, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled, callback)
	{
	}

	public HTTPRequest(Uri uri, bool isKeepAlive, OnRequestFinishedDelegate callback)
		: this(uri, HTTPMethods.Get, isKeepAlive, HTTPManager.IsCachingDisabled, callback)
	{
	}

	public HTTPRequest(Uri uri, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback)
		: this(uri, HTTPMethods.Get, isKeepAlive, disableCache, callback)
	{
	}

	public HTTPRequest(Uri uri, HTTPMethods methodType)
		: this(uri, methodType, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled || methodType != HTTPMethods.Get, null)
	{
	}

	public HTTPRequest(Uri uri, HTTPMethods methodType, OnRequestFinishedDelegate callback)
		: this(uri, methodType, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled || methodType != HTTPMethods.Get, callback)
	{
	}

	public HTTPRequest(Uri uri, HTTPMethods methodType, bool isKeepAlive, OnRequestFinishedDelegate callback)
		: this(uri, methodType, isKeepAlive, HTTPManager.IsCachingDisabled || methodType != HTTPMethods.Get, callback)
	{
	}

	public HTTPRequest(Uri uri, HTTPMethods methodType, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback)
	{
		Uri = uri;
		MethodType = methodType;
		IsKeepAlive = isKeepAlive;
		DisableCache = disableCache;
		Callback = callback;
		StreamFragmentSize = 4096;
		MaxFragmentQueueLength = 10;
		DisableRetry = methodType != HTTPMethods.Get;
		MaxRedirects = int.MaxValue;
		RedirectCount = 0;
		IsCookiesEnabled = HTTPManager.IsCookiesEnabled;
		Downloaded = (DownloadLength = 0L);
		DownloadProgressChanged = false;
		State = HTTPRequestStates.Initial;
		ConnectTimeout = HTTPManager.ConnectTimeout;
		Timeout = HTTPManager.RequestTimeout;
		EnableTimoutForStreaming = false;
		EnableSafeReadOnUnknownContentLength = true;
		Proxy = HTTPManager.Proxy;
		UseUploadStreamLength = true;
		DisposeUploadStream = true;
		CustomCertificateVerifyer = HTTPManager.DefaultCertificateVerifyer;
		CustomClientCredentialsProvider = HTTPManager.DefaultClientCredentialsProvider;
		UseAlternateSSL = HTTPManager.UseAlternateSSLDefaultValue;
		CustomCertificationValidator += HTTPManager.DefaultCertificationValidator;
		TryToMinimizeTCPLatency = HTTPManager.TryToMinimizeTCPLatency;
	}

	public void AddField(string fieldName, string value)
	{
		AddField(fieldName, value, Encoding.UTF8);
	}

	public void AddField(string fieldName, string value, Encoding e)
	{
		if (FieldCollector == null)
		{
			FieldCollector = new HTTPFormBase();
		}
		FieldCollector.AddField(fieldName, value, e);
	}

	public void AddBinaryData(string fieldName, byte[] content)
	{
		AddBinaryData(fieldName, content, null, null);
	}

	public void AddBinaryData(string fieldName, byte[] content, string fileName)
	{
		AddBinaryData(fieldName, content, fileName, null);
	}

	public void AddBinaryData(string fieldName, byte[] content, string fileName, string mimeType)
	{
		if (FieldCollector == null)
		{
			FieldCollector = new HTTPFormBase();
		}
		FieldCollector.AddBinaryData(fieldName, content, fileName, mimeType);
	}

	public void SetForm(HTTPFormBase form)
	{
		FormImpl = form;
	}

	public List<HTTPFieldData> GetFormFields()
	{
		if (FieldCollector == null || FieldCollector.IsEmpty)
		{
			return null;
		}
		return new List<HTTPFieldData>(FieldCollector.Fields);
	}

	public void ClearForm()
	{
		FormImpl = null;
		FieldCollector = null;
	}

	private HTTPFormBase SelectFormImplementation()
	{
		if (FormImpl != null)
		{
			return FormImpl;
		}
		if (FieldCollector == null)
		{
			return null;
		}
		switch (FormUsage)
		{
		case HTTPFormUsage.Automatic:
			if (!FieldCollector.HasBinary && !FieldCollector.HasLongValue)
			{
				goto case HTTPFormUsage.UrlEncoded;
			}
			goto case HTTPFormUsage.Multipart;
		case HTTPFormUsage.UrlEncoded:
			FormImpl = new HTTPUrlEncodedForm();
			break;
		case HTTPFormUsage.Multipart:
			FormImpl = new HTTPMultiPartForm();
			break;
		case HTTPFormUsage.RawJSon:
			FormImpl = new RawJsonForm();
			break;
		}
		FormImpl.CopyFrom(FieldCollector);
		return FormImpl;
	}

	public void AddHeader(string name, string value)
	{
		if (Headers == null)
		{
			Headers = new Dictionary<string, List<string>>();
		}
		if (!Headers.TryGetValue(name, out var value2))
		{
			Headers.Add(name, value2 = new List<string>(1));
		}
		value2.Add(value);
	}

	public void SetHeader(string name, string value)
	{
		if (Headers == null)
		{
			Headers = new Dictionary<string, List<string>>();
		}
		if (!Headers.TryGetValue(name, out var value2))
		{
			Headers.Add(name, value2 = new List<string>(1));
		}
		value2.Clear();
		value2.Add(value);
	}

	public bool RemoveHeader(string name)
	{
		if (Headers == null)
		{
			return false;
		}
		return Headers.Remove(name);
	}

	public bool HasHeader(string name)
	{
		if (Headers != null)
		{
			return Headers.ContainsKey(name);
		}
		return false;
	}

	public string GetFirstHeaderValue(string name)
	{
		if (Headers == null)
		{
			return null;
		}
		List<string> value = null;
		if (Headers.TryGetValue(name, out value) && value.Count > 0)
		{
			return value[0];
		}
		return null;
	}

	public List<string> GetHeaderValues(string name)
	{
		if (Headers == null)
		{
			return null;
		}
		List<string> value = null;
		if (Headers.TryGetValue(name, out value) && value.Count > 0)
		{
			return value;
		}
		return null;
	}

	public void RemoveHeaders()
	{
		if (Headers != null)
		{
			Headers.Clear();
		}
	}

	public void SetRangeHeader(int firstBytePos)
	{
		SetHeader("Range", $"bytes={firstBytePos}-");
	}

	public void SetRangeHeader(int firstBytePos, int lastBytePos)
	{
		SetHeader("Range", $"bytes={firstBytePos}-{lastBytePos}");
	}

	public void EnumerateHeaders(OnHeaderEnumerationDelegate callback)
	{
		EnumerateHeaders(callback, callBeforeSendCallback: false);
	}

	public void EnumerateHeaders(OnHeaderEnumerationDelegate callback, bool callBeforeSendCallback)
	{
		if (!HasHeader("Host"))
		{
			if (CurrentUri.Port == 80 || CurrentUri.Port == 443)
			{
				SetHeader("Host", CurrentUri.Host);
			}
			else
			{
				SetHeader("Host", CurrentUri.Authority);
			}
		}
		if (IsRedirected && !HasHeader("Referer"))
		{
			AddHeader("Referer", Uri.ToString());
		}
		if (!HasHeader("Accept-Encoding"))
		{
			AddHeader("Accept-Encoding", "gzip, identity");
		}
		if (HasProxy && !HasHeader("Proxy-Connection"))
		{
			AddHeader("Proxy-Connection", IsKeepAlive ? "Keep-Alive" : "Close");
		}
		if (!HasHeader("Connection"))
		{
			AddHeader("Connection", IsKeepAlive ? "Keep-Alive, TE" : "Close, TE");
		}
		if (!HasHeader("TE"))
		{
			AddHeader("TE", "identity");
		}
		if (!HasHeader("User-Agent"))
		{
			AddHeader("User-Agent", "BestHTTP");
		}
		long num = -1L;
		if (UploadStream == null)
		{
			byte[] entityBody = GetEntityBody();
			num = ((entityBody != null) ? entityBody.Length : 0);
			if (RawData == null && (FormImpl != null || (FieldCollector != null && !FieldCollector.IsEmpty)))
			{
				SelectFormImplementation();
				if (FormImpl != null)
				{
					FormImpl.PrepareRequest(this);
				}
			}
		}
		else
		{
			num = UploadStreamLength;
			if (num == -1)
			{
				SetHeader("Transfer-Encoding", "Chunked");
			}
			if (!HasHeader("Content-Type"))
			{
				SetHeader("Content-Type", "application/octet-stream");
			}
		}
		if (num >= 0 && !HasHeader("Content-Length"))
		{
			SetHeader("Content-Length", num.ToString());
		}
		if (HasProxy && Proxy.Credentials != null)
		{
			switch (Proxy.Credentials.Type)
			{
			case AuthenticationTypes.Basic:
				SetHeader("Proxy-Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Proxy.Credentials.UserName + ":" + Proxy.Credentials.Password)));
				break;
			case AuthenticationTypes.Unknown:
			case AuthenticationTypes.Digest:
			{
				Digest digest = DigestStore.Get(Proxy.Address);
				if (digest != null)
				{
					string value = digest.GenerateResponseHeader(this, Proxy.Credentials);
					if (!string.IsNullOrEmpty(value))
					{
						SetHeader("Proxy-Authorization", value);
					}
				}
				break;
			}
			}
		}
		if (Credentials != null)
		{
			switch (Credentials.Type)
			{
			case AuthenticationTypes.Basic:
				SetHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Credentials.UserName + ":" + Credentials.Password)));
				break;
			case AuthenticationTypes.Unknown:
			case AuthenticationTypes.Digest:
			{
				Digest digest2 = DigestStore.Get(CurrentUri);
				if (digest2 != null)
				{
					string value2 = digest2.GenerateResponseHeader(this, Credentials);
					if (!string.IsNullOrEmpty(value2))
					{
						SetHeader("Authorization", value2);
					}
				}
				break;
			}
			}
		}
		List<Cookie> list = (IsCookiesEnabled ? CookieJar.Get(CurrentUri) : null);
		if (list == null || list.Count == 0)
		{
			list = customCookies;
		}
		else if (customCookies != null)
		{
			for (int i = 0; i < customCookies.Count; i++)
			{
				Cookie customCookie = customCookies[i];
				int num2 = list.FindIndex((Cookie c) => c.Name.Equals(customCookie.Name));
				if (num2 >= 0)
				{
					list[num2] = customCookie;
				}
				else
				{
					list.Add(customCookie);
				}
			}
		}
		if (list != null && list.Count > 0)
		{
			bool flag = true;
			string text = string.Empty;
			bool flag2 = HTTPProtocolFactory.IsSecureProtocol(CurrentUri);
			foreach (Cookie item in list)
			{
				if (!item.IsSecure || (item.IsSecure && flag2))
				{
					if (!flag)
					{
						text += "; ";
					}
					else
					{
						flag = false;
					}
					text += item.ToString();
					item.LastAccess = DateTime.UtcNow;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				SetHeader("Cookie", text);
			}
		}
		if (callBeforeSendCallback && _onBeforeHeaderSend != null)
		{
			try
			{
				_onBeforeHeaderSend(this);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HTTPRequest", "OnBeforeHeaderSend", ex);
			}
		}
		if (callback == null || Headers == null)
		{
			return;
		}
		foreach (KeyValuePair<string, List<string>> header in Headers)
		{
			callback(header.Key, header.Value);
		}
	}

	private void SendHeaders(Stream stream)
	{
		EnumerateHeaders(delegate(string header, List<string> values)
		{
			if (!string.IsNullOrEmpty(header) && values != null)
			{
				byte[] aSCIIBytes = (header + ": ").GetASCIIBytes();
				for (int i = 0; i < values.Count; i++)
				{
					if (string.IsNullOrEmpty(values[i]))
					{
						HTTPManager.Logger.Warning("HTTPRequest", $"Null/empty value for header: {header}");
					}
					else
					{
						if ((int)HTTPManager.Logger.Level <= 1)
						{
							VerboseLogging("Header - '" + header + "': '" + values[i] + "'");
						}
						byte[] aSCIIBytes2 = values[i].GetASCIIBytes();
						stream.WriteArray(aSCIIBytes);
						stream.WriteArray(aSCIIBytes2);
						stream.WriteArray(EOL);
						VariableSizedBufferPool.Release(aSCIIBytes2);
					}
				}
				VariableSizedBufferPool.Release(aSCIIBytes);
			}
		}, callBeforeSendCallback: true);
	}

	public string DumpHeaders()
	{
		using BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream(5120);
		SendHeaders(bufferPoolMemoryStream);
		return bufferPoolMemoryStream.ToArray().AsciiToString();
	}

	public byte[] GetEntityBody()
	{
		if (RawData != null)
		{
			return RawData;
		}
		if (FormImpl != null || (FieldCollector != null && !FieldCollector.IsEmpty))
		{
			SelectFormImplementation();
			if (FormImpl != null)
			{
				return FormImpl.GetData();
			}
		}
		return null;
	}

	internal void SendOutTo(Stream stream)
	{
		try
		{
			string arg = (HasProxy ? Proxy.GetRequestPath(CurrentUri) : CurrentUri.GetRequestPathAndQueryURL());
			string text = $"{MethodNames[(uint)MethodType]} {arg} HTTP/1.1";
			if ((int)HTTPManager.Logger.Level <= 1)
			{
				HTTPManager.Logger.Information("HTTPRequest", $"Sending request: '{text}'");
			}
			using (WriteOnlyBufferedStream writeOnlyBufferedStream = new WriteOnlyBufferedStream(stream, (int)((float)UploadChunkSize * 1.5f)))
			{
				byte[] aSCIIBytes = text.GetASCIIBytes();
				writeOnlyBufferedStream.WriteArray(aSCIIBytes);
				writeOnlyBufferedStream.WriteArray(EOL);
				VariableSizedBufferPool.Release(aSCIIBytes);
				SendHeaders(writeOnlyBufferedStream);
				writeOnlyBufferedStream.WriteArray(EOL);
				writeOnlyBufferedStream.Flush();
				byte[] array = RawData;
				if (array == null && FormImpl != null)
				{
					array = FormImpl.GetData();
				}
				if (array != null || UploadStream != null)
				{
					Stream stream2 = UploadStream;
					if (stream2 == null)
					{
						stream2 = new MemoryStream(array, 0, array.Length);
						UploadLength = array.Length;
					}
					else
					{
						UploadLength = (UseUploadStreamLength ? UploadStreamLength : (-1));
					}
					Uploaded = 0L;
					byte[] array2 = VariableSizedBufferPool.Get(UploadChunkSize, canBeLarger: true);
					int num = 0;
					while ((num = stream2.Read(array2, 0, array2.Length)) > 0)
					{
						if (!UseUploadStreamLength)
						{
							byte[] aSCIIBytes2 = num.ToString("X").GetASCIIBytes();
							writeOnlyBufferedStream.WriteArray(aSCIIBytes2);
							writeOnlyBufferedStream.WriteArray(EOL);
							VariableSizedBufferPool.Release(aSCIIBytes2);
						}
						writeOnlyBufferedStream.Write(array2, 0, num);
						if (!UseUploadStreamLength)
						{
							writeOnlyBufferedStream.WriteArray(EOL);
						}
						Uploaded += num;
						writeOnlyBufferedStream.Flush();
						UploadProgressChanged = true;
					}
					VariableSizedBufferPool.Release(array2);
					if (!UseUploadStreamLength)
					{
						byte[] array3 = VariableSizedBufferPool.Get(1L, canBeLarger: true);
						array3[0] = 48;
						writeOnlyBufferedStream.Write(array3, 0, 1);
						writeOnlyBufferedStream.WriteArray(EOL);
						writeOnlyBufferedStream.WriteArray(EOL);
						VariableSizedBufferPool.Release(array3);
					}
					writeOnlyBufferedStream.Flush();
					if (UploadStream == null)
					{
						stream2?.Dispose();
					}
				}
				else
				{
					writeOnlyBufferedStream.Flush();
				}
			}
			HTTPManager.Logger.Information("HTTPRequest", "'" + text + "' sent out");
		}
		finally
		{
			if (UploadStream != null && DisposeUploadStream)
			{
				UploadStream.Dispose();
			}
		}
	}

	internal void UpgradeCallback()
	{
		if (Response == null || !Response.IsUpgraded)
		{
			return;
		}
		try
		{
			if (OnUpgraded != null)
			{
				OnUpgraded(this, Response);
			}
		}
		catch (Exception ex)
		{
			HTTPManager.Logger.Exception("HTTPRequest", "UpgradeCallback", ex);
		}
	}

	internal void CallCallback()
	{
		try
		{
			if (Callback != null)
			{
				Callback(this, Response);
			}
		}
		catch (Exception ex)
		{
			HTTPManager.Logger.Exception("HTTPRequest", "CallCallback", ex);
		}
	}

	internal bool CallOnBeforeRedirection(Uri redirectUri)
	{
		if (onBeforeRedirection != null)
		{
			return onBeforeRedirection(this, Response, redirectUri);
		}
		return true;
	}

	internal void FinishStreaming()
	{
		if (Response != null && UseStreaming)
		{
			Response.FinishStreaming();
		}
	}

	internal void Prepare()
	{
	}

	internal bool CallCustomCertificationValidator(X509Certificate cert, X509Chain chain)
	{
		if (this.CustomCertificationValidator != null)
		{
			return this.CustomCertificationValidator(this, cert, chain);
		}
		return true;
	}

	public HTTPRequest Send()
	{
		return HTTPManager.SendRequest(this);
	}

	public void Abort()
	{
		if (Monitor.TryEnter(HTTPManager.Locker, TimeSpan.FromMilliseconds(100.0)))
		{
			try
			{
				if (State >= HTTPRequestStates.Finished)
				{
					HTTPManager.Logger.Warning("HTTPRequest", $"Abort - Already in a state({State.ToString()}) where no Abort required!");
				}
				else
				{
					ConnectionBase connectionWith = HTTPManager.GetConnectionWith(this);
					if (connectionWith == null)
					{
						if (!HTTPManager.RemoveFromQueue(this))
						{
							HTTPManager.Logger.Warning("HTTPRequest", "Abort - No active connection found with this request! (The request may already finished?)");
						}
						State = HTTPRequestStates.Aborted;
						CallCallback();
					}
					else
					{
						if (Response != null && Response.IsStreamed)
						{
							Response.Dispose();
						}
						connectionWith.Abort(HTTPConnectionStates.AbortRequested);
					}
				}
				return;
			}
			finally
			{
				Monitor.Exit(HTTPManager.Locker);
			}
		}
		throw new Exception("Wasn't able to acquire a thread lock. Abort failed!");
	}

	public void Clear()
	{
		ClearForm();
		RemoveHeaders();
		IsRedirected = false;
		RedirectCount = 0;
		long downloaded = (DownloadLength = 0L);
		Downloaded = downloaded;
	}

	private void VerboseLogging(string str)
	{
		HTTPManager.Logger.Verbose("HTTPRequest", "'" + CurrentUri.ToString() + "' - " + str);
	}

	public bool MoveNext()
	{
		return State < HTTPRequestStates.Finished;
	}

	public void Reset()
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		if (Response != null)
		{
			Response.Dispose();
		}
	}
}
