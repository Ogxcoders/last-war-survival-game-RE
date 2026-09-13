using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Bindings;
using UnityEngineInternal;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequest/Public/UnityWebRequest.h")]
public class UnityWebRequest : IDisposable
{
	internal enum UnityWebRequestMethod
	{
		Get,
		Post,
		Put,
		Head,
		Custom
	}

	internal enum UnityWebRequestError
	{
		OK,
		Unknown,
		SDKError,
		UnsupportedProtocol,
		MalformattedUrl,
		CannotResolveProxy,
		CannotResolveHost,
		CannotConnectToHost,
		AccessDenied,
		GenericHttpError,
		WriteError,
		ReadError,
		OutOfMemory,
		Timeout,
		HTTPPostError,
		SSLCannotConnect,
		Aborted,
		TooManyRedirects,
		ReceivedNoData,
		SSLNotSupported,
		FailedToSendData,
		FailedToReceiveData,
		SSLCertificateError,
		SSLCipherNotAvailable,
		SSLCACertError,
		UnrecognizedContentEncoding,
		LoginFailed,
		SSLShutdownFailed,
		NoInternetConnection
	}

	[NonSerialized]
	internal IntPtr m_Ptr;

	[NonSerialized]
	internal DownloadHandler m_DownloadHandler;

	[NonSerialized]
	internal UploadHandler m_UploadHandler;

	[NonSerialized]
	internal CertificateHandler m_CertificateHandler;

	[NonSerialized]
	internal Uri m_Uri;

	public const string kHttpVerbGET = "GET";

	public const string kHttpVerbHEAD = "HEAD";

	public const string kHttpVerbPOST = "POST";

	public const string kHttpVerbPUT = "PUT";

	public const string kHttpVerbCREATE = "CREATE";

	public const string kHttpVerbDELETE = "DELETE";

	public bool disposeCertificateHandlerOnDispose { get; set; }

	public bool disposeDownloadHandlerOnDispose { get; set; }

	public bool disposeUploadHandlerOnDispose { get; set; }

