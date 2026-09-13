namespace UnityEngine.XR.WSA;

internal class SimulatedHand
{
	public Handedness m_Hand;

	public Vector3 position
	{
		get
		{
			return HolographicAutomation.GetHandPosition(m_Hand);
		}
		set
		{
			HolographicAutomation.SetHandPosition(m_Hand, value);
		}
	}

	public bool activated
	{
		get
		{
			return HolographicAutomation.GetHandActivated(m_Hand);
		}
		set
		{
			HolographicAutomation.SetHandActivated(m_Hand, value);
		}
	}

	public bool visible => HolographicAutomation.GetHandVisible(m_Hand);

	internal SimulatedHand(Handedness hand)
	{
		m_Hand = hand;
	}

	public void EnsureVisible()
	{
		HolographicAutomation.EnsureHandVisible(m_Hand);
	}

	public void PerformGesture(SimulatedGesture gesture)
	{
		HolographicAutomation.PerformGesture(m_Hand, gesture);
	}
}
