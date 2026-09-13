using System.Collections.Generic;
using UnityEngine;

public class WorldPeopleTruckBase : MonoBehaviour
{
	public enum States
	{
		Init,
		GotoTarget,
		WaitTarget,
		GoBack,
		Circle
	}

	public Animator Anim;

	public SimpleAnimation Anim2;

	public float Speed;

	public float ApproachTargetOffset;

	public float TurnRadius;

	public float TurnAccuracy;

	public int targetPos;

	public float ObjOffset;

	public float ViaductOffset;

	public float MainInnerOffset;

	public float MainOutterOffset;

	public GameObject ShowObject;

	public bool m_bFocusBus;

	public bool isRandom;

	public List<List<Vector2Int>> pathList;

	protected int _nowPathIndex;

	protected Dictionary<States, ITruckPeopleSate> _Tsm;

	[SerializeField]
	public WorldTruckViaductCurve curve;

	public bool hasInit;

	public bool isCircle;

	public float radius;

	public Vector3 mainPos;

	public bool isInner;

	public float initAngle;

	protected string _curAnim;

	public TruckAndPeopleMove TruckAndPeopleMove { get; private set; }

	public WorldTruckCircleMove WorldTruckCircleMove { get; private set; }

	protected SceneInterface Scene { get; set; }

	public TruckManagerBase Manager { get; set; }

	public States _curState { get; protected set; }

	protected void BaseInit()
	{
		Scene = SceneManager.World;
		TruckAndPeopleMove = new TruckAndPeopleMove(this);
		WorldTruckCircleMove = new WorldTruckCircleMove(this);
		if ((bool)Anim2)
		{
			Anim2.cullingMode = AnimatorCullingMode.CullCompletely;
		}
		else if ((bool)Anim)
		{
			Anim.cullingMode = AnimatorCullingMode.CullCompletely;
		}
	}

	protected void BaseShow()
	{
		_nowPathIndex = 0;
	}

	protected void BaseUpdate()
	{
		if (m_bFocusBus)
		{
			Scene.Lookat(base.transform.position);
		}
	}

	public void ChangeState(States state)
	{
		if (_Tsm.ContainsKey(_curState))
		{
			_Tsm[_curState].OnLeave(this);
			_curState = state;
			if (_Tsm.ContainsKey(_curState))
			{
				_Tsm[_curState].OnEnter(this);
			}
		}
	}

	public List<Vector2Int> GetNowPath()
	{
		if (_nowPathIndex < pathList.Count)
		{
			return pathList[_nowPathIndex];
		}
		return new List<Vector2Int>();
	}

	public void ReachTarget()
	{
		_nowPathIndex++;
	}

	public bool IsPathFinish()
	{
		return _nowPathIndex >= pathList.Count;
	}

	public void PlayAnim(string anim)
	{
		_curAnim = anim;
		if ((bool)Anim2)
		{
			Anim2.Play(anim);
		}
	}
}
