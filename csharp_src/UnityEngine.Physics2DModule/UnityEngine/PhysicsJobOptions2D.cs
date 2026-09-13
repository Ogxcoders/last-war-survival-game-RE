using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
[NativeClass("PhysicsJobOptions2D", "struct PhysicsJobOptions2D;")]
[NativeHeader("Modules/Physics2D/Public/Physics2DSettings.h")]
public struct PhysicsJobOptions2D
{
	private bool m_UseMultithreading;

	private bool m_UseConsistencySorting;

	private int m_InterpolationPosesPerJob;

	private int m_NewContactsPerJob;

	private int m_CollideContactsPerJob;

	private int m_ClearFlagsPerJob;

	private int m_ClearBodyForcesPerJob;

	private int m_SyncDiscreteFixturesPerJob;

	private int m_SyncContinuousFixturesPerJob;

	private int m_FindNearestContactsPerJob;

	private int m_UpdateTriggerContactsPerJob;

	private int m_IslandSolverCostThreshold;

	private int m_IslandSolverBodyCostScale;

	private int m_IslandSolverContactCostScale;

	private int m_IslandSolverJointCostScale;

	private int m_IslandSolverBodiesPerJob;

	private int m_IslandSolverContactsPerJob;

	public bool useMultithreading
	{
		get
		{
			return m_UseMultithreading;
		}
		set
		{
			m_UseMultithreading = value;
		}
	}

	public bool useConsistencySorting
	{
		get
		{
			return m_UseConsistencySorting;
		}
		set
		{
			m_UseConsistencySorting = value;
		}
	}

	public int interpolationPosesPerJob
	{
		get
		{
			return m_InterpolationPosesPerJob;
		}
		set
		{
			m_InterpolationPosesPerJob = value;
		}
	}

	public int newContactsPerJob
	{
		get
		{
			return m_NewContactsPerJob;
		}
		set
		{
			m_NewContactsPerJob = value;
		}
	}

	public int collideContactsPerJob
	{
		get
		{
			return m_CollideContactsPerJob;
		}
		set
		{
			m_CollideContactsPerJob = value;
		}
	}

	public int clearFlagsPerJob
	{
		get
		{
			return m_ClearFlagsPerJob;
		}
		set
		{
			m_ClearFlagsPerJob = value;
		}
	}

	public int clearBodyForcesPerJob
	{
		get
		{
			return m_ClearBodyForcesPerJob;
		}
		set
		{
			m_ClearBodyForcesPerJob = value;
		}
	}

	public int syncDiscreteFixturesPerJob
	{
		get
		{
			return m_SyncDiscreteFixturesPerJob;
		}
		set
		{
			m_SyncDiscreteFixturesPerJob = value;
		}
	}

	public int syncContinuousFixturesPerJob
	{
		get
		{
			return m_SyncContinuousFixturesPerJob;
		}
		set
		{
			m_SyncContinuousFixturesPerJob = value;
		}
	}

	public int findNearestContactsPerJob
	{
		get
		{
			return m_FindNearestContactsPerJob;
		}
		set
		{
			m_FindNearestContactsPerJob = value;
		}
	}

	public int updateTriggerContactsPerJob
	{
		get
		{
			return m_UpdateTriggerContactsPerJob;
		}
		set
		{
			m_UpdateTriggerContactsPerJob = value;
		}
	}

	public int islandSolverCostThreshold
	{
		get
		{
			return m_IslandSolverCostThreshold;
		}
		set
		{
			m_IslandSolverCostThreshold = value;
		}
	}

	public int islandSolverBodyCostScale
	{
		get
		{
			return m_IslandSolverBodyCostScale;
		}
		set
		{
			m_IslandSolverBodyCostScale = value;
		}
	}

	public int islandSolverContactCostScale
	{
		get
		{
			return m_IslandSolverContactCostScale;
		}
		set
		{
			m_IslandSolverContactCostScale = value;
		}
	}

	public int islandSolverJointCostScale
	{
		get
		{
			return m_IslandSolverJointCostScale;
		}
		set
		{
			m_IslandSolverJointCostScale = value;
		}
	}

	public int islandSolverBodiesPerJob
	{
		get
		{
			return m_IslandSolverBodiesPerJob;
		}
		set
		{
			m_IslandSolverBodiesPerJob = value;
		}
	}

	public int islandSolverContactsPerJob
	{
		get
		{
			return m_IslandSolverContactsPerJob;
		}
		set
		{
			m_IslandSolverContactsPerJob = value;
		}
	}
}
