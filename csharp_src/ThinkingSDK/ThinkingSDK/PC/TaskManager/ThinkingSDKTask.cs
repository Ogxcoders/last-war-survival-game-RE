using System.Collections;
using System.Collections.Generic;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.Request;
using ThinkingSDK.PC.Storage;
using UnityEngine;

namespace ThinkingSDK.PC.TaskManager;

[DisallowMultipleComponent]
public class ThinkingSDKTask : MonoBehaviour
{
	private static readonly object _locker = new object();

	private List<ThinkingSDKBaseRequest> requestList = new List<ThinkingSDKBaseRequest>();

	private List<ResponseHandle> responseHandleList = new List<ResponseHandle>();

	private List<int> batchSizeList = new List<int>();

	private List<string> appIdList = new List<string>();

	private static ThinkingSDKTask mSingleTask;

	private bool isWaiting;

	private int mBatchSize = 30;

	public static ThinkingSDKTask SingleTask()
	{
		return mSingleTask;
	}

	private void Awake()
	{
		mSingleTask = this;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (requestList.Count > 0 && !isWaiting)
		{
			WaitOne();
			StartRequestSendData();
		}
	}

	public void WaitOne()
	{
		isWaiting = true;
	}

	public void Release()
	{
		isWaiting = false;
	}

	public void SyncInvokeAllTask()
	{
	}

	public void StartRequest(ThinkingSDKBaseRequest mRequest, ResponseHandle responseHandle, int batchSize, string appId)
	{
		lock (_locker)
		{
			requestList.Add(mRequest);
			responseHandleList.Add(responseHandle);
			batchSizeList.Add(batchSize);
			appIdList.Add(appId);
		}
	}

	private void StartRequestSendData()
	{
		if (requestList.Count <= 0)
		{
			return;
		}
		ThinkingSDKBaseRequest thinkingSDKBaseRequest;
		ResponseHandle responseHandle;
		IList<Dictionary<string, object>> list;
		lock (_locker)
		{
			thinkingSDKBaseRequest = requestList[0];
			responseHandle = responseHandleList[0];
			list = ThinkingSDKFileJson.DequeueBatchTrackingData(batchSizeList[0], appIdList[0]);
		}
		if (thinkingSDKBaseRequest != null)
		{
			if (list.Count > 0)
			{
				StartCoroutine(SendData(thinkingSDKBaseRequest, responseHandle, list));
			}
			else
			{
				responseHandle?.Invoke();
			}
			lock (_locker)
			{
				requestList.RemoveAt(0);
				responseHandleList.RemoveAt(0);
				batchSizeList.RemoveAt(0);
				appIdList.RemoveAt(0);
			}
		}
	}

	private IEnumerator SendData(ThinkingSDKBaseRequest mRequest, ResponseHandle responseHandle, IList<Dictionary<string, object>> list)
	{
		yield return mRequest.SendData_2(responseHandle, list);
	}
}
