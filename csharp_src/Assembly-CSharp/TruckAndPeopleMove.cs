using System.Collections.Generic;
using UnityEngine;

public class TruckAndPeopleMove
{
	public enum MoveStates
	{
		Moving,
		TurnLeft,
		TurnRight,
		LeftReturn,
		RightReturn
	}

	public enum MoveUpdateResult
	{
		MOVING,
		WAITING,
		FINISH_PATH
	}

	private WorldPeopleTruckBase itemBase;

	private List<Vector3> _path;

	private int _pathIndex;

	private float _tempTime;

	private float animatorTime;

	private Vector3 m_LastPos;

	private Vector3 m_NextPos;

	private bool pause;

	private float lastRotation;

	private List<MoveStates> animatorStateList;

	private MoveStates curState;

	private static readonly string Moving = "run";

	public TruckAndPeopleMove(WorldPeopleTruckBase itemBase)
	{
		this.itemBase = itemBase;
	}

	private float getOffset(CityRoadPathParam.PathType pathType, bool useInner)
	{
		switch (pathType)
		{
		case CityRoadPathParam.PathType.VIADUCT:
			return itemBase.ViaductOffset;
		case CityRoadPathParam.PathType.MAIN_ROAD:
			if (!useInner)
			{
				return itemBase.MainOutterOffset;
			}
			return itemBase.MainInnerOffset;
		default:
			return itemBase.ObjOffset;
		}
	}

