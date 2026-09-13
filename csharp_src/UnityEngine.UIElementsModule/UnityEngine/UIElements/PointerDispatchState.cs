#define UNITY_ASSERTIONS
namespace UnityEngine.UIElements;

internal class PointerDispatchState
{
	private IEventHandler[] m_PendingPointerCapture = new IEventHandler[PointerId.maxPointers];

	private IEventHandler[] m_PointerCapture = new IEventHandler[PointerId.maxPointers];

	private bool[] m_ShouldSendCompatibilityMouseEvents = new bool[PointerId.maxPointers];

	public PointerDispatchState()
	{
		Reset();
	}

	internal void Reset()
	{
		for (int i = 0; i < m_PointerCapture.Length; i++)
		{
			m_PendingPointerCapture[i] = null;
			m_PointerCapture[i] = null;
			m_ShouldSendCompatibilityMouseEvents[i] = true;
		}
	}

	public IEventHandler GetCapturingElement(int pointerId)
	{
		return m_PendingPointerCapture[pointerId];
	}

	public bool HasPointerCapture(IEventHandler handler, int pointerId)
	{
		return m_PendingPointerCapture[pointerId] == handler;
	}

	public void CapturePointer(IEventHandler handler, int pointerId)
	{
		if (pointerId == PointerId.mousePointerId && m_PendingPointerCapture[pointerId] != handler && GUIUtility.hotControl != 0)
		{
			Debug.Assert(GUIUtility.hotControl != 0, "Should not be capturing when there is a hotcontrol");
		}
		else
		{
			m_PendingPointerCapture[pointerId] = handler;
		}
	}

	public void ReleasePointer(int pointerId)
	{
		m_PendingPointerCapture[pointerId] = null;
	}

	public void ReleasePointer(IEventHandler handler, int pointerId)
	{
		if (handler == m_PendingPointerCapture[pointerId])
		{
			m_PendingPointerCapture[pointerId] = null;
		}
	}

	public void ProcessPointerCapture(int pointerId)
	{
		if (m_PointerCapture[pointerId] == m_PendingPointerCapture[pointerId])
		{
			return;
		}
		if (m_PointerCapture[pointerId] != null)
		{
			using (PointerCaptureOutEvent e = PointerCaptureEventBase<PointerCaptureOutEvent>.GetPooled(m_PointerCapture[pointerId], m_PendingPointerCapture[pointerId], pointerId))
			{
				m_PointerCapture[pointerId].SendEvent(e);
			}
			if (pointerId == PointerId.mousePointerId)
			{
				using MouseCaptureOutEvent e2 = PointerCaptureEventBase<MouseCaptureOutEvent>.GetPooled(m_PointerCapture[pointerId], m_PendingPointerCapture[pointerId], pointerId);
				m_PointerCapture[pointerId].SendEvent(e2);
			}
		}
		if (m_PendingPointerCapture[pointerId] != null)
		{
			using (PointerCaptureEvent e3 = PointerCaptureEventBase<PointerCaptureEvent>.GetPooled(m_PendingPointerCapture[pointerId], m_PointerCapture[pointerId], pointerId))
			{
				m_PendingPointerCapture[pointerId].SendEvent(e3);
			}
			if (pointerId == PointerId.mousePointerId)
			{
				using MouseCaptureEvent e4 = PointerCaptureEventBase<MouseCaptureEvent>.GetPooled(m_PendingPointerCapture[pointerId], m_PointerCapture[pointerId], pointerId);
				m_PendingPointerCapture[pointerId].SendEvent(e4);
			}
		}
		m_PointerCapture[pointerId] = m_PendingPointerCapture[pointerId];
	}

	public void ActivateCompatibilityMouseEvents(int pointerId)
	{
		m_ShouldSendCompatibilityMouseEvents[pointerId] = true;
	}

	public void PreventCompatibilityMouseEvents(int pointerId)
	{
		m_ShouldSendCompatibilityMouseEvents[pointerId] = false;
	}

	public bool ShouldSendCompatibilityMouseEvents(IPointerEvent evt)
	{
		return evt.isPrimary && m_ShouldSendCompatibilityMouseEvents[evt.pointerId];
	}
}
