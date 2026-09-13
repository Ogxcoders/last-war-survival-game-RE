using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal class EventDebuggerEventRecord
{
	public string eventBaseName { get; private set; }

	public long eventTypeId { get; private set; }

	public ulong eventId { get; private set; }

	private ulong triggerEventId { get; set; }

	private long timestamp { get; set; }

	public IEventHandler target { get; private set; }

	private List<IEventHandler> skipElements { get; set; }

	public bool hasUnderlyingPhysicalEvent { get; private set; }

	private bool isPropagationStopped { get; set; }

	private bool isImmediatePropagationStopped { get; set; }

	private bool isDefaultPrevented { get; set; }

	public PropagationPhase propagationPhase { get; private set; }

	private IEventHandler currentTarget { get; set; }

	private bool dispatch { get; set; }

	private Vector2 originalMousePosition { get; set; }

	public EventModifiers modifiers { get; private set; }

	public Vector2 mousePosition { get; private set; }

	public int clickCount { get; private set; }

	public int button { get; private set; }

	public Vector3 delta { get; private set; }

	public char character { get; private set; }

	public KeyCode keyCode { get; private set; }

	public string commandName { get; private set; }

	private void Init(EventBase evt)
	{
		eventBaseName = evt.GetType().Name;
		eventTypeId = evt.eventTypeId;
		eventId = evt.eventId;
		triggerEventId = evt.triggerEventId;
		timestamp = evt.timestamp;
		target = evt.target;
		skipElements = evt.skipElements;
		isPropagationStopped = evt.isPropagationStopped;
		isImmediatePropagationStopped = evt.isImmediatePropagationStopped;
		isDefaultPrevented = evt.isDefaultPrevented;
		IMouseEvent mouseEvent = evt as IMouseEvent;
		IMouseEventInternal mouseEventInternal = evt as IMouseEventInternal;
		hasUnderlyingPhysicalEvent = mouseEvent != null && mouseEventInternal != null && mouseEventInternal.triggeredByOS;
		propagationPhase = evt.propagationPhase;
		originalMousePosition = evt.originalMousePosition;
		currentTarget = evt.currentTarget;
		dispatch = evt.dispatch;
		if (mouseEvent != null)
		{
			modifiers = mouseEvent.modifiers;
			mousePosition = mouseEvent.mousePosition;
			button = mouseEvent.button;
			clickCount = mouseEvent.clickCount;
		}
		if (evt is IKeyboardEvent keyboardEvent)
		{
			character = keyboardEvent.character;
			keyCode = keyboardEvent.keyCode;
		}
		if (evt is ICommandEvent commandEvent)
		{
			commandName = commandEvent.commandName;
		}
	}

	public EventDebuggerEventRecord(EventBase evt)
	{
		Init(evt);
	}

	public string TimestampString()
	{
		long ticks = (long)((float)timestamp / 1000f * 10000000f);
		return new DateTime(ticks).ToString("HH:mm:ss.ffffff");
	}
}
