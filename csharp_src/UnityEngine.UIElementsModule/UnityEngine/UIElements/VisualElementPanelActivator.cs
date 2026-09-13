namespace UnityEngine.UIElements;

internal class VisualElementPanelActivator
{
	private IVisualElementPanelActivatable m_Activatable;

	public bool isActive { get; private set; }

	public bool isDetaching { get; private set; }

	public VisualElementPanelActivator(IVisualElementPanelActivatable activatable)
	{
		m_Activatable = activatable;
	}

	public void SetActive(bool action)
	{
		if (isActive != action)
		{
			isActive = action;
			if (isActive)
			{
				m_Activatable.element.RegisterCallback<AttachToPanelEvent>(OnEnter);
				m_Activatable.element.RegisterCallback<DetachFromPanelEvent>(OnLeave);
				SendActivation();
			}
			else
			{
				m_Activatable.element.UnregisterCallback<AttachToPanelEvent>(OnEnter);
				m_Activatable.element.UnregisterCallback<DetachFromPanelEvent>(OnLeave);
				SendDeactivation();
			}
		}
	}

	public void SendActivation()
	{
		if (m_Activatable.CanBeActivated())
		{
			m_Activatable.OnPanelActivate();
		}
	}

	public void SendDeactivation()
	{
		if (m_Activatable.CanBeActivated())
		{
			m_Activatable.OnPanelDeactivate();
		}
	}

	private void OnEnter(AttachToPanelEvent evt)
	{
		if (isActive)
		{
			SendActivation();
		}
	}

	private void OnLeave(DetachFromPanelEvent evt)
	{
		if (isActive)
		{
			isDetaching = true;
			try
			{
				SendDeactivation();
			}
			finally
			{
				isDetaching = false;
			}
		}
	}
}
