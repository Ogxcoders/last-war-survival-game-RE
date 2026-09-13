#define UNITY_ASSERTIONS
namespace UnityEngine.UIElements;

internal class ElementUnderPointer
{
	private VisualElement[] m_PendingTopElementUnderPointer = new VisualElement[PointerId.maxPointers];

	private VisualElement[] m_TopElementUnderPointer = new VisualElement[PointerId.maxPointers];

	private IPointerEvent[] m_TriggerPointerEvent = new IPointerEvent[PointerId.maxPointers];

	private IMouseEvent[] m_TriggerMouseEvent = new IMouseEvent[PointerId.maxPointers];

	private Vector2[] m_PickingPointerPositions = new Vector2[PointerId.maxPointers];

	private bool[] m_IsPickingPointerTemporaries = new bool[PointerId.maxPointers];

	internal VisualElement GetTopElementUnderPointer(int pointerId, out Vector2 pickPosition, out bool isTemporary)
	{
		pickPosition = m_PickingPointerPositions[pointerId];
		isTemporary = m_IsPickingPointerTemporaries[pointerId];
		return m_PendingTopElementUnderPointer[pointerId];
	}

	internal VisualElement GetTopElementUnderPointer(int pointerId)
	{
		return m_PendingTopElementUnderPointer[pointerId];
	}

	internal void SetElementUnderPointer(VisualElement newElementUnderPointer, int pointerId, Vector2 pointerPos)
	{
		Debug.Assert(pointerId >= 0);
		VisualElement visualElement = m_TopElementUnderPointer[pointerId];
		m_IsPickingPointerTemporaries[pointerId] = false;
		m_PickingPointerPositions[pointerId] = pointerPos;
		if (newElementUnderPointer != visualElement)
		{
			m_PendingTopElementUnderPointer[pointerId] = newElementUnderPointer;
			m_TriggerPointerEvent[pointerId] = null;
			m_TriggerMouseEvent[pointerId] = null;
		}
	}

	private Vector2 GetEventPointerPosition(EventBase triggerEvent)
	{
		if (triggerEvent is IPointerEvent pointerEvent)
		{
			return new Vector2(pointerEvent.position.x, pointerEvent.position.y);
		}
		if (!(triggerEvent is IMouseEvent { mousePosition: var mousePosition }))
		{
			return new Vector2(float.MinValue, float.MinValue);
		}
		return mousePosition;
	}

	internal void SetTemporaryElementUnderPointer(VisualElement newElementUnderPointer, EventBase triggerEvent)
	{
		SetElementUnderPointer(newElementUnderPointer, triggerEvent, temporary: true);
	}

	internal void SetElementUnderPointer(VisualElement newElementUnderPointer, EventBase triggerEvent)
	{
		SetElementUnderPointer(newElementUnderPointer, triggerEvent, temporary: false);
	}

	private void SetElementUnderPointer(VisualElement newElementUnderPointer, EventBase triggerEvent, bool temporary)
	{
		int num = -1;
		if (triggerEvent is IPointerEvent)
		{
			num = ((IPointerEvent)triggerEvent).pointerId;
		}
		else if (triggerEvent is IMouseEvent)
		{
			num = PointerId.mousePointerId;
		}
		Debug.Assert(num >= 0);
		m_IsPickingPointerTemporaries[num] = temporary;
		m_PickingPointerPositions[num] = GetEventPointerPosition(triggerEvent);
		VisualElement visualElement = m_TopElementUnderPointer[num];
		if (newElementUnderPointer != visualElement)
		{
			m_PendingTopElementUnderPointer[num] = newElementUnderPointer;
			if (m_TriggerPointerEvent[num] == null && triggerEvent is IPointerEvent)
			{
				m_TriggerPointerEvent[num] = triggerEvent as IPointerEvent;
			}
			if (m_TriggerMouseEvent[num] == null && triggerEvent is IMouseEvent)
			{
				m_TriggerMouseEvent[num] = triggerEvent as IMouseEvent;
			}
		}
	}

