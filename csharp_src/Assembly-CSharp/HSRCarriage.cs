using GameFramework;
using UnityEngine;

public class HSRCarriage
{
	private Transform parent;

	private InstanceRequest requestInst;

	private Transform transform;

	private HSRMarch hsrMarch;

	private CarriagePos carriagePos;

	private HSRDirection direction = HSRDirection.None;

	private Vector3 position;

	private long timeOffset;

	private string[] PrefabPath = new string[8] { "Assets/Main/SeasonRes/S5/Prefabs/World/HSR_Locomotive_East.prefab", "Assets/Main/SeasonRes/S5/Prefabs/World/HSR_Locomotive_West.prefab", "Assets/Main/SeasonRes/S5/Prefabs/World/HSR_Locomotive_South.prefab", "Assets/Main/SeasonRes/S5/Prefabs/World/HSR_Locomotive_North.prefab", "Assets/Main/SeasonRes/S5/Prefabs/World/HSR_Carriage_East.prefab", "Assets/Main/SeasonRes/S5/Prefabs/World/HSR_Carriage_West.prefab", "Assets/Main/SeasonRes/S5/Prefabs/World/HSR_Carriage_South.prefab", "Assets/Main/SeasonRes/S5/Prefabs/World/HSR_Carriage_North.prefab" };

	public HSRCarriage(HSRMarch hsrMarch, int index, Transform parent, float speed)
	{
		this.hsrMarch = hsrMarch;
		this.parent = parent;
		if (index == 0)
		{
			carriagePos = CarriagePos.Locomotive;
			timeOffset = 0L;
		}
		else
		{
			carriagePos = CarriagePos.Coach;
			timeOffset = (long)((3.6 + (double)(index - 1) * 2.5) * 1000.0 / (double)speed);
		}
		Init();
	}

	public void Refresh()
	{
	}

	public void Destroy()
	{
		if (requestInst != null)
		{
			requestInst.Destroy();
			requestInst = null;
		}
		parent = null;
		hsrMarch = null;
	}

	private void Init()
	{
	}

	public void OnUpdate(long now)
	{
		now -= timeOffset;
		hsrMarch.GetPosition(now, out var vector, out var hSRDirection, out var _);
		SetDirection(hSRDirection);
		SetPosition(vector);
	}

	private void SetDirection(HSRDirection dir)
	{
		if (dir != direction)
		{
			direction = dir;
			Instantiate();
		}
	}

	private void SetPosition(Vector3 pos)
	{
		position = pos;
		if (transform != null)
		{
			transform.position = pos;
		}
	}

	private string GetPrefabPath()
	{
		int num = (int)((int)carriagePos * 4 + direction);
		if (num < 0 || num >= PrefabPath.Length)
		{
			Log.Error("HSRCarriage GetPrefabPath 数组越界:{0},{1}", direction, carriagePos);
			return PrefabPath[0];
		}
		return PrefabPath[num];
	}

	private void Instantiate()
	{
		if (requestInst != null)
		{
			requestInst.Destroy();
		}
		transform = null;
		requestInst = GameEntry.Resource.InstantiateAsync(GetPrefabPath());
		if (requestInst == null)
		{
			return;
		}
		requestInst.completed += delegate
		{
			GameObject gameObject = requestInst.gameObject;
			if (!(gameObject == null))
			{
				transform = gameObject.transform;
				transform.SetParent(parent);
				transform.position = position;
				transform.localScale = Vector3.one;
				transform.rotation = Quaternion.identity;
			}
		};
	}
}
