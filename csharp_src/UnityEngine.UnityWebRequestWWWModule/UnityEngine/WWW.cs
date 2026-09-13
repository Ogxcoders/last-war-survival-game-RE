using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine.Networking;

namespace UnityEngine;

[Obsolete("Use UnityWebRequest, a fully featured replacement which is more efficient and has additional features")]
public class WWW : CustomYieldInstruction, IDisposable
{
	private UnityWebRequest _uwr;

	private AssetBundle _assetBundle;

	private Dictionary<string, string> _responseHeaders;

	public AssetBundle assetBundle
	{
		get
		{
			if (_assetBundle == null)
			{
				if (!WaitUntilDoneIfPossible())
				{
					return null;
				}
				if (_uwr.isNetworkError)
				{
					return null;
				}
				if (_uwr.downloadHandler is DownloadHandlerAssetBundle downloadHandlerAssetBundle)
				{
					_assetBundle = downloadHandlerAssetBundle.assetBundle;
				}
				else
				{
					byte[] array = bytes;
					if (array == null)
					{
						return null;
					}
					_assetBundle = AssetBundle.LoadFromMemory(array);
				}
			}
			return _assetBundle;
		}
	}

	[Obsolete("Obsolete msg (UnityUpgradable) -> * UnityEngine.WWW.GetAudioClip()", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Object audioClip => null;

	public byte[] bytes
	{
		get
		{
			if (!WaitUntilDoneIfPossible())
			{
				return new byte[0];
			}
			if (_uwr.isNetworkError)
			{
				return new byte[0];
			}
			DownloadHandler downloadHandler = _uwr.downloadHandler;
			if (downloadHandler == null)
			{
				return new byte[0];
			}
			return downloadHandler.data;
		}
	}

	[Obsolete("WWW.size is obsolete. Please use WWW.bytesDownloaded instead")]
	public int size => bytesDownloaded;

	public int bytesDownloaded => (int)_uwr.downloadedBytes;

	public string error
	{
		get
		{
			if (!_uwr.isDone)
			{
				return null;
			}
			if (_uwr.isNetworkError)
			{
				return _uwr.error;
			}
			if (_uwr.responseCode >= 400)
			{
				string hTTPStatusString = UnityWebRequest.GetHTTPStatusString(_uwr.responseCode);
				return $"{_uwr.responseCode} {hTTPStatusString}";
			}
			return null;
		}
	}

	public bool isDone => _uwr.isDone;

	public float progress
	{
		get
		{
			float num = _uwr.downloadProgress;
			if (num < 0f)
			{
				num = 0f;
			}
			return num;
		}
	}

	public Dictionary<string, string> responseHeaders
	{
		get
		{
			if (!isDone)
			{
				return new Dictionary<string, string>();
			}
			if (_responseHeaders == null)
			{
				_responseHeaders = _uwr.GetResponseHeaders();
				if (_responseHeaders != null)
				{
					string hTTPStatusString = UnityWebRequest.GetHTTPStatusString(_uwr.responseCode);
					_responseHeaders["STATUS"] = $"HTTP/1.1 {_uwr.responseCode} {hTTPStatusString}";
				}
				else
				{
					_responseHeaders = new Dictionary<string, string>();
				}
			}
			return _responseHeaders;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Please use WWW.text instead. (UnityUpgradable) -> text", true)]
	public string data => text;

	public string text
	{
		get
		{
			if (!WaitUntilDoneIfPossible())
			{
				return "";
			}
			if (_uwr.isNetworkError)
			{
				return "";
			}
			DownloadHandler downloadHandler = _uwr.downloadHandler;
			if (downloadHandler == null)
			{
				return "";
			}
			return downloadHandler.text;
		}
	}

	public Texture2D texture => CreateTextureFromDownloadedData(markNonReadable: false);

	public Texture2D textureNonReadable => CreateTextureFromDownloadedData(markNonReadable: true);

	public ThreadPriority threadPriority { get; set; }

	public float uploadProgress
	{
		get
		{
			float num = _uwr.uploadProgress;
			if (num < 0f)
			{
				num = 0f;
			}
			return num;
		}
	}

	public string url => _uwr.url;

	public override bool keepWaiting => _uwr != null && !_uwr.isDone;

	public static string EscapeURL(string s)
	{
		return EscapeURL(s, Encoding.UTF8);
	}

	public static string EscapeURL(string s, Encoding e)
	{
		return UnityWebRequest.EscapeURL(s, e);
	}

	public static string UnEscapeURL(string s)
	{
		return UnEscapeURL(s, Encoding.UTF8);
	}

	public static string UnEscapeURL(string s, Encoding e)
	{
		return UnityWebRequest.UnEscapeURL(s, e);
	}

	public static WWW LoadFromCacheOrDownload(string url, int version)
	{
		return LoadFromCacheOrDownload(url, version, 0u);
	}

	public static WWW LoadFromCacheOrDownload(string url, int version, uint crc)
	{
		Hash128 hash = new Hash128(0u, 0u, 0u, (uint)version);
		return LoadFromCacheOrDownload(url, hash, crc);
	}

	public static WWW LoadFromCacheOrDownload(string url, Hash128 hash)
	{
		return LoadFromCacheOrDownload(url, hash, 0u);
	}

	public static WWW LoadFromCacheOrDownload(string url, Hash128 hash, uint crc)
	{
		return new WWW(url, "", hash, crc);
	}

	public static WWW LoadFromCacheOrDownload(string url, CachedAssetBundle cachedBundle, uint crc = 0u)
	{
		return new WWW(url, cachedBundle.name, cachedBundle.hash, crc);
	}

	public WWW(string url)
	{
		_uwr = UnityWebRequest.Get(url);
		_uwr.SendWebRequest();
	}

	public WWW(string url, WWWForm form)
	{
		_uwr = UnityWebRequest.Post(url, form);
		_uwr.chunkedTransfer = false;
		_uwr.SendWebRequest();
	}

	public WWW(string url, byte[] postData)
	{
		_uwr = new UnityWebRequest(url, "POST");
		_uwr.chunkedTransfer = false;
		UploadHandler uploadHandler = new UploadHandlerRaw(postData)
		{
			contentType = "application/x-www-form-urlencoded"
		};
		_uwr.uploadHandler = uploadHandler;
		_uwr.downloadHandler = new DownloadHandlerBuffer();
		_uwr.SendWebRequest();
	}

	[Obsolete("This overload is deprecated. Use UnityEngine.WWW.WWW(string, byte[], System.Collections.Generic.Dictionary<string, string>) instead.")]
	public WWW(string url, byte[] postData, Hashtable headers)
	{
		_uwr = new UnityWebRequest(url, (postData == null) ? "GET" : "POST");
		_uwr.chunkedTransfer = false;
		UploadHandler uploadHandler = new UploadHandlerRaw(postData)
		{
			contentType = "application/x-www-form-urlencoded"
		};
		_uwr.uploadHandler = uploadHandler;
		_uwr.downloadHandler = new DownloadHandlerBuffer();
		foreach (object key in headers.Keys)
		{
			_uwr.SetRequestHeader((string)key, (string)headers[key]);
		}
		_uwr.SendWebRequest();
	}

	public WWW(string url, byte[] postData, Dictionary<string, string> headers)
	{
		_uwr = new UnityWebRequest(url, (postData == null) ? "GET" : "POST");
		_uwr.chunkedTransfer = false;
		UploadHandler uploadHandler = new UploadHandlerRaw(postData)
		{
			contentType = "application/x-www-form-urlencoded"
		};
		_uwr.uploadHandler = uploadHandler;
		_uwr.downloadHandler = new DownloadHandlerBuffer();
		foreach (KeyValuePair<string, string> header in headers)
		{
			_uwr.SetRequestHeader(header.Key, header.Value);
		}
		_uwr.SendWebRequest();
	}

	internal WWW(string url, string name, Hash128 hash, uint crc)
	{
		_uwr = UnityWebRequestAssetBundle.GetAssetBundle(url, new CachedAssetBundle(name, hash), crc);
		_uwr.SendWebRequest();
	}

	private Texture2D CreateTextureFromDownloadedData(bool markNonReadable)
	{
		if (!WaitUntilDoneIfPossible())
		{
			return new Texture2D(2, 2);
		}
		if (_uwr.isNetworkError)
		{
			return null;
		}
		DownloadHandler downloadHandler = _uwr.downloadHandler;
		if (downloadHandler == null)
		{
			return null;
		}
		Texture2D texture2D = new Texture2D(2, 2);
		texture2D.LoadImage(downloadHandler.data, markNonReadable);
		return texture2D;
	}

	public void LoadImageIntoTexture(Texture2D texture)
	{
		if (!WaitUntilDoneIfPossible())
		{
			return;
		}
		if (_uwr.isNetworkError)
		{
			Debug.LogError("Cannot load image: download failed");
			return;
		}
		DownloadHandler downloadHandler = _uwr.downloadHandler;
		if (downloadHandler == null)
		{
			Debug.LogError("Cannot load image: internal error");
		}
		else
		{
			texture.LoadImage(downloadHandler.data, markNonReadable: false);
		}
	}

	public void Dispose()
	{
		if (_uwr != null)
		{
			_uwr.Dispose();
			_uwr = null;
		}
	}

	internal Object GetAudioClipInternal(bool threeD, bool stream, bool compressed, AudioType audioType)
	{
		return WebRequestWWW.InternalCreateAudioClipUsingDH(_uwr.downloadHandler, _uwr.url, stream, compressed, audioType);
	}

	public AudioClip GetAudioClip()
	{
		return GetAudioClip(threeD: true, stream: false, AudioType.UNKNOWN);
	}

	public AudioClip GetAudioClip(bool threeD)
	{
		return GetAudioClip(threeD, stream: false, AudioType.UNKNOWN);
	}

	public AudioClip GetAudioClip(bool threeD, bool stream)
	{
		return GetAudioClip(threeD, stream, AudioType.UNKNOWN);
	}

	public AudioClip GetAudioClip(bool threeD, bool stream, AudioType audioType)
	{
		return (AudioClip)GetAudioClipInternal(threeD, stream, compressed: false, audioType);
	}

	public AudioClip GetAudioClipCompressed()
	{
		return GetAudioClipCompressed(threeD: false, AudioType.UNKNOWN);
	}

	public AudioClip GetAudioClipCompressed(bool threeD)
	{
		return GetAudioClipCompressed(threeD, AudioType.UNKNOWN);
	}

	public AudioClip GetAudioClipCompressed(bool threeD, AudioType audioType)
	{
		return (AudioClip)GetAudioClipInternal(threeD, stream: false, compressed: true, audioType);
	}

	private bool WaitUntilDoneIfPossible()
	{
		if (_uwr.isDone)
		{
			return true;
		}
		if (url.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
		{
			while (!_uwr.isDone)
			{
			}
			return true;
		}
		Debug.LogError("You are trying to load data from a www stream which has not completed the download yet.\nYou need to yield the download or wait until isDone returns true.");
		return false;
	}
}
