using System.Collections.Generic;

namespace UnityEngine.UIElements;

public abstract class MouseManipulator : Manipulator
{
	private ManipulatorActivationFilter m_currentActivator;

	public List<ManipulatorActivationFilter> activators { get; private set; }

	protected MouseManipulator()
	{
		activators = new List<ManipulatorActivationFilter>();
	}

	protected bool CanStartManipulation(IMouseEvent e)
	{
		foreach (ManipulatorActivationFilter activator in activators)
		{
			if (activator.Matches(e))
			{
				m_currentActivator = activator;
				return true;
			}
		}
		return false;
	}

	protected bool CanStopManipulation(IMouseEvent e)
	{
		if (e == null)
		{
			return false;
		}
		return e.button == (int)m_currentActivator.button;
	}
}
