namespace UnityEngine.XR.WSA;

internal class SimulatedHead
{
	public float diameter
	{
		get
		{
			return HolographicAutomation.GetHeadDiameter();
		}
		set
		{
			HolographicAutomation.SetHeadDiameter(value);
		}
	}

	public Vector3 eulerAngles
	{
		get
		{
			return HolographicAutomation.GetHeadRotation();
		}
		set
		{
			HolographicAutomation.SetHeadRotation(value);
		}
	}
}
