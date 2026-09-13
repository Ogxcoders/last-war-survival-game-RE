namespace UnityEngine.XR.WSA;

internal class SimulatedSpatialController
{
	public Handedness m_ControllerHandednss;

	public Quaternion orientation
	{
		get
		{
			return HolographicAutomation.GetHandOrientation(m_ControllerHandednss);
		}
		set
		{
			HolographicAutomation.TrySetHandOrientation(m_ControllerHandednss, value);
		}
	}

	public Vector3 position
	{
		get
		{
			return HolographicAutomation.GetControllerPosition(m_ControllerHandednss);
		}
		set
		{
			HolographicAutomation.TrySetControllerPosition(m_ControllerHandednss, value);
		}
	}

	public bool activated
	{
		get
		{
			return HolographicAutomation.GetControllerActivated(m_ControllerHandednss);
		}
		set
		{
			HolographicAutomation.TrySetControllerActivated(m_ControllerHandednss, value);
		}
	}

	public bool visible => HolographicAutomation.GetControllerVisible(m_ControllerHandednss);

	internal SimulatedSpatialController(Handedness controller)
	{
		m_ControllerHandednss = controller;
	}

	public void EnsureVisible()
	{
		HolographicAutomation.TryEnsureControllerVisible(m_ControllerHandednss);
	}

	public void PerformControllerPress(SimulatedControllerPress button)
	{
		HolographicAutomation.PerformButtonPress(m_ControllerHandednss, button);
	}
}
