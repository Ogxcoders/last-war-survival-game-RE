using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal static class PointerEventsHelper
{
	internal static void SendEnterLeave<TLeaveEvent, TEnterEvent>(VisualElement previousTopElementUnderPointer, VisualElement currentTopElementUnderPointer, IPointerEvent triggerEvent, Vector2 position, int pointerId) where TLeaveEvent : PointerEventBase<TLeaveEvent>, new() where TEnterEvent : PointerEventBase<TEnterEvent>, new()
	{
		if (previousTopElementUnderPointer != null && previousTopElementUnderPointer.panel == null)
		{
			previousTopElementUnderPointer = null;
		}
		int num = 0;
		VisualElement visualElement;
		for (visualElement = previousTopElementUnderPointer; visualElement != null; visualElement = visualElement.hierarchy.parent)
		{
			num++;
		}
		int num2 = 0;
		VisualElement visualElement2;
		for (visualElement2 = currentTopElementUnderPointer; visualElement2 != null; visualElement2 = visualElement2.hierarchy.parent)
		{
			num2++;
		}
		visualElement = previousTopElementUnderPointer;
		visualElement2 = currentTopElementUnderPointer;
		while (num > num2)
		{
			using (TLeaveEvent val = PointerEventBase<TLeaveEvent>.GetPooled(triggerEvent, position, pointerId))
			{
				val.target = visualElement;
				visualElement.SendEvent(val);
			}
			num--;
			visualElement = visualElement.hierarchy.parent;
		}
		List<VisualElement> list = VisualElementListPool.Get(num2);
		while (num2 > num)
		{
			list.Add(visualElement2);
			num2--;
			visualElement2 = visualElement2.hierarchy.parent;
		}
		while (visualElement != visualElement2)
		{
			using (TLeaveEvent val2 = PointerEventBase<TLeaveEvent>.GetPooled(triggerEvent, position, pointerId))
			{
				val2.target = visualElement;
				visualElement.SendEvent(val2);
			}
			list.Add(visualElement2);
			visualElement = visualElement.hierarchy.parent;
			visualElement2 = visualElement2.hierarchy.parent;
		}
		for (int num3 = list.Count - 1; num3 >= 0; num3--)
		{
			using TEnterEvent val3 = PointerEventBase<TEnterEvent>.GetPooled(triggerEvent, position, pointerId);
			val3.target = list[num3];
			list[num3].SendEvent(val3);
		}
		VisualElementListPool.Release(list);
	}

	internal static void SendOverOut(VisualElement previousTopElementUnderPointer, VisualElement currentTopElementUnderPointer, IPointerEvent triggerEvent, Vector2 position, int pointerId)
	{
		if (previousTopElementUnderPointer != null && previousTopElementUnderPointer.panel != null)
		{
			using PointerOutEvent pointerOutEvent = PointerEventBase<PointerOutEvent>.GetPooled(triggerEvent, position, pointerId);
			pointerOutEvent.target = previousTopElementUnderPointer;
			previousTopElementUnderPointer.SendEvent(pointerOutEvent);
		}
		if (currentTopElementUnderPointer != null)
		{
			using (PointerOverEvent pointerOverEvent = PointerEventBase<PointerOverEvent>.GetPooled(triggerEvent, position, pointerId))
			{
				pointerOverEvent.target = currentTopElementUnderPointer;
				currentTopElementUnderPointer.SendEvent(pointerOverEvent);
			}
		}
	}
}
