namespace UnityEngine.XR.WSA;

internal class SimulatedBody
{
	public Vector3 position
	{
		get
		{
			return HolographicAutomation.GetBodyPosition();
		}
		set
		{
			HolographicAutomation.SetBodyPosition(value);
		}
	}

	public float rotation
	{
		get
		{
			return HolographicAutomation.GetBodyRotation();
		}
		set
		{
			HolographicAutomation.SetBodyRotation(value);
		}
	}

	public float height
	{
		get
		{
			return HolographicAutomation.GetBodyHeight();
		}
		set
		{
			HolographicAutomation.SetBodyHeight(value);
		}
	}
}
