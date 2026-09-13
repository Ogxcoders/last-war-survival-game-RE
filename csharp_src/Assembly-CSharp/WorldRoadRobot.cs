using System.Collections.Generic;
using UnityEngine;

public class WorldRoadRobot : MonoBehaviour
{
	public enum State
	{
		Idle,
		TakeOff,
		TakeOffRotation,
		GoTarget,
		ApproachTarget,
		WorkRotation,
		Work,
		GoBackRotation,
		GoBack,
		LandingRotation,
		Landing
	}

	public GameObject go;

	public new Transform transform;

	public Animator animator;

	public List<RobotPathNode> WorkingPath;

	private Dictionary<State, IRoadBotState> _botStateManager;

	[SerializeField]
	public BuildingRobotCurve curve;

	private State _curState;

	public State _lastState;

	private float normalSpeed;

	private bool createFinish;

	private float smallCornerWaitTime;

	private float midCornerSpeed;

	private float hugeCornerSpeed;

	public float flyHeight;

	public float buildHeight;

	public float flySpeed;

	public float rotationSpeed;

	public Vector3 BotCenterPos;

	public Vector3 MoveStartPos;

	public Vector3 MoveEndPos;

	public Vector3 FirstRoadPos;

	public Vector3 BackStartPos;

	public Vector3 ApproachDir;

	public bool isOther;

	public bool canAutoUpdate;

	public Dictionary<State, float> stateMachineDic = new Dictionary<State, float>();

	public Transform scan;

	public static readonly int TakeOff = Animator.StringToHash("shengkong");

	public static readonly int Standby = Animator.StringToHash("daiji");

	public static readonly int Flying = Animator.StringToHash("feixing");

	public static readonly int Building = Animator.StringToHash("jianzao");

	public static readonly int Landing = Animator.StringToHash("jiangluo");

	public RoadBotMove BotMove { get; private set; }

	public long uuid { get; private set; }

	public void Init(long uuid)
	{
		this.uuid = uuid;
		flyHeight = 4.81f;
		buildHeight = 0f;
		BotMove = new RoadBotMove(this);
		WorkingPath = new List<RobotPathNode>();
		flySpeed = 15f;
		rotationSpeed = 270f;
		_botStateManager = new Dictionary<State, IRoadBotState>
		{
			{
				State.Idle,
				new RoadBotIdleState()
			},
			{
				State.TakeOff,
				new RoadBotTakeOffState()
			},
			{
				State.TakeOffRotation,
				new RoadBotTakeOffRotationState()
			},
			{
				State.GoTarget,
				new RoadBotGoTargetState()
			},
			{
				State.ApproachTarget,
				new RoadBotApproachTargetState()
			},
			{
				State.WorkRotation,
				new RoadBotWorkRotation()
			},
			{
				State.Work,
				new RoadBotWorkState()
			},
			{
				State.GoBackRotation,
				new RoadBotGoBackRotation()
			},
			{
				State.GoBack,
				new RoadRobotGoBackState()
			},
			{
				State.LandingRotation,
				new RoadBotLandingRotationState()
			},
			{
				State.Landing,
				new RoadBotLandingState()
			}
		};
		_curState = State.Idle;
		_botStateManager[_curState].OnEnter(this);
	}

	public void StartBuild(int[] roads, bool IsOther)
	{
		if (roads == null || roads.Length == 0)
		{
			return;
		}
		createFinish = false;
		isOther = IsOther;
		BotCenterPos = Vector3.zero;
		if (!isOther)
		{
			LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(477000);
			if (buildingDataByBuildId != null && buildingDataByBuildId.state != 2)
			{
				BotCenterPos = PathUtils.GetDroneCenterPos(SceneManager.World.TileIndexToWorld(buildingDataByBuildId.pointId));
			}
			else
			{
				buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(10100000);
				if (buildingDataByBuildId != null && buildingDataByBuildId.state != 2)
				{
					BotCenterPos = PathUtils.GetDroneCenterPos(SceneManager.World.TileIndexToWorld(buildingDataByBuildId.pointId));
				}
				else
				{
					isOther = true;
				}
			}
		}
		InitSpeedAndWaitTime(roads.Length);
		BuildMovePath(BotCenterPos, roads);
		GetAnimatorTime();
		if (isOther)
		{
			ChangeState(State.Work);
		}
		else
		{
			ChangeState(State.TakeOff);
		}
		createFinish = true;
	}

	public void BotUpdate(Vector3 scanPos)
	{
		if (!(go == null) && createFinish && _curState != State.Idle && base.gameObject.activeSelf)
		{
			_botStateManager[_curState].OnUpdate(this, Time.deltaTime, scanPos);
		}
	}

	public void UnInit()
	{
		createFinish = false;
		Object.Destroy(transform.gameObject);
	}

