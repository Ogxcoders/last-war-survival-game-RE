using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GameKit.Base;

public class WebRequestManager : SingletonBehaviour<WebRequestManager>
{
	[Serializable]
	private struct WebRequestParams
	{
		public OnWebRequestCallback calback;

		public int priority;

		public object userdata;
	}

	public delegate void OnWebRequestCallback(UnityWebRequest request, bool hasErr, object userdata);

	public class WebRequestCertificateHandler : CertificateHandler
	{
		protected override bool ValidateCertificate(byte[] certificateData)
		{
			return true;
		}
	}

	public const string FILE_NO_EXISTS = "file no exists";

	private readonly Dictionary<string, UnityWebRequestAsyncOperation> m_WorkingRequests = new Dictionary<string, UnityWebRequestAsyncOperation>();

	private readonly List<UnityWebRequest> m_WaitingRequests = new List<UnityWebRequest>();

	private readonly Dictionary<UnityWebRequest, WebRequestParams> m_ParamsStack = new Dictionary<UnityWebRequest, WebRequestParams>();

	[SerializeField]
	private int maxWorkingWebRequestThread = 3;

	private Dictionary<string, UnityWebRequestAsyncOperation>.Enumerator enumeratorWorking;

	private readonly List<string> keysToRemove = new List<string>();

	private void OnDisable()
	{
		enumeratorWorking = m_WorkingRequests.GetEnumerator();
		while (enumeratorWorking.MoveNext())
		{
			UnityWebRequest webRequest = enumeratorWorking.Current.Value.webRequest;
			Dispose(webRequest);
		}
		m_WorkingRequests.Clear();
	}

	public bool IsDownloadResult(UnityWebRequest request, string result)
	{
		if (string.IsNullOrEmpty(result))
		{
			return request.downloadedBytes == 0;
		}
		if (request.downloadedBytes == (ulong)result.Length)
		{
			return request.downloadHandler.text == result;
		}
		return false;
	}

