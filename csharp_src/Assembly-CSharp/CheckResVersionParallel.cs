using System.Collections.Generic;
using GameFramework;
using Main.Scripts.Application.LoadingState;
using UnityEngine.Networking;

public class CheckResVersionParallel
{
	public enum QueryStatus
	{
		None,
		Querying,
		Succeed,
		Failed,
		Timeout
	}

	private List<string> _checkUrls = new List<string>();

	private List<HttpRequest> _requests = new List<HttpRequest>(2);

	private Dictionary<string, string> _requestHostMap = new Dictionary<string, string>(2);

	public HttpRequest succeedRequest { get; private set; }

	public DownloadHandler succeedDownloadHandler { get; private set; }

	public QueryStatus queryStatus { get; private set; }

	public void StartVersionRequest()
	{
		Log.Info("[ParallelInit] CheckResVersion::StartVersionRequest");
		queryStatus = QueryStatus.None;
		string[] hostListByCurGroupType = NetworkURLConfig.GetHostListByCurGroupType();
		int num = hostListByCurGroupType.Length;
		string[] array = new string[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = NetworkURLConfig.GetCheckVersionURL(hostListByCurGroupType[i]);
			_checkUrls.Add(hostListByCurGroupType[i]);
		}
		StartRequestQueue(array, hostListByCurGroupType);
		queryStatus = QueryStatus.Querying;
	}

	private void StartRequestQueue(string[] urls, string[] hosts)
	{
		_requests.Clear();
		_requestHostMap.Clear();
		int num = urls.Length;
		for (int i = 0; i < num; i++)
		{
			string text = urls[i];
			HttpRequest httpRequest = new HttpRequest(text);
			httpRequest.onFailed += OnRequestFailed;
			httpRequest.onTimeOut += OnRequestTimeOut;
			httpRequest.onSuccess += OnRequestSuccess;
			httpRequest.SendRequest();
			_requests.Add(httpRequest);
			_requestHostMap.Add(text, hosts[i]);
			Log.Info("[ParallelInit] CheckResVersion::send:" + text);
		}
	}

	public void OnUpdate()
	{
		int count = _requests.Count;
		for (int i = 0; i < count; i++)
		{
			if (_requests.Count == 0)
			{
				break;
			}
			_requests[i].OnUpdate();
		}
	}

	private void OnRequestFailed(HttpRequest req, string error)
	{
		Log.Info("[ParallelInit] CheckResVersion::Failed, err: " + error + ", url: " + req.url);
		bool flag = true;
		int count = _requests.Count;
		for (int i = 0; i < count; i++)
		{
			if (_requests.Count == 0)
			{
				break;
			}
			HttpRequest httpRequest = _requests[i];
			if (httpRequest.status != HttpRequest.Status.Failed && httpRequest.status != HttpRequest.Status.TimeOut)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			queryStatus = QueryStatus.Failed;
			Log.Info("[ParallelInit] CheckResVersion::Failed AllFailed");
		}
	}

	private void OnRequestTimeOut(HttpRequest req)
	{
		Log.Info("[ParallelInit] CheckResVersion::Timeout, url: " + req.url);
		bool flag = true;
		int count = _requests.Count;
		for (int i = 0; i < count; i++)
		{
			if (_requests.Count == 0)
			{
				break;
			}
			HttpRequest httpRequest = _requests[i];
			if (httpRequest.status != HttpRequest.Status.Failed && httpRequest.status != HttpRequest.Status.TimeOut)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			queryStatus = QueryStatus.Timeout;
			Log.Info("[ParallelInit] CheckResVersion::Timeout AllFailed");
		}
	}

	private void OnRequestSuccess(HttpRequest req, DownloadHandler downloadHandler)
	{
		Log.Info("[ParallelInit] CheckResVersion::OnRequestSuccess::url: " + req.url + ", raw json:" + downloadHandler.text);
		if (succeedRequest == null)
		{
			queryStatus = QueryStatus.Succeed;
			succeedRequest = req;
			succeedDownloadHandler = downloadHandler;
		}
	}

	public void StopAllRequest()
	{
		Log.Info("[ParallelInit] CheckResVersion::StopAllRequest");
		if (_requests.Count != 0)
		{
			for (int num = _requests.Count - 1; num >= 0; num--)
			{
				_requests[num].Dispose();
				_requests.RemoveAt(num);
			}
			queryStatus = QueryStatus.None;
			succeedRequest = null;
			succeedDownloadHandler = null;
		}
	}

	public void CopyRequestHost(Dictionary<string, string> requestHostMap)
	{
		requestHostMap.Clear();
		foreach (KeyValuePair<string, string> item in _requestHostMap)
		{
			requestHostMap[item.Key] = item.Value;
		}
	}
}
