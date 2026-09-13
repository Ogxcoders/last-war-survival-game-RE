using System.Collections.Generic;
using UnityEngine;

public class AutoDoMovePos : MonoBehaviour
{
	public class PosParam
	{
		public Vector3 pos;

		public PositionType positionType;
	}

	public enum PositionType
	{
		World = 1,
		Screen
	}

	public enum PlayAnimName
	{
		Down = 1,
		Up
	}

	public static float MoveSpeed = 500f;

	public static float DownTime = 0.8f;

	public static float UpTime = 0.75f;

	private List<PosParam> _posList;

	private AutoDoMovePosMachine _machine;

	private void Awake()
	{
		_posList = new List<PosParam>();
	}

	public void Init(string posLists)
	{
		_posList.Clear();
		if (!string.IsNullOrEmpty(posLists))
		{
			string[] array = posLists.Split(new char[1] { '|' });
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[1] { ';' });
				if (array2.Length > 3)
				{
					PosParam posParam = new PosParam();
					posParam.pos = new Vector3(array2[0].ToFloat(), array2[1].ToFloat(), array2[2].ToFloat());
					posParam.positionType = (PositionType)array2[3].ToInt();
					_posList.Add(posParam);
				}
			}
		}
		_machine = new AutoDoMovePosMachine(this);
	}

	private void Update()
	{
		_machine?.OnUpdate(Time.deltaTime);
	}

	public float GetDownTime()
	{
		return DownTime;
	}

	public float GetUpTime()
	{
		return UpTime;
	}

	public float GetMoveSpeed()
	{
		return MoveSpeed;
	}

	public void ChangeStartPos()
	{
		base.transform.position = GetScreenPos(0);
	}

	public void ChangeEndPos()
	{
		base.transform.position = GetScreenPos(_posList.Count - 1);
	}

	public int GetMovePosListCount()
	{
		return _posList.Count;
	}

	public Vector3 GetScreenPos(int index)
	{
		if (index >= 0 && index < _posList.Count)
		{
			if (_posList[index].positionType != PositionType.World)
			{
				return _posList[index].pos;
			}
			return SceneManager.World.WorldToScreenPoint(_posList[index].pos);
		}
		return Vector3.zero;
	}
}
