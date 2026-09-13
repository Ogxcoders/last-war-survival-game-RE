using System.Collections.Generic;
using UnityEngine;

public class WorldBuildRobot : MonoBehaviour
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

	private Dictionary<State, IBuildBotState> _botStateManager;

	private InstanceRequest request;

	private const float accuracy = 80f;

	public float calAccuracy;

	public new Transform transform;

	public Animator animator;

	[SerializeField]
	public BuildingRobotCurve curve;

	private State _curState;

	public State _lastState;

	public float workSpeed = 4f;

	public float flySpeed = 15f;

	public float rotationSpeed = 270f;

	public float flyHeight = 4.81f;

	public Vector3 MoveStartPos;

	public Vector3 MoveEndPos;

	public Vector3 WorkEndPos;

	public Vector3 posCenter;

	public Vector3 BotCenterPos;

	public Vector3 ApproachDir;

	public Vector3 BackStartPos;

	public List<Vector3> WorkingPath;

	public float buildTime;

	public float moveTargetTime;

	public Dictionary<State, float> stateMachineDic = new Dictionary<State, float>();

	public long uuid;

	public bool isTransit;

	public bool isOther;

	public Transform scan;

	public static readonly int TakeOff = Animator.StringToHash("shengkong");

	public static readonly int Standby = Animator.StringToHash("daiji");

	public static readonly int Flying = Animator.StringToHash("feixing");

	public static readonly int Building = Animator.StringToHash("jianzao");

	public static readonly int Landing = Animator.StringToHash("jiangluo");

	public BuildBotMove BotMove { get; private set; }

	public void Init()
	{
		flySpeed = 15f;
		rotationSpeed = 270f;
		flyHeight = 4.81f;
		workSpeed = 4f;
		calAccuracy = 80f / workSpeed;
		BotMove = new BuildBotMove(this);
		WorkingPath = new List<Vector3>();
		_botStateManager = new Dictionary<State, IBuildBotState>
		{
			{
				State.Idle,
				new BuildBotIdleState()
			},
			{
				State.TakeOff,
				new BuildBotTakeOffState()
			},
			{
				State.TakeOffRotation,
				new BuildBotTakeOffRotationState()
			},
			{
				State.GoTarget,
				new BuildBotGoTargetState()
			},
			{
				State.ApproachTarget,
				new BuildBotApproachTargetState()
			},
			{
				State.WorkRotation,
				new BuildingBotWorkRotation()
			},
			{
				State.Work,
				new BuildBotWorkState()
			},
			{
				State.GoBackRotation,
				new BuildingBotGoBackRotation()
			},
			{
				State.GoBack,
				new BuildRobotGoBackState()
			},
			{
				State.LandingRotation,
				new BuildBotLandingRotationState()
			},
			{
				State.Landing,
				new BuildBotLandingState()
			}
		};
		_curState = State.Idle;
		_botStateManager[_curState].OnEnter(this);
	}

	public void StartBuild(long bUuid, Vector3 startPosition, Vector3 buildPos, float buildHeight, float buildingTime, int tileSizeX, int tileSizeY, bool transit = false)
	{
		isOther = false;
		uuid = bUuid;
		BotCenterPos = PathUtils.GetDroneCenterPos(startPosition);
		isTransit = transit;
		BuildMovePath(BotCenterPos, buildPos, buildHeight, tileSizeX, tileSizeY);
		transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
		buildTime = buildingTime;
		GetAnimatorTime();
		if (isTransit)
		{
			ChangeState(State.Work);
		}
		else
		{
			ChangeState(State.TakeOff);
		}
	}

	public void StarBuildOther(long bUuid, Vector3 startPosition, Vector3 buildPos, float buildHeight, float buildingTime, int tileSizeX, int tileSizeY)
	{
		isOther = true;
		uuid = bUuid;
		BotCenterPos = PathUtils.GetDroneCenterPos(startPosition);
		transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
		isTransit = false;
		BuildMovePath(BotCenterPos, buildPos, buildHeight, tileSizeX, tileSizeY);
		buildTime = buildingTime;
		GetAnimatorTime();
		ChangeState(State.Work);
	}

	public void BotUpdate(float deltaTime)
	{
		if (_curState != State.Idle && base.gameObject.activeSelf)
		{
			MoveUpdate(deltaTime);
		}
	}

	public void UnInit()
	{
		Object.Destroy(transform.gameObject);
	}

	public void ChangeState(State state)
	{
		if (state != _curState)
		{
			_botStateManager[_curState].OnLeave(this);
			_lastState = _curState;
			_curState = state;
			_botStateManager[_curState].OnEnter(this);
		}
	}

	public bool isWorking()
	{
		return _curState == State.Work;
	}

	private void BuildMovePath(Vector3 startPos, Vector3 buildPos, float buildHeight, int tileSizeX, int tileSizeY)
	{
		MoveStartPos = startPos + Vector3.up * flyHeight;
		float num = (float)tileSizeX * 0.5f - 0.5f;
		float num2 = (float)tileSizeY * 0.5f - 0.5f;
		posCenter = buildPos - new Vector3(num * SceneManager.World.TileSize, 0f, num2 * SceneManager.World.TileSize);
		bool flag = MoveStartPos.x <= posCenter.x;
		Vector3 vector = posCenter + Vector3.right * (SceneManager.World.TileSize * (1f + num));
		Vector3 vector2 = posCenter + Vector3.back * (SceneManager.World.TileSize * (1f + num2));
		Vector3 vector3 = posCenter + Vector3.left * (SceneManager.World.TileSize * (1f + num));
		Vector3 vector4 = posCenter + Vector3.forward * (SceneManager.World.TileSize * (1f + num2));
		MoveEndPos = (flag ? vector3 : vector);
		moveTargetTime = PathUtils.CalRobotMoveNeedTime(MoveStartPos, MoveEndPos);
		WorkEndPos = MoveEndPos + Vector3.up * buildHeight;
		Vector3 vector5 = (flag ? vector : vector3);
		float num3 = SceneManager.World.TileSize * (float)(1 + tileSizeX) * (Mathf.Sqrt(2f) - 1f) * 4f / 3f;
		Bezier bezier = new Bezier(MoveEndPos, (MoveEndPos - vector2).normalized * num3, (vector2 - vector5).normalized * num3, vector2);
		List<Vector3> list = new List<Vector3>();
		float num4 = 1f / calAccuracy;
		for (int i = 0; (float)i <= calAccuracy; i++)
		{
			if (i < 1 || !((float)i <= 0.3f * calAccuracy))
			{
				Vector3 pointAtTime = bezier.GetPointAtTime((float)i * num4);
				list.Add(pointAtTime);
			}
		}
		WorkingPath.AddRange(list);
		WorkingPath.RemoveAt(WorkingPath.Count - 1);
		list.Reverse();
		WorkingPath.AddRange(list);
		list.Clear();
		WorkingPath.RemoveAt(WorkingPath.Count - 1);
		bezier = new Bezier(MoveEndPos, (MoveEndPos - vector4).normalized * num3, (vector4 - vector5).normalized * num3, vector4);
		for (int j = 0; (float)j <= calAccuracy; j++)
		{
			if (j < 1 || !((float)j <= 0.3f * calAccuracy))
			{
				Vector3 pointAtTime2 = bezier.GetPointAtTime((float)j * num4);
				list.Add(pointAtTime2);
			}
		}
		WorkingPath.AddRange(list);
		WorkingPath.RemoveAt(WorkingPath.Count - 1);
		list.Reverse();
		WorkingPath.AddRange(list);
		list.Clear();
	}

	private void MoveUpdate(float deltaTime)
	{
		_botStateManager[_curState].OnUpdate(this, deltaTime);
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

	public void ChangeRobotFinishTime(float finishTime)
	{
		buildTime = finishTime;
		ChangeState(_curState);
	}

	public State GetCurState()
	{
		return _curState;
	}
}