	public string method
	{
		get
		{
			return GetMethod() switch
			{
				UnityWebRequestMethod.Get => "GET", 
				UnityWebRequestMethod.Post => "POST", 
				UnityWebRequestMethod.Put => "PUT", 
				UnityWebRequestMethod.Head => "HEAD", 
				_ => GetCustomMethod(), 
			};
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException("Cannot set a UnityWebRequest's method to an empty or null string");
			}
			switch (value.ToUpper())
			{
			case "GET":
				InternalSetMethod(UnityWebRequestMethod.Get);
				break;
			case "POST":
				InternalSetMethod(UnityWebRequestMethod.Post);
				break;
			case "PUT":
				InternalSetMethod(UnityWebRequestMethod.Put);
				break;
			case "HEAD":
				InternalSetMethod(UnityWebRequestMethod.Head);
				break;
			default:
				InternalSetCustomMethod(value.ToUpper());
				break;
			}
		}
	}

	public string error
	{
		get
		{
			if (!isNetworkError && !isHttpError)
			{
				return null;
			}
			if (isHttpError)
			{
				string hTTPStatusString = GetHTTPStatusString(responseCode);
				return $"HTTP/1.1 {responseCode} {hTTPStatusString}";
			}
			return GetWebErrorString(GetError());
		}
	}

	private extern bool use100Continue
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public bool useHttpContinue
	{
		get
		{
			return use100Continue;
		}
		set
		{
			if (!isModifiable)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent and its 100-Continue setting cannot be altered");
			}
			use100Continue = value;
		}
	}

	public string url
	{
		get
		{
			return GetUrl();
		}
		set
		{
			string localUrl = "http://localhost/";
			InternalSetUrl(WebRequestUtils.MakeInitialUrl(value, localUrl));
		}
	}

	public Uri uri
	{
		get
		{
			return new Uri(GetUrl());
		}
		set
		{
			if (!value.IsAbsoluteUri)
			{
				throw new ArgumentException("URI must be absolute");
			}
			InternalSetUrl(WebRequestUtils.MakeUriString(value, value.OriginalString, prependProtocol: false));
			m_Uri = value;
		}
	}

	public extern long responseCode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public float uploadProgress
	{
		get
		{
			if (!IsExecuting() && !isDone)
			{
				return -1f;
			}
			return GetUploadProgress();
		}
	}

	public extern bool isModifiable
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("IsModifiable")]
		get;
	}

	public extern bool isDone
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("IsDone")]
		get;
	}

	public extern bool isNetworkError
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("IsNetworkError")]
		get;
	}

	public extern bool isHttpError
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("IsHttpError")]
		get;
	}

	public float downloadProgress
	{
		get
		{
			if (!IsExecuting() && !isDone)
			{
				return -1f;
			}
			return GetDownloadProgress();
		}
	}

	public extern ulong uploadedBytes
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern ulong downloadedBytes
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public int redirectLimit
	{
		get
		{
			return GetRedirectLimit();
		}
		set
		{
			SetRedirectLimitFromScripting(value);
		}
	}

	[Obsolete("HTTP/2 and many HTTP/1.1 servers don't support this; we recommend leaving it set to false (default).", false)]
	public bool chunkedTransfer
	{
		get
		{
			return GetChunked();
		}
		set
		{
			if (!isModifiable)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent and its chunked transfer encoding setting cannot be altered");
			}
			UnityWebRequestError unityWebRequestError = SetChunked(value);
			if (unityWebRequestError != UnityWebRequestError.OK)
			{
				throw new InvalidOperationException(GetWebErrorString(unityWebRequestError));
			}
		}
	}

	public UploadHandler uploadHandler
	{
		get
		{
			return m_UploadHandler;
		}
		set
		{
			if (!isModifiable)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the upload handler");
			}
			UnityWebRequestError unityWebRequestError = SetUploadHandler(value);
			if (unityWebRequestError != UnityWebRequestError.OK)
			{
				throw new InvalidOperationException(GetWebErrorString(unityWebRequestError));
			}
			m_UploadHandler = value;
		}
	}

	public DownloadHandler downloadHandler
	{
		get
		{
			return m_DownloadHandler;
		}
		set
		{
			if (!isModifiable)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the download handler");
			}
			UnityWebRequestError unityWebRequestError = SetDownloadHandler(value);
			if (unityWebRequestError != UnityWebRequestError.OK)
			{
				throw new InvalidOperationException(GetWebErrorString(unityWebRequestError));
			}
			m_DownloadHandler = value;
		}
	}

	public CertificateHandler certificateHandler
	{
		get
		{
			return m_CertificateHandler;
		}
		set
		{
			if (!isModifiable)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the certificate handler");
			}
			UnityWebRequestError unityWebRequestError = SetCertificateHandler(value);
			if (unityWebRequestError != UnityWebRequestError.OK)
			{
				throw new InvalidOperationException(GetWebErrorString(unityWebRequestError));
			}
			m_CertificateHandler = value;
		}
	}

	public int timeout
	{
		get
		{
			return GetTimeoutMsec() / 1000;
		}
		set
		{
			if (!isModifiable)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the timeout");
			}
			value = Math.Max(value, 0);
			UnityWebRequestError unityWebRequestError = SetTimeoutMsec(value * 1000);
			if (unityWebRequestError != UnityWebRequestError.OK)
			{
				throw new InvalidOperationException(GetWebErrorString(unityWebRequestError));
			}
		}
	}

	internal bool suppressErrorsToConsole
	{
		get
		{
			return GetSuppressErrorsToConsole();
		}
		set
		{
			if (!isModifiable)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the timeout");
			}
			UnityWebRequestError unityWebRequestError = SetSuppressErrorsToConsole(value);
			if (unityWebRequestError != UnityWebRequestError.OK)
			{
				throw new InvalidOperationException(GetWebErrorString(unityWebRequestError));
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	[NativeConditional("ENABLE_UNITYWEBREQUEST")]
	private static extern string GetWebErrorString(UnityWebRequestError err);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules]
	internal static extern string GetHTTPStatusString(long responseCode);

	public static void ClearCookieCache()
	{
		ClearCookieCache(null, null);
	}

	public static void ClearCookieCache(Uri uri)
	{
		if (uri == null)
		{
			ClearCookieCache(null, null);
			return;
		}
		string host = uri.Host;
		string text = uri.AbsolutePath;
		if (text == "/")
		{
			text = null;
		}
		ClearCookieCache(host, text);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void ClearCookieCache(string domain, string path);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr Create();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private extern void Release();

	internal void InternalDestroy()
	{
		if (m_Ptr != IntPtr.Zero)
		{
			Abort();
			Release();
			m_Ptr = IntPtr.Zero;
		}
	}

	private void InternalSetDefaults()
	{
		disposeDownloadHandlerOnDispose = true;
		disposeUploadHandlerOnDispose = true;
		disposeCertificateHandlerOnDispose = true;
	}

	public UnityWebRequest()
	{
		m_Ptr = Create();
		InternalSetDefaults();
	}

	public UnityWebRequest(string url)
	{
		m_Ptr = Create();
		InternalSetDefaults();
		this.url = url;
	}

	public UnityWebRequest(Uri uri)
	{
		m_Ptr = Create();
		InternalSetDefaults();
		this.uri = uri;
	}

	public UnityWebRequest(string url, string method)
	{
		m_Ptr = Create();
		InternalSetDefaults();
		this.url = url;
		this.method = method;
	}

	public UnityWebRequest(Uri uri, string method)
	{
		m_Ptr = Create();
		InternalSetDefaults();
		this.uri = uri;
		this.method = method;
	}

	public UnityWebRequest(string url, string method, DownloadHandler downloadHandler, UploadHandler uploadHandler)
	{
		m_Ptr = Create();
		InternalSetDefaults();
		this.url = url;
		this.method = method;
		this.downloadHandler = downloadHandler;
		this.uploadHandler = uploadHandler;
	}

	public UnityWebRequest(Uri uri, string method, DownloadHandler downloadHandler, UploadHandler uploadHandler)
	{
		m_Ptr = Create();
		InternalSetDefaults();
		this.uri = uri;
		this.method = method;
		this.downloadHandler = downloadHandler;
		this.uploadHandler = uploadHandler;
	}

	~UnityWebRequest()
	{
		DisposeHandlers();
		InternalDestroy();
	}

	public void Dispose()
	{
		DisposeHandlers();
		InternalDestroy();
		GC.SuppressFinalize(this);
	}

	private void DisposeHandlers()
	{
		if (disposeDownloadHandlerOnDispose)
		{
			downloadHandler?.Dispose();
		}
		if (disposeUploadHandlerOnDispose)
		{
			uploadHandler?.Dispose();
		}
		if (disposeCertificateHandlerOnDispose)
		{
			certificateHandler?.Dispose();
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	internal extern UnityWebRequestAsyncOperation BeginWebRequest();

	[Obsolete("Use SendWebRequest.  It returns a UnityWebRequestAsyncOperation which contains a reference to the WebRequest object.", false)]
	public AsyncOperation Send()
	{
		return SendWebRequest();
	}

	public UnityWebRequestAsyncOperation SendWebRequest()
	{
		UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = BeginWebRequest();
		if (unityWebRequestAsyncOperation != null)
		{
			unityWebRequestAsyncOperation.webRequest = this;
		}
		return unityWebRequestAsyncOperation;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	public extern void Abort();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern UnityWebRequestError SetMethod(UnityWebRequestMethod methodType);

	internal void InternalSetMethod(UnityWebRequestMethod methodType)
	{
		if (!isModifiable)
		{
			throw new InvalidOperationException("UnityWebRequest has already been sent and its request method can no longer be altered");
		}
		UnityWebRequestError unityWebRequestError = SetMethod(methodType);
		if (unityWebRequestError != UnityWebRequestError.OK)
		{
			throw new InvalidOperationException(GetWebErrorString(unityWebRequestError));
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern UnityWebRequestError SetCustomMethod(string customMethodName);

	internal void InternalSetCustomMethod(string customMethodName)
	{
		if (!isModifiable)
		{
			throw new InvalidOperationException("UnityWebRequest has already been sent and its request method can no longer be altered");
		}
		UnityWebRequestError unityWebRequestError = SetCustomMethod(customMethodName);
		if (unityWebRequestError != UnityWebRequestError.OK)
		{
			throw new InvalidOperationException(GetWebErrorString(unityWebRequestError));
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern UnityWebRequestMethod GetMethod();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern string GetCustomMethod();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern UnityWebRequestError GetError();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern string GetUrl();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern UnityWebRequestError SetUrl(string url);

	private void InternalSetUrl(string url)
	{
		if (!isModifiable)
		{
			throw new InvalidOperationException("UnityWebRequest has already been sent and its URL cannot be altered");
		}
		UnityWebRequestError unityWebRequestError = SetUrl(url);
		if (unityWebRequestError != UnityWebRequestError.OK)
		{
			throw new InvalidOperationException(GetWebErrorString(unityWebRequestError));
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern float GetUploadProgress();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool IsExecuting();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern float GetDownloadProgress();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int GetRedirectLimit();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private extern void SetRedirectLimitFromScripting(int limit);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool GetChunked();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern UnityWebRequestError SetChunked(bool chunked);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern string GetRequestHeader(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("SetRequestHeader")]
	internal extern UnityWebRequestError InternalSetRequestHeader(string name, string value);

	public void SetRequestHeader(string name, string value)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("Cannot set a Request Header with a null or empty name");
		}
		if (value == null)
		{
			throw new ArgumentException("Cannot set a Request header with a null");
		}
		if (!isModifiable)
		{
			throw new InvalidOperationException("UnityWebRequest has already been sent and its request headers cannot be altered");
		}
		UnityWebRequestError unityWebRequestError = InternalSetRequestHeader(name, value);
		if (unityWebRequestError != UnityWebRequestError.OK)
		{
			throw new InvalidOperationException(GetWebErrorString(unityWebRequestError));
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern string GetResponseHeader(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern string[] GetResponseHeaderKeys();

	public Dictionary<string, string> GetResponseHeaders()
	{
		string[] responseHeaderKeys = GetResponseHeaderKeys();
		if (responseHeaderKeys == null || responseHeaderKeys.Length == 0)
		{
			return null;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>(responseHeaderKeys.Length, StringComparer.OrdinalIgnoreCase);
		for (int i = 0; i < responseHeaderKeys.Length; i++)
		{
			string responseHeader = GetResponseHeader(responseHeaderKeys[i]);
			dictionary.Add(responseHeaderKeys[i], responseHeader);
		}
		return dictionary;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern UnityWebRequestError SetUploadHandler(UploadHandler uh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern UnityWebRequestError SetDownloadHandler(DownloadHandler dh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern UnityWebRequestError SetCertificateHandler(CertificateHandler ch);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int GetTimeoutMsec();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern UnityWebRequestError SetTimeoutMsec(int timeout);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool GetSuppressErrorsToConsole();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern UnityWebRequestError SetSuppressErrorsToConsole(bool suppress);

	public static UnityWebRequest Get(string uri)
	{
		return new UnityWebRequest(uri, "GET", new DownloadHandlerBuffer(), null);
	}

	public static UnityWebRequest Get(Uri uri)
	{
		return new UnityWebRequest(uri, "GET", new DownloadHandlerBuffer(), null);
	}

	public static UnityWebRequest Delete(string uri)
	{
		return new UnityWebRequest(uri, "DELETE");
	}

	public static UnityWebRequest Delete(Uri uri)
	{
		return new UnityWebRequest(uri, "DELETE");
	}

	public static UnityWebRequest Head(string uri)
	{
		return new UnityWebRequest(uri, "HEAD");
	}

	public static UnityWebRequest Head(Uri uri)
	{
		return new UnityWebRequest(uri, "HEAD");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestTexture.GetTexture(*)", true)]
	public static UnityWebRequest GetTexture(string uri)
	{
		throw new NotSupportedException("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead.");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestTexture.GetTexture(*)", true)]
	public static UnityWebRequest GetTexture(string uri, bool nonReadable)
	{
		throw new NotSupportedException("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead.");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("UnityWebRequest.GetAudioClip is obsolete. Use UnityWebRequestMultimedia.GetAudioClip instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestMultimedia.GetAudioClip(*)", true)]
	public static UnityWebRequest GetAudioClip(string uri, AudioType audioType)
	{
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
	public static UnityWebRequest GetAssetBundle(string uri)
	{
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
	public static UnityWebRequest GetAssetBundle(string uri, uint crc)
	{
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
	public static UnityWebRequest GetAssetBundle(string uri, uint version, uint crc)
	{
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
	public static UnityWebRequest GetAssetBundle(string uri, Hash128 hash, uint crc)
	{
		return null;
	}

	[Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static UnityWebRequest GetAssetBundle(string uri, CachedAssetBundle cachedAssetBundle, uint crc)
	{
		return null;
	}

	public static UnityWebRequest Put(string uri, byte[] bodyData)
	{
		return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(bodyData));
	}

	public static UnityWebRequest Put(Uri uri, byte[] bodyData)
	{
		return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(bodyData));
	}

	public static UnityWebRequest Put(string uri, string bodyData)
	{
		return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyData)));
	}

	public static UnityWebRequest Put(Uri uri, string bodyData)
	{
		return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyData)));
	}

	public static UnityWebRequest Post(string uri, string postData)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
		SetupPost(unityWebRequest, postData);
		return unityWebRequest;
	}

	public static UnityWebRequest Post(Uri uri, string postData)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
		SetupPost(unityWebRequest, postData);
		return unityWebRequest;
	}

	private static void SetupPost(UnityWebRequest request, string postData)
	{
		byte[] data = null;
		if (!string.IsNullOrEmpty(postData))
		{
			string s = WWWTranscoder.DataEncode(postData, Encoding.UTF8);
			data = Encoding.UTF8.GetBytes(s);
		}
		request.uploadHandler = new UploadHandlerRaw(data);
		request.uploadHandler.contentType = "application/x-www-form-urlencoded";
		request.downloadHandler = new DownloadHandlerBuffer();
	}

	public static UnityWebRequest Post(string uri, WWWForm formData)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
		SetupPost(unityWebRequest, formData);
		return unityWebRequest;
	}

	public static UnityWebRequest Post(Uri uri, WWWForm formData)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
		SetupPost(unityWebRequest, formData);
		return unityWebRequest;
	}

	private static void SetupPost(UnityWebRequest request, WWWForm formData)
	{
		byte[] array = null;
		if (formData != null)
		{
			array = formData.data;
			if (array.Length == 0)
			{
				array = null;
			}
		}
		request.uploadHandler = new UploadHandlerRaw(array);
		request.downloadHandler = new DownloadHandlerBuffer();
		if (formData == null)
		{
			return;
		}
		Dictionary<string, string> headers = formData.headers;
		foreach (KeyValuePair<string, string> item in headers)
		{
			request.SetRequestHeader(item.Key, item.Value);
		}
	}

	public static UnityWebRequest Post(string uri, List<IMultipartFormSection> multipartFormSections)
	{
		byte[] boundary = GenerateBoundary();
		return Post(uri, multipartFormSections, boundary);
	}

	public static UnityWebRequest Post(Uri uri, List<IMultipartFormSection> multipartFormSections)
	{
		byte[] boundary = GenerateBoundary();
		return Post(uri, multipartFormSections, boundary);
	}

	public static UnityWebRequest Post(string uri, List<IMultipartFormSection> multipartFormSections, byte[] boundary)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
		SetupPost(unityWebRequest, multipartFormSections, boundary);
		return unityWebRequest;
	}

	public static UnityWebRequest Post(Uri uri, List<IMultipartFormSection> multipartFormSections, byte[] boundary)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
		SetupPost(unityWebRequest, multipartFormSections, boundary);
		return unityWebRequest;
	}

	private static void SetupPost(UnityWebRequest request, List<IMultipartFormSection> multipartFormSections, byte[] boundary)
	{
		byte[] data = null;
		if (multipartFormSections != null && multipartFormSections.Count != 0)
		{
			data = SerializeFormSections(multipartFormSections, boundary);
		}
		UploadHandler uploadHandler = new UploadHandlerRaw(data);
		uploadHandler.contentType = "multipart/form-data; boundary=" + Encoding.UTF8.GetString(boundary, 0, boundary.Length);
		request.uploadHandler = uploadHandler;
		request.downloadHandler = new DownloadHandlerBuffer();
	}

	public static UnityWebRequest Post(string uri, Dictionary<string, string> formFields)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
		SetupPost(unityWebRequest, formFields);
		return unityWebRequest;
	}

	public static UnityWebRequest Post(Uri uri, Dictionary<string, string> formFields)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
		SetupPost(unityWebRequest, formFields);
		return unityWebRequest;
	}

	private static void SetupPost(UnityWebRequest request, Dictionary<string, string> formFields)
	{
		byte[] data = null;
		if (formFields != null && formFields.Count != 0)
		{
			data = SerializeSimpleForm(formFields);
		}
		UploadHandler uploadHandler = new UploadHandlerRaw(data);
		uploadHandler.contentType = "application/x-www-form-urlencoded";
		request.uploadHandler = uploadHandler;
		request.downloadHandler = new DownloadHandlerBuffer();
	}

	public static string EscapeURL(string s)
	{
		return EscapeURL(s, Encoding.UTF8);
	}

	public static string EscapeURL(string s, Encoding e)
	{
		if (s == null)
		{
			return null;
		}
		if (s == "")
		{
			return "";
		}
		if (e == null)
		{
			return null;
		}
		byte[] bytes = e.GetBytes(s);
		byte[] bytes2 = WWWTranscoder.URLEncode(bytes);
		return e.GetString(bytes2);
	}

	public static string UnEscapeURL(string s)
	{
		return UnEscapeURL(s, Encoding.UTF8);
	}

	public static string UnEscapeURL(string s, Encoding e)
	{
		if (s == null)
		{
			return null;
		}
		if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
		{
			return s;
		}
		byte[] bytes = e.GetBytes(s);
		byte[] bytes2 = WWWTranscoder.URLDecode(bytes);
		return e.GetString(bytes2);
	}

	public static byte[] SerializeFormSections(List<IMultipartFormSection> multipartFormSections, byte[] boundary)
	{
		if (multipartFormSections == null || multipartFormSections.Count == 0)
		{
			return null;
		}
		byte[] bytes = Encoding.UTF8.GetBytes("\r\n");
		byte[] bytes2 = WWWForm.DefaultEncoding.GetBytes("--");
		int num = 0;
		foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
		{
			num += 64 + multipartFormSection.sectionData.Length;
		}
		List<byte> list = new List<byte>(num);
		foreach (IMultipartFormSection multipartFormSection2 in multipartFormSections)
		{
			string text = "form-data";
			string sectionName = multipartFormSection2.sectionName;
			string fileName = multipartFormSection2.fileName;
			string text2 = "Content-Disposition: " + text;
			if (!string.IsNullOrEmpty(sectionName))
			{
				text2 = text2 + "; name=\"" + sectionName + "\"";
			}
			if (!string.IsNullOrEmpty(fileName))
			{
				text2 = text2 + "; filename=\"" + fileName + "\"";
			}
			text2 += "\r\n";
			string contentType = multipartFormSection2.contentType;
			if (!string.IsNullOrEmpty(contentType))
			{
				text2 = text2 + "Content-Type: " + contentType + "\r\n";
			}
			list.AddRange(bytes);
			list.AddRange(bytes2);
			list.AddRange(boundary);
			list.AddRange(bytes);
			list.AddRange(Encoding.UTF8.GetBytes(text2));
			list.AddRange(bytes);
			list.AddRange(multipartFormSection2.sectionData);
		}
		list.AddRange(bytes);
		list.AddRange(bytes2);
		list.AddRange(boundary);
		list.AddRange(bytes2);
		list.AddRange(bytes);
		return list.ToArray();
	}

	public static byte[] GenerateBoundary()
	{
		byte[] array = new byte[40];
		for (int i = 0; i < 40; i++)
		{
			int num = Random.Range(48, 110);
			if (num > 57)
			{
				num += 7;
			}
			if (num > 90)
			{
				num += 6;
			}
			array[i] = (byte)num;
		}
		return array;
	}

	public static byte[] SerializeSimpleForm(Dictionary<string, string> formFields)
	{
		string text = "";
		foreach (KeyValuePair<string, string> formField in formFields)
		{
			if (text.Length > 0)
			{
				text += "&";
			}
			text = text + WWWTranscoder.DataEncode(formField.Key) + "=" + WWWTranscoder.DataEncode(formField.Value);
		}
		return Encoding.UTF8.GetBytes(text);
	}
}