	public void LoadAssetBundle(string uri, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequestAssetBundle.GetAssetBundle(uri, 0u), callback, priority, timeout, userdata);
	}

	public void LoadAssetBundle(string uri, Hash128 hash, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequestAssetBundle.GetAssetBundle(uri, hash), callback, priority, timeout, userdata);
	}

	public void LoadTexture(string uri, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequestTexture.GetTexture(uri, nonReadable: false), callback, priority, timeout, userdata);
	}

	public void LoadTexture(string uri, bool nonReadable, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequestTexture.GetTexture(uri, nonReadable), callback, priority, timeout, userdata);
	}

	public void LoadMultimedia(string uri, AudioType audioType, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequestMultimedia.GetAudioClip(uri, audioType), callback, priority, timeout, userdata);
	}

	public void Get(string uri, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequest.Get(uri), callback, priority, timeout, userdata);
	}

	public void Post(string uri, Dictionary<string, string> formFields, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequest.Post(uri, formFields), callback, priority, timeout, userdata);
	}

	public void Post(string uri, string postData, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequest.Post(uri, postData), callback, priority, timeout, userdata);
	}

	public UnityWebRequest Post(string uri, WWWForm formData, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		return Request(UnityWebRequest.Post(uri, formData), callback, priority, timeout, userdata);
	}

	public UnityWebRequest PostJson(string uri, string json, OnWebRequestCallback callback, int priority = 0, int timeout = 0, Dictionary<string, string> headers = null)
	{
		return Request(CreatePostJson(uri, json, headers), callback, priority, timeout);
	}

	public static UnityWebRequest CreatePostJson(string uri, string json, Dictionary<string, string> headers = null)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
		byte[] bytes = Encoding.UTF8.GetBytes(json);
		unityWebRequest.uploadHandler = new UploadHandlerRaw(bytes);
		unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
		unityWebRequest.SetRequestHeader("Content-Type", "application/json");
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				unityWebRequest.SetRequestHeader(header.Key, header.Value);
			}
		}
		return unityWebRequest;
	}

	public void Post(string uri, List<IMultipartFormSection> multipartFormSections, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequest.Post(uri, multipartFormSections), callback, priority, timeout, userdata);
	}

	public void Head(string uri, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequest.Head(uri), callback, priority, timeout, userdata);
	}

	public void Put(string uri, string bodyData, OnWebRequestCallback callback = null, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequest.Put(uri, bodyData), callback, priority, timeout, userdata);
	}

	public void Put(string uri, byte[] bodyData, OnWebRequestCallback callback = null, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequest.Put(uri, bodyData), callback, priority, timeout, userdata);
	}

	public void Delete(string uri, OnWebRequestCallback callback = null, int priority = 0, int timeout = 0, object userdata = null)
	{
		Request(UnityWebRequest.Delete(uri), callback, priority, timeout, userdata);
	}

	public UnityWebRequest DownFile(string uri, string localFilePath, OnWebRequestCallback callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
		unityWebRequest.downloadHandler = new DownloadHandlerFile(localFilePath);
		return Request(unityWebRequest, callback, priority, timeout, userdata);
	}

	private UnityWebRequest Request(UnityWebRequest request, OnWebRequestCallback callback = null, int priority = 0, int timeout = 0, object userdata = null)
	{
		if (request != null)
		{
			if (timeout > 0)
			{
				request.timeout = timeout;
			}
			if (!DisableCertificateHandler() && request.url.StartsWith("https://"))
			{
				request.certificateHandler = new WebRequestCertificateHandler();
			}
			m_WaitingRequests.Add(request);
			m_ParamsStack.Add(request, new WebRequestParams
			{
				calback = callback,
				priority = priority,
				userdata = userdata
			});
		}
		return request;
	}

	public void Cancel(string url)
	{
		for (int num = m_WaitingRequests.Count - 1; num >= 0; num--)
		{
			if (m_WaitingRequests[num].url == url)
			{
				Dispose(m_WaitingRequests[num]);
				m_WaitingRequests.RemoveAt(num);
				break;
			}
		}
	}

	public void Abort(string url)
	{
		Cancel(url);
		if (m_WorkingRequests.TryGetValue(url, out var value))
		{
			m_WorkingRequests.Remove(url);
			m_ParamsStack.Remove(value.webRequest);
			value.webRequest.Abort();
			Dispose(value.webRequest);
		}
	}

	public void ChatSendPhotoAbort(UnityWebRequest webRequest)
	{
		for (int num = m_WaitingRequests.Count - 1; num >= 0; num--)
		{
			if (m_WaitingRequests[num] == webRequest)
			{
				Dispose(webRequest);
				m_WaitingRequests.RemoveAt(num);
				break;
			}
		}
		foreach (KeyValuePair<string, UnityWebRequestAsyncOperation> item in m_WorkingRequests.ToList())
		{
			if (item.Value.webRequest == webRequest)
			{
				m_WorkingRequests.Remove(item.Key);
				m_ParamsStack.Remove(webRequest);
				webRequest.Abort();
				Dispose(webRequest);
				break;
			}
		}
	}

	protected override void OnUpdate(float delta)
	{
		enumeratorWorking = m_WorkingRequests.GetEnumerator();
		while (enumeratorWorking.MoveNext())
		{
			string key = enumeratorWorking.Current.Key;
			UnityWebRequestAsyncOperation value = enumeratorWorking.Current.Value;
			try
			{
				if (m_ParamsStack.TryGetValue(value.webRequest, out var value2))
				{
					bool hasErr = false;
					if (value.webRequest.isHttpError || value.webRequest.isNetworkError)
					{
						hasErr = true;
						Debug.LogErrorFormat("{0} : {1}", value.webRequest.error, key);
					}
					value2.calback?.Invoke(value.webRequest, hasErr, value2.userdata);
					continue;
				}
				throw new Exception($"{value.webRequest.url} is out of control");
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			finally
			{
				if (value.isDone)
				{
					keysToRemove.Add(key);
				}
			}
		}
		for (int i = 0; i < keysToRemove.Count; i++)
		{
			if (m_WorkingRequests.TryGetValue(keysToRemove[i], out var value3))
			{
				m_WorkingRequests.Remove(keysToRemove[i]);
				m_ParamsStack.Remove(value3.webRequest);
				Dispose(value3.webRequest);
			}
		}
		keysToRemove.Clear();
		if (m_WorkingRequests.Count >= maxWorkingWebRequestThread)
		{
			return;
		}
		int num = 0;
		while (num < m_WaitingRequests.Count)
		{
			if (m_WaitingRequests[num] != null)
			{
				UnityWebRequest unityWebRequest = m_WaitingRequests[num];
				if (!m_WorkingRequests.ContainsKey(unityWebRequest.url))
				{
					UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
					unityWebRequestAsyncOperation.priority = m_ParamsStack[unityWebRequest].priority;
					m_WorkingRequests.Add(unityWebRequest.url, unityWebRequestAsyncOperation);
					m_WaitingRequests.RemoveAt(num);
					if (m_WorkingRequests.Count >= maxWorkingWebRequestThread)
					{
						break;
					}
				}
				else
				{
					num++;
				}
			}
			else
			{
				m_WaitingRequests.RemoveAt(num);
			}
		}
	}

	private void Dispose(UnityWebRequest request)
	{
		if (request != null)
		{
			request.Dispose();
			GC.SuppressFinalize(request);
		}
	}

	public static bool DisableCertificateHandler()
	{
		return SDKManager.IS_IPhonePlayer();
	}
}
