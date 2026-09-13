using System;
using System.Net;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using UnityEngine.Networking;

namespace Main.Scripts.Application.LoadingState;

public class HttpRequest
{
	public enum Status
	{
		Wait,
		WaitRetry,
		Progressing,
		Success,
		TimeOut,
		Failed
	}

	private class WebRequestCertificateHandler : CertificateHandler
	{
		protected override bool ValidateCertificate(byte[] certificateData)
		{
			return true;
		}
	}

	private UnityWebRequest _httpRequest;

	private float _elapseTime;

	private float _timeout = 8f;

	private int _maxTryCount = 5;

	private int _tryCount;

	private Status _status;

	private string _url;

	private string _error;

	private float _retryInterval = 1f;

	public Status status => _status;

	public string url => _url;

	public event Action<HttpRequest, string> onFailed;

	public event Action<HttpRequest, int> onRetry;

	public event Action<HttpRequest> onTimeOut;

	public event Action<HttpRequest, DownloadHandler> onSuccess;

	public event Func<HttpRequest, DownloadHandler, bool> checkSuccess;

	public HttpRequest(string url)
	{
		_elapseTime = 0f;
		_tryCount = 1;
		_url = url;
		_status = Status.Wait;
		_httpRequest = UnityWebRequest.Get(url);
		_retryInterval = 1f;
		if (!WebRequestManager.DisableCertificateHandler() && _url.StartsWith("https://"))
		{
			_httpRequest.certificateHandler = new WebRequestCertificateHandler();
		}
	}

	public void SendRequest()
	{
		_httpRequest.SendWebRequest();
		_status = Status.Progressing;
	}

	public void Dispose()
	{
		if (_httpRequest != null)
		{
			_httpRequest.Dispose();
			_httpRequest = null;
		}
	}

	public void OnUpdate()
	{
		if (_httpRequest == null)
		{
			return;
		}
		if (_httpRequest.isDone)
		{
			GameEntry.Event.Fire(EventId.NetworkRetry, false);
			if (_httpRequest.isHttpError || _httpRequest.isNetworkError)
			{
				Failed(Status.Failed, _httpRequest.error, _httpRequest.responseCode);
			}
			else
			{
				Success();
			}
			return;
		}
		_elapseTime += Time.deltaTime;
		if (_status == Status.WaitRetry)
		{
			if (_elapseTime >= _retryInterval)
			{
				_elapseTime = 0f;
				_httpRequest.SendWebRequest();
				_status = Status.Progressing;
				Log.Info($"HttpRequest::Retry start:{_tryCount}/{_maxTryCount} {url}");
			}
		}
		else if (_elapseTime > _timeout)
		{
			Failed(Status.TimeOut, "TimeOut", 0L);
		}
	}

	private void Success()
	{
		_error = "HttpRequest::Success:" + url;
		if (this.checkSuccess != null)
		{
			if (this.checkSuccess(this, _httpRequest.downloadHandler))
			{
				_status = Status.Success;
				this.onSuccess?.Invoke(this, _httpRequest.downloadHandler);
			}
			else
			{
				Failed(Status.Failed, "CheckSuccess Failed", 0L);
			}
		}
		else
		{
			_status = Status.Success;
			this.onSuccess?.Invoke(this, _httpRequest.downloadHandler);
		}
	}

	private void Failed(Status s, string reason, long code)
	{
		Log.Info($"HttpRequest::Failed reason:{reason} code:{code}   url:{url}");
		if (_tryCount < _maxTryCount)
		{
			Retry();
			return;
		}
		_error = $"HttpRequest::Failed:{url},status:{s},reason:{reason}";
		if (_httpRequest != null)
		{
			_httpRequest.Dispose();
			_httpRequest = null;
		}
		_status = s;
		GameEntry.Event.Fire(EventId.NetworkRetry, false);
		if (s == Status.TimeOut)
		{
			this.onTimeOut?.Invoke(this);
			return;
		}
		if (reason.Contains("destination"))
		{
			try
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(url);
				foreach (IPAddress iPAddress in hostAddresses)
				{
					Log.Info($"HttpRequest::Failed Dns:{iPAddress.AddressFamily} {iPAddress}");
				}
			}
			catch (Exception ex)
			{
				Log.Info("HttpRequest::Failed Dns Exception " + ex.ToString());
			}
		}
		this.onFailed?.Invoke(this, _error);
	}

	private void Retry()
	{
		if (_httpRequest != null)
		{
			_httpRequest.Dispose();
			_httpRequest = null;
		}
		_tryCount++;
		_elapseTime = 0f;
		_httpRequest = UnityWebRequest.Get(_url);
		_status = Status.WaitRetry;
		_retryInterval += 1f;
		GameEntry.Event.Fire(EventId.NetworkRetry, true);
		this.onRetry?.Invoke(this, _tryCount);
	}
}