	internal void CommitElementUnderPointers(EventDispatcher dispatcher)
	{
		for (int i = 0; i < m_TopElementUnderPointer.Length; i++)
		{
			IPointerEvent pointerEvent = m_TriggerPointerEvent[i];
			VisualElement visualElement = m_TopElementUnderPointer[i];
			VisualElement visualElement2 = m_PendingTopElementUnderPointer[i];
			if (visualElement2 == visualElement)
			{
				if (pointerEvent != null)
				{
					Vector3 position = pointerEvent.position;
					m_PickingPointerPositions[i] = new Vector2(position.x, position.y);
				}
				else if (m_TriggerMouseEvent[i] != null)
				{
					m_PickingPointerPositions[i] = m_TriggerMouseEvent[i].mousePosition;
				}
				continue;
			}
			m_TopElementUnderPointer[i] = visualElement2;
			if (pointerEvent == null && m_TriggerMouseEvent[i] == null)
			{
				using (new EventDispatcherGate(dispatcher))
				{
					Vector2 pointerPosition = PointerDeviceState.GetPointerPosition(i);
					PointerEventsHelper.SendOverOut(visualElement, visualElement2, null, pointerPosition, i);
					PointerEventsHelper.SendEnterLeave<PointerLeaveEvent, PointerEnterEvent>(visualElement, visualElement2, null, pointerPosition, i);
					m_PickingPointerPositions[i] = pointerPosition;
					if (i == PointerId.mousePointerId)
					{
						MouseEventsHelper.SendMouseOverMouseOut(visualElement, visualElement2, null, pointerPosition);
						MouseEventsHelper.SendEnterLeave<MouseLeaveEvent, MouseEnterEvent>(visualElement, visualElement2, null, pointerPosition);
					}
				}
			}
			if (pointerEvent != null)
			{
				Vector3 position2 = pointerEvent.position;
				m_PickingPointerPositions[i] = new Vector2(position2.x, position2.y);
				if (pointerEvent is EventBase eventBase && (eventBase.eventTypeId == EventBase<PointerMoveEvent>.TypeId() || eventBase.eventTypeId == EventBase<PointerDownEvent>.TypeId() || eventBase.eventTypeId == EventBase<PointerUpEvent>.TypeId() || eventBase.eventTypeId == EventBase<PointerCancelEvent>.TypeId()))
				{
					using (new EventDispatcherGate(dispatcher))
					{
						PointerEventsHelper.SendOverOut(visualElement, visualElement2, pointerEvent, position2, i);
						PointerEventsHelper.SendEnterLeave<PointerLeaveEvent, PointerEnterEvent>(visualElement, visualElement2, pointerEvent, position2, i);
					}
				}
			}
			m_TriggerPointerEvent[i] = null;
			IMouseEvent mouseEvent = m_TriggerMouseEvent[i];
			if (mouseEvent == null)
			{
				continue;
			}
			Vector2 mousePosition = mouseEvent.mousePosition;
			m_PickingPointerPositions[i] = mousePosition;
			if (mouseEvent is EventBase eventBase2)
			{
				if (eventBase2.eventTypeId == EventBase<MouseMoveEvent>.TypeId() || eventBase2.eventTypeId == EventBase<MouseDownEvent>.TypeId() || eventBase2.eventTypeId == EventBase<MouseUpEvent>.TypeId() || eventBase2.eventTypeId == EventBase<WheelEvent>.TypeId())
				{
					using (new EventDispatcherGate(dispatcher))
					{
						MouseEventsHelper.SendMouseOverMouseOut(visualElement, visualElement2, mouseEvent, mousePosition);
						MouseEventsHelper.SendEnterLeave<MouseLeaveEvent, MouseEnterEvent>(visualElement, visualElement2, mouseEvent, mousePosition);
					}
				}
				else if (eventBase2.eventTypeId == EventBase<MouseEnterWindowEvent>.TypeId() || eventBase2.eventTypeId == EventBase<MouseLeaveWindowEvent>.TypeId())
				{
					using (new EventDispatcherGate(dispatcher))
					{
						PointerEventsHelper.SendOverOut(visualElement, visualElement2, null, mousePosition, i);
						PointerEventsHelper.SendEnterLeave<PointerLeaveEvent, PointerEnterEvent>(visualElement, visualElement2, null, mousePosition, i);
						if (i == PointerId.mousePointerId)
						{
							MouseEventsHelper.SendMouseOverMouseOut(visualElement, visualElement2, mouseEvent, mousePosition);
							MouseEventsHelper.SendEnterLeave<MouseLeaveEvent, MouseEnterEvent>(visualElement, visualElement2, mouseEvent, mousePosition);
						}
					}
				}
				else if (eventBase2.eventTypeId == EventBase<DragUpdatedEvent>.TypeId() || eventBase2.eventTypeId == EventBase<DragExitedEvent>.TypeId())
				{
					using (new EventDispatcherGate(dispatcher))
					{
						PointerEventsHelper.SendOverOut(visualElement, visualElement2, null, mousePosition, i);
						PointerEventsHelper.SendEnterLeave<PointerLeaveEvent, PointerEnterEvent>(visualElement, visualElement2, null, mousePosition, i);
						MouseEventsHelper.SendMouseOverMouseOut(visualElement, visualElement2, mouseEvent, mousePosition);
						MouseEventsHelper.SendEnterLeave<MouseLeaveEvent, MouseEnterEvent>(visualElement, visualElement2, mouseEvent, mousePosition);
						MouseEventsHelper.SendEnterLeave<DragLeaveEvent, DragEnterEvent>(visualElement, visualElement2, mouseEvent, mousePosition);
					}
				}
			}
			m_TriggerMouseEvent[i] = null;
		}
	}
}
