namespace UnityEngine.Android;

public class RequestToUseMobileDataAsyncOperation : CustomYieldInstruction
{
	private AndroidAssetPackUseMobileDataRequestResult m_RequestResult;

	private readonly object m_OperationLock;

	public override bool keepWaiting
	{
		get
		{
			lock (m_OperationLock)
			{
				return m_RequestResult == null;
			}
		}
	}

	public bool isDone => !keepWaiting;

	public AndroidAssetPackUseMobileDataRequestResult result
	{
		get
		{
			lock (m_OperationLock)
			{
				return m_RequestResult;
			}
		}
	}

	internal RequestToUseMobileDataAsyncOperation()
	{
		m_OperationLock = new object();
	}

	internal void OnResult(AndroidAssetPackUseMobileDataRequestResult result)
	{
		lock (m_OperationLock)
		{
			m_RequestResult = result;
		}
	}
}
