namespace UnityEngine.UIElements;

public static class PointerCaptureHelper
{
	private static PointerDispatchState GetStateFor(IEventHandler handler)
	{
		return (!(handler is VisualElement visualElement)) ? null : visualElement.panel?.dispatcher?.pointerState;
	}

	public static bool HasPointerCapture(this IEventHandler handler, int pointerId)
	{
		return GetStateFor(handler)?.HasPointerCapture(handler, pointerId) ?? false;
	}

	public static void CapturePointer(this IEventHandler handler, int pointerId)
	{
		GetStateFor(handler)?.CapturePointer(handler, pointerId);
	}

	public static void ReleasePointer(this IEventHandler handler, int pointerId)
	{
		GetStateFor(handler)?.ReleasePointer(handler, pointerId);
	}

	public static IEventHandler GetCapturingElement(this IPanel panel, int pointerId)
	{
		return panel?.dispatcher?.pointerState.GetCapturingElement(pointerId);
	}

	public static void ReleasePointer(this IPanel panel, int pointerId)
	{
		panel?.dispatcher?.pointerState.ReleasePointer(pointerId);
	}

	internal static void ActivateCompatibilityMouseEvents(this IPanel panel, int pointerId)
	{
		panel?.dispatcher?.pointerState.ActivateCompatibilityMouseEvents(pointerId);
	}

	internal static void PreventCompatibilityMouseEvents(this IPanel panel, int pointerId)
	{
		panel?.dispatcher?.pointerState.PreventCompatibilityMouseEvents(pointerId);
	}

	internal static bool ShouldSendCompatibilityMouseEvents(this IPanel panel, IPointerEvent evt)
	{
		return panel?.dispatcher?.pointerState.ShouldSendCompatibilityMouseEvents(evt) ?? true;
	}

	internal static void ProcessPointerCapture(this IPanel panel, int pointerId)
	{
		panel?.dispatcher?.pointerState.ProcessPointerCapture(pointerId);
	}

	internal static void ResetPointerDispatchState(this IPanel panel)
	{
		panel?.dispatcher?.pointerState.Reset();
	}
}
