using System.Collections.Generic;
using UnityEngine;

public class BuildBotMove
{
	private enum MoveType
	{
		PATH_ONLY,
		ROTATION_ONLY,
		WORK_MOVE
	}

	private WorldBuildRobot robot;

	private List<Vector3> _path;

	private List<Vector3> _rotationList;

	private float moveEndHeight;

	private bool goTarget;

	private int _rotationIndex;

	private int _pathIndex;

	private float moveDis;

	private float buildTime;

	private float totalTime;

	private float time;

	private float heightLen;

	private Vector3 m_LastPos;

	private Vector3 m_NextPos;

	private Vector3 m_LastRotation;

	private Vector3 m_NextRotation;

	private Vector3 m_LootAtPos;

	private MoveType _moveType;

	public BuildBotMove(WorldBuildRobot robot)
	{
		this.robot = robot;
		_path = new List<Vector3>();
		_rotationList = new List<Vector3>();
	}

	public void InitPath(Vector3 startPos, Vector3 endPos, bool isGoTarget = false, float moveTime = 0f)
	{
		_moveType = MoveType.PATH_ONLY;
		_path.Clear();
		_path.Add(startPos);
		_path.Add(endPos);
		_pathIndex = 0;
		goTarget = isGoTarget;
		heightLen = (endPos - startPos).y;
		moveDis = 0f;
		totalTime = 0f;
		buildTime = moveTime;
		time = 0f;
		m_LastPos = robot.transform.position;
		m_NextPos = _path[0];
	}

	public void InitRotation(Vector3 startRotation, Vector3 endRotation)
	{
		_moveType = MoveType.ROTATION_ONLY;
		_rotationList.Clear();
		if (startRotation.y - endRotation.y > 180f)
		{
			endRotation += Vector3.up * 360f;
		}
		else if (startRotation.y - endRotation.y < -180f)
		{
			startRotation += Vector3.up * 360f;
		}
		_rotationList.Add(startRotation);
		_rotationList.Add(endRotation);
		totalTime = 0f;
		_rotationIndex = 1;
		time = 0f;
		m_LastRotation = _rotationList[0];
		m_NextRotation = _rotationList[1];
	}

	public void InitWorkMove(List<Vector3> workPath, Vector3 centerPos, float endHeight, float workTime)
	{
		_moveType = MoveType.WORK_MOVE;
		_path = workPath;
		moveEndHeight = endHeight;
		m_LootAtPos = centerPos;
		_pathIndex = 0;
		totalTime = 0f;
		moveDis = 0f;
		time = 0f;
		buildTime = workTime;
		m_LastPos = robot.transform.position;
		m_NextPos = _path[0];
	}

	public bool UpdateMove(float deltaTime)
	{
		time += deltaTime;
		totalTime += deltaTime;
		return _moveType switch
		{
			MoveType.PATH_ONLY => PathOnlyMove(), 
			MoveType.ROTATION_ONLY => RotationOnlyMove(), 
			MoveType.WORK_MOVE => WorkMove(), 
			_ => false, 
		};
	}

	private float calDistance()
	{
		float num = Mathf.Min((totalTime - Time.deltaTime) * 12f, robot.flySpeed);
		float num2 = Mathf.Min(totalTime * 12f, robot.flySpeed);
		return (num + num2) / 2f * Time.deltaTime;
	}

	private bool PathOnlyMove()
	{
		if (_pathIndex >= _path.Count)
		{
			return false;
		}
		float magnitude = (m_NextPos - m_LastPos).magnitude;
		moveDis += calDistance();
		Vector3 nextPos = m_NextPos;
		while (moveDis >= magnitude)
		{
			if (++_pathIndex >= _path.Count)
			{
				nextPos = new Vector3(nextPos.x, 0f, nextPos.z);
				if (goTarget)
				{
					nextPos += new Vector3(0f, robot.MoveStartPos.y, 0f) + robot.curve.goTargetPosYCurve.Evaluate(totalTime / buildTime) * heightLen * Vector3.up;
				}
				else
				{
					nextPos += new Vector3(0f, robot.BackStartPos.y, 0f) + robot.curve.goBackPosYCurve.Evaluate(totalTime / buildTime) * heightLen * Vector3.up;
				}
				robot.transform.position = nextPos;
				return false;
			}
			m_LastPos = m_NextPos;
			m_NextPos = _path[_pathIndex];
			moveDis -= magnitude;
			magnitude = (m_NextPos - m_LastPos).magnitude;
		}
		float t = moveDis / magnitude;
		nextPos = Vector3.Lerp(m_LastPos, m_NextPos, t);
		nextPos = new Vector3(nextPos.x, 0f, nextPos.z);
		if (goTarget)
		{
			nextPos += new Vector3(0f, robot.MoveStartPos.y, 0f) + robot.curve.goTargetPosYCurve.Evaluate(totalTime / buildTime) * heightLen * Vector3.up;
		}
		else
		{
			nextPos += new Vector3(0f, robot.BackStartPos.y, 0f) + robot.curve.goBackPosYCurve.Evaluate(totalTime / buildTime) * heightLen * Vector3.up;
		}
		robot.transform.position = nextPos;
		return true;
	}

	private bool RotationOnlyMove()
	{
		if (_rotationIndex >= _rotationList.Count)
		{
			return false;
		}
		float num = time * robot.rotationSpeed / (m_NextRotation - m_LastRotation).magnitude;
		robot.transform.rotation = Quaternion.Euler(Vector3.Lerp(m_LastRotation, m_NextRotation, num));
		if (num >= 1f)
		{
			time = 0f;
			if (++_rotationIndex >= _rotationList.Count)
			{
				return false;
			}
			m_LastRotation = m_NextRotation;
			m_NextRotation = _rotationList[_rotationIndex];
		}
		return true;
	}

	private bool WorkMove()
	{
		float num = totalTime / buildTime;
		float magnitude = (m_NextPos - m_LastPos).magnitude;
		float num2 = robot.curve.buildingMoveSpeedCurve.Evaluate((float)_pathIndex / (float)_path.Count) * robot.workSpeed;
		moveDis += Time.deltaTime * num2;
		while (moveDis >= magnitude)
		{
			_pathIndex++;
			m_LastPos = m_NextPos;
			m_NextPos = _path[_pathIndex];
			if (_pathIndex + 1 >= _path.Count)
			{
				_pathIndex = 1;
			}
			moveDis -= magnitude;
			magnitude = (m_NextPos - m_LastPos).magnitude;
		}
		float t = moveDis / magnitude;
		robot.transform.position = Vector3.Lerp(m_LastPos, m_NextPos, t) + Vector3.up * (num * moveEndHeight);
		Vector3 eulerAngles = robot.transform.rotation.eulerAngles;
		robot.transform.LookAt(m_LootAtPos);
		robot.transform.rotation = Quaternion.Euler(new Vector3(eulerAngles.x, robot.transform.rotation.eulerAngles.y, eulerAngles.z));
		return true;
	}
}
