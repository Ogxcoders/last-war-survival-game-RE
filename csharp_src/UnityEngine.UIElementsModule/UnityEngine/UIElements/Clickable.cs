using System;

namespace UnityEngine.UIElements;

public class Clickable : MouseManipulator
{
	private readonly long m_Delay;

	private readonly long m_Interval;

	private IVisualElementScheduledItem m_Repeater;

	protected bool active { get; set; }

	public Vector2 lastMousePosition { get; private set; }

	public event Action<EventBase> clickedWithEventInfo;

	public event Action clicked;

	public Clickable(Action handler, long delay, long interval)
		: this(handler)
	{
		m_Delay = delay;
		m_Interval = interval;
		active = false;
	}

	public Clickable(Action<EventBase> handler)
	{
		this.clickedWithEventInfo = handler;
		base.activators.Add(new ManipulatorActivationFilter
		{
			button = MouseButton.LeftMouse
		});
	}

	public Clickable(Action handler)
	{
		this.clicked = handler;
		base.activators.Add(new ManipulatorActivationFilter
		{
			button = MouseButton.LeftMouse
		});
		active = false;
	}

	private void OnTimer(TimerState timerState)
	{
		if ((this.clicked != null || this.clickedWithEventInfo != null) && IsRepeatable())
		{
			if (base.target.ContainsPoint(lastMousePosition))
			{
				Invoke(null);
				base.target.pseudoStates |= PseudoStates.Active;
			}
			else
			{
				base.target.pseudoStates &= ~PseudoStates.Active;
			}
		}
	}

	private bool IsRepeatable()
	{
		return m_Delay > 0 || m_Interval > 0;
	}

	protected override void RegisterCallbacksOnTarget()
	{
		base.target.RegisterCallback<MouseDownEvent>(OnMouseDown);
		base.target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
		base.target.RegisterCallback<MouseUpEvent>(OnMouseUp);
	}

	protected override void UnregisterCallbacksFromTarget()
	{
		base.target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
		base.target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
		base.target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
	}

	private void Invoke(EventBase evt)
	{
		if (this.clicked != null)
		{
			this.clicked();
		}
		this.clickedWithEventInfo?.Invoke(evt);
	}

	protected void OnMouseDown(MouseDownEvent evt)
	{
		if (evt == null || !CanStartManipulation(evt))
		{
			return;
		}
		active = true;
		base.target.CaptureMouse();
		lastMousePosition = evt.localMousePosition;
		if (IsRepeatable())
		{
			if (base.target.ContainsPoint(evt.localMousePosition))
			{
				Invoke(evt);
			}
			if (m_Repeater == null)
			{
				m_Repeater = base.target.schedule.Execute(OnTimer).Every(m_Interval).StartingIn(m_Delay);
			}
			else
			{
				m_Repeater.ExecuteLater(m_Delay);
			}
		}
		base.target.pseudoStates |= PseudoStates.Active;
		evt.StopImmediatePropagation();
	}

	protected void OnMouseMove(MouseMoveEvent evt)
	{
		if (evt != null && active)
		{
			lastMousePosition = evt.localMousePosition;
			if (base.target.ContainsPoint(evt.localMousePosition))
			{
				base.target.pseudoStates |= PseudoStates.Active;
			}
			else
			{
				base.target.pseudoStates &= ~PseudoStates.Active;
			}
			evt.StopPropagation();
		}
	}

	protected void OnMouseUp(MouseUpEvent evt)
	{
		if (evt == null || !active || !CanStopManipulation(evt))
		{
			return;
		}
		active = false;
		base.target.ReleaseMouse();
		base.target.pseudoStates &= ~PseudoStates.Active;
		if (IsRepeatable())
		{
			if (m_Repeater != null)
			{
				m_Repeater.Pause();
			}
		}
		else if (base.target.ContainsPoint(evt.localMousePosition))
		{
			Invoke(evt);
		}
		evt.StopPropagation();
	}
}
