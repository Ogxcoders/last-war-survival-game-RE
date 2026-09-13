namespace UnityEngine.AI;

public struct NavMeshLinkData
{
	private Vector3 m_StartPosition;

	private Vector3 m_EndPosition;

	private float m_CostModifier;

	private int m_Bidirectional;

	private float m_Width;

	private int m_Area;

	private int m_AgentTypeID;

	public Vector3 startPosition
	{
		get
		{
			return m_StartPosition;
		}
		set
		{
			m_StartPosition = value;
		}
	}

	public Vector3 endPosition
	{
		get
		{
			return m_EndPosition;
		}
		set
		{
			m_EndPosition = value;
		}
	}

	public float costModifier
	{
		get
		{
			return m_CostModifier;
		}
		set
		{
			m_CostModifier = value;
		}
	}

	public bool bidirectional
	{
		get
		{
			return m_Bidirectional != 0;
		}
		set
		{
			m_Bidirectional = (value ? 1 : 0);
		}
	}

	public float width
	{
		get
		{
			return m_Width;
		}
		set
		{
			m_Width = value;
		}
	}

	public int area
	{
		get
		{
			return m_Area;
		}
		set
		{
			m_Area = value;
		}
	}

	public int agentTypeID
	{
		get
		{
			return m_AgentTypeID;
		}
		set
		{
			m_AgentTypeID = value;
		}
	}
}
