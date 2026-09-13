using System.Collections.Generic;
using UnityEngine;

public class RoadBotMove
{
	private enum MoveType
	{
		PATH_ONLY,
		ROTATION_ONLY,
		WORK_MOVE
	}

	private WorldRoadRobot robot;

	private List<Vector3> _path;

	private List<Vector3> _rotationList;

	private List<RobotPathNode> _workPath;

	private bool goTarget;

	private float moveDis;

	private float moveTargetTime;

	private float heightLen;

	private int _rotationIndex;

	private int _pathIndex;

	private float time;

	private float totalTime;

	private float segmentTime;

	private Vector3 m_LastPos;

	private Vector3 m_NextPos;

	private Vector3 m_LastRotation;

	private Vector3 m_NextRotation;

	private bool canRotation;

	private MoveType _moveType;

	public RoadBotMove(WorldRoadRobot robot)
	{
		this.robot = robot;
		_path = new List<Vector3>();
		_rotationList = new List<Vector3>();
		_workPath = new List<RobotPathNode>();
	}

	public void InitPath(Vector3 startPos, Vector3 endPos, bool isGoTarget = false, float moveTime = 0f)
	{
		_moveType = MoveType.PATH_ONLY;
		_path.Clear();
		_path.Add(startPos);
		_path.Add(endPos);
		_pathIndex = 0;
		time = 0f;
		totalTime = 0f;
		goTarget = isGoTarget;
		heightLen = (endPos - startPos).y;
		moveDis = 0f;
		moveTargetTime = moveTime;
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
		_rotationIndex = 1;
		time = 0f;
		m_LastRotation = _rotationList[0];
		m_NextRotation = _rotationList[1];
	}

	public void InitWorkMove(List<RobotPathNode> workPath)
	{
		_moveType = MoveType.WORK_MOVE;
		_workPath = workPath;
		_pathIndex = 0;
		time = 0f;
		m_LastPos = _workPath[0].pos;
		m_NextPos = _workPath[1].pos;
		segmentTime = robot.calMoveTimeByPathNode(_workPath[0], _workPath[1]);
		canRotation = true;
	}

	public bool UpdateMove(float deltaTime, Vector3 scanPos)
	{
		totalTime += deltaTime;
		time += deltaTime;
		return _moveType switch
		{
			MoveType.PATH_ONLY => PathOnlyMove(), 
			MoveType.ROTATION_ONLY => RotationOnlyMove(), 
			MoveType.WORK_MOVE => WorkMove(scanPos), 
			_ => false, 
		};
	}

	private float calPathOnlyDistance()
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
		moveDis += calPathOnlyDistance();
		Vector3 nextPos = m_NextPos;
		while (moveDis >= magnitude)
		{
			if (++_pathIndex >= _path.Count)
			{
				nextPos = new Vector3(nextPos.x, 0f, nextPos.z);
				if (goTarget)
				{
					nextPos += new Vector3(0f, robot.MoveStartPos.y, 0f) + robot.curve.goTargetPosYCurve.Evaluate(totalTime / moveTargetTime) * heightLen * Vector3.up;
				}
				else
				{
					nextPos += new Vector3(0f, robot.BackStartPos.y, 0f) + robot.curve.goBackPosYCurve.Evaluate(totalTime / moveTargetTime) * heightLen * Vector3.up;
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
			nextPos += new Vector3(0f, robot.MoveStartPos.y, 0f) + robot.curve.goTargetPosYCurve.Evaluate(totalTime / moveTargetTime) * heightLen * Vector3.up;
		}
		else
		{
			nextPos += new Vector3(0f, robot.BackStartPos.y, 0f) + robot.curve.goBackPosYCurve.Evaluate(totalTime / moveTargetTime) * heightLen * Vector3.up;
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

	private bool WorkMove(Vector3 scanPos)
	{
		if (time >= segmentTime)
		{
			canRotation = true;
			time = 0f;
			_pathIndex++;
			if (_pathIndex >= _workPath.Count - 1)
			{
				return false;
			}
			m_LastPos = _workPath[_pathIndex].pos;
			m_NextPos = _workPath[_pathIndex + 1].pos;
			if (RobotPathNode.IsSmallCorner(_workPath[_pathIndex], _workPath[_pathIndex + 1]))
			{
				canRotation = false;
			}
			segmentTime = robot.calMoveTimeByPathNode(_workPath[_pathIndex], _workPath[_pathIndex + 1]);
		}
		if (canRotation)
		{
			Vector3 vector = Vector3.Lerp(m_LastPos, m_NextPos, time / segmentTime);
			robot.transform.position = vector + new Vector3(0f, robot.buildHeight, 0f);
		}
		if (!scanPos.Equals(Vector3.zero))
		{
			robot.transform.LookAt(scanPos);
		}
		return true;
	}
}
