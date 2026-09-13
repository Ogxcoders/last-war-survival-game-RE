using UnityEngine.Scripting;

namespace UnityEngine.XR;

[UsedByNativeCode]
public struct XRNodeState
{
	private XRNode m_Type;

	private AvailableTrackingData m_AvailableFields;

	private Vector3 m_Position;

	private Quaternion m_Rotation;

	private Vector3 m_Velocity;

	private Vector3 m_AngularVelocity;

	private Vector3 m_Acceleration;

	private Vector3 m_AngularAcceleration;

	private int m_Tracked;

	private ulong m_UniqueID;

	public ulong uniqueID
	{
		get
		{
			return m_UniqueID;
		}
		set
		{
			m_UniqueID = value;
		}
	}

	public XRNode nodeType
	{
		get
		{
			return m_Type;
		}
		set
		{
			m_Type = value;
		}
	}

	public bool tracked
	{
		get
		{
			return m_Tracked == 1;
		}
		set
		{
			m_Tracked = (value ? 1 : 0);
		}
	}

	public Vector3 position
	{
		set
		{
			m_Position = value;
			m_AvailableFields |= AvailableTrackingData.PositionAvailable;
		}
	}

	public Quaternion rotation
	{
		set
		{
			m_Rotation = value;
			m_AvailableFields |= AvailableTrackingData.RotationAvailable;
		}
	}

	public Vector3 velocity
	{
		set
		{
			m_Velocity = value;
			m_AvailableFields |= AvailableTrackingData.VelocityAvailable;
		}
	}

	public Vector3 angularVelocity
	{
		set
		{
			m_AngularVelocity = value;
			m_AvailableFields |= AvailableTrackingData.AngularVelocityAvailable;
		}
	}

	public Vector3 acceleration
	{
		set
		{
			m_Acceleration = value;
			m_AvailableFields |= AvailableTrackingData.AccelerationAvailable;
		}
	}

	public Vector3 angularAcceleration
	{
		set
		{
			m_AngularAcceleration = value;
			m_AvailableFields |= AvailableTrackingData.AngularAccelerationAvailable;
		}
	}

	public bool TryGetPosition(out Vector3 position)
	{
		return TryGet(m_Position, AvailableTrackingData.PositionAvailable, out position);
	}

	public bool TryGetRotation(out Quaternion rotation)
	{
		return TryGet(m_Rotation, AvailableTrackingData.RotationAvailable, out rotation);
	}

	public bool TryGetVelocity(out Vector3 velocity)
	{
		return TryGet(m_Velocity, AvailableTrackingData.VelocityAvailable, out velocity);
	}

	public bool TryGetAngularVelocity(out Vector3 angularVelocity)
	{
		return TryGet(m_AngularVelocity, AvailableTrackingData.AngularVelocityAvailable, out angularVelocity);
	}

	public bool TryGetAcceleration(out Vector3 acceleration)
	{
		return TryGet(m_Acceleration, AvailableTrackingData.AccelerationAvailable, out acceleration);
	}

	public bool TryGetAngularAcceleration(out Vector3 angularAcceleration)
	{
		return TryGet(m_AngularAcceleration, AvailableTrackingData.AngularAccelerationAvailable, out angularAcceleration);
	}

	private bool TryGet(Vector3 inValue, AvailableTrackingData availabilityFlag, out Vector3 outValue)
	{
		if ((m_AvailableFields & availabilityFlag) > AvailableTrackingData.None)
		{
			outValue = inValue;
			return true;
		}
		outValue = Vector3.zero;
		return false;
	}

	private bool TryGet(Quaternion inValue, AvailableTrackingData availabilityFlag, out Quaternion outValue)
	{
		if ((m_AvailableFields & availabilityFlag) > AvailableTrackingData.None)
		{
			outValue = inValue;
			return true;
		}
		outValue = Quaternion.identity;
		return false;
	}
}
