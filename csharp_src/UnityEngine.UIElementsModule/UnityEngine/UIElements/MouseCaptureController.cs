namespace UnityEngine.UIElements;

public static class MouseCaptureController
{
	private static bool m_IsMouseCapturedWarningEmitted = false;

	private static bool m_ReleaseMouseWarningEmitted = false;

	public static bool IsMouseCaptured()
	{
		if (!m_IsMouseCapturedWarningEmitted)
		{
			Debug.LogError("MouseCaptureController.IsMouseCaptured() can not be used in playmode. Please use PointerCaptureHelper.GetCapturingElement() instead.");
			m_IsMouseCapturedWarningEmitted = true;
		}
		return false;
	}

	public static bool HasMouseCapture(this IEventHandler handler)
	{
		VisualElement handler2 = handler as VisualElement;
		return handler2.HasPointerCapture(PointerId.mousePointerId);
	}

	public static void CaptureMouse(this IEventHandler handler)
	{
		if (handler is VisualElement visualElement)
		{
			visualElement.CapturePointer(PointerId.mousePointerId);
			visualElement.panel.ProcessPointerCapture(PointerId.mousePointerId);
		}
	}

	public static void ReleaseMouse(this IEventHandler handler)
	{
		if (handler is VisualElement visualElement)
		{
			visualElement.ReleasePointer(PointerId.mousePointerId);
			visualElement.panel.ProcessPointerCapture(PointerId.mousePointerId);
		}
	}

	public static void ReleaseMouse()
	{
		if (!m_ReleaseMouseWarningEmitted)
		{
			Debug.LogError("MouseCaptureController.ReleaseMouse() can not be used in playmode. Please use PointerCaptureHelper.GetCapturingElement() instead.");
			m_ReleaseMouseWarningEmitted = true;
		}
	}
}
