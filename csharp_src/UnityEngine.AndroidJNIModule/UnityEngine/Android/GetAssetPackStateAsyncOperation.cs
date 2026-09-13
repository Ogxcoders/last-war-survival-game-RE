namespace UnityEngine.Android;

public class GetAssetPackStateAsyncOperation : CustomYieldInstruction
{
	private ulong m_Size;

	private AndroidAssetPackState[] m_States;

	private readonly object m_OperationLock;

	public override bool keepWaiting
	{
		get
		{
			lock (m_OperationLock)
			{
				return m_States == null;
			}
		}
	}

	public bool isDone => !keepWaiting;

	public ulong size
	{
		get
		{
			lock (m_OperationLock)
			{
				return m_Size;
			}
		}
	}

	public AndroidAssetPackState[] states
	{
		get
		{
			lock (m_OperationLock)
			{
				return m_States;
			}
		}
	}

	internal GetAssetPackStateAsyncOperation()
	{
		m_OperationLock = new object();
	}

	internal void OnResult(ulong size, AndroidAssetPackState[] states)
	{
		lock (m_OperationLock)
		{
			m_Size = size;
			m_States = states;
		}
	}
}