	private void BuildMovePath(Vector3 startPos, int[] roads)
	{
		if (roads == null || roads.Length == 0)
		{
			return;
		}
		MoveStartPos = startPos + Vector3.up * flyHeight;
		List<Vector3> list = new List<Vector3>();
		Vector3 vector = Vector3.right;
		for (int i = 0; i < roads.Length; i++)
		{
			int index = roads[i];
			if (i == 1)
			{
				vector = (SceneManager.World.TileIndexToWorld(index) - list[0]).normalized;
			}
			list.Add(SceneManager.World.TileIndexToWorld(index));
		}
		FirstRoadPos = list[0] - vector * (SceneManager.World.TileSize * 0.5f);
		WorkingPath.Clear();
		_ = Vector3.zero;
		_ = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		Vector3 zero = Vector3.zero;
		if (list.Count == 1)
		{
			Vector3 vector3 = list[0];
			vector2 = Vector3.Normalize(new Vector3(SceneManager.World.TileSize, 0f, 0f));
			zero = Vector3.Cross(Vector3.up, vector2) * SceneManager.World.TileSize;
			WorkingPath.Add(new RobotPathNode(vector3 + zero, RobotPathNode.NodeType.NORMAL));
			WorkingPath.Add(new RobotPathNode(vector3 + zero + new Vector3(SceneManager.World.TileSize, 0f, 0f), RobotPathNode.NodeType.NORMAL));
		}
		else
		{
			for (int j = 1; j < list.Count; j++)
			{
				vector2 = Vector3.Normalize(list[j] - list[j - 1]);
				zero = Vector3.Cross(Vector3.up, vector2) * SceneManager.World.TileSize;
				Vector3 vector4 = list[j - 1] + zero;
				if (j >= 2)
				{
					RobotPathNode robotPathNode = WorkingPath[WorkingPath.Count - 1];
					if (list[j - 2] == vector4)
					{
						WorkingPath[WorkingPath.Count - 1] = new RobotPathNode(robotPathNode.pos, RobotPathNode.NodeType.SMALL_CORNER);
						WorkingPath.Add(new RobotPathNode(robotPathNode.pos, RobotPathNode.NodeType.SMALL_CORNER));
						continue;
					}
				}
				if (WorkingPath.Count >= 1)
				{
					RobotPathNode robotPathNode2 = WorkingPath[WorkingPath.Count - 1];
					if (Vector3.Distance(vector4, robotPathNode2.pos) > Mathf.Epsilon)
					{
						checkAndAddMidHugeCornerPath(vector4, robotPathNode2.pos, list[j - 1]);
					}
					else
					{
						WorkingPath.Add(new RobotPathNode(vector4, RobotPathNode.NodeType.SMALL_CORNER));
					}
				}
				else
				{
					WorkingPath.Add(new RobotPathNode(vector4, RobotPathNode.NodeType.NORMAL));
				}
			}
			WorkingPath.Add(new RobotPathNode(WorkingPath[WorkingPath.Count - 1].pos + vector2 * SceneManager.World.TileSize, RobotPathNode.NodeType.NORMAL));
			SmoothPath();
		}
		MoveEndPos = WorkingPath[0].pos + new Vector3(0f, buildHeight, 0f);
	}

	private void checkAndAddMidHugeCornerPath(Vector3 pTemp, Vector3 prePos, Vector3 preRoad)
	{
		Vector3 vector = pTemp - prePos;
		if (Mathf.Approximately(Mathf.Abs(vector.x), SceneManager.World.TileSize * 2f) || Mathf.Approximately(Mathf.Abs(vector.z), SceneManager.World.TileSize * 2f))
		{
			WorkingPath[WorkingPath.Count - 1] = new RobotPathNode(prePos, RobotPathNode.NodeType.HUGE_CORNER);
			if (Mathf.Approximately(Mathf.Abs(vector.x), SceneManager.World.TileSize * 2f))
			{
				float num = ((vector.x > 0f) ? SceneManager.World.TileSize : (0f - SceneManager.World.TileSize));
				WorkingPath.Add(new RobotPathNode(new Vector3(prePos.x + num, prePos.y, prePos.z), RobotPathNode.NodeType.HUGE_CORNER));
				WorkingPath.Add(new RobotPathNode(new Vector3(prePos.x + num * 2f, prePos.y, prePos.z), RobotPathNode.NodeType.HUGE_CORNER));
			}
			else
			{
				float num2 = ((vector.z > 0f) ? SceneManager.World.TileSize : (0f - SceneManager.World.TileSize));
				WorkingPath.Add(new RobotPathNode(new Vector3(prePos.x, prePos.y, prePos.z + num2), RobotPathNode.NodeType.HUGE_CORNER));
				WorkingPath.Add(new RobotPathNode(new Vector3(prePos.x, prePos.y, prePos.z + num2 * 2f), RobotPathNode.NodeType.HUGE_CORNER));
			}
			WorkingPath.Add(new RobotPathNode(pTemp, RobotPathNode.NodeType.HUGE_CORNER));
		}
		else if (Mathf.Approximately(Mathf.Abs(vector.x), SceneManager.World.TileSize) && Mathf.Approximately(Mathf.Abs(vector.z), SceneManager.World.TileSize))
		{
			WorkingPath[WorkingPath.Count - 1] = new RobotPathNode(prePos, RobotPathNode.NodeType.MID_CORNER);
			if (Mathf.Approximately(prePos.x + SceneManager.World.TileSize, preRoad.x))
			{
				float num3 = ((vector.z > 0f) ? SceneManager.World.TileSize : (0f - SceneManager.World.TileSize));
				WorkingPath.Add(new RobotPathNode(new Vector3(prePos.x, prePos.y, prePos.z + num3), RobotPathNode.NodeType.MID_CORNER));
			}
			else
			{
				float num4 = ((vector.x > 0f) ? SceneManager.World.TileSize : (0f - SceneManager.World.TileSize));
				WorkingPath.Add(new RobotPathNode(new Vector3(prePos.x + num4, prePos.y, prePos.z), RobotPathNode.NodeType.MID_CORNER));
			}
			WorkingPath.Add(new RobotPathNode(pTemp, RobotPathNode.NodeType.MID_CORNER));
		}
		else
		{
			WorkingPath.Add(new RobotPathNode(pTemp, RobotPathNode.NodeType.NORMAL));
		}
	}

