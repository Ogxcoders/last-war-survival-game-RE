using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using BestHTTP;
using GameFramework;
using Newtonsoft.Json;

namespace RiverBISDK;

public class NetManager
{
	private Queue<PackParams> _sampleQueue;

	private Thread _moniterThread;

	private ManualResetEvent _stoppedEvt = new ManualResetEvent(initialState: false);

	private List<HTTPRequest> _pendingRequests = new List<HTTPRequest>();

	private static RemoteCertificateValidationCallback _sslValidationCallback;

	private Queue<PackParams> _workThreadQueue = new Queue<PackParams>();

	private static ConcurrentStack<IDictionary<string, object>> _paramPool = new ConcurrentStack<IDictionary<string, object>>();

	private static readonly ConcurrentStack<ArrayList> _innerListPool = new ConcurrentStack<ArrayList>();

	private readonly Func<object, string> _serializeFunc;

	public NetManager()
	{
		_serializeFunc = JsonConvert.SerializeObject;
		_sampleQueue = new Queue<PackParams>();
		_moniterThread = new Thread(WorkThread);
		_moniterThread.IsBackground = true;
		_moniterThread.Start();
		if (_sslValidationCallback == null)
		{
			_sslValidationCallback = ServerCertificateValidationCallback;
		}
		TextFile.ReadTextFile();
	}

	public void Dispose()
	{
		_stoppedEvt.Set();
		if (_moniterThread.ThreadState == ThreadState.Background)
		{
			try
			{
				_moniterThread.Join();
			}
			catch (Exception message)
			{
				Log.Error(message);
			}
		}
		lock (_pendingRequests)
		{
			int i = 0;
			for (int count = _pendingRequests.Count; i < count; i++)
			{
				HTTPRequest hTTPRequest = _pendingRequests[i];
				IDictionary<string, object> param = hTTPRequest.Tag as IDictionary<string, object>;
				SendQueueListFailHandle(param, -1);
				try
				{
					hTTPRequest.Callback = null;
					hTTPRequest.Abort();
					hTTPRequest.Dispose();
				}
				catch (Exception message2)
				{
					Log.Error(message2);
				}
			}
			_pendingRequests.Clear();
		}
	}

	public void WorkThread()
	{
		do
		{
			WorkThreadQueueList();
		}
		while (!_stoppedEvt.WaitOne(100));
	}

	private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
	{
		return true;
	}

	private static IDictionary<string, object> AllocateParam()
	{
		if (!_paramPool.TryPop(out var result))
		{
			return new Dictionary<string, object>(6);
		}
		return result;
	}

	private static void RecycleParam(IDictionary<string, object> param)
	{
		if (param.TryGetValue("l", out var value) && value is ArrayList arrayList)
		{
			arrayList.Clear();
			_innerListPool.Push(arrayList);
		}
		param.Clear();
		_paramPool.Push(param);
	}

	private static ArrayList AllocateInnerList()
	{
		if (!_innerListPool.TryPop(out var result))
		{
			return new ArrayList(BIConfig.maxQueueList);
		}
		return result;
	}

	private void WorkThreadQueueList()
	{
		if (_workThreadQueue.Count > 0)
		{
			_workThreadQueue.Clear();
		}
		lock (_sampleQueue)
		{
			while (_sampleQueue.Count > 0)
			{
				_workThreadQueue.Enqueue(_sampleQueue.Dequeue());
			}
		}
		while (_workThreadQueue.Count > 0)
		{
			IDictionary<string, object> dictionary = DeQueueListHttps(_workThreadQueue);
			if (dictionary == null)
			{
				continue;
			}
			try
			{
				string s = JsonConvert.SerializeObject(dictionary);
				HTTPRequest hTTPRequest = new HTTPRequest(new Uri(BIConfig.postUrl), HTTPMethods.Post, OnHttpRequestComplete);
				hTTPRequest.Timeout = TimeSpan.FromMilliseconds(BIConfig.timeOut);
				hTTPRequest.SetHeader("Content-Type", "application/json; charset=UTF-8");
				hTTPRequest.RawData = Encoding.UTF8.GetBytes(s);
				hTTPRequest.Tag = dictionary;
				hTTPRequest.Send();
				lock (_pendingRequests)
				{
					_pendingRequests.Add(hTTPRequest);
				}
			}
			catch (Exception message)
			{
				Log.Error(message);
				SendQueueListFailHandle(dictionary, 502);
			}
		}
	}

	private void OnHttpRequestComplete(HTTPRequest request, HTTPResponse response)
	{
		lock (_pendingRequests)
		{
			_pendingRequests.Remove(request);
		}
		int num = response?.StatusCode ?? (-1);
		BIManagerCore.instance.Dlog("NetManager WorkThreadQueueList statusCode:" + num);
		if (response != null && response.IsSuccess)
		{
			RecycleParam(request.Tag as IDictionary<string, object>);
			string dataAsText = response.DataAsText;
			BIManagerCore.instance.Dlog("NetManager WorkThreadQueueList result:" + dataAsText);
		}
		else
		{
			SendQueueListFailHandle(request.Tag as IDictionary<string, object>, num);
		}
	}

	private void SendQueueListFailHandle(IDictionary<string, object> param, int statusCode)
	{
		if (param != null)
		{
			if (param.ContainsKey("l"))
			{
				TextFile.WriteTextFile((ArrayList)param["l"], _serializeFunc);
			}
			RecycleParam(param);
		}
	}

	private static long GetTimestampSince1970()
	{
		return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
	}

	private IDictionary<string, object> DeQueueListHttps(Queue<PackParams> workQueue)
	{
		ArrayList arrayList = AllocateInnerList();
		IDictionary<string, object> dictionary = AllocateParam();
		try
		{
			int num = BIConfig.maxQueueList;
			if (workQueue.Count < BIConfig.maxQueueList)
			{
				num = workQueue.Count;
			}
			int num2 = 0;
			while (num2 < num)
			{
				PackParams packParams = workQueue.Dequeue();
				if (packParams != null)
				{
					arrayList.Add(packParams.GetSendMap());
					num2++;
				}
			}
			if (arrayList.Count == 0)
			{
				return null;
			}
			dictionary.Add("rid", GetTimestampSince1970() + Guid.NewGuid().ToString());
			dictionary.Add("v", "0.1.0");
			dictionary.Add("s", "client");
			dictionary.Add("rc", 0);
			dictionary.Add("app", BIConfig.appId);
			dictionary.Add("l", arrayList);
			return dictionary;
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		return null;
	}

	public void PushQueueHttps(PackParams sampleParam)
	{
		try
		{
			lock (_sampleQueue)
			{
				_sampleQueue.Enqueue(sampleParam);
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
	}
}