	private List<Vector3> SmoothTurnPath(List<Vector2Int> path)
	{
		if (path == null)
		{
			return new List<Vector3>();
		}
		float num = 50f * itemBase.TurnAccuracy;
		bool useInner = Random.Range(0, 2) == 0;
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = 1; i < path.Count; i++)
		{
			list.Add(path[i - 1]);
			if (path[i - 1].x == path[i].x)
			{
				int num2 = path[i].y - path[i - 1].y;
				if (num2 != 0)
				{
					int num3 = num2 / Mathf.Abs(num2);
					int num4 = 0;
					while (Mathf.Abs(num2) > 1)
					{
						num4++;
						list.Add(new Vector2Int(path[i - 1].x, path[i - 1].y + num3 * num4));
						num2 -= num3;
					}
				}
			}
			else if (path[i - 1].y == path[i].y)
			{
				int num5 = path[i].x - path[i - 1].x;
				int num6 = num5 / Mathf.Abs(num5);
				int num7 = 0;
				while (Mathf.Abs(num5) > 1)
				{
					num7++;
					list.Add(new Vector2Int(path[i - 1].x + num6 * num7, path[i - 1].y));
					num5 -= num6;
				}
			}
		}
		list.Add(path[path.Count - 1]);
		Vector3[] array = new Vector3[list.Count - 1];
		CityRoadPathParam.PathType[] array2 = new CityRoadPathParam.PathType[list.Count];
		for (int j = 1; j < list.Count; j++)
		{
			if (j == 1)
			{
				array2[0] = CityRoadPathParam.genPathType(new Vector2Int(-1, -1), list[j - 1]);
				Vector3 vector = SceneManager.World.TileToWorld(list[j - 1]);
				Vector3 vector2 = SceneManager.World.TileToWorld(list[j]);
				array[j - 1] = vector2 - vector;
			}
			else
			{
				array[j - 1] = SceneManager.World.TileToWorld(list[j]) - SceneManager.World.TileToWorld(list[j - 1]);
			}
			array2[j] = CityRoadPathParam.genPathType(list[j - 1], list[j]);
		}
		List<Vector3> list2 = new List<Vector3>();
		for (int k = 1; k < list.Count; k++)
		{
			float offset = getOffset(array2[k - 1], useInner);
			float offset2 = getOffset(array2[k], useInner);
			Vector3 vector3;
			Vector3 vector4;
			if (array[k - 1].x < 0f)
			{
				vector3 = Vector3.back * offset;
				vector4 = Vector3.back * offset2;
			}
			else if (array[k - 1].x > 0f)
			{
				vector3 = Vector3.forward * offset;
				vector4 = Vector3.forward * offset2;
			}
			else if (array[k - 1].z < 0f)
			{
				vector3 = Vector3.right * offset;
				vector4 = Vector3.right * offset2;
			}
			else
			{
				vector3 = Vector3.left * offset;
				vector4 = Vector3.left * offset2;
			}
			if (k == 1)
			{
				Vector3 a = SceneManager.World.TileToWorld(list[k - 1]) + vector3;
				Vector3 b = SceneManager.World.TileToWorld(list[k]) + vector4;
				a = Vector3.Lerp(a, b, 0.5f);
				list2.Add(Vector3.Lerp(a, b, 0.5f));
			}
			else
			{
				Vector3 a2 = SceneManager.World.TileToWorld(list[k - 1]) + vector3;
				Vector3 b2 = SceneManager.World.TileToWorld(list[k]) + vector4;
				list2.Add(Vector3.Lerp(a2, b2, 0.5f));
			}
			if (k == list.Count - 1)
			{
				list2.Add(SceneManager.World.TileToWorld(list[k]) + vector4);
			}
		}
		List<Vector3> list3 = new List<Vector3>();
		list3.Add(list2[0]);
		float turnRadius = itemBase.TurnRadius;
		for (int l = 1; l < list.Count - 1; l++)
		{
			if (array2[l] == CityRoadPathParam.PathType.VIADUCT)
			{
				int m;
				for (m = l + 1; m < list2.Count; m++)
				{
					if (array2[m] != CityRoadPathParam.PathType.VIADUCT)
					{
						m--;
						break;
					}
				}
				if (m - 1 >= 0 && m < list2.Count)
				{
					Vector3 a3 = list2[l - 1];
					Vector3 b3 = list2[m];
					float num8 = num * (float)(m - l + 1);
					float num9 = 1f / num8;
					for (int n = 0; (float)n <= num8; n++)
					{
						Vector3 item = Vector3.Lerp(a3, b3, (float)n * num9) + Vector3.up * itemBase.curve.viaductYPos.Evaluate((float)n * num9) * 1.5f;
						list3.Add(item);
					}
					list3.Add(list2[m]);
					l = m;
				}
			}
			else if (Vector3.Angle(array[l], array[l - 1]) > 1f)
			{
				float num10 = Mathf.Abs(list2[l].x - list2[l - 1].x);
				Bezier bezier = new Bezier(list2[l - 1], array[l - 1] * (turnRadius * num10), array[l] * ((0f - turnRadius) * num10), list2[l]);
				float num11 = 1f / num;
				for (int num12 = 1; (float)num12 <= num; num12++)
				{
					Vector3 pointAtTime = bezier.GetPointAtTime((float)num12 * num11);
					list3.Add(pointAtTime);
				}
			}
		}
		list3.Add(list2[list2.Count - 1]);
		return list3;
	}

	public static float SignedAngleBetween(Vector3 a, Vector3 b)
	{
		Vector3 up = Vector3.up;
		float num = Vector3.Angle(a, b);
		float num2 = Mathf.Sign(Vector3.Dot(up, Vector3.Cross(a, b)));
		float num3 = num * num2;
		if (!(num3 <= 0f))
		{
			return num3;
		}
		return 360f + num3;
	}

	public void GoToTarget(List<Vector2Int> path)
	{
		if (path == null)
		{
			_path = new List<Vector3>();
			return;
		}
		_path = SmoothTurnPath(path);
		_pathIndex = 0;
		_tempTime = 0f;
		m_LastPos = _path[0];
		m_NextPos = _path[1];
		itemBase.transform.LookAt(m_NextPos);
		curState = MoveStates.Moving;
	}

	public Vector3 GetPathLastPoint()
	{
		return m_LastPos;
	}

	public Vector3 GetPathNextPoint()
	{
		return m_NextPos;
	}

	public void GoBack(List<Vector2Int> path)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		for (int num = path.Count - 1; num >= 0; num--)
		{
			list.Add(path[num]);
		}
		_path = SmoothTurnPath(list);
		_pathIndex = 0;
		_tempTime = 0f;
		m_LastPos = _path[0];
		m_NextPos = _path[1];
		itemBase.transform.LookAt(m_NextPos);
		itemBase.PlayAnim(Moving);
		curState = MoveStates.Moving;
	}

	private bool IsApproachTarget()
	{
		if (!itemBase.IsPathFinish() && _pathIndex == _path.Count - 1)
		{
			return Vector3.Distance(itemBase.transform.position, m_NextPos) < itemBase.ApproachTargetOffset;
		}
		return false;
	}

	public MoveUpdateResult UpdateMove(float deltaTime)
	{
		if (_pathIndex >= _path.Count)
		{
			return MoveUpdateResult.FINISH_PATH;
		}
		if (pause)
		{
			return MoveUpdateResult.WAITING;
		}
		if (!IsApproachTarget())
		{
			float num = deltaTime * itemBase.Speed / (m_NextPos - m_LastPos).magnitude;
			_tempTime += num;
			Vector3 position = itemBase.transform.position;
			new Vector3(position.x, position.y, position.z);
			position = Vector3.Lerp(m_LastPos, m_NextPos, _tempTime);
			itemBase.transform.position = position;
			if (_tempTime >= 1f)
			{
				_tempTime = 0f;
				if (++_pathIndex >= _path.Count)
				{
					return MoveUpdateResult.FINISH_PATH;
				}
				m_LastPos = m_NextPos;
				m_NextPos = _path[_pathIndex];
				itemBase.transform.LookAt(m_NextPos);
			}
			return MoveUpdateResult.MOVING;
		}
		_tempTime = 0f;
		return MoveUpdateResult.FINISH_PATH;
	}

	public void PauseMove()
	{
		pause = true;
	}

	public void ResumeMove()
	{
		pause = false;
	}
}