	private void SmoothPath()
	{
		float num = 0.3f;
		float num2 = 60f;
		Vector3[] array = new Vector3[WorkingPath.Count - 1];
		for (int i = 1; i < WorkingPath.Count; i++)
		{
			array[i - 1] = WorkingPath[i].pos - WorkingPath[i - 1].pos;
		}
		List<RobotPathNode> list = new List<RobotPathNode>();
		list.Add(WorkingPath[0]);
		for (int j = 1; j < WorkingPath.Count - 1; j++)
		{
			if (Vector3.Angle(array[j], array[j - 1]) > 1f)
			{
				float num3 = num2 / normalSpeed;
				if (RobotPathNode.IsHugeCorner(WorkingPath[j - 1], WorkingPath[j]))
				{
					num3 = num2 / hugeCornerSpeed;
				}
				else if (RobotPathNode.IsMidCorner(WorkingPath[j - 1], WorkingPath[j]))
				{
					num3 = num2 / midCornerSpeed;
				}
				Bezier bezier = new Bezier(WorkingPath[j - 1].pos, array[j - 1] * num, array[j] * (0f - num), WorkingPath[j].pos);
				float num4 = 1f / num3;
				for (int k = 1; (float)k <= num3; k++)
				{
					Vector3 pointAtTime = bezier.GetPointAtTime((float)k * num4);
					list.Add(new RobotPathNode(pointAtTime, WorkingPath[j].type));
				}
			}
			else
			{
				list.Add(WorkingPath[j]);
			}
		}
		list.Add(WorkingPath[WorkingPath.Count - 1]);
		WorkingPath = list;
	}

	public bool isWorking()
	{
		return _curState == State.Work;
	}

	public float calMoveTimeByPathNode(RobotPathNode now, RobotPathNode next)
	{
		float num = Vector3.Distance(now.pos, next.pos);
		if (RobotPathNode.IsSmallCorner(now, next) || num <= Mathf.Epsilon)
		{
			return smallCornerWaitTime;
		}
		if (RobotPathNode.IsHugeCorner(now, next))
		{
			return num / hugeCornerSpeed;
		}
		if (RobotPathNode.IsMidCorner(now, next))
		{
			return num / midCornerSpeed;
		}
		return num / normalSpeed;
	}

	private void InitSpeedAndWaitTime(int roadCount)
	{
		float num = (float)roadCount * GameEntry.Lua.CallWithReturn<float>("CSharpCallLuaInterface.GetRoadBuildTime");
		normalSpeed = (float)((roadCount == 1) ? roadCount : (roadCount - 1)) * SceneManager.World.TileSize / num;
		smallCornerWaitTime = SceneManager.World.TileSize / normalSpeed;
		midCornerSpeed = normalSpeed * 2f;
		hugeCornerSpeed = normalSpeed * 3f;
	}

	public void ChangeState(State state)
	{
		_botStateManager[_curState].OnLeave(this);
		_lastState = _curState;
		_curState = state;
		_botStateManager[_curState].OnEnter(this);
	}

	private void GetAnimatorTime()
	{
		stateMachineDic.Clear();
		AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
		foreach (AnimationClip animationClip in animationClips)
		{
			if (animationClip.name == "shengkong")
			{
				stateMachineDic[State.TakeOff] = animationClip.length + 0.2f;
			}
			if (animationClip.name == "jiangluo")
			{
				stateMachineDic[State.Landing] = animationClip.length;
			}
		}
	}
}
